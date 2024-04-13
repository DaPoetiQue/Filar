using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Android;
using System.IO;

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
        private string settingsFileName = "settings";

        [Space(5)]
        [SerializeField]
        private bool startBootSequence = false;

        #region Loading Data

        #endregion

        #endregion

        #region Main

        protected override void Init()
        {
            var callbackResults = new AppData.Callback();

            if (startBootSequence)
            {
                BootSequence(bootSequenceCallbackResults => 
                {
                    callbackResults.SetResult(bootSequenceCallbackResults);

                    if(callbackResults.UnSuccessful())
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
            {
                BuildAppUI(buildAppUICallbackResults => 
                {
                    callbackResults.SetResult(buildAppUICallbackResults);

                    if (callbackResults.UnSuccessful())
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
        }

        private void BuildAppUI(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Build App UI Failed - App Database Manager Instance Is Not Yet Initialized - Invalid Operation."));

            if (callbackResults.Success())
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
                                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(SurfacingGraphManager.Instance, "Surfacing Graph Manager Instance", "Build App UI Failed - Surfacing Graph Manager Instance Is Not Yet Initialized - Invalid Operation."));

                                        if(callbackResults.Success())
                                        {
                                            var surfacingGraphManagerInstance = AppData.Helpers.GetAppComponentValid(SurfacingGraphManager.Instance, "Surfacing Graph Manager Instance").GetData();

                                            surfacingGraphManagerInstance.BuildGraphs(buildGraphsCallbackResults => 
                                            {
                                                callbackResults.SetResult(buildGraphsCallbackResults);

                                                if(callbackResults.UnSuccessful())
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

            callback?.Invoke(callbackResults);
        }

        private void BootSequence(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "App Database Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).GetData();

                databaseManager.InitializeLocalCacheStorage(cacheStorageInitializedCallbackResults =>
                {
                    callbackResults.SetResult(cacheStorageInitializedCallbackResults);

                    if (callbackResults.Success())
                    {
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
                                            databaseManager.GetScreenLoadInfoInstanceFromLibrary(AppData.ScreenType.SplashScreen, splashScreenLoadInfoCallbackResults =>
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
                                                                AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance", async loadingManagerCallbackResults =>
                                                                {
                                                                    callbackResults.SetResults(loadingManagerCallbackResults);

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        var loadingManager = loadingManagerCallbackResults.GetData();
                                                                        var currentScreenView = loadingScreenCallbackResults.GetData();

                                                                        splashScreenLoadInfo.SetReferencedScreen(currentScreenView);

                                                                        var splashDisplayerWidgetCallbackResults = currentScreenView.GetWidget(AppData.WidgetType.ImageDisplayerWidget);

                                                                        callbackResults.SetResults(splashDisplayerWidgetCallbackResults);

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
                                                                                            var initialLoadInfo = databaseManager.GetInitialScreenLoadInfoInstanceFromLibrary().GetData();

                                                                                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, NetworkManager.Instance.name, "Network Manager Instance Is Not Yet Initialized."));

                                                                                            if (callbackResults.Success())
                                                                                            {
                                                                                                var networkManager = AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, NetworkManager.Instance.name).GetData();

                                                                                                if (networkManager.Connected)
                                                                                                    initialLoadInfo.RemoveSequenceInstanceData(AppData.LoadingSequenceID.CheckingNetworkConnection);

                                                                                                initialLoadInfo.SetReferencedScreen(currentScreenView);

                                                                                                await loadingManager.LoadScreen(initialLoadInfo, initialLoadInfoCallbackResults =>
                                                                                                {
                                                                                                    callbackResults.SetResult(initialLoadInfoCallbackResults);
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
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public AppData.Callback BootSequenceEnabled()
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if(startBootSequence)
            {
                callbackResults.result = "Start Boot Sequence Is Enabled.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Start Boot Sequence Is Disabled.";
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            return callbackResults;
        }

        void OnProjectSupport(Action<AppData.CallbackData<AppData.ProjectRestriction>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.ProjectRestriction>();

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

        #endregion

        #region Download Entry Data

        public async Task<AppData.CallbackData<AppData.AppInfo>> DownloadPostEntryDataAsync()
        {
            var callbackResults = new AppData.CallbackData<AppData.AppInfo>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).GetData();

                var downloadInitialPostContentAsyncCallbackResultsTask = await appDatabaseManagerInstance.DownloadInitialPostContentAsync(AppData.ScreenType.LandingPageScreen);

                callbackResults.SetResult(downloadInitialPostContentAsyncCallbackResultsTask);
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

        #region App Language

        public void CacheAppSettingsDataFile(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "Cache App Settings Data File Failed - App Database Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                callbackResults.SetResult(appDatabaseManagerInstance.GetAppDirectory(AppData.StorageType.Settings_Storage));

                if (callbackResults.Success())
                {
                    var settingsStorageDirectory = appDatabaseManagerInstance.GetAppDirectory(AppData.StorageType.Settings_Storage).GetData();

                    callbackResults.SetResult(GetSettingsFileName());

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(appDatabaseManagerInstance.GetDataPath(GetSettingsFileName().GetData(), settingsStorageDirectory, AppData.FileExtensionType.JSON));

                        if(callbackResults.Success())
                        {
                            settingsStorageDirectory.SetPath(appDatabaseManagerInstance.GetDataPath(GetSettingsFileName().GetData(), settingsStorageDirectory, AppData.FileExtensionType.JSON).GetData());

                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(LocalizationManager.Instance, "Localization Manager Instance", "Cache App Settings Data File Failed - Localization Manager Instance Is Not Initialized Yet - Invalid Operation."));

                            if (callbackResults.Success())
                            {
                                var localizationManagerInstance = AppData.Helpers.GetAppComponentValid(LocalizationManager.Instance, "Localization Manager Instance").GetData();

                                callbackResults.SetResult(localizationManagerInstance.GetCurrentLanguage());

                                if (callbackResults.Success())
                                {
                                    appDatabaseManagerInstance.FileFound(settingsStorageDirectory.GetPath(), pathFoundCallbackResults =>
                                    {
                                        callbackResults.SetResult(pathFoundCallbackResults);

                                        if (callbackResults.UnSuccessful())
                                        {
                                            var appSettingsDataFile = new AppData.AppSettingsDataFile();

                                            appSettingsDataFile.SetAppLanguage((int)localizationManagerInstance.GetCurrentLanguage().GetData());

                                            appDatabaseManagerInstance.CreateData(appSettingsDataFile, settingsStorageDirectory, fileCreatedCallbackResults =>
                                            {
                                                callbackResults.SetResult(fileCreatedCallbackResults);

                                                if (callbackResults.UnSuccessful())
                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            });
                                        }
                                        else
                                        {
                                            callbackResults.SetResult(GetAppSettingsDataFile());

                                            if (callbackResults.Success())
                                            {
                                                var settingsFileData = GetAppSettingsDataFile().GetData();

                                                settingsFileData.SetAppLanguage((int)localizationManagerInstance.GetCurrentLanguage().GetData());

                                                appDatabaseManagerInstance.CreateData(settingsFileData, settingsStorageDirectory, fileCreatedCallbackResults =>
                                                {
                                                    callbackResults.SetResult(fileCreatedCallbackResults);

                                                    if (callbackResults.UnSuccessful())
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        }
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
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackData<AppData.AppSettingsDataFile> GetAppSettingsDataFile()
        {
            var callbackResults = new AppData.CallbackData<AppData.AppSettingsDataFile>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "Get App Settings Data File Failed - App Database Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                callbackResults.SetResult(appDatabaseManagerInstance.GetAppDirectory(AppData.StorageType.Settings_Storage));

                if (callbackResults.Success())
                {
                    var settingsStorageDirectory = appDatabaseManagerInstance.GetAppDirectory(AppData.StorageType.Settings_Storage).GetData();

                    callbackResults.SetResult(GetSettingsFileName());

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(appDatabaseManagerInstance.GetDataPath(GetSettingsFileName().GetData(), settingsStorageDirectory, AppData.FileExtensionType.JSON));

                        if (callbackResults.Success())
                        {
                            settingsStorageDirectory.SetPath(appDatabaseManagerInstance.GetDataPath(GetSettingsFileName().GetData(), settingsStorageDirectory, AppData.FileExtensionType.JSON).GetData());

                            appDatabaseManagerInstance.FileFound(settingsStorageDirectory.GetPath(), pathFoundCallbackResults =>
                            {
                                callbackResults.SetResult(pathFoundCallbackResults);

                                if (callbackResults.Success())
                                {
                                    LogSuccess($"Log_Infos//: Settings File Found At Path : {settingsStorageDirectory.GetPath()}", this);

                                    callbackResults.result = $"Get App Settings Data File Success - Settings File Found At Path : {settingsStorageDirectory.GetPath()}.";

                                    // Deserialize Data.

                                    appDatabaseManagerInstance.LoadData<AppData.AppSettingsDataFile>(settingsStorageDirectory, settingFileLoadedCallbackResults => 
                                    {
                                        callbackResults.SetResult(settingFileLoadedCallbackResults);

                                        if(callbackResults.Success())
                                        {
                                            callbackResults.result = "App Settings Data File Has Been Loaded Successfully.";
                                            callbackResults.data = settingFileLoadedCallbackResults.GetData();
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
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        private AppData.CallbackData<string> GetSettingsFileName()
        {
            var callbackResults = new AppData.CallbackData<string>(AppData.Helpers.GetAppStringValueNotNullOrEmpty(settingsFileName, "Settings File Name", "Get Settings File Name Failed - Settings File Name Value Is Null - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = $"Get Settings File Name Success - Settings File Name Value Is Set To : {settingsFileName}.";
                callbackResults.data = settingsFileName;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

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
