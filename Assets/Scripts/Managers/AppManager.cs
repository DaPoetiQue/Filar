using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Android;

namespace Com.RedicalGames.Filar
{
    public class AppManager : AppData.SingletonBaseComponent<AppManager>
    {
        #region Components

        [SerializeField]
        AppData.AppInfoData appInfo = new AppData.AppInfoData();

        [Space(5)]
        [SerializeField]
        AppData.Compatibility testProjectSupport;

        [Space(5)]
        [SerializeField]
        bool requestStoragePermissions;

        [Space(5)]
        [SerializeField]
        AppData.AppMode appMode = AppData.AppMode.None;

        [Space(5)]
        [SerializeField]
        List<AppData.PermissionInfo> permissionInfos = new List<AppData.PermissionInfo>();

        [Space(5)]
        [SerializeField]
        string infoFolderName = "Filar";

        AppData.AppInfo entry = new AppData.AppInfo();
        public bool AppinfoSynced { get; private set; }

        #region Loading Data

        #endregion

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main

        private void Init(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "App Database Manager Instance Is Not Yet Initialized."));

            if(callbackResults.Success())
            {
                var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).GetData();

                databaseManager.InitializeLocalCacheStorage(cacheStorageInitializedCallbackResults =>
                {
                    callbackResults.SetResult(cacheStorageInitializedCallbackResults);

                    if (callbackResults.Success())
                    {
                        AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, async screenUIManagerInstanceCallbackResults =>
                        {
                            callbackResults.SetResults(screenUIManagerInstanceCallbackResults);

                            if (callbackResults.Success())
                            {
                                callbackResults.SetResults(databaseManager.GetAssetBundlesLibrary());

                                if (callbackResults.Success())
                                {
                                    databaseManager.LoadSplashImagesDataOnInitialization();
                                    databaseManager.GetAssetBundlesLibrary().GetData().Initialize();

                                    var screenUIManager = screenUIManagerInstanceCallbackResults.GetData();
                                    var onScreenInitializationTaskResultsCallback = await screenUIManager.OnScreenInitAsync();

                                    callbackResults.SetResult(onScreenInitializationTaskResultsCallback);

                                    if (callbackResults.Success())
                                    {
                                        databaseManager.GetScreenLoadInfoInstanceFromLibrary(AppData.ScreenType.SplashScreen, async splashScreenLoadInfoCallbackResults =>
                                        {
                                            callbackResults.SetResults(splashScreenLoadInfoCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                var splashScreenLoadInfo = splashScreenLoadInfoCallbackResults?.GetData();

                                                #region Trigger Splash Image

                                                screenUIManager.GetScreen(AppData.ScreenType.LoadingScreen, loadingScreenCallbackResults =>
                                                {
                                                    callbackResults.SetResults(loadingScreenCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        if (splashScreenLoadInfo != null)
                                                        {
                                                            AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, LoadingManager.Instance.name, async loadingManagerCallbackResults =>
                                                            {
                                                                callbackResults.SetResults(loadingManagerCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    var loadingManager = loadingManagerCallbackResults.data;
                                                                    var currentScreenView = loadingScreenCallbackResults.data;

                                                                    splashScreenLoadInfo.SetReferencedScreen(currentScreenView);

                                                                    var splashDisplayerWidgetCallbackResults = currentScreenView.GetWidgetOfType(AppData.WidgetType.ImageDisplayerWidget);

                                                                    callbackResults.SetResults(splashDisplayerWidgetCallbackResults);

                                                                    LogInfo($" __________________________________________++++++++++++++ Geting Widget From Screen : {currentScreenView.name} - Of Type : {currentScreenView.GetScreenType()} With : {currentScreenView.GetWidgets().GetData().Count} Widget(s) - Code : {callbackResults.GetResultCode} - Resuts : {callbackResults.GetResult}", this);

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        callbackResults.SetResult(splashDisplayerWidgetCallbackResults.GetData().Initialized());

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            var splashDisplayerWidget = splashDisplayerWidgetCallbackResults.GetData();

                                                                            await loadingManager.LoadScreen(splashScreenLoadInfo, async showSplashScreenCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(showSplashScreenCallbackResults);

                                                                                if (callbackResults.Success())
                                                                                {
                                                                                    currentScreenView.ShowWidget(splashDisplayerWidget);

                                                                                    callbackResults.SetResult(databaseManager.GetInitialScreenLoadInfoInstanceFromLibrary());

                                                                                    if (callbackResults.Success())
                                                                                    {
                                                                                        var initialLoadInfo = databaseManager.GetInitialScreenLoadInfoInstanceFromLibrary().data;

                                                                                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, NetworkManager.Instance.name, "Network Manager Instance Is Not Yet Initialized."));

                                                                                        if (callbackResults.Success())
                                                                                        {
                                                                                            var networkManager = AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, NetworkManager.Instance.name).data;

                                                                                            if (networkManager.Connected)
                                                                                                initialLoadInfo.RemoveSequenceInstanceData(AppData.LoadingSequenceID.CheckingNetworkConnection);

                                                                                            initialLoadInfo.SetReferencedScreen(currentScreenView);

                                                                                            await loadingManager.LoadScreen(initialLoadInfo, initialLoadInfoCallbackResults =>
                                                                                            {
                                                                                                callbackResults.SetResult(initialLoadInfoCallbackResults);

                                                                                                if (callbackResults.Success())
                                                                                                {
                                                                                                    screenUIManager.GetCurrentScreen(async currentScreenCallbackResults =>
                                                                                                    {
                                                                                                        callbackResults.SetResult(currentScreenCallbackResults);

                                                                                                        if (callbackResults.Success())
                                                                                                        {
                                                                                                            var screen = currentScreenCallbackResults.data;

                                                                                                            if (screen.GetType().GetData() == AppData.ScreenType.LandingPageScreen)
                                                                                                            {
                                                                                                                var widget = screen.GetWidget(AppData.WidgetType.PostsWidget);

                                                                                                                if (widget != null)
                                                                                                                {
                                                                                                                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(PostManager.Instance, "Post Manager Instance", "Post Manager Instance Is Not Yet Initialized."));

                                                                                                                    if (callbackResults.Success())
                                                                                                                    {
                                                                                                                        var postManagerInstance = AppData.Helpers.GetAppComponentValid(PostManager.Instance, "Post Manager Instance", "Post Manager Instance Is Not Yet Initialized.").GetData();

                                                                                                                        widget.GetData().SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Shown);
                                                                                                                        widget.GetData().SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Hidden);

                                                                                                                        await screenUIManager.RefreshAsync();

                                                                                                                        screen.ShowWidget(widget.GetData());

                                                                                                                        widget.GetData().SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Shown);
                                                                                                                        widget.GetData().SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Hidden);

                                                                                                                        screen.ShowWidget(AppData.WidgetType.LoadingWidget, async widgetShownCallbackResults => 
                                                                                                                        {
                                                                                                                            callbackResults.SetResult(widgetShownCallbackResults);

                                                                                                                            if(callbackResults.Success())
                                                                                                                            {
                                                                                                                                await Task.Delay(2500);

                                                                                                                                postManagerInstance.SelectPost(postSelectedCallbacKResults =>
                                                                                                                                {
                                                                                                                                    callbackResults.SetResult(postSelectedCallbacKResults);

                                                                                                                                    if (callbackResults.Success())
                                                                                                                                    {
                                                                                                                                        screen.HideScreenWidget(AppData.WidgetType.LoadingWidget);
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                                                                                });
                                                                                                                            }
                                                                                                                        });
                                                                                                                    }
                                                                                                                    else
                                                                                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    callbackResults.result = $"Widget Type : {AppData.WidgetType.PostsWidget} For Screen : {screen.GetType().GetData()} Not Found.";
                                                                                                                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                callbackResults.result = $"Screen : {screen.GetType().GetData()} Does Not Match Expected Screen Type : {AppData.ScreenType.LandingPageScreen}";
                                                                                                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                                                                            }
                                                                                                        }
                                                                                                    });
                                                                                                }
                                                                                            });
                                                                                        }
                                                                                    }
                                                                                }
                                                                            });
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                            }, "Screen UI Manager Instance Is Not Yet Initialized");
                                                        }
                                                        else
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });

                                                #endregion
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
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        }, "Screen UI Manager Instance Is Not Yet Initialized");
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        void OnProjectSupport(Action<AppData.CallbackData<AppData.ProjectRestriction>> callback)
        {
            AppData.CallbackData<AppData.ProjectRestriction> callbackResults = new AppData.CallbackData<AppData.ProjectRestriction>();

            var supportRestriction = new AppData.ProjectRestriction();

            supportRestriction.name = "Project Support Restriction";
            supportRestriction.SetRestrictionType(AppData.AppRestrictionType.ProjectSupport);
            supportRestriction.SetProjectSupportType(GetProjectSupportType());

            if(!appInfo.appRestrictions.Contains(supportRestriction))
            {
                appInfo.appRestrictions.Add(supportRestriction);

                callbackResults.result = $"Added Project Restriction With Support Type : {GetProjectSupportType()}.";
                callbackResults.data = supportRestriction;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Project Restriction Of Type : {GetProjectSupportType()} Already Exists In App Restrictions.";
                callbackResults.data = supportRestriction;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }

            callback.Invoke(callbackResults);
        }

        AppData.Compatibility GetProjectSupportType()
        {
            return testProjectSupport;
        }

        public void StoragePermissionRequest()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) || !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }

            string[] permissions = new string[1];
            permissions[0] = "android.permission.MANAGE_EXTERNAL_STORAGE";

            Permission.RequestUserPermissions(permissions);
        }

        public void CameraUsagePermissionRequest(string description = "")
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
                Permission.RequestUserPermission(Permission.Camera);
        }

        public bool PermissionGranted(string permission)
        {
            if (Permission.HasUserAuthorizedPermission(permission))
                return true;
            else
                return false;

        }

        public bool ReadWritePermissionsGranted() => PermissionGranted(Permission.ExternalStorageRead) && PermissionGranted(Permission.ExternalStorageRead);

        public AndroidJavaObject GetInitializedPluginInstance(string pluginBundle)
        {
            AndroidJavaObject pluginInstance = new AndroidJavaObject(pluginBundle);

            return pluginInstance;
        }

        public bool IsRuntime()
        {
            return Application.platform == RuntimePlatform.Android;
        }

        public void GetAppRestriction(AppData.AppRestrictionType restrictionType, Action<AppData.CallbackData<AppData.ProjectRestriction>> callback)
        {
            AppData.CallbackData<AppData.ProjectRestriction> callbackResults = new AppData.CallbackData<AppData.ProjectRestriction>();

            if(appInfo.GetAppRestriction() != null)
            {
                var restriction = appInfo.GetAppRestriction().Find(x => x.GetAppRestrictionType() == restrictionType);

                if(restriction != null)
                {
                    callbackResults.result = $"App Info Restriction Found.";
                    callbackResults.data = restriction;
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.result = $"Get App Restriction Failed : App Info Restriction Of Type : {restrictionType} Not Found / Not Yet Initialized.";
                    callbackResults.data = default;
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.result = "Get App Restriction Failed : App Info Restrictions Are Not Yet Initialized.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        #region Info

        public AppData.CallbackData<string> GetApplicationName()
        {
            var callbackResults = new AppData.CallbackData<string>();

            string appName = Application.productName;

            if(!string.IsNullOrEmpty(appName))
            {
                callbackResults.result = $"Application Name Is Set To : {appName}.";
                callbackResults.data = appName;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Application Name Not Assigned.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            return callbackResults;
        }

        public AppData.CallbackData<string> GetCompanyName()
        {
            var callbackResults = new AppData.CallbackData<string>();

            string companyName = Application.companyName;

            if (!string.IsNullOrEmpty(companyName))
            {
                callbackResults.result = $"Application's Company Name Is Set To : {companyName}.";
                callbackResults.data = companyName;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Application's Company Name Not Assigned.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            return callbackResults;
        }

        public AppData.CallbackData<string> GetApplicationVersion()
        {
            var callbackResults = new AppData.CallbackData<string>();

            string version = Application.version;

            if (!string.IsNullOrEmpty(version))
            {
                callbackResults.result = $"Application's Version Is Set To : {version}.";
                callbackResults.data = version;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Application's Version Not Assigned.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            return callbackResults;
        }

        #endregion

        #region Synchronizing App Info

        public async Task<AppData.CallbackData<AppData.AppInfo>> SynchronizingAppInfo()
        {
            AppData.CallbackData<AppData.AppInfo> callbackResults = new AppData.CallbackData<AppData.AppInfo>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager is Not Yet Initialized."));

            if(callbackResults.Success())
            {
                var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;
                await databaseManager.InitializeDatabase();
            }

            return callbackResults;
        }

        public void SyncAppInfo(AppData.AppInfo appInfo)
        {
            this.entry = appInfo;
            AppinfoSynced = true;
        }

        #endregion

        #region Initialize App Entry

        public async Task<AppData.CallbackData<AppData.AppInfo>> CheckEntryPointAsync()
        {
            AppData.CallbackData<AppData.AppInfo> callbackResults = new AppData.CallbackData<AppData.AppInfo>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                while (!databaseManager.IsServerAppInfoDatabaseInitialized)
                    await Task.Yield();

                if(databaseManager.IsServerAppInfoDatabaseInitialized)
                {
                    callbackResults.result = "App Info Has Been Synchronized.";
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.result = "App Info Synchronization Failed.";
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }

            return callbackResults;
        }

        public void GetAppInfo(Action<AppData.CallbackData<AppData.AppInfo>> callback)
        {
            AppData.CallbackData<AppData.AppInfo> callbackResults = new AppData.CallbackData<AppData.AppInfo>();

            if(AppinfoSynced)
            {
                callbackResults.result = "App Info Is Synced.";
                callbackResults.data = entry;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "App Info Is Not Synced.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        #endregion

        #region Download Entry Data

        public async Task<AppData.CallbackData<AppData.AppInfo>> DownloadPostEntryDataAsync()
        {
            var callbackResults = new AppData.CallbackData<AppData.AppInfo>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).GetData();

                var downloadInitialPostContentAsyncCallbackResultsTask = await appDatabaseManagerInstance.DownloadInitialPostContentAsync();

                callbackResults.SetResult(downloadInitialPostContentAsyncCallbackResultsTask);

                if(callbackResults.Success())
                {
                    LogInfo("", this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

        #region Permissions

        public async Task<AppData.Callback> PermissionsGranted()
        {
            AppData.Callback callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(AppData.Helpers.GetQueue(permissionInfos), queueIdentifier: "Permission Infos", failedOperationFallbackResults: "Permissions Info Are Not Yet Initialized In App Manager", " Permissions Info Initoialized Successfully"));

            if (callbackResults.Success())
            {
                var permissionInfoQueue = AppData.Helpers.GetAppComponentsValid(AppData.Helpers.GetQueue(permissionInfos), queueIdentifier: "", failedOperationFallbackResults: "", "").data;

                while(permissionInfoQueue.Count > 0)
                {
                    var permisionInfo = permissionInfoQueue.Dequeue();

                    while(permisionInfo.IsGranted == false)
                        await Task.Delay(100);

                    await Task.Yield();
                }

                callbackResults.result = "Permissions Granted.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }

            return callbackResults;
        }

        #endregion

        #region Compatibility

        public async Task<AppData.CallbackData<AppData.Compatibility>> GetCompatibilityStatusAsync()
        {
            AppData.CallbackData<AppData.Compatibility> callbackResults = new AppData.CallbackData<AppData.Compatibility>();

            await Task.Delay(1000);

            #region Grant Default 3D Support.

            callbackResults.result = "Is Only Compitable With Default 3D Support.";
            callbackResults.data = AppData.Compatibility.Supports_3D;
            callbackResults.resultCode = AppData.Helpers.SuccessCode;

            #endregion

            await Task.Delay(1000);

            #region Check For AR Support.

            StartCoroutine(ARSession.CheckAvailability());

            while (ARSession.state == ARSessionState.CheckingAvailability)
                await Task.Yield();

            if (ARSession.state != ARSessionState.Unsupported)
            {
                if (ARSession.state == ARSessionState.NeedsInstall)
                {
                    // Show Require Install Pop Up
                }

                if (ARSession.state == ARSessionState.Installing)
                {
                    while (ARSession.state == ARSessionState.Installing)
                        await Task.Yield();
                }

                if (ARSession.state == ARSessionState.Ready)
                {
                    callbackResults.result = "Has AR Support.";
                    callbackResults.data = AppData.Compatibility.Supports_AR;
                }
            }

            #endregion

            await Task.Delay(1000);

            #region Check For VR Support.

            if (callbackResults.Success() && callbackResults.data == AppData.Compatibility.Supports_AR && Input.gyro.enabled)
            {
                callbackResults.result = "Has VR Support.";
                callbackResults.data = AppData.Compatibility.Supports_VR;
            }

            #endregion

            return callbackResults;
        }

        #endregion

        #region Storage

        public async Task<AppData.Callback> StorageInitialized()
        {
            AppData.Callback callbackResults = new AppData.Callback();

            await Task.Delay(2000);

            callbackResults.result = "Storage Initialized.";
            callbackResults.resultCode = AppData.Helpers.SuccessCode;

            return callbackResults;
        }

        #endregion

        public void SetAppMode(AppData.AppMode appMode) => this.appMode = appMode;

        public AppData.AppMode GetAppMode()
        {
            return appMode;
        }

        #endregion
    }
}
