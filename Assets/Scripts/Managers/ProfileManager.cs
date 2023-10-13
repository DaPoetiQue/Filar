using System;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ProfileManager : AppData.SingletonBaseComponent<ProfileManager>
    {
        #region Components

        [Space(5)]
        public AppData.StorageDirectoryData profileStorageData = new AppData.StorageDirectoryData();

        [Space(5)]
        public DependencyStatus authDependencyStatus = DependencyStatus.UnavailableOther;

        public bool SignedIn { get; private set; }
        public bool AuthenticationInitialized { get; private set; }

        #region Firebase

        FirebaseAuth authentication;
        public bool TermsAndConditionsAccepted { get; private set; }

        #endregion

        #endregion

        #region Main

        void CreateProfile(AppData.Profile profile, Action<AppData.CallbackData<AppData.Profile>> callback = null)
        {
            AppData.CallbackData<AppData.Profile> callbackResults = new AppData.CallbackData<AppData.Profile>();

            AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, sceneAssetsManagerCallbackResults =>
            {
                callbackResults.result = sceneAssetsManagerCallbackResults.result;
                callbackResults.resultCode = sceneAssetsManagerCallbackResults.resultCode;

                if (callbackResults.Success())
                {

                }

            }, "Scene Assets Manager Instance In Not Yet Initialized.");

            callback?.Invoke(callbackResults);
        }

        public void GetUserProfile(Action<AppData.CallbackData<AppData.Profile>> callback)
        {
            AppData.CallbackData<AppData.Profile> callbackResults = new AppData.CallbackData<AppData.Profile>();

            AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, sceneAssetsManagerCallbackResults =>
            {

                callbackResults.SetResults(sceneAssetsManagerCallbackResults);

                if(callbackResults.Success())
                {

                }

            }, "Scene Assets Manager Instance In Not Yet Initialized.");

            callback.Invoke(callbackResults);
        }

        public async Task<AppData.Callback> SynchronizingProfile()
        {
            AppData.Callback callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager Is Not Yet Initialized."));

            if(callbackResults.Success())
            {
                var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                {
                    authDependencyStatus = task.Result;

                    if (task.Exception == null)
                    {
                        if (authDependencyStatus == DependencyStatus.Available)
                        {
                            authentication = FirebaseAuth.DefaultInstance;
                            authentication.StateChanged += AuthenticationStateChangeEvent;
                        }
                        else
                            LogInfo($"Authentication Dependency Is Not Available - Dependency Status : {authDependencyStatus}", this);
                    }
                    else
                        throw task.Exception;
                });

                string deviceID = AppData.Helpers.GetDeviceInfo().deviceID;

                var appInfoTaskResults = await databaseManager.GetAppInfoAsync(deviceID);

                await Task.Delay(2000);

                if (callbackResults.Success())
                {
                    AppData.Helpers.GetAppComponentValid(AppManager.Instance, AppManager.Instance.name, async appManagerCallbackResults =>
                    {
                        if (appManagerCallbackResults.Success())
                        {
                            appManagerCallbackResults.data.SyncAppInfo(appInfoTaskResults.data);
                        }
                        else
                            Log(appManagerCallbackResults.GetResultCode, appManagerCallbackResults.GetResult, this);

                        await Task.Delay(1000);

                    }, "App Manager instane Is Not Yet Initialized.");
                }
            }

            return callbackResults;
        }

        private void AuthenticationStateChangeEvent(object sender, EventArgs state)
        {
            if(!AuthenticationInitialized)
                AuthenticationInitialized = true;
            else
                LogInfo(" >>>>>>>>>>>>>>> Sign In State Changed.", this);
        }

        public async Task<AppData.CallbackData<AuthError>> AppSignInAsync()
        {
            AppData.CallbackData<AuthError> callbackResults = new AppData.CallbackData<AuthError>();

            try
            {
                if (authDependencyStatus == DependencyStatus.Available)
                {
                    var anonymousSignInTadk = await authentication.SignInAnonymouslyAsync().ContinueWith(async signedInTaskCompletion =>
                    {
                        if (signedInTaskCompletion.Exception == null)
                        {
                            if (signedInTaskCompletion.Result.User.IsValid())
                            {
                                callbackResults.result = "App Signed In Successgully.";
                                callbackResults.data = default;
                                callbackResults.resultCode = AppData.Helpers.SuccessCode;

                                await Task.Delay(1000);
                            }
                            else
                            {
                                callbackResults.result = "App Sign In Failed - Please Check Here.";
                                callbackResults.data = AuthError.Failure;
                                callbackResults.resultCode = AppData.Helpers.WarningCode;
                            }
                        }
                        else
                        {
                            FirebaseException fbException = signedInTaskCompletion.Exception.GetBaseException() as FirebaseException;

                            callbackResults.result = $"Sign In Failed With Exception : {fbException.Message}";
                            callbackResults.data = (AuthError)fbException.ErrorCode;
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }

                        return callbackResults;
                    });
                }

                return callbackResults;
            }
            catch (Exception exception)
            {
                FirebaseException fbException = exception.GetBaseException() as FirebaseException;
                var errorCode = (AuthError)fbException.ErrorCode;

                return callbackResults;
            }
        }

        public async Task<AuthError> SignInAsync(AppData.Profile profile)
        {
            AuthError authError = AuthError.None;

            try
            {
                if (authDependencyStatus == DependencyStatus.Available)
                {
                    await authentication.SignInWithEmailAndPasswordAsync(profile.GetUserEmail(), profile.GetUserPassword()).ContinueWith(signedInTaskCompletion =>
                    {
                        if (signedInTaskCompletion.Exception == null)
                        {
                            if (signedInTaskCompletion.Result.User.IsValid())
                            {
                                LogSuccess($"User : {authentication.CurrentUser.DisplayName} Has Signed In Successfully.", this);
                            }
                            else
                                LogWarning($"User : {profile.GetUserEmail()} Is Not Valid", this);
                        }
                        else
                        {
                            FirebaseException fbException = signedInTaskCompletion.Exception.GetBaseException() as FirebaseException;
                            authError = (AuthError)fbException.ErrorCode;
                        }

                        return authError;
                    });
                }

                return authError;
            }
            catch (Exception exception)
            {
                FirebaseException fbException = exception.GetBaseException() as FirebaseException;
                var errorCode = (AuthError)fbException.ErrorCode;

                return errorCode;
            }
        }

        public async Task<AuthError> SignUpAsync(string userEmail, string userPassWord)
        {
            AuthError authError = AuthError.None;

            try
            {
                if (authDependencyStatus == DependencyStatus.Available)
                {
                    await authentication.CreateUserWithEmailAndPasswordAsync(userEmail, userPassWord).ContinueWith(async signedUpTaskCompletion =>
                    {
                        if (signedUpTaskCompletion.Exception == null)
                        {
                            if (signedUpTaskCompletion.Result.User.IsValid())
                            {
                                LogSuccess($" <+++++++++++++++++++++> User : {userEmail} Has Been Created Successfully. Choose Varification Method.", this);
                            }
                            else
                                LogWarning($"User : {userEmail} Is Not Valid", this);
                        }
                        else
                        {
                            FirebaseException fbException = signedUpTaskCompletion.Exception.GetBaseException() as FirebaseException;
                            authError = (AuthError)fbException.ErrorCode;
                        }

                        return authError;
                    });
                }

                return authError;
            }
            catch (Exception exception)
            {
                FirebaseException fbException = exception.GetBaseException() as FirebaseException;
                var errorCode = (AuthError)fbException.ErrorCode;

                return errorCode;
            }
        }

        public void SignOut()
        {
            if (authDependencyStatus == DependencyStatus.Available)
            {
                LogInfo($"User : {authentication.CurrentUser.DisplayName} Has Signed Out.");
                authentication.SignOut();
            }
        }

        public void AcceptTermsAndConditions() => TermsAndConditionsAccepted = true;

        #endregion
    }
}