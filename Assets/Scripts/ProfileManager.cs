using System;
<<<<<<< HEAD
using Firebase;
using Firebase.Auth;
=======
//using Firebase;
//using Firebase.Auth;
>>>>>>> 80bd9825b60a76f615d009cae012bcec7f5eb7c4
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ProfileManager : AppMonoBaseClass
    {
        #region Static

        private static ProfileManager _instance;

        public static ProfileManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<ProfileManager>();

                return _instance;
            }
        }

        #endregion

        #region Components

        [Space(5)]
        public AppData.StorageDirectoryData profileStorageData = new AppData.StorageDirectoryData();

<<<<<<< HEAD
        [Space(5)]
        public DependencyStatus authDependencyStatus = DependencyStatus.UnavailableOther;
=======
        //[Space(5)]
        //public DependencyStatus authDependencyStatus = DependencyStatus.UnavailableOther;
>>>>>>> 80bd9825b60a76f615d009cae012bcec7f5eb7c4

        public bool SignedIn { get; private set; }

        #region Firebase

<<<<<<< HEAD
        FirebaseAuth authentication;
=======
        //FirebaseAuth authentication;
>>>>>>> 80bd9825b60a76f615d009cae012bcec7f5eb7c4

        public bool TermsAndConditionsAccepted { get; private set; }

        #endregion

        #endregion

        #region Unity Callbacks

        void Awake() => Setup();

        #endregion

        #region Main

        void Setup()
        {
            //Initialize Authentication
<<<<<<< HEAD
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                authDependencyStatus = task.Result;

                if (task.Exception == null)
                {
                    if (authDependencyStatus == DependencyStatus.Available)
                        authentication = FirebaseAuth.DefaultInstance;
                    else
                        LogInfo($"Authentication Dependency Is Not Available - Dependency Status : {authDependencyStatus}", this);
                }
                else
                    throw task.Exception;
            });
=======
            //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
            //{
            //    authDependencyStatus = task.Result;

            //    if (task.Exception == null)
            //    {
            //        if (authDependencyStatus == DependencyStatus.Available)
            //            authentication = FirebaseAuth.DefaultInstance;
            //        else
            //            LogInfo($"Authentication Dependency Is Not Available - Dependency Status : {authDependencyStatus}", this);
            //    }
            //    else
            //        throw task.Exception;
            //});
>>>>>>> 80bd9825b60a76f615d009cae012bcec7f5eb7c4
        }

        void CreateProfile(AppData.Profile profile, Action<AppData.CallbackData<AppData.Profile>> callback = null)
        {
            AppData.CallbackData<AppData.Profile> callbackResults = new AppData.CallbackData<AppData.Profile>();

            AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name, sceneAssetsManagerCallbackResults =>
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

            AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name, sceneAssetsManagerCallbackResults =>
            {

                callbackResults.SetResults(sceneAssetsManagerCallbackResults);

                if(callbackResults.Success())
                {

                }

            }, "Scene Assets Manager Instance In Not Yet Initialized.");

            callback.Invoke(callbackResults);
        }

<<<<<<< HEAD
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
=======
        //public async Task<AuthError> SignInAsync(AppData.Profile profile)
        //{
        //    AuthError authError = AuthError.None;

        //    try
        //    {
        //        if (authDependencyStatus == DependencyStatus.Available)
        //        {
        //            await authentication.SignInWithEmailAndPasswordAsync(profile.GetUserEmail(), profile.GetUserPassword()).ContinueWith(signedInTaskCompletion =>
        //            {
        //                if (signedInTaskCompletion.Exception == null)
        //                {
        //                    if (signedInTaskCompletion.Result.User.IsValid())
        //                    {
        //                        LogSuccess($"User : {authentication.CurrentUser.DisplayName} Has Signed In Successfully.", this);
        //                    }
        //                    else
        //                        LogWarning($"User : {profile.GetUserEmail()} Is Not Valid", this);
        //                }
        //                else
        //                {
        //                    FirebaseException fbException = signedInTaskCompletion.Exception.GetBaseException() as FirebaseException;
        //                    authError = (AuthError)fbException.ErrorCode;
        //                }

        //                return authError;
        //            });
        //        }

        //        return authError;
        //    }
        //    catch (Exception exception)
        //    {
        //        FirebaseException fbException = exception.GetBaseException() as FirebaseException;
        //        var errorCode = (AuthError)fbException.ErrorCode;

        //        return errorCode;
        //    }
        //}

        //public async Task<AuthError> SignUpAsync(string userEmail, string userPassWord)
        //{
        //    AuthError authError = AuthError.None;

        //    try
        //    {
        //        if(authDependencyStatus == DependencyStatus.Available)
        //        {
        //            await authentication.CreateUserWithEmailAndPasswordAsync(userEmail, userPassWord).ContinueWith(async signedUpTaskCompletion => 
        //            {
        //                if (signedUpTaskCompletion.Exception == null)
        //                {
        //                    if (signedUpTaskCompletion.Result.User.IsValid())
        //                    {
        //                        LogSuccess($" <+++++++++++++++++++++> User : {userEmail} Has Been Created Successfully. Choose Varification Method.", this);
        //                    }
        //                    else
        //                        LogWarning($"User : {userEmail} Is Not Valid", this);
        //                }
        //                else
        //                {
        //                    FirebaseException fbException = signedUpTaskCompletion.Exception.GetBaseException() as FirebaseException;
        //                    authError = (AuthError)fbException.ErrorCode;
        //                }

        //                return authError;
        //            });
        //        }

        //        return authError;
        //    }
        //    catch(Exception exception)
        //    {
        //        FirebaseException fbException = exception.GetBaseException() as FirebaseException;
        //        var errorCode = (AuthError)fbException.ErrorCode;

        //        return errorCode;
        //    }
        //}

        //public void SignOut()
        //{
        //    if (authDependencyStatus == DependencyStatus.Available)
        //    {
        //        LogInfo($"User : {authentication.CurrentUser.DisplayName} Has Signed Out.");
        //        authentication.SignOut();
        //    }
        //}
>>>>>>> 80bd9825b60a76f615d009cae012bcec7f5eb7c4

        public void AcceptTermsAndConditions() => TermsAndConditionsAccepted = true;

        #endregion
    }
}