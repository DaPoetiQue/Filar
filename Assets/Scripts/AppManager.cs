using System;
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

        [SerializeField]
        AppData.AppProjectSupportType testProjectSupport;

        [Space(5)]
        [SerializeField]
        AppData.SceneDataPackets initialLoadDataPackets = new AppData.SceneDataPackets();

        [Space(5)]
        [SerializeField]
        bool requestStoragePermissions;

        #endregion

        #region Unity Callbacks

        void Awake() => SetupInstance();

        void OnEnable() => OnSubscribeToActionEvents(true);
        void OnDisable() => OnSubscribeToActionEvents(false);

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
                    AppData.Helpers.ComponentValid(SceneAssetsManager.Instance, hasComponentCallbackResults =>
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

                                            SceneAssetsManager.Instance.SetCurrentProjectStructureData(rootProjectStructure);

                                            SceneAssetsManager.Instance.GetDynamicWidgetsContainer(SceneAssetsManager.Instance.GetContainerType(initialLoadDataPackets.screenType), containerResults =>
                                            {
                                                if (containerResults.Success())
                                                {
                                                    var rootFolder = rootProjectStructure.rootFolder;
                                                    var container = containerResults.data;

                                                    SceneAssetsManager.Instance.SetWidgetsRefreshData(rootFolder, container);
                                                }
                                                else
                                                    Log(containerResults.resultsCode, containerResults.results, this);
                                            });
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

        void OnSubscribeToActionEvents(bool subscribe)
        {
            if (subscribe)
                AppData.ActionEvents._OnAppScreensInitializedEvent += ActionEvents__OnAppScreensInitializedEvent;
            else
                AppData.ActionEvents._OnAppScreensInitializedEvent -= ActionEvents__OnAppScreensInitializedEvent;
        }

        private void ActionEvents__OnAppScreensInitializedEvent() => LoadInitialScreen();

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

        public AppData.SceneDataPackets GetInitialLoadDataPackets()
        {
            return initialLoadDataPackets;
        }

        public void SetCurrentScreenType(AppData.UIScreenType screenType)
        {
            initialLoadDataPackets.screenType = screenType;
        }

        public AppData.UIScreenType GetCurrentScreenType()
        {
            return initialLoadDataPackets.screenType;
        }

        void LoadInitialScreen()
        {
            AppData.Helpers.ComponentValid(ScreenUIManager.Instance, validComponentCallbackResults => 
            {
                if (validComponentCallbackResults.Success())
                    ScreenUIManager.Instance.ShowScreen(initialLoadDataPackets);
                else
                    Log(validComponentCallbackResults.resultsCode, "Screen UI Manager Is Not Yet Initialized.", this);
            });
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

        #endregion
    }
}
