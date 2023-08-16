using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Android;
using System.IO;

namespace Com.RedicalGames.Filar
{
    public class AppManager : AppMonoBaseClass
    {
        #region Static

        private static AppManager _instance;

        public static AppManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<AppManager>();

                return _instance;
            }
        }

        #endregion

        #region Components

        [SerializeField]
        AppData.AppInfoData appInfo = new AppData.AppInfoData();

        [Space(5)]
        [SerializeField]
        AppData.UIScreenType appBootScreen = AppData.UIScreenType.LandingPageScreen;

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

        AppData.Profile userProfile = new AppData.Profile();

        public AppData.Compatibility Compatibility { get; private set; }

        #region Loading Data

        #endregion

        #endregion

        #region Unity Callbacks

        void Awake() => SetupInstance();

        void Start() => Init();

        #endregion

        #region Main

        void SetupInstance()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(_instance.gameObject);
            }

            _instance = this;

            if (requestStoragePermissions)
                StoragePermissionRequest();
        }

        async void Init(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, "Screen UI Manager Is Not Yet Initialized."));

            if(callbackResults.Success())
            {
                var screenUIManager = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, "Screen UI Manager Is Not Yet Initialized.").data;

                do
                {
                    screenUIManager.OnScreenInit(appInitializationCallbackResults =>
                    {
                        callbackResults.SetResults(appInitializationCallbackResults);
                    });

                    await Task.Yield();
                }
                while (!screenUIManager.IsInitialized());

                if (callbackResults.Success())
                {
                    AppData.Helpers.GetAppComponentValid(DatabaseManager.Instance, DatabaseManager.Instance.name, sceneAssetsManagerCallbackResults =>
                    {
                        callbackResults.SetResults(sceneAssetsManagerCallbackResults);

                        if (callbackResults.Success())
                        {
                            var sceneAssetsManager = sceneAssetsManagerCallbackResults.data;

                            sceneAssetsManager.GetScreenLoadInfoInstanceFromLibrary(AppData.UIScreenType.SplashScreen, splashScreenLoadInfoCallbackResults =>
                            {
                                callbackResults.SetResults(splashScreenLoadInfoCallbackResults);

                                if (callbackResults.Success())
                                {
                                    var splashScreenLoadInfo = splashScreenLoadInfoCallbackResults?.data;

                                    if (splashScreenLoadInfo != null)
                                    {
                                        AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, LoadingManager.Instance.name, async loadingManagerCallbackResults =>
                                        {
                                            callbackResults.SetResults(loadingManagerCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                var loadingManager = loadingManagerCallbackResults.data;

                                                await loadingManager.LoadScreen(splashScreenLoadInfo, async showSplashScreenCallbackResults =>
                                                {
                                                    callbackResults.SetResult(showSplashScreenCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        callbackResults.SetResult(sceneAssetsManager.GetInitialScreenLoadInfoInstanceFromLibrary());

                                                        if (callbackResults.Success())
                                                        {
                                                            var initialLoadInfo = sceneAssetsManager.GetInitialScreenLoadInfoInstanceFromLibrary().data;

                                                            await loadingManager.LoadScreen(initialLoadInfo, initialLoadInfoCallbackResults =>
                                                            {
                                                                callbackResults.SetResult(initialLoadInfoCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    screenUIManager.GetCurrentScreen(currentScreenCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(currentScreenCallbackResults);

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            screenUIManager.Refresh();

                                                                            //var currentScreen = currentScreenCallbackResults.data;

                                                                            //AppData.SceneDataPackets dataPackets = new AppData.SceneDataPackets
                                                                            //{
                                                                            //    screenType = AppData.UIScreenType.LandingPageScreen,
                                                                            //    widgetType = AppData.WidgetType.SignInWidget,
                                                                            //    blurScreen = true,
                                                                            //    blurContainerLayerType = AppData.ScreenBlurContainerLayerType.Background
                                                                            //};

                                                                            //currentScreen.value.ShowWidget(dataPackets);

                                                                            // LogSuccess($" *==========> Loaded Screen : {currentScreen.value.GetUIScreenType()}", this);

                                                                        }
                                                                    });
                                                                }
                                                            });
                                                        }
                                                    }
                                                });
                                            }

                                        }, "Screen UI Manager Instance Is Not Yet Initialized");
                                    }
                                }
                            });
                        }

                    }, "Screen UI Manager Instance Is Not Yet Initialized");
                }
            }
        }

        void DeleteThis()
        {
            //if (SceneAssetsManager.Instance != null)
            //    SceneAssetsManager.Instance.SetCurrentSceneAsset(SceneAssetsManager.Instance.GetSceneAssets()[0]);;

            //OnProjectSupport(projectSupportCallbackResults => 
            //{
            //    if (projectSupportCallbackResults.Success())
            //    {
            //        AppData.Helpers.GetComponent(SceneAssetsManager.Instance, hasComponentCallbackResults =>
            //        {
            //            if (hasComponentCallbackResults.Success())
            //            {
            //                SceneAssetsManager.Instance.InitializeStorage(storageInitializedCallbackResults =>
            //                {
            //                    if (storageInitializedCallbackResults.Success())
            //                    {
            //                        SceneAssetsManager.Instance.LoadRootStructureData(loadedProjectDataResultsCallback =>
            //                        {
            //                            if (loadedProjectDataResultsCallback.Success())
            //                            {
            //                                var rootProjectStructure = loadedProjectDataResultsCallback.data.GetProjectStructureData();

            //                                SceneAssetsManager.Instance.SetCurrentProjectStructureData(rootProjectStructure, projectStructureInitializedCallbackResults => 
            //                                {
            //                                    if (projectStructureInitializedCallbackResults.Success())
            //                                        OnLoadAppInitializationBootScreen();
            //                                    else
            //                                        Log(projectStructureInitializedCallbackResults.resultsCode, projectStructureInitializedCallbackResults.results, this);
            //                                });

            //                                // Check This Later On.......................................................................................................................................

            //                                //SceneAssetsManager.Instance.GetDynamicWidgetsContainer(SceneAssetsManager.Instance.GetContainerType(initialLoadDataPackets.screenType), containerResults =>
            //                                //{
            //                                //    if (containerResults.Success())
            //                                //    {
            //                                //        var rootFolder = rootProjectStructure.rootFolder;
            //                                //        var container = containerResults.data;

            //                                //        SceneAssetsManager.Instance.SetWidgetsRefreshData(rootFolder, container);

            //                                //        LogInfo($" <<<<<<<<<<<-------------->>>>>>>>>>> On Load App", this);
            //                                //    }
            //                                //    else
            //                                //        Log(containerResults.resultsCode, containerResults.results, this);
            //                                //});
            //                            }
            //                            else
            //                                Log(loadedProjectDataResultsCallback.resultsCode, loadedProjectDataResultsCallback.results, this);
            //                        });
            //                    }
            //                    else
            //                        Log(storageInitializedCallbackResults.resultsCode, storageInitializedCallbackResults.results, this);
            //                });
            //            }
            //        });
            //    }
            //    else
            //        Log(projectSupportCallbackResults.resultsCode, projectSupportCallbackResults.results, this);
            //});
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

        void OnLoadAppInitializationBootScreen()
        {
            AppData.Helpers.GetAppComponentValid(DatabaseManager.Instance, DatabaseManager.Instance.name, sceneAssetsManagerCallbackResults => 
            {
                if (sceneAssetsManagerCallbackResults.Success())
                {
                    AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, screenManagerComponentCallbackResults =>
                    {
                        if (screenManagerComponentCallbackResults.Success())
                        {
                            var screenManager = screenManagerComponentCallbackResults.data;

                            screenManager.OnAppBootScreen(AppData.UIScreenType.SplashScreen, async onAppBootCallbackResults =>
                            {
                                if (onAppBootCallbackResults.Success())
                                {
                                    await screenManager.ShowScreenAsync(onAppBootCallbackResults.data);

                                    int splashScreenDuration = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(sceneAssetsManagerCallbackResults.data.GetDefaultExecutionValue(AppData.RuntimeExecution.OnSplashScreenExitDelay).value);
                                    await Task.Delay(splashScreenDuration);

                                    await screenManager.HideScreenAsync(onAppBootCallbackResults.data);

                                    int appLoadDelayedDuration = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(sceneAssetsManagerCallbackResults.data.GetDefaultExecutionValue(AppData.RuntimeExecution.OnScreenChangedExitDelay).value);
                                    await Task.Delay(appLoadDelayedDuration);

                                    #region Trigger Loading Manager

                                    sceneAssetsManagerCallbackResults.data.GetDataPacketsLibrary().GetDataPacket(appBootScreen, loadedInitialDataPacketsCallbackResults =>
                                    {
                                        if (loadedInitialDataPacketsCallbackResults.Success())
                                        {
                                            if (loadedInitialDataPacketsCallbackResults.data.dataPackets.screenTransition == AppData.ScreenLoadTransitionType.LoadingScreen)

                                                AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, LoadingManager.Instance.name, loadingManagerCallbackResults => 
                                                {
                                                    if (loadingManagerCallbackResults.Success())
                                                    {
                                                        loadingManagerCallbackResults.data.Init(async loadingManagerInitializationCallbackResults => 
                                                        {
                                                            if(loadingManagerInitializationCallbackResults.Success())
                                                            {
                                                                await screenManager.GoToSelectedScreenAsync(loadedInitialDataPacketsCallbackResults.data.dataPackets, screenLoadedCallbackResults =>
                                                                {
                                                                    if (screenLoadedCallbackResults.Success())
                                                                    {
                                                                        //AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, ProfileManager.Instance.name, profileManagerCallbackResults =>
                                                                        //{
                                                                        //    if (profileManagerCallbackResults.Success())
                                                                        //    {
                                                                        //        if (profileManagerCallbackResults.data.SignedIn == false)
                                                                        //        {
                                                                        //            screenManager.GetScreen(screenLoadedCallbackResults.data.screenType, async loadedScreenCallbacResults =>
                                                                        //            {
                                                                        //                if (loadedScreenCallbacResults.Success())
                                                                        //                {
                                                                        //                    AppData.SceneDataPackets dataPackets = new AppData.SceneDataPackets
                                                                        //                    {
                                                                        //                        screenType = AppData.UIScreenType.LandingPageScreen,
                                                                        //                        widgetType = AppData.WidgetType.SignInWidget,
                                                                        //                        blurScreen = true,
                                                                        //                        blurContainerLayerType = AppData.ScreenBlurContainerLayerType.Background
                                                                        //                    };

                                                                        //                    var success = await CheckConnectionStatus();

                                                                        //                    if (success)
                                                                        //                        loadedScreenCallbacResults.data.value.ShowWidget(dataPackets);
                                                                        //                    else
                                                                        //                    {
                                                                        //                        AppData.SceneDataPackets networkDataPackets = new AppData.SceneDataPackets
                                                                        //                        {
                                                                        //                            screenType = AppData.UIScreenType.LandingPageScreen,
                                                                        //                            widgetType = AppData.WidgetType.NetworkNotificationWidget,
                                                                        //                            blurScreen = true,
                                                                        //                            blurContainerLayerType = AppData.ScreenBlurContainerLayerType.Default
                                                                        //                        };

                                                                        //                        loadedScreenCallbacResults.data.value.ShowWidget(networkDataPackets);
                                                                        //                        LogError(" <++++++++++++++++++++++++++> Show Network Error Pop-Up", this);
                                                                        //                    }
                                                                        //                }
                                                                        //            });
                                                                        //        }
                                                                        //    }
                                                                        //    else
                                                                        //        Log(profileManagerCallbackResults.resultsCode, profileManagerCallbackResults.results, this);

                                                                        //}, "Profile Manager Instance Is Not Yet Initialized.");
                                                                    }

                                                                    Log(screenLoadedCallbackResults.resultCode, screenLoadedCallbackResults.result, this);
                                                                });
                                                            }
                                                            else
                                                                Log(loadingManagerInitializationCallbackResults.resultCode, loadingManagerInitializationCallbackResults.result, this);
                                                        });
                                                    }
                                                    else
                                                        Log(loadingManagerCallbackResults.resultCode, loadingManagerCallbackResults.result, this);
                                                
                                                }, "Profile Manager Instance Is Not Yet Initialized.");

                                            else
                                                LogInfo($"Load Screen : {loadedInitialDataPacketsCallbackResults.data.dataPackets.screenType} With Transition Type : {loadedInitialDataPacketsCallbackResults.data.dataPackets.screenTransition}", this);
                                        }
                                        else
                                            Log(loadedInitialDataPacketsCallbackResults.resultCode, loadedInitialDataPacketsCallbackResults.result, this);
                                    });

                                    #endregion
                                }
                                else
                                    Log(onAppBootCallbackResults.resultCode, onAppBootCallbackResults.result, this);
                            });
                        }
                        else
                            Log(screenManagerComponentCallbackResults.resultCode, screenManagerComponentCallbackResults.result, this);
                    }, "Screen UI Manager Instance Is Not Yet Initialized.");
                }
                else
                    Log(sceneAssetsManagerCallbackResults.resultCode, sceneAssetsManagerCallbackResults.result, this);
            
            }, "Scene Assets Manager Instance Is Not Yet Initialized.");
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

        #region Synchronizing App Info

        public async Task<AppData.CallbackData<AppData.AppInfo>> SynchronizingAppInfo()
        {
            AppData.CallbackData<AppData.AppInfo> callbackResults = new AppData.CallbackData<AppData.AppInfo>(AppData.Helpers.GetAppComponentValid(DatabaseManager.Instance, DatabaseManager.Instance.name, "Database Manager is Not Yet Initialized."));

            if(callbackResults.Success())
            {
                var sceneAssetsManager = AppData.Helpers.GetAppComponentValid(DatabaseManager.Instance, DatabaseManager.Instance.name).data;
                await sceneAssetsManager.InitializeDatabase();
            }

            return callbackResults;
        }

        #endregion

        #region Initialize App Entry

        public async Task<AppData.CallbackData<AppData.AppInfo>> CheckEntryPointAsync()
        {
            AppData.CallbackData<AppData.AppInfo> callbackResults = new AppData.CallbackData<AppData.AppInfo>(AppData.Helpers.GetAppComponentValid(DatabaseManager.Instance, DatabaseManager.Instance.name, "Database Manager is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var sceneAssetsManager = AppData.Helpers.GetAppComponentValid(DatabaseManager.Instance, DatabaseManager.Instance.name).data;

                do
                    await Task.Delay(100);
                while (!sceneAssetsManager.IsServerAppInfoDatabaseInitialized);

                if(sceneAssetsManager.IsServerAppInfoDatabaseInitialized)
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

        #region Profile

        public async Task<AppData.Callback> ProfileInitialization()
        {
            AppData.Callback callbackResults = new AppData.Callback();

            await Task.Delay(2000);

            callbackResults.result = "Profile Initialization.";
            callbackResults.resultCode = AppData.Helpers.SuccessCode;

            return callbackResults;
        }

        public async Task<AppData.Callback> ProfileInitialized()
        {
            AppData.Callback callbackResults = new AppData.Callback();

            await Task.Delay(2000);

            callbackResults.result = "Profile Initialized.";
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
