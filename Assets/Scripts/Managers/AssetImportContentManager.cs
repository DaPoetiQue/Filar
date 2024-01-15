using System.Linq;
using System.IO;
using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.Android;

namespace Com.RedicalGames.Filar
{
    public class AssetImportContentManager : AppData.SingletonBaseComponent<AssetImportContentManager>
    {
        #region Components

        [SerializeField]
        AppData.AssetInfoDisplayer assetInfoDisplayer = new AppData.AssetInfoDisplayer();

        [Space(5)]
        [SerializeField]
        List<AppData.AssetPath> assetImportEditorPathList = new List<AppData.AssetPath>();

        [Space(5)]
        [SerializeField]
        AppData.SceneConfigDataPacket userPermissionsDataPackets;

        [Space(5)]
        [SerializeField]
        string filePickerPluginBundleID;

        Vector3 importPosition = Vector3.zero;

        [SerializeField]
        AppData.SceneConfigDataPacket currentDataPackets = new AppData.SceneConfigDataPacket();

        [SerializeField]
        AppData.AssetData currentSceneAssetData;

        AndroidJavaObject filePickerPluginInstance;

        [SerializeField]
        bool permissionsGranted;

        [SerializeField]
        bool showPermissionDialogue;

        [SerializeField]
        AppData.PermissionType requestedPermissionType;

        [SerializeField]
        AppData.SceneConfigDataPacket permissionDialogDataPackets = new AppData.SceneConfigDataPacket();

        #endregion

        #region Unity Callbacks

        void OnEnable() => ActionEventsSubscription(true);

        void OnDisable() => ActionEventsSubscription(false);

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

        protected override void Init()
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

        public void UserRequestedAppPermissions(AppData.SceneConfigDataPacket dataPackets)
        {
            userPermissionsDataPackets = dataPackets;

            Debug.Log($"RG_Unity : UserRequestedAppPermissions Called From Unity - Data Packet : {dataPackets.referencedScreenType}");

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

                Debug.Log($"RG_Unity : SetPermissionGrantedState Called In Unity - Screen Type : {userPermissionsDataPackets.referencedScreenType}");

                if (AppDatabaseManager.Instance)
                    AppDatabaseManager.Instance.SetCurrentSceneMode(userPermissionsDataPackets.sceneMode);
                else
                    Debug.LogWarning("--> Scene Assets Not Yet Initialized.");

                Debug.Log($"RG_Unity : UserRequestedAppPermissions Called In Unity - Data Packet : {userPermissionsDataPackets.referencedScreenType}");
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

                Debug.Log($"RG_Unity : SetPermissionGrantedState Called In Unity - Screen Type : {userPermissionsDataPackets.referencedScreenType}");

                if (AppDatabaseManager.Instance)
                    AppDatabaseManager.Instance.SetCurrentSceneMode(userPermissionsDataPackets.sceneMode);
                else
                    Debug.LogWarning("--> Scene Assets Not Yet Initialized.");

                Debug.Log($"RG_Unity : UserRequestedAppPermissions Called In Unity - Data Packet : {userPermissionsDataPackets.referencedScreenType}");
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

        public void SetRequestedPermissionData(AppData.SceneConfigDataPacket dataPackets)
        {
            requestedPermissionType = dataPackets.requiredPermission;
            permissionDialogDataPackets = dataPackets;
        }

        public AppData.SceneConfigDataPacket GetRequestedPermissionData()
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

                            if (AppDatabaseManager.Instance != null)
                            {
                                AppDatabaseManager.Instance.OnNewAssetDataCreated(assetData, (createdAsset, results) =>
                                {
                                    if (results)
                                    {
                                        createdAsset.assetMode = AppData.AssetModeType.CreateMode;
                                        AppDatabaseManager.Instance.UpdateCurrentSceneAsset(createdAsset);

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

        void GenerateNewSceneAssetData(string assetPath, string assetMTLPath, Action<AppData.AssetData, bool> callBack)
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
                    mtlField.extensionType = AppData.FileExtensionType.MTL;
                    mtlField.directoryType = AppData.StorageType.Meta_File_Storage;

                    if (assetPath.Contains(".obj"))
                        objectField.extensionType = AppData.FileExtensionType.OBJ;

                    switch (objectField.extensionType)
                    {
                        case AppData.FileExtensionType.OBJ:

                            objectField.fieldType = AppData.AssetFieldType.OBJFile;

                            break;
                    }

                    objectField.directoryType = AppData.StorageType.Object_Asset_Storage;

                    if (!assetFieldList.Contains(objectField))
                        assetFieldList.Add(objectField);

                    if (!assetFieldList.Contains(mtlField))
                        assetFieldList.Add(mtlField);

                    AppData.AssetData assetData = new AppData.AssetData()
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
                        field.extensionType = AppData.FileExtensionType.OBJ;

                    switch (field.extensionType)
                    {
                        case AppData.FileExtensionType.OBJ:

                            field.fieldType = AppData.AssetFieldType.OBJFile;

                            break;
                    }

                    field.directoryType = AppData.StorageType.Object_Asset_Storage;

                    if (!assetFieldList.Contains(field))
                        assetFieldList.Add(field);

                    AppData.AssetData assetData = new AppData.AssetData()
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

                if (AppDatabaseManager.Instance != null)
                {
                    var sceneAsset = AppDatabaseManager.Instance.GetCurrentSceneAsset();

                    AppData.AssetField field = new AppData.AssetField();

                    field.path = path;

                    field.fieldType = AppData.AssetFieldType.Thumbnail;

                    #region Extension Data

                    if (path.Contains(".png"))
                        field.extensionType = AppData.FileExtensionType.PNG;

                    if (path.Contains(".jpg"))
                        field.extensionType = AppData.FileExtensionType.JPG;

                    if (path.Contains(".jpeg"))
                        field.extensionType = AppData.FileExtensionType.JPEG;

                    #endregion

                    field.name = "Thumbnail Field";
                    field.directoryType = AppData.StorageType.Image_Asset_Storage;

                    sceneAsset.AddAssetField(field);

                    AppDatabaseManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

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

                if (AppDatabaseManager.Instance != null)
                {
                    var sceneAsset = AppDatabaseManager.Instance.GetCurrentSceneAsset();

                    AppData.AssetField field = new AppData.AssetField();

                    field.path = path;

                    field.fieldType = AppData.AssetFieldType.MainTexture;

                    #region Extension Data

                    if (path.Contains(".png"))
                        field.extensionType = AppData.FileExtensionType.PNG;

                    if (path.Contains(".jpg"))
                        field.extensionType = AppData.FileExtensionType.JPG;

                    if (path.Contains(".jpeg"))
                        field.extensionType = AppData.FileExtensionType.JPEG;

                    #endregion

                    field.name = "Main Texture Field";
                    field.directoryType = AppData.StorageType.Image_Asset_Storage;

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

                    AppDatabaseManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

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

                if (AppDatabaseManager.Instance != null)
                {
                    var sceneAsset = AppDatabaseManager.Instance.GetCurrentSceneAsset();

                    AppData.AssetField field = new AppData.AssetField();

                    field.path = path;

                    field.fieldType = AppData.AssetFieldType.NormalMap;

                    #region Extension Data

                    if (path.Contains(".png"))
                        field.extensionType = AppData.FileExtensionType.PNG;

                    if (path.Contains(".jpg"))
                        field.extensionType = AppData.FileExtensionType.JPG;

                    if (path.Contains(".jpeg"))
                        field.extensionType = AppData.FileExtensionType.JPEG;

                    #endregion

                    field.name = "Normal Map Field";
                    field.directoryType = AppData.StorageType.Image_Asset_Storage;

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

                    AppDatabaseManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

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

                if (AppDatabaseManager.Instance != null)
                {
                    var sceneAsset = AppDatabaseManager.Instance.GetCurrentSceneAsset();

                    AppData.AssetField field = new AppData.AssetField();

                    field.path = path;

                    field.fieldType = AppData.AssetFieldType.AmbientOcclusionMap;

                    #region Extension Data

                    if (path.Contains(".png"))
                        field.extensionType = AppData.FileExtensionType.PNG;

                    if (path.Contains(".jpg"))
                        field.extensionType = AppData.FileExtensionType.JPG;

                    if (path.Contains(".jpeg"))
                        field.extensionType = AppData.FileExtensionType.JPEG;

                    #endregion

                    field.name = "AO Map Field";
                    field.directoryType = AppData.StorageType.Image_Asset_Storage;

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

                    AppDatabaseManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

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
                    projectDirectory = path,
                    type = AppData.StorageType.Image_Asset_Storage
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
                    projectDirectory = path,
                    type = AppData.StorageType.Image_Asset_Storage
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


        public void SetCurrentDataPacket(AppData.SceneConfigDataPacket dataPackets)
        {
            currentDataPackets = dataPackets;
        }

        public AppData.SceneConfigDataPacket GetCurrentDataPacket()
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
                if (AppDatabaseManager.Instance != null)
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

                //var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                //if (sceneAsset.modelAsset != null)
                //{
                //    if (Application.platform == RuntimePlatform.Android)
                //    {
                //        if (filePickerPluginInstance != null)
                //        {
                //            filePickerPluginInstance.Call("SelectThumbnailAssetFile");
                //        }
                //        else
                //            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                //    }
                //    else
                //        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                //}
                //else
                //{
                //    if (ScreenUIManager.Instance != null)
                //        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                //    else
                //        Debug.LogWarning("--> Screen UI Manager Not Found.");
                //}

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
                if (AppDatabaseManager.Instance != null)
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

                //var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                //if (sceneAsset.modelAsset != null)
                //{
                //    if (Application.platform == RuntimePlatform.Android)
                //    {
                //        if (filePickerPluginInstance != null)
                //        {
                //            filePickerPluginInstance.Call("SelectMainTextureAssetFile");
                //        }
                //        else
                //            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                //    }
                //    else
                //        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                //}
                //else
                //{
                //    if (ScreenUIManager.Instance != null)
                //        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                //    else
                //        Debug.LogWarning("--> Screen UI Manager Not Found.");
                //}

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
                if (AppDatabaseManager.Instance != null)
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

                //var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                //if (sceneAsset.modelAsset != null)
                //{
                //    if (Application.platform == RuntimePlatform.Android)
                //    {
                //        if (filePickerPluginInstance != null)
                //        {
                //            filePickerPluginInstance.Call("SelectNormalMapAssetFile");
                //        }
                //        else
                //            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                //    }
                //    else
                //        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                //}
                //else
                //{
                //    if (ScreenUIManager.Instance != null)
                //        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                //    else
                //        Debug.LogWarning("--> Screen UI Manager Not Found.");
                //}

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
                if (AppDatabaseManager.Instance != null)
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

                //var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                //if (sceneAsset.modelAsset != null)
                //{
                //    if (Application.platform == RuntimePlatform.Android)
                //    {
                //        if (filePickerPluginInstance != null)
                //        {
                //            filePickerPluginInstance.Call("SelectAOMapAssetFile");
                //        }
                //        else
                //            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                //    }
                //    else
                //        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                //}
                //else
                //{
                //    if (ScreenUIManager.Instance != null)
                //        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                //    else
                //        Debug.LogWarning("--> Screen UI Manager Not Found.");
                //}

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
                if (AppDatabaseManager.Instance != null)
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

                //var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                //if (sceneAsset.modelAsset != null)
                //{
                //    if (Application.platform == RuntimePlatform.Android)
                //    {
                //        if (filePickerPluginInstance != null)
                //        {
                //            filePickerPluginInstance.Call("SelectImageAssetFile");
                //        }
                //        else
                //            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                //    }
                //    else
                //        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                //}
                //else
                //{
                //    if (ScreenUIManager.Instance != null)
                //        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                //    else
                //        Debug.LogWarning("--> Screen UI Manager Not Found.");
                //}

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
                if (AppDatabaseManager.Instance != null)
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

                //var sceneAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();

                //if (sceneAsset.modelAsset != null)
                //{
                //    if (Application.platform == RuntimePlatform.Android)
                //    {
                //        if (filePickerPluginInstance != null)
                //        {
                //            filePickerPluginInstance.Call("SelectImageAssetFile");
                //        }
                //        else
                //            Debug.LogWarning("--> Android File Picker Plugin Not Properly Initialized.");
                //    }
                //    else
                //        Debug.LogWarning("--> Current Platform Is Not Runtime Android.");
                //}
                //else
                //{
                //    if (ScreenUIManager.Instance != null)
                //        ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(currentDataPackets);
                //    else
                //        Debug.LogWarning("--> Screen UI Manager Not Found.");
                //}

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