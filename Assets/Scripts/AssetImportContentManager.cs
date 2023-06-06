using System.Linq;
using System.IO;
using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.Android;

namespace Com.RedicalGames.Filar
{
    public class AssetImportContentManager : MonoBehaviour
    {
        #region Static

        private static AssetImportContentManager _instance;

        public static AssetImportContentManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<AssetImportContentManager>();

                return _instance;
            }
        }

        #endregion

        #region Components

        [SerializeField]
        AppData.AssetInfoDisplayer assetInfoDisplayer = new AppData.AssetInfoDisplayer();

        [Space(5)]
        [SerializeField]
        List<AppData.AssetPath> assetImportEditorPathList = new List<AppData.AssetPath>();

        [Space(5)]
        [SerializeField]
        AppData.SceneDataPackets userPermissionsDataPackets;

        [Space(5)]
        [SerializeField]
        string filePickerPluginBundleID;

        Vector3 importPosition = Vector3.zero;

        [SerializeField]
        AppData.SceneDataPackets currentDataPackets = new AppData.SceneDataPackets();

        [SerializeField]
        AppData.SceneAssetData currentSceneAssetData;

        AndroidJavaObject filePickerPluginInstance;

        [SerializeField]
        bool permissionsGranted;

        [SerializeField]
        bool showPermissionDialogue;

        [SerializeField]
        AppData.PermissionType requestedPermissionType;

        [SerializeField]
        AppData.SceneDataPackets permissionDialogDataPackets = new AppData.SceneDataPackets();

        #endregion

        #region Unity Callbacks

        void OnEnable() => ActionEventsSubscription(true);

        void OnDisable() => ActionEventsSubscription(false);

        void Start() => Init();

        #endregion

        #region Initializations

        void ActionEventsSubscription(bool subscribe)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnActionButtonClickedEvent += OnActionButtonClickedEvents;
            }
            else
            {
                AppData.ActionEvents._OnActionButtonClickedEvent -= OnActionButtonClickedEvents;
            }
        }

        void Init()
        {
            if (AppManager.Instance)
                filePickerPluginInstance = AppManager.Instance.GetInitializedPluginInstance(filePickerPluginBundleID);
            else
                Debug.LogWarning("--> App Manager Not Yet Initialized.");
        }

        void OnActionButtonClickedEvents(AppData.InputActionButtonType actionType)
        {
            switch (actionType)
            {
                case AppData.InputActionButtonType.OpenFilePicker_OBJ:

                    SelectOBJFile();

                    break;

                case AppData.InputActionButtonType.OpenFilePicker_Thumbnail:

                    SelectThumbnailFile();

                    break;

                case AppData.InputActionButtonType.OpenFilePicker_MainTexture:

                    SelectMainTextureFile();

                    break;

                case AppData.InputActionButtonType.OpenFilePicker_NormalMap:

                    SelectNormalMapFile();

                    break;

                case AppData.InputActionButtonType.OpenFilePicker_AOMap:

                    SelectAmbientOcclusionFile();

                    break;

                case AppData.InputActionButtonType.OpenFilePicker_Image:

                    SelectImageFile();

                    break;

                case AppData.InputActionButtonType.OpenFilePicker_HDRI:

                    SelectHDRIFile();

                    break;

                case AppData.InputActionButtonType.VoiceInputButton:

                    OnVoiceCommands();

                    break;
            }
        }

        #region Permissions

        public void UserRequestedAppPermissions(AppData.SceneDataPackets dataPackets)
        {
            userPermissionsDataPackets = dataPackets;

            Debug.Log($"RG_Unity : UserRequestedAppPermissions Called From Unity - Data Packet : {dataPackets.screenType}");

            if (Application.platform == RuntimePlatform.Android)
            {
                if (filePickerPluginInstance != null)
                {
                    filePickerPluginInstance.Call("RequestAppPermissions");
                }
                else
                    Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
            }
            else
                Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
        }

        public AndroidJavaObject GetReferencedPluginInstance()
        {
            return filePickerPluginInstance;
        }

        public void SetStoragePermissionGrantedState(string granted)
        {
            permissionsGranted = bool.Parse(granted);

            Debug.Log($"RG_Unity : Unity OnCreate SetPermissionGrantedState Called In Unity - State : {permissionsGranted}");

            AppData.ActionEvents.OnPermissionGrantedResults(permissionsGranted);

            if (permissionsGranted)
            {

                if (ScreenUIManager.Instance != null)
                    ScreenUIManager.Instance.ShowNewAssetScreen(userPermissionsDataPackets);
                else
                    Debug.LogWarning("--> Screen Manager Missing.");

                Debug.Log($"RG_Unity : SetPermissionGrantedState Called In Unity - Screen Type : {userPermissionsDataPackets.screenType}");

                if (SceneAssetsManager.Instance)
                    SceneAssetsManager.Instance.SetCurrentSceneMode(userPermissionsDataPackets.sceneMode);
                else
                    Debug.LogWarning("--> Scene Assets Not Yet Initialized.");

                Debug.Log($"RG_Unity : UserRequestedAppPermissions Called In Unity - Data Packet : {userPermissionsDataPackets.screenType}");
            }
        }

        public void SetAudioPermissionGrantedState(string granted)
        {
            permissionsGranted = bool.Parse(granted);

            Debug.Log($"RG_Unity : Unity OnCreate SetPermissionGrantedState Called In Unity - State : {permissionsGranted}");

            AppData.ActionEvents.OnPermissionGrantedResults(permissionsGranted);

            if (permissionsGranted)
            {

                if (ScreenUIManager.Instance != null)
                    ScreenUIManager.Instance.ShowNewAssetScreen(userPermissionsDataPackets);
                else
                    Debug.LogWarning("--> Screen Manager Missing.");

                Debug.Log($"RG_Unity : SetPermissionGrantedState Called In Unity - Screen Type : {userPermissionsDataPackets.screenType}");

                if (SceneAssetsManager.Instance)
                    SceneAssetsManager.Instance.SetCurrentSceneMode(userPermissionsDataPackets.sceneMode);
                else
                    Debug.LogWarning("--> Scene Assets Not Yet Initialized.");

                Debug.Log($"RG_Unity : UserRequestedAppPermissions Called In Unity - Data Packet : {userPermissionsDataPackets.screenType}");
            }
        }

        public bool IsStoragePermissionsGranted()
        {
            Debug.Log($"RG_Unity : Check Permission On Create Asset - SetPermissionGrantedState Called In Unity - State : {permissionsGranted}");

            return permissionsGranted;
        }

        public bool IsCameraPermissionsGranted()
        {
            return Permission.HasUserAuthorizedPermission(Permission.Camera);
        }

        public bool ShowPermissionDialogue()
        {
            return showPermissionDialogue;
        }

        public void SetRequestedPermissionData(AppData.SceneDataPackets dataPackets)
        {
            requestedPermissionType = dataPackets.requiredPermission;
            permissionDialogDataPackets = dataPackets;
        }

        public AppData.SceneDataPackets GetRequestedPermissionData()
        {
            return permissionDialogDataPackets;
        }

        #endregion

        #endregion

        #region Messages From Android

        #region OBJ Files

        public void OnSelectOBJFile(string path)
        {
            try
            {
                Debug.Log($"--> Unity - Selected OBJ From Path : {path}");

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    Debug.LogWarning($"--> Path : {path} Doesn't Exist.");
                    //return;
                }

                if (File.Exists(path))
                {

                    Debug.Log($"--> RG_Unity OBJ Selected From Path In Unity : {path}");

                    var mtlFilePath = path.Replace(".obj", ".mtl");

                    GenerateNewSceneAssetData(path, mtlFilePath, (assetData, results) =>
                    {
                        if (results)
                        {

                            if (SceneAssetsManager.Instance != null)
                            {
                                SceneAssetsManager.Instance.OnNewAssetDataCreated(assetData, (createdAsset, results) =>
                                {
                                    if (results)
                                    {
                                        createdAsset.currentAssetMode = AppData.SceneAssetModeType.CreateMode;
                                        SceneAssetsManager.Instance.UpdateCurrentSceneAsset(createdAsset);

                                    // Update Button Field Widgets.
                                    AppData.ActionEvents.OnActionButtonFieldUploadedEvent(AppData.InputActionButtonType.OpenFilePicker_OBJ, true, true);
                                    }
                                    else
                                        Debug.LogWarning("--> Asset Creation Failed.");

                                });
                            }
                            else
                                Debug.LogWarning("--> Assets Manager Missing.");
                        }
                        else
                            Debug.LogWarning("--> Failed to create a new asset data.");

                    });
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"--> Unity - Failed To Load OBJ File From Path : {path} - With Exception : {exception}");
            }

        }

        void GenerateNewSceneAssetData(string assetPath, string assetMTLPath, Action<AppData.SceneAssetData, bool> callBack)
        {
            try
            {

                List<AppData.AssetField> assetFieldList = new List<AppData.AssetField>();

                if (File.Exists(assetMTLPath))
                {
                    AppData.AssetField objectField = new AppData.AssetField();
                    AppData.AssetField mtlField = new AppData.AssetField();

                    objectField.name = "Asset Model Field";
                    objectField.path = assetPath;

                    mtlField.name = "Asset Model MTL Field";
                    mtlField.path = assetMTLPath;
                    mtlField.fieldType = AppData.AssetFieldType.MTLFile;
                    mtlField.extensionType = AppData.AssetFileExtensionType.MTL;
                    mtlField.directoryType = AppData.DirectoryType.Meta_File_Storage;

                    if (assetPath.Contains(".obj"))
                        objectField.extensionType = AppData.AssetFileExtensionType.OBJ;

                    switch (objectField.extensionType)
                    {
                        case AppData.AssetFileExtensionType.OBJ:

                            objectField.fieldType = AppData.AssetFieldType.OBJFile;

                            break;
                    }

                    objectField.directoryType = AppData.DirectoryType.Object_Asset_Storage;

                    if (!assetFieldList.Contains(objectField))
                        assetFieldList.Add(objectField);

                    if (!assetFieldList.Contains(mtlField))
                        assetFieldList.Add(mtlField);

                    AppData.SceneAssetData assetData = new AppData.SceneAssetData()
                    {
                        assetFields = assetFieldList,
                        hasMLTFile = true
                    };

                    if (!string.IsNullOrEmpty(assetPath))
                        callBack.Invoke(assetData, true);
                    else
                        callBack.Invoke(assetData, false);
                }
                else
                {
                    AppData.AssetField field = new AppData.AssetField();

                    field.name = "Asset Model Field";
                    field.path = assetPath;

                    if (assetPath.Contains(".obj"))
                        field.extensionType = AppData.AssetFileExtensionType.OBJ;

                    switch (field.extensionType)
                    {
                        case AppData.AssetFileExtensionType.OBJ:

                            field.fieldType = AppData.AssetFieldType.OBJFile;

                            break;
                    }

                    field.directoryType = AppData.DirectoryType.Object_Asset_Storage;

                    if (!assetFieldList.Contains(field))
                        assetFieldList.Add(field);

                    AppData.SceneAssetData assetData = new AppData.SceneAssetData()
                    {
                        assetFields = assetFieldList,
                        hasMLTFile = false
                    };

                    if (!string.IsNullOrEmpty(assetPath))
                        callBack.Invoke(assetData, true);
                    else
                        callBack.Invoke(assetData, false);
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"--> Unity - Failed To Generate New Asset Data From Asset Path : {assetPath} - With Exception : {exception}");
            }
        }

        #endregion

        #region Texture Files

        public void OnSelectThumbnailFile(string path)
        {
            try
            {

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    Debug.LogWarning($"--> Path : {path} Is Invalid.");
                    return;
                }

                if (SceneAssetsManager.Instance != null)
                {
                    var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                    AppData.AssetField field = new AppData.AssetField();

                    field.path = path;

                    field.fieldType = AppData.AssetFieldType.Thumbnail;

                    #region Extension Data

                    if (path.Contains(".png"))
                        field.extensionType = AppData.AssetFileExtensionType.PNG;

                    if (path.Contains(".jpg"))
                        field.extensionType = AppData.AssetFileExtensionType.JPG;

                    if (path.Contains(".jpeg"))
                        field.extensionType = AppData.AssetFileExtensionType.JPEG;

                    #endregion

                    field.name = "Thumbnail Field";
                    field.directoryType = AppData.DirectoryType.Image_Asset_Storage;

                    sceneAsset.AddAssetField(field);

                    SceneAssetsManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

                    // Update Button Field Widgets.
                    AppData.ActionEvents.OnActionButtonFieldUploadedEvent(AppData.InputActionButtonType.OpenFilePicker_Thumbnail, true, true);

                }
                else
                    Debug.LogWarning("--> Assets Manager Missing.");
            }
            catch (Exception exception)
            {
                throw new Exception($"--> Unity - Failed To Load Thumbnail File From Path : {path} - With Exception : {exception}");
            }
        }

        public void OnSelectMainTextureFile(string path)
        {
            try
            {

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    Debug.LogWarning($"--> Path : {path} Is Invalid.");
                    return;
                }

                if (SceneAssetsManager.Instance != null)
                {
                    var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                    AppData.AssetField field = new AppData.AssetField();

                    field.path = path;

                    field.fieldType = AppData.AssetFieldType.MainTexture;

                    #region Extension Data

                    if (path.Contains(".png"))
                        field.extensionType = AppData.AssetFileExtensionType.PNG;

                    if (path.Contains(".jpg"))
                        field.extensionType = AppData.AssetFileExtensionType.JPG;

                    if (path.Contains(".jpeg"))
                        field.extensionType = AppData.AssetFileExtensionType.JPEG;

                    #endregion

                    field.name = "Main Texture Field";
                    field.directoryType = AppData.DirectoryType.Image_Asset_Storage;

                    sceneAsset.AddAssetField(field);

                    if (RenderingSettingsManager.Instance)
                    {
                        if (RenderingSettingsManager.Instance.IsAssetRendererReady())
                        {
                            RenderingSettingsManager.Instance.SetTexture(RenderingSettingsManager.Instance.GetMaterialTextureID(AppData.MaterialTextureType.MainTexture), path, true);
                        }
                        else
                            Debug.LogWarning("--> Rendering Manager Asset Renderer Not Ready.");

                    }
                    else
                        Debug.LogWarning("--> Rendering Manager Not Yet Initialized.");

                    SceneAssetsManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

                    // Update Button Field Widgets.
                    AppData.ActionEvents.OnActionButtonFieldUploadedEvent(AppData.InputActionButtonType.OpenFilePicker_MainTexture, true, true);

                }
                else
                    Debug.LogWarning("--> Assets Manager Missing.");
            }
            catch (Exception exception)
            {
                throw new Exception($"--> Unity - Failed To Load Main Texture From Path : {path} - With Exception : {exception}");
            }
        }

        public void OnSelectNormalMapFile(string path)
        {
            try
            {

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    Debug.LogWarning($"--> Path : {path} Is Invalid.");
                    return;
                }

                if (SceneAssetsManager.Instance != null)
                {
                    var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                    AppData.AssetField field = new AppData.AssetField();

                    field.path = path;

                    field.fieldType = AppData.AssetFieldType.NormalMap;

                    #region Extension Data

                    if (path.Contains(".png"))
                        field.extensionType = AppData.AssetFileExtensionType.PNG;

                    if (path.Contains(".jpg"))
                        field.extensionType = AppData.AssetFileExtensionType.JPG;

                    if (path.Contains(".jpeg"))
                        field.extensionType = AppData.AssetFileExtensionType.JPEG;

                    #endregion

                    field.name = "Normal Map Field";
                    field.directoryType = AppData.DirectoryType.Image_Asset_Storage;

                    sceneAsset.AddAssetField(field);

                    if (RenderingSettingsManager.Instance)
                    {
                        if (RenderingSettingsManager.Instance.IsAssetRendererReady())
                        {
                            RenderingSettingsManager.Instance.SetTexture(RenderingSettingsManager.Instance.GetMaterialTextureID(AppData.MaterialTextureType.NormalMapTexture), path, true);
                        }
                        else
                            Debug.LogWarning("--> Rendering Manager Asset Renderer Not Ready.");

                    }
                    else
                        Debug.LogWarning("--> Rendering Manager Not Yet Initialized.");

                    SceneAssetsManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

                    // Update Button Field Widgets.
                    AppData.ActionEvents.OnActionButtonFieldUploadedEvent(AppData.InputActionButtonType.OpenFilePicker_NormalMap, true, true);
                }
                else
                    Debug.LogWarning("--> Assets Manager Missing.");
            }
            catch (Exception exception)
            {
                throw new Exception($"--> Unity - Failed To Load Normal Map Texture From Path : {path} - With Exception : {exception}");
            }
        }

        public void OnSelectAmbientOcclusionFile(string path)
        {
            try
            {

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    Debug.LogWarning($"--> Path : {path} Is Invalid.");
                    return;
                }

                if (SceneAssetsManager.Instance != null)
                {
                    var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                    AppData.AssetField field = new AppData.AssetField();

                    field.path = path;

                    field.fieldType = AppData.AssetFieldType.AmbientOcclusionMap;

                    #region Extension Data

                    if (path.Contains(".png"))
                        field.extensionType = AppData.AssetFileExtensionType.PNG;

                    if (path.Contains(".jpg"))
                        field.extensionType = AppData.AssetFileExtensionType.JPG;

                    if (path.Contains(".jpeg"))
                        field.extensionType = AppData.AssetFileExtensionType.JPEG;

                    #endregion

                    field.name = "AO Map Field";
                    field.directoryType = AppData.DirectoryType.Image_Asset_Storage;

                    sceneAsset.AddAssetField(field);

                    if (RenderingSettingsManager.Instance)
                    {
                        if (RenderingSettingsManager.Instance.IsAssetRendererReady())
                        {
                            RenderingSettingsManager.Instance.SetTexture(RenderingSettingsManager.Instance.GetMaterialTextureID(AppData.MaterialTextureType.AOMapTexture), path, true);
                        }
                        else
                            Debug.LogWarning("--> Rendering Manager Asset Renderer Not Ready.");

                    }
                    else
                        Debug.LogWarning("--> Rendering Manager Not Yet Initialized.");

                    SceneAssetsManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

                    // Update Button Field Widgets.
                    AppData.ActionEvents.OnActionButtonFieldUploadedEvent(AppData.InputActionButtonType.OpenFilePicker_AOMap, true, true);
                }
                else
                    Debug.LogWarning("--> Assets Manager Missing.");
            }
            catch (Exception exception)
            {
                throw new Exception($"--> Unity - Failed To Load AO Map Texture From Path : {path} - With Exception : {exception}");
            }
        }

        public void OnSelectImageFile(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    Debug.LogWarning($"--> Path : {path} Is Invalid.");
                    return;
                }

                AppData.StorageDirectoryData directoryData = new AppData.StorageDirectoryData
                {
                    name = "Image Field",
                    directory = path,
                    type = AppData.DirectoryType.Image_Asset_Storage
                };

                AppData.ActionEvents.OnFilePickerDirectoryFieldSelectedEvent(AppData.AssetFieldType.Image, directoryData);
            }
            catch (Exception exception)
            {
                throw new Exception($"--> Unity - Failed To Load Image File From Path : {path} - With Exception : {exception}");
            }
        }

        public void OnSelectHDRIFile(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    Debug.LogWarning($"--> Path : {path} Is Invalid.");
                    return;
                }

                AppData.StorageDirectoryData directoryData = new AppData.StorageDirectoryData
                {
                    name = "HDRI Field",
                    directory = path,
                    type = AppData.DirectoryType.Image_Asset_Storage
                };

                AppData.ActionEvents.OnFilePickerDirectoryFieldSelectedEvent(AppData.AssetFieldType.HDRI, directoryData);
            }
            catch (Exception exception)
            {
                throw new Exception($"--> Unity - Failed To Load HDRI File From Path : {path} - With Exception : {exception}");
            }
        }

        #endregion'

        #region Asset Data Setup


        public void SetCurrentDataPacket(AppData.SceneDataPackets dataPackets)
        {
            currentDataPackets = dataPackets;
        }

        public AppData.SceneDataPackets GetCurrentDataPacket()
        {
            return currentDataPackets;
        }

        public AppData.SceneAsset GetCurrentSceneAsset()
        {

            if (currentSceneAssetData.ToSceneAsset().modelAsset == null)
                Debug.Log("--> Model Asset Missing");

            return currentSceneAssetData.ToSceneAsset();
        }

        void DisplayAssetInfo(AppData.AssetInfoHandler info)
        {
            assetInfoDisplayer.SetAssetInfo(info);
        }

        #endregion

        #region Speech Recognition

        public void VoiceCommandReciever(string command)
        {
            string[] commands = command.Split();

            if (commands.Length > 0)
                AppData.ActionEvents.OnVoiceCommandResultsEvent(commands);
            else
                Debug.LogWarning("--> RG_Unity - VoiceCommandReciever Failed : Commands Are Null Or Empty.");
        }

        public void OnReadyForSpeech()
        {
            Debug.LogError("==> RG_Unity - Ready For Speech ");
        }

        public void OnBeginningOfSpeech()
        {
            Debug.LogError("==> RG_Unity - Beginning Of Speech ");
        }

        #endregion

        #endregion

        #region Messages To Android Studio

        public void SelectOBJFile()
        {
            try
            {

#if UNITY_EDITOR

                if (assetImportEditorPathList != null)
                {
                    if (assetImportEditorPathList.Count > 0)
                    {
                        foreach (var assetPath in assetImportEditorPathList)
                        {
                            if (!string.IsNullOrEmpty(assetPath.path))
                            {
                                if (assetPath.fieldType == AppData.AssetFieldType.OBJFile && assetPath.active)
                                {
                                    OnSelectOBJFile(assetPath.path);
                                    break;
                                }
                                else
                                    continue;
                            }
                            else
                                Debug.LogWarning($"--> Path For : {assetPath.name} Is Empty / Null..");
                        }
                    }
                    else
                        Debug.LogWarning("--> Editor Asset Path List Is Empty.");
                }
                else
                    Debug.LogWarning("--> Editor Asset Path List Is Null.");

#else

            if (Application.platform == RuntimePlatform.Android)
            {
                       if (filePickerPluginInstance != null)
                        {
                            filePickerPluginInstance.Call("SelectOBJAssetFile");
                        }
                        else
                            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");

            }
            else
                Debug.LogWarning("--> Current Platform Is Not Runtime Android.");

#endif
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SelectThumbnailFile()
        {
            try
            {
                if (SceneAssetsManager.Instance != null)
                {
#if UNITY_EDITOR

                    if (assetImportEditorPathList != null)
                    {
                        if (assetImportEditorPathList.Count > 0)
                        {
                            foreach (var assetPath in assetImportEditorPathList)
                            {
                                if (!string.IsNullOrEmpty(assetPath.path))
                                {
                                    if (assetPath.fieldType == AppData.AssetFieldType.Thumbnail && assetPath.active)
                                    {
                                        OnSelectThumbnailFile(assetPath.path);
                                        break;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    Debug.LogWarning($"--> Path For : {assetPath.name} Is Empty / Null..");
                            }
                        }
                        else
                            Debug.LogWarning("--> Editor Asset Path List Is Empty.");
                    }
                    else
                        Debug.LogWarning("--> Editor Asset Path List Is Null.");

#else

                var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                if (sceneAsset.modelAsset != null)
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        if (filePickerPluginInstance != null)
                        {
                            filePickerPluginInstance.Call("SelectThumbnailAssetFile");
                        }
                        else
                            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                    }
                    else
                        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                }
                else
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                    else
                        Debug.LogWarning("--> Screen UI Manager Not Found.");
                }

#endif
                }
                else
                    Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SelectMainTextureFile()
        {
            try
            {
                if (SceneAssetsManager.Instance != null)
                {
#if UNITY_EDITOR

                    if (assetImportEditorPathList != null)
                    {
                        if (assetImportEditorPathList.Count > 0)
                        {
                            foreach (var assetPath in assetImportEditorPathList)
                            {
                                if (!string.IsNullOrEmpty(assetPath.path))
                                {
                                    if (assetPath.fieldType == AppData.AssetFieldType.MainTexture && assetPath.active)
                                    {
                                        OnSelectMainTextureFile(assetPath.path);
                                        break;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    Debug.LogWarning($"--> Path For : {assetPath.name} Is Empty / Null..");
                            }
                        }
                        else
                            Debug.LogWarning("--> Editor Asset Path List Is Empty.");
                    }
                    else
                        Debug.LogWarning("--> Editor Asset Path List Is Null.");

#else

                var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                if (sceneAsset.modelAsset != null)
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        if (filePickerPluginInstance != null)
                        {
                            filePickerPluginInstance.Call("SelectMainTextureAssetFile");
                        }
                        else
                            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                    }
                    else
                        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                }
                else
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                    else
                        Debug.LogWarning("--> Screen UI Manager Not Found.");
                }

#endif
                }
                else
                    Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SelectNormalMapFile()
        {
            try
            {
                if (SceneAssetsManager.Instance != null)
                {

#if UNITY_EDITOR

                    if (assetImportEditorPathList != null)
                    {
                        if (assetImportEditorPathList.Count > 0)
                        {
                            foreach (var assetPath in assetImportEditorPathList)
                            {
                                if (!string.IsNullOrEmpty(assetPath.path))
                                {
                                    if (assetPath.fieldType == AppData.AssetFieldType.NormalMap && assetPath.active)
                                    {
                                        OnSelectNormalMapFile(assetPath.path);
                                        break;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    Debug.LogWarning($"--> Path For : {assetPath.name} Is Empty / Null..");
                            }
                        }
                        else
                            Debug.LogWarning("--> Editor Asset Path List Is Empty.");
                    }
                    else
                        Debug.LogWarning("--> Editor Asset Path List Is Null.");

#else

                var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                if (sceneAsset.modelAsset != null)
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        if (filePickerPluginInstance != null)
                        {
                            filePickerPluginInstance.Call("SelectNormalMapAssetFile");
                        }
                        else
                            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                    }
                    else
                        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                }
                else
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                    else
                        Debug.LogWarning("--> Screen UI Manager Not Found.");
                }

#endif
                }
                else
                    Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SelectAmbientOcclusionFile()
        {
            try
            {
                if (SceneAssetsManager.Instance != null)
                {

#if UNITY_EDITOR

                    if (assetImportEditorPathList != null)
                    {
                        if (assetImportEditorPathList.Count > 0)
                        {
                            foreach (var assetPath in assetImportEditorPathList)
                            {
                                if (!string.IsNullOrEmpty(assetPath.path))
                                {
                                    if (assetPath.fieldType == AppData.AssetFieldType.AmbientOcclusionMap && assetPath.active)
                                    {
                                        OnSelectAmbientOcclusionFile(assetPath.path);
                                        break;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    Debug.LogWarning($"--> Path For : {assetPath.name} Is Empty / Null..");
                            }
                        }
                        else
                            Debug.LogWarning("--> Editor Asset Path List Is Empty.");
                    }
                    else
                        Debug.LogWarning("--> Editor Asset Path List Is Null.");

#else

                var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                if (sceneAsset.modelAsset != null)
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        if (filePickerPluginInstance != null)
                        {
                            filePickerPluginInstance.Call("SelectAOMapAssetFile");
                        }
                        else
                            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                    }
                    else
                        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                }
                else
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                    else
                        Debug.LogWarning("--> Screen UI Manager Not Found.");
                }

#endif
                }
                else
                    Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SelectImageFile()
        {
            try
            {
                if (SceneAssetsManager.Instance != null)
                {
#if UNITY_EDITOR

                    if (assetImportEditorPathList != null)
                    {
                        if (assetImportEditorPathList.Count > 0)
                        {
                            foreach (var assetPath in assetImportEditorPathList)
                            {
                                if (!string.IsNullOrEmpty(assetPath.path))
                                {
                                    if (assetPath.fieldType == AppData.AssetFieldType.Image && assetPath.active)
                                    {
                                        OnSelectImageFile(assetPath.path);
                                        break;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    Debug.LogWarning($"--> Path For : {assetPath.name} Is Empty / Null..");
                            }
                        }
                        else
                            Debug.LogWarning("--> Editor Asset Path List Is Empty.");
                    }
                    else
                        Debug.LogWarning("--> Editor Asset Path List Is Null.");

#else

                var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                if (sceneAsset.modelAsset != null)
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        if (filePickerPluginInstance != null)
                        {
                            filePickerPluginInstance.Call("SelectImageAssetFile");
                        }
                        else
                            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                    }
                    else
                        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                }
                else
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                    else
                        Debug.LogWarning("--> Screen UI Manager Not Found.");
                }

#endif
                }
                else
                    Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SelectHDRIFile()
        {
            try
            {
                if (SceneAssetsManager.Instance != null)
                {
#if UNITY_EDITOR

                    if (assetImportEditorPathList != null)
                    {
                        if (assetImportEditorPathList.Count > 0)
                        {
                            foreach (var assetPath in assetImportEditorPathList)
                            {
                                if (!string.IsNullOrEmpty(assetPath.path))
                                {
                                    if (assetPath.fieldType == AppData.AssetFieldType.HDRI && assetPath.active)
                                    {
                                        OnSelectHDRIFile(assetPath.path);
                                        break;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    Debug.LogWarning($"--> Path For : {assetPath.name} Is Empty / Null..");
                            }
                        }
                        else
                            Debug.LogWarning("--> Editor Asset Path List Is Empty.");
                    }
                    else
                        Debug.LogWarning("--> Editor Asset Path List Is Null.");

#else

                var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                if (sceneAsset.modelAsset != null)
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        if (filePickerPluginInstance != null)
                        {
                            filePickerPluginInstance.Call("SelectImageAssetFile");
                        }
                        else
                            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                    }
                    else
                        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                }
                else
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                    else
                        Debug.LogWarning("--> Screen UI Manager Not Found.");
                }

#endif
                }
                else
                    Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void OnVoiceCommands()
        {

            if (Application.platform == RuntimePlatform.Android)
            {
                if (filePickerPluginInstance != null)
                    filePickerPluginInstance.Call("StartVoiceCommands");
                else
                    Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
            }
            else
                Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
        }

        public void ExportAsset(AppData.AssetExportData exportData)
        {
            Debug.LogError($"----------------> Exporting Scene Asset : {exportData.name}");
        }

        #endregion
    }
}