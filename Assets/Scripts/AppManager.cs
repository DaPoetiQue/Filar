using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;

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
        AppData.AppProjectSupportType testProjectSupport;

        [Space(5)]
        [SerializeField]
        AppData.SceneDataPackets initialLoadDataPackets = new AppData.SceneDataPackets();

        [Space(5)]
        [SerializeField]
        bool requestStoragePermissions;

        AppData.Profile userProfile = new AppData.Profile();

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

        void Init()
        {
            //if (SceneAssetsManager.Instance != null)
            //    SceneAssetsManager.Instance.SetCurrentSceneAsset(SceneAssetsManager.Instance.GetSceneAssets()[0]);;

            OnProjectSupport(projectSupportCallbackResults => 
            {
                if (projectSupportCallbackResults.Success())
                {
                    AppData.Helpers.GetComponent(SceneAssetsManager.Instance, hasComponentCallbackResults =>
                    {
                        if (hasComponentCallbackResults.Success())
                        {
                            SceneAssetsManager.Instance.InitializeStorage(storageInitializedCallbackResults =>
                            {
                                if (storageInitializedCallbackResults.Success())
                                {
                                    SceneAssetsManager.Instance.LoadRootStructureData(loadedProjectDataResultsCallback =>
                                    {
                                        if (loadedProjectDataResultsCallback.Success())
                                        {
                                            var rootProjectStructure = loadedProjectDataResultsCallback.data.GetProjectStructureData();

                                            SceneAssetsManager.Instance.SetCurrentProjectStructureData(rootProjectStructure, projectStructureInitializedCallbackResults => 
                                            {
                                                if (projectStructureInitializedCallbackResults.Success())
                                                    OnLoadAppInitializationBootScreen();
                                                else
                                                    Log(projectStructureInitializedCallbackResults.resultsCode, projectStructureInitializedCallbackResults.results, this);
                                            });

                                            // Check This Later On.......................................................................................................................................

                                            //SceneAssetsManager.Instance.GetDynamicWidgetsContainer(SceneAssetsManager.Instance.GetContainerType(initialLoadDataPackets.screenType), containerResults =>
                                            //{
                                            //    if (containerResults.Success())
                                            //    {
                                            //        var rootFolder = rootProjectStructure.rootFolder;
                                            //        var container = containerResults.data;

                                            //        SceneAssetsManager.Instance.SetWidgetsRefreshData(rootFolder, container);

                                            //        LogInfo($" <<<<<<<<<<<-------------->>>>>>>>>>> On Load App", this);
                                            //    }
                                            //    else
                                            //        Log(containerResults.resultsCode, containerResults.results, this);
                                            //});
                                        }
                                        else
                                            Log(loadedProjectDataResultsCallback.resultsCode, loadedProjectDataResultsCallback.results, this);
                                    });
                                }
                                else
                                    Log(storageInitializedCallbackResults.resultsCode, storageInitializedCallbackResults.results, this);
                            });
                        }
                    });
                }
                else
                    Log(projectSupportCallbackResults.resultsCode, projectSupportCallbackResults.results, this);
            });
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

                callbackResults.results = $"Added Project Restriction With Support Type : {GetProjectSupportType()}.";
                callbackResults.data = supportRestriction;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = $"Project Restriction Of Type : {GetProjectSupportType()} Already Exists In App Restrictions.";
                callbackResults.data = supportRestriction;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }

            callback.Invoke(callbackResults);
        }

        AppData.AppProjectSupportType GetProjectSupportType()
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

        public AndroidJavaObject GetInitializedPluginInstance(string pluginBundle)
        {
            AndroidJavaObject pluginInstance = new AndroidJavaObject(pluginBundle);

            return pluginInstance;
        }

        void OnLoadAppInitializationBootScreen()
        {
            AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name, sceneAssetsManagerCallbackResults => 
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

                                    int splashScreenDuration = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(sceneAssetsManagerCallbackResults.data.GetDefaultExecutionValue(AppData.RuntimeValueType.OnSplashScreenExitDelay).value);
                                    await Task.Delay(splashScreenDuration);

                                    await screenManager.HideScreenAsync(onAppBootCallbackResults.data);

                                    int appLoadDelayedDuration = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(sceneAssetsManagerCallbackResults.data.GetDefaultExecutionValue(AppData.RuntimeValueType.OnScreenChangedExitDelay).value);
                                    await Task.Delay(appLoadDelayedDuration);

                                    #region Trigger Loading Manager

                                    sceneAssetsManagerCallbackResults.data.GetDataPacketsLibrary().GetDataPacket(appBootScreen, async loadedInitialDataPacketsCallbackResults =>
                                    {
                                        if (loadedInitialDataPacketsCallbackResults.Success())
                                        {
                                            if (loadedInitialDataPacketsCallbackResults.data.dataPackets.screenTransition == AppData.ScreenLoadTransitionType.LoadingScreen)
                                                await screenManager.GoToSelectedScreenAsync(loadedInitialDataPacketsCallbackResults.data.dataPackets, screenLoadedCallbackResults =>
                                                {
                                                    if(screenLoadedCallbackResults.Success())
                                                    {
                                                        AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, ProfileManager.Instance.name, profileManagerCallbackResults => 
                                                        {
                                                            if (profileManagerCallbackResults.Success())
                                                            {
                                                                if(profileManagerCallbackResults.data.SignedIn == false)
                                                                {
                                                                    screenManager.GetScreen(screenLoadedCallbackResults.data.screenType, async loadedScreenCallbacResults => 
                                                                    {
                                                                        if (loadedScreenCallbacResults.Success())
                                                                        {
                                                                            AppData.SceneDataPackets dataPackets = new AppData.SceneDataPackets
                                                                            {
                                                                                screenType = AppData.UIScreenType.LandingPageScreen,
                                                                                widgetType = AppData.WidgetType.LoginViewWidget,
                                                                                blurScreen = true,
                                                                                blurContainerLayerType = AppData.ScreenBlurContainerLayerType.Background
                                                                            };

                                                                            var success = await CheckConnectionStatus();

                                                                            if(success)
                                                                                loadedScreenCallbacResults.data.value.ShowWidget(dataPackets);
                                                                            else
                                                                            {
                                                                                AppData.SceneDataPackets networkDataPackets = new AppData.SceneDataPackets
                                                                                {
                                                                                    screenType = AppData.UIScreenType.LandingPageScreen,
                                                                                    widgetType = AppData.WidgetType.NetworkNotificationWidget,
                                                                                    blurScreen = true,
                                                                                    blurContainerLayerType = AppData.ScreenBlurContainerLayerType.Default
                                                                                };

                                                                                loadedScreenCallbacResults.data.value.ShowWidget(networkDataPackets);
                                                                                LogError(" <++++++++++++++++++++++++++> Show Network Error Pop-Up", this);
                                                                            }
                                                                        }
                                                                    });
                                                                }
                                                            }
                                                            else
                                                                Log(profileManagerCallbackResults.resultsCode, profileManagerCallbackResults.results, this);

                                                        }, "Profile Manager Is Not Yet Initialized.");
                                                    }

                                                    Log(screenLoadedCallbackResults.resultsCode, screenLoadedCallbackResults.results, this);
                                                });

                                            else
                                                LogInfo($"Load Screen : {loadedInitialDataPacketsCallbackResults.data.dataPackets.screenType} With Transition Type : {loadedInitialDataPacketsCallbackResults.data.dataPackets.screenTransition}", this);
                                        }
                                        else
                                            Log(loadedInitialDataPacketsCallbackResults.resultsCode, loadedInitialDataPacketsCallbackResults.results, this);
                                    });

                                    #endregion
                                }
                                else
                                    Log(onAppBootCallbackResults.resultsCode, onAppBootCallbackResults.results, this);
                            });
                        }
                        else
                            Log(screenManagerComponentCallbackResults.resultsCode, screenManagerComponentCallbackResults.results, this);
                    }, "Screen UI Manager Instance Is Not Yet Initialized.");
                }
                else
                    Log(sceneAssetsManagerCallbackResults.resultsCode, sceneAssetsManagerCallbackResults.results, this);
            
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
                    callbackResults.results = $"App Info Restriction Found.";
                    callbackResults.data = restriction;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Get App Restriction Failed : App Info Restriction Of Type : {restrictionType} Not Found / Not Yet Initialized.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "Get App Restriction Failed : App Info Restrictions Are Not Yet Initialized.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        #region Networking

        public async Task<bool> CheckConnectionStatus()
        {
            float timeOut = 3.0f;

            await Task.Delay(1000);

           while(Application.internetReachability == NetworkReachability.NotReachable || timeOut > 0.0f)
            {
                timeOut -= 1 * Time.deltaTime;;

                if (Application.internetReachability != NetworkReachability.NotReachable && timeOut > 0 || timeOut <= 0)
                    break;

                await Task.Yield();
            }

            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        #endregion

        #endregion
    }
}
