using System.Collections;
using System.Collections.Generic;
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

            Debug.Log("--> Back Button Pressed.");

            //if (SceneAssetsManager.Instance != null)
            //    SceneAssetsManager.Instance.SetCurrentSceneAsset(SceneAssetsManager.Instance.GetSceneAssets()[0]);

            SceneAssetsManager.Instance.GetDynamicWidgetsContainer(SceneAssetsManager.Instance.GetContainerType(initialLoadDataPackets.screenType), containerResults =>
            {
                if (containerResults.Success())
                {
                    var rootFolder = SceneAssetsManager.Instance.GetFolderStructureData().rootFolder;
                    var container = containerResults.data;

                    SceneAssetsManager.Instance.SetWidgetsRefreshData(rootFolder, container);
                }
            });



            //StartCoroutine(OnLoadScreen());
        }

        void OnSubscribeToActionEvents(bool subscribe)
        {
            if (subscribe)
                AppData.ActionEvents._OnAppScreensInitializedEvent += ActionEvents__OnAppScreensInitializedEvent;
            else
                AppData.ActionEvents._OnAppScreensInitializedEvent -= ActionEvents__OnAppScreensInitializedEvent;
        }

        private void ActionEvents__OnAppScreensInitializedEvent()
        {
            if (SceneAssetsManager.Instance != null && SceneAssetsManager.Instance.GetSceneAssets().Count > 0)
                SceneAssetsManager.Instance.SetCurrentSceneAsset(SceneAssetsManager.Instance.GetSceneAssets()[0]);

            if (ScreenUIManager.Instance != null)
                ScreenUIManager.Instance.ShowScreen(initialLoadDataPackets);
            else
                Debug.LogWarning("--> Screen Manager Missing.");
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
            if (SceneAssetsManager.Instance != null)
                SceneAssetsManager.Instance.SetCurrentSceneAsset(SceneAssetsManager.Instance.GetSceneAssets()[0]);

            if (ScreenUIManager.Instance != null)
                ScreenUIManager.Instance.ShowScreen(initialLoadDataPackets);
            else
                Debug.LogWarning("--> Screen Manager Missing.");

        }

        IEnumerator OnLoadScreen()
        {
            yield return new WaitForEndOfFrame();

            LoadInitialScreen();
        }

        public bool IsRuntime()
        {
            return Application.platform == RuntimePlatform.Android;
        }

        #endregion
    }
}
