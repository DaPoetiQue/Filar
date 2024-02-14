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

        [SerializeField]
        private string profileURL = "User Profile";


        [Space(5)]
        public AppData.StorageDirectoryData profileStorageData = new AppData.StorageDirectoryData();

        [Space(5)]
        public DependencyStatus authDependencyStatus = DependencyStatus.UnavailableOther;

        public bool AuthenticationInitialized { get; private set; }

        #region Firebase

        FirebaseAuth authentication;
        public bool TermsAndConditionsAccepted { get; private set; }

        [SerializeField]
        private AppData.SignInState signInState = AppData.SignInState.None;

        #endregion

        #endregion

        #region Main

        protected override void Init()
        {

        }

        public void UpdateUserProfile(AppData.Profile referenceProfile, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(referenceProfile, "Reference Profile", "Update Profile Failed - Reference Profile Parameter Value Is Null - invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Services Manager Instance", "App Services Manager Instance Is Not Yet Initialized."));

                if(callbackResults.Success())
                {
                    var appServicesManagerInstance = AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Services Manager Instance").GetData();

                    appServicesManagerInstance.GetAppInfo(getAppInfoCallbackResults =>
                    {
                        callbackResults.SetResult(getAppInfoCallbackResults);

                        if(callbackResults.Success())
                        {
                            var appInfo = getAppInfoCallbackResults.GetData();

                            appInfo.SetProfile(referenceProfile, referenceProfileCallbackResults => 
                            {
                                callbackResults.SetResult(referenceProfileCallbackResults);

                                if(callbackResults.Success())
                                {
                                    appServicesManagerInstance.SyncAppInfo(appInfo, appInfoCallbackResults => 
                                    {
                                        callbackResults.SetResult(appInfoCallbackResults);
                                    });
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            });
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }   
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void GetUserProfile(Action<AppData.CallbackData<AppData.Profile>> callback)
        {
            AppData.CallbackData<AppData.Profile> callbackResults = new AppData.CallbackData<AppData.Profile>(AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Services Manager Instance", "App Services Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appServicesManagerInstance = AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Services Manager Instance").GetData();

                appServicesManagerInstance.GetAppInfo(getAppInfoCallbackResults => 
                {
                    callbackResults.SetResult(getAppInfoCallbackResults);
                
                    if(callbackResults.Success())
                    {
                        callbackResults.SetResult(getAppInfoCallbackResults.GetData().GetProfile());

                        if (callbackResults.Success())
                            callbackResults.data = getAppInfoCallbackResults.GetData().GetProfile().GetData();
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback.Invoke(callbackResults);
        }

        private void SetSignInState(AppData.SignInState signInState) => this.signInState = signInState;

        public AppData.CallbackData<AppData.SignInState> GetSignInState()
        {
            var callbackResults = new AppData.CallbackData<AppData.SignInState>();

            if(signInState != AppData.SignInState.None)
            {
                callbackResults.result = $"Get Sign In State Success - Sign In State Is Set To : {signInState}";
                callbackResults.data = signInState;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Get Sign In State Failed - Sign In State Is Set To Default : {signInState}";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            return callbackResults;
        }

        public AppData.Callback OnCheckProfileSignIn(bool isProfileButton = false)
        {
            var callbackResults = new AppData.Callback(GetSignInState());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                    if (callbackResults.Success())
                    {
                        var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                        if (GetSignInState().GetData() != AppData.SignInState.SignedIn)
                        {
                            if (isProfileButton)
                            {
                                #region Sign In

                                #endregion
                            }
                            else
                            {
                                #region Feature Blocker

                                var featureBlockerWidgetConfig = new AppData.SceneConfigDataPacket();

                                featureBlockerWidgetConfig.SetReferencedWidgetType(AppData.WidgetType.FeatureBlockerPopUpWidget);
                                featureBlockerWidgetConfig.blurScreen = true;

                                screen.ShowWidget(featureBlockerWidgetConfig);

                                #endregion
                            }

                            callbackResults.result = $"On Check Profile Sign In Unsuccessful - Profile Is Not Signed In.";
                            callbackResults.resultCode = AppData.Helpers.WarningCode;
                        }
                        else
                            callbackResults.result = $"Profile Is Signed In.";
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
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

                var serverInitializationCallbackTask = await databaseManager.OnServerAppInfoDatabaseInitializationCompleted();

                callbackResults.SetResult(serverInitializationCallbackTask);

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Service Manager Instance", "App Service Manager Instance Is Not initialized Yet."));

                    if (callbackResults.Success())
                    {
                        var appServiceManagerInstance = AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Service Manager Instance").GetData();

                        string deviceID = AppData.Helpers.GetDeviceInfo().deviceID;

                        var appInfoTaskResults = await databaseManager.GetAppInfoAsync(deviceID);

                        await Task.Delay(2000);

                        callbackResults.SetResult(appInfoTaskResults);

                        if (callbackResults.Success())
                        {
                            appServiceManagerInstance.SyncAppInfo(appInfoTaskResults.GetData(), syncCallbackResults =>
                            {
                                callbackResults.SetResult(syncCallbackResults);
                            });

                            await Task.Delay(1000);
                        }
                        else
                        {
                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Services Manager Instance", "App Services Manager Instance Is not Yet Initialized."));

                            if (callbackResults.Success())
                            {
                                var appServicesManagerInstance = AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Services Manager Instance").GetData();
                                var newProfile = CreateProfile(deviceID).GetData();
                                var newAppInfo = appServicesManagerInstance.CreateAppDeviceInfo(newProfile).GetData();
                                var registerDeviceInfoCallbackResultsTask = await databaseManager.RegisterDeficeAppInfoAsync(profileURL, newAppInfo);

                                callbackResults.SetResult(registerDeviceInfoCallbackResultsTask);

                                if (callbackResults.Success())
                                {
                                    appServiceManagerInstance.SyncAppInfo(newAppInfo, syncCallbackResults =>
                                    {
                                        callbackResults.SetResult(syncCallbackResults);
                                    });

                                    await Task.Delay(1000);

                                    callbackResults.result = "App Signed In Successfully.";
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }

            return callbackResults;
        }

        private AppData.CallbackData<AppData.Profile> CreateProfile(string deviceID)
        {
            var callbackResults = new AppData.CallbackData<AppData.Profile>();

            if (!string.IsNullOrEmpty(deviceID))
            {
                var profile = new AppData.Profile(AppData.ProfileStatus.Anonymous);

                callbackResults.result = $"Create Profile Success - A Profile Has Been Successfully Created Using Device ID : {deviceID}";
                callbackResults.data = profile;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Create Profile Failed - Device ID Is Missing - Invalid Operation.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
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
                                SetSignInState(AppData.SignInState.GuestSignIn);

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

        public async Task<AppData.CallbackData<AuthError>> SignInAsync(AppData.Profile profile)
        {
            var callbackResults = new AppData.CallbackData<AuthError>(AppData.Helpers.GetAppComponentValid(profile, "Profile", "SignInAsync In Async Failed - Profile Parameter Value Is Missing / Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(profile.Initialized());

                if (callbackResults.Success())
                {
                    AuthError authError = AuthError.None;

                    try
                    {
                        if (authDependencyStatus == DependencyStatus.Available)
                        {
                            await authentication.SignInWithEmailAndPasswordAsync(profile.GetUserEmail().GetData(), profile.GetUserPassword().GetData()).ContinueWith(signedInTaskCompletion =>
                            {
                                if (signedInTaskCompletion.Exception == null)
                                {
                                    if (signedInTaskCompletion.Result.User.IsValid())
                                    {
                                        callbackResults.result = $"User : {authentication.CurrentUser.DisplayName} Has Signed In Successfully.";
                                        callbackResults.data = authError;
                                    }
                                    else

                                    {
                                        callbackResults.result = $"User : {profile.GetUserEmail()} Is Not Valid";
                                        callbackResults.data = authError;
                                        callbackResults.resultCode = AppData.Helpers.WarningCode;
                                    }
                                }
                                else
                                {
                                    FirebaseException fbException = signedInTaskCompletion.Exception.GetBaseException() as FirebaseException;
                                    authError = (AuthError)fbException.ErrorCode;

                                    callbackResults.result = fbException.Message;
                                    callbackResults.data = authError;
                                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                }

                                return authError;
                            });
                        }

                        return callbackResults;
                    }
                    catch (Exception exception)
                    {
                        FirebaseException fbException = exception.GetBaseException() as FirebaseException;
                        var errorCode = (AuthError)fbException.ErrorCode;

                        callbackResults.result = fbException.Message;
                        callbackResults.data = errorCode;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;

                        return callbackResults;
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
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