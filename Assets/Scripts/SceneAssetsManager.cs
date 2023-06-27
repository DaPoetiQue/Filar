using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;

namespace Com.RedicalGames.Filar
{
    public class SceneAssetsManager : AppMonoBaseClass
    {
        #region Static

        static SceneAssetsManager _instance;

        public static SceneAssetsManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<SceneAssetsManager>();

                return _instance;
            }
        }

        #endregion

        #region Components

        [Space(5)]
        [SerializeField]
        string defaultAppDirectoryFolderName;

        [Space(5)]
        [SerializeField]
        string defaultAssetName = "New Scene Asset";

        [Space(5)]
        [SerializeField]
        Sprite defaultFallbackSceneAssetIcon;

        [Space(5)]
        [SerializeField]
        List<AppData.StorageDirectoryData> defaultDirectories = new List<AppData.StorageDirectoryData>();

        [Space(5)]
        [SerializeField]
        AppData.FolderStructureData folderStructureData;

        [Space(5)]
        [SerializeField]
        string folderStructureSearchFieldPlaceHolderTextValue = "Search";

        //[SerializeField]
        //AppData.Folder mainFolder = new AppData.Folder();

        [Space(5)]
        [SerializeField]
        bool strictValidateAssetSearch = false;

        [Space(5)]
        [SerializeField]
        List<AppData.ScreenText> screenTextList = new List<AppData.ScreenText>();

        [Space(5)]
        [SerializeField]
        int assetDisplayNameLength = 16;

        [Space(5)]
        [SerializeField]
        bool insertDottedSurfix = true;

        [Space(5)]
        [SerializeField]
        List<AppData.SceneAssetDynamicContentContainer> assetContainerList = new List<AppData.SceneAssetDynamicContentContainer>();

        [Space(5)]
        [SerializeField]
        List<DynamicWidgetsContainer> dynamicWidgetsContainersList = new List<DynamicWidgetsContainer>();

        [Space(5)]
        [SerializeField]
        AppData.SceneAssetRenderMode assetRenderMode = AppData.SceneAssetRenderMode.Shaded;

        [Space(5)]
        [SerializeField]
        LayerMask arSceneAssetGroundLayer;

        [Space(5)]
        [SerializeField]
        float defaultImportScale = 1.0f, assetScaleRatio = 0.9f;

        [Space(5)]
        [SerializeField]
        List<AppData.RuntimeValue<float>> sceneAssetScale = new List<AppData.RuntimeValue<float>>();

        [Space(5)]
        [SerializeField]
        List<AppData.RuntimeValue<float>> defaultExecutionTimes = new List<AppData.RuntimeValue<float>>();

        [Space(5)]
        [SerializeField]
        AppData.ColorSwatchData colorSwatchData = new AppData.ColorSwatchData();

        List<AppData.ColorSwatch> colorSwatchLibrary = new List<AppData.ColorSwatch>();

        [Space(5)]
        [SerializeField]
        AppData.UIScreenWidgetsPrefabDataLibrary screenWidgetPrefabLibrary = new AppData.UIScreenWidgetsPrefabDataLibrary();


        [Header("Deprecated - Will Be Removed")]
        [Space(5)]
        [SerializeField]
        AppData.UIScreenWidget fileListViewHandlerPrefab = null;

        [Header("Deprecated - Will Be Removed")]
        [Space(5)]
        [SerializeField]
        AppData.UIScreenWidget fileItemViewHandlerPrefab = null;

        [Space(5)]
        [SerializeField]
        List<AppData.UIImageData> imageDataLibrary = new List<AppData.UIImageData>();

        List<AppData.UIScreenWidget> loadedWidgets = new List<AppData.UIScreenWidget>();

        #region Folder Data

        [Space(5)]
        [SerializeField]
        string folderListViewWidgetPrefabDirectory = "UI Prefabs/Folder_ListView";

        [Space(5)]
        [SerializeField]
        string folderItemViewWidgetPrefabDirectory = "UI Prefabs/Folder_ItemView";

        #endregion

        #region File Data

        [Space(5)]
        [SerializeField]
        string fileListViewWidgetPrefabDirectory = "UI Prefabs/File_ListView";

        [Space(5)]
        [SerializeField]
        string fileItemViewWidgetPrefabDirectory = "UI Prefabs/File_ItemView";

        #endregion


        [Space(5)]
        [SerializeField]
        string profileWidgetPrefabDirectory = "UI Prefabs/Profile";

        [Space(5)]
        [SerializeField]
        string colorSwatchButtonHandlerPrefabDirectory = "UI Prefabs/ColorSwatch";

        AppData.SceneMode currentSceneMode;

        List<string> assetSearchList = new List<string>();
        List<AppData.SceneAsset> selectedSceneAssetList = new List<AppData.SceneAsset>();

        List<AppData.StorageDirectoryData> appDirectories = new List<AppData.StorageDirectoryData>();

        [Space(5)]
        [SerializeField]
        AppData.StorageDirectoryData folderStructureDirectoryData = new AppData.StorageDirectoryData();

        AppData.SceneAsset currentSceneAsset;

        AppData.SceneAssetLibrary sceneAssetLibrary = new AppData.SceneAssetLibrary();

        [Space(5)]
        [SerializeField]
        List<AppData.SceneAsset> sceneAssetList = new List<AppData.SceneAsset>();

        List<AppData.SceneAssetWidget> screenWidgetList = new List<AppData.SceneAssetWidget>();
        AppData.AssetExportData currentAssetExportData = new AppData.AssetExportData();

        [SerializeField]
        List<AppData.DropDownContentData> dropDownContentDataList = new List<AppData.DropDownContentData>();

        RenderProfileUIHandler renderProfileUIHandlerPrefab = null;
        ColorSwatchButtonHandler colorSwatchButtonHandlerPrefab = null;

        AppData.Folder<UIScreenHandler> folderList = new AppData.Folder<UIScreenHandler>();

        AppData.FolderStructureType currentViewedFolderStructure;

        AppData.SceneAssetCategoryType assetFilterType;
        AppData.SceneAssetSortType assetSortType;

        Quaternion assetDefaultImportRotation;

        List<string> tempFolderNameList = new List<string>();
        public string CreateNewFolderName { get; set; }

        List<UIScreenFolderWidget> folderHandlerComponentsList = new List<UIScreenFolderWidget>();

        [SerializeField]
        AppData.Folder currentFolder;

        AppData.Folder widgetsRefreshFolder = new AppData.Folder();
        DynamicWidgetsContainer widgetsRefreshDynamicContainer = null;

        List<AppData.Folder> folders = new List<AppData.Folder>();

        List<string> dropdownContentPlaceholder = new List<string> { "NONE" };

        AppData.FocusedWidgetOrderType currentFocusedWidgetOrderType;
        bool onNewAssetCreated = false;
        string newAssetName;

        #region Rnder Profile Data Components

        List<RenderProfileUIHandler> renderProfileUIHandlerComponentsList = new List<RenderProfileUIHandler>();

        AppData.NavigationRenderSettingsProfileID profileID;

        #endregion

        #endregion

        #region Unity Callbacks

        void Awake() => SetupInstance();

        void Start() => Init();

        void OnEnable() => OnActionEventSubscription(true);

        void OnDisable() => OnActionEventSubscription(false);

        #endregion

        void SetupInstance()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(_instance.gameObject);
            }

            _instance = this;
        }

        void Init()
        {
            if (defaultDirectories == null)
            {
                LogWarning("App Default Directories Missing.", this, () => Init());
                return;
            }

            if (assetContainerList.Count > 0)
            {
                foreach (var assetContainer in assetContainerList)
                {
                    if (assetContainer.value == null)
                    {
                        LogWarning($"--> Container Missing for : {assetContainer.name}", this, () => Init());
                        break;
                    }
                }
            }

            if (dynamicWidgetsContainersList.Count > 0)
            {
                foreach (var assetContainer in dynamicWidgetsContainersList)
                {
                    if (assetContainer == null)
                    {
                        LogWarning($"Container Missing for : {assetContainer.name}", this, () => Init());
                        break;
                    }
                }
            }

            InitializeDropDownContentDataList();

            foreach (var directory in defaultDirectories)
            {
                if (directory.type != AppData.DirectoryType.None)
                {

                    string path = defaultAppDirectoryFolderName + "/" + directory.type.ToString();

                    string directoryPath = directoryPath = Application.productName;
                    string defaultDirectory = String.Empty;

                    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        defaultDirectory = Path.Combine(directoryPath, path);
                    }
                    else
                    {
                        defaultDirectory = Path.Combine(Application.dataPath, path);
                    }

                    string formattedDirectory = defaultDirectory.Replace("\\", "/");

                    // Create a new default storage path
                    AppData.StorageDirectoryData appDirectory = new AppData.StorageDirectoryData
                    {
                        name = directory.name,
                        directory = formattedDirectory,
                        type = directory.type
                    };

                    if (!Directory.Exists(formattedDirectory))
                    {
                        CreateDirectory(appDirectory, (directoryCreated) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(directoryCreated.resultsCode))
                            {
                                if (!appDirectories.Contains(appDirectory))
                                    appDirectories.Add(appDirectory);
                            }
                            else
                                LogError($"Failed To Create Directory : {directory}", this, () => Init());
                        });
                    }
                    else
                    {
                        if (!appDirectories.Contains(appDirectory))
                            appDirectories.Add(appDirectory);
                    }
                }
                else
                    LogWarning("App Data Path Set To None.", this, () => Init());
            }

            if (sceneAssetList == null || sceneAssetList.Count == 0)
                OnInitializeLoadSceneAssets();

            sceneAssetLibrary.InitializeLibrary();

            #region Move This To Streaming Assets Or Addressables (Prefered).

            // Load from Resources Folder

            #region File Data

            Debug.LogError("===========> Please Load Prefabs From Library........");

            //if (fileListViewHandlerPrefab == null)
            //    if (!string.IsNullOrEmpty(fileListViewWidgetPrefabDirectory))
            //        fileListViewHandlerPrefab = Resources.Load<UIScreenFolderWidget>(fileListViewWidgetPrefabDirectory);
            //    else
            //        LogWarning("Couldn't Load Asset From Resources - Directory Missing.", this, () => Init());

            //if (fileItemViewHandlerPrefab == null)
            //    if (!string.IsNullOrEmpty(fileItemViewWidgetPrefabDirectory))
            //        fileItemViewHandlerPrefab = Resources.Load<UIScreenFolderWidget>(fileItemViewWidgetPrefabDirectory);
            //    else
            //        LogWarning("Couldn't Load Asset From Resources - Directory Missing.", this, () => Init());

            #endregion

            if (renderProfileUIHandlerPrefab == null)
                if (!string.IsNullOrEmpty(profileWidgetPrefabDirectory))
                    renderProfileUIHandlerPrefab = Resources.Load<RenderProfileUIHandler>(profileWidgetPrefabDirectory);
                else
                    LogWarning("Couldn't Load Asset From Resources - Directory Missing.", this, () => Init());

            if (colorSwatchButtonHandlerPrefab == null)
                if (!string.IsNullOrEmpty(profileWidgetPrefabDirectory))
                    colorSwatchButtonHandlerPrefab = Resources.Load<ColorSwatchButtonHandler>(colorSwatchButtonHandlerPrefabDirectory);
                else
                    LogWarning("Couldn't Load Asset From Resources - Directory Missing.", this, () => Init());

            #endregion

            // SetWidgetsRefreshData(folderStructureData.rootFolder, folderStructureData.GetMainFolderDynamicWidgetsContainer());



            InitializeFolderLayoutView(GetLayoutViewType());

            #region Load Folder Stucture Data

            LoadFolderStuctureData((structureLoader) =>
            {
                if (AppData.Helpers.IsSuccessCode(structureLoader.resultsCode))
                {
                    folderStructureData = structureLoader.data;
                    SetCurrentFolder(folderStructureData.rootFolder);
                }
                else
                    LogWarning(structureLoader.results, this, () => Init());
            });

            #endregion

            Resources.UnloadUnusedAssets();
        }

        void InitializeDropDownContentDataList()
        {
            if (dropDownContentDataList == null)
                dropDownContentDataList = new List<AppData.DropDownContentData>();

            // Export Extensions
            AppData.DropDownContentData exportExtensionsData = new AppData.DropDownContentData
            {
                name = "Export Extensions Data List",
                data = AppData.Helpers.GetEnumToStringList<AppData.ExportExtensionType>(),
                contentType = AppData.DropDownContentType.Extensions
            };

            if (exportExtensionsData.data.Count <= 0)
                LogWarning($"Couldn't Create Drop Down Content Data For : {exportExtensionsData.name}", this, () => InitializeDropDownContentDataList());

            // Asset Categories
            AppData.DropDownContentData assetCategoriesData = new AppData.DropDownContentData
            {
                name = "Asset Categories Data List",
                data = AppData.Helpers.GetEnumToStringList<AppData.SceneAssetCategoryType>(),
                contentType = AppData.DropDownContentType.Categories
            };

            if (assetCategoriesData.data.Count <= 0)
                LogWarning($"Couldn't Create Drop Down Content Data For : {assetCategoriesData.name}", this, () => InitializeDropDownContentDataList());

            // Asset Sorting List
            AppData.DropDownContentData assetSortingData = new AppData.DropDownContentData
            {
                name = "Asset Sorting List",
                data = AppData.Helpers.GetEnumToStringList<AppData.SceneAssetSortType>(),
                contentType = AppData.DropDownContentType.Sorting
            };

            if (assetSortingData.data.Count <= 0)
                LogWarning($"Couldn't Create Drop Down Content Data For : {assetSortingData.name}", this, () => InitializeDropDownContentDataList());

            // Rendering Modes List
            AppData.DropDownContentData renderModegData = new AppData.DropDownContentData
            {
                name = "Rendering Mode List",
                data = AppData.Helpers.GetEnumToStringList<AppData.SceneAssetRenderMode>(),
                contentType = AppData.DropDownContentType.RenderingModes
            };

            if (renderModegData.data.Count <= 0)
                LogWarning($"Couldn't Create Drop Down Content Data For : {renderModegData.name}", this, () => InitializeDropDownContentDataList());

            // Render Profile List
            AppData.DropDownContentData renderProfileData = new AppData.DropDownContentData
            {
                name = "Render Profile List",
                data = AppData.Helpers.GetEnumToStringList<AppData.NavigationRenderSettingsProfileID>(),
                contentType = AppData.DropDownContentType.RenderProfiles
            };

            if (renderProfileData.data.Count <= 0)
                LogWarning($"Couldn't Create Drop Down Content Data For : {renderProfileData.name}", this, () => InitializeDropDownContentDataList());

            // Color Space List
            AppData.DropDownContentData colorSpaceData = new AppData.DropDownContentData
            {
                name = "Color Space List",
                data = AppData.Helpers.GetEnumToStringList<AppData.ColorSpaceType>(),
                contentType = AppData.DropDownContentType.ColorSpaces
            };

            if (renderProfileData.data.Count <= 0)
                LogWarning($"Couldn't Create Drop Down Content Data For : {colorSpaceData.name}", this, () => InitializeDropDownContentDataList());

            // Color Picker Type List
            AppData.DropDownContentData colorPickerData = new AppData.DropDownContentData
            {
                name = "Color Picker Type List",
                data = AppData.Helpers.GetEnumToStringList<AppData.ColorPickerType>(),
                contentType = AppData.DropDownContentType.ColorPickers
            };

            if (renderProfileData.data.Count <= 0)
                LogWarning($"Couldn't Create Drop Down Content Data For : {colorPickerData.name}", this, () => InitializeDropDownContentDataList());

            // Skybox Settings Type List
            AppData.DropDownContentData skyboxSettingsData = new AppData.DropDownContentData
            {
                name = "Skybox Settings Type List",
                data = AppData.Helpers.GetEnumToStringList<AppData.SkyboxSettingsType>(),
                contentType = AppData.DropDownContentType.SkyboxSettings
            };

            if (skyboxSettingsData.data.Count <= 0)
                LogWarning($"Couldn't Create Drop Down Content Data For : {skyboxSettingsData.name}", this, () => InitializeDropDownContentDataList());

            // Rotational Direction List
            AppData.DropDownContentData rotationalDirectionsData = new AppData.DropDownContentData
            {
                name = "Rotational Direction List",
                data = AppData.Helpers.GetEnumToStringList<AppData.RotationalDirection>(),
                contentType = AppData.DropDownContentType.Directions
            };

            if (rotationalDirectionsData.data.Count <= 0)
                LogWarning($"Couldn't Create Drop Down Content Data For : {rotationalDirectionsData.name}", this, () => InitializeDropDownContentDataList());

            dropDownContentDataList.Add(exportExtensionsData);
            dropDownContentDataList.Add(assetCategoriesData);
            dropDownContentDataList.Add(assetSortingData);
            dropDownContentDataList.Add(renderModegData);
            dropDownContentDataList.Add(renderProfileData);
            dropDownContentDataList.Add(colorSpaceData);
            dropDownContentDataList.Add(colorPickerData);
            dropDownContentDataList.Add(skyboxSettingsData);
            dropDownContentDataList.Add(rotationalDirectionsData);

            if (dropDownContentDataList.Count > 0)
                AppData.ActionEvents.OnDropDownContentDataInitializedEvent();
        }

        void OnInitializeLoadSceneAssets()
        {
            LoadSceneAssets(folderStructureData.rootFolder, (loadedAssetsCallback) =>
            {
                if (AppData.Helpers.IsSuccessCode(loadedAssetsCallback.resultsCode))
                    sceneAssetList = loadedAssetsCallback.data;
                else
                    LogWarning("Couldn't Initially Load Scene Assets.", this, () => OnInitializeLoadSceneAssets());
            });
        }

        void OnActionEventSubscription(bool subscribe = false)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnActionButtonFieldUploadedEvent += OnActionButtonFieldUploadedEvent;
                AppData.ActionEvents._OnScreenRefreshed += OnScreenRefreshedEvent;
                AppData.ActionEvents._OnUpdateSceneAssetDefaultRotation += OnUpdateSceneAssetDefaultRotationEvent;
            }
            else
            {
                AppData.ActionEvents._OnActionButtonFieldUploadedEvent -= OnActionButtonFieldUploadedEvent;
                AppData.ActionEvents._OnScreenRefreshed -= OnScreenRefreshedEvent;
                AppData.ActionEvents._OnUpdateSceneAssetDefaultRotation -= OnUpdateSceneAssetDefaultRotationEvent;
            }
        }

        void OnUpdateSceneAssetDefaultRotationEvent(Quaternion rotation)
        {
            assetDefaultImportRotation = rotation;
        }

        void OnActionButtonFieldUploadedEvent(AppData.InputActionButtonType actionType, bool interactable, bool isSelected)
        {
            try
            {
                if (ScreenUIManager.Instance != null)
                {
                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                    {
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(actionType, interactable, isSelected);
                    }
                    else
                        LogWarning("Couldn't To Get Current Screen Data From Screen Manager.", this, () => OnActionButtonFieldUploadedEvent(actionType, interactable, isSelected));
                }
                else
                    LogError("Screen Manager Not Yet Initialized", this, () => OnActionButtonFieldUploadedEvent(actionType, interactable, isSelected));
            }
            catch (NullReferenceException exception)
            {
                ThrowException(AppData.LogExceptionType.NullReference, exception, this, "OnActionButtonFieldUploadedEvent(AppData.InputActionButtonType actionType, bool interactable, bool isSelected)");
                //throw new Exception($"--> Unity - Failed On Action Button Field Uploaded Event - With Exception : {exception}");
            }
        }

        void OnScreenRefreshedEvent(AppData.UIScreenViewComponent screenData)
        {
            if (screenData.value.GetUIScreenType() == AppData.UIScreenType.AssetCreationScreen)
            {
                #region OBJ Field

                // Toggle OBJ State
                if (currentSceneAsset.modelAsset)
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.OpenFilePicker_OBJ, true, true);
                    else
                        LogWarning($"Screen Manager Not Yet Initialized.", this, () => OnScreenRefreshedEvent(screenData));
                }
                else
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.OpenFilePicker_OBJ, false, false);
                    else
                        LogWarning($"Screen Manager Not Yet Initialized.", this, () => OnScreenRefreshedEvent(screenData));
                }

                #endregion

                #region Thumbnail Field

                // Toggle Thumbnail State
                if (!string.IsNullOrEmpty(currentSceneAsset.GetAssetField(AppData.AssetFieldType.Thumbnail).path))
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.OpenFilePicker_Thumbnail, true, true);
                    else
                        Debug.LogWarning("--> Screen Manager Not Yet Initialized.");
                }
                else
                {
                    Debug.LogWarning($"--> Current Screen : {screenData.value.GetUIScreenType()}'s Current Scene Asset Thumbnail Missing / Not Assigned In The Inspector Panel.");

                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.OpenFilePicker_Thumbnail, false, false);
                    else
                        Debug.LogWarning("--> Screen Manager Not Yet Initialized.");
                }

                #endregion

                #region Main Texture Field

                // Toggle Main Texture State
                if (!string.IsNullOrEmpty(currentSceneAsset.GetAssetField(AppData.AssetFieldType.MainTexture).path))
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.OpenFilePicker_MainTexture, true, true);
                    else
                        Debug.LogWarning("--> Screen Manager Not Yet Initialized.");
                }
                else
                {
                    Debug.LogWarning($"--> Current Screen : {screenData.value.GetUIScreenType()}'s Current Scene Asset Main Texture Missing / Not Assigned In The Inspector Panel.");

                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.OpenFilePicker_MainTexture, false, false);
                    else
                        Debug.LogWarning("--> Screen Manager Not Yet Initialized.");
                }

                #endregion

                #region Normal Map Field

                // Toggle Normal Map State
                if (!string.IsNullOrEmpty(currentSceneAsset.GetAssetField(AppData.AssetFieldType.NormalMap).path))
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.OpenFilePicker_NormalMap, true, true);
                    else
                        Debug.LogWarning("--> Screen Manager Not Yet Initialized.");
                }
                else
                {
                    Debug.LogWarning($"--> Current Screen : {screenData.value.GetUIScreenType()}'s Current Scene Asset Normal Map Missing / Not Assigned In The Inspector Panel.");

                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.OpenFilePicker_NormalMap, false, false);
                    else
                        Debug.LogWarning("--> Screen Manager Not Yet Initialized.");
                }

                #endregion

                #region AO Map Field

                // Toggle AO Map State
                if (!string.IsNullOrEmpty(currentSceneAsset.GetAssetField(AppData.AssetFieldType.AmbientOcclusionMap).path))
                {
                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.OpenFilePicker_AOMap, true, true);
                    else
                        Debug.LogWarning("--> Screen Manager Not Yet Initialized.");
                }
                else
                {
                    Debug.LogWarning($"--> Current Screen : {screenData.value.GetUIScreenType()}'s Current Scene Asset AO Map Missing / Not Assigned In The Inspector Panel.");

                    if (ScreenUIManager.Instance != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.OpenFilePicker_AOMap, false, false);
                    else
                        Debug.LogWarning("--> Screen Manager Not Yet Initialized.");
                }

                #endregion
            }
            else
                Debug.Log($"---> Screen : {screenData.value.GetUIScreenType()} Refreshed.");
        }

        public void OnNewAssetDataCreated(AppData.SceneAssetData assetData, Action<AppData.SceneAsset, bool> callback)
        {
            // Create
            CreateNewAsset(assetData, (newAsset, results) =>
            {
                if (results)
                {
                    currentSceneAsset = newAsset;
                    AppData.ActionEvents.OnCreatedAssetDataEditEvent(currentSceneAsset);

                    callback.Invoke(currentSceneAsset, true);
                }
                else
                {
                    Debug.LogWarning("--> Failed To Create A New Scene Asset.");
                    callback.Invoke(new AppData.SceneAsset(), false);
                }

            });
        }

        public void OnSceneAssetEditMode(AppData.SceneDataPackets dataPackets)
        {
            try
            {
                if (dataPackets.sceneAsset != null)
                    OnSceneAssetScreenPreviewSetup(dataPackets);
                else
                    Debug.LogWarning("--> Data Packets Scene Asset Null.");
            }
            catch (Exception e)
            {
                Debug.LogError($"--> Scene Asset Enter Edit Mode Failed With Exception : {e}");
            }
        }

        public void OnSceneAssetPreviewMode(AppData.SceneDataPackets dataPackets)
        {
            try
            {
                if (dataPackets.sceneAsset != null)
                    OnSceneAssetScreenPreviewSetup(dataPackets);
                else
                    Debug.LogWarning("--> Data Packets Scene Asset Null.");
            }
            catch (Exception e)
            {
                Debug.LogError($"--> On Scene Asset Preview Mode Failed With Exception : {e}");
            }
        }

        void OnSceneAssetScreenPreviewSetup(AppData.SceneDataPackets dataPackets)
        {
            if (GetSceneAssetsContainer(dataPackets.containerType, dataPackets.screenType))
            {
                if (sceneAssetLibrary.SceneAssetExists(dataPackets.sceneAsset.name))
                {
                    dataPackets.sceneAsset = sceneAssetLibrary.GetAsset(dataPackets.sceneAsset.name);

                    if (dataPackets.sceneAsset != null)
                    {
                        Debug.Log($"--> Scene Asset Found : {  dataPackets.sceneAsset.name} - Reuse Asset Model");

                        if (dataPackets.sceneAsset.modelAsset)
                        {
                            SetCurrentSceneMode(dataPackets.sceneMode);

                            if (SelectableManager.Instance)
                            {
                                SelectableManager.Instance.UpdateSelectableAssetContainer(dataPackets.sceneAsset.sceneObject.value, dataPackets.containerType, dataPackets.screenType, (results) =>
                                {
                                    if (!results)
                                        Debug.LogError($"--> Update Selectable Asset Container Failed - Scene Asset Model : {dataPackets.sceneAsset.name} Not Found In The Selectable Game Object List.");
                                });
                            }
                            else
                                Debug.LogWarning("--> Selectable Manager Not Yet Initialized.");


                            Debug.Log($"--------------> Loaded Re-Used Asset : {dataPackets.sceneAsset.name}'s Position Is : {dataPackets.sceneAsset.assetImportPosition} - Rotation : {dataPackets.sceneAsset.assetImportRotation}");

                            AddAssetToContainer(dataPackets.sceneAsset.sceneObject.value, dataPackets.keepAssetWorldPose, true, true, dataPackets.containerType, dataPackets.screenType, dataPackets.sceneAssetScaleValueType, dataPackets.keepAssetCentered, dataPackets.scaleSceneAsset, dataPackets.clearContentContainer, false);

                            //if (RenderingManager.Instance)
                            //{
                            //    bool hasMTLFile = (!string.IsNullOrEmpty(dataPackets.sceneAsset.GetAssetField(AppData.AssetFieldType.MTLFile).path)) ? true : false;
                            //    RenderingManager.Instance.SetCurrentRenderedSceneAsset(dataPackets.sceneAsset.sceneObject.value, hasMTLFile, materialProperties);
                            //}
                            //else
                            //    Debug.LogWarning("--> Rendering Manager Failed To Initialize.");

                            if (dataPackets.sceneAsset.sceneObject.info.fields != null)
                            {
                                dataPackets.sceneAsset.info = dataPackets.sceneAsset.sceneObject.info;
                            }

                            if (dataPackets.sceneAsset.sceneObject.value != null)
                                dataPackets.sceneAsset.modelAsset = dataPackets.sceneAsset.sceneObject.value;
                            else
                                Debug.LogWarning($"--> Model Asset For : {dataPackets.sceneAsset.name} Is Missing Or Null.");

                            if (dataPackets.sceneAsset.modelAsset != null)
                            {
                                dataPackets.sceneAsset.modelAsset.name = dataPackets.sceneAsset.name;
                                currentSceneAsset = dataPackets.sceneAsset;

                                if (ScreenUIManager.Instance != null)
                                {
                                    var screen = ScreenUIManager.Instance.GetCurrentScreenData();
                                    screen.value.GetScreenData().sceneAsset = currentSceneAsset;

                                   ScreenUIManager.Instance.UpdateInfoDisplayer(screen);
                                }
                                else
                                    Debug.LogWarning("--> Screen Manager Not Yet Initialized.");

                                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() != AppData.UIScreenType.ARViewScreen)
                                    dataPackets.sceneAsset.sceneObject.value.transform.position = dataPackets.sceneAsset.assetImportPosition;
                                else
                                {
                                    Vector3 assetPos = dataPackets.sceneAsset.assetImportPosition;
                                    assetPos.y /= 100;
                                    dataPackets.sceneAsset.sceneObject.value.transform.position = assetPos;
                                }

                                dataPackets.sceneAsset.sceneObject.value.transform.rotation = GetSceneAssetImportRotation(dataPackets.sceneAsset.assetImportRotation);

                                AppData.ActionEvents.OnCreatedAssetDataEditEvent(currentSceneAsset);

                                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == AppData.UIScreenType.ARViewScreen)
                                    AppData.ActionEvents.OnARSceneAssetStateEvent(AppData.ARSceneContentState.Place);
                            }
                            else
                                Debug.LogWarning($"--> Model Asset For : {dataPackets.sceneAsset.name} Is Missing Or Null.");
                        }
                        else
                            Debug.LogWarning($"--> On Scene Asset Screen Preview Setup Scene Asset : {dataPackets.sceneAsset.name} Model Missing / Null / Not Found.");

                    }
                    else
                        Debug.LogWarning("--> On Scene Asset Screen Preview Setup Loaded Scene Asset Data Is Null");
                }

                if (!sceneAssetLibrary.SceneAssetExists(dataPackets.sceneAsset.name))
                {
                    Debug.Log($"--> Scene Asset : {dataPackets.sceneAsset.name} Model Is Not Loaded. Instantiate.");

                    SetCurrentSceneMode(dataPackets.sceneMode);

                    dataPackets.sceneAsset.sceneObject = AppData.Helpers.LoadFormattedSceneAssetModel(dataPackets.sceneAsset.GetAssetField(AppData.AssetFieldType.OBJFile).path);

                    if (dataPackets.sceneAsset.sceneObject.value)
                    {

                        Debug.Log($"--------------> Loaded Initial Asset : {dataPackets.sceneAsset.name}'s Position Is : {dataPackets.sceneAsset.assetImportPosition} - Rotation : {dataPackets.sceneAsset.assetImportRotation}");

                        if (dataPackets.sceneAsset.sceneObject.value.name.Length > assetDisplayNameLength)
                        {
                            string formattedAssetName = string.Empty;

                            if (insertDottedSurfix)
                                formattedAssetName = dataPackets.sceneAsset.sceneObject.value.name.Substring(0, assetDisplayNameLength) + "...";
                            else
                                formattedAssetName = dataPackets.sceneAsset.sceneObject.value.name.Substring(0, assetDisplayNameLength);

                            dataPackets.sceneAsset.sceneObject.value.name = formattedAssetName;
                        }
                    }

                    if (SelectableManager.Instance)
                        SelectableManager.Instance.AddToSelectableList(dataPackets.sceneAsset.sceneObject.value, dataPackets.containerType, dataPackets.screenType);
                    else
                        Debug.LogWarning("--> Selectable Manager Not Yet Initialized.");

                    AppData.MaterialProperties materialProperties = dataPackets.sceneAsset.GetMaterialProperties();

                    if (materialProperties.glossiness == 0.0f || materialProperties.bumpScale == 0.0f || materialProperties.aoStrength == 0.0f)
                        materialProperties = AppData.Helpers.GetMaterialProperties(dataPackets.sceneAsset.sceneObject.value, dataPackets.sceneAsset);

                    // Set Texture Info
                    materialProperties.mainTexturePath = dataPackets.sceneAsset.GetAssetField(AppData.AssetFieldType.MainTexture).path;
                    materialProperties.normalMapTexturePath = dataPackets.sceneAsset.GetAssetField(AppData.AssetFieldType.NormalMap).path;
                    materialProperties.aoMapTexturePath = dataPackets.sceneAsset.GetAssetField(AppData.AssetFieldType.AmbientOcclusionMap).path;

                    dataPackets.sceneAsset.SetMaterialProperties(materialProperties);

                    AddAssetToContainer(dataPackets.sceneAsset.sceneObject.value, dataPackets.keepAssetWorldPose, true, true, dataPackets.containerType, dataPackets.screenType, dataPackets.sceneAssetScaleValueType, dataPackets.keepAssetCentered, dataPackets.scaleSceneAsset, dataPackets.clearContentContainer, false);

                    if (RenderingSettingsManager.Instance)
                    {
                        bool hasMTLFile = (!string.IsNullOrEmpty(dataPackets.sceneAsset.GetAssetField(AppData.AssetFieldType.MTLFile).path)) ? true : false;
                        RenderingSettingsManager.Instance.SetCurrentRenderedSceneAsset(dataPackets.sceneAsset.sceneObject.value, hasMTLFile, materialProperties);
                    }
                    else
                        Debug.LogWarning("--> Rendering Manager Failed To Initialize.");

                    if (dataPackets.sceneAsset.sceneObject.info.fields != null)
                    {
                        dataPackets.sceneAsset.info = dataPackets.sceneAsset.sceneObject.info;
                    }

                    if (dataPackets.sceneAsset.sceneObject.value != null)
                        dataPackets.sceneAsset.modelAsset = dataPackets.sceneAsset.sceneObject.value;
                    else
                        Debug.LogWarning($"--> Model Asset For : {dataPackets.sceneAsset.name} Is Missing Or Null.");

                    if (dataPackets.sceneAsset.modelAsset != null)
                    {
                        dataPackets.sceneAsset.modelAsset.name = dataPackets.sceneAsset.name;
                        currentSceneAsset = dataPackets.sceneAsset;

                        if (ScreenUIManager.Instance != null)
                        {
                            var screen = ScreenUIManager.Instance.GetCurrentScreenData();
                            screen.value.GetScreenData().sceneAsset = currentSceneAsset;

                            ScreenUIManager.Instance.UpdateInfoDisplayer(screen);
                        }
                        else
                            Debug.LogWarning("--> Screen Manager Not Yet Initialized.");

                        AppData.ActionEvents.OnCreatedAssetDataEditEvent(currentSceneAsset);
                    }
                    else
                        Debug.LogWarning($"--> Model Asset For : {dataPackets.sceneAsset.name} Is Missing Or Null.");

                    dataPackets.sceneAsset.sceneObject.value.transform.position = dataPackets.sceneAsset.assetImportPosition;
                    dataPackets.sceneAsset.sceneObject.value.transform.rotation = GetSceneAssetImportRotation(dataPackets.sceneAsset.assetImportRotation);
                    sceneAssetLibrary.AddSceneAssetObjectToLibrary(dataPackets.sceneAsset);

                    if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == AppData.UIScreenType.ARViewScreen)
                        AppData.ActionEvents.OnARSceneAssetStateEvent(AppData.ARSceneContentState.Place);
                }
            }
            else
                Debug.LogWarning("--> Scene Asset Container Not Yet Assigned.");
        }

        public void ClearSceneAssetLibrary()
        {
            List<GameObject> assets = sceneAssetLibrary.GetSceneAssetModels();

            if (assets.Count > 0)
            {
                for (int i = 0; i < assets.Count; i++)
                {
                    Destroy(assets[i]);
                }

                sceneAssetLibrary.ClearLibrary();
            }
            else
                Debug.LogWarning("--> Scene Asset Library Is Empty. Nothing To Clear.");
        }

        public void OnClearPreviewedContent(bool scaleContent, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (assetContainerList.Count > 0)
            {
                foreach (var content in assetContainerList)
                {
                    if (content.value)
                        content.Clear(scaleContent);
                    else
                        Debug.LogWarning($"--> Container Found Assigned For Game Object : {content.name}.");
                }

                if(assetContainerList.Count == 0)
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                else
                {
                    callbackResults.results = "Assets Didn't Clear. Check Here Please.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
                Debug.LogWarning("--> No Asset Container Found.");

            callback?.Invoke(callbackResults);
        }

        void AddAssetToContainer(GameObject asset, bool keepWorldPos, bool fitInsideContainer, bool isEnabled, AppData.ContentContainerType containerType, AppData.UIScreenType screenType, AppData.RuntimeValueType sceneAssetScaleValueType, bool keepAssetCentered, bool scaleContent, bool clearContainer = false, bool isImport = false)
        {
            if (assetContainerList.Count > 0)
            {
                foreach (var content in assetContainerList)
                {
                    if (content.value)
                    {
                        if (clearContainer)
                            content.Clear(scaleContent);

                        if (content.containerType == containerType && content.screenType == screenType)
                        {
                            content.Add(asset, arSceneAssetGroundLayer, keepWorldPos, fitInsideContainer, isEnabled, defaultImportScale, assetScaleRatio, keepAssetCentered, isImport);
                            content.ScaleContent(sceneAssetScale.Find((x) => x.valueType == sceneAssetScaleValueType).value, scaleContent);
                        }
                    }
                    else
                        Debug.LogWarning($"--> Container Found Assigned For Game Object : {content.name}.");
                }
            }
            else
                Debug.LogWarning("--> No Asset Container Found.");
        }

        public void AddContentToDynamicWidgetContainer(AppData.UIScreenWidget contentWidget, AppData.ContentContainerType containerType, AppData.OrientationType orientation)
        {
            DynamicWidgetsContainer container = dynamicWidgetsContainersList.Find((x) => x.GetContentContainerType() == containerType);

            if (container != null && container.IsContainerActive())
                container.AddDynamicWidget(contentWidget, orientation, false);
            else
                Debug.LogWarning("--> AddContentToDynamicWidgetContainer Failed : DynamicWidgetsContainer Is Null.");
        }

        public Transform GetSceneAssetsContainer(AppData.ContentContainerType containerType, AppData.UIScreenType screenType)
        {
            Transform container = null;

            if (assetContainerList.Count > 0)
            {
                foreach (var content in assetContainerList)
                {
                    if (content.value)
                    {
                        if (content.containerType == containerType && content.screenType == screenType)
                        {
                            container = content.value;
                            break;
                        }
                        else
                            continue;
                    }
                    else
                        Debug.LogWarning($"--> Container Found Assigned For Game Object : {content.name}.");
                }
            }
            else
                Debug.LogWarning("--> No Asset Container Found.");

            return container;
        }

        public List<Transform> GetSceneAssetsContainerList(AppData.UIScreenType screenType = AppData.UIScreenType.None)
        {
            List<Transform> containers = new List<Transform>();

            if (assetContainerList.Count > 0)
            {
                foreach (var content in assetContainerList)
                {
                    if (content.value)
                    {
                        if (screenType != AppData.UIScreenType.None)
                        {
                            if (content.screenType == screenType)
                            {
                                containers.Add(content.value);
                                break;
                            }
                        }
                        else
                        {
                            containers.Add(content.value);
                        }
                    }
                    else
                        Debug.LogWarning($"--> Container Found Assigned For Game Object : {content.name}.");
                }
            }
            else
                Debug.LogWarning("--> No Asset Container Found.");

            return containers;
        }

        public AppData.UIScreenWidgetsPrefabDataLibrary GetWidgetsPrefabDataLibrary()
        {
            return screenWidgetPrefabLibrary;
        }

        public Transform GetSceneAssetsContainer(AppData.UIScreenType screenType = AppData.UIScreenType.None)
        {
            Transform container = null;

            if (assetContainerList.Count > 0)
            {
                foreach (var content in assetContainerList)
                {
                    if (content.value)
                    {
                        if (screenType != AppData.UIScreenType.None)
                        {
                            if (content.screenType == screenType)
                            {
                                container = content.value;
                                break;
                            }
                        }
                        else
                            Debug.LogWarning("--> Get Scene Assets Container Screen Sype Is Set To Null.");
                    }
                    else
                        Debug.LogWarning($"--> Container Found Assigned For Game Object : {content.name}.");
                }
            }
            else
                Debug.LogWarning("--> No Asset Container Found.");

            return container;
        }

        public AppData.SceneAssetDynamicContentContainer GetSceneAssetsContainerData(AppData.UIScreenType screenType = AppData.UIScreenType.None)
        {
            AppData.SceneAssetDynamicContentContainer container = new AppData.SceneAssetDynamicContentContainer();

            if (assetContainerList.Count > 0)
            {
                foreach (var content in assetContainerList)
                {
                    if (content.value)
                    {
                        if (screenType != AppData.UIScreenType.None)
                        {
                            if (content.screenType == screenType)
                            {
                                container = content;
                            }
                        }
                        else
                            Debug.LogWarning("--> Get Scene Assets Container Screen Sype Is Set To Null.");
                    }
                    else
                        Debug.LogWarning($"--> Container Found Assigned For Game Object : {content.name}.");
                }
            }
            else
                Debug.LogWarning("--> No Asset Container Found.");

            return container;
        }

        public List<AppData.SceneAssetDynamicContentContainer> GetSceneAssetDynamicContentContainer()
        {
            return assetContainerList;
        }

        public async void CreateNewAsset(AppData.SceneAssetData assetData, Action<AppData.SceneAsset, bool> callback)
        {
            try
            {
                //if (screenManager == null)
                //    screenManager = ScreenUIManager.Instance;

                //if (renderingManager == null)
                //    renderingManager = RenderingManager.Instance;

                //if (selectableManager == null)
                //    selectableManager = SelectableManager.Instance;

                if (assetData.assetFields != null && AppData.Helpers.FileIsValid(assetData.GetAssetField(AppData.AssetFieldType.OBJFile).path))
                {
                    AppData.ActionEvents.OnClearPreviewedSceneAssetObjectEvent();

                    AppData.SceneAsset sceneAsset = assetData.ToSceneAsset();

                    if (GetSceneAssetsContainer(AppData.ContentContainerType.AssetImport, AppData.UIScreenType.AssetCreationScreen) != null)
                    {
                        SetCurrentSceneMode(AppData.SceneMode.EditMode);

                        await LoadSceneAsset(assetData, sceneAsset, (sceneDataResult, isSuccessfull) =>
                        {

                            if (isSuccessfull)
                            {

                                // Temp Solution For Enabling Import Asset Container On New Asset Import - Refreshes Screen Manually.
                                AppData.ActionEvents.OnScreenChangeEvent(new AppData.SceneDataPackets { screenType = AppData.UIScreenType.AssetCreationScreen, containerType = AppData.ContentContainerType.AssetImport });

                                callback.Invoke(sceneAsset, true);

                            }
                            else
                                callback.Invoke(sceneAsset, false);

                        });

                    }
                    else
                        Debug.LogWarning("--> Failed To Get Scene Content Container Of Type : Asset Import For Asset Creation Screen");
                }
                else
                {
                    Debug.LogWarning("-->");
                    callback.Invoke(new AppData.SceneAsset(), false);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"--> Create New Scene Asset Mode Failed With Exception : {e}");
            }

        }

        async Task LoadSceneAsset(AppData.SceneAssetData assetData, AppData.SceneAsset sceneAsset, Action<AppData.SceneAsset, bool> callback)
        {
            if (!sceneAssetLibrary.AssetExists(sceneAsset))
            {
                AppData.SceneObject sceneObject = AppData.Helpers.LoadFormattedSceneAssetModel(assetData.GetAssetField(AppData.AssetFieldType.OBJFile).path, assetData.GetAssetField(AppData.AssetFieldType.MTLFile).path);

                if (sceneObject.value != null)
                {
                    sceneObject.value.AddComponent<SceneAssetModelHandler>();
                    sceneObject.value.transform.rotation = GetSceneAssetImportRotation(sceneAsset.assetImportRotation);

                    if (sceneObject.value.name.Length > assetDisplayNameLength)
                    {
                        string formattedAssetName = string.Empty;

                        if (insertDottedSurfix)
                            formattedAssetName = sceneObject.value.name.Substring(0, assetDisplayNameLength) + "...";
                        else
                            formattedAssetName = sceneObject.value.name.Substring(0, assetDisplayNameLength);

                        sceneObject.value.name = formattedAssetName;
                    }

                    if (SelectableManager.Instance)
                        SelectableManager.Instance.AddToSelectableList(sceneObject.value, AppData.ContentContainerType.AssetImport, AppData.UIScreenType.AssetCreationScreen);
                    else
                        Debug.LogWarning("--> Selectable Manager Not Yet Initialized.");
                }
                else
                    Debug.LogWarning("--> Scene Object Value Missing / Null.");

                AppData.MaterialProperties materialProperties = AppData.Helpers.GetMaterialProperties(sceneObject.value, sceneAsset);
                sceneAsset.SetMaterialProperties(materialProperties);

                if (RenderingSettingsManager.Instance)
                {
                    bool hasMTLFile = (!string.IsNullOrEmpty(assetData.GetAssetField(AppData.AssetFieldType.MTLFile).path)) ? true : false;
                    RenderingSettingsManager.Instance.SetCurrentRenderedSceneAsset(sceneObject.value, hasMTLFile);
                }
                else
                    Debug.LogWarning("--> Rendering Manager Failed To Initialize.");


                if (sceneObject.value != null)
                {
                    sceneAsset.sceneObject = sceneObject;

                    sceneAsset.modelAsset = sceneObject.value;
                    sceneAsset.name = sceneAsset.modelAsset.name;

                    sceneAsset.info = sceneObject.info;

                    currentSceneAsset = sceneAsset;

                    if (ScreenUIManager.Instance != null)
                    {
                        // Fix This ASAP
                        //ScreenUIManager.Instance.UpdateInfoDisplayer(sceneAsset);
                    }
                    else
                        Debug.LogWarning("--> Screen Manager Not Yet Initialized.");

                    AddAssetToContainer(sceneObject.value, false, true, true, AppData.ContentContainerType.AssetImport, AppData.UIScreenType.AssetCreationScreen, AppData.RuntimeValueType.InspectorModeAsseScaleDeviderValue, true, false, true, true);

                    sceneAssetLibrary.AddSceneAssetObjectToLibrary(sceneAsset);

                    AppData.ActionEvents.OnCreatedAssetDataEditEvent(currentSceneAsset);

                    await Task.CompletedTask;

                    callback.Invoke(sceneAsset, true);

                }
                else
                    callback.Invoke(sceneAsset, false);
            }
            else
            {
                callback.Invoke(sceneAsset, false);
            }
        }

        Quaternion GetSceneAssetImportRotation(Vector3 eularAngle)
        {
            return Quaternion.Euler(eularAngle);
        }

        public void SetCurrentSceneAsset(AppData.SceneAsset sceneAsset)
        {
            // Reset States To Default - Disable All Fields On Start.
            if (ScreenUIManager.Instance != null)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonChildWidgetsState(AppData.InputActionButtonType.None, false, false);
                }
                else
                    Debug.LogWarning($"--> Failed To Set Current Scene Asset : {sceneAsset.name} - Current Screen Data Is Null.");
            }
            else
                Debug.LogWarning($"--> Failed To Set Current Scene Asset : {sceneAsset.name}");

            currentSceneAsset = sceneAsset;
        }

        public void UpdateCurrentSceneAsset(AppData.SceneAsset updatedSceneAsset)
        {
            currentSceneAsset = updatedSceneAsset;
        }

        public AppData.SceneAsset GetCurrentSceneAsset()
        {
            return currentSceneAsset;
        }

        public List<AppData.SceneAsset> GetSceneAssets()
        {
            return sceneAssetList;
        }

        public string GetDefaultAssetName()
        {
            return defaultAssetName;
        }

        public AppData.StorageDirectoryData GetAppDirectoryData(AppData.DirectoryType directoryType)
        {
            AppData.StorageDirectoryData directoryData = new AppData.StorageDirectoryData();

            if (appDirectories.Count > 0)
            {
                foreach (var directory in appDirectories)
                {
                    if (directory.type == directoryType)
                    {
                        directoryData = directory;
                        break;
                    }
                }
            }

            return directoryData;
        }

        public void BuildSceneAsset(AppData.StorageDirectoryData directoryData, Action<AppData.CallbackData<AppData.StorageDirectoryData>> callback = null)
        {
            AppData.CallbackData<AppData.StorageDirectoryData> callbackResults = new AppData.CallbackData<AppData.StorageDirectoryData>();

            if (currentSceneAsset.modelAsset != null)
            {
                DirectoryFound(directoryData.directory, directoryFoundCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(directoryFoundCallback.resultsCode))
                    {
                        AppData.SceneAssetData assetData = currentSceneAsset.ToSceneAssetData();

                        if (assetData.currentAssetMode == AppData.SceneAssetModeType.CreateMode)
                        {
                            if (assetData.assetFields != null)
                            {
                                List<AppData.AssetField> newAssetFields = new List<AppData.AssetField>();

                                foreach (var field in assetData.assetFields)
                                {
                                    if (AppData.Helpers.FileIsValid(field.path))
                                    {
                                        if (field.fieldType != AppData.AssetFieldType.None)
                                        {
                                            AppData.AssetField newField = field;

                                            string validPath = GetAppDirectory(field.directoryType).directory;
                                            string newDirectory = Path.Combine(validPath, assetData.name);

                                        // Create New Directory.
                                        if (CreateDirectory(newDirectory))
                                            {
                                                string fileNameWithExtension = field.name + "." + field.extensionType.ToString().ToLower();
                                                string newPath = Path.Combine(newDirectory, fileNameWithExtension);
                                                string formattedTargetFilePath = AppData.Helpers.GetFormattedDirectoryPath(newPath);

                                                CopyFilesFromUserStorage(field.path, formattedTargetFilePath, (newAssetPath, results) =>
                                                {
                                                    if (results)
                                                        newField.path = newAssetPath;
                                                    else
                                                        Debug.LogWarning($"--> Copying File : {field.name} Failed.");

                                                });

                                                newAssetFields.Add(newField);
                                            }
                                            else
                                                Debug.LogWarning("--> Failed To Create Directory And Add File.");
                                        }
                                    }
                                    else
                                        Debug.LogWarning($"--> Copying File Failed, Path : {field.path} Is Invalid.");
                                }

                                if (newAssetFields.Count > 0)
                                {
                                    assetData.assetFields = newAssetFields;
                                }
                            }
                        }

                        string validAssetName = assetData.name + "_FileData";
                        string fileNameWithJSONExtension = validAssetName + ".json";
                        string filePath = Path.Combine(directoryData.directory, fileNameWithJSONExtension);
                        string formattedFilePath = AppData.Helpers.GetFormattedDirectoryPath(filePath);

                        var dateTime = DateTime.Now;

                        AppData.StorageDirectoryData storageDirectory = new AppData.StorageDirectoryData
                        {
                            name = validAssetName,
                            type = AppData.DirectoryType.Sub_Folder_Structure,
                            path = formattedFilePath,
                            directory = directoryData.directory
                        };

                        assetData.creationDateTime = dateTime.ToString();
                        assetData.storageData = storageDirectory;

                        string JSONString = JsonUtility.ToJson(assetData);

                        if (!string.IsNullOrEmpty(JSONString))
                        {

                            if (!File.Exists(formattedFilePath))
                            {
                                File.WriteAllText(formattedFilePath, JSONString);

                                callbackResults.results = $"Success - Building Asset : {assetData.name} As : {formattedFilePath}";
                                callbackResults.data = directoryData;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                            else
                            {
                                File.Delete(formattedFilePath);

                                if (!File.Exists(formattedFilePath))
                                    File.WriteAllText(formattedFilePath, JSONString);

                                callbackResults.results = $"Success - Replaced Asset : {assetData.name} At Path : {formattedFilePath}";
                                callbackResults.data = directoryData;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = "Asset Build Failed - Couldn't Create A New JSON File.";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }

                    }
                    else
                    {
                        callbackResults.results = directoryFoundCallback.results;
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                });
            }
            else
            {
                callbackResults.results = "Asset Build Failed, Current Scene Asset Missing / Not Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        void CopyFilesFromUserStorage(string file, string targetFile, Action<string, bool> callback)
        {
            if (File.Exists(targetFile))
                File.Delete(targetFile);

            if (!File.Exists(targetFile))
                File.Copy(file, targetFile);

            if (File.Exists(targetFile))
            {
                callback.Invoke(targetFile, true);
            }
            else
                callback.Invoke(targetFile, false);
        }

        public AppData.FolderStructureData GetFolderStructureData()
        {
            return folderStructureData;
        }

        public string GetCreateNewFolderTempName()
        {
            // Do Some Calculations And Get A Tem Name.
            string tempName = string.Empty;
            AppData.Folder currentFolder = GetCurrentFolder();

            DirectoryFound(currentFolder.storageData.directory, directoryFoundCallback =>
            {
                if (AppData.Helpers.IsSuccessCode(directoryFoundCallback.resultsCode))
                {
                    string[] folderDataPathList = Directory.GetFiles(currentFolder.storageData.directory, "*_FolderData.json", SearchOption.TopDirectoryOnly);
                    string newTempName = "New Folder";

                    if (folderDataPathList.Length > 0)
                    {
                        List<string> folderNameList = new List<string>();
                        List<string> matchingFolderNameList = new List<string>();
                        string folderName = string.Empty;

                        foreach (var folderDataPath in folderDataPathList)
                        {
                            folderName = GetAssetNameFormatted(Path.GetFileNameWithoutExtension(folderDataPath), AppData.SelectableAssetType.Folder);

                            if (!folderNameList.Contains(folderName))
                                folderNameList.Add(folderName);
                        }

                        if (folderNameList.Count > 0)
                            foreach (var name in folderNameList)
                                if (name.Contains(newTempName))
                                    if (!matchingFolderNameList.Contains(name))
                                        matchingFolderNameList.Add(name);

                        if (matchingFolderNameList.Count == 0)
                            tempName = newTempName;

                        if (matchingFolderNameList.Count > 0)
                        {
                            if (matchingFolderNameList.Contains(newTempName))
                                tempName = newTempName + $" ({matchingFolderNameList.Count})";
                            else
                                tempName = newTempName;
                        }
                    }
                    else
                        tempName = newTempName;
                }
                else
                    Debug.LogWarning($"--> GetCreateNewFolderTempName's DirectoryFound Failed With Results : {directoryFoundCallback.results}");
            });

            return tempName;
        }

        public bool AssetNameExist(string assetName)
        {
            bool exist = false;

            return exist;
        }

        public void SetCurrentFolder(AppData.Folder folder) => currentFolder = folder;

        public AppData.Folder GetCurrentFolder()
        {
            if (string.IsNullOrEmpty(currentFolder.name))
                currentFolder = GetFolderStructureData().GetRootFolder();
            else
                currentFolder.isRootFolder = false;

            return currentFolder;
        }

        public (AppData.FocusedWidgetOrderType widgetOrderType, bool onNewAssetCreation, string widgetName) GetCurrentFocusedWidgetOrderType()
        {
            return (currentFocusedWidgetOrderType, onNewAssetCreated, newAssetName);
        }

        public void SetCurrentFocusedWidgetOrderType(AppData.FocusedWidgetOrderType focusedWidgetOrderType, bool onNewAssetCreated, string newAssetName)
        {
            currentFocusedWidgetOrderType = focusedWidgetOrderType;
            this.onNewAssetCreated = onNewAssetCreated;
            this.newAssetName = newAssetName;
        }

        public void OpenUIFolderStructure(AppData.Folder folder, AppData.UIWidgetInfo folderWidgetInfo, AppData.FolderStructureType structureType)
        {
            currentViewedFolderStructure = structureType;

            if (ScreenUIManager.Instance != null)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                {
                    if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == AppData.UIScreenType.ProjectViewScreen)
                    {
                        SetCurrentFolder(folder);

                        if (ScreenNavigationManager.Instance != null)
                        {
                            //Debug.LogError($"==> Show Folder Footer Nav : {ScreenNavigationManager.Instance.GetFolderNavigationDataPackets().widgetTitle}");
                            //ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(ScreenNavigationManager.Instance.GetFolderNavigationDataPackets());

                            ScreenNavigationManager.Instance.UpdateNavigationRootTitleDisplayer();
                        }
                        else
                            Debug.LogWarning("--> OpenUIFolderStructure's GetFolderNavigationDataPackets Failed : ScreenNavigationManager.Instance Is Not Yet Initialized.");

                        //ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonUIImageValue(AppData.InputActionButtonType.Return, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ReturnIcon);
                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, folder.name);
                        ScreenUIManager.Instance.Refresh();
                    }
                }
                else
                    Debug.LogWarning("--> OpenUIFolderStructure's Refresh Failed : ScreenUIManager.Instance.GetCurrentScreenData().value Is Null.");
            }
            else
                Debug.LogWarning("--> OpenUIFolderStructure's Refresh Failed : ScreenUIManager.Instance Is Not Yet Initialized.");
        }

        #region Formatted Names

        public string GetFileDataName(string name, AppData.SelectableAssetType assetType)
        {
            return ((assetType == AppData.SelectableAssetType.Folder) ? name.Replace("_FolderData.json", "") : name.Replace("_FileData.json", ""));
        }

        public string GetFileDataNameWithExtension(string name, AppData.SelectableAssetType assetType)
        {
            return name + ((assetType == AppData.SelectableAssetType.Folder) ? "_FolderData.json" : "_FileData.json");
        }

        public string GetFileDataNameWithoutExtension(string name, AppData.SelectableAssetType assetType)
        {
            return ((assetType == AppData.SelectableAssetType.Folder) ? name + "_FolderData" : name + "_FileData");
        }

        #endregion

        public void SetDefaultUIWidgetActionState(List<AppData.UIScreenWidget> widgets, AppData.DefaultUIWidgetActionState widgetActionState, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (widgets != null)
            {
                foreach (var widget in widgets)
                {
                    if (widget.GetSelectableAssetType() == AppData.SelectableAssetType.Folder)
                    {
                        AppData.Folder folder = widget.GetFolderData();
                        string formattedName = GetFormattedName(folder.name, AppData.SelectableAssetType.Folder, false);
                        folder.name = formattedName;
                        folder.defaultWidgetActionState = widgetActionState;

                        SaveFolderWidget(folder, folderSaved =>
                        {
                            if (AppData.Helpers.IsSuccessCode(folderSaved.resultsCode))
                                callbackResults = folderSaved;
                        });

                        if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                            widget.SetDefaultUIWidgetActionState(widgetActionState);
                    }

                    if (!AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                        break;

                    if (widget.GetSelectableAssetType() == AppData.SelectableAssetType.File)
                    {
                        AppData.SceneAsset sceneAsset = widget.GetSceneAssetData();
                        sceneAsset.defaultWidgetActionState = widgetActionState;

                        SaveAssetWidget(sceneAsset, assetSaved =>
                        {
                            callbackResults = assetSaved;
                        });

                        if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                            widget.SetDefaultUIWidgetActionState(widgetActionState);
                    }

                    if (!AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                        break;
                }
            }
            else
            {
                callbackResults.results = "Widgets Null.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void SaveFolderWidget(AppData.Folder folder, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (DirectoryFound(currentFolder.storageData.directory))
            {
                CreateData(folder, currentFolder.storageData, (folderDataCreated) =>
                {
                    callbackResults.results = folderDataCreated.results;
                    callbackResults.resultsCode = folderDataCreated.resultsCode;

                    if (AppData.Helpers.IsSuccessCode(folderDataCreated.resultsCode))
                    {
                        if (!DirectoryFound(folder.storageData.directory))
                            CreateDirectory(folder.storageData.directory, (folderCreated) => { });
                    }
                    else
                        Debug.LogWarning($"--> Failed To Create DIrectory With Results : {folderDataCreated.results}");
                });
            }
            else
            {
                callbackResults.results = $"Directory : {currentFolder.storageData.directory} Doesn't Exist.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void SaveAssetWidget(AppData.SceneAsset sceneAsset, Action<AppData.Callback> callback = null)
        {

        }

        public string GetAssetNameFormatted(string assetName, AppData.SelectableAssetType assetType)
        {
            string assetFormattedName = (assetType == AppData.SelectableAssetType.Folder) ? assetName.Replace("_FolderData", "") : assetName.Replace("_FileData", "");

            return assetFormattedName;
        }

        public void ChangeFolderLayoutView(AppData.LayoutViewType viewType, AppData.SceneDataPackets dataPackets)
        {
            folderStructureData.SetCurrentLayoutViewType(viewType);

            SaveData(folderStructureData, (folderStructureDataSaved) =>
            {
                if (AppData.Helpers.IsSuccessCode(folderStructureDataSaved.resultsCode))
                {
                    if (SelectableManager.Instance != null)
                        SelectableManager.Instance.SmoothTransitionToSelection = false;
                    else
                        Debug.LogWarning("--> OpenUIFolderStructure Failed : SelectableManager.Instance Is Not Yet Initialized.");

                    ScreenUIManager.Instance.Refresh();

                    if (dataPackets.notification.showNotifications)
                        NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);
                }
                else
                    Debug.LogWarning($"--> Save Data Failed With Results : {folderStructureDataSaved.results}");
            });
        }

        public void ChangePaginationView(AppData.PaginationViewType paginationView, AppData.SceneDataPackets dataPackets)
        {
            folderStructureData.SetCurrentPaginationViewType(paginationView);

            SaveData(folderStructureData, (folderStructureDataSaved) =>
            {
                if (AppData.Helpers.IsSuccessCode(folderStructureDataSaved.resultsCode))
                {
                    if (SelectableManager.Instance != null)
                        SelectableManager.Instance.SmoothTransitionToSelection = false;
                    else
                        Debug.LogWarning("--> OpenUIFolderStructure Failed : SelectableManager.Instance Is Not Yet Initialized.");

                    ScreenUIManager.Instance.Refresh();

                    if (dataPackets.notification.showNotifications)
                        NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);
                }
                else
                    Debug.LogWarning($"--> Save Data Failed With Results : {folderStructureDataSaved.results}");
            });
        }

        public void InitializeFolderLayoutView(AppData.LayoutViewType viewType) => GetWidgetsRefreshData().widgetsContainer.SetViewLayout(folderStructureData.GetFolderLayoutView(viewType));

        public AppData.LayoutViewType GetLayoutViewType()
        {
            return GetWidgetsRefreshData().widgetsContainer.GetLayout().viewType;
        }

        public AppData.PaginationViewType GetPaginationViewType()
        {
            return folderStructureData.GetCurrentPaginationViewType();
        }

        public void GetDynamicWidgetsContainer(AppData.ContentContainerType containerType, Action<AppData.CallbackData<DynamicWidgetsContainer>> callback)
        {
            AppData.CallbackData<DynamicWidgetsContainer> callbackResults = new AppData.CallbackData<DynamicWidgetsContainer>();

            if (dynamicWidgetsContainersList.Count > 0)
            {
                DynamicWidgetsContainer container = dynamicWidgetsContainersList.Find(container => container.GetContentContainerType() == containerType);

                if (container != null)
                {
                    callbackResults.results = "Success";
                    callbackResults.data = container;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Failed : Container Of Tye : {containerType} Not Found In dynamicWidgetsContainersList.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "Failed : dynamicWidgetsContainersList Is Null / Empty.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public AppData.FolderStructureType GetCurrentViewedFolderStructure()
        {
            return currentViewedFolderStructure;
        }

        public AppData.Folder GetMainFolder()
        {
            return folderStructureData.rootFolder;
        }

        public List<AppData.Folder> GetFolders()
        {
            return folders;
        }

        #region On Create Functions

        #region Create UI

        public void CreateUIScreenProjectSelectionWidgets(AppData.UIScreenType screenType, List<AppData.FolderStructureData> projectData, DynamicWidgetsContainer contentContainer, Action<AppData.CallbackDatas<AppData.UIScreenWidget>> callback)
        {
            try
            {
                AppData.CallbackDatas<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDatas<AppData.UIScreenWidget>();

                if (contentContainer != null && contentContainer.IsContainerActive())
                {
                    contentContainer.InitializeContainer();

                    if(screenType == AppData.UIScreenType.ProjectSelectionScreen)
                    {
                        GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                        {
                            if (widgetsCallback.Success())
                            {
                                var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                if (widgetPrefabData != null)
                                {
                                    widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableAssetType.Project, AppData.LayoutViewType.ListView, prefabCallbackResults =>
                                    {
                                        if (prefabCallbackResults.Success())
                                        {
                                            foreach (var project in projectData)
                                            {
                                                GameObject projectWidget = Instantiate(prefabCallbackResults.data.gameObject);

                                                if (projectWidget != null)
                                                {
                                                    AppData.UIScreenWidget widgetComponent = projectWidget.GetComponent<AppData.UIScreenWidget>();

                                                    if (widgetComponent != null)
                                                    {
                                                        //if (folderStructureData.currentPaginationViewType == AppData.PaginationViewType.Pager)
                                                        //    widgetComponent.Hide();


                                                        widgetComponent.SetFolderData(project.rootFolder);

                                                        projectWidget.name = project.name;
                                                        contentContainer.AddDynamicWidget(widgetComponent, contentContainer.GetContainerOrientation(), false);
                                                    }
                                                    else
                                                        LogError("Project Widget Component Is Null.", this);
                                                }
                                                else
                                                    LogError("Project Widget Prefab Data Is Null.", this);
                                            }
                                        }
                                        else
                                            Log(prefabCallbackResults.resultsCode, prefabCallbackResults.results, this);
                                    });
                                }
                                else
                                    LogError("Widget Prefab Data Missing.", this);
                            }
                        });
                    }
                }
                else
                {
                    callbackResults.results = "Dynamic Widgets Content Container Missing / Null.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void CreateNewFolderStructureData(Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>> callback)
        {
            try
            {
                AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>();

                string newFolderDataFileName = !string.IsNullOrEmpty(CreateNewFolderName) ? CreateNewFolderName : GetCreateNewFolderTempName();

                DirectoryFound(GetCurrentFolder().storageData.directory, currentDirectoryFoundCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(currentDirectoryFoundCallback.resultsCode))
                    {
                    #region Get Folder File Storage Data

                    // Folder Storage Info
                    string newFolderDirectoryInfo = Path.Combine(GetCurrentFolder().storageData.directory, newFolderDataFileName);
                        string newFolderDirectory = newFolderDirectoryInfo.Replace("\\", "/");

                    // Folder Storage File Path

                    // Folder File Storage Data
                    string newFolderFileDataName = GetFileDataNameWithoutExtension(newFolderDataFileName, AppData.SelectableAssetType.Folder);
                        string newStorageDataName = GetFormattedName(newFolderFileDataName, AppData.SelectableAssetType.Folder, true);

                        string newFolderFileDataDirectoryInfo = Path.Combine(GetCurrentFolder().storageData.directory, GetFileDataNameWithExtension(newFolderDataFileName, AppData.SelectableAssetType.Folder));
                        string newFolderFileDataDirectory = newFolderFileDataDirectoryInfo.Replace("\\", "/");

                        AppData.StorageDirectoryData newFolderFileDataStorageData = new AppData.StorageDirectoryData
                        {
                            name = newFolderFileDataName,
                            directory = newFolderDirectory
                        };

                    #endregion

                    #region Create New Folder File Data

                    AppData.Folder newFolderFileData = new AppData.Folder()
                        {
                            name = newStorageDataName,
                            storageData = newFolderFileDataStorageData
                        };

                        CreateData(newFolderFileData, currentFolder.storageData, (folderDataCreated) =>
                        {
                            callbackResults.results = folderDataCreated.results;
                            callbackResults.resultsCode = folderDataCreated.resultsCode;

                            if (AppData.Helpers.IsSuccessCode(folderDataCreated.resultsCode))
                            {
                                CreateDirectory(newFolderDirectory, (folderCreated) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(folderCreated.resultsCode))
                                    {
                                        if (SelectableManager.Instance != null)
                                        {
                                            if (SelectableManager.Instance.HasActiveSelection())
                                            {
                                                SelectableManager.Instance.OnClearFocusedSelectionsInfo(selectionInfoCleared =>
                                                {
                                                    if (AppData.Helpers.IsSuccessCode(selectionInfoCleared.resultsCode))
                                                    {
                                                        AppData.FocusedSelectionInfo<AppData.SceneDataPackets> selectionInfo = new AppData.FocusedSelectionInfo<AppData.SceneDataPackets>
                                                        {
                                                            name = newFolderDataFileName,
                                                            selectionInfoType = AppData.FocusedSelectionType.NewItem
                                                        };

                                                        callbackResults.results = $"Set Highlighted Folder To Widget Named : {newFolderDataFileName} Success.";
                                                        callbackResults.data = selectionInfo;
                                                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                                    }
                                                    else
                                                    {
                                                        callbackResults.results = selectionInfoCleared.results;
                                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                        callbackResults.data = default;
                                                    }
                                                });
                                            }
                                            else
                                            {
                                                AppData.FocusedSelectionInfo<AppData.SceneDataPackets> selectionInfo = new AppData.FocusedSelectionInfo<AppData.SceneDataPackets>
                                                {
                                                    name = newFolderDataFileName,
                                                    selectionInfoType = AppData.FocusedSelectionType.NewItem
                                                };

                                                callbackResults.results = $"Set Highlighted Folder To Widget Named : {newFolderDataFileName} Success.";
                                                callbackResults.data = selectionInfo;
                                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                            }
                                        }
                                        else
                                        {
                                            callbackResults.results = "Selectable Manager Instance Not Yet Initialized.";
                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                            callbackResults.data = default;
                                        }
                                    }
                                    else
                                    {
                                        callbackResults.results = $"Failed To Create DIrectory With Results : {folderCreated.results}";
                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                        callbackResults.data = default;
                                    }
                                });
                            }
                            else
                            {
                                callbackResults.data = default;
                                Debug.LogWarning($"--> Create Folder Data Failed With Results : {folderDataCreated.results}");
                            }
                        });

                    #endregion
                }
                    else
                    {
                        callbackResults.results = currentDirectoryFoundCallback.results;
                        callbackResults.data = default;
                        callbackResults.resultsCode = currentDirectoryFoundCallback.resultsCode;
                    }
                });

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void CreateUIScreenFolderWidgets(AppData.UIScreenType screenType, AppData.Folder folder, DynamicWidgetsContainer contentContainer, Action<AppData.CallbackDatas<AppData.UIScreenWidget>> callback)
        {
            try
            {
                AppData.CallbackDatas<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDatas<AppData.UIScreenWidget>();

                if (contentContainer != null && contentContainer.IsContainerActive())
                {
                    contentContainer.InitializeContainer();

                    switch (screenType)
                    {
                        case AppData.UIScreenType.ProjectViewScreen:

                            LoadFolderData(folder, (foldersLoaded) =>
                            {
                                List<AppData.UIScreenWidget> loadedWidgetsList = new List<AppData.UIScreenWidget>();

                                if (AppData.Helpers.IsSuccessCode(foldersLoaded.resultsCode))
                                {
                                    List<AppData.Folder> pinnedFolders = new List<AppData.Folder>();

                                    foreach (var folder in foldersLoaded.data)
                                        if (folder.defaultWidgetActionState == AppData.DefaultUIWidgetActionState.Pinned)
                                            pinnedFolders.Add(folder);

                                    GetSortedWidgetList(foldersLoaded.data, pinnedFolders, sortedList =>
                                    {
                                        if (AppData.Helpers.IsSuccessCode(sortedList.resultsCode))
                                        {
                                            GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                                            {
                                                if (widgetsCallback.Success())
                                                {
                                                    var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                                    if (widgetPrefabData != null)
                                                    {
                                                        widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableAssetType.Folder, folderStructureData.GetCurrentLayoutViewType(), prefabCallbackResults =>
                                                        {
                                                            if (prefabCallbackResults.Success())
                                                            {
                                                                pinnedFolders = sortedList.data;

                                                                foreach (var folder in pinnedFolders)
                                                                {
                                                                    GameObject folderWidget = Instantiate(prefabCallbackResults.data.gameObject);

                                                                    if (folderWidget != null)
                                                                    {
                                                                        AppData.UIScreenWidget widgetComponent = folderWidget.GetComponent<AppData.UIScreenWidget>();

                                                                        if (widgetComponent != null)
                                                                        {
                                                                            widgetComponent.SetDefaultUIWidgetActionState(folder.defaultWidgetActionState);

                                                                            if (folderStructureData.currentPaginationViewType == AppData.PaginationViewType.Pager)
                                                                                widgetComponent.Hide();

                                                                            folderWidget.name = folder.name;
                                                                            widgetComponent.SetFolderData(folder);
                                                                            contentContainer.AddDynamicWidget(widgetComponent, contentContainer.GetContainerOrientation(), false);
                                                                        }

                                                                        if (!loadedWidgetsList.Contains(widgetComponent))
                                                                            loadedWidgetsList.Add(widgetComponent);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                Log(prefabCallbackResults.resultsCode, prefabCallbackResults.results, this);
                                                        });
                                                    }
                                                    else
                                                        LogError("Widget Prefab Data Missing.", this);
                                                }
                                            });
                                        }
                                        else
                                            LogWarning(sortedList.results, this);
                                    });
                                }
                                else
                                    LogWarning(foldersLoaded.results, this);

                                if (loadedWidgetsList.Count > 0)
                                    callbackResults.data = loadedWidgetsList;
                                else
                                    callbackResults.data = default;

                                callbackResults.results = foldersLoaded.results;
                                callbackResults.resultsCode = foldersLoaded.resultsCode;
                            });

                            break;
                    }
                }
                else
                {
                    callbackResults.results = "Dynamic Widgets Content Container Missing / Null.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void CreateUIScreenFolderWidgets(AppData.UIScreenType screenType, List<AppData.StorageDirectoryData> foldersDirectoryList, DynamicWidgetsContainer contentContainer, Action<AppData.CallbackDatas<AppData.UIScreenWidget>> callback)
        {
            try
            {
                AppData.CallbackDatas<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDatas<AppData.UIScreenWidget>();

                if (contentContainer != null && contentContainer.IsContainerActive())
                {
                    contentContainer.InitializeContainer();

                    switch (screenType)
                    {
                        case AppData.UIScreenType.ProjectViewScreen:

                            LoadFolderData(foldersDirectoryList, (foldersLoaded) =>
                            {
                                if (AppData.Helpers.IsSuccessCode(foldersLoaded.resultsCode))
                                {
                                    List<AppData.UIScreenWidget> loadedWidgetsList = new List<AppData.UIScreenWidget>();

                                    List<AppData.Folder> pinnedFolders = new List<AppData.Folder>();

                                    foreach (var folder in foldersLoaded.data)
                                        if (folder.defaultWidgetActionState == AppData.DefaultUIWidgetActionState.Pinned)
                                            pinnedFolders.Add(folder);

                                    GetSortedWidgetList(foldersLoaded.data, pinnedFolders, sortedList =>
                                    {
                                        if (AppData.Helpers.IsSuccessCode(sortedList.resultsCode))
                                        {
                                            GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                                            {
                                                if (widgetsCallback.Success())
                                                {
                                                    var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                                    if (widgetPrefabData != null)
                                                    {
                                                        widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableAssetType.Folder, folderStructureData.GetCurrentLayoutViewType(), prefabCallbackResults =>
                                                        {
                                                            if (prefabCallbackResults.Success())
                                                            {
                                                                pinnedFolders = sortedList.data;

                                                                foreach (var folder in pinnedFolders)
                                                                {
                                                                    GameObject folderWidget = Instantiate(prefabCallbackResults.data.gameObject);

                                                                    if (folderWidget != null)
                                                                    {
                                                                        AppData.UIScreenWidget widgetComponent = folderWidget.GetComponent<AppData.UIScreenWidget>();

                                                                        if (widgetComponent != null)
                                                                        {
                                                                            widgetComponent.SetDefaultUIWidgetActionState(folder.defaultWidgetActionState);

                                                                            if (folderStructureData.currentPaginationViewType == AppData.PaginationViewType.Pager)
                                                                                widgetComponent.Hide();

                                                                            folderWidget.name = folder.name;
                                                                            widgetComponent.SetFolderData(folder);
                                                                            contentContainer.AddDynamicWidget(widgetComponent, contentContainer.GetContainerOrientation(), false);
                                                                        }

                                                                        if (!loadedWidgetsList.Contains(widgetComponent))
                                                                            loadedWidgetsList.Add(widgetComponent);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                Log(prefabCallbackResults.resultsCode, prefabCallbackResults.results, this);
                                                        });
                                                    }
                                                    else
                                                        LogError("Widget Prefab Data Missing.", this);
                                                }
                                            });
                                        }
                                        else
                                            Debug.LogWarning($"--> GetSortedWidgetList Failed With Results : {sortedList.results}");
                                    });

                                    if (loadedWidgetsList.Count > 0)
                                        callbackResults.data = loadedWidgetsList;
                                    else
                                        callbackResults.data = default;
                                }
                                else
                                    Debug.LogWarning($"--> LoadFolderData Failed With Results : {foldersLoaded.results}");

                                callbackResults.results = foldersLoaded.results;
                                callbackResults.resultsCode = foldersLoaded.resultsCode;
                            });

                            break;
                    }
                }
                else
                {
                    callbackResults.results = "Dynamic Widgets Content Container Missing / Null.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void CreateUIScreenFileWidgets(AppData.UIScreenType screenType, AppData.Folder folder, DynamicWidgetsContainer contentContainer, Action<AppData.CallbackDatas<AppData.UIScreenWidget>> callback)
        {
            try
            {
                AppData.CallbackDatas<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDatas<AppData.UIScreenWidget>();

                if (contentContainer != null && contentContainer.IsContainerActive())
                {
                    switch (screenType)
                    {
                        case AppData.UIScreenType.ProjectViewScreen:

                            LoadSceneAssets(folder, (loadedAssetsResults) =>
                            {
                                if (AppData.Helpers.IsSuccessCode(loadedAssetsResults.resultsCode))
                                {
                                    sceneAssetList = new List<AppData.SceneAsset>();

                                    if (loadedAssetsResults.data.Count > 0)
                                    {
                                        List<AppData.UIScreenWidget> loadedWidgetsList = new List<AppData.UIScreenWidget>();

                                        foreach (AppData.SceneAsset asset in loadedAssetsResults.data)
                                        {
                                            if (!sceneAssetList.Contains(asset))
                                            {
                                                GameObject newWidget = Instantiate((folderStructureData.GetCurrentLayoutViewType() == AppData.LayoutViewType.ListView) ? fileListViewHandlerPrefab.GetSceneAssetObject() : fileItemViewHandlerPrefab.GetSceneAssetObject());

                                                if (newWidget != null)
                                                {
                                                    AppData.UIScreenWidget widgetComponent = newWidget.GetComponent<AppData.UIScreenWidget>();

                                                    if (widgetComponent != null)
                                                    {
                                                        widgetComponent.SetDefaultUIWidgetActionState(asset.defaultWidgetActionState);

                                                        if (folderStructureData.currentPaginationViewType == AppData.PaginationViewType.Pager)
                                                            widgetComponent.Hide();

                                                        newWidget.name = asset.name;

                                                        widgetComponent.SetSceneAssetData(asset);
                                                        widgetComponent.SetWidgetParentScreen(ScreenUIManager.Instance.GetCurrentScreenData().value);
                                                        widgetComponent.SetWidgetAssetData(asset);

                                                        contentContainer.AddDynamicWidget(widgetComponent, contentContainer.GetContainerOrientation(), false);

                                                        sceneAssetList.Add(asset);

                                                        AppData.SceneAssetWidget assetWidget = new AppData.SceneAssetWidget();
                                                        assetWidget.name = widgetComponent.GetSceneAssetData().name;
                                                        assetWidget.value = newWidget;
                                                        assetWidget.categoryType = widgetComponent.GetSceneAssetData().categoryType;
                                                        assetWidget.creationDateTime = widgetComponent.GetSceneAssetData().creationDateTime;

                                                        screenWidgetList.Add(assetWidget);

                                                        widgetComponent.SetFileData();

                                                        if (!loadedWidgetsList.Contains(widgetComponent))
                                                            loadedWidgetsList.Add(widgetComponent);
                                                    }
                                                    else
                                                        Debug.LogWarning($"--> Failed To Load Screen Widget : {asset.modelAsset.name}");
                                                }
                                                else
                                                    Debug.LogWarning($"--> Failed To Instantiate Prefab For Screen Widget : {asset.modelAsset.name}");
                                            }
                                            else
                                                Debug.LogWarning($"--> Widget : {asset.modelAsset.name} Already Exists.");
                                        }

                                        if (loadedWidgetsList.Count >= loadedAssetsResults.data.Count)
                                        {
                                            callbackResults.results = "Created Screen Widgets";
                                            callbackResults.data = loadedWidgetsList;
                                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;

                                        // Take A Look
                                        contentContainer.InitializeContainer();
                                        }
                                        else
                                        {
                                            callbackResults.results = "Failed To Create Screen Widgets";
                                            callbackResults.data = default;
                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                        }
                                    }
                                    else
                                    {
                                        callbackResults.results = "Failed To Create Screen Widgets";
                                        callbackResults.data = default;
                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                    }
                                }
                                else
                                {
                                    callbackResults.results = loadedAssetsResults.results;
                                    callbackResults.data = default;
                                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                }

                            });

                            break;
                    }
                }
                else
                {
                    callbackResults.results = "Dynamic Widgets Content Container Missing / Null.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void CreateUIScreenFileWidgets(AppData.UIScreenType screenType, List<AppData.StorageDirectoryData> filesDirectoryList, DynamicWidgetsContainer contentContainer, Action<AppData.CallbackDatas<AppData.UIScreenWidget>> callback)
        {
            try
            {
                AppData.CallbackDatas<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDatas<AppData.UIScreenWidget>();

                if (contentContainer != null && contentContainer.IsContainerActive())
                {
                    switch (screenType)
                    {
                        case AppData.UIScreenType.ProjectViewScreen:

                            LoadSceneAssets(filesDirectoryList, (loadedAssetsResults) =>
                            {
                                if (AppData.Helpers.IsSuccessCode(loadedAssetsResults.resultsCode))
                                {
                                    sceneAssetList = new List<AppData.SceneAsset>();

                                    if (loadedAssetsResults.data.Count > 0)
                                    {
                                        List<AppData.UIScreenWidget> loadedWidgetsList = new List<AppData.UIScreenWidget>();

                                        foreach (AppData.SceneAsset asset in loadedAssetsResults.data)
                                        {
                                            if (!sceneAssetList.Contains(asset))
                                            {
                                                GameObject newWidget = Instantiate((folderStructureData.GetCurrentLayoutViewType() == AppData.LayoutViewType.ListView) ? fileListViewHandlerPrefab.GetSceneAssetObject() : fileItemViewHandlerPrefab.GetSceneAssetObject());

                                                if (newWidget != null)
                                                {
                                                    AppData.UIScreenWidget widgetComponent = newWidget.GetComponent<AppData.UIScreenWidget>();

                                                    if (widgetComponent != null)
                                                    {
                                                        widgetComponent.SetDefaultUIWidgetActionState(asset.defaultWidgetActionState);

                                                        if (folderStructureData.currentPaginationViewType == AppData.PaginationViewType.Pager)
                                                            widgetComponent.Hide();

                                                        newWidget.name = asset.name;

                                                        widgetComponent.SetSceneAssetData(asset);
                                                        widgetComponent.SetWidgetParentScreen(ScreenUIManager.Instance.GetCurrentScreenData().value);
                                                        widgetComponent.SetWidgetAssetData(asset);

                                                        contentContainer.AddDynamicWidget(widgetComponent, contentContainer.GetContainerOrientation(), false);

                                                        sceneAssetList.Add(asset);

                                                        AppData.SceneAssetWidget assetWidget = new AppData.SceneAssetWidget();
                                                        assetWidget.name = widgetComponent.GetSceneAssetData().name;
                                                        assetWidget.value = newWidget;
                                                        assetWidget.categoryType = widgetComponent.GetSceneAssetData().categoryType;
                                                        assetWidget.creationDateTime = widgetComponent.GetSceneAssetData().creationDateTime;

                                                        screenWidgetList.Add(assetWidget);

                                                        widgetComponent.SetFileData();

                                                        if (!loadedWidgetsList.Contains(widgetComponent))
                                                            loadedWidgetsList.Add(widgetComponent);
                                                    }
                                                    else
                                                        Debug.LogWarning($"--> Failed To Load Screen Widget : {asset.modelAsset.name}");
                                                }
                                                else
                                                    Debug.LogWarning($"--> Failed To Instantiate Prefab For Screen Widget : {asset.modelAsset.name}");
                                            }
                                            else
                                                Debug.LogWarning($"--> Widget : {asset.modelAsset.name} Already Exists.");
                                        }

                                        if (loadedWidgetsList.Count >= loadedAssetsResults.data.Count)
                                        {
                                            callbackResults.results = "Created Screen Widgets";
                                            callbackResults.data = loadedWidgetsList;
                                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;

                                        // Take A Look
                                        contentContainer.InitializeContainer();
                                        }
                                        else
                                        {
                                            callbackResults.results = "Failed To Create Screen Widgets";
                                            callbackResults.data = default;
                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                        }
                                    }
                                    else
                                    {
                                        callbackResults.results = "Failed To Create Screen Widgets";
                                        callbackResults.data = default;
                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                    }
                                }
                                else
                                {
                                    callbackResults.results = loadedAssetsResults.results;
                                    callbackResults.data = default;
                                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                }

                            });

                            break;
                    }
                }
                else
                {
                    callbackResults.results = "Dynamic Widgets Content Container Missing / Null.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        #endregion

        #region Create Data

        public void CreateNewProjectData(AppData.FolderStructureData newProjectStructureData, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            callbackResults.results = $"New Project Name {newProjectStructureData.name} Has Been Created.";

            callback?.Invoke(callbackResults);
        }

        public void CreateDirectory(AppData.StorageDirectoryData directoryData, Action<AppData.CallbackData<AppData.StorageDirectoryData>> callback)
        {
            try
            {
                AppData.CallbackData<AppData.StorageDirectoryData> callbackResults = new AppData.CallbackData<AppData.StorageDirectoryData>();

                if (Application.platform == RuntimePlatform.Android)
                {
                    AndroidJavaClass jc = new AndroidJavaClass("com.redicalgames.designar.OverrideUnityActivity");
                    AndroidJavaObject overrideActivity = jc.GetStatic<AndroidJavaObject>("instance");

                    if (overrideActivity != null)
                        directoryData.directory = overrideActivity.Call<string>("GetAppDataDirectory", directoryData.directory);
                    else
                        Debug.LogWarning("--> RG_Unity - Asset Import Content Manager Referenced Plugin Instance Is Null.");

                    //if (AssetImportContentManager.Instance.GetReferencedPluginInstance() != null)
                    //    directoryData.directory = AssetImportContentManager.Instance.GetReferencedPluginInstance().Call<string>("GetAppDataDirectory", directoryData.directory);
                    //else
                    //    Debug.LogWarning("--> RG_Unity - Asset Import Content Manager Referenced Plugin Instance Is Null.");

                    if (Directory.Exists(directoryData.directory))
                    {
                        if (!appDirectories.Contains(directoryData))
                            appDirectories.Add(directoryData);

                        callbackResults.results = "Success : Directory Exists.";
                        callbackResults.data = directoryData;
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"--> Failed To Create Directory : {directoryData.directory}";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    Directory.CreateDirectory(directoryData.directory);

                    if (Directory.Exists(directoryData.directory))
                    {
                        if (!appDirectories.Contains(directoryData))
                            appDirectories.Add(directoryData);

                        callbackResults.results = "Success : Directory Exists.";
                        callbackResults.data = directoryData;
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"--> Failed To Create Directory : {directoryData.directory}";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void CreateDirectory(string directory, Action<AppData.CallbackData<string>> callback)
        {
            try
            {
                AppData.CallbackData<string> callbackResults = new AppData.CallbackData<string>();

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);

                    if (Directory.Exists(directory))
                    {
                        callbackResults.results = $"Create Directory - Success : {directory}.";
                        callbackResults.data = directory;
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"Create Directory - Failed To Create Directory : {directory}.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"Create Directory Failed : Directory : {directory} Already Exists.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public bool CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                    if (Directory.Exists(path))
                        return true;
                    else
                    {
                        Debug.LogWarning($"--> Failed To Create Directory : {path}");
                        return false;
                    }
                }
                else
                    return true;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        #endregion

        #endregion

        public AppData.StorageDirectoryData GetAppDirectory(AppData.DirectoryType directoryType)
        {
            try
            {
                AppData.StorageDirectoryData directoryData = new AppData.StorageDirectoryData();

                if (appDirectories != null)
                {
                    foreach (AppData.StorageDirectoryData directory in appDirectories)
                    {
                        if (directory.type == directoryType)
                        {
                            directoryData = directory;

                            break;
                        }
                    }
                }
                else
                    Debug.LogWarning("--> App Directories Are Null.");

                return directoryData;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public bool DirectoryFound(AppData.DirectoryType directoryType)
        {
            bool directoryExists = false;

            if (appDirectories.Count > 0)
            {
                foreach (var directoryData in appDirectories)
                {
                    if (directoryData.type == directoryType)
                    {
                        if (Directory.Exists(directoryData.directory))
                        {
                            directoryExists = true;
                        }
                        else
                            directoryExists = false;

                        break;
                    }
                }
            }

            return directoryExists;
        }

        public bool DirectoryFound(AppData.StorageDirectoryData directoryData)
        {
            try
            {
                bool directoryExists = false;

                if (appDirectories.Count > 0)
                {
                    foreach (var appDirectory in appDirectories)
                    {
                        if (appDirectory.type == directoryData.type)
                        {
                            if (Directory.Exists(appDirectory.directory))
                            {
                                directoryExists = true;
                            }
                            else
                                directoryExists = false;

                            break;
                        }
                    }
                }

                return directoryExists;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public bool DirectoryFound(string directory)
        {
            try
            { 
                bool directoryExists = false;

                if (Directory.Exists(directory))
                {
                    directoryExists = true;
                }
                else
                    directoryExists = false;

                return directoryExists;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void DirectoryFound(string directory, Action<AppData.Callback> callback)
        {
            try
            {
                AppData.Callback callbackResults = new AppData.Callback();

                if (Directory.Exists(directory))
                {
                    callbackResults.results = $"Directory Found At Path : {directory}.";
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Directory : {directory} Not Found.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void FileFound(string path, Action<AppData.Callback> callback)
        {
            try
            {
                AppData.Callback callbackResults = new AppData.Callback();

                if (File.Exists(path))
                {
                    callbackResults.results = $"File Found At Path : {path}.";
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"File : {path} Not Found.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void GetFolderContentCount(AppData.Folder folder, Action<AppData.CallbackData<int>> callback)
        {
            try
            {
                AppData.CallbackData<int> callbackResults = new AppData.CallbackData<int>();

                if (!string.IsNullOrEmpty(folder.storageData.directory))
                {
                    if (DirectoryFound(folder.storageData.directory))
                    {
                        string[] files = Directory.GetFiles(folder.storageData.directory);

                        if (files.Length > 0)
                        {
                            List<string> validFiles = new List<string>();

                            for (int i = 0; i < files.Length; i++)
                                if (files[i].Contains(".json") && !files[i].Contains(".meta"))
                                    validFiles.Add(files[i]);

                            if (validFiles.Count > 0)
                            {
                                callbackResults.results = $"GetFolderContentCount Success - Directory : {folder.storageData.directory} Found.";
                                callbackResults.data = validFiles.Count;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = $"GetFolderContentCount Failed - There Were No Valid Files Found In Directory : {folder.storageData.directory}.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = $"GetFolderContentCount Failed - There Were No Files Found In Directory : {folder.storageData.directory}.";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"GetFolderContentCount Failed - Directory : {folder.storageData.directory} Not Found.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"GetFolderContentCount Failed - Directory Is Null / Empty..";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public string GetFormattedName(string source, AppData.SelectableAssetType assetType, bool isDisplayName = true, int length = 0)
        {
            try
            {
                string formattedName = string.Empty;

                if (isDisplayName)
                {
                    bool isValid = (assetType == AppData.SelectableAssetType.Folder) ? !source.Contains("_FolderData") : !source.Contains("_FileData");

                    if (isValid)
                        formattedName = source;
                    else
                        formattedName = (assetType == AppData.SelectableAssetType.Folder) ? source.Replace("_FolderData", "") : source.Replace("_FileData", "");
                }
                else
                {
                    if (source.Contains("_FolderData") || source.Contains("_FileData"))
                        formattedName = source;
                    else
                        formattedName = (assetType == AppData.SelectableAssetType.Folder) ? source + "_FolderData" : source + "_FileData";
                }

                return formattedName;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void SetWidgetsRefreshData(AppData.Folder folder, DynamicWidgetsContainer widgetsContainer)
        {

            widgetsRefreshFolder = folder;

            if (widgetsContainer != null)
                widgetsRefreshDynamicContainer = widgetsContainer;
        }

        public (AppData.Folder folder, DynamicWidgetsContainer widgetsContainer) GetWidgetsRefreshData()
        {
            try
            {
                return (widgetsRefreshFolder, widgetsRefreshDynamicContainer);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        #region On Refresh Functions

        public void Refresh(AppData.Folder folder, DynamicWidgetsContainer widgetsContainer, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            LoadFolderStuctureData((structureLoader) =>
            {
                if (AppData.Helpers.IsSuccessCode(structureLoader.resultsCode))
                {
                    Debug.LogError($"==> On Refresh Folder : {folder.name} - Directory : {folder.GetDirectoryData().directory} ");
                }
                else
                    Debug.LogError($"==> Folder Structure Failed To Load With Results : {structureLoader.results}");

                callbackResults.results = structureLoader.results;
                callbackResults.resultsCode = structureLoader.resultsCode;
            });

            callback?.Invoke(callbackResults);
        }

        public bool Refreshed(AppData.Folder folder, DynamicWidgetsContainer widgetsContainer, AppData.SceneDataPackets dataPackets)
        {
            try
            {
                bool isRefreshed = false;

                if (ScreenUIManager.Instance != null)
                {
                    widgetsContainer.SetAssetsLoaded(isRefreshed);

                    if (dataPackets.refreshScreenOnLoad)
                    {
                        switch (dataPackets.screenType)
                        {
                            case AppData.UIScreenType.ProjectSelectionScreen:

                                widgetsContainer.ClearWidgets(widgetsClearedCallback =>
                                {
                                    if (widgetsClearedCallback.Success())
                                    {
                                        LoadProjectStructureData((structureLoader) =>
                                        {
                                            if (AppData.Helpers.IsSuccessCode(structureLoader.resultsCode))
                                            {
                                                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == AppData.UIScreenType.ProjectSelectionScreen)
                                                {
                                                    CreateUIScreenProjectSelectionWidgets(ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType(), structureLoader.data, widgetsContainer, createProjectWidgetCallback =>
                                                    {
                                                        Log(createProjectWidgetCallback.resultsCode, createProjectWidgetCallback.results, this);
                                                    });
                                                }
                                                else
                                                    LogError($"--> Folder Structure Screen : {ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType()}", this);
                                            }
                                            else
                                                LogError($"--> Folder Structure Failed To Load With Results : {structureLoader.results}", this);
                                        });


                                        isRefreshed = true;
                                    }
                                    else
                                        Log(widgetsClearedCallback.resultsCode, widgetsClearedCallback.results, this);
                                });

                                break;

                            case AppData.UIScreenType.ProjectViewScreen:

                                LogError($"=========> Project Container : {widgetsContainer.name}");

                                widgetsContainer.ClearWidgets(widgetsClearedCallback =>
                                {
                                    if (widgetsClearedCallback.Success())
                                    {
                                        LoadFolderStuctureData((structureLoader) =>
                                        {
                                            if (AppData.Helpers.IsSuccessCode(structureLoader.resultsCode))
                                            {
                                                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == AppData.UIScreenType.ProjectViewScreen)
                                                {
                                                    SetCurrentFolder(folder);

                                                    GetWidgetsRefreshData().widgetsContainer.SetViewLayout(folderStructureData.GetFolderLayoutView(folderStructureData.GetCurrentLayoutViewType()));

                                                    RefreshLayoutViewButtonIcon();

                                                #region Pegination

                                                OnPaginationViewRefreshed(widgetsContainer);

                                                #endregion

                                                loadedWidgets = new List<AppData.UIScreenWidget>();

                                                    int contentCount = 0;

                                                    CreateUIScreenFolderWidgets(ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType(), folder, widgetsContainer, (widgetsCreated) =>
                                                    {
                                                    // Get Loaded Widgets
                                                    if (AppData.Helpers.IsSuccessCode(widgetsCreated.resultsCode))
                                                        {
                                                            contentCount += widgetsCreated.data.Count;

                                                            if (widgetsCreated.data != null)
                                                                if (widgetsCreated.data.Count > 0)
                                                                    foreach (var widget in widgetsCreated.data)
                                                                        if (!loadedWidgets.Contains(widget))
                                                                            loadedWidgets.Add(widget);
                                                        }
                                                    });

                                                    CreateUIScreenFileWidgets(ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType(), folder, widgetsContainer, (widgetsCreated) =>
                                                    {
                                                    // Get Loaded Widgets
                                                    if (AppData.Helpers.IsSuccessCode(widgetsCreated.resultsCode))
                                                        {
                                                            contentCount += widgetsCreated.data.Count;

                                                            if (widgetsCreated.data != null)
                                                                if (widgetsCreated.data.Count > 0)
                                                                    foreach (var widget in widgetsCreated.data)
                                                                        if (!loadedWidgets.Contains(widget))
                                                                            loadedWidgets.Add(widget);
                                                        }
                                                    });

                                                    if (loadedWidgets.Count > 0)
                                                    {
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.UITextDisplayerWidget);
                                                        SelectableManager.Instance.AddSelectables(loadedWidgets);
                                                    }
                                                    else
                                                        StartCoroutine(RefreshAssetsAsync());

                                                    if (widgetsContainer.GetContentCount() == 0)
                                                    {
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.InputUIState.Disabled);
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Disabled);

                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputUIState.Disabled);
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldState(AppData.InputFieldActionType.AssetSearchField, AppData.InputUIState.Disabled);
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldPlaceHolderText(AppData.InputFieldActionType.AssetSearchField, string.Empty);
                                                    }
                                                    else
                                                    {
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.InputUIState.Enabled);
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Enabled);

                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputUIState.Enabled);
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldState(AppData.InputFieldActionType.AssetSearchField, AppData.InputUIState.Enabled);
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldPlaceHolderText(AppData.InputFieldActionType.AssetSearchField, folderStructureSearchFieldPlaceHolderTextValue);
                                                    }

                                                    widgetsContainer.GetUIScroller().ScrollToBottom();
                                                    isRefreshed = loadedWidgets.Count > 0;

                                                    if (isRefreshed)
                                                    {
                                                        AppData.UIImageType selectionOptionImageViewType = AppData.UIImageType.Null_TransparentIcon;

                                                        switch (GetLayoutViewType())
                                                        {
                                                            case AppData.LayoutViewType.ItemView:

                                                                if (SelectableManager.Instance.HasActiveSelection())
                                                                {
                                                                    widgetsContainer.HasAllWidgetsSelected(selectedAllCallback =>
                                                                    {
                                                                        if (selectedAllCallback.Success())
                                                                            selectionOptionImageViewType = AppData.UIImageType.ItemViewDeselectionIcon;
                                                                        else
                                                                            selectionOptionImageViewType = AppData.UIImageType.ItemViewSelectionIcon;
                                                                    });
                                                                }
                                                                else
                                                                    selectionOptionImageViewType = AppData.UIImageType.ItemViewSelectionIcon;

                                                                break;

                                                            case AppData.LayoutViewType.ListView:

                                                                if (SelectableManager.Instance.HasActiveSelection())
                                                                {
                                                                    widgetsContainer.HasAllWidgetsSelected(selectedAllCallback =>
                                                                    {
                                                                        if (selectedAllCallback.Success())
                                                                            selectionOptionImageViewType = AppData.UIImageType.ListViewDeselectionIcon;
                                                                        else
                                                                            selectionOptionImageViewType = AppData.UIImageType.ListViewSelectionIcon;
                                                                    });
                                                                }
                                                                else
                                                                    selectionOptionImageViewType = AppData.UIImageType.ListViewSelectionIcon;

                                                                break;
                                                        }

                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.ButtonIcon, selectionOptionImageViewType);
                                                    }
                                                }
                                            }
                                            else
                                                Debug.LogWarning($"--> Folder Structure Failed To Load With Results : {structureLoader.results}");
                                        });
                                    }
                                    else
                                        Log(widgetsClearedCallback.resultsCode, widgetsClearedCallback.results, this);
                                });

                                break;
                        };
                    }
                }
                else
                    LogError("Screen UI Manager Instance Is Not Yet Initialized.", this);

                return isRefreshed;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        IEnumerator RefreshAssetsAsync()
        {
            yield return new WaitForEndOfFrame();

            AppData.SceneDataPackets dataPackets = ScreenNavigationManager.Instance.GetEmptyFolderDataPackets();
            dataPackets.isRootFolder = GetCurrentFolder().IsRootFolder();
            dataPackets.popUpMessage = (dataPackets.isRootFolder) ? "There's No Content Found. Create New" : "Folder Is Empty";

            dataPackets.referencedActionButtonDataList = new List<AppData.ReferencedActionButtonData>()
        {
            new AppData.ReferencedActionButtonData
            {
                title = (dataPackets.isRootFolder)? "Create New" : "Delete",
                type = AppData.InputActionButtonType.FolderActionButton,
                state = AppData.InputUIState.Enabled
            }
        };


            ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);

            #region Disable UI

            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.InputUIState.Disabled);
            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Disabled);

            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputUIState.Disabled);
            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldState(AppData.InputFieldActionType.AssetSearchField, AppData.InputUIState.Disabled);

            #endregion

            ScreenUIManager.Instance.ScreenRefresh();
        }

        void RefreshLayoutViewButtonIcon()
        {
            switch (folderStructureData.GetCurrentLayoutViewType())
            {
                case AppData.LayoutViewType.ItemView:

                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonUIImageValue(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ListViewIcon);

                    break;

                case AppData.LayoutViewType.ListView:

                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonUIImageValue(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ItemViewIcon);

                    break;
            }
        }

        #endregion

        public AppData.ContentContainerType GetContainerType(AppData.UIScreenType screenType)
        {
            AppData.ContentContainerType containerType = AppData.ContentContainerType.None;

            switch(screenType)
            {
                case AppData.UIScreenType.ProjectSelectionScreen:

                    containerType = AppData.ContentContainerType.ProjectSelectionContent;

                    break;

                case AppData.UIScreenType.ProjectViewScreen:

                    containerType = AppData.ContentContainerType.FolderStuctureContent;

                    break;

                case AppData.UIScreenType.AssetCreationScreen:

                    containerType = AppData.ContentContainerType.AssetImport;

                    break;
            }

            return containerType;
        }

        public int GetWidgetsContentCount()
        {
           return GetWidgetsRefreshData().widgetsContainer.GetContentCount();
        }

        public List<AppData.UIScreenWidget> GetLoadedSceneAssetsList()
        {
            return loadedWidgets;
        }

        public void DisableUIOnScreenEnter(AppData.UIScreenType screenType, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            ScreenUIManager.Instance.SetScreenActionButtonState(screenType, AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.InputUIState.Disabled);
            ScreenUIManager.Instance.SetScreenActionButtonState(screenType, AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Disabled);
            ScreenUIManager.Instance.SetScreenActionDropdownState(screenType, AppData.InputUIState.Disabled, dropdownContentPlaceholder);
            ScreenUIManager.Instance.SetScreenActionInputFieldState(screenType, AppData.InputFieldActionType.AssetSearchField, AppData.InputUIState.Disabled);
            ScreenUIManager.Instance.SetScreenActionInputFieldPlaceHolderText(screenType, AppData.InputFieldActionType.AssetSearchField, string.Empty);

            callback?.Invoke(callbackResults);
        }

        void OnPaginationViewRefreshed(DynamicWidgetsContainer widgetsContainer)
        {
            switch (folderStructureData.GetCurrentPaginationViewType())
            {
                case AppData.PaginationViewType.Pager:

                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonUIImageValue(AppData.InputActionButtonType.PaginationButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ScrollerIcon);
                    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.ScrollerNavigationWidget);
                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(ScreenNavigationManager.Instance.GetPagerNavigationWidgetDataPackets());

                    break;

                case AppData.PaginationViewType.Scroller:

                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonUIImageValue(AppData.InputActionButtonType.PaginationButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.PagerIcon);
                    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.PagerNavigationWidget);
                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(ScreenNavigationManager.Instance.GetScrollerNavigationWidgetDataPackets());

                    break;
            }

            widgetsContainer.SetPaginationView(folderStructureData.GetCurrentPaginationViewType());
        }

        #region Content Load

        public void LoadFolderData(AppData.Folder folder, Action<AppData.CallbackDatas<AppData.Folder>> callback)
        {
            AppData.CallbackDatas<AppData.Folder> callbackResults = new AppData.CallbackDatas<AppData.Folder>();

            if (DirectoryFound(folder.storageData.directory))
            {
                List<AppData.Folder> loadedFolders = new List<AppData.Folder>();

                var folderDataList = Directory.GetFiles(folder.storageData.directory, "*_FolderData.json", SearchOption.TopDirectoryOnly).ToList();

                if (folderDataList.Count > 0)
                {
                    foreach (var file in folderDataList)
                    {
                        string JSONString = File.ReadAllText(file);
                        AppData.Folder folderData = JsonUtility.FromJson<AppData.Folder>(JSONString);

                        if (!string.IsNullOrEmpty(folderData.name))
                            loadedFolders.Add(folderData);
                    }

                    if (loadedFolders.Count > 0)
                    {
                        callbackResults.results = "Success";
                        callbackResults.data = loadedFolders;
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"Failed - No Valid Files Loaded In Directory : {folder.storageData.directory} Not Found.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"Failed - No Files Found In Directory : {folder.storageData.directory} Not Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = $"Failed - Directory : {folder.storageData.directory} Not Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void LoadFolderData(List<AppData.StorageDirectoryData> folderDirectoryDataList, Action<AppData.CallbackDatas<AppData.Folder>> callback)
        {
            AppData.CallbackDatas<AppData.Folder> callbackResults = new AppData.CallbackDatas<AppData.Folder>();

            if (folderDirectoryDataList != null && folderDirectoryDataList.Count > 0)
            {
                List<AppData.Folder> loadedFolders = new List<AppData.Folder>();

                foreach (var folderDirectory in folderDirectoryDataList)
                {
                    FileFound(folderDirectory.directory, folderFoundCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(folderFoundCallback.resultsCode))
                        {
                            if (folderDirectory.directory.Contains("_FolderData.json"))
                            {
                                string directory = folderDirectory.directory.Replace("_FolderData.json", "");

                                DirectoryFound(directory, directoryexistCallack =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(directoryexistCallack.resultsCode))
                                    {
                                        string JSONString = File.ReadAllText(folderDirectory.directory);
                                        AppData.Folder folderData = JsonUtility.FromJson<AppData.Folder>(JSONString);

                                        if (!string.IsNullOrEmpty(folderData.name))
                                            loadedFolders.Add(folderData);
                                    }
                                    else
                                    {
                                        callbackResults.results = directoryexistCallack.results;
                                        callbackResults.data = default;
                                        callbackResults.resultsCode = directoryexistCallack.resultsCode;
                                    }

                                });
                            }
                        }
                        else
                        {
                            callbackResults.results = folderFoundCallback.results;
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    });
                }

                if (loadedFolders.Count > 0)
                {
                    callbackResults.results = "Success";
                    callbackResults.data = loadedFolders;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"No Valid Folders Loaded.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "Folder Directory Data List Is Null / Empty";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void LoadSceneAssets(AppData.Folder folder, Action<AppData.CallbackDatas<AppData.SceneAsset>> callback)
        {
            AppData.CallbackDatas<AppData.SceneAsset> callbackResults = new AppData.CallbackDatas<AppData.SceneAsset>();

            DirectoryFound(folder.storageData.directory, foundDirectoryCallback =>
            {
                if (AppData.Helpers.IsSuccessCode(foundDirectoryCallback.resultsCode))
                {
                    List<AppData.SceneAsset> loadedAssetsList = new List<AppData.SceneAsset>();

                    string[] fileDataList = Directory.GetFiles(folder.storageData.directory, "*.json");

                    if (fileDataList.Length > 0)
                    {
                        List<string> validFileList = new List<string>();
                        List<string> fileDataBlackList = new List<string>();

                        foreach (var file in fileDataList)
                        {
                            if (GetFolderStructureData().GetExcludedSystemFolderData() != null)
                            {
                                foreach (var excludedFile in GetFolderStructureData().GetExcludedSystemFolderData())
                                {
                                    if (!file.Contains(excludedFile) && !fileDataBlackList.Contains(file))
                                    {
                                        if (!validFileList.Contains(file))
                                            validFileList.Add(file);
                                    }
                                    else
                                        fileDataBlackList.Add(file);
                                }
                            }
                            else
                                Debug.LogWarning($"==> LoadFolderData's GetExcludedSystemFolders Failed - GetFolderStructureData().GetExcludedSystemFolders() Returned Null.");
                        }

                        if (validFileList.Count > 0)
                        {
                            foreach (var file in validFileList)
                            {
                            // Debug.LogError($"==> Valid Folders : {file}");

                            string JSONString = File.ReadAllText(file);
                                AppData.SceneAssetData sceneAssetData = JsonUtility.FromJson<AppData.SceneAssetData>(JSONString);

                                AppData.SceneAsset sceneAsset = sceneAssetData.ToSceneAsset();

                                if (!loadedAssetsList.Contains(sceneAsset))
                                    loadedAssetsList.Add(sceneAsset);
                            }

                            if (loadedAssetsList.Count > 0)
                            {
                                callbackResults.results = $"Success - {loadedAssetsList.Count} : Files Found In Directory : {folder.storageData.directory}";
                                callbackResults.data = loadedAssetsList;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = $"No Loaded Assets Files Found In Directory : {folder.storageData.directory}";
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = $"No Valid Files Found In Directory : {folder.storageData.directory}";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"Load Scene Assets's Directory.GetFiles Failed - FileDataList Is Null / Empty.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = foundDirectoryCallback.results;
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            });

            callback.Invoke(callbackResults);
        }

        public void LoadSceneAssets(List<AppData.StorageDirectoryData> fileDirectoryDataList, Action<AppData.CallbackDatas<AppData.SceneAsset>> callback)
        {
            AppData.CallbackDatas<AppData.SceneAsset> callbackResults = new AppData.CallbackDatas<AppData.SceneAsset>();

            if (fileDirectoryDataList != null && fileDirectoryDataList.Count > 0)
            {
                List<AppData.SceneAsset> loadedFiles = new List<AppData.SceneAsset>();

                foreach (var fileDirectory in fileDirectoryDataList)
                {
                    FileFound(fileDirectory.directory, fileFoundCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(fileFoundCallback.resultsCode))
                        {
                            string JSONString = File.ReadAllText(fileDirectory.directory);
                            AppData.SceneAssetData fileData = JsonUtility.FromJson<AppData.SceneAssetData>(JSONString);

                            AppData.SceneAsset sceneAsset = fileData.ToSceneAsset();

                            if (!string.IsNullOrEmpty(fileData.name))
                                loadedFiles.Add(sceneAsset);
                        }
                        else
                        {
                            callbackResults.results = fileFoundCallback.results;
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    });
                }

                if (loadedFiles.Count > 0)
                {
                    callbackResults.results = "Success";
                    callbackResults.data = loadedFiles;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"No Valid Folders Loaded.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "Folder Directory Data List Is Null / Empty";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void LoadProjectStructureData(Action<AppData.CallbackDatas<AppData.FolderStructureData>> callback)
        {
            try
            {
                AppData.CallbackDatas<AppData.FolderStructureData> callbackResults = new AppData.CallbackDatas<AppData.FolderStructureData>();

                AppData.StorageDirectoryData directoryData = GetAppDirectoryData(folderStructureDirectoryData.type);

                if (DirectoryFound(directoryData))
                {
                    var projectFiles = Directory.GetFileSystemEntries(directoryData.directory);

                    if(projectFiles != null && projectFiles.Length > 0)
                    {
                        List<AppData.StorageDirectoryData> validEntries = new List<AppData.StorageDirectoryData>();

                        foreach (var item in projectFiles)
                            if (item.Contains(".json") && !item.Contains(".meta"))
                            {
                                AppData.StorageDirectoryData validEntry = new AppData.StorageDirectoryData
                                {
                                    name = Path.GetFileName(item).Replace(".json", ""),
                                    path = item,
                                    directory = directoryData.directory
                                };

                                validEntries.Add(validEntry);
                            }

                        if(validEntries.Count > 0)
                        {
                            List<AppData.FolderStructureData> loadedEntries = new List<AppData.FolderStructureData>();

                            foreach (var entry in validEntries)
                            {
                                LoadData<AppData.FolderStructureData>(entry, loadedResults =>
                                {
                                    if (loadedResults.Success())
                                        loadedEntries.Add(loadedResults.data);
                                });
                            }

                            if(loadedEntries.Count > 0)
                            {
                                callbackResults.results = $"Directory : {directoryData.directory} Found.";
                                callbackResults.data = loadedEntries;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                            else
                            {

                                callbackResults.results = $" Failed To Load Project Structure From Directory : {directoryData.directory} - Please Check Here For Details.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = $" There Are No Valid Project Data Files Found In Directory : {directoryData.directory}.";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $" There Are No Valid Project Data Files Found In Directory : {directoryData.directory}.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"Directory : {directoryData.directory} Not Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void LoadFolderStuctureData(Action<AppData.CallbackData<AppData.FolderStructureData>> callback)
        {
            try
            {
                AppData.CallbackData<AppData.FolderStructureData> callbackResults = new AppData.CallbackData<AppData.FolderStructureData>();

                AppData.StorageDirectoryData directoryData = GetAppDirectoryData(folderStructureDirectoryData.type);

                if (DirectoryFound(directoryData))
                {
                    LoadData<AppData.FolderStructureData>(folderStructureData.name, directoryData, (folderStructureLoaded) =>
                    {
                        callbackResults = folderStructureLoaded;

                        if (folderStructureLoaded.Success())
                            folderStructureData = folderStructureLoaded.data;
                        else
                        {
                            AppData.Folder mainFolder = new AppData.Folder();
                            mainFolder.name = folderStructureData.rootFolder.name;
                            string fileName = mainFolder.name + "_Root";
                            string fileNameWithExtension = GetFileDataNameWithExtension(fileName, AppData.SelectableAssetType.Folder);
                            string fileNameWithoutExtension = GetFileDataNameWithoutExtension(fileNameWithExtension, AppData.SelectableAssetType.Folder);
                            ;
                            var storageData = GetAppDirectoryData(folderStructureData.rootFolder.storageData.type);

                            string path = Path.Combine(storageData.directory, fileNameWithExtension);
                            string validPath = path.Replace("\\", "/");

                            string directory = Path.Combine(storageData.directory, mainFolder.name);
                            string validDirectory = directory.Replace("\\", "/");

                            storageData.path = validPath;
                            storageData.directory = validDirectory;

                            mainFolder.storageData = storageData;
                            mainFolder.isRootFolder = true;

                            folderStructureData.name = fileNameWithoutExtension;
                            folderStructureData.rootFolder = mainFolder;

                            CreateData(folderStructureData, directoryData, (folderStructureCreated) =>
                            {
                                callbackResults = folderStructureCreated;

                                if (AppData.Helpers.IsSuccessCode(folderStructureCreated.resultsCode))
                                {
                                    CreateDirectory(validDirectory, directoryCreatedCallback =>
                                    {
                                        callbackResults.results = directoryCreatedCallback.results;
                                        callbackResults.resultsCode = directoryCreatedCallback.resultsCode;

                                        if (!AppData.Helpers.IsSuccessCode(directoryCreatedCallback.resultsCode))
                                            LogWarning(directoryCreatedCallback.results, this);
                                    });
                                }
                                else
                                    LogWarning(folderStructureCreated.results, this);

                            });
                        }
                    });
                }
                else
                {
                    callbackResults.results = $"Directory : {directoryData.directory} Not Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        #endregion

        public void AddToSelectedSceneAsseList(AppData.SceneAsset asset)
        {
            if (!selectedSceneAssetList.Contains(asset))
                selectedSceneAssetList.Add(asset);
        }

        public void RemoveFromSelectedSceneAsseListt(AppData.SceneAsset asset)
        {
            if (selectedSceneAssetList.Contains(asset))
                selectedSceneAssetList.Remove(asset);
        }

        #region On Move / Copy Data

        public void OnMoveToDirectory(AppData.StorageDirectoryData sourceStorageData, AppData.StorageDirectoryData targetStorageData, AppData.SelectableAssetType type, Action<AppData.CallbackData<AppData.DirectoryInfo>> callback = null)
        {
            try
            {
                AppData.CallbackData<AppData.DirectoryInfo> callbackResults = new AppData.CallbackData<AppData.DirectoryInfo>();

                if (type == AppData.SelectableAssetType.File)
                {
                    #region File Data

                    FileFound(sourceStorageData.path, fileCheckCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(fileCheckCallback.resultsCode))
                        {
                            if (!File.Exists(targetStorageData.path))
                            {
                                MoveFile(sourceStorageData.path, targetStorageData.path, type, fileDataMoveCallback =>
                                {
                                    Debug.LogError($"==> File Moved : {fileDataMoveCallback.resultsCode}");

                                    callbackResults.results = fileDataMoveCallback.results;
                                    callbackResults.data = default;
                                    callbackResults.resultsCode = fileDataMoveCallback.resultsCode;

                                    if (!AppData.Helpers.IsSuccessCode(fileDataMoveCallback.resultsCode))
                                        callback.Invoke(callbackResults);
                                });
                            }
                            else
                            {
                                callbackResults.results = $"File : {sourceStorageData.name} Already Exists In : {targetStorageData.name}";
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                                callbackResults.data = new AppData.DirectoryInfo
                                {
                                    name = sourceStorageData.name,
                                    assetType = type,
                                    dataAlreadyExistsInTargetDirectory = true
                                };

                                callback?.Invoke(callbackResults);
                            }
                        }
                        else
                        {
                            callbackResults.results = fileCheckCallback.results;
                            callbackResults.data = default;
                            callbackResults.resultsCode = fileCheckCallback.resultsCode;
                        }

                        callback?.Invoke(callbackResults);

                    });

                    #endregion
                }

                if (type == AppData.SelectableAssetType.Folder)
                {
                    #region File Data

                    FileFound(sourceStorageData.path, fileCheckCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(fileCheckCallback.resultsCode))
                        {
                            if (!File.Exists(targetStorageData.path))
                            {
                                MoveFile(sourceStorageData.path, targetStorageData.path, type, fileDataMoveCallback =>
                                {
                                    callbackResults.results = fileDataMoveCallback.results;
                                    callbackResults.data = default;
                                    callbackResults.resultsCode = fileDataMoveCallback.resultsCode;

                                    if (!AppData.Helpers.IsSuccessCode(fileDataMoveCallback.resultsCode))
                                        callback.Invoke(callbackResults);
                                });
                            }
                            else
                            {
                                callbackResults.results = $"File : {sourceStorageData.name} Already Exists In : {targetStorageData.name}";
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                                callbackResults.data = new AppData.DirectoryInfo
                                {
                                    name = sourceStorageData.name,
                                    assetType = type,
                                    dataAlreadyExistsInTargetDirectory = true
                                };

                                callback?.Invoke(callbackResults);
                            }
                        }
                        else
                        {
                            callbackResults.results = fileCheckCallback.results;
                            callbackResults.data = default;
                            callbackResults.resultsCode = fileCheckCallback.resultsCode;
                        }
                    });

                    #endregion

                    #region Folder Moved

                    DirectoryFound(sourceStorageData.directory, checkDirectoryCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(checkDirectoryCallback.resultsCode))
                        {
                            if (!Directory.Exists(targetStorageData.directory))
                            {
                                MoveDirectory(sourceStorageData.directory, targetStorageData.directory, checkDirectoryMoveCallback =>
                                {
                                    callbackResults.resultsCode = checkDirectoryMoveCallback.resultsCode;

                                    if (AppData.Helpers.IsSuccessCode(checkDirectoryMoveCallback.resultsCode))
                                    {
                                        string sourceDirectoryName = Path.GetFileNameWithoutExtension(sourceStorageData.path);
                                        string sourceDirectoryNameFormatted = GetAssetNameFormatted(sourceDirectoryName, type);

                                        string targetDirectoryName = Path.GetFileNameWithoutExtension(targetStorageData.path);
                                        string targetDirectoryNameFormatted = GetAssetNameFormatted(targetDirectoryName, type);

                                        callbackResults.results = $"<b>{sourceDirectoryNameFormatted}</b> Moved From <b>{currentFolder.name}</b> To <b>{GetFormattedName(targetStorageData.name, type, true)}</b>";

                                        var storageData = new AppData.StorageDirectoryData
                                        {
                                            name = sourceStorageData.name,
                                            directory = targetStorageData.directory
                                        };

                                        callbackResults.data = new AppData.DirectoryInfo
                                        {
                                            name = sourceStorageData.name,
                                            storageData = storageData,
                                            dataAlreadyExistsInTargetDirectory = false
                                        };
                                    }
                                    else
                                        callback.Invoke(callbackResults);
                                });
                            }
                            else
                            {
                                callbackResults.results = $"Directory : {sourceStorageData.directory} Already Exists In : {targetStorageData.name}";
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                                callbackResults.data = new AppData.DirectoryInfo
                                {
                                    name = sourceStorageData.name,
                                    assetType = type,
                                    dataAlreadyExistsInTargetDirectory = true
                                };

                                callback?.Invoke(callbackResults);
                            }
                        }
                        else
                        {
                            callbackResults.results = checkDirectoryCallback.results;
                            callbackResults.data = default;
                            callbackResults.resultsCode = checkDirectoryCallback.resultsCode;
                        }
                    });

                    callback.Invoke(callbackResults);

                    #endregion
                }
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void MoveFile(AppData.StorageDirectoryData sourceStorageData, AppData.StorageDirectoryData targetStorageData, Action<AppData.CallbackData<AppData.StorageDirectoryData>> callback)
        {
            try
            { 
                AppData.CallbackData<AppData.StorageDirectoryData> callbackResults = new AppData.CallbackData<AppData.StorageDirectoryData>();

                string sourceFileName = sourceStorageData.name + ".json";
                string targetDirectory = Path.Combine(targetStorageData.directory, sourceFileName);
                string formattedDirectory = targetDirectory.Replace("\\", "/");

                File.Move(sourceStorageData.directory, formattedDirectory);

                FileFound(formattedDirectory, fileCheckCallback =>
                {
                    callbackResults.results = fileCheckCallback.results;
                    callbackResults.resultsCode = fileCheckCallback.resultsCode;

                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                        callbackResults.data = new AppData.StorageDirectoryData
                        {
                            name = sourceStorageData.name,
                            directory = formattedDirectory
                        };
                });

                callback.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void MoveFile(string sourceDirectory, string targetDirectory, AppData.SelectableAssetType assetType, Action<AppData.CallbackData<AppData.StorageDirectoryData>> callback)
        {
            AppData.CallbackData<AppData.StorageDirectoryData> callbackResults = new AppData.CallbackData<AppData.StorageDirectoryData>();

            File.Move(sourceDirectory, targetDirectory);

            // Check If File Moved Successfully.
            FileFound(targetDirectory, fileCheckCallback =>
            {
                callbackResults.results = fileCheckCallback.results;
                callbackResults.resultsCode = fileCheckCallback.resultsCode;

                if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                {
                    string fileDataPath = targetDirectory;
                    string fileName = Path.GetFileNameWithoutExtension(fileDataPath);
                    string fileDataDirectory = AppData.Helpers.GetFormattedDirectoryPath(Path.Combine(Path.GetDirectoryName(fileDataPath), GetAssetNameFormatted(fileName, assetType)));

                    AppData.StorageDirectoryData newStorageData = new AppData.StorageDirectoryData
                    {
                        name = Path.GetFileNameWithoutExtension(fileDataPath),
                        path = fileDataPath,
                        directory = fileDataDirectory
                    };

                #region File Data Update

                if (assetType == AppData.SelectableAssetType.File)
                    {
                        LoadData<AppData.SceneAssetData>(newStorageData, fileLoaderCallback =>
                        {
                            callbackResults.results = fileLoaderCallback.results;
                            callbackResults.resultsCode = fileLoaderCallback.resultsCode;

                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                            {
                                AppData.SceneAssetData loadedFileData = fileLoaderCallback.data;
                                loadedFileData.storageData = newStorageData;

                                SaveData(loadedFileData, checkFileSavedCallback =>
                                {
                                    callbackResults.resultsCode = checkFileSavedCallback.resultsCode;

                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    {
                                        callbackResults.results = checkFileSavedCallback.results;
                                        callbackResults.data = newStorageData;
                                    }
                                    else
                                    {
                                        callbackResults.results = $"Couldn't Save File Data : {fileLoaderCallback.data.name} At Directory : {newStorageData.directory}";
                                        callbackResults.data = default;
                                    }
                                });
                            }
                            else
                                callbackResults.data = default;
                        });

                        callback.Invoke(callbackResults);
                    }

                #endregion

                #region Folder Data Update

                if (assetType == AppData.SelectableAssetType.Folder)
                    {
                        LoadData<AppData.Folder>(newStorageData, fileLoaderCallback =>
                        {
                            callbackResults.results = fileLoaderCallback.results;
                            callbackResults.resultsCode = fileLoaderCallback.resultsCode;

                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                            {
                                AppData.Folder loadedFolderData = fileLoaderCallback.data;
                                loadedFolderData.storageData = newStorageData;

                                SaveData(loadedFolderData, checkFileSavedCallback =>
                                {
                                    callbackResults.resultsCode = checkFileSavedCallback.resultsCode;

                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    {
                                        callbackResults.results = checkFileSavedCallback.results;
                                        callbackResults.data = newStorageData;
                                    }
                                    else
                                    {
                                        callbackResults.results = $"Couldn't Save Folder Data : {fileLoaderCallback.data.name} At Directory : {newStorageData.directory}";
                                        callbackResults.data = default;
                                    }
                                });
                            }
                            else
                                callbackResults.data = default;
                        });

                        callback.Invoke(callbackResults);
                    }

                #endregion
            }
            });
        }

        public void MoveDirectory(AppData.StorageDirectoryData sourceStorageData, AppData.StorageDirectoryData targetStorageData, Action<AppData.CallbackData<AppData.StorageDirectoryData>> callback)
        {
            AppData.CallbackData<AppData.StorageDirectoryData> callbackResults = new AppData.CallbackData<AppData.StorageDirectoryData>();

            Debug.LogError($"==> Moving Directory From : {sourceStorageData.directory} To : {targetStorageData.directory}");

            //Directory.Move(sourceStorageData.directory)

            callback.Invoke(callbackResults);
        }

        public void MoveDirectory(string sourceDirectory, string targetDirectory, Action<AppData.CallbackData<AppData.StorageDirectoryData>> callback)
        {
            AppData.CallbackData<AppData.StorageDirectoryData> callbackResults = new AppData.CallbackData<AppData.StorageDirectoryData>();

            Directory.Move(sourceDirectory, targetDirectory);

            DirectoryFound(targetDirectory, directoryCheckCallback =>
            {
                callbackResults.results = directoryCheckCallback.results;
                callbackResults.resultsCode = directoryCheckCallback.resultsCode;

                if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                {
                    string[] contentInDirectory = Directory.GetFiles(targetDirectory, "*Data.json", SearchOption.AllDirectories);

                    if (contentInDirectory.Length > 0)
                    {
                        Debug.LogError($"==> Found : {contentInDirectory.Length} Files In Moved Folder.");

                        List<AppData.StorageDirectoryData> formattedFileSorageDataList = new List<AppData.StorageDirectoryData>();
                        List<AppData.StorageDirectoryData> formattedFoldeStorageDataList = new List<AppData.StorageDirectoryData>();

                        foreach (var content in contentInDirectory)
                        {
                            string formattedPath = AppData.Helpers.GetFormattedDirectoryPath(content);
                            string formattedName = Path.GetFileNameWithoutExtension(formattedPath);
                            string formattedFolderName = GetFormattedName(formattedName, AppData.SelectableAssetType.Folder, true);
                            string formattedDirectory = AppData.Helpers.GetFormattedDirectoryPath(Path.GetDirectoryName(formattedPath));
                            string newDirectory = Path.Combine(formattedDirectory, formattedFolderName);
                            string newFormattedDirectory = AppData.Helpers.GetFormattedDirectoryPath(newDirectory);

                            AppData.SelectableAssetType assetType = GetAssetTypeFromAssetDataName(Path.GetFileNameWithoutExtension(formattedPath));

                            AppData.StorageDirectoryData storageData = new AppData.StorageDirectoryData
                            {
                                name = formattedName,
                                path = formattedPath,
                                directory = newFormattedDirectory,
                            };

                            if (assetType == AppData.SelectableAssetType.File)
                            {
                                if (!formattedFileSorageDataList.Contains(storageData))
                                    formattedFileSorageDataList.Add(storageData);
                            }

                            if (assetType == AppData.SelectableAssetType.Folder)
                            {
                                if (!formattedFoldeStorageDataList.Contains(storageData))
                                    formattedFoldeStorageDataList.Add(storageData);
                            }
                        }

                    #region Update Sub Files

                    if (formattedFileSorageDataList.Count > 0)
                        {
                            foreach (var fileSorageData in formattedFileSorageDataList)
                            {
                                LoadData<AppData.SceneAssetData>(fileSorageData, (fileLoaderCallback) =>
                                {
                                    callbackResults.results = fileLoaderCallback.results;
                                    callbackResults.resultsCode = fileLoaderCallback.resultsCode;

                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    {
                                        AppData.SceneAssetData loadedFileData = fileLoaderCallback.data;
                                        loadedFileData.storageData.path = fileSorageData.path;
                                        loadedFileData.storageData.directory = fileSorageData.directory;

                                        SaveData(loadedFileData, checkFileSavedCallback =>
                                        {
                                            callbackResults.resultsCode = checkFileSavedCallback.resultsCode;

                                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                            {
                                                callbackResults.results = checkFileSavedCallback.results;
                                                callbackResults.data = fileSorageData;
                                            }
                                            else
                                            {
                                                callbackResults.results = $"Couldn't Save Asset File Data : {fileLoaderCallback.data.name} At Directory : {fileSorageData.directory}";
                                                callbackResults.data = default;
                                            }
                                        });
                                    }
                                    else
                                        callbackResults.data = default;
                                });
                            }
                        }

                    #endregion

                    #region Update Sub Folders

                    if (formattedFoldeStorageDataList.Count > 0)
                        {
                            foreach (var folderStorageData in formattedFoldeStorageDataList)
                            {
                                LoadData<AppData.Folder>(folderStorageData, (fileLoaderCallback) =>
                                {
                                    callbackResults.results = fileLoaderCallback.results;
                                    callbackResults.resultsCode = fileLoaderCallback.resultsCode;

                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    {
                                        AppData.Folder loadedFoldereData = fileLoaderCallback.data;
                                        loadedFoldereData.storageData.path = folderStorageData.path;
                                        loadedFoldereData.storageData.directory = folderStorageData.directory;

                                        SaveData(loadedFoldereData, checkFileSavedCallback =>
                                        {
                                            callbackResults.resultsCode = checkFileSavedCallback.resultsCode;

                                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                            {
                                                callbackResults.results = checkFileSavedCallback.results;
                                                callbackResults.data = folderStorageData;
                                            }
                                            else
                                            {
                                                callbackResults.results = $"Couldn't Save Folder Data : {fileLoaderCallback.data.name} At Directory : {folderStorageData.directory}";
                                                callbackResults.data = default;
                                            }
                                        });
                                    }
                                    else
                                        callbackResults.data = default;
                                });
                            }
                        }

                    #endregion
                }
                }
            });

            callback.Invoke(callbackResults);
        }

        #endregion

        #region On Delete Asset / Folder Widgets

        public void OnDelete(List<AppData.UIScreenWidget> assets, Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            int assetsToDeleteCount = assets.Count;

            foreach (var asset in assets)
            {
                switch (asset.GetSelectableAssetType())
                {
                    case AppData.SelectableAssetType.File:

                        AppData.SceneAsset assetToDelete = asset.GetSceneAssetData();

                        Delete(assetToDelete, assetDeletedCallback =>
                        {
                            if (AppData.Helpers.IsSuccessCode(assetDeletedCallback.resultsCode))
                                assetsToDeleteCount--;
                            else
                                Debug.LogWarning($"--> Delete Failed With Results : {assetDeletedCallback.results}");
                        });

                        break;

                    case AppData.SelectableAssetType.Folder:

                        AppData.Folder folderToDelete = asset.GetFolderData();

                        Delete(folderToDelete, folderDeletedCallback =>
                        {
                            if (AppData.Helpers.IsSuccessCode(folderDeletedCallback.resultsCode))
                                assetsToDeleteCount--;
                            else
                                Debug.LogWarning($"--> DeleteAssetFolder Failed With Results : {folderDeletedCallback.results}");
                        });

                        break;
                }
            }

            if (assetsToDeleteCount == 0)
            {
                callbackResults.results = "Assets Deleted Successfully.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Assets Failed To Delete For Unknown Reasons. Please Check Here.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void Delete(AppData.Folder folder, Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            Debug.LogError($"==> Deleting Folder Directory : {folder.storageData} : Folder Content Path : {folder.storageData.directory}");

            if (File.Exists(folder.storageData.directory))
                File.Delete(folder.storageData.directory);

            if (Directory.Exists(folder.storageData.directory))
                Directory.Delete(folder.storageData.directory, true);

            if (!File.Exists(folder.storageData.directory) && !Directory.Exists(folder.storageData.directory))
            {
                callbackResults.results = "Folder Deleted Successfully.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Folder Failed To Deleted For Unknown Reasons.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void Delete(AppData.SceneAsset asset, Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            Debug.LogError($"==> Deleting Asset From Directory : {asset.storageData}");

            if (File.Exists(asset.storageData.directory))
                File.Delete(asset.storageData.directory);

            if (!File.Exists(asset.storageData.directory))
            {
                callbackResults.results = "Asset Deleted Successfully.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Asset Failed To Deleted For Unknown Reasons.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        #endregion

        public AppData.SelectableAssetType GetAssetTypeFromAssetDataName(string assetName)
        {
            if (assetName.Contains("File"))
                return AppData.SelectableAssetType.File;

            if (assetName.Contains("Folder"))
                return AppData.SelectableAssetType.Folder;

            return AppData.SelectableAssetType.PlaceHolder;
        }

        public bool HasSelectedAssets()
        {
            return selectedSceneAssetList.Count > 0;
        }

        public void SetAssetSortFilterOnDropDownAction(AppData.InputDropDownActionType actionType, int dropDownIndex)
        {
            if (ScreenUIManager.Instance != null)
            {
                switch (actionType)
                {
                    case AppData.InputDropDownActionType.FilterList:

                        SetSceneAssetFilterType((AppData.SceneAssetCategoryType)dropDownIndex);
                        ScreenUIManager.Instance.Refresh();
                        //FilterSceneAssetWidgets();

                        break;

                    case AppData.InputDropDownActionType.SortingList:

                        SetSceneAssetSortType((AppData.SceneAssetSortType)dropDownIndex);
                        ScreenUIManager.Instance.Refresh();
                        SortSceneAssetWidgets();

                        break;
                }
            }
            else
                Debug.LogWarning("--> SetAssetSortFilterOnDropDownAction Failed : ScreenUIManager.Instance Is Not Yet Initialized.");
        }

        public void GetSortedWidgetList<T>(List<T> serializableDataList, List<T> pinnedList, Action<AppData.CallbackDatas<T>> callback) where T : AppData.SerializableData
        {
            AppData.CallbackDatas<T> callbackResults = new AppData.CallbackDatas<T>();

            if (serializableDataList != null)
            {
                switch (GetSceneAssetSortType())
                {
                    case AppData.SceneAssetSortType.Ascending:

                        serializableDataList.Sort((firstWidget, secondWidget) => firstWidget.name.CompareTo(secondWidget.name));

                        break;


                    case AppData.SceneAssetSortType.Category:

                        //serializableDataList.Sort((firstWidget, secondWidget) => firstWidget.categoryType.CompareTo(secondWidget.categoryType));

                        break;


                    case AppData.SceneAssetSortType.Descending:

                        serializableDataList.Sort((firstWidget, secondWidget) => secondWidget.name.CompareTo(firstWidget.name));

                        break;

                    case AppData.SceneAssetSortType.DateModified:

                        //serializableDataList.Sort((firstWidget, secondWidget) => secondWidget.GetModifiedDateTime().CompareTo(firstWidget.GetModifiedDateTime()));

                        break;
                }

                for (int i = 0; i < pinnedList.Count; i++)
                {
                    if (serializableDataList.Contains(pinnedList[i]))
                    {
                        serializableDataList.Remove(pinnedList[i]);
                        serializableDataList.Insert(i, pinnedList[i]);
                    }
                }

                callbackResults.results = "GetSortedWidgetList Success";
                callbackResults.data = serializableDataList;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "GetSortedWidgetList Failed : serializableDataList Is Null / Empty.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void GetFilteredWidgetList<T>(T data, Action<AppData.CallbackDatas<T>> callback) where T : AppData.SerializableData
        {
            AppData.CallbackDatas<T> callbackResults = new AppData.CallbackDatas<T>();

            callback?.Invoke(callbackResults);
        }

        public void SortScreenWidgets(List<AppData.UIScreenWidget> widgets)
        {
            if (widgets != null)
            {
                switch (GetSceneAssetSortType())
                {
                    case AppData.SceneAssetSortType.Ascending:

                        widgets.Sort((firstWidget, secondWidget) => firstWidget.name.CompareTo(secondWidget.name));

                        break;


                    case AppData.SceneAssetSortType.Category:

                        //serializableDataList.Sort((firstWidget, secondWidget) => firstWidget.categoryType.CompareTo(secondWidget.categoryType));

                        break;


                    case AppData.SceneAssetSortType.Descending:

                        widgets.Sort((firstWidget, secondWidget) => secondWidget.name.CompareTo(firstWidget.name));

                        break;

                    case AppData.SceneAssetSortType.DateModified:

                        //serializableDataList.Sort((firstWidget, secondWidget) => secondWidget.GetModifiedDateTime().CompareTo(firstWidget.GetModifiedDateTime()));

                        break;
                }
            }
        }

        public void GetSortedWidgetsFromList(List<AppData.UIScreenWidget> widgets, Action<AppData.CallbackData<List<AppData.UIScreenWidget>>> callback)
        {
            AppData.CallbackData<List<AppData.UIScreenWidget>> callbackResults = new AppData.CallbackData<List<AppData.UIScreenWidget>>();

            if (widgets != null)
            {
                switch (GetSceneAssetSortType())
                {
                    case AppData.SceneAssetSortType.Ascending:

                        widgets.Sort((firstWidget, secondWidget) => firstWidget.name.CompareTo(secondWidget.name));

                        break;


                    case AppData.SceneAssetSortType.Category:

                        //serializableDataList.Sort((firstWidget, secondWidget) => firstWidget.categoryType.CompareTo(secondWidget.categoryType));

                        break;


                    case AppData.SceneAssetSortType.Descending:

                        widgets.Sort((firstWidget, secondWidget) => secondWidget.name.CompareTo(firstWidget.name));

                        break;

                    case AppData.SceneAssetSortType.DateModified:

                        //serializableDataList.Sort((firstWidget, secondWidget) => secondWidget.GetModifiedDateTime().CompareTo(firstWidget.GetModifiedDateTime()));

                        break;
                }

                callbackResults.results = "Widgets Sorted Successfully.";
                callbackResults.data = widgets;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Failed To Get Sorted Widgets : Widgets To Sort Null.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        void SetSceneAssetSortType(AppData.SceneAssetSortType sortType) => assetSortType = sortType;

        public AppData.SceneAssetSortType GetSceneAssetSortType()
        {
            return assetSortType;
        }

        void SetSceneAssetFilterType(AppData.SceneAssetCategoryType filterType) => assetFilterType = filterType;

        public AppData.SceneAssetCategoryType GetSceneAssetFilterType()
        {
            return assetFilterType;
        }

        public void SetCurrentSceneMode(AppData.SceneMode sceneMode)
        {
            currentSceneMode = sceneMode;
        }

        public List<AppData.UIImageData> GetImageDataLibrary()
        {
            return imageDataLibrary;
        }

        public AppData.UIImageData GetImageFromLibrary(AppData.UIImageType imageType)
        {
            return imageDataLibrary.Find(imageData => imageData.imageType == imageType);
        }

        public AppData.SceneMode GetCurrentSceneMode()
        {
            return currentSceneMode;
        }

        public AppData.SceneAssetLibrary GetAssetsLibrary()
        {
            return sceneAssetLibrary;
        }

        public Sprite GetDefaultFallbackSceneAssetIcon()
        {
            if (defaultFallbackSceneAssetIcon != null)
                return defaultFallbackSceneAssetIcon;
            else
            {
                Debug.LogWarning("--> Get Default Fall back Scene Asset Icon Returns A Null Ref.");
                return null;
            }
        }

        public AppData.RuntimeValue<float> GetDefaultExecutionValue(AppData.RuntimeValueType valueType)
        {
            return defaultExecutionTimes.Find((x) => x.valueType == valueType);
        }

        #region Searching

        public void SearchScreenWidgetList(string searchValue, Action<AppData.CallbackData<List<string>>> callback = null)
        {
            AppData.CallbackData<List<string>> callbackResults = new AppData.CallbackData<List<string>>();

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (GetWidgetsRefreshData().widgetsContainer && GetWidgetsRefreshData().widgetsContainer.IsContainerActive())
                {
                    #region Search

                    var searchFolder = GetCurrentFolder();

                    if (!string.IsNullOrEmpty(searchFolder.storageData.directory))
                    {
                        DirectoryFound(searchFolder.storageData.directory, foundDirectoriesCallback =>
                        {
                            GetWidgetsRefreshData().widgetsContainer.ClearWidgets();

                            if (AppData.Helpers.IsSuccessCode(foundDirectoriesCallback.resultsCode))
                            {
                                var searchedItems = Directory.GetFileSystemEntries(searchFolder.storageData.directory, "*.json", SearchOption.AllDirectories);

                                if (searchedItems.Length > 0)
                                {
                                #region Get System Files

                                List<string> validFoldersfound = new List<string>();
                                    List<string> validFilesfound = new List<string>();

                                    List<string> foldersDataBlackList = new List<string>();
                                    List<string> filesDataBlackList = new List<string>();

                                    bool folderFound = false;
                                    bool fileFound = false;

                                    foreach (var searchedItem in searchedItems)
                                    {
                                        if (GetFolderStructureData().GetExcludedSystemFileData() != null)
                                        {
                                            foreach (var excludedFolder in GetFolderStructureData().GetExcludedSystemFileData())
                                            {
                                                if (!searchedItem.Contains(excludedFolder) && !filesDataBlackList.Contains(searchedItem))
                                                {
                                                    if (!validFoldersfound.Contains(searchedItem))
                                                        validFoldersfound.Add(searchedItem);
                                                }
                                                else
                                                    filesDataBlackList.Add(searchedItem);
                                            }
                                        }
                                        else
                                            Debug.LogWarning($"==> LoadFolderData's GetExcludedSystemFolders Failed - GetFolderStructureData().GetExcludedSystemFileData() Returned Null.");

                                        if (GetFolderStructureData().GetExcludedSystemFolderData() != null)
                                        {
                                            foreach (var excludedFile in GetFolderStructureData().GetExcludedSystemFolderData())
                                            {
                                                if (!searchedItem.Contains(excludedFile) && !foldersDataBlackList.Contains(searchedItem))
                                                {
                                                    if (!validFilesfound.Contains(searchedItem))
                                                        validFilesfound.Add(searchedItem);
                                                }
                                                else
                                                    foldersDataBlackList.Add(searchedItem);
                                            }
                                        }
                                        else
                                            Debug.LogWarning($"==> LoadFilesData's GetExcludedSystemFolders Failed - GetFolderStructureData().GetExcludedSystemFolders() Returned Null.");
                                    }

                                #endregion

                                #region Search Files

                                #region Folders

                                if (validFoldersfound.Count > 0)
                                    {
                                        List<AppData.StorageDirectoryData> validFoldersfoundDirectories = new List<AppData.StorageDirectoryData>();
                                        List<AppData.StorageDirectoryData> foldersSearchResults = new List<AppData.StorageDirectoryData>();

                                        foreach (var validFolder in validFoldersfound)
                                        {
                                            var fileName = Path.GetFileName(validFolder);
                                            AppData.StorageDirectoryData directoryData = new AppData.StorageDirectoryData
                                            {
                                                name = fileName,
                                                directory = validFolder,
                                                type = searchFolder.storageData.type
                                            };

                                            validFoldersfoundDirectories.Add(directoryData);
                                        }

                                        if (validFoldersfoundDirectories.Count > 0)
                                        {
                                        #region Folder Search Filter

                                        foreach (var validDirectory in validFoldersfoundDirectories)
                                            {
                                                string folderName = validDirectory.name.ToLower();

                                                if (strictValidateAssetSearch)
                                                {
                                                    if (folderName.Contains(searchValue.ToLower()) && folderName.StartsWith(searchValue[0].ToString().ToLower()))
                                                    {
                                                        if (!foldersSearchResults.Contains(validDirectory))
                                                            foldersSearchResults.Add(validDirectory);
                                                    }
                                                    else
                                                    {
                                                        if (foldersSearchResults.Contains(validDirectory))
                                                            foldersSearchResults.Remove(validDirectory);
                                                    }
                                                }
                                                else
                                                {
                                                    if (folderName.Contains(searchValue.ToLower()))
                                                    {
                                                        if (!foldersSearchResults.Contains(validDirectory))
                                                            foldersSearchResults.Add(validDirectory);
                                                    }
                                                    else
                                                    {
                                                        if (foldersSearchResults.Contains(validDirectory))
                                                            foldersSearchResults.Remove(validDirectory);
                                                    }
                                                }
                                            }

                                        #endregion

                                        #region Create Folder Widgets

                                        if (foldersSearchResults.Count > 0)
                                            {
                                                CreateUIScreenFolderWidgets(ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType(), foldersSearchResults, GetWidgetsRefreshData().widgetsContainer, (widgetsCreated) =>
                                                {
                                                    folderFound = widgetsCreated.resultsCode == AppData.Helpers.SuccessCode;
                                                });
                                            }

                                        #endregion
                                    }
                                    }

                                #endregion

                                #region Files

                                if (validFilesfound.Count > 0)
                                    {
                                        List<AppData.StorageDirectoryData> validFilesfoundDirectories = new List<AppData.StorageDirectoryData>();
                                        List<AppData.StorageDirectoryData> filesSearchResults = new List<AppData.StorageDirectoryData>();

                                        foreach (var validFileDirectory in validFilesfound)
                                        {
                                            var fileName = Path.GetFileName(validFileDirectory);
                                            AppData.StorageDirectoryData directoryData = new AppData.StorageDirectoryData
                                            {
                                                name = fileName,
                                                directory = validFileDirectory,
                                                type = searchFolder.storageData.type
                                            };

                                            validFilesfoundDirectories.Add(directoryData);
                                        }

                                        if (validFilesfoundDirectories.Count > 0)
                                        {
                                        #region File Search Filter

                                        foreach (var validDirectory in validFilesfoundDirectories)
                                            {
                                                string fileName = validDirectory.name.ToLower();

                                                if (strictValidateAssetSearch)
                                                {
                                                    if (fileName.Contains(searchValue.ToLower()) && fileName.StartsWith(searchValue[0].ToString().ToLower()))
                                                    {
                                                        if (!filesSearchResults.Contains(validDirectory))
                                                            filesSearchResults.Add(validDirectory);
                                                    }
                                                    else
                                                    {
                                                        if (filesSearchResults.Contains(validDirectory))
                                                            filesSearchResults.Remove(validDirectory);
                                                    }
                                                }
                                                else
                                                {
                                                    if (fileName.Contains(searchValue.ToLower()))
                                                    {
                                                        if (!filesSearchResults.Contains(validDirectory))
                                                            filesSearchResults.Add(validDirectory);
                                                    }
                                                    else
                                                    {
                                                        if (filesSearchResults.Contains(validDirectory))
                                                            filesSearchResults.Remove(validDirectory);
                                                    }
                                                }
                                            }

                                        #endregion

                                        #region Create File Widgets

                                        if (filesSearchResults.Count > 0)
                                            {
                                                foreach (var file in filesSearchResults)
                                                {

                                                    Debug.LogError($"==> Found File : {file.name} : Directory : {file.directory}");
                                                }
                                            }


                                        #region Create File Widgets

                                        if (filesSearchResults.Count > 0)
                                            {
                                                CreateUIScreenFileWidgets(ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType(), filesSearchResults, GetWidgetsRefreshData().widgetsContainer, (widgetsCreated) =>
                                                {
                                                    folderFound = widgetsCreated.resultsCode == AppData.Helpers.SuccessCode;
                                                });
                                            }

                                        #endregion

                                        #endregion
                                    }
                                    }

                                #endregion

                                #region No Results Found

                                if (ScreenUIManager.Instance)
                                    {
                                        if (!folderFound && !fileFound)
                                        {
                                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.InputUIState.Disabled);
                                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Disabled);

                                            ScreenUIManager.Instance.GetCurrentScreenData().value.ShowLoadingItem(AppData.LoadingItemType.Spinner, true);
                                        }
                                        else
                                        {
                                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.InputUIState.Enabled);
                                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Enabled);

                                            ScreenUIManager.Instance.GetCurrentScreenData().value.ShowLoadingItem(AppData.LoadingItemType.Spinner, false);
                                        }

                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewFolderButton, AppData.InputUIState.Disabled);
                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewAsset, AppData.InputUIState.Disabled);
                                    }
                                    else
                                        Debug.LogWarning("--> Screen Manager Not Yet Initialized.");

                                #endregion

                                #endregion
                            }
                            }
                            else
                                Debug.LogWarning($"--> SearchScreenWidgetList Failed With Results : {foundDirectoriesCallback.results}.");
                        });
                    }
                    else
                        Debug.LogWarning($"--> SearchScreenWidgetList Failed - Search Folder Directory Data Is Missing / Null.");

                    #endregion
                }

            }
            else
            {

                ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.InputUIState.Enabled);
                ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Enabled);

                ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewFolderButton, AppData.InputUIState.Enabled);
                ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewAsset, AppData.InputUIState.Enabled);

                ScreenUIManager.Instance.GetCurrentScreenData().value.ShowLoadingItem(AppData.LoadingItemType.Spinner, false);
                ScreenUIManager.Instance.Refresh();
            }

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Filtering

        public void FilterSceneAssetWidgets()
        {

            if (screenWidgetList.Count > 0)
            {
                foreach (var widget in screenWidgetList)
                {
                    if (assetFilterType != AppData.SceneAssetCategoryType.None)
                    {
                        if (widget.categoryType == assetFilterType)
                            widget.SetVisibilityState(true);
                        else
                            widget.SetVisibilityState(false);
                    }
                    else
                    {
                        widget.SetVisibilityState(true);
                    }
                }
            }
            else
                Debug.LogWarning("--> Screen Widget List Is Null / Not Initialized.");
        }

        public void FilterSceneAssetWidgets(AppData.SceneAssetCategoryType filterType)
        {

            if (screenWidgetList.Count > 0)
            {
                foreach (var widget in screenWidgetList)
                {
                    if (assetFilterType != AppData.SceneAssetCategoryType.None)
                    {
                        if (widget.categoryType == assetFilterType)
                            widget.SetVisibilityState(true);
                        else
                            widget.SetVisibilityState(false);
                    }
                    else
                    {
                        widget.SetVisibilityState(true);
                    }
                }
            }
            else
                Debug.LogWarning("--> Screen Widget List Is Null / Not Initialized.");
        }

        public List<string> GetAssetCategoryList()
        {
            return dropDownContentDataList.Find((x) => x.contentType == AppData.DropDownContentType.Categories).data;
        }

        #endregion

        #region Sorting

        public void SortSceneAssetWidgets()
        {

            switch (assetSortType)
            {
                case AppData.SceneAssetSortType.Ascending:

                    screenWidgetList.Sort((firstWidget, secondWidget) => firstWidget.name.CompareTo(secondWidget.name));

                    break;


                case AppData.SceneAssetSortType.Category:

                    screenWidgetList.Sort((firstWidget, secondWidget) => firstWidget.categoryType.CompareTo(secondWidget.categoryType));

                    break;


                case AppData.SceneAssetSortType.Descending:

                    screenWidgetList.Sort((firstWidget, secondWidget) => secondWidget.name.CompareTo(firstWidget.name));

                    break;

                case AppData.SceneAssetSortType.DateModified:

                    screenWidgetList.Sort((firstWidget, secondWidget) => secondWidget.GetModifiedDateTime().CompareTo(firstWidget.GetModifiedDateTime()));

                    break;
            }

            if (screenWidgetList.Count > 0)
                for (int i = 0; i < screenWidgetList.Count; i++)
                    screenWidgetList[i].SetAssetListIndex(i);
            else
                Debug.LogWarning("--> Screen Widget List Is Null / Not Initialized.");
        }

        public void SortSceneAssetWidgets(AppData.SceneAssetSortType sortType)
        {

            switch (sortType)
            {
                case AppData.SceneAssetSortType.Ascending:

                    screenWidgetList.Sort((firstWidget, secondWidget) => firstWidget.name.CompareTo(secondWidget.name));

                    break;


                case AppData.SceneAssetSortType.Category:

                    screenWidgetList.Sort((firstWidget, secondWidget) => firstWidget.categoryType.CompareTo(secondWidget.categoryType));

                    break;


                case AppData.SceneAssetSortType.Descending:

                    screenWidgetList.Sort((firstWidget, secondWidget) => secondWidget.name.CompareTo(firstWidget.name));

                    break;

                case AppData.SceneAssetSortType.DateModified:

                    screenWidgetList.Sort((firstWidget, secondWidget) => secondWidget.GetModifiedDateTime().CompareTo(firstWidget.GetModifiedDateTime()));

                    break;
            }

            if (screenWidgetList.Count > 0)
                for (int i = 0; i < screenWidgetList.Count; i++)
                    screenWidgetList[i].SetAssetListIndex(i);
            else
                Debug.LogWarning("--> Screen Widget List Is Null / Not Initialized.");
        }

        public AppData.SceneAssetSortType GetCurrentAssetSortType()
        {
            return assetSortType;
        }

        public List<AppData.DropDownContentData> GetDropDownContentDataList()
        {
            return dropDownContentDataList;
        }

        public AppData.DropDownContentData GetDropDownContentData(AppData.DropDownContentType contentType)
        {
            return dropDownContentDataList.Find((x) => x.contentType == contentType);
        }

        public void SetSceneAssetRenderMode(AppData.SceneAssetRenderMode renderMode)
        {
            assetRenderMode = renderMode;

            if (RenderingSettingsManager.Instance)
                RenderingSettingsManager.Instance.OnRenderMode(assetRenderMode);
            else
                Debug.LogWarning("--> Rendering Manager Not Yet Initialized.");
        }

        public AppData.SceneAssetRenderMode GetSceneAssetRenderMode()
        {
            return assetRenderMode;
        }

        public void SetCurrentAssetExportData(AppData.AssetExportData exportData)
        {
            currentAssetExportData = exportData;
        }

        public AppData.AssetExportData GetCurrentAssetExportData()
        {
            return currentAssetExportData;
        }

        public List<string> GetFormatedDropDownContentList(List<string> dropDownContent)
        {
            if (dropDownContent != null)
            {
                if (dropDownContent.Count > 0)
                    for (int i = 0; i < dropDownContent.Count; i++)
                    {
                        dropDownContent[i] = dropDownContent[i].Replace("_", " ");
                    }
                else
                    Debug.LogWarning("--> RG_Unity - GetFormatedDropDownContentList Failed : Drop Down Content Is Null / Empty.");
            }
            else
                Debug.LogWarning("--> RG_Unity - GetFormatedDropDownContentList Failed : Drop Down Content Parameter Is Null / Empty.");

            return dropDownContent;
        }

        string GetFormattedName(string name, List<string> replaceStringList = null)
        {
            if (replaceStringList != null)
            {
                foreach (var replaceString in replaceStringList)
                {
                    name = name.Replace(replaceString, "");
                }
            }
            else
                Debug.LogWarning("--> ");

            return name;
        }

        #region Render Profile Data

        public void GetRenderProfileUIHandlerPrefab(Action<AppData.CallbackData<RenderProfileUIHandler>> callback)
        {
            AppData.CallbackData<RenderProfileUIHandler> callbackResults = new AppData.CallbackData<RenderProfileUIHandler>();

            if (renderProfileUIHandlerPrefab != null)
            {
                callbackResults.data = renderProfileUIHandlerPrefab;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                callbackResults.results = "Prefab Retrieved Successfully.";
            }
            else
            {
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                callbackResults.results = "Render Profile UI Handler Prefab Is Missing / Null.";
            }

            callback.Invoke(callbackResults);
        }

        public void CreateNewRenderProfile(AppData.ButtonDataPackets dataPackets, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            CreateProfileWidget(dataPackets, (results) =>
            {
                callbackResults = results;
            });

            callback.Invoke(callbackResults);
        }

        public void CreateNewFolderWidget(AppData.ButtonDataPackets dataPackets, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(ScreenUIManager.Instance.GetCurrentUIScreenType(), widgetsCallback =>
            {
                if (widgetsCallback.Success())
                {
                    var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == ScreenUIManager.Instance.GetCurrentUIScreenType());

                    if (widgetPrefabData != null)
                    {
                        widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableAssetType.Folder, folderStructureData.GetCurrentLayoutViewType(), prefabCallbackResults =>
                        {
                            if (prefabCallbackResults.Success())
                            {
                                GameObject folder = Instantiate(prefabCallbackResults.data.gameObject);

                                UIScreenFolderWidget folderHandler = folder.GetComponent<UIScreenFolderWidget>();

                                if (folderHandler != null)
                                {
                                    folderHandlerComponentsList.Add(folderHandler);
                                }
                                else
                                    folderHandlerComponentsList.Add(folderHandler = folder.AddComponent<UIScreenFolderWidget>());

                                AddContentToDynamicWidgetContainer(folder.GetComponent<AppData.UIScreenWidget>(), dataPackets.containerType, dataPackets.containerContentOrientation);

                                callbackResults.results = "Folder Created Successfully.";
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = "CreateFolderWidget Failed : folderHandlerPrefab Is Null.";
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                                Log(prefabCallbackResults.resultsCode, prefabCallbackResults.results, this);
                            }
                        });
                    }
                    else
                        LogError("Widget Prefab Data Missing.", this);
                }
            });

            callback.Invoke(callbackResults);
        }

        void CreateProfileWidget(AppData.ButtonDataPackets dataPackets, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (renderProfileUIHandlerPrefab != null)
            {
                GameObject profileAsset = Instantiate(renderProfileUIHandlerPrefab.gameObject);

                RenderProfileUIHandler renderProfile = profileAsset.GetComponent<RenderProfileUIHandler>();

                if (renderProfile != null)
                {
                    renderProfile.Initialize(GetNewRenderProfileID());
                    renderProfileUIHandlerComponentsList.Add(renderProfile);
                }
                else
                    renderProfileUIHandlerComponentsList.Add(renderProfile = profileAsset.AddComponent<RenderProfileUIHandler>());

                AddContentToDynamicWidgetContainer(profileAsset.GetComponent<AppData.UIScreenWidget>(), dataPackets.containerType, dataPackets.containerContentOrientation);

                callbackResults.results = "Render Profile Created Successfully.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "CreateProfileWidget Failed : renderProfileUIHandlerPrefab Is Null.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void Duplicate(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            callbackResults.results = "----------------> Duplicating A Profile";

            callback.Invoke(callbackResults);
        }

        public void ClearAllRenderProfiles(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            callbackResults.results = "----------------> Clearing All User Profiles";

            callback.Invoke(callbackResults);
        }

        public void SetNewRenderProfileID(AppData.NavigationRenderSettingsProfileID profileID)
        {
            this.profileID = profileID;
        }

        public AppData.NavigationRenderSettingsProfileID GetNewRenderProfileID()
        {
            return profileID;
        }

        #endregion

        #endregion

        #region Color Swatch Data

        public void GetColorFromHexidecimal(string hexadecimal, Action<AppData.CallbackData<AppData.ColorInfo>> callback)
        {
            AppData.CallbackData<AppData.ColorInfo> callbackResults = new AppData.CallbackData<AppData.ColorInfo>();

            if (!string.IsNullOrEmpty(hexadecimal))
            {
                Color color;
                string htmlString = hexadecimal;

                if (!htmlString.Contains("#"))
                    htmlString = "#" + htmlString;

                if (ColorUtility.TryParseHtmlString(htmlString, out color))
                {
                    AppData.ColorInfo colorInfo = new AppData.ColorInfo
                    {
                        color = color,
                        hexadecimal = hexadecimal
                    };

                    callbackResults.results = $"Get Color From Hexadecimal Success : Returning Color For Hexadecimal Value : {hexadecimal}.";
                    callbackResults.data = colorInfo;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Get Color From Hexadecimal Failed : Couldn't Try Parse Html String : {hexadecimal} Using Unity's Color Utility Class.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = $"Get Color From Hexadecimal Failed : Invalid Hexadecimal Value - Parameter Value Is Null / Empty.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void GetHexidecimalFromColor(Color color, Action<AppData.CallbackData<AppData.ColorInfo>> callback)
        {
            AppData.CallbackData<AppData.ColorInfo> callbackResults = new AppData.CallbackData<AppData.ColorInfo>();

            AppData.ColorInfo colorInfo = new AppData.ColorInfo();

            colorInfo.hexadecimal = ColorUtility.ToHtmlStringRGBA(color);

            if (!string.IsNullOrEmpty(colorInfo.hexadecimal))
            {
                GetColorFromHexidecimal(colorInfo.hexadecimal, (getColorCallback) =>
                {
                    callbackResults = getColorCallback;
                });
            }
            else
            {
                callbackResults.results = $"Get Hexidecimal From Color Failed : Couldn't Get Data.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public Color[] GetColorSpectrum(int spectrumSize)
        {
            Color[] colors = new Color[spectrumSize];

            for (int i = 0; i < spectrumSize; i++)
                GetColorFromHexidecimal(AppData.Helpers.GetColorGradientHexadecimal(spectrumSize, i), (callbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                        colors[i] = callbackResults.data.color;
                    else
                        Debug.LogError("-----------------> Color Not Found");
                });


            return colors;
        }

        public List<AppData.ColorInfo> GetColorInfoSpectrum(int spectrumSize)
        {
            List<AppData.ColorInfo> colors = new List<AppData.ColorInfo>();

            for (int i = 0; i < spectrumSize; i++)
                GetColorFromHexidecimal(AppData.Helpers.GetColorGradientHexadecimal(spectrumSize, i), (callbackResults) =>
                {
                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                        colors.Add(callbackResults.data);
                    else
                        Debug.LogError("-----------------> Color Not Found");
                });


            return colors;
        }

        public void OnInitializeColorSwatchData(string fileName)
        {
            colorSwatchData.Init(fileName, (swatchCreated) =>
            {
                if (AppData.Helpers.IsSuccessCode(swatchCreated.resultsCode))
                {
                    CreateColorSwatchContent(swatchCreated.data, AppData.ContentContainerType.ColorSwatches, AppData.OrientationType.VerticalGrid, (callback) =>
                    {
                        if (AppData.Helpers.IsSuccessCode(callback.resultsCode))
                            Debug.Log($"--------------> CreateColorSwatchContent Success With Results : {swatchCreated.results}");
                        else
                            Debug.LogError($"-----------> CreateColorSwatchContent Failed With Results : {callback.results}");
                    });
                }
                else
                    Debug.LogError($"--> Failed To Create Swatch Drop Down Data With Results : {swatchCreated.results}");
            });
        }

        public void CreateColorInfoContent(AppData.ColorInfo colorInfo, string swatchName, AppData.ContentContainerType containerType = AppData.ContentContainerType.ColorSwatches, AppData.OrientationType containerOrientation = AppData.OrientationType.Default, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (colorSwatchData.swatches != null && colorSwatchData.swatches.Count > 0)
            {
                if (!string.IsNullOrEmpty(swatchName))
                {
                    AppData.ColorSwatch swatch = colorSwatchData.swatches.Find((x) => x.name == swatchName);

                    if (swatch.colorIDList != null)
                    {
                        CreateDynamicScreenContent(colorSwatchButtonHandlerPrefab, containerType, containerOrientation, (callbackDataResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(callbackDataResults.resultsCode))
                            {
                                ColorSwatchButtonHandler swatchHandler = callbackDataResults.data as ColorSwatchButtonHandler;

                                if (swatchHandler != null)
                                {
                                    swatchHandler.Initialize(colorInfo, (initializationCallback) =>
                                    {
                                        callbackResults = initializationCallback;

                                        if (AppData.Helpers.IsSuccessCode(initializationCallback.resultsCode))
                                        {
                                            colorSwatchData.AddColorInfoToLibrary(colorInfo);

                                            swatchHandler.SetSwatchID(swatchName);

                                            AppData.ColorSwatchPallet swatchPallet = colorSwatchData.GetColorSwatchPallet(swatchName);

                                            if (swatchPallet != null)
                                            {
                                                swatchPallet.AddSwatchButton(swatchHandler);
                                                Debug.Log($"-----------------> Color Pallet : {swatchPallet.name}");
                                            }
                                            else
                                                Debug.LogWarning($"--> CreateColorInfoContent Failed : Swatch Pallet : {swatchName} Not Found.");
                                        }
                                    });
                                }
                                else
                                {
                                    callbackResults.results = "CreateColorSwatchContent Failed : Swatch Handler Component Is Missing / Null.";
                                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                }

                            // Add To Custom Swatch.
                            //if (callbackResults.success)
                            //{
                            //    AppData.ColorSwatchPallet swatchPallet = GetColorSwatchPallet(swatchName);
                            //}

                        }
                            else
                            {
                                callbackResults.results = $"Color Swatches Failed With Results : {callbackDataResults.results}";
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        });
                    }
                    else
                    {
                        callbackResults.results = "CreateColorSwatchContent Failed : Swatch Color ID List Is Null.";
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "CreateColorSwatchContent Failed : Swatch Name Is Null / Empty.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "CreateColorSwatchContent Failed : colorSwatchData Swatches Is Null.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void CreateColorSwatchContent(string swatchName, AppData.ContentContainerType containerType = AppData.ContentContainerType.ColorSwatches, AppData.OrientationType containerOrientation = AppData.OrientationType.Default, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (colorSwatchData.swatches != null && colorSwatchData.swatches.Count > 0)
            {
                if (!string.IsNullOrEmpty(swatchName))
                {
                    if (colorSwatchData.SwatchDataLoadedSuccessfully())
                    {
                        AppData.SwatchData swatchData = colorSwatchData.GetLoadedSwatchData();

                        AppData.ColorSwatch swatch = swatchData.swatches.Find((x) => x.name == swatchName);

                        AppData.ColorSwatchPallet swatchPallet = new AppData.ColorSwatchPallet();

                        if (swatch.colorIDList != null)
                        {
                            CreateDynamicScreenContents(colorSwatchButtonHandlerPrefab, swatch.colorIDList, containerType, containerOrientation, (callback) =>
                            {
                                if (AppData.Helpers.IsSuccessCode(callback.resultsCode))
                                {
                                    swatchPallet.name = swatch.name;

                                    foreach (var item in callback.data)
                                    {
                                        ColorSwatchButtonHandler swatchHandler = item as ColorSwatchButtonHandler;

                                        if (swatchHandler != null)
                                        {
                                            swatchHandler.Initialize(swatch.colorIDList[callback.data.IndexOf(item)], (initializationCallback) =>
                                            {
                                                callbackResults = initializationCallback;

                                                if (AppData.Helpers.IsSuccessCode(initializationCallback.resultsCode))
                                                {
                                                    swatchHandler.SetSwatchID(swatchName);
                                                    swatchPallet.AddSwatchButton(swatchHandler);

                                                    colorSwatchData.AddColorInfoToLibrary(swatch.colorIDList[callback.data.IndexOf(item)]);
                                                }
                                            });
                                        }
                                        else
                                        {
                                            callbackResults.results = "CreateColorSwatchContent Failed : Swatch Handler Component Is Missing / Null.";
                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                        }

                                    }

                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                        if (swatchPallet.swatchButtonList.Count > 0)
                                            colorSwatchData.AddPallet(swatchPallet);
                                }
                                else
                                {
                                    callbackResults.results = $"Color Swatches Failed With Results : {callback.results}";
                                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                }
                            });
                        }
                        else
                        {
                            callbackResults.results = "CreateColorSwatchContent Failed : Swatch Color ID List Is Null.";
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = "CreateColorSwatchContent Failed : Color Swatch Data Has Failed To Load.";
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "CreateColorSwatchContent Failed : Swatch Name Is Null / Empty.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "CreateColorSwatchContent Failed : colorSwatchData Swatches Is Null.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void GetColorSwatchData(Action<AppData.CallbackData<AppData.ColorSwatchData>> callback)
        {
            AppData.CallbackData<AppData.ColorSwatchData> callbackResults = new AppData.CallbackData<AppData.ColorSwatchData>();

            if (colorSwatchData.swatches.Count > 0 && colorSwatchData.swatchDropDownList.Count > 0)
            {
                callbackResults.results = "CreateColorSwatchContent Success : Color Swatch Data Initialized.";
                callbackResults.data = colorSwatchData;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "CreateColorSwatchContent Failed : Color Swatch Data Swatches / Color Swatch Data swatchDropDownList Null.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void SelectColorSwatchPallet(string fileName, string swatchName, Action<AppData.CallbackData<string>> callback)
        {
            AppData.CallbackData<string> callbackResults = new AppData.CallbackData<string>();

            if (colorSwatchData.SwatchPalletExist(swatchName))
            {
                colorSwatchData.ShowPallet(swatchName);

                callbackResults.results = "Select Color Swatch Pallet Selected Successful.";
                callbackResults.data = swatchName;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                // Create New data
                colorSwatchData.Init(fileName, (swatchCreated) =>
                {
                    if (AppData.Helpers.IsSuccessCode(swatchCreated.resultsCode))
                    {
                        CreateColorSwatchContent(swatchName, AppData.ContentContainerType.ColorSwatches, AppData.OrientationType.VerticalGrid, (callbackDataResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(callbackDataResults.resultsCode))
                            {
                                colorSwatchData.ShowPallet(swatchName);

                                callbackResults.results = "SelectColorSwatchPallet Selected Successful.";
                                callbackResults.data = swatchName;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = $"SelectColorSwatchPallet Failed With Results : {callbackDataResults.results}.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        });
                    }
                    else
                    {
                        callbackResults.results = $"SelectColorSwatchPallet  Failed To Create Swatch Drop Down Data With Results : {swatchCreated.results}.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                });
            }

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Dynamic Content

        void CreateDynamicScreenContent(AppData.UIScreenWidget contentPrefab, AppData.ContentContainerType containerType, AppData.OrientationType containerOrientation, Action<AppData.CallbackData<AppData.UIScreenWidget>> callback = null)
        {
            AppData.CallbackData<AppData.UIScreenWidget> callbackResults = new AppData.CallbackData<AppData.UIScreenWidget>();

            if (contentPrefab != null)
            {
                GameObject content = Instantiate(contentPrefab.GetSceneAssetObject());

                if (content)
                {
                    content.name = GetFormattedName(content.name, new List<string>() { "(Clone)" });

                    AppData.UIScreenWidget contentComponent = content.GetComponent<AppData.UIScreenWidget>();

                    if (contentComponent != null)
                    {
                        AddContentToDynamicWidgetContainer(contentComponent, containerType, containerOrientation);

                        callbackResults.data = contentComponent;
                        callbackResults.results = $"CreateDynamicScreenContent Success : Content : {content.name} Instantiated.";
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }

                    if (contentComponent == null)
                    {
                        callbackResults.data = default;
                        callbackResults.results = $"CreateDynamicScreenContent Failed : Content Component Type : {contentPrefab.GetType()} Doesn't Match Required Components.";
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.data = default;
                    callbackResults.results = $"CreateDynamicScreenContent Failed : Content Failed To Instantiate.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.data = default;
                callbackResults.results = "CreateDynamicScreenContent Failed : Content Prefab Is Not Found / Null.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        void CreateDynamicScreenContents<U>(AppData.UIScreenWidget contentPrefab, List<U> contents, AppData.ContentContainerType containerType, AppData.OrientationType containerOrientation, Action<AppData.CallbackDatas<AppData.UIScreenWidget>> callback = null)
        {
            AppData.CallbackDatas<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDatas<AppData.UIScreenWidget>();

            if (contentPrefab != null)
            {
                List<AppData.UIScreenWidget> createdContentList = new List<AppData.UIScreenWidget>();

                for (int i = 0; i < contents.Count; i++)
                {
                    GameObject content = Instantiate(contentPrefab.GetSceneAssetObject());

                    if (content)
                    {
                        content.name = GetFormattedName(content.name, new List<string>() { "(Clone)" });

                        AppData.UIScreenWidget contentComponent = content.GetComponent<AppData.UIScreenWidget>();

                        if (contentComponent != null)
                        {
                            if (!createdContentList.Contains(contentComponent))
                                createdContentList.Add(contentComponent);

                            AddContentToDynamicWidgetContainer(contentComponent, containerType, containerOrientation);

                            callbackResults.data = createdContentList;
                            callbackResults.results = $"CreateDynamicScreenContent Success : Content : {content.name} Instantiated.";
                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                        }

                        if (contentComponent == null)
                        {
                            callbackResults.data = default;
                            callbackResults.results = $"CreateDynamicScreenContent Failed : Content Component Type : {contentPrefab.GetType()} Doesn't Match Required Components.";
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.data = default;
                        callbackResults.results = $"CreateDynamicScreenContent Failed : Content Failed To Instantiate.";
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }

                if (createdContentList.Count == contents.Count)
                {
                    callbackResults.data = createdContentList;
                    callbackResults.results = $"CreateDynamicScreenContent Success :{createdContentList.Count} Content(s) Created Inside Container : {containerType}.";
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.data = default;
                    callbackResults.results = $"CreateDynamicScreenContent Failed : { contents.Count} Content(s) Failed To Create.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.data = default;
                callbackResults.results = "CreateDynamicScreenContent Failed : Content Prefab Is Not Found / Null.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        #endregion

        #region Data Initialization

        public void CreateData<T>(T data, AppData.StorageDirectoryData directoryData, Action<AppData.CallbackData<T>> callback) where T : AppData.SerializableData
        {
            AppData.CallbackData<T> callbackResults = new AppData.CallbackData<T>();

            DirectoryFound(directoryData.directory, directoryCheckCallback =>
            {
                if (AppData.Helpers.IsSuccessCode(directoryCheckCallback.resultsCode))
                {
                    if (string.IsNullOrEmpty(data.name))
                        data.name = data.GetType().ToString();

                    string storageDirectory = data.storageData.directory;

                    string fileNameWithJSONExtension = data.storageData.name + ".json";
                    string filePath = Path.Combine(directoryData.directory, fileNameWithJSONExtension);
                    string formattedFilePath = filePath.Replace("\\", "/");

                    data.storageData.path = formattedFilePath;
                    data.storageData.directory = storageDirectory;

                    string JSONString = JsonUtility.ToJson(data);

                    if (!string.IsNullOrEmpty(JSONString))
                    {
                        if (!File.Exists(formattedFilePath))
                        {
                            File.WriteAllText(formattedFilePath, JSONString);

                            callbackResults.results = $"-->  Create New Data Success : : {data.name} As : {formattedFilePath}";
                            callbackResults.data = data;
                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                        }
                        else
                        {
                            File.Delete(formattedFilePath);

                            if (!File.Exists(formattedFilePath))
                                File.WriteAllText(formattedFilePath, JSONString);

                            callbackResults.results = $"--> Create New Data Success : Replaced Asset : {data.name} At Path : {formattedFilePath}";
                            callbackResults.data = data;
                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"--> Failed To Create A JSON File.";
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = directoryCheckCallback.results;
                    callbackResults.resultsCode = directoryCheckCallback.resultsCode;
                }
            });

            callback.Invoke(callbackResults);
        }

        public void SaveData<T>(T data, AppData.StorageDirectoryData storageData, Action<AppData.CallbackData<T>> callback) where T : AppData.SerializableData
        {
            AppData.CallbackData<T> callbackResults = new AppData.CallbackData<T>();

            FileFound(storageData.path, checkFileFoundCallback =>
            {
                if (AppData.Helpers.IsSuccessCode(checkFileFoundCallback.resultsCode))
                {
                    Debug.LogError($"==> File : {data.name} Found At Directory : {storageData.path}");

                    File.Delete(storageData.path);

                    FileFound(storageData.path, checkFileFoundCallback =>
                    {
                        if (!AppData.Helpers.IsSuccessCode(checkFileFoundCallback.resultsCode))
                        {
                            string JSONString = JsonUtility.ToJson(data);
                            File.WriteAllText(storageData.path, JSONString);

                            FileFound(storageData.path, checkFileFoundCallback =>
                            {
                                if (AppData.Helpers.IsSuccessCode(checkFileFoundCallback.resultsCode))
                                {
                                    Debug.Log($"--> Replaced File : {data.name} At Directory : {storageData.path}");
                                    callbackResults.data = data;
                                }
                                else
                                {
                                    callbackResults.results = $"Couldn't Replace File : {data.name} At Directory : {storageData.path} - Check Here Please.";
                                    callbackResults.data = default;
                                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                }
                            });
                        }
                        else
                        {
                            Debug.LogError($"==> File : {data.name} Couldn't Delete From Directory : {storageData.path}");

                            callbackResults.results = $"Couldn't Delete File : {data.name} From Directory : {storageData.path} - Check Here Please.";
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    });
                }
                else
                {
                    callbackResults.results = $"Couldn't Save File : {data.name} At Directory : {storageData.path} - File Doesn't Exist, Create File First Before Attempting To Save.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            });

            callback.Invoke(callbackResults);
        }

        public void LoadData<T>(AppData.StorageDirectoryData directoryData, Action<AppData.CallbackData<T>> callback) where T : AppData.SerializableData
        {
            AppData.CallbackData<T> callbackResults = new AppData.CallbackData<T>();

            FileFound(directoryData.path, checkFileExistenceCallback =>
            {
                if (AppData.Helpers.IsSuccessCode(checkFileExistenceCallback.resultsCode))
                {
                    string JSONString = File.ReadAllText(directoryData.path);
                    T data = JsonUtility.FromJson<T>(JSONString);

                    callbackResults.results = checkFileExistenceCallback.results;
                    callbackResults.data = data;
                    callbackResults.resultsCode = checkFileExistenceCallback.resultsCode;
                }
                else
                {
                    callbackResults.results = checkFileExistenceCallback.results;
                    callbackResults.data = default;
                    callbackResults.resultsCode = checkFileExistenceCallback.resultsCode;
                }
            });

            callback.Invoke(callbackResults);
        }

        public void LoadData<T>(string fileName, AppData.StorageDirectoryData directoryData, Action<AppData.CallbackData<T>> callback) where T : AppData.SerializableData
        {
            AppData.CallbackData<T> callbackResults = new AppData.CallbackData<T>();

            if (DirectoryFound(directoryData))
            {
                string[] files = Directory.GetFiles(directoryData.directory, "*.json");

                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        string JSONString = File.ReadAllText(file);
                        T data = JsonUtility.FromJson<T>(JSONString);

                        if (data.name == fileName)
                        {
                            callbackResults.results = "Data Loaded Successfully.";
                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            callbackResults.data = data;
                            break;
                        }
                        else
                        {
                            callbackResults.results = $"--> File : {fileName} Not Loaded From Directory : {directoryData.directory} - Loaded Data Is Null / Empty.";
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            callbackResults.data = default;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"--> No Files Found In Directory : {directoryData.directory}";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    callbackResults.data = default;
                }
            }
            else
            {
                callbackResults.results = $"Load Data Failed : Directory : {directoryData.directory} Not Found.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                callbackResults.data = default;
            }

            callback.Invoke(callbackResults);
        }

        public void SaveData<T>(T data, Action<AppData.Callback> callback = null) where T : AppData.SerializableData
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (File.Exists(data.storageData.path))
            {
                string JSONString = JsonUtility.ToJson(data);

                if (!string.IsNullOrEmpty(JSONString))
                {
                    if (!File.Exists(data.storageData.path))
                    {
                        File.WriteAllText(data.storageData.path, JSONString);

                        callbackResults.results = $"-->  Save Data Success : : {data.name} As : {data.storageData}";
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        File.Delete(data.storageData.path);

                        if (!File.Exists(data.storageData.path))
                            File.WriteAllText(data.storageData.path, JSONString);

                        callbackResults.results = $"--> Create New Data Success : Replaced Asset : {data.name} At Path : {data.storageData}";
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                }
                else
                {
                    callbackResults.results = $"--> Failed To Create A JSON File.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callbackResults.results = $"Sucees - File Saved in Directory : {data.storageData}";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = $"Save data Failed : File Not found In Directory : {data.storageData}";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        #endregion
    }
}
