using System;
using System.Linq;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

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

        public async Task<AppData.CallbackData<(List<AppData.Profile> profiles, AppData.CredentialStatusInfo statusInfo)>> GetAppProfiles()
        {
            var callbackResults = new AppData.CallbackData<(List<AppData.Profile> profiles, AppData.CredentialStatusInfo statusInfo)>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "Get App Profiles Failed - App Database Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, "Network Manager Instance", "Get App Profiles Failed - Network Manager Instance Is Not Initialized Yet - Invalid Operation."));

                if (callbackResults.Success())
                {
                    var networkManagerInstance = AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, "Network Manager Instance").GetData();

                    var networkStatusCallbackResultsTask = await networkManagerInstance.CheckConnectionStatus();

                    callbackResults.SetResult(networkStatusCallbackResultsTask);

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(appDatabaseManagerInstance.GetDatabaseReference());

                        if (callbackResults.Success())
                        {
                            var profileDatabase = await appDatabaseManagerInstance.GetDatabaseReference().GetData().Child("Filar Authentications").Child("User Profiles").GetValueAsync();

                            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(profileDatabase.Children.ToList(), "Profile Database", "Get App Profiles Failed - Profile Database Is Not Found - Please Check Profile Manager's Get App Profiles Method - Invalid Operation."));

                            if (callbackResults.Success())
                            {
                                var appInfoSnapshots = profileDatabase.Children.ToList();

                                var registeredProfiles = new List<AppData.Profile>();

                                foreach (var appInfoSnapshot in appInfoSnapshots)
                                {
                                    var resultsJson = (string)appInfoSnapshot.GetValue(true);
                                    var appInfo = JsonUtility.FromJson<AppData.AppInfo>(resultsJson);

                                    callbackResults.SetResult(appInfo.GetProfile());

                                    if (callbackResults.Success())
                                    {
                                        if(appInfo.GetProfile().GetData().GetStatus().GetData() == AppData.ProfileStatus.Registered)
                                        {
                                            if (!registeredProfiles.Contains(appInfo.GetProfile().GetData()))
                                            {
                                                registeredProfiles.Add(appInfo.GetProfile().GetData());

                                                callbackResults.result = $"Profile For : {appInfo.GetProfile().GetData().GetUserName().GetData()} Has Been successfully Loaded.";
                                            }
                                            else
                                            {
                                                callbackResults.result = $"Profile For : {appInfo.GetProfile().GetData().GetUserName().GetData()} Already Loaded - Invalid Operation.";
                                                callbackResults.resultCode = AppData.Helpers.WarningCode;

                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        break;
                                    }
                                }

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(registeredProfiles, "Registered Profiles", "Get App Profiles Failed - Couldn't Find Registered Profiles From Database - Invalid Operation."));

                                if (callbackResults.Success())
                                {
                                    callbackResults.result = $"Get App Profiles Success - {registeredProfiles.Count} Registered Profile(s) Have Been Found.";
                                    callbackResults.data = callbackResults.data = (registeredProfiles, AppData.CredentialStatusInfo.Success);
                                }
                                else
                                {
                                    callbackResults.result = "There Are No Registered User Profiles Found - Continue Execution.";
                                    callbackResults.data = (null, AppData.CredentialStatusInfo.NoRegisteredProfilesFound);
                                }
                            }
                            else
                            {
                                callbackResults.result = "Couldn't Find App Info Data From Database - Invalid Operation.";
                                callbackResults.data = (null, AppData.CredentialStatusInfo.DatabaseError);
                            }
                        }
                        else
                        {
                            callbackResults.result = "Database Couldn't Be Found.";
                            callbackResults.data = (null, AppData.CredentialStatusInfo.DatabaseError);
                        }
                    }
                    else
                    {
                        callbackResults.result = "Network Connection Failed.";
                        callbackResults.data = (null, AppData.CredentialStatusInfo.DeviceNetworkError);
                    }
                }
                else
                    callbackResults.data = (null, AppData.CredentialStatusInfo.CompilerError);
            }
            else
                callbackResults.data = (null, AppData.CredentialStatusInfo.CompilerError);

            return callbackResults;
        }

        private async Task<AppData.CallbackData<(List<string> userNames, List<string> userEmails, AppData.CredentialStatusInfo statusInfo)>> GetAppUsers()
        {
            var callbackResults = new AppData.CallbackData<(List<string> userNames, List<string> userEmails, AppData.CredentialStatusInfo statusInfo)>();

            var profilesCallbackResultsTask = await GetAppProfiles();

            callbackResults.SetResult(profilesCallbackResultsTask);

            if (callbackResults.Success())
            {
                var userNames = profilesCallbackResultsTask.GetData().profiles.FindAll(profile => profile.Initialized().Success()).Select(profile => profile.GetUserName().GetData()).ToList();

                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(userNames, "User Names", "Get App User Names Failed - Couldn't Find Any User Names - Invalid Operation."));

                if (callbackResults.Success())
                {
                    var userEmails = profilesCallbackResultsTask.GetData().profiles.FindAll(profile => profile.Initialized().Success()).Select(profile => profile.GetUserEmail().GetData()).ToList();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(userEmails, "User Emails", "Get App User Emails Failed - Couldn't Find Any User Emails - Invalid Operation."));

                    if (callbackResults.Success())
                    {
                        callbackResults.result = $"Get App Users Success - {userNames.Count} User(s) Have Been Found.";
                        callbackResults.data = (userNames, userEmails, AppData.CredentialStatusInfo.Success);
                    }
                    else
                    {
                        callbackResults.result = "There Are No Users Found - User Names Could'nt Be Found.";
                        callbackResults.data = (null, null, AppData.CredentialStatusInfo.NullReferenceError);
                    }
                }
                else
                {
                    callbackResults.result = "There Are No Users Found - User Names Could'nt Be Found.";
                    callbackResults.data = (null, null, AppData.CredentialStatusInfo.NullReferenceError);
                }
            }
            else
            {
                if (profilesCallbackResultsTask.GetData().statusInfo == AppData.CredentialStatusInfo.NoRegisteredProfilesFound)
                    callbackResults.data = (null, null, AppData.CredentialStatusInfo.NoRegisteredProfilesFound);
                else
                    callbackResults.data = (null, null, profilesCallbackResultsTask.GetData().statusInfo);
            }

            return callbackResults;
        }

        public async Task<AppData.CallbackData<AppData.CredentialStatusInfo>> CredentialsAvailable(string userName, string userEmail)
        {
            var callbackResults = new AppData.CallbackData<AppData.CredentialStatusInfo>();

            var getUsersCallbackResultsTask = await GetAppUsers();

            callbackResults.SetResult(getUsersCallbackResultsTask);

            if (callbackResults.Success())
            {
                var userNames = getUsersCallbackResultsTask.GetData().userNames;
                var userEmails = getUsersCallbackResultsTask.GetData().userEmails;

                #region User Name

                if(!userNames.Contains(userName))
                {
                    callbackResults.result = $"User Name {userName} Is Available.";
                    callbackResults.data = AppData.CredentialStatusInfo.Success;
                }
                else
                {
                    callbackResults.result = $"User Name {userName} Is Already In use.";
                    callbackResults.data = AppData.CredentialStatusInfo.UserNameError;
                    callbackResults.resultCode = AppData.Helpers.WarningCode;

                    return callbackResults;
                }

                #endregion

                #region User Email

                if (!userEmails.Contains(userEmail))
                {
                    callbackResults.result = $"User Email {userEmail} Is Available.";
                    callbackResults.data = AppData.CredentialStatusInfo.Success;
                }
                else
                {
                    callbackResults.result = $"User Email {userEmail} Is Already In use.";
                    callbackResults.data = AppData.CredentialStatusInfo.UserNameError;
                    callbackResults.resultCode = AppData.Helpers.WarningCode;

                    return callbackResults;
                }

                #endregion
            }

            if(callbackResults.UnSuccessful())
            {
                if(getUsersCallbackResultsTask.GetData().statusInfo == AppData.CredentialStatusInfo.NoRegisteredProfilesFound)
                {
                    callbackResults.result = $"There Are No Registered Profiles Found In The Database - This Is The Initial Profile Registration - Proceeding To Register User : {userName} With Email ID : {userEmail}";
                    callbackResults.data = AppData.CredentialStatusInfo.NoRegisteredProfilesFound;
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                    callbackResults.data = getUsersCallbackResultsTask.GetData().statusInfo;
            }

            return callbackResults;
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
            var callbackResults = new AppData.CallbackData<AppData.Profile>(AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Services Manager Instance", "App Services Manager Instance Is Not Yet Initialized."));

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

        public AppData.CallbackData<AppData.Profile> GetUserProfile()
        {
            var callbackResults = new AppData.CallbackData<AppData.Profile>(AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Services Manager Instance", "App Services Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appServicesManagerInstance = AppData.Helpers.GetAppComponentValid(AppServicesManager.Instance, "App Services Manager Instance").GetData();

                appServicesManagerInstance.GetAppInfo(getAppInfoCallbackResults =>
                {
                    callbackResults.SetResult(getAppInfoCallbackResults);

                    if (callbackResults.Success())
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

            return callbackResults;
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
                            #region Auth

                            authentication = FirebaseAuth.DefaultInstance;
                            authentication.StateChanged += AuthenticationStateChangeEvent;

                            #endregion
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

        public async Task<AppData.CallbackData<AuthError>> SignUpAsync(AppData.Profile profile)
        {
            var callbackResults = new AppData.CallbackData<AuthError>(AppData.Helpers.GetAppComponentValid(profile, "Profile", "Sign Up async Failed - Profile Parameter Value Is Null - Invalid Operation."));

            try
            {
                if (authDependencyStatus == DependencyStatus.Available)
                {
                    await authentication.CreateUserWithEmailAndPasswordAsync(profile.GetUserEmail().GetData(), profile.GetUserPassword().GetData()).ContinueWith(async signedUpTaskCompletion =>
                    {
                        if (signedUpTaskCompletion.Exception == null)
                        {
                            if (signedUpTaskCompletion.Result.User.IsValid())
                            {
                                await signedUpTaskCompletion.Result.User.SendEmailVerificationAsync().ContinueWith(emailVerificationCallbackResults => 
                                {
                                    if(emailVerificationCallbackResults.IsCompletedSuccessfully)
                                    {
                                        callbackResults.result = $"User : {profile.GetUserEmail().GetData()} Has Been Created Successfully. A Verification Email Has Been sent.";
                                        callbackResults.data = default;
                                    }
                                    else
                                    {
                                        if (emailVerificationCallbackResults.IsCanceled)
                                        {
                                            callbackResults.result = $"User : {profile.GetUserEmail().GetData()} Sign Up Has Been Cancelled By User.";
                                            callbackResults.data = default;
                                            callbackResults.resultCode = AppData.Helpers.WarningCode;
                                        }
                                        else
                                        {
                                            callbackResults.result = $"User : {profile.GetUserEmail().GetData()} Is Not Valid";
                                            callbackResults.data = default;
                                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                        }
                                    }
                                });

                                return callbackResults;
                            }
                            else
                            {
                                callbackResults.result = $"User : {profile.GetUserEmail().GetData()} Is Not Valid";
                                callbackResults.data = default;
                                callbackResults.resultCode = AppData.Helpers.WarningCode;
                            }
                        }
                        else
                        {
                            FirebaseException fbException = signedUpTaskCompletion.Exception.GetBaseException() as FirebaseException;

                            callbackResults.result = $"Sign Up User Profile : {profile.GetUserEmail().GetData()} Failed With Exception : {fbException.Message}";
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

                callbackResults.result = $"Sign Up User Profile : {profile.GetUserEmail().GetData()} Failed With Exception : {fbException.Message}";
                callbackResults.data = errorCode;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;

                return callbackResults;
            }
        }

        public async Task<AppData.Callback> ResendUserEmailVerificationAsync()
        {
            var callbackResults = new AppData.Callback(GetCurrentUser());

            if (callbackResults.Success())
            {
                var checkIfUserIsVerifiedOrNotCallbackResultsTask = await UserEmailVerified();

                callbackResults.SetResult(checkIfUserIsVerifiedOrNotCallbackResultsTask);

                if(callbackResults.UnSuccessful())
                {
                    await GetCurrentUser().GetData().SendEmailVerificationAsync().ContinueWith(emailVerificationCallbackResults =>
                    {
                        if (emailVerificationCallbackResults.IsCompletedSuccessfully)
                        {
                            callbackResults.result = $"User : {GetCurrentUser().GetData().Email} Has Been Created Successfully. A Verification Email Has Been sent.";
                            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                        }
                        else
                        {
                            if (emailVerificationCallbackResults.IsCanceled)
                            {
                                callbackResults.result = $"User : {GetCurrentUser().GetData().Email} Sign Up Has Been Cancelled By User.";
                                callbackResults.resultCode = AppData.Helpers.WarningCode;
                            }
                            else
                            {
                                callbackResults.result = $"User : {GetCurrentUser().GetData().Email} Is Not Valid";
                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                            }
                        }
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public async Task<AppData.CallbackData<AppData.Profile>> RegisterUserProfileAsync()
        {
            var callbackResults = new AppData.CallbackData<AppData.Profile>(GetUserProfile());

            if (callbackResults.Success())
            {
                var userProfile = GetUserProfile().GetData();

                callbackResults.SetResult(userProfile.Initialized());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(userProfile.SetProfileStatus(AppData.ProfileStatus.Verified));

                    if (callbackResults.Success())
                    {
                        await Task.Delay(1000);

                        callbackResults.result = $"Register User Profile Async Success - {userProfile.GetUserName().GetData()}'s Profile Has Been Successfully Registered.";
                        callbackResults.data = userProfile;
                    }
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public async Task<AppData.Callback> OnAccountDeleteRequestAsync()
        {
            var callbackResults = new AppData.Callback(GetCurrentUser());

            if(callbackResults.Success())
            {
                await GetCurrentUser().GetData().DeleteAsync().ContinueWith(userDeletedCallbackResults => 
                {
                    if(userDeletedCallbackResults.IsCompletedSuccessfully)
                        callbackResults.result = "On Account Delete Request Async Success - User Account Has Been Deleted Successfully.";
                    else
                    {
                        callbackResults.result = $"On Account Delete Request Async Failed - User Account Couldn't Be Deleted With Error : {userDeletedCallbackResults.Exception.Message}.";
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public async Task<AppData.Callback> UserEmailVerified()
        {
            var callbackResults = new AppData.Callback(GetCurrentUser());

            if(callbackResults.Success())
            {
                await GetCurrentUser().GetData().ReloadAsync().ContinueWith(reloadedTaskResults => 
                {
                    if (reloadedTaskResults.IsCompletedSuccessfully)
                    {
                        if (GetCurrentUser().GetData().IsEmailVerified)
                        {
                            callbackResults.result = $"User Email : {GetCurrentUser().GetData().Email} Has Been Successuffly Verified.";
                            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.result = $"User Email : {GetCurrentUser().GetData().Email} Has Not Been Verified Yet.";
                            callbackResults.resultCode = AppData.Helpers.WarningCode;
                        }
                    }
                    else
                    {
                        callbackResults.result = $"User Email Verified Failed - Couldn't Refresh : {GetCurrentUser().GetData().Email}.";
                        callbackResults.resultCode = AppData.Helpers.WarningCode;
                    }
                });
            }

            return callbackResults;
        }

        public AppData.CallbackData<FirebaseUser> GetCurrentUser()
        {
            var callbackResults = new AppData.CallbackData<FirebaseUser>(AppData.Helpers.GetAppComponentValid(authentication, "User", "Get Current Failed - User Is not Found - Invalid Operation"));

            if (callbackResults.Success())
            {
                if (authentication.CurrentUser.IsValid())
                {
                    callbackResults.result = $"User : {authentication.CurrentUser.Email} Has Been Successfully Found.";
                    callbackResults.data = authentication.CurrentUser;
                }
                else
                {
                    callbackResults.result = $"User Is Not Valid.";
                    callbackResults.data = default;
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }

            return callbackResults;
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