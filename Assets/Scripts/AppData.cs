using Dummiesman;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Com.RedicalGames.Filar
{

    public class AppData
    {
        public enum AssetFieldType
        {
            None,
            OBJFile,
            Thumbnail,
            MainTexture,
            NormalMap,
            AmbientOcclusionMap,
            MTLFile,
            Image,
            HDRI
        }

        public enum AssetFileExtensionType
        {
            OBJ,
            PNG,
            JPG,
            JPEG,
            MTL
        }

        public enum SceneAssetScaleDirection
        {
            None,
            Up,
            Down
        }

        public enum SceneAssetCategoryType : int
        {
            None = 0,
            Animals = 1,
            Aircraft = 2,
            Architectural = 3,
            Car = 4,
            Character = 5,
            Exterior = 6,
            Furniture = 7,
            Household = 8,
            Industrial = 9,
            Interior = 10,
            Space = 11,
            Vehicle = 12,
        }

        public enum SceneAssetSortType : int
        {
            Ascending = 0,
            Category = 1,
            Descending = 2,
            DateModified = 3
        }

        public enum RuntimeInputs
        {
            Mobile,
            PC,
            Editor
        }

        public enum ImageExtension
        {
            none,
            png,
            jpeg,
            jpg
        }

        public enum WidgetType
        {
            None,
            ConfirmationPopWidget,
            SliderValueWidget,
            WarningPromptWidget,
            SceneAssetPreviewWidget,
            SceneAssetPropertiesWidget,
            AssetImportWidget,
            PermissionsRequestWidget,
            DeleteAssetWidget,
            SceneAssetExportWidget,
            RenderSettingsWidget,
            AssetPublishingWidget,
            NetworkNotificationWidget,
            ColorPickerWidget,
            SnapShotWidget,
            FolderCreationWidget,
            FileSelectionOptionsWidget,
            ScrollerNavigationWidget,
            UITextDisplayerWidget,
            PagerNavigationWidget,
            UIAssetActionWarningWidget,
            UIAssetRenameWidget,
            SelectionOptionsWidget
        }

        public enum SubWidgetType
        {
            None,
            SingleAssetDeleteWidget,
            MultipleAssetsDeleteWidget
        }

        public enum AssetFieldSettingsType
        {
            None,
            NormalMapSettings,
            AOMapSettings,
            MainTextureSettings
        }

        public enum InputActionButtonType
        {
            Confirm,
            Cancel,
            OpenPopUp,
            HideScreenWidget,
            Undo,
            Edit,
            Delete,
            Hide,
            Remove,
            Info,
            Add,
            CreateNewAsset,
            BuildNewAsset,
            OpenFilePicker_OBJ,
            OpenFilePicker_Thumbnail,
            OpenFilePicker_MainTexture,
            OpenFilePicker_NormalMap,
            OpenFilePicker_AOMap,
            Return,
            OpenSceneAssetPreview,
            OpenARView,
            ResetAssetPreviewPose,
            GoToProfile,
            PlaceItemInAR,
            CaptureSnapShot,
            ExportAsset,
            OpenRenderSettings,
            PublishAsset,
            HideNavigationScreenWidget,
            OpenColorPicker,
            CreateNewProfileButton,
            ClearAllButton,
            DuplicateButton,
            ColorPickerButton,
            CreateNewColorButton,
            RevertSettingsButton,
            UndoActionButton,
            RedoActionButton,
            ImportColorButton,
            OpenFilePicker_Image,
            ClearButton,
            GenerateColorSwatchButton,
            RetryButton,
            OpenColorPromptWidget,
            Randomize,
            VoiceInputButton,
            SkyboxSelectionButton,
            CreateSkyboxButton,
            OpenFilePicker_HDRI,
            CreateNewFolderButton,
            ChangeLayoutViewButton,
            OpenFolderButton,
            FolderReturnButton,
            Copy_PasteButton,
            Show_HideButton,
            ScrollToTopButton,
            RefreshButton,
            PaginationButton,
            HomeButton,
            ScrollToBottomButton,
            NextNavigationButton,
            PreviousNavigationButton,
            PinButton,
            FolderActionButton,
            RenameButton,
            ReplaceButton,
            SelectionOptionsButton,
            SelectionButton,
            DeselectButton,
            None
        }

        public enum SelectableWidgetType
        {
            ActionButton,
            ActionWidget
        }

        public enum InputUIState
        {
            Normal,
            Enabled,
            Disabled,
            Selected,
            Deselected,
            Shown,
            Hidden,
            Deselect,
            Hovered,
            Highlighted,
            Focused,
            Pressed,
            Pinned
        }

        public enum SettingsWidgetType
        {
            MainColorSettingsWidget,
            ColorSettingsImportWidget,
            CustomSwatchConfirmationWidget,
            ColorGeneratorInfoWidget,
            LoadingSpinnerWidget,
            ScreenWarningInfoWidget,
            ColorPickerPromptWidget,
            SkyboxCreationWidget
        }

        public enum NavigationTabID
        {
            LightSettings,
            PostProcessingSettings,
            RenderingSettings,
            PropertiesSettings,
            Default
        }

        public enum NavigationWidgetVisibilityState
        {
            Hide,
            Show
        }

        public enum NavigationTabType
        {
            CreateRenderProfileWidget,
            ColorPalletWidget,
            SkyboxCreationWidget,
            None
        }

        public enum NavigationRenderSettingsProfileID
        {
            None,
            Bloom,
            Color_Grading,
            Noise,
            Ambient_Occlusion
        }

        public enum InputDropDownActionType
        {
            FilterList,
            SceneAssetRenderMode,
            SortingList,
            ExtensionsList,
            RenderingProfileType,
            SwatchPicker,
            ColorPickerSelection,
            ColorModeSelection,
            SettingsSelectionType,
            RotationalDirection,
            None
        }

        public enum InputFieldActionType
        {
            AssetDescriptionField,
            AssetNameField,
            AssetSearchField,
            UserNameField,
            UserEmailField,
            UserPasswordField,
            XRotationPropertyField,
            YRotationPropertyField,
            ZRotationPropertyField,
            ColorValueField,
            ColorHexidecimalField,
            ColorReferenceImageURLField,
            ColorPromptField,
            InputPageNumberField
        }

        public enum CheckboxInputActionType
        {
            DontShowAgain,
            ToggleAssetField,
            TriangulateWireframe,
            ToggleColorDropPicker,
            ToggleVoiceInput,
            InverseSelection
        }

        public enum ScreenUITextType
        {
            TitleDisplayer,
            MessageDisplayer
        }

        public enum InputSliderActionType
        {
            None,
            RedColorChannelField,
            GreenColorChannelField,
            BlueColorChannelField,
            AlphaColorChannelField
        }

        public enum SettingsWidgetTabID
        {
            Swatches,
            Gradient,
            Lighting,
            ColorInfo,
            General
        }

        public enum ColorSpaceType
        {
            RGBA,
            RGB,
            HSV
        }

        public enum ColorPickerType
        {
            Swatches,
            Gradient
        }

        public enum SkyboxSettingsType
        {
            Lighting,
            ColorInfo,
            General
        }

        public enum RotationalDirection
        {
            Clockwise,
            AntiClockwise
        }

        public enum InputSliderValueType
        {
            InputField,
            Slider
        }

        public enum ToggleActionType
        {
            ThumbnailState,
            MainTextureState,
            NormalMapState,
            AOState
        }

        public enum ScreenTextType
        {
            ErrorNotification,
            ResultsNotFound,
            Toaster,
            WarningNotification,
            TitleDisplayer,
            MessageDisplayer,
            FileCountDisplayer,
            TimeDateDisplayer,
            InfoDisplayer,
            NavigationRootTitleDisplayer,
            PageCountDisplayer
        }

        public enum SliderValueType
        {
            MaterialBumpScaleValue,
            MaterialGlossinessValue,
            MaterialOcclusionIntensityValue,
            SceneAssetScale,
            ColorValue,
            LightIntensity,
            SkyboxExposure,
            SkyboxRotationSpeed,
            None
        }

        public enum ScreenImageType
        {
            Thumbnail,
            ScreenSnap
        }

        public enum UIStateType
        {
            InteractivityState,
            VisibilityState
        }

        public enum ColorValueType
        {
            None,
            Red,
            Green,
            Blue,
            Alpha,
            Hexidecimal,
            Hue,
            Saturation,
            Value
        }

        public enum InputType
        {
            None,
            Button,
            Checkbox,
            Input,
            Slider,
            InputSlider,
            DropDown,
            Text
        }

        public enum UIScreenType
        {
            None,
            ProjectViewScreen,
            AssetCreationScreen,
            ARViewScreen,
            HomeScreen
        }

        public enum ContentContainerType
        {
            ARPreview,
            AssetImport,
            AssetPreview,
            FocusContentView,
            RenderProfileContent,
            ColorSwatches,
            SkyboxContent,
            FolderStuctureContent,
            None
        }

        public enum LoadingItemType
        {
            Bar,
            Spinner,
            Text
        }

        // Rename To Proper Types.
        public enum ColorSwatchType
        {
            Default,
            Custom
        }

        public enum DirectionType
        {
            Up,
            Down,
            Left,
            Right,
            Default
        }

        public enum DirectionAxisType
        {
            None,
            Horizontal,
            Vertical
        }

        public enum OrientationType
        {
            Vertical,
            Horizontal,
            VerticalGrid,
            HorizontalGrid,
            Default,
        }

        public enum DirectoryType
        {
            None,
            Default_App_Storage,
            Object_Asset_Storage,
            Image_Asset_Storage,
            Meta_File_Storage,
            Settings_Storage,
            Folder_Structure,
            Sub_Folder_Structure
        }

        public enum SceneAssetModeType
        {
            None,
            CreateMode,
            EditMode,
            PreviewMode,
            ARMode
        }

        public enum InfoDisplayerFieldType
        {
            None,
            Title,
            TriangleCounter,
            VerticesCounter
        }

        public enum MaterialTextureType
        {
            MainTexture,
            NormalMapTexture,
            AOMapTexture
        }

        public enum SceneAssetRenderMode
        {
            Shaded,
            Wireframe
        }

        public enum PreviewOrbitWidgetType
        {
            SceneAssetOrbitWidget,
            SkyboxOrbitWidget
        }

        public enum RendererMaterialType
        {
            DefaultMaterial,
            SelectionMaterial,
            SelectionWireframeMaterial,
            TriangulatedWireframeMaterial,
            WireframeMaterial
        }

        public enum SceneEventCameraType
        {
            None,
            ARViewCamera,
            AssetPreviewCamera
        }

        public enum ScreenWidgetTransitionType
        {
            PopUp,
            Slide
        }

        public enum BuildType
        {
            Runtime,
            Editor
        }

        public enum RuntimeValueType
        {
            InspectorModePanSpeed,
            PreviewModeOrbitSpeed,
            ARModeTranslateSpeed,
            ARModeRotateSpeed,
            ARModeScaleSpeed,
            ARModeAsseScaleDeviderValue,
            InspectorModeAsseScaleDeviderValue,
            PreviewModeAsseScaleDeviderValue,
            DefaultScreenRefreshDuration,
            DefaultAssetCreationYieldValue,
            PreviewWidgetOrbitSpeed,
            NotificationTransitionalSpeed,
            NotificationDelay,
            NotificationDuration,
            ScreenWidgetTransitionalSpeed,
            ScreenWidgetTransitionalSnapSpeed,
            ScreenWidgetShowDelayValue,
            ScreenWidgetHideDelayValue,
            ScrollToFocusedPositionSpeedValue,
            EdgeScrollSpeedValue,
            ScrollToTopSpeedValue,
            ScrollbarFadeInSpeed,
            ScrollbarFadeOutSpeed,
            ScrollBarFadeDelayDuration,
            HighlightHoveredFolderDistance,
            SnapDraggedWidgetToHoveredFolderDistance
        }


        public enum EventCameraState
        {
            FocusedMode,
            InspectorMode
        }

        public enum SceneMode
        {
            None,
            ARMode,
            PreviewMode,
            EditMode,
        }

        public enum PermissionType
        {
            None,
            Camera,
            Storage
        }

        public enum TogglableWidgetType
        {
            None,
            ResetAssetModelRotationButton
        }

        public enum SceneAssetInteractableMode
        {
            All,
            Rotation,
            Orbit
        }

        public enum SceneARSessionState
        {
            None,
            TrackingFound,
            TrackingInProgress,
            TrackingLost,
            AssetPlaced
        }

        public enum ARSceneContentState
        {
            None,
            Place,
            Remove
        }

        public enum SceneAssetPivot
        {
            BottomCenter,
            BottomLeft,
            BottomRight,
            MiddleCenter,
            TopCenter,
            TopLeft,
            TopRight
        }

        public enum ExportExtensionType
        {
            GLTF2_0,
            FBX,
            OBJ
        }

        public enum ShaderType
        {
            Default,
            Sculpting,
            Skybox,
            Wireframe
        }

        // --> Add This Manualy To List In Scene Assets Manager.
        public enum DropDownContentType
        {
            Categories,
            Extensions,
            Sorting,
            RenderingModes,
            RenderProfiles,
            ColorSpaces,
            ColorPickers,
            SkyboxSettings,
            Directions
        }

        public enum ScreenBlurContainerLayerType
        {
            Default,
            Background,
            ForeGround,
            None

        }

        public enum ScreenViewState
        {
            None,
            Blurred,
            Focused,
            Overlayed
        }

        public enum NotificationType
        {
            Info,
            Objectives,
            Tooltip,
            Hints
        }

        public enum AudioType
        {
            CameraShutter
        }

        public enum UIScreenAssetType
        {
            File,
            Folder
        }

        public enum InputFieldValueType
        {
            String,
            Integer
        }

        public enum FolderStructureType
        {
            MainFolder,
            SubFolder
        }

        public enum LayoutViewType
        {
            ItemView,
            ListView
        }

        public enum PaginationViewType
        {
            Pager,
            Scroller
        }

        public enum PaginationNavigationActionType
        {
            GoToNextPage,
            GoToPreviousPage
        }

        public enum UIImageDisplayerType
        {
            ButtonIcon,
            ItemThumbnail,
            SelectionFrame,
            ActionIcon,
            ItemIcon,
            InteractionIcon,
            BadgeIcon,
            NotificationIcon,
            PinnedIcon,
            None
        }

        public enum UIImageType
        {
            ItemViewIcon,
            ListViewIcon,
            HomeIcon,
            ReturnIcon,
            CancelIcon,
            FolderIcon,
            EmptyFolderIcon,
            MultiFilesFolderIcon,
            PagerIcon,
            ScrollerIcon,
            UIWidget_MoveIcon,
            Null_TransparentIcon,
            PinEnabledIcon,
            PinDisabledIcon,
            ItemViewSelectionIcon,
            ItemViewDeselectionIcon,
            ListViewSelectionIcon,
            ListViewDeselectionIcon
        }

        public enum SelectableAssetType
        {
            File,
            Folder,
            PlaceHolder
        }

        public enum SelectionOption
        {
            Default,
            SelectAll,
            SelectPage
        }

        public enum FocusedWidgetOrderType
        {
            First,
            Last,
            Default
        }

        public enum UIWidgetVisibilityState
        {
            Visible,
            Hidden
        }

        public enum DefaultUIWidgetActionState
        {
            Default,
            Pinned,
            Hidden
        }

        public enum FocusedSelectionType
        {
            Default,
            SelectedItem,
            NewItem,
            HoveredItem,
            InteractedItem,
            ModifiedItem
        }

        public enum LogInfoType
        {
            All,
            Info,
            Success,
            Error,
            Warning,
            None
        }

        public enum LogExceptionType
        {
            All,
            Exception,
            NullReference,
            Argument,
            ArgumentNull,
            ArgumentOutOfRange,
            AccessViolation,
            IndexOutOfRange,
            EntryPointNotFound,
            NotImplemented,
            MissingField,
            DllNotFound,
            Aggregate,
            MissingComponent,
            None
        }

        public enum LogAttributeType
        {
            Class,
            Function,
            LogMessage,
            Symbols,
            None
        }

        #region Debugging

        [Serializable]
        public struct MonoLogInfo
        {
            #region Components

            public string attributeName;

            [Space(5)]
            public LogInfoType logType;

            [Space(5)]
            public string logColorValue;

            #endregion
        }

        [Serializable]
        public struct MonoLogAttributes
        {
            #region Components

            public string attributeName;

            [Space(5)]
            public LogAttributeType attributeType;

            [Space(5)]
            public string attributeColorValue;

            [Space(5)]
            public bool isBoldFontWeight;

            #endregion
        }

        #endregion

        #region AR Data Types

        public enum ARFocusType
        {
            Finding,
            Found
        }

        public enum WidgetLayoutViewType
        {
            DefaultView,
            ItemView,
            ListView
        }

        #endregion

        #region AR Data Structs

        [Serializable]
        public struct ARFocusContent
        {
            #region Components

            public GameObject value;

            [Space(5)]
            public GameObject icon;

            [Space(5)]
            public SceneARSessionState sessionState;

            SceneARSessionState previousSessionState;

            #endregion

            #region Main

            public void Show()
            {
                if (icon)
                {
                    if (icon.activeSelf == true)
                        return;

                    icon.SetActive(true);
                }
                else
                    Debug.LogWarning("--> RG_Unity - Show : AR Focus Content Value Is Null.");
            }

            public void Hide()
            {
                if (icon)
                {
                    if (icon.activeSelf == false)
                        return;

                    icon.SetActive(false);
                }
                else
                    Debug.LogWarning("--> RG_Unity - Show : AR Focus Content Value Is Null.");
            }

            public void SetPose(ARSceneTrackedPose trackedPose)
            {
                if (value == null)
                    return;

                value.transform.position = trackedPose.position;
                value.transform.rotation = trackedPose.rotation;
            }

            public void SetSessionState(SceneARSessionState state)
            {
                previousSessionState = sessionState;
                sessionState = state;
            }

            public SceneARSessionState GetSessionState()
            {
                return sessionState;
            }

            public void OnSessionStateChanged()
            {
                if (sessionState == previousSessionState)

                    switch (sessionState)
                    {
                        case SceneARSessionState.AssetPlaced:

                            break;
                    }
            }

            #endregion
        }

        [Serializable]
        public struct ARSceneTrackingData
        {
            public ARSession session;

            [Space(5)]
            public ARRaycastManager rayCastManager;

            [Space(5)]
            public ARFocusContent focusContent;

            [Space(5)]
            public float trackingDistance;

            [Space(5)]
            public float groundOffSet;

            [Space(5)]
            public TrackableType trackableTypes;

            [Space(5)]
            public bool initializeOnStart;

            [HideInInspector]
            public List<ARRaycastHit> hitInfoList;

            [HideInInspector]
            public Vector3 screenCenter;

            public ARSceneTrackedPose trackedPose;
        }

        public struct ARSceneTrackedPose
        {
            public Vector3 position;
            public Quaternion rotation;
        }

        #endregion

        #region UI Image Data

        [Serializable]
        public struct UIImageDisplayer
        {
            public string name;

            [Space(5)]
            public Image value;

            [Space(5)]
            public UIImageDisplayerType imageDisplayerType;
        }

        [Serializable]
        public struct UIImageData
        {
            public string name;

            [Space(5)]
            public Sprite value;

            [Space(5)]
            public UIImageType imageType;
        }

        #endregion

        #region Folder Structure

        [Serializable]
        public class FolderStructureData : SerializableData
        {
            #region Components

            //public List<FolderStructure> structureList = new List<FolderStructure>();

            //[Space(5)]
            //public FolderStructureType initialFOlderStructure = FolderStructureType.MainFolder;

            [Space(5)]
            public Folder rootFolder;

            [Space(5)]
            public List<string> excludedSystemFiles;

            [Space(5)]
            public List<string> excludedSystemFolders;

            [Space(5)]
            public DynamicWidgetsContainer mainFolderDynamicWidgetsContainer = null;

            [Space]
            public LayoutViewType currentLayoutViewType;

            [Space]
            public PaginationViewType currentPaginationViewType;

            [Space(5)]
            public List<FolderLayoutView> layouts = new List<FolderLayoutView>();

            [HideInInspector]
            public List<Folder> folders = new List<Folder>();

            [HideInInspector]
            public bool inverseSelect = false;

            #endregion

            #region Main

            //public void Initialize(Folder folder) => OpenFolderStructure(folder, initialFOlderStructure);

            //public void SetCurrentSelectedFolder(Folder folder) => currentFolder = folder;

            public Folder GetRootFolder()
            {
                return rootFolder;
            }

            public List<string> GetExcludedSystemFileData()
            {
                return excludedSystemFiles;
            }

            public List<string> GetExcludedSystemFolderData()
            {
                return excludedSystemFolders;
            }

            public List<FolderLayoutView> GetFolderLayoutViewList()
            {
                return layouts;
            }

            public void SetCurrentLayoutViewType(LayoutViewType viewType) => currentLayoutViewType = viewType;

            public LayoutViewType GetCurrentLayoutViewType()
            {
                return currentLayoutViewType;
            }

            public void SetCurrentPaginationViewType(PaginationViewType paginationView) => currentPaginationViewType = paginationView;

            public PaginationViewType GetCurrentPaginationViewType()
            {
                return currentPaginationViewType;
            }

            public FolderLayoutView GetFolderLayoutView(LayoutViewType viewType)
            {
                return layouts.Find(layout => layout.viewType == viewType);
            }

            public void SetInverseSelect(bool value) => inverseSelect = value;

            public bool InverseSelect()
            {
                return inverseSelect;
            }

            public DynamicWidgetsContainer GetMainFolderDynamicWidgetsContainer()
            {
                return mainFolderDynamicWidgetsContainer;
            }

            public void AddFolder(Folder folder)
            {
                if (!folders.Contains(folder))
                    folders.Add(folder);
                else
                    Debug.LogWarning("--> Failed : Folder Already Exists In Folders.");
            }

            public void RemodeFolder(Folder folder)
            {
                if (folders.Contains(folder))
                    folders.Remove(folder);
                else
                    Debug.LogWarning("--> Failed : Folder Doesn't Exists In Folders.");
            }

            public void RemoveFolders()
            {
                if (folders.Count > 0)
                    foreach (var folder in folders)
                        folders.Remove(folder);
                else
                    Debug.LogWarning("--> Failed : No Folders Found To Remove.");
            }

            public List<Folder> GetFolders()
            {
                return folders;
            }

            #endregion
        }


        [Serializable]
        public class Folder : SerializableData
        {
            #region Components

            [Space(5)]
            public bool isRootFolder;

            [HideInInspector]
            public DefaultUIWidgetActionState defaultWidgetActionState;

            [HideInInspector]
            public Folder rootFolder;

            //[HideInInspector]
            //public string directory;

            List<string> validFilePath = new List<string>();

            #endregion

            #region Main

            public Folder()
            {

            }

            public Folder(string name, StorageDirectoryData directoryData)
            {
                this.name = name;
                this.storageData = directoryData;
            }

            public bool IsRootFolder()
            {
                if (storageData.type == DirectoryType.Folder_Structure)
                    isRootFolder = true;
                else
                    isRootFolder = false;

                return isRootFolder;
            }

            public StorageDirectoryData GetDirectoryData()
            {
                return storageData;
            }

            public int GetFileCount()
            {
                int fileCount = 0;

                if (Directory.Exists(storageData.directory))
                {

                    validFilePath = new List<string>();
                    string[] files = Directory.GetFiles(storageData.directory);

                    if (files.Length > 0)
                        foreach (var file in files)
                            if (!file.Contains(".meta"))
                                if (!validFilePath.Contains(file))
                                    validFilePath.Add(file);

                    fileCount = validFilePath.Count;
                }
                else
                    Debug.LogError($"Directory : {storageData.directory} Not Valid / Doesn't Exist Or Make Sense Anymore!");

                return fileCount;
            }

            #endregion
        }

        [Serializable]
        public class Folder<T>
        {
            #region Components

            public string name;

            [Space(5)]
            public Dictionary<string, T> files = new Dictionary<string, T>();

            [Space(5)]
            public StorageDirectoryData directoryData;

            #endregion

            #region Main

            public void AddFile(string fileName, T file)
            {
                if (files.ContainsKey(fileName))
                    files.Add(fileName, file);
            }

            public void AddFiles(Dictionary<string, T> files)
            {
                if (files.Count > 0)
                    foreach (var file in files)
                        if (this.files.ContainsKey(file.Key))
                            this.files.Add(file.Key, file.Value);
            }

            public int GetContentCount()
            {
                return files.Count;
            }

            #endregion
        }


        [Serializable]
        public struct FolderLayoutView
        {
            public string name;

            [Space(5)]
            public LayoutView layout;

            [Space(5)]
            public RectOffset padding;

            [Space(5)]
            public LayoutViewType viewType;
        }

        [Serializable]
        public struct UIWidgetInfo
        {
            #region Components

            public string widgetName;
            public Vector2 position;
            public UIScreenDimensions dimensions;
            public InputUIState selectionState;

            #endregion

            #region Main

            public string GetWidgetName()
            {
                return widgetName;
            }

            public Vector2 GetWidgetScreenPosition()
            {
                return position;
            }

            public UIScreenDimensions GetWidgetUIScreenDimensions()
            {
                return dimensions;
            }

            public InputUIState GetSelectionState()
            {
                return selectionState;
            }

            #endregion
        }

        [Serializable]
        public class FolderNavigationCommand : ICommand
        {
            public Folder folder;
            public FolderStructureType structureType;
            public UIWidgetInfo folderWidgetInfo;

            public FolderNavigationCommand(Folder folder, UIWidgetInfo folderWidgetInfo, FolderStructureType structureType)
            {
                this.folder = folder;
                this.structureType = structureType;
                this.folderWidgetInfo = folderWidgetInfo;
            }

            public void Execute() => SceneAssetsManager.Instance.OpenUIFolderStructure(folder, folderWidgetInfo, structureType);

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }


        [Serializable]
        public struct LayoutView
        {
            public Vector2 itemViewSize;

            [Space(5)]
            public Vector2 itemViewSpacing;
        }

        #region Pagination

        [Serializable]
        public class PaginationComponent
        {
            #region Components

            public int itemView_ItemsPerPage;

            [Space(5)]
            public int listView_ItemsPerPage;

            public List<List<UIScreenWidget<SceneDataPackets>>> pages = new List<List<UIScreenWidget<SceneDataPackets>>>();

            public int CurrentPageIndex { get; set; }

            List<UIScreenWidget<SceneDataPackets>> itemList = new List<UIScreenWidget<SceneDataPackets>>();
            List<UIScreenWidget<SceneDataPackets>> currentPage = new List<UIScreenWidget<SceneDataPackets>>();

            Widget paginationWidget;

            #endregion

            #region Main

            public void Initialize()
            {
                paginationWidget = ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(WidgetType.PagerNavigationWidget);
            }

            public void Paginate(List<UIScreenWidget<SceneDataPackets>> source, int itemsPerPage)
            {
                itemList = source;
                pages = AppDataExtensions.GetSubList(itemList, itemsPerPage);

                foreach (var item in itemList)
                    item.Hide();
            }

            public void GoToPage(int pageIndex, bool fromInputField = false, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                foreach (var item in itemList)
                    item.Hide();

                if (pageIndex >= pages.Count)
                    pageIndex = pages.Count - 1;

                currentPage = pages[pageIndex];

                foreach (var item in currentPage)
                    item.Show();

                if (fromInputField)
                    CurrentPageIndex = pageIndex;

                // Pagination Widget

                if (paginationWidget)
                {
                    #region UI Text / Input Value

                    if (!fromInputField)
                    {
                        int pageNumber = CurrentPageIndex + 1;
                        paginationWidget.SetActionInputFieldValueText(InputFieldActionType.InputPageNumberField, pageNumber);
                    }

                    if (fromInputField && pageIndex >= pages.Count)
                    {
                        paginationWidget.SetActionInputFieldPlaceHolderText(InputFieldActionType.InputPageNumberField, pages.Count - 1);
                    }

                    string pageCount = $"of  {pages.Count} - {itemList.Count} Item(s)";
                    paginationWidget.SetUITextDisplayerValue(ScreenTextType.PageCountDisplayer, pageCount);

                    #endregion

                    #region UI Buttons State

                    paginationWidget.SetActionButtonState(InputActionButtonType.PreviousNavigationButton, InputUIState.Enabled);
                    paginationWidget.SetActionButtonState(InputActionButtonType.NextNavigationButton, InputUIState.Enabled);

                    if (CurrentPageIndex == 0)
                        paginationWidget.SetActionButtonState(InputActionButtonType.PreviousNavigationButton, InputUIState.Disabled);

                    if (CurrentPageIndex >= pages.Count - 1)
                        paginationWidget.SetActionButtonState(InputActionButtonType.NextNavigationButton, InputUIState.Disabled);

                    //ScreenUIManager.Instance.Refresh();

                    callbackResults.results = $"Go To Page : {pageIndex} Success. Page Found.";
                    callbackResults.resultsCode = Helpers.SuccessCode;

                    #endregion
                }

                if (paginationWidget == null || currentPage == null)
                {
                    callbackResults.results = $"Go To Page : {pageIndex} Failed. Page Not Found.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public int GetPageCount()
            {
                return pages.Count;
            }

            public int GetItemsCount()
            {
                return itemList.Count;
            }

            public void GetItemPageIndex(string itemName, Action<CallbackData<int>> callback)
            {
                CallbackData<int> callbackResults = new CallbackData<int>();

                bool pageIndexFound = false;

                if (pages.Count > 0)
                {
                    for (int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
                    {
                        var page = pages[pageIndex];

                        if (page.Count > 0)
                        {
                            for (int itemIndex = 0; itemIndex < page.Count; itemIndex++)
                            {
                                if (page[itemIndex].name == itemName)
                                {
                                    pageIndexFound = true;

                                    callbackResults.results = $"Success - Item Page Index Found At : {pageIndex}";
                                    callbackResults.data = pageIndex;
                                    callbackResults.resultsCode = (pageIndexFound)? Helpers.SuccessCode : Helpers.ErrorCode;

                                    break;
                                }
                                else
                                    continue;
                            }
                        }
                    }

                    if (!pageIndexFound)
                    {
                        callbackResults.results = $"Failed : Page Index Not Found For : {itemName}.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = (pageIndexFound)? Helpers.SuccessCode : Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "Failed : No Pages Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = (pageIndexFound)? Helpers.SuccessCode : Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            public bool ItemExistInCurrentPage(UIScreenWidget<SceneDataPackets> itemToCheck)
            {
                bool itemExist = false;

                if (currentPage != null && currentPage.Count > 0)
                {
                    foreach (var item in currentPage)
                    {
                        if (item == itemToCheck)
                        {
                            itemExist = true;
                            break;
                        }
                        else
                            continue;
                    }
                }
                else
                    Debug.LogError("==> Current Page Is Null.");

                return itemExist;
            }

            public bool ItemExistInCurrentPage(string itemToCheck)
            {
                bool itemExist = false;

                if (currentPage != null && currentPage.Count > 0)
                {
                    foreach (var item in currentPage)
                    {
                        if (item.name == itemToCheck)
                        {
                            Debug.LogError("==> Asset Exist In Current Page.");
                            itemExist = true;
                            break;
                        }
                        else
                            continue;
                    }
                }
                else
                    Debug.LogError("==> Current Page Is Null.");

                return itemExist;
            }

            public List<UIScreenWidget<SceneDataPackets>> GetPage(int pageIndex)
            {
                return pages[pageIndex];
            }

            public List<UIScreenWidget<SceneDataPackets>> GetCurrentPage()
            {
                return currentPage;
            }

            public List<UIScreenWidget<SceneDataPackets>> GoToPageIndex(int pageIndex)
            {
                CurrentPageIndex = pageIndex;

                return pages[pageIndex];
            }

            public void NextPage()
            {
                if (CurrentPageIndex < pages.Count - 1)
                    CurrentPageIndex++;
                else
                    CurrentPageIndex = pages.Count - 1;

                GoToPage(CurrentPageIndex);
            }

            public void PreviousPage()
            {
                if (CurrentPageIndex > 0)
                    CurrentPageIndex--;
                else
                    CurrentPageIndex = 0;

                GoToPage(CurrentPageIndex);
            }

            public void GetSlotAvailablePageNumber(int itemsPerPage, Action<CallbackData<int>> callback)
            {
                CallbackData<int> callbackResults = new CallbackData<int>();

                if (pages != null && pages.Count > 0)
                {
                    bool available = false;
                    int pageID = 0;

                    foreach (var page in pages)
                    {
                        if (page.Count != itemsPerPage)
                        {
                            available = true;
                            pageID = pages.IndexOf(page);

                            break;
                        }
                    }

                    if (pageID == 0 && pages.Count > 1)
                        pageID = pages.Count - 1;

                    if (available)
                    {
                        callbackResults.results = "Slot Found.";
                        callbackResults.data = pageID;
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = "Slot Not Found.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "Pages Not Found.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void GetSlotAvailableOnScroller(List<UIScreenWidget<SceneDataPackets>> itemList, Action<Callback> callback)
            {
                Callback callbackResults = new Callback();

                if (itemList.Count % 2 == 0)
                {
                    callbackResults.results = "No Slot Available - Recalculate Content Container";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }
                else
                {
                    callbackResults.results = "Slot Available";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }

                callback?.Invoke(callbackResults);
            }

            #endregion
        }

        [Serializable]
        public struct PageItem<T>
        {
            public string name;
            public T item;
            public int pageID;
        }

        #endregion

        #endregion

        #region Scroller

        [Serializable]
        public class UIScroller<T> where T : DataPackets
        {
            #region Components

            public ScrollRect value;

            [Space(5)]
            public ScrollRect.MovementType movementType;

            [Space(5)]
            public OrientationType orientation;

            [Space(5)]
            public bool resetScrollerPositionOnHide;

            [Space(5)]
            public bool enableScrollBar;

            [Space(5)]
            public UIScrollBar scrollBarComponent = new UIScrollBar();

            [Space(5)]
            public bool fadeUIScrollBar;

            [Space(5)]
            public RectTransform dragViewPort;

            [Space(5)]
            public T dataPackets;

            #endregion

            #region main

            public void Update()
            {
                if (enableScrollBar)
                    scrollBarComponent.Update(value);
            }

            public void ScrollToTop()
            {
                value.verticalNormalizedPosition = 0;
            }

            public void ScrollToBottom()
            {
                value.verticalNormalizedPosition = 1;
            }

            public bool IsScrollBarEnabled()
            {
                return enableScrollBar;
            }

            public UIScrollBar GetUIScrollBarComponent()
            {
                return scrollBarComponent;
            }

            public bool GetFadeUIScrollBar()
            {
                return fadeUIScrollBar;
            }

            public void OnScrollbarFadeIn()
            {
                Debug.LogError("==> Fade In");

                if (fadeUIScrollBar)
                {
                    GetUIScrollBarComponent().SetIsFading();
                    scrollBarComponent.OnScrollbarFadeInUpdate();
                }
                else
                    return;
            }

            public void OnScrollbarFadeOut()
            {
                if (fadeUIScrollBar)
                    scrollBarComponent.OnScrollbarFadeOutUpdate();
                else
                    return;
            }

            public void Initialized(Action<Callback> callback)
            {
                Callback callbackResults = new Callback();

                if (value != null)
                {
                    if (enableScrollBar)
                    {
                        if (scrollBarComponent.value != null)
                        {
                            scrollBarComponent.Initialize(fadeUIScrollBar);

                            callbackResults.results = "Initialized Success : Scroller Value & Scrollbar Value Components Assigned";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = "Initialized Failed : Scrollbar Is Enabled But Scrollbar Value Is Not Assigned";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = "Initialized Success : Scroller Value & Scrollbar Value Components Assigned";
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                }
                else
                {
                    callbackResults.results = "Initialized Failed : Scroller Value & Scrollbar Components Not Assigned";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void Initialize() => OnInitializeScrollBarComponent();

            public RectTransform GetDragViewPort()
            {
                return dragViewPort;
            }

            void OnInitializeScrollBarComponent()
            {
                if (enableScrollBar)
                {
                    switch (scrollBarComponent.GetLayoutDirection())
                    {
                        case UILayoutDirection.Horizontal:

                            Initialized(isInitializedCallback =>
                            {
                                if (Helpers.IsSuccessCode(isInitializedCallback.resultsCode))
                                {
                                    scrollBarComponent.Show();
                                    value.horizontalScrollbar = GetUIScrollBarComponent().value;
                                }
                                else
                                    Debug.LogWarning($"Initialize's Initialized Failed With Results : {isInitializedCallback.results}");
                            });

                            break;

                        case UILayoutDirection.Vertical:

                            Initialized(isInitializedCallback =>
                            {
                                if (Helpers.IsSuccessCode(isInitializedCallback.resultsCode))
                                {
                                    scrollBarComponent.Show();
                                    value.verticalScrollbar = GetUIScrollBarComponent().value;
                                }
                                else
                                    Debug.LogWarning($"Initialize's Initialized Failed With Results : {isInitializedCallback.results}");
                            });

                            break;
                    }

                    if (fadeUIScrollBar)
                        scrollBarComponent.Initialize(fadeUIScrollBar);
                    else
                        Debug.LogWarning("==> fadeUIScrollBar Not Enabled");
                }
                else
                    scrollBarComponent.Hide();
            }

            public void ResetPosition(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                Vector3 scrollerValue = value.content.localPosition;

                scrollerValue.x = (orientation == OrientationType.Horizontal)? scrollerValue.x = 0 : scrollerValue.x;
                scrollerValue.y = (orientation == OrientationType.Vertical) ? scrollerValue.y = 0 : scrollerValue.y;

                value.content.localPosition = scrollerValue;

                bool isReset = (orientation == OrientationType.Horizontal) ? scrollerValue.x == 0 : scrollerValue.y == 0;

                if (value != null && isReset)
                {
                    callbackResults.results = "Scroller Reset.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "Scroller Not Reset Reset.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            #endregion
        }

        public enum UILayoutDirection
        {
            Horizontal,
            Vertical
        }

        [Serializable]
        public class UIScrollBarHandle
        {
            #region Components

            public RectTransform value;

            [Space(5)]
            public int padding;

            int paddingStart = 0,
                  paddingEnd = 0;

            RectOffset rectOffset = new RectOffset();

            #endregion

            #region Main

            public void Update(UILayoutDirection layoutDirection, float scrollDistance)
            {
                if (value && padding > 0)
                {
                    float distanceScrolled = Mathf.Clamp01(scrollDistance);
                    paddingStart = Mathf.RoundToInt(padding * distanceScrolled);
                    paddingEnd = Mathf.RoundToInt(padding * -distanceScrolled);
                    int calculatedPaddingEnd = padding + paddingEnd;

                    if (layoutDirection == UILayoutDirection.Horizontal)
                    {
                        rectOffset.left = paddingStart;
                        rectOffset.right = calculatedPaddingEnd;
                    }

                    if (layoutDirection == UILayoutDirection.Vertical)
                    {
                        rectOffset.top = paddingStart;
                        rectOffset.bottom = calculatedPaddingEnd;
                    }

                    UpdateHandle(layoutDirection, rectOffset);
                }
                else
                    Debug.LogWarning("--> UIScrollBarHandle Update Failed : Handle Value Is Missing / Null Or Padding Is Set To 0.");
            }

            void UpdateHandle(UILayoutDirection layoutDirection, RectOffset rectOffset)
            {
                if (layoutDirection == UILayoutDirection.Horizontal)
                {
                    value.offsetMin = new Vector2(Mathf.Abs(rectOffset.left), value.offsetMin.y);
                    value.offsetMax = new Vector2(-Mathf.Abs(rectOffset.right), value.offsetMax.y);
                }

                if (layoutDirection == UILayoutDirection.Vertical)
                {
                    value.offsetMin = new Vector2(value.offsetMin.x, Mathf.Abs(rectOffset.bottom));
                    value.offsetMax = new Vector2(value.offsetMax.x, Mathf.Abs(rectOffset.top));
                }
            }

            #endregion
        }

        [Serializable]
        public class UIScrollBar
        {
            #region Components

            [Space(5)]
            public Scrollbar value;

            [Space(5)]
            public UILayoutDirection layoutDirection;

            [Space(5)]
            public bool updateHandle;

            [Space(5)]
            public UIScrollBarHandle scrollBarHandle;

            [Space(5)]
            public UIFaderComponent scrollBarUIFaderComponent;

            bool hideOnOutOfFocus;
            bool isFading = false;

            float visibleStateValue = 1.0f;
            float hiddenStateValue = 0.0f;

            UIWidgetVisibilityState scrollBarVisibilityState;

            #endregion

            #region Main

            public void Update(ScrollRect scroller)
            {
                if (updateHandle)
                    scrollBarHandle.Update(layoutDirection, (layoutDirection == UILayoutDirection.Horizontal) ? scroller.horizontalNormalizedPosition : scroller.verticalNormalizedPosition);
            }

            public void Initialize(bool hideOnOutOfFocus)
            {
                if (value != null)
                {
                    if (hideOnOutOfFocus)
                    {
                        if (scrollBarUIFaderComponent.value == null)
                        {
                            if (value.gameObject.GetComponent<CanvasGroup>() == null)
                                value.gameObject.AddComponent<CanvasGroup>();

                            if (value.gameObject.GetComponent<CanvasGroup>() != null)
                                scrollBarUIFaderComponent.value = value.gameObject.GetComponent<CanvasGroup>();
                        }
                    }

                    this.hideOnOutOfFocus = hideOnOutOfFocus;
                }
                else
                    Debug.LogWarning("--> Initialize Failed : Scrollbar Value Missing.");
            }

            public Scrollbar GetScrollbar()
            {
                if (value == null)
                    Debug.LogWarning("--> Initialize Failed : Scrollbar Value Missing.");

                return value ?? null;
            }

            public void GetScrollbarFaderAlphaValue(Action<CallbackData<float>> callback)
            {
                CallbackData<float> callbackResults = new CallbackData<float>();

                if (hideOnOutOfFocus)
                {
                    if (scrollBarUIFaderComponent != null)
                    {
                        callbackResults.results = "Success";
                        callbackResults.data = scrollBarUIFaderComponent.GetFaderAlphaValue();
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = "Failed : UIScrollbar scrollbarFaderCanvasgroupComponent Is Missing / Null.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "Failed : UIScrollbar Variable Not Set To 'fadeOnOutOfFocus'";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void SetIsFading() => this.isFading = true;

            public void OnScrollbarFadeInUpdate()
            {
                if (hideOnOutOfFocus)
                {
                    if (isFading)
                    {
                        float fadeValue = scrollBarUIFaderComponent.GetFaderAlphaValue();
                        fadeValue = Mathf.Lerp(fadeValue, visibleStateValue, SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.ScrollbarFadeInSpeed).value * Time.smoothDeltaTime);

                        scrollBarUIFaderComponent.SetFaderAlphaValue(fadeValue);

                        float fadeDistance = scrollBarUIFaderComponent.GetFaderAlphaValue() - visibleStateValue;

                        if (fadeDistance <= visibleStateValue)
                        {
                            scrollBarUIFaderComponent.SetFaderAlphaValue(visibleStateValue);
                            SetVisibilityState(UIWidgetVisibilityState.Visible);
                            isFading = false;
                        }
                    }
                }
                else
                    Debug.LogWarning("--> OnScrollbarFadeIn Failed : UIScrollbar scrollbarFaderCanvasgroupComponent Is Missing / Null.");
            }

            public void OnScrollbarFadeOutUpdate()
            {
                if (hideOnOutOfFocus)
                {
                    if (isFading)
                    {
                        float fadeValue = scrollBarUIFaderComponent.GetFaderAlphaValue();
                        fadeValue = Mathf.Lerp(fadeValue, hiddenStateValue, SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.ScrollbarFadeInSpeed).value * Time.smoothDeltaTime);

                        scrollBarUIFaderComponent.SetFaderAlphaValue(fadeValue);

                        float fadeDistance = scrollBarUIFaderComponent.GetFaderAlphaValue() - hiddenStateValue;

                        if (fadeDistance <= hiddenStateValue)
                        {
                            scrollBarUIFaderComponent.SetFaderAlphaValue(hiddenStateValue);
                            SetVisibilityState(UIWidgetVisibilityState.Hidden);
                            isFading = false;
                        }
                    }
                }
                else
                    Debug.LogWarning("--> OnScrollbarFadeIn Failed : UIScrollbar scrollbarFaderCanvasgroupComponent Is Missing / Null.");
            }

            public void SetVisibilityState(UIWidgetVisibilityState visibilityState)
            {
                switch (visibilityState)
                {
                    case UIWidgetVisibilityState.Visible:

                        scrollBarUIFaderComponent.SetFaderAlphaValue(visibleStateValue);
                        GetScrollbar().interactable = true;

                        break;

                    case UIWidgetVisibilityState.Hidden:

                        scrollBarUIFaderComponent.SetFaderAlphaValue(hiddenStateValue);
                        GetScrollbar().interactable = false;

                        break;
                }

                this.scrollBarVisibilityState = visibilityState;
            }

            public UIWidgetVisibilityState GetScrollBarVisibilityState()
            {
                return scrollBarVisibilityState;
            }

            public UIFaderComponent GetScrollBarFader()
            {
                return scrollBarUIFaderComponent;
            }

            public UILayoutDirection GetLayoutDirection()
            {
                return layoutDirection;
            }

            public void Show()
            {
                if (value != null)
                    value.gameObject.SetActive(true);
                else
                    Debug.LogWarning("--> Show ScrollBar Failed : ScrollBar Value Is Missing / Null.");
            }

            public void Hide()
            {
                if (value != null)
                    value.gameObject.SetActive(false);
                else
                    Debug.LogWarning("--> Hide ScrollBar Failed : ScrollBar Value Is Missing / Null.");
            }

            #endregion
        }

        [Serializable]
        public class UIFaderComponent
        {
            #region Component

            public CanvasGroup value;

            #endregion

            #region Main

            public void SetFaderAlphaValue(float value) => this.value.alpha = value;

            public float GetFaderAlphaValue()
            {
                return value.alpha;
            }

            #endregion
        }

        #endregion

        #region Selection

        public struct UIStateData
        {
            public FocusedSelectionType selectionType;
            public InputUIState state;
            public Color color;
        }

        [Serializable]
        public struct UISelectionFrame
        {
            #region Components

            public UIImageDisplayer selectionFrame;

            [Space(5)]
            public GameObject tint;

            [Space(5)]
            public List<SelectionStateColorProperties> selectionStates;

            public UIStateData uiStateInfoData;

            #endregion

            #region Main

            public (bool hasSelectionFrame, bool hasSelectionState, bool hasTint) IsInitialized()
            {
                bool hasSelectionFrame = selectionFrame.value != null;
                bool hasSelectionState = selectionStates.Count > 0;
                bool hasTint = tint != null;

                return (hasSelectionFrame, hasSelectionState, hasTint);
            }

            public void Show(InputUIState state, bool showTint = false, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (selectionFrame.value != null)
                {
                    if (selectionFrame.imageDisplayerType == UIImageDisplayerType.SelectionFrame)
                    {
                        uiStateInfoData = new UIStateData
                        {
                            state = state,
                            color = selectionStates.Find(selectionState => selectionState.selectionColorState == state).selectionColor
                        };

                        if (uiStateInfoData.color != null)
                            selectionFrame.value.color = uiStateInfoData.color;

                        selectionFrame.value.gameObject.SetActive(true);

                        if (IsInitialized().hasTint)
                            tint.SetActive(showTint);

                        if (callback != null)
                        {
                            if (GetCurrentSelectionState().showSelection)
                            {
                                callbackResults.results = $"Show Selection Frame For [State] : {state} --> Show Tint Set To : {showTint}";
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = "Couldn't Showing Selection Frame For Some Reason - Asset possibly Disabled";
                                callbackResults.resultsCode = Helpers.WarningCode;
                            }
                        }
                        else
                        {
                            if (!GetCurrentSelectionState().showSelection)
                            {
                                callbackResults.results = "Selection Frame Not Showing --> Possibility : UIImageDisplayerType Not Set To Selection Frame";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                    }
                    else
                    {
                        if (callback != null)
                        {
                            callbackResults.results = "UIImageDisplayerType Not Set To Selection Frame";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                        else
                        {
                            callbackResults.results = $"Couldn't Show Selection Frame For [State] : {state} --> Possibility : UIImageDisplayerType Not Set To Selection Frame.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callbackResults.results = "Selection Frame's Value Is Missing / Null.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                    else
                    {
                        callbackResults.results = $"Couldn't Show Selection Frame For [State] : {state} --> Possibility : Selection Frame's Value Is Missing / Null.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }

                callback?.Invoke(callbackResults);
            }

            public (UIStateData uiStateData, bool showSelection, bool showTint) GetCurrentSelectionState()
            {
                bool showSelectionInfo = selectionFrame.value.isActiveAndEnabled && selectionFrame.value.gameObject.activeInHierarchy && selectionFrame.value.gameObject.activeSelf;
                bool showTintInfo = tint.activeInHierarchy && tint.activeSelf;
                uiStateInfoData.selectionType = SelectableManager.Instance.GetFocusedSelectionTypeFromState(uiStateInfoData.state);

                return (uiStateInfoData, showSelectionInfo, showTintInfo);
            }

            public async void Hide(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (IsInitialized().hasSelectionFrame)
                {
                    if (selectionFrame.imageDisplayerType == UIImageDisplayerType.SelectionFrame)
                    {
                        selectionFrame.value.gameObject.SetActive(false);
                        tint.SetActive(false);

                        bool hidden = selectionFrame.value.gameObject.activeSelf == false && selectionFrame.value.gameObject.activeInHierarchy == false;
                        await Helpers.GetWaitUntilAsync(hidden);

                        if (hidden)
                        {
                            callbackResults.results = $"Selection Frame : {selectionFrame.name} Hidden.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"Couldn't Hide Selection Frame : {selectionFrame.name} For Some Reason.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"Couldn't Hide : {selectionFrame.name}. UI Image Displayer Type Is Not Set To Selection Frame.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"Couldn't Hide : {selectionFrame.name}. - Selection Frame Missing / Null / Not Yet Initialized.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public bool IsActive()
            {
                return selectionFrame.value.gameObject.activeSelf;
            }

            #endregion
        }

        [Serializable]
        public struct SelectionStateColorProperties
        {
            #region Components

            public string name;

            [Space(5)]
            public Color selectionColor;

            [Space(5)]
            public InputUIState selectionColorState;

            #endregion
        }

        [Serializable]
        public struct WidgetPlaceHolderInfo
        {
            #region Components

            public Vector2 dimensions;

            public Vector2 anchoredPosition;
            public Vector3 worldPosition, localPosition;

            public Transform parent;

            public bool isActive;

            #endregion
        }

        [Serializable]
        public class WidgetPlaceHolder
        {
            #region Components

            public RectTransform value;

            [Space(5)]
            public Transform container;

            public bool isActive;

            WidgetPlaceHolderInfo info = new WidgetPlaceHolderInfo();

            #endregion

            #region Main

            public RectTransform GetPlaceHolder(UIScreenWidget<SceneDataPackets> screenWidget)
            {
                value.name = screenWidget.name;
                value.anchoredPosition = screenWidget.GetWidgetRect().anchoredPosition;
                value.sizeDelta = screenWidget.GetWidgetRect().sizeDelta;
                value.SetSiblingIndex(screenWidget.GetWidgetRect().GetSiblingIndex());

                return value;
            }

            public UIScreenWidget<SceneDataPackets> GetWidget()
            {
                return value.GetComponent<UIScreenWidget<SceneDataPackets>>();
            }

            public RectTransform GetContainer()
            {
                return value;
            }

            void SetIndex(int index)
            {
                if (value != null)
                    value.transform.SetSiblingIndex(index);
                else
                    Debug.LogWarning("--> Failed : Value Is Missing / Null.");
            }


            void SetPlaceHolderWidgetDimensions(Vector2 dimensions)
            {
                if (value != null)
                    value.sizeDelta = dimensions;
                else
                    Debug.LogWarning("--> SetPlaceHolderWidgetDimensions Failed : Value Is Missing / Null.");
            }

            public void ShowPlaceHolder(Transform container, Vector2 dimensions, int index, bool keepWorldPos = false)
            {
                //Debug.LogError($"===> On Show Placeholder - Setting Parent To : {container.name} - Index : {index}");

                if (container != null)
                {
                    if (value != null)
                    {
                        isActive = true;
                        //Debug.LogError($"==> Setting Parent To : {container.name} - Index : {index}");        
                        value.transform.SetParent(container, keepWorldPos);
                        SetPlaceHolderWidgetDimensions(dimensions);
                        SetIndex(index);
                        value.gameObject.SetActive(isActive);
                    }
                    else
                        Debug.LogWarning("--> SetContainer Failed : Value Is Missing / Null.");
                }
                else
                    Debug.LogWarning("--> SetContainer Failed : Container Is Missing / Null.");
            }

            public int GetWidgetIndex()
            {
                return value.GetSiblingIndex();
            }

            public void ResetPlaceHolder()
            {
                if (container != null)
                {
                    if (value != null)
                    {


                        isActive = false;
                        value.transform.SetParent(container, isActive);
                        value.gameObject.SetActive(isActive);
                    }
                    else
                        Debug.LogWarning("--> SetContainer Failed : Value Is Missing / Null.");
                }
                else
                    Debug.LogWarning("--> SetContainer Failed : Container Is Missing / Null.");
            }

            public void ResetPlaceHolder(ref RectTransform widgetRef)
            {
                if (container != null)
                {
                    if (value != null)
                    {
                        isActive = false;
                        widgetRef.SetSiblingIndex(value.GetSiblingIndex());
                        value.transform.SetParent(container, isActive);
                        value.gameObject.SetActive(isActive);
                    }
                    else
                        Debug.LogWarning("--> SetContainer Failed : Value Is Missing / Null.");
                }
                else
                    Debug.LogWarning("--> SetContainer Failed : Container Is Missing / Null.");
            }

            public bool IsActive()
            {
                return isActive;
            }

            public WidgetPlaceHolderInfo GetInfo()
            {
                if (value != null)
                {
                    info.dimensions = value.sizeDelta;

                    info.anchoredPosition = value.anchoredPosition;
                    info.worldPosition = value.position;
                    info.localPosition = value.localPosition;

                    info.isActive = IsActive();

                    if (container != null)
                        info.parent = container;
                    else
                        Debug.LogWarning("--> SetContainer Failed : Container Is Missing / Null.");
                }
                else
                    Debug.LogWarning("--> SetPlaceHolderWidgetDimensions Failed : Value Is Missing / Null.");

                return info;
            }

            #endregion
        }

        [Serializable]
        public class FolderStructureSelectionSystem
        {
            #region Components

            [Space(5)]
            public List<FocusedSelectionStateInfo> selectionStates = new List<FocusedSelectionStateInfo>();

            [Space(5)]
            public FocusedSelectionData focusedSelectionData = new FocusedSelectionData();

            //[HideInInspector]
            [Space(5)]
            public FocusedSelectionData cachedSelectionData = new FocusedSelectionData();

            //[Space(5)]
            //public List<string> cachedSelectables = new List<string>();

            //[Space(5)]
            //public List<UIScreenWidget<SceneDataPackets>> currentSelections = new List<UIScreenWidget<SceneDataPackets>>();

            public Action<SceneDataPackets> OnSelection { get; set; }
            public Action OnDeselection { get; set; }

            #endregion

            #region Event Delegates Callbacks

            #region Delegates

            private delegate void SelectionEvent();

            #endregion

            #region Callbacks

            #endregion

            #endregion

            #region Main

            public void Select(UIScreenWidget<SceneDataPackets> selectable, SceneDataPackets dataPackets, bool isInitialSelection = false)
            {
                if(isInitialSelection)
                {
                    Select(selectable.name, FocusedSelectionType.SelectedItem, selectionCallback => 
                    {
                        if (selectionCallback.Success())
                        {
                            OnSelection?.Invoke(dataPackets);

                            ActionEvents.OnWidgetSelectionEvent();
                        }
                    });
                }
                else
                {
                    if (GetCurrentSelectionType() != FocusedSelectionType.SelectedItem)
                        OnClearFocusedSelectionsInfo();

                    HasFocusedSelectionInfo(selectable.name, hasSelectionCallback =>
                    {
                        if (!Helpers.IsSuccessCode(hasSelectionCallback.resultsCode))
                        {
                            OnAddSelection(selectable.name, FocusedSelectionType.SelectedItem, addedSelectionCallback =>
                            {
                                if (Helpers.IsSuccessCode(addedSelectionCallback.resultsCode))
                                    OnSelection?.Invoke(dataPackets);
                                else
                                    Debug.LogError(addedSelectionCallback.results);
                            });
                        }
                        else
                            Debug.LogError(hasSelectionCallback.results);
                    });
                }
            }

            public void Select(string name, FocusedSelectionType selectionType)
            {
                Select(name, selectionType, selectionCallback =>
                {

                });
            }

            public void Deselect(UIScreenWidget<SceneDataPackets> deselectedWidget, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                OnRemoveSelection(deselectedWidget.name, FocusedSelectionType.SelectedItem, deselectionCallback => 
                {
                    Debug.LogError($"================> Removed : {deselectionCallback.resultsCode} So Deselect Widget - Results : {deselectionCallback.results}...............");

                    if (Helpers.IsSuccessCode(deselectionCallback.resultsCode))
                        OnDeselection?.Invoke();
                    else
                        callbackResults = deselectionCallback;
                });

                callback?.Invoke(callbackResults);
            }

            public void Deselect(string selectionName, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                OnRemoveSelection(selectionName, FocusedSelectionType.SelectedItem, deselectionCallback =>
                {
                    Debug.LogError($"================> Removed Is : {deselectionCallback.resultsCode} To Deselect Widget - Results : {deselectionCallback.results}...............");

                    if (Helpers.IsSuccessCode(deselectionCallback.resultsCode))
                        OnDeselection?.Invoke();
                    else
                        callbackResults = deselectionCallback;
                });

                callback?.Invoke(callbackResults);
            }

            public void DeselectAll()
            {
                DeselectWidgets(GetFocusedSelectionInfoList(), deselectionCallback => 
                {
                    if (Helpers.IsSuccessCode(deselectionCallback.resultsCode))
                    {
                        OnDeselection?.Invoke();
                    }
                    else
                        Debug.LogError(deselectionCallback.results);
                });
            }

            public void AddSelectables(List<UIScreenWidget<SceneDataPackets>> selectables)
            {

                Debug.LogError($"=====> Added {selectables.Count} Items On Refresh");

                if (selectables != null && selectables.Count > 0)
                {
                    OnClearFocusedSelectionsInfo();

                    //if (cachedSelectables.Count > 0)
                    //    for (int selectable = 0; selectable < selectables.Count; selectable++)
                    //        if (cachedSelectables.Contains(selectables[selectable].name))
                    //            if (!currentSelections.Contains(selectables[selectable]))
                    //                currentSelections.Add(selectables[selectable]);

                    OnSelected();
                }
                else
                    Debug.LogWarning("--> AddSelectables Failed : Selectables List Is Null.");
            }

            public List<UIScreenWidget<SceneDataPackets>> GetCurrentSelections()
            {
                List<UIScreenWidget<SceneDataPackets>> widgetsList = new List<UIScreenWidget<SceneDataPackets>>();

                OnGetSelectedWidgets(selectionCallback => 
                {
                    if (Helpers.IsSuccessCode(selectionCallback.resultsCode))
                        widgetsList = selectionCallback.data;
                    else
                        Debug.LogError(selectionCallback.results);
                });

                return widgetsList;
            }

            void OnSelected()
            {
                var widgetsContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                if (widgetsContainer != null)
                {
                    widgetsContainer.OnWidgetSelectionState(GetFocusedSelectionInfoList(), FocusedSelectionType.SelectedItem, selectionCallback => 
                    {
                        if(!Helpers.IsSuccessCode(selectionCallback.resultsCode))
                            Debug.LogError(selectionCallback.results);
                    });
                }
                else
                    Debug.LogError("On Selected Failed. Check Here Please.");

                //if (currentSelections.Count > 0)
                //    foreach (var selectable in currentSelections)
                //        selectable.OnSelect();
            }

            #region Selection Data

            public void CacheSelection(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                cachedSelectionData = focusedSelectionData;

                if(cachedSelectionData.selections.Count > 0)
                {
                    callbackResults.results = $"{cachedSelectionData.selections.Count} Selections Cached.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Couldn't Cache : {focusedSelectionData.selections.Count} Selections Of Type : {focusedSelectionData.selectionType}";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void ClearSelectionCache(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                cachedSelectionData.Clear();

                if(cachedSelectionData.selections.Count == 0)
                {
                    callbackResults.results = $"Cache Cleared";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Cache Couldn't Clear - : {cachedSelectionData.selections.Count} Selections Found";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void GetCachedSelectionInfo(Action<CallbackDatas<FocusedSelectionInfo<SceneDataPackets>>> callback)
            {
                CallbackDatas<FocusedSelectionInfo<SceneDataPackets>> callbackResults = new CallbackDatas<FocusedSelectionInfo<SceneDataPackets>>();

                if(HasCachedSelectionInfo())
                {
                    callbackResults.results = "Cached Selection Info Data Found.";
                    callbackResults.data = cachedSelectionData.selections;
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "There Are No Cached Selection Info Data Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            public void GetCachedSelectionInfoNameList(Action<CallbackDatas<string>> callback)
            {
                CallbackDatas<string> callbackResults = new CallbackDatas<string>();

                if (HasCachedSelectionInfo())
                {
                    List<string> cachedSelectionInfoNameList = new List<string>();

                    foreach (var selection in cachedSelectionData.selections)
                        if (!cachedSelectionInfoNameList.Contains(selection.name))
                            cachedSelectionInfoNameList.Add(selection.name);

                    if (cachedSelectionInfoNameList.Count > 0)
                    {
                        callbackResults.results = "Cached Selection Info Data Found.";
                        callbackResults.data = cachedSelectionInfoNameList;
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = "There Are No Valid Cached Selection Info Data Loaded From Cache List - Something Is Really Wrong.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "There Are No Cached Selection Info Data Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            public bool HasCachedSelectionInfo()
            {
                return cachedSelectionData.isActiveSelection && cachedSelectionData.selections.Count > 0;
            }

            public void SelectWidgets(List<FocusedSelectionInfo<SceneDataPackets>> selections, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if(selections != null && selections.Count > 0)
                {
                    var widgetsContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                    if(widgetsContainer != null)
                        widgetsContainer.OnWidgetSelectionState(selections, selectionCallback => { callbackResults = selectionCallback; });
                    else
                    {
                        callbackResults.results = "Widgets Container Missing / Not Found";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "No Selections Found";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void DeselectWidgets(List<FocusedSelectionInfo<SceneDataPackets>> selections, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (selections != null && selections.Count > 0)
                {
                    var widgetsContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                    if (widgetsContainer != null)
                    {
                        widgetsContainer.OnWidgetSelectionState(selections, FocusedSelectionType.Default, selectionCallback => 
                        {
                            if (Helpers.IsSuccessCode(selectionCallback.resultsCode))
                                OnClearFocusedSelectionsInfo();

                            callbackResults = selectionCallback; 
                        });
                    }
                    else
                    {
                        callbackResults.results = "Widgets Container Missing / Not Found";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "No Selections Found";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void OnGetSelectedWidgets(Action<CallbackDatas<UIScreenWidget<SceneDataPackets>>> callback)
            {
                CallbackDatas<UIScreenWidget<SceneDataPackets>> callbackResults = new CallbackDatas<UIScreenWidget<SceneDataPackets>>();

                if (HasActiveSelections())
                {
                    List<UIScreenWidget<SceneDataPackets>> selectionList = new List<UIScreenWidget<SceneDataPackets>>();

                    foreach (var widget in focusedSelectionData?.selections)
                    {
                        UIScreenWidget<SceneDataPackets> screenWidget = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetWidgetNamed(widget.name);

                        if (screenWidget != null)
                        {
                            if (!selectionList.Contains(screenWidget))
                                selectionList.Add(screenWidget);
                            else
                            {
                                callbackResults.results = $"Screen Widget Named : {widget.name} Already Added To List.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = Helpers.ErrorCode;

                                break;
                            }
                        }
                        else
                        {
                            callbackResults.results = $"Screen Widget Named : {widget.name} Not Found.";
                            callbackResults.data = default;
                            callbackResults.resultsCode = Helpers.ErrorCode;

                            break;
                        }
                    }

                    if (selectionList.Count > 0)
                    {
                        callbackResults.results = $"{selectionList.Count} Selections Found.";
                        callbackResults.data = selectionList;
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = "Selection Widgets Not Found.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "There Are No Selections Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            public void Select(string selectionName, FocusedSelectionType selectionType, Action<CallbackData<FocusedSelectionInfo<SceneDataPackets>>> callback = null)
            {
                CallbackData<FocusedSelectionInfo<SceneDataPackets>> callbackResults = new CallbackData<FocusedSelectionInfo<SceneDataPackets>>();

                FocusedSelectionInfo<SceneDataPackets> selectionInfo = new FocusedSelectionInfo<SceneDataPackets>
                {
                    name = selectionName,
                    selectionInfoType = selectionType
                };

                if (HasActiveSelections())
                {
                    OnClearFocusedSelectionsInfo(selectionInfoCleared =>
                    {
                        if (Helpers.IsSuccessCode(selectionInfoCleared.resultsCode))
                        {
                            OnSetFocusedWidgetSelectionInfo(selectionInfo, true, onSelectionCallback =>
                            {
                                if (Helpers.IsSuccessCode(onSelectionCallback.resultsCode))
                                {
                                    callbackResults.results = $"Set Highlighted Folder To Widget Named : {selectionName} Success.";
                                    callbackResults.data = selectionInfo;
                                    callbackResults.resultsCode = Helpers.SuccessCode;

                                    ActionEvents.OnWidgetSelectionEvent();
                                }
                                else
                                    Debug.LogError(onSelectionCallback.results);
                            });
                        }
                        else
                        {
                            callbackResults.results = selectionInfoCleared.results;
                            callbackResults.resultsCode = Helpers.ErrorCode;
                            callbackResults.data = default;
                        }
                    });
                }
                else
                {
                    OnSetFocusedWidgetSelectionInfo(selectionInfo, true, onSelectionCallback =>
                    {
                        if (Helpers.IsSuccessCode(onSelectionCallback.resultsCode))
                        {
                            callbackResults.results = $"Set Highlighted Folder To Widget Named : {selectionName} Success.";
                            callbackResults.data = selectionInfo;
                            callbackResults.resultsCode = Helpers.SuccessCode;

                            ActionEvents.OnWidgetSelectionEvent(selectionInfo);
                        }
                        else
                            Debug.LogError(onSelectionCallback.results);
                    });
                }

                callback?.Invoke(callbackResults);
            }

            public void OnAddSelection(string selectionName, FocusedSelectionType selectionType, Action<CallbackData<FocusedSelectionInfo<SceneDataPackets>>> callback = null)
            {
                CallbackData<FocusedSelectionInfo<SceneDataPackets>> callbackResults = new CallbackData<FocusedSelectionInfo<SceneDataPackets>>();

                if (HasActiveSelections())
                {
                    HasFocusedSelectionInfo(selectionName, selectionCheckCallback =>
                    {
                        if (!Helpers.IsSuccessCode(selectionCheckCallback.resultsCode))
                        {
                            FocusedSelectionInfo<SceneDataPackets> selectionInfo = new FocusedSelectionInfo<SceneDataPackets>
                            {
                                name = selectionName,
                                selectionInfoType = selectionType
                            };

                            OnAddFocusedWidgetSelectionInfo(selectionInfo, true, selectionAddedCallback =>
                            {
                                if (selectionAddedCallback.Success())
                                {
                                    SelectWidgets(selectionAddedCallback.data, selectionCallback =>
                                    {
                                        if (selectionCallback.Success())
                                        {
                                            callbackResults.results = selectionCallback.results;
                                            callbackResults.resultsCode = selectionCallback.resultsCode;

                                            ActionEvents.OnWidgetSelectionAdded();
                                        }
                                        else
                                            Debug.LogError(selectionCallback.results);
                                    });
                                }
                            });
                        }
                        else
                        {
                            callbackResults.results = selectionCheckCallback.results;
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    });
                }
                else
                {
                    Select(selectionName, selectionType, selectionCallback =>
                    {
                        callbackResults = selectionCallback;
                    });
                }

                callback?.Invoke(callbackResults);
            }

            public void OnRemoveSelection(string selectionName, FocusedSelectionType selectionType = FocusedSelectionType.SelectedItem, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (HasActiveSelections())
                {
                    HasFocusedSelectionInfo(selectionName, selectionCallback =>
                    {
                        if (selectionCallback.Success())
                        {
                            GetSelectionInfo(selectionName, selectionFoundCallback => 
                            {
                                if (selectionFoundCallback.Success())
                                {
                                    OnRemoveFocusedWidgetSelectionInfo(selectionFoundCallback.data, selectionRemovedCallback => 
                                    {
                                        if (selectionRemovedCallback.Success())
                                        {
                                            var widgetsContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                                            if(widgetsContainer != null)
                                            {
                                                var selectionInfo = selectionFoundCallback.data;
                                                selectionInfo.selectionInfoType = FocusedSelectionType.Default;

                                                widgetsContainer.OnWidgetSelectionState(selectionInfo, deselectionCallback => 
                                                {
                                                
                                                
                                                });
                                            }

                                            ActionEvents.OnWidgetSelectionRemoved();
                                        }

                                        callbackResults = selectionRemovedCallback;
                                    });
                                }
                                else
                                {
                                    callbackResults.results = selectionFoundCallback.results;
                                    callbackResults.resultsCode = Helpers.ErrorCode;
                                }
                            });
                        }
                        else
                        {
                            callbackResults.results = selectionCallback.results;
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    });
                }
                else
                {
                    callbackResults.results = "There Are Currently No Selectables To Deslect.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void GetSelectionInfo(string selectionName, Action<CallbackData<FocusedSelectionInfo<SceneDataPackets>>> callback)
            {
                CallbackData<FocusedSelectionInfo<SceneDataPackets>> callbackResults = new CallbackData<FocusedSelectionInfo<SceneDataPackets>>();

                FocusedSelectionInfo<SceneDataPackets> selectionInfo = focusedSelectionData.selections.Find(selection => selection.name == selectionName);

                if(selectionInfo != null)
                {
                    callbackResults.results = $"Selection Info For : {selectionName} Found";
                    callbackResults.data = selectionInfo;
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Selection Info For : {selectionName} Not Found";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            public List<FocusedSelectionInfo<SceneDataPackets>> GetFocusedSelectionInfoList()
            {
                return focusedSelectionData.selections;
            }

            public void Select(List<string> selectionNames, FocusedSelectionType selectionType, Action<CallbackData<FocusedSelectionInfo<SceneDataPackets>>> callback = null)
            {
                CallbackData<FocusedSelectionInfo<SceneDataPackets>> callbackResults = new CallbackData<FocusedSelectionInfo<SceneDataPackets>>();

                List<FocusedSelectionInfo<SceneDataPackets>> selectionInfoList = new List<FocusedSelectionInfo<SceneDataPackets>>();

                if (selectionNames.Count > 0)
                {
                    foreach (var name in selectionNames)
                    {
                        FocusedSelectionInfo<SceneDataPackets> selectionInfo = new FocusedSelectionInfo<SceneDataPackets>
                        {
                            name = name,
                            selectionInfoType = selectionType
                        };

                        selectionInfoList.Add(selectionInfo);
                    }

                    if (selectionInfoList.Count > 0)
                    {
                        OnSetFocusedWidgetSelectionInfo(selectionInfoList, selectionType, true, selectionCallback =>
                        {
                            callbackResults.results = selectionCallback.results;
                            callbackResults.resultsCode = selectionCallback.resultsCode;
                        });
                    }
                    else
                    {
                        callbackResults.results = "There Are No Selection Info Created - Data Missing.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "There Are No Selection Widgets Found";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void HasFocusedSelectionInfo(string selectionName, Action<Callback> callback)
            {
                Callback callbackResults = new Callback();

                bool hasSelection = false;

                if (HasActiveSelections())
                {
                    foreach (var selection in focusedSelectionData?.selections)
                    {
                        if (selection.name.Equals(selectionName))
                        {
                            hasSelection = true;
                            break;
                        }
                    }

                    callbackResults.results = (hasSelection) ? $"Selection : {selectionName} Exists." : $"Selection : {selectionName} Doesn't Exists.";
                    callbackResults.resultsCode = (hasSelection)? Helpers.SuccessCode : Helpers.ErrorCode;
                }
                else
                {
                    callbackResults.results = "There Are No Active Selections Found.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public bool HasActiveSelections()
            {
                bool hasSelection = false;

                if (focusedSelectionData != null && focusedSelectionData?.selections != null && focusedSelectionData?.selections?.Count >= 1 && focusedSelectionData.isActiveSelection)
                    hasSelection = true;

                return hasSelection;
            }

            public FocusedSelectionType GetCurrentSelectionType()
            {
                FocusedSelectionType selectionType = FocusedSelectionType.Default;

                if(HasActiveSelections())
                    selectionType = focusedSelectionData.selectionType;

                return selectionType;
            }

            public int GetFocusedSelectionDataCount()
            {
                return (focusedSelectionData.selections == null) ? 0 : focusedSelectionData.selections.Count;
            }

            public void GetFocusedSelectionData(Action<CallbackData<FocusedSelectionData>> callback)
            {
                CallbackData<FocusedSelectionData> callbackResults = new CallbackData<FocusedSelectionData>();

                if (HasActiveSelections())
                {
                    callbackResults.results = $"Found {GetFocusedSelectionDataCount()} Focused Asset(s)";
                    callbackResults.data = focusedSelectionData;
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "There Is No Focused Widget Info Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void OnClearFocusedSelectionsInfo(Action<Callback> callback = null)
            {
                try
                {
                    Callback callbackResults = new Callback();

                    if (HasActiveSelections())
                    {
                        var widgetsContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                        if (widgetsContainer.GetAssetsLoaded())
                        {
                            bool cleared = false;

                            if (widgetsContainer != null)
                            {
                                foreach (var selection in focusedSelectionData.selections)
                                {
                                    widgetsContainer.GetWidgetNamed(selection.name, widgetFoundCallback => 
                                    {
                                        if(widgetFoundCallback.Success())
                                        {
                                            cleared = true;
                                            widgetFoundCallback.data.OnReset(cleared);
                                        }
                                        else
                                        {
                                            callbackResults.results = $"Widget Named : {selection.name} Not Found";
                                            callbackResults.resultsCode = Helpers.ErrorCode;

                                            cleared = false;
                                        }
                                    });
                                }
                            }

                            if (cleared)
                            {
                                focusedSelectionData.Clear();

                                if (!HasActiveSelections())
                                {
                                    callbackResults.results = "Focused Selection Cleared Successfully.";
                                    callbackResults.resultsCode = Helpers.SuccessCode;
                                }
                                else
                                {
                                    callbackResults.results = "Focused Selection Failed To Clear For Some Reason.";
                                    callbackResults.resultsCode = Helpers.ErrorCode;
                                }
                            }
                            else
                            {
                                callbackResults.results = $"Couldn't Clear Content.";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                    }
                    else
                    {
                        focusedSelectionData.Clear();

                        if (!HasActiveSelections())
                        {
                            callbackResults.results = "Focused Selection Cleared Successfully.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = "Focused Selection Failed To Clear For Some Reason.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }

                    callback?.Invoke(callbackResults);
                }
                catch(Exception exception)
                {
                    Debug.LogError($"==> [OnClearFocusedSelectionsInfo] Exception Found : {exception.Message}");
                    throw exception;
                }
            }

            public void OnClearFocusedSelectionsInfo(bool resetWidgets, Action<Callback> callback = null)
            {
                try
                {
                    Callback callbackResults = new Callback();

                    if(resetWidgets)
                    {
                        if (focusedSelectionData.selections.Count > 0)
                        {
                            var widgetsContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                            bool cleared = false;

                            if (widgetsContainer != null)
                            {
                                foreach (var selection in focusedSelectionData.selections)
                                {
                                    var widget = widgetsContainer.GetWidgetNamed(selection.name);

                                    if (widget != null)
                                    {
                                        widget.OnDeselect();
                                        cleared = true;
                                    }
                                    else
                                    {
                                        Debug.LogError($"Widget Named : {selection.name} Not Found");

                                        callbackResults.results = $"Widget Named : {selection.name} Not Found";
                                        callbackResults.resultsCode = Helpers.ErrorCode;

                                        cleared = false;

                                        break;
                                    }
                                }
                            }

                            if (cleared)
                            {
                                focusedSelectionData.Clear();

                                if (!HasActiveSelections())
                                {
                                    callbackResults.results = "Focused Selection Cleared Successfully.";
                                    callbackResults.resultsCode = Helpers.SuccessCode;
                                }
                                else
                                {
                                    callbackResults.results = "Focused Selection Failed To Clear For Some Reason.";
                                    callbackResults.resultsCode = Helpers.ErrorCode;
                                }
                            }
                            else
                            {
                                callbackResults.results = $"Couldn't Clear Content.";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            focusedSelectionData.Clear();

                            if (!HasActiveSelections())
                            {
                                callbackResults.results = "Focused Selection Cleared Successfully.";
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = "Focused Selection Failed To Clear For Some Reason.";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                    }
                    else
                    {
                        callbackResults.results = "No Widgets Found To Clear.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }

                    callback?.Invoke(callbackResults);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"==> [OnClearFocusedSelectionsInfo] Exception Found : {exception.Message}");
                    throw exception;
                }
            }

            public void OnSetFocusedWidgetSelectionInfo(FocusedSelectionInfo<SceneDataPackets> newSelectionInfo, bool isActiveSelection = true, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (HasActiveSelections())
                {
                    OnClearFocusedSelectionsInfo(cleared =>
                    {
                        if (Helpers.IsSuccessCode(cleared.resultsCode))
                        {
                            focusedSelectionData.selections = new List<FocusedSelectionInfo<SceneDataPackets>>();
                            focusedSelectionData.selections.Add(newSelectionInfo);

                            focusedSelectionData.selectionType = newSelectionInfo.selectionInfoType;
                            focusedSelectionData.isActiveSelection = isActiveSelection;

                            if (focusedSelectionData.selections.Count > 0)
                            {
                                callbackResults.results = "New Focused Selection Info List Set.";
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = "New Focused Selection Info List Not Set - Check Here.";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = $"Failed To Clear Focused Selection Data.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    });
                }
                else
                {
                    focusedSelectionData.selections = new List<FocusedSelectionInfo<SceneDataPackets>>();
                    focusedSelectionData.selections.Add(newSelectionInfo);
                    focusedSelectionData.selectionType = newSelectionInfo.selectionInfoType;
                    focusedSelectionData.isActiveSelection = isActiveSelection;

                    if (focusedSelectionData.selections.Count > 0)
                    {
                        callbackResults.results = "New Focused Selection Info List Set.";
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = "New Focused Selection Info List Not Set - Check Here.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }

                callback?.Invoke(callbackResults);
            }

            public void OnSetFocusedWidgetSelectionInfo(List<FocusedSelectionInfo<SceneDataPackets>> newSelectionInfoList, FocusedSelectionType selectionType, bool isActiveSelection = true, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (HasActiveSelections())
                {
                    OnClearFocusedSelectionsInfo(cleared =>
                    {
                        if (Helpers.IsSuccessCode(cleared.resultsCode))
                        {
                            focusedSelectionData.selections = newSelectionInfoList;
                            focusedSelectionData.selectionType = selectionType;
                            focusedSelectionData.isActiveSelection = isActiveSelection;

                            if (focusedSelectionData.selections.Count > 0)
                            {
                                callbackResults.results = "New Focused Selection Info List Set.";
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = "New Focused Selection Info List Not Set - Check Here.";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = $"Failed To Clear Focused Selection Data.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    });
                }
                else
                {
                    focusedSelectionData.selections = newSelectionInfoList;
                    focusedSelectionData.selectionType = selectionType;
                    focusedSelectionData.isActiveSelection = isActiveSelection;

                    if (focusedSelectionData.selections.Count > 0)
                    {
                        callbackResults.results = "New Focused Selection Info List Set.";
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = "New Focused Selection Info List Not Set - Check Here.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }

                callback?.Invoke(callbackResults);
            }

            public void OnAddFocusedWidgetSelectionInfo(FocusedSelectionInfo<SceneDataPackets> newSelectionInfo, bool isActiveSelection = true, Action<CallbackDatas<FocusedSelectionInfo<SceneDataPackets>>> callback = null)
            {
                CallbackDatas<FocusedSelectionInfo<SceneDataPackets>> callbackResults = new CallbackDatas<FocusedSelectionInfo<SceneDataPackets>>();

                focusedSelectionData.selections.Add(newSelectionInfo);
                focusedSelectionData.selectionType = newSelectionInfo.selectionInfoType;
                focusedSelectionData.isActiveSelection = isActiveSelection;

                if (focusedSelectionData.selections.Count > 0)
                {
                    callbackResults.results = "New Focused Selection Info List Set.";
                    callbackResults.data = focusedSelectionData.selections;
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "New Focused Selection Info List Not Set - Check Here.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void OnRemoveFocusedWidgetSelectionInfo(FocusedSelectionInfo<SceneDataPackets> selectionToRemove, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                focusedSelectionData.selections.Remove(selectionToRemove);

                HasFocusedSelectionInfo(selectionToRemove.name, selectionCallback =>
                {
                    if (!Helpers.IsSuccessCode(selectionCallback.resultsCode))
                    {
                        callbackResults.results = $"{selectionToRemove.name} Removed Successfully.";
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"{selectionToRemove.name} Couldn't Be Removed.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                });

                callback?.Invoke(callbackResults);
            }

            public void SetSelectionInfoState(List<UIScreenWidget<SceneDataPackets>> selectionList, FocusedSelectionType selectionType, Action<CallbackData<UIScreenWidget<SceneDataPackets>>> callback = null)
            {
                CallbackData<UIScreenWidget<SceneDataPackets>> callbackResults = new CallbackData<UIScreenWidget<SceneDataPackets>>();

                if (selectionList != null && selectionList.Count > 0)
                {
                    GetFocusedSelectionState(selectionType, selectionStateCallback =>
                    {
                        if (Helpers.IsSuccessCode(selectionStateCallback.resultsCode))
                        {
                            foreach (var selection in selectionList)
                                selection.OnSelectionFrameState(selectionStateCallback.data);

                            callbackResults.results = $"Selected : {selectionList.Count} Widgets To State : {selectionStateCallback.data.state} - From Type : {selectionStateCallback.data.selectionInfoType} Nou.";
                            callbackResults.data = selectionList.FindLast(x => x.GetActive());
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = selectionStateCallback.results;
                            callbackResults.data = default;
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    });
                }
                else
                {
                    callbackResults.results = "Selection Info List Is Null / Empty.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public FocusedSelectionType GetFocusedSelectionTypeFromState(InputUIState state)
            {
                var selectionData = selectionStates.Find(selection => selection.state == state);

                if (selectionData != null)
                {
                    return selectionData.selectionInfoType;
                }

                return FocusedSelectionType.Default;
            }

            public void GetFocusedSelectionState(FocusedSelectionType selectionType, Action<CallbackData<FocusedSelectionStateInfo>> callback)
            {
                CallbackData<FocusedSelectionStateInfo> callbackResults = new CallbackData<FocusedSelectionStateInfo>();

                var selectionData = selectionStates.Find(selection => selection.selectionInfoType == selectionType);

                if (selectionData != null)
                {
                    callbackResults.results = $"Found Focused Selection Info Data Of Type : {selectionType}";
                    callbackResults.data = selectionData;
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Couldn't Find Focused Selection Info Data Of Type : {selectionType}";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public List<AppData.FocusedSelectionStateInfo> GetSelectionStates()
            {
                return selectionStates;
            }

            #endregion

            #endregion

            #endregion
        }

        #region Audio

        [Serializable]
        public class AudioPlayerData
        {
            #region Components

            public string name;

            [Space(5)]
            public AudioClip value;

            [Space(5)]
            public AudioType audioType;

            [HideInInspector]
            public AudioSource audioPlayer;

            #endregion

            #region Main

            public void Initialize(AudioSource audioPlayer) => this.audioPlayer = audioPlayer;

            public void PlayAudio(ulong delay = 0)
            {
                if (audioPlayer != null)
                {
                    audioPlayer.clip = value;
                    audioPlayer.Play(delay);
                }
                else
                    Debug.LogWarning("--> Failed : Audio Player Component Not Found / Missing.");
            }

            public void StopAudio()
            {
                if (audioPlayer != null)
                {
                    audioPlayer.clip = null;
                    audioPlayer.Stop();
                }
                else
                    Debug.LogWarning("--> Failed : Audio Player Component Not Found / Missing.");
            }

            #endregion
        }

        #endregion

        [Serializable]
        public struct AssetImportField
        {
            public string name;

            [Space(5)]
            public GameObject fieldAssignedIcon;

            [Space(5)]
            public TMP_Text fieldText;

            [Space(5)]
            public AssetFieldType assetType;

            [Space(5)]
            public string placeHolderMessage;
        }

        [Serializable]
        public struct RuntimeValue<T>
        {
            public string name;

            [Space(5)]
            public T value;

            [Space(5)]
            public BuildType runtimeType;

            [Space(5)]
            public RuntimeValueType valueType;
        }

        [Serializable]
        public class AssetInfoField
        {
            public string name;
            public int? value;

            [Space(5)]
            public InfoDisplayerFieldType type;
        }

        [Serializable]
        public class InputSliderValueContent<T>
        {
            #region Components

            public string name;

            [Space(5)]
            public UISlider<T> sliderValue;

            [Space(5)]
            public UIInputField<T> inputValue;

            #endregion

            #region Main

            void Initialized(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (sliderValue.value != null && inputValue.value != null)
                {
                    callbackResults.results = "";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void SetValue(string value)
            {

            }

            #endregion
        }

        #region Notification System

        [Serializable]
        public struct Notification
        {
            public string title;

            [Space(5)]
            public string message;

            [Space(5)]
            public NotificationType notificationType;

            [Space(5)]
            public UIScreenType screenType;

            [Space(5)]
            public SceneAssetPivot screenPosition;

            [Space(5)]
            public bool blurScreen;

            [Space(5)]
            public float delay;

            [Space(5)]
            public float duration;

            [HideInInspector]
            public bool proccessed;
        }

        [Serializable]
        public class NotificationWidget
        {
            #region Components

            public string name;

            [Space(5)]
            public RectTransform value;

            [Space(5)]
            public UIText<TextDataPackets> titleDisplayer;

            [Space(5)]
            public UIText<TextDataPackets> messageDisplayer;

            [Space(5)]
            public NotificationType notificationType;

            [Space(5)]
            public UIScreenType screenType;

            UIScreenWidgetContainer widgetContainer;

            #endregion

            #region Main

            public void SetNotificationScreenData(Notification notification, UIScreenWidgetContainer widgetContainer)
            {
                if (titleDisplayer.value != null)
                    titleDisplayer.value.text = notification.title;
                else
                    AppMonoDebugManager.Instance.LogWarning($"Title Displayer Value Missing / Not Assigned In The Editor --> For Widget Named : {name}.", "NotificationWidget", () => SetNotificationScreenData(notification, widgetContainer));

                if (messageDisplayer.value != null)
                    messageDisplayer.value.text = notification.message;
                else
                    AppMonoDebugManager.Instance.LogWarning($"Message Displayer Value Missing / Not Assigned In The Editor --> For Widget Named : {name}.", "NotificationWidget", () => SetNotificationScreenData(notification, widgetContainer));

                if (value != null)
                {
                    value.transform.SetParent(widgetContainer.value, true);
                    this.widgetContainer = widgetContainer;
                }
                else
                    AppMonoDebugManager.Instance.LogWarning($"NotificationWidget Value Missing / Not Assigned In The Editor --> For Widget Named : {name}.", "NotificationWidget", () => SetNotificationScreenData(notification, widgetContainer));
            }

            public RectTransform GetTransform()
            {
                return value;
            }

            public RectTransform GetHiddenScreenMountPoint()
            {
                return widgetContainer.hiddenScreenPoint;
            }

            public RectTransform GetVisibleScreenMountPoint()
            {
                return widgetContainer.visibleScreenPoint;
            }

            #endregion
        }

        [Serializable]
        public struct UIScreenWidgetContainer
        {
            public string name;

            [Space(5)]
            public RectTransform value;

            [Space(5)]
            public NotificationType notificationType;

            [Space(5)]
            public UIScreenType screenType;

            [Space(5)]
            public SceneAssetPivot screenPosition;

            [Space(5)]
            public RectTransform visibleScreenPoint, hiddenScreenPoint;
        }

        [Serializable]
        public class NotificationCommand : ICommand
        {
            Notification notification;

            public NotificationCommand(Notification notification) => this.notification = notification;

            public void Execute() => NotificationSystemManager.Instance.ShowNotification(notification);

            public void Undo()
            {

            }
        }

        #endregion

        #region Screen Capture Data

        [Serializable]
        public struct ImageData
        {
            public Texture2D texture;

            public byte[] data;

            public AssetFileExtensionType extensionType;

            public StorageDirectoryData storageData;
        }

        #endregion

        [Serializable]
        public struct ScreenAssetUIData
        {
            public Image thumbnail;

            [Space(5)]
            public TMP_Text title;

            [Space(5)]
            public TMP_Text description;

            [Space(5)]
            public TMP_Text dateTime;
        }

        [Serializable]
        public struct DirectoryInfo
        {
            public string name;
            public AppData.SelectableAssetType assetType;
            public StorageDirectoryData storageData;
            public bool dataAlreadyExistsInTargetDirectory;
        }

        [Serializable]
        public class AssetInfoHandler
        {
            [Space(5)]
            public List<AssetInfoField> fields = new List<AssetInfoField>();

            public void UpdateInfoField(AssetInfoField infoField, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                IsValidField(fields, fieldCheckCallback => 
                {
                    if(fieldCheckCallback.Success())
                    {
                        var field = fields.Find(field => field.type == infoField.type);

                        IsValidField(field, validFieldCallback => 
                        {
                            if(validFieldCallback.Success())
                            {
                                fields.Remove(field);

                                IsValidField(fields.Find(fieldData => fieldData.type == infoField.type), fieldExistCheckCallback => 
                                {
                                    if(!fieldExistCheckCallback.Success())
                                    {
                                        fields.Add(infoField);

                                        IsValidField(fields.Find(fieldData => fieldData.type == infoField.type), fileUpdatredCallback => 
                                        {
                                            if(fileUpdatredCallback.Success())
                                            {
                                                callbackResults.results = $"Field Of Type : {infoField.type} - Has Been Updated.";
                                                callbackResults.resultsCode = Helpers.SuccessCode;
                                            }
                                            else
                                            {
                                                callbackResults.results = $"Couldn't Update Field Of Type : {infoField.type} - {fileUpdatredCallback.results}.";
                                                callbackResults.resultsCode = Helpers.ErrorCode;
                                            }
                                        });
                                    }
                                    else
                                    {
                                        callbackResults.results = $"Couldn't Update Field Of Type : {infoField.type} - Check Here.";
                                        callbackResults.resultsCode = Helpers.ErrorCode;
                                    }
                                });
                            }
                            else
                                callbackResults = validFieldCallback;
                        });
                    }
                    else
                        callbackResults = fieldCheckCallback;
                });

                callback?.Invoke(callbackResults);
            }

            public void GetInfoField(Action<CallbackDatas<AssetInfoField>> callback = null)
            {
                CallbackDatas<AssetInfoField> callbackResults = new CallbackDatas<AssetInfoField>();

                Helpers.GetComponentsNotNullOrEmpty(fields, checkComponentsCallback => 
                {
                    if(checkComponentsCallback.Success())
                    {
                        callbackResults = checkComponentsCallback;
                    }
                    else
                    {
                        callbackResults.results = checkComponentsCallback.results;
                        callbackResults.data = default;
                        callbackResults.resultsCode = checkComponentsCallback.resultsCode;
                    }
                });

                callback?.Invoke(callbackResults);
            }

            void IsValidField(AssetInfoField field, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if(field.value == null)
                {
                    callbackResults.results = $"Field Of Type : {field.type} Value Is Missing / Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;

                    callback?.Invoke(callbackResults);
                }

                if(string.IsNullOrEmpty(field.name))
                {
                    callbackResults.results = $"Field Of Type : {field.type} Value Is Missing / Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;

                    callback?.Invoke(callbackResults);
                }

                if(field.value != null && !string.IsNullOrEmpty(field.name))
                {
                    callbackResults.results = $"Field : {field.name} Of Type : {field.type} Is Valid.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }

                callback?.Invoke(callbackResults);
            }

            void IsValidField(List<AssetInfoField> fields, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (Helpers.ComponentIsNotNullOrEmpty(fields))
                {
                    foreach (var field in fields)
                    {
                        IsValidField(field, fieldCheckCallback => 
                        {
                            if(!fieldCheckCallback.Success())
                            {
                                callbackResults.results = $"Field Of Type : {field.type} Value Is Missing / Null.";
                                callbackResults.resultsCode = Helpers.ErrorCode;

                                callback?.Invoke(callbackResults);
                            }
                            else
                            {
                                callbackResults.results = $"Field : {field.name} Of Type : {field.type} Is Valid.";
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                        });
                    }
                }
                else
                    Debug.LogWarning("--> Info Fields Are Not Yet Initialized.");

                callback?.Invoke(callbackResults);
            }

            public void GetInfoField(InfoDisplayerFieldType fieldType, Action<CallbackData<AssetInfoField>> callback = null)
            {
                CallbackData<AssetInfoField> callbackResults = new CallbackData<AssetInfoField>();

                Helpers.GetComponentsNotNullOrEmpty<AssetInfoField>(fields, checkComponentsCallback =>
                {
                    if(checkComponentsCallback.Success())
                    {
                        GetInfoField(getFieldCallback => 
                        {
                            if(getFieldCallback.Success())
                            {
                                AssetInfoField field = getFieldCallback.data.Find(field => field.type == fieldType);

                                IsValidField(field, vaildFieldCallback => 
                                {
                                    if(vaildFieldCallback.Success())
                                        callbackResults.data = field;

                                    callbackResults.results = vaildFieldCallback.results;
                                    callbackResults.resultsCode = vaildFieldCallback.resultsCode;
                                });
                            }
                            else
                            {
                                callbackResults.results = getFieldCallback.results;
                                callbackResults.data = default;
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        });
                    }
                    else 
                    {
                        callbackResults.results = checkComponentsCallback.results;
                        callbackResults.data = default;
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                });

                callback?.Invoke(callbackResults);
            }

            public AssetInfoField GetInfoField(InfoDisplayerFieldType fieldType)
            {
                AssetInfoField info = new AssetInfoField();

                if (fields != null)
                {
                    foreach (var field in fields)
                    {
                        if (field.type == fieldType)
                        {
                            info = field;
                            break;
                        }
                    }
                }

                return info;
            }

            //public string name;
            //public int verticesCount, trianglesCount, uvCount;
        }

        [Serializable]
        public struct UIHidableScreenContent
        {
            #region Components

            public string name;

            [Space(5)]
            public GameObject value;

            #endregion

            #region Main

            public void Show()
            {
                if (value != null)
                    value.SetActive(true);
                else
                    Debug.LogWarning("--> Show UIHidableScreenContent Failed : Value Is Missing / Null.");
            }

            public void Hide()
            {
                if (value != null)
                    value.SetActive(false);
                else
                    Debug.LogWarning("--> Hide UIHidableScreenContent Failed : Value Is Missing / Null.");
            }

            #endregion
        }

        [Serializable]
        public struct StorageDirectoryData
        {
            public string name;

            [Space(5)]
            public DirectoryType type;

            [HideInInspector]
            public string path;

            [HideInInspector]
            public string directory;
        }

        [Serializable]
        public struct AssetField
        {
            public string name;

            [Space(5)]
            public AssetFieldType fieldType;

            [Space(5)]
            public AssetFileExtensionType extensionType;

            [Space(5)]
            public string path;

            [Space(5)]
            public DirectoryType directoryType;
        }

        [Serializable]
        public struct AssetPath
        {
            public string name;

            [Space(5)]
            public string path;

            [Space(5)]
            public AssetFieldType fieldType;

            [Space(5)]
            public bool active;
        }

        [Serializable]
        public struct DropDownContentData
        {
            public string name;

            [Space(5)]
            public List<string> data;

            [Space(5)]
            public DropDownContentType contentType;
        }

        [Serializable]
        public struct RendererMaterial
        {
            #region Components

            public string name;

            [Space(5)]
            public Material value;

            [Space(5)]
            public RendererMaterialType materialType;

            RenderingSettingsManager renderingManager;

            #endregion

            #region Main

            public void Init()
            {
                renderingManager = RenderingSettingsManager.Instance;
            }

            public void UpdateToMatchMaterial(Material material)
            {

                Debug.Log($"--> Updating Material : {materialType.ToString()}");

                if (renderingManager == null)
                    renderingManager = RenderingSettingsManager.Instance;

                if (value != null)
                {
                    if (renderingManager)
                    {
                        value.SetTexture(renderingManager.GetMaterialTextureID(MaterialTextureType.MainTexture), material.GetTexture(renderingManager.GetMaterialTextureID(MaterialTextureType.MainTexture)));
                    }
                    else
                        Debug.LogWarning("--> Rendering Manager Not Yet Initialized.");
                }
                else
                    Debug.LogWarning("--> Material Value Missing / Null.");
            }

            #endregion
        }

        [Serializable]
        public class MaterialProperties
        {
            #region Components

            public string mainTexturePath;

            [HideInInspector]
            public string normalMapTexturePath;

            [HideInInspector]
            public string aoMapTexturePath;

            #region Properties

            [HideInInspector]
            public float glossiness;

            [HideInInspector]
            public float bumpScale;

            [HideInInspector]
            public float aoStrength;

            [HideInInspector]
            public MaterialProperties properties;

            #endregion

            #endregion
        }

        [Serializable]
        public class SceneAsset
        {
            #region Components

            public string name;

            [Space(5)]
            public GameObject modelAsset;

            [Space(5)]
            public AssetInfoHandler info;

            [Space(5)]
            public SceneObject sceneObject;

            [Space(5)]
            public MaterialProperties materialProperties;

            [Space(5)]
            public float sceneScale;

            [Space(5)]
            public string description;

            //[HideInInspector]
            public SceneAssetModeType currentAssetMode;

            [HideInInspector]
            public string creationDateTime;

            [Space(5)]
            public bool hasMLTFile;

            [HideInInspector]
            public StorageDirectoryData storageData;

            [HideInInspector]
            public List<AssetField> assetFields;

            public bool dontShowMetadataWidget;

            public SceneAssetCategoryType categoryType;

            [HideInInspector]
            public DefaultUIWidgetActionState defaultWidgetActionState;

            public Vector3 assetImportPosition;

            public Vector3 assetImportRotation;

            #endregion

            #region Main

            public SceneAssetData ToSceneAssetData()
            {
                return new SceneAssetData
                {
                    name = this.name,
                    info = this.info,
                    sceneObject = this.sceneObject,
                    materialProperties = this.materialProperties,
                    sceneScale = this.sceneScale,
                    description = this.description,
                    creationDateTime = this.creationDateTime,
                    currentAssetMode = this.currentAssetMode,
                    hasMLTFile = this.hasMLTFile,
                    storageData = this.storageData,
                    assetFields = this.assetFields,
                    showMetadataWidget = this.dontShowMetadataWidget,
                    categoryType = this.categoryType,
                    defaultWidgetActionState = this.defaultWidgetActionState,
                    assetImportPosition = this.assetImportPosition,
                    assetImportRotation = this.assetImportRotation
                };
            }

            public AssetInfoHandler GetInfo()
            {
                //if(string.IsNullOrEmpty(info.name))
                //    info.name = name;

                return info;
            }

            public void SetMaterialProperties(MaterialProperties properties)
            {
                materialProperties = properties;
            }

            public MaterialProperties GetMaterialProperties()
            {
                Debug.Log($"--> Get Material Propertis - Glossiness : {materialProperties.glossiness} - Bump Scale : {materialProperties.bumpScale} - AO Strength : {materialProperties.aoStrength}");
                return materialProperties;
            }

            public void SetSceneAssetModelContainer(Transform container, bool keepPose)
            {
                if (modelAsset == null || container == null)
                    return;

                modelAsset.transform.SetParent(container, keepPose);
            }

            public void AddAssetField(AssetField field)
            {
                if (assetFields == null)
                    assetFields = new List<AssetField>();


                if (!assetFields.Contains(field))
                    assetFields.Add(field);
            }

            public AssetField GetAssetField(AssetFieldType fieldType)
            {
                AssetField assetField = new AssetField();

                if (assetFields != null)
                {
                    foreach (var field in assetFields)
                    {
                        if (field.fieldType == fieldType)
                        {
                            assetField = field;
                            break;
                        }
                    }
                }
                else
                    Debug.LogWarning("--> Asset Field List Is Null.");

                return assetField;
            }

            public AssetField GetAssetField(AssetFileExtensionType extensionType)
            {
                AssetField assetField = new AssetField();

                if (assetFields != null)
                {
                    foreach (var field in assetFields)
                    {
                        if (field.extensionType == extensionType)
                        {
                            assetField = field;
                            break;
                        }
                    }
                }

                return assetField;
            }

            public AssetField GetAssetField(DirectoryType directoryType)
            {
                AssetField assetField = new AssetField();

                if (assetFields != null)
                {
                    foreach (var field in assetFields)
                    {
                        if (field.directoryType == directoryType)
                        {
                            assetField = field;
                            break;
                        }
                    }
                }

                return assetField;
            }

            #endregion
        }

        [Serializable]
        public class SceneAssetData : SerializableData
        {
            #region Components

            public AssetInfoHandler info;

            public SceneObject sceneObject;

            public MaterialProperties materialProperties;

            public float sceneScale;

            public string description;

            public string creationDateTime;

            public SceneAssetModeType currentAssetMode;

            public bool hasMLTFile;

            public string materialShaderName;

            public List<AssetField> assetFields;

            public bool showMetadataWidget;

            public SceneAssetCategoryType categoryType;

            public DefaultUIWidgetActionState defaultWidgetActionState;

            public Vector3 assetImportPosition;

            public Vector3 assetImportRotation;

            #endregion

            #region Main

            public SceneAsset ToSceneAsset()
            {
                SceneAsset asset = new SceneAsset
                {
                    name = this.name,
                    info = this.info,
                    sceneObject = this.sceneObject,
                    materialProperties = materialProperties,
                    sceneScale = this.sceneScale,
                    description = this.description,
                    creationDateTime = this.creationDateTime,
                    currentAssetMode = this.currentAssetMode,
                    storageData = this.storageData,
                    hasMLTFile = this.hasMLTFile,
                    assetFields = this.assetFields,
                    dontShowMetadataWidget = this.showMetadataWidget,
                    categoryType = this.categoryType,
                    defaultWidgetActionState = this.defaultWidgetActionState,
                    assetImportPosition = this.assetImportPosition,
                    assetImportRotation = this.assetImportRotation
                };

                return asset;
            }


            public void AddAssetField(AssetField field)
            {
                if (assetFields == null)
                    assetFields = new List<AssetField>();


                if (!assetFields.Contains(field))
                    assetFields.Add(field);
            }

            public void RemoveField(AssetField field)
            {
                if (assetFields == null)
                {
                    Debug.LogWarning("--> Asset Fields Are Null.");
                    return;
                }

                if (assetFields.Contains(field))
                    assetFields.Remove(field);
            }

            public void ReplaceField(AssetField field, AssetField newField)
            {
                if (assetFields == null)
                {
                    Debug.LogWarning("--> Asset Fields Are Null.");
                    return;
                }

                if (assetFields.Contains(field))
                    assetFields.Remove(field);

                if (!assetFields.Contains(newField))
                    assetFields.Add(newField);
            }

            public AssetField GetAssetField(AssetFieldType fieldType)
            {
                AssetField assetField = new AssetField();

                if (assetFields != null)
                {
                    foreach (var field in assetFields)
                    {
                        if (field.fieldType == fieldType)
                        {
                            assetField = field;
                            break;
                        }
                    }
                }
                else
                    Debug.LogWarning("--> Asset Field List Is Null.");

                return assetField;
            }

            public AssetField GetAssetField(AssetFileExtensionType extensionType)
            {
                AssetField assetField = new AssetField();

                if (assetFields != null)
                {
                    foreach (var field in assetFields)
                    {
                        if (field.extensionType == extensionType)
                        {
                            assetField = field;
                            break;
                        }
                    }
                }

                return assetField;
            }

            public AssetField GetAssetField(DirectoryType directoryType)
            {
                AssetField assetField = new AssetField();

                if (assetFields != null)
                {
                    foreach (var field in assetFields)
                    {
                        if (field.directoryType == directoryType)
                        {
                            assetField = field;
                            break;
                        }
                    }
                }

                return assetField;
            }

            #endregion
        }

        [Serializable]
        public struct SceneAssetPose
        {
            public string name;

            [Space(5)]
            public Vector3 position;

            [Space(5)]
            public Quaternion rotation;

            [Space(5)]
            public Vector3 scale;
        }

        [Serializable]
        public struct MaterialShader
        {
            public string name;

            [Space(5)]
            public Shader value;

            [Space(5)]
            public ShaderType shaderType;
        }

        #region UI Actions

        [Serializable]
        public struct SelectionState
        {
            #region Components

            public string name;

            [Space(5)]
            public Sprite value;

            [Space(5)]
            public Color color;

            [Space(5)]
            public InputUIState state;

            #endregion
        }

        [Serializable]
        public abstract class UIInputComponent<T, U, V> : IUIInputComponent<V> where T : UnityEngine.Object
        {
            #region Components

            public string name;

            [Space(5)]
            public TMP_Text fieldName;

            [Space(5)]
            public List<UIImageDisplayer> fieldUIImageList;

            [Space(5)]
            public T value;

            [Space(5)]
            public List<SelectionState> selectionStates = new List<SelectionState>
        {
            new SelectionState
            {
                name = "Normal",
                color = Color.white,
                state = InputUIState.Normal
            },

            new SelectionState
            {
                name = "Selected",
                color = Color.white,
                state = InputUIState.Selected
            },

            new SelectionState
            {
                name = "Enabled",
                color = Color.white,
                state = InputUIState.Enabled
            },

            new SelectionState
            {
                name = "Disabled",
                color = Color.grey,
                state = InputUIState.Disabled
            }
        };

            [Space(5)]
            public U dataPackets;

            [Space(5)]
            public StatelessChildWidgets widgets;

            [Space(5)]
            public bool selectableInput;

            [Space(5)]
            public InputUIState inputState;

            [Space(5)]
            public GameObject selectionFrame;

            [Space(5)]
            public bool initialVisibilityState;

            #endregion

            #region Main

            /// <summary>
            /// Sets The Title Of The UI Input.
            /// </summary>
            /// <param name="title">The Title To Display On The UI Input.</param>
            public void SetTitle(string title)
            {
                if (title != null)
                {
                    if(fieldName)
                        fieldName.text = title;
                    else
                        Debug.LogWarning("--> Set Title Failed - Field Name Is Missing / Null.");
                }
                else
                    Debug.LogWarning("--> Set Title Failed - Title Is Missing / Null.");
            }

            /// <summary>
            /// Sets The Color Of The UI Input Field.
            /// </summary>
            /// <param name="color">The Color To Use.</param>
            public abstract void SetFieldColor(Color color);

            /// <summary>
            /// Returns The Current UI Input Interactable State.
            /// </summary>
            /// <returns></returns>
            public abstract bool GetInteractableState();

            /// <summary>
            /// Sets The Current UI Interactability State.
            /// </summary>
            /// <param name="interactable">The Interactable State Of The UI Input.</param>
            public abstract void SetInteractableState(bool interactable);

            /// <summary>
            /// Sets The Current UI Visibility State.
            /// </summary>
            /// <param name="visible">The Visibility State Of The UI Input.</param>
            public abstract void SetUIInputVisibilityState(bool visible);

            /// <summary>
            /// Returns The Current UI Input Visibility State.
            /// </summary>
            /// <returns></returns>
            public abstract bool GetUIInputVisibilityState();

            /// <summary>
            /// Sets The Current UI State.
            /// </summary>
            /// <param name="state">The Current State Of The input To Set.</param>
            public abstract void SetUIInputState(InputUIState state);

            /// <summary>
            /// Returns The Current UI State.
            /// </summary>
            /// <returns></returns>
            public abstract InputUIState GetUIInputState();

            /// <summary>
            /// Sets The States For The Child Widgets On This UI Input.
            /// </summary>
            /// <param name="interactable">The Interactable State Of The Child Widget.</param>
            /// <param name="isSelected">The Selection State Of The Child Widget.</param>
            public abstract void SetChildWidgetsState(bool interactable, bool isSelected);

            /// <summary>
            /// Sets Input To Selected State.
            /// </summary>
            public abstract void OnInputSelected();

            /// <summary>
            /// Sets Input To Deselected State.
            /// </summary>
            public abstract void OnInputDeselected();

            public abstract void SetUIInputState(V input, InputUIState state);

            /// <summary>
            /// This Function Is Fired When A Ponter Is Down On A UI Input.
            /// </summary>
            public abstract void OnInputPointerDownEvent();

            /// <summary>
            /// This Function Sets UI Image Value.
            /// </summary>
            public void SetUIImageValue(UIImageData image, UIImageDisplayerType imageType)
            {
                if (image.value != null)
                {
                    UIImageDisplayer displayer = fieldUIImageList.Find(displayer => displayer.imageDisplayerType == imageType);
                    displayer.value.sprite = image.value;
                }
                else
                    Debug.LogWarning($"--> Set UI Image Value Failed - Image IS Null.");
            }

            public void GetSelectionState(InputUIState state, Action<CallbackData<SelectionState>> callback)
            {
                CallbackData<SelectionState> callbackResults = new CallbackData<SelectionState>();

                if (selectionStates.Count > 0)
                {
                    SelectionState selectionState = selectionStates.Find(selectionState => selectionState.state == state);

                    if (selectionStates.Contains(selectionState))
                    {
                        callbackResults.results = "Success : Selection State Found.";
                        callbackResults.data = selectionState;
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"Failed : Selection State : {state} Not Found.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "Failed : There Are No Selection States Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            #endregion

            #region Events

            public delegate void EventsListeners(InputDropDownActionType actionType);

            public event EventsListeners _AddInputEventListener;

            protected void TriggerEvent(InputDropDownActionType actionType) => _AddInputEventListener?.Invoke(actionType);

            #endregion
        }

        [Serializable]
        public abstract class SelectableUIInputComponent<T, U, V> : AppMonoBaseClass, IPointerDownHandler where T : UnityEngine.Object
        {

            #region Components

            public string inputName;

            RectTransform uiInputTransform;

            protected UIInputComponent<T, U, V> input;

            #endregion

            #region Main

            public void Init(UIInputComponent<T, U, V> input)
            {
                this.input = input;
            }

            public void OnPointerDown(PointerEventData eventData)
            {
                if (uiInputTransform == null)
                    uiInputTransform = this.GetComponent<RectTransform>();

                if (uiInputTransform)
                {
                    Vector2 pos;

                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(uiInputTransform, eventData.position, eventData.enterEventCamera, out pos))
                        OnInputSelected();
                }
            }

            protected abstract void OnInputSelected();
            protected abstract void OnInputSelected(UIInputComponent<T, U, V> input);

            #endregion

        }

        [Serializable]
        public class UIButton<T> : UIInputComponent<Button, T, UIButton<T>>
        {
            #region Components

            [Space(5)]
            public InputActionButtonType actionType;

            #endregion

            #region Main

            public void SetButtonActionEvent(ActionEvents.ButtonAction<T> buttonEvent)
            {
                if (IsInitialized())
                {
                    if (buttonEvent != null)
                    {
                        value.onClick.AddListener(() => buttonEvent(this));
                        value.CancelInvoke();
                    }
                    else
                        value.onClick.RemoveAllListeners();
                }
                else
                    Debug.LogWarning("--> SetButtonActionEvent Failed : Value Is Missing / Null.");
            }

            public override bool GetInteractableState()
            {
                if (IsInitialized())
                    return value.interactable;
                else
                {
                    Debug.LogWarning("--> GetInteractableState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetInteractableState(bool interactable)
            {
                if (IsInitialized())
                {
                    value.interactable = interactable;

                    if (interactable)
                        inputState = InputUIState.Enabled;
                    else
                        inputState = InputUIState.Disabled;
                }
                else
                    Debug.LogWarning("--> Set UI Input Interactable State Failed : Value Is Missing / Null.");
            }

            public override void SetUIInputVisibilityState(bool visible)
            {
                if (IsInitialized())
                {
                    value.gameObject.SetActive(visible);

                    if (visible)
                        inputState = InputUIState.Shown;
                    else
                        inputState = InputUIState.Hidden;
                }
                else
                    Debug.LogWarning("--> Set UI Input Visibility State Failed : Value Is Missing / Null.");
            }

            public override bool GetUIInputVisibilityState()
            {
                if (IsInitialized())
                    return value.isActiveAndEnabled;
                else
                {
                    Debug.LogWarning("--> GetUIInputVisibilityState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetUIInputState(InputUIState state)
            {
                switch (state)
                {
                    case InputUIState.Deselect:

                        if (selectableInput)
                        {
                            Debug.LogError("---------------------> Set UI Input Selection State...........");

                            if (selectionFrame)
                                selectionFrame.SetActive(false);
                        }

                        break;

                    case InputUIState.Enabled:

                        SetInteractableState(true);

                        break;

                    case InputUIState.Disabled:

                        SetInteractableState(false);

                        break;

                    case InputUIState.Selected:

                        if (selectableInput)
                        {

                            Debug.LogError($"--> Set UI State : {state}");
                            SetUIInputVisibilityState(true);

                            if (selectionFrame)
                                selectionFrame.SetActive(true);
                        }

                        break;

                    case InputUIState.Shown:

                        SetUIInputVisibilityState(true);

                        break;

                    case InputUIState.Hidden:

                        SetUIInputVisibilityState(false);

                        break;
                }
            }

            public override void SetChildWidgetsState(bool interactable, bool isSelected)
            {
                widgets.SetWidgetsInteractableState(interactable, isSelected);
            }

            public override InputUIState GetUIInputState()
            {
                return inputState;
            }

            public override void OnInputSelected()
            {

                if (inputState == InputUIState.Selected)
                {
                    Debug.LogError($"--> Selected : {name}");

                    if (selectionFrame)
                        selectionFrame.SetActive(false);
                }
            }

            public override void OnInputDeselected()
            {
                if (inputState == InputUIState.Selected)
                {
                    Debug.LogError($"--> Deselected : {name}");

                    if (selectionFrame)
                        selectionFrame.SetActive(false);
                }
            }

            public override void SetUIInputState(UIButton<T> input, InputUIState state)
            {
                if (input == this)
                {
                    SetUIInputState(state);
                    Debug.LogError($"--> Setting UI Button : {this.name} To State : {state}");
                }
                else
                {
                    Debug.LogError($"-------> Deselection Called On : {this.name}.");
                }
            }

            public override void OnInputPointerDownEvent()
            {

            }

            bool IsInitialized()
            {
                return value;
            }

            public override void SetFieldColor(Color color)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        [Serializable]
        public class UIDropDown<T> : UIInputComponent<TMP_Dropdown, T, UIDropDown<T>> where T : DataPackets
        {
            #region Components

            [Space(5)]
            public InputDropDownActionType actionType;

            #endregion

            #region Main

            public void Initialize()
            {
                if (IsInitialized())
                {
                    if (selectableInput)
                    {
                        if (value.gameObject.GetComponent<SelectableInputDropdownHandler>() == null)
                        {
                            SelectableInputDropdownHandler handler = value.gameObject.AddComponent<SelectableInputDropdownHandler>();

                            if (handler)
                                handler.Init(this);
                            else
                                Debug.LogWarning("UIDropDown Initialize Failed : SelectableInputDropdownHandler Component Missing / Not Found.");
                        }
                    }
                }
            }

            bool IsInitialized()
            {
                return value;
            }

            public void SetContent(List<string> contentList)
            {
                if (contentList.Count > 0)
                {
                    if (value != null)
                    {
                        value.ClearOptions();

                        List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                        foreach (var content in contentList)
                        {
                            dropdownOption.Add(new TMP_Dropdown.OptionData() { text = content });
                        }

                        value.AddOptions(dropdownOption);

                        //value.onValueChanged.AddListener((value) => OnDropDownFilterOptions(value));
                    }
                    else
                        Debug.LogWarning("--> Set Content Failed : Value Missing / Null.");
                }
            }

            public void SetContent(List<string> contentList, string filter, string content)
            {
                if (contentList.Count > 0)
                {
                    if (value != null)
                    {
                        value.ClearOptions();

                        List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                        foreach (var item in contentList)
                        {
                            dropdownOption.Add(new TMP_Dropdown.OptionData() { text = (item.Contains(filter) ? content : item) });
                        }

                        value.AddOptions(dropdownOption);

                        //value.onValueChanged.AddListener((value) => OnDropDownFilterOptions(value));
                    }
                    else
                        Debug.LogWarning("--> Set Content Failed : Value Missing / Null.");
                }
            }

            public override bool GetInteractableState()
            {
                if (IsInitialized())
                    return value.interactable;
                else
                {
                    Debug.LogWarning("--> GetInteractableState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetInteractableState(bool interactable)
            {
                if (IsInitialized())
                {
                    value.interactable = interactable;

                    InputUIState state = (interactable) ? InputUIState.Enabled : InputUIState.Disabled;

                    GetSelectionState(state, selectionState =>
                    {
                        if (Helpers.IsSuccessCode(selectionState.resultsCode))
                            value.captionText.color = selectionState.data.color;
                        else
                            Debug.LogWarning($"--> SetInteractableState Failed With Results : {selectionState.results}");
                    });

                    if (interactable)
                        inputState = InputUIState.Enabled;
                    else
                        inputState = InputUIState.Disabled;
                }
                else
                    Debug.LogWarning("--> Set UI Input Interactable State Failed : Value Is Missing / Null.");
            }

            public override void SetUIInputVisibilityState(bool visible)
            {
                if (IsInitialized())
                {
                    value.gameObject.SetActive(visible);

                    if (visible)
                        inputState = InputUIState.Shown;
                    else
                        inputState = InputUIState.Hidden;
                }
                else
                    Debug.LogWarning("--> Set UI Input Visibility State Failed : Value Is Missing / Null.");
            }

            public override bool GetUIInputVisibilityState()
            {
                if (IsInitialized())
                    return value.isActiveAndEnabled;
                else
                {
                    Debug.LogWarning("--> GetUIInputVisibilityState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetUIInputState(InputUIState state)
            {
                switch (state)
                {
                    case InputUIState.Enabled:

                        SetInteractableState(true);

                        break;

                    case InputUIState.Disabled:

                        SetInteractableState(false);

                        break;

                    case InputUIState.Selected:

                        if (selectableInput)
                        {
                            if (inputState == InputUIState.Hidden)
                                SetUIInputVisibilityState(true);

                            if (inputState == InputUIState.Shown)
                            {
                                Debug.LogError("--> Set UI Input Selection State");

                                if (selectionFrame)
                                    selectionFrame.SetActive(true);
                            }
                        }

                        break;

                    case InputUIState.Shown:

                        SetUIInputVisibilityState(true);

                        break;

                    case InputUIState.Hidden:

                        SetUIInputVisibilityState(false);

                        break;
                }
            }

            public override void SetChildWidgetsState(bool interactable, bool isSelected)
            {
                widgets.SetWidgetsInteractableState(interactable, isSelected);
            }

            public override InputUIState GetUIInputState()
            {
                return inputState;
            }

            public override void OnInputSelected()
            {

                if (inputState == InputUIState.Selected)
                {
                    Debug.LogError($"--> Selected : {name}");

                    if (selectionFrame)
                        selectionFrame.SetActive(false);
                }
            }

            public override void OnInputDeselected()
            {
                if (inputState == InputUIState.Selected)
                {
                    Debug.LogError($"--> Deselected : {name}");

                    if (selectionFrame)
                        selectionFrame.SetActive(false);
                }
            }

            public override void SetUIInputState(UIDropDown<T> input, InputUIState state)
            {
                if (input == this)
                    SetUIInputState(state);
            }

            public override void OnInputPointerDownEvent() => TriggerEvent(actionType);

            public override void SetFieldColor(Color color)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        [Serializable]
        public class UIInputField<T> : UIInputComponent<TMP_InputField, T, UIInputField<T>>
        {
            #region Components

            [Space(5)]
            public Button clearButton;

            [Space(5)]
            public TMP_Text placeholderDisplayer;

            [Space(5)]
            public int characterLimit;

            [Space(5)]
            public InputFieldActionType actionType;

            [Space(5)]
            public InputFieldValueType valueType;

            string clearTextPlaceHolder;

            #endregion

            #region Main

            public void Initialize(string clearTextPlaceHolder = null)
            {
                if (clearButton != null)
                    clearButton.onClick.AddListener(OnClearField);

                if (value != null)
                    value.characterLimit = characterLimit;
            }

            public void SetPlaceHolderText(string placeholder)
            {
                this.clearTextPlaceHolder = placeholder;

                value.text = string.Empty;

                if (placeholderDisplayer)
                    placeholderDisplayer.text = placeholder;
            }

            public void SetPlaceHolderText(int placeholder)
            {
                this.clearTextPlaceHolder = placeholder.ToString();

                if (value != null)
                    value.text = string.Empty;
                else
                    Debug.LogWarning("--> SetPlaceHolderText Failed : Value Missing / Null.");

                if (placeholderDisplayer)
                    placeholderDisplayer.text = placeholder.ToString();
            }

            public void SetValue(string value) => this.value.text = value;

            public void OnClearField()
            {
                if (value)
                {
                    value.text = string.Empty;

                    if (placeholderDisplayer)
                        placeholderDisplayer.text = clearTextPlaceHolder;

                    value.Select();
                }
            }

            public void OnSelect()
            {
                if (value)
                {
                    if (!string.IsNullOrEmpty(value.text))
                        value.Select();
                }
                else
                    Debug.LogWarning("--> On Select Failed : Value Missing / Null.");
            }

            bool IsInitialized()
            {
                return value;
            }

            public override bool GetInteractableState()
            {
                if (IsInitialized())
                    return value.interactable;
                else
                {
                    Debug.LogWarning("--> GetInteractableState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetInteractableState(bool interactable)
            {
                if (IsInitialized())
                {
                    value.interactable = interactable;

                    if (clearButton != null)
                        clearButton.interactable = interactable;

                    if (interactable)
                        inputState = InputUIState.Enabled;
                    else
                        inputState = InputUIState.Disabled;
                }
                else
                    Debug.LogWarning("--> Set UI Input Interactable State Failed : Value Is Missing / Null.");
            }

            public override void SetUIInputVisibilityState(bool visible)
            {
                if (IsInitialized())
                {
                    value.gameObject.SetActive(visible);

                    if (visible)
                        inputState = InputUIState.Shown;
                    else
                        inputState = InputUIState.Hidden;
                }
                else
                    Debug.LogWarning("--> Set UI Input Visibility State Failed : Value Is Missing / Null.");
            }

            public override bool GetUIInputVisibilityState()
            {
                if (IsInitialized())
                    return value.isActiveAndEnabled;
                else
                {
                    Debug.LogWarning("--> GetUIInputVisibilityState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetUIInputState(InputUIState state)
            {
                switch (state)
                {
                    case InputUIState.Enabled:

                        SetInteractableState(true);

                        break;

                    case InputUIState.Disabled:

                        SetInteractableState(false);

                        break;

                    case InputUIState.Selected:

                        if (selectableInput)
                        {
                            if (inputState == InputUIState.Hidden)
                                SetUIInputVisibilityState(true);

                            if (inputState == InputUIState.Shown)
                            {
                                Debug.LogError("--> Set UI Input Selection State");

                                if (selectionFrame)
                                    selectionFrame.SetActive(true);
                            }
                        }

                        break;

                    case InputUIState.Shown:

                        SetUIInputVisibilityState(true);

                        break;

                    case InputUIState.Hidden:

                        SetUIInputVisibilityState(false);

                        break;
                }
            }

            public override void SetChildWidgetsState(bool interactable, bool isSelected)
            {
                widgets.SetWidgetsInteractableState(interactable, isSelected);
            }

            public override InputUIState GetUIInputState()
            {
                return inputState;
            }

            public override void OnInputSelected()
            {

                if (inputState == InputUIState.Selected)
                {
                    Debug.LogError($"--> Selected : {name}");

                    if (selectionFrame)
                        selectionFrame.SetActive(false);
                }
            }

            public override void OnInputDeselected()
            {
                if (inputState == InputUIState.Selected)
                {
                    Debug.LogError($"--> Deselected : {name}");

                    if (selectionFrame)
                        selectionFrame.SetActive(false);
                }
            }

            public override void SetUIInputState(UIInputField<T> input, InputUIState state)
            {
                if (input == this)
                {

                }
            }

            public override void OnInputPointerDownEvent()
            {

            }

            public override void SetFieldColor(Color color)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        [Serializable]
        public class UICheckbox<T> : UIInputComponent<Toggle, T, UICheckbox<T>>
        {
            #region Components

            [Space(5)]
            public CheckboxInputActionType actionType;

            [Space(5)]
            public bool initialSelectionState;

            [Space(5)]
            public bool initialInteractabilityState;

            #endregion

            #region Main

            public void Initialize()
            {
                if (IsInitialized())
                {
                    value.isOn = initialSelectionState;
                    value.interactable = initialInteractabilityState;
                    value.gameObject.SetActive(initialVisibilityState);
                }
                else
                    Debug.LogWarning("--> Initialize UICheckbox Failed : Value Is Missing / Null.");
            }

            public void SetInteractableState(bool interactable, bool isVisible)
            {
                value.interactable = interactable;
                value.gameObject.SetActive(isVisible);
            }

            public void SetSelectionState(bool isSelected)
            {
                if (IsInitialized())
                    value.isOn = isSelected;
                else
                    Debug.LogWarning("--> UICheckbox Selected Failed : Checkbox Value Is Missing / Null.");
            }

            bool IsInitialized()
            {
                return value;
            }

            public override bool GetInteractableState()
            {
                if (IsInitialized())
                    return value.interactable;
                else
                {
                    Debug.LogWarning("--> GetInteractableState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetInteractableState(bool interactable)
            {
                if (IsInitialized())
                {
                    value.interactable = interactable;

                    if (interactable)
                        inputState = InputUIState.Enabled;
                    else
                        inputState = InputUIState.Disabled;
                }
                else
                    Debug.LogWarning("--> Set UI Input Interactable State Failed : Value Is Missing / Null.");
            }

            public override void SetUIInputVisibilityState(bool visible)
            {
                if (IsInitialized())
                {
                    value.gameObject.SetActive(visible);

                    if (visible)
                        inputState = InputUIState.Shown;
                    else
                        inputState = InputUIState.Hidden;
                }
                else
                    Debug.LogWarning("--> Set UI Input Visibility State Failed : Value Is Missing / Null.");
            }

            public override bool GetUIInputVisibilityState()
            {
                if (IsInitialized())
                    return value.isActiveAndEnabled;
                else
                {
                    Debug.LogWarning("--> GetUIInputVisibilityState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetUIInputState(InputUIState state)
            {
                switch (state)
                {
                    case InputUIState.Enabled:

                        SetInteractableState(true);

                        break;

                    case InputUIState.Disabled:

                        SetInteractableState(false);

                        break;

                    case InputUIState.Selected:

                        if (selectableInput)
                        {
                            if (inputState == InputUIState.Hidden)
                                SetUIInputVisibilityState(true);

                            if (inputState == InputUIState.Shown)
                            {
                                Debug.LogError("--> Set UI Input Selection State");

                                if (selectionFrame)
                                    selectionFrame.SetActive(true);
                            }
                        }

                        break;

                    case InputUIState.Shown:

                        SetUIInputVisibilityState(true);

                        break;

                    case InputUIState.Hidden:

                        SetUIInputVisibilityState(false);

                        break;
                }
            }

            public override void SetChildWidgetsState(bool interactable, bool isSelected)
            {
                widgets.SetWidgetsInteractableState(interactable, isSelected);
            }

            public override InputUIState GetUIInputState()
            {
                return inputState;
            }

            public override void OnInputSelected()
            {

                if (inputState == InputUIState.Selected)
                {
                    Debug.LogError($"--> Selected : {name}");

                    if (selectionFrame)
                        selectionFrame.SetActive(false);
                }
            }

            public override void OnInputDeselected()
            {
                if (inputState == InputUIState.Selected)
                {
                    Debug.LogError($"--> Deselected : {name}");

                    if (selectionFrame)
                        selectionFrame.SetActive(false);
                }
            }

            public override void SetUIInputState(UICheckbox<T> input, InputUIState state)
            {

            }

            public override void OnInputPointerDownEvent()
            {

            }

            public override void SetFieldColor(Color color)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        [Serializable]
        public class UIText<T> : UIInputComponent<TMP_Text, T, UIText<T>>
        {
            #region Components

            [Space(5)]
            public ScreenTextType textType;

            #endregion

            #region Main

            bool IsInitialized()
            {
                return value;
            }

            public override bool GetInteractableState()
            {
                return false;
            }

            public override void SetInteractableState(bool interactable)
            {

            }

            public override void SetUIInputVisibilityState(bool visible)
            {
                if (IsInitialized())
                {
                    value.gameObject.SetActive(visible);

                    if (visible)
                        inputState = InputUIState.Shown;
                    else
                        inputState = InputUIState.Hidden;
                }
                else
                    Debug.LogWarning("--> Set UI Input Visibility State Failed : Value Is Missing / Null.");
            }

            public override bool GetUIInputVisibilityState()
            {
                if (IsInitialized())
                    return value.isActiveAndEnabled;
                else
                {
                    Debug.LogWarning("--> GetUIInputVisibilityState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetUIInputState(InputUIState state)
            {

            }

            public override void SetChildWidgetsState(bool interactable, bool isSelected)
            {
                widgets.SetWidgetsInteractableState(interactable, isSelected);
            }

            public override InputUIState GetUIInputState()
            {
                return inputState;
            }

            public override void OnInputSelected()
            {
            }

            public override void OnInputDeselected()
            {
            }

            public override void SetUIInputState(UIText<T> input, InputUIState state)
            {
                throw new NotImplementedException();
            }

            public override void OnInputPointerDownEvent()
            {
                throw new NotImplementedException();
            }

            public override void SetFieldColor(Color color)
            {
                throw new NotImplementedException();
            }

            public void SetScreenUITextValue(string value)
            {
                if (this.value)
                    this.value.text = value;
                else
                    Debug.LogWarning("--> SetScreenUITextValue Failed - Value Is Missing / Null.");
            }

            #endregion
        }

        [Serializable]
        public class UISlider<T> : UIInputComponent<Slider, T, UISlider<T>>
        {
            #region Components

            [Space(5)]
            public int initialMaxValue;

            [Space(5)]
            public SliderValueType valueType;

            #endregion

            #region Main

            public void Initialize()
            {
                if (IsInitialized())
                {
                    value.minValue = 0;
                    value.maxValue = (initialMaxValue > 0) ? initialMaxValue : 1;
                }
                else
                    Debug.LogWarning("--> Slider Value Component Not Assigned");
            }

            bool IsInitialized()
            {
                return value;
            }

            public override bool GetInteractableState()
            {
                if (IsInitialized())
                    return value.interactable;
                else
                {
                    Debug.LogWarning("--> GetInteractableState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetInteractableState(bool interactable)
            {
                if (IsInitialized())
                {
                    value.interactable = interactable;

                    if (interactable)
                        inputState = InputUIState.Enabled;
                    else
                        inputState = InputUIState.Disabled;
                }
                else
                    Debug.LogWarning("--> Set UI Input Interactable State Failed : Value Is Missing / Null.");
            }

            public override void SetUIInputVisibilityState(bool visible)
            {
                if (IsInitialized())
                {
                    value.gameObject.SetActive(visible);

                    if (visible)
                        inputState = InputUIState.Shown;
                    else
                        inputState = InputUIState.Hidden;
                }
                else
                    Debug.LogWarning("--> Set UI Input Visibility State Failed : Value Is Missing / Null.");
            }

            public override bool GetUIInputVisibilityState()
            {
                if (IsInitialized())
                    return value.isActiveAndEnabled;
                else
                {
                    Debug.LogWarning("--> GetUIInputVisibilityState Failed : Value Is Missing / Null.");
                    return false;
                }
            }

            public override void SetUIInputState(InputUIState state)
            {
                switch (state)
                {
                    case InputUIState.Enabled:

                        SetInteractableState(true);

                        break;

                    case InputUIState.Disabled:

                        SetInteractableState(false);

                        break;

                    case InputUIState.Selected:

                        if (selectableInput)
                        {
                            if (inputState == InputUIState.Hidden)
                                SetUIInputVisibilityState(true);

                            if (inputState == InputUIState.Shown)
                            {
                                Debug.LogError("--> Set UI Input Selection State");

                                if (selectionFrame)
                                    selectionFrame.SetActive(true);
                            }
                        }

                        break;

                    case InputUIState.Shown:

                        SetUIInputVisibilityState(true);

                        break;

                    case InputUIState.Hidden:

                        SetUIInputVisibilityState(false);

                        break;
                }
            }

            public override void SetChildWidgetsState(bool interactable, bool isSelected)
            {
                widgets.SetWidgetsInteractableState(interactable, isSelected);
            }

            public override InputUIState GetUIInputState()
            {
                return inputState;
            }

            public override void OnInputSelected()
            {

                if (inputState == InputUIState.Selected)
                {
                    Debug.LogError($"--> Selected : {name}");

                    if (selectionFrame)
                        selectionFrame.SetActive(false);
                }
            }

            public override void OnInputDeselected()
            {
                if (inputState == InputUIState.Selected)
                {
                    Debug.LogError($"--> Deselected : {name}");

                    if (selectionFrame)
                        selectionFrame.SetActive(false);
                }
            }

            public override void SetUIInputState(UISlider<T> input, InputUIState state)
            {
                throw new NotImplementedException();
            }

            public override void OnInputPointerDownEvent()
            {
                throw new NotImplementedException();
            }

            public override void SetFieldColor(Color color)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        [Serializable]
        public class UIInputSlider<T> : UIInputComponent<Slider, T, UIInputSlider<T>>
        {
            #region Components

            [Space(5)]
            public Slider slider;

            [Space(5)]
            public int initialMaxValue;

            [Space(5)]
            public TMP_InputField inputField;

            [Space(5)]
            public int inputFieldCharacterLimit;

            [Space(5)]
            public Button clearFieldsButton;

            [Space(5)]
            public SliderValueType valueType;

            float sliderValue;
            string inputValue;

            #endregion

            #region Main

            public void Initialize()
            {
                if (IsInitialized())
                {
                    slider.minValue = 0;
                    slider.maxValue = (initialMaxValue > 0) ? initialMaxValue : 1;
                }

                if (clearFieldsButton)
                    clearFieldsButton.onClick.AddListener(OnClearFieldButtonClickedEvent);
                else
                    Debug.LogWarning("--> UIInputSlider Clear Field Button Initialize Failed : Button Value Is Null.");
            }

            public void SetValue(string inputValue, float sliderValue)
            {
                if (IsInitialized())
                {
                    slider.value = sliderValue;
                    inputField.text = inputValue;
                }
            }

            public bool IsInitialized()
            {
                return (slider != null && inputField != null);
            }

            void OnClearFieldButtonClickedEvent()
            {
                if (IsInitialized())
                {
                    slider.value = sliderValue;
                    inputField.text = inputValue;
                }
            }

            public override bool GetInteractableState()
            {
                return false;
            }

            public override void SetInteractableState(bool interactable)
            {
                slider.interactable = interactable;
                inputField.interactable = interactable;
            }

            public override void SetUIInputVisibilityState(bool visible)
            {
                if (IsInitialized())
                {
                    if (visible)
                        inputState = InputUIState.Shown;
                    else
                        inputState = InputUIState.Hidden;
                }
                else
                    Debug.LogWarning("--> Set UI Input Visibility State Failed : Value Is Missing / Null.");
            }

            public override bool GetUIInputVisibilityState()
            {
                return false;
            }

            public override void SetUIInputState(InputUIState state)
            {
                switch (state)
                {

                    case InputUIState.Enabled:

                        SetInteractableState(true);

                        break;

                    case InputUIState.Disabled:

                        SetInteractableState(false);

                        break;
                }
            }

            public override void SetChildWidgetsState(bool interactable, bool isSelected)
            {
                widgets.SetWidgetsInteractableState(interactable, isSelected);
            }

            public override InputUIState GetUIInputState()
            {
                return inputState;
            }

            public override void OnInputSelected()
            {
            }

            public override void OnInputDeselected()
            {
            }

            public override void SetUIInputState(UIInputSlider<T> input, InputUIState state)
            {
                throw new NotImplementedException();
            }

            public override void OnInputPointerDownEvent()
            {
                throw new NotImplementedException();
            }

            public override void SetFieldColor(Color color)
            {
                if (fieldUIImageList.Count > 0)
                {
                    foreach (var uiField in fieldUIImageList)
                    {
                        if (uiField.value != null)
                            uiField.value.color = color;
                        else
                            Debug.LogWarning("--> SetFieldColor Failed : UI Field Value Null / Empty.");
                    }
                }
                else
                    Debug.LogWarning("--> SetFieldColor Failed : fieldUIImageList Values Are Null / Empty.");
            }

            #endregion
        }


        [Serializable]
        public class UIImageDisplayer<T> : UIInputComponent<Image, T, UIInputSlider<T>>
        {
            #region Components

            [Space(5)]
            public ScreenImageType imageType;

            #endregion

            #region Main

            public void SetImageData(ImageData screenCaptureData, ImageDataPackets dataPackets)
            {
                if (dataPackets.useData)
                {
                    value.rectTransform.sizeDelta = new Vector2(dataPackets.resolution.width, dataPackets.resolution.height);
                    value.preserveAspect = dataPackets.preserveAspectRatio;

                    if (screenCaptureData.texture != null)
                        value.sprite = Helpers.Texture2DToSprite(screenCaptureData.texture);
                    else
                        Debug.LogWarning("--> Failed : screenCaptureData.texture Is Null.");
                }
                else
                {
                    if (screenCaptureData.texture != null)
                        value.sprite = Helpers.Texture2DToSprite(screenCaptureData.texture);
                    else
                        Debug.LogWarning("--> Failed : screenCaptureData.texture Is Null.");
                }
            }

            public void SetImageData(Sprite image)
            {
                if (value != null)
                    value.sprite = image;
                else
                    Debug.LogWarning("--> SetImageData Failed - Displayer Value Missing.");
            }

            public void SetImageData(Texture2D image)
            {
                if (value != null)
                    value.sprite = Helpers.Texture2DToSprite(image);
                else
                    Debug.LogWarning("--> SetImageData Failed - Displayer Value Missing.");
            }

            public override bool GetInteractableState()
            {
                return false;
            }

            public override void SetInteractableState(bool interactable)
            {

            }

            public override void SetUIInputVisibilityState(bool visible)
            {

            }

            public override bool GetUIInputVisibilityState()
            {
                return false;
            }

            public override void SetUIInputState(InputUIState state)
            {
                switch (state)
                {

                    case InputUIState.Enabled:

                        SetInteractableState(true);

                        break;

                    case InputUIState.Disabled:

                        SetInteractableState(false);

                        break;
                }
            }

            public override void SetChildWidgetsState(bool interactable, bool isSelected)
            {
                widgets.SetWidgetsInteractableState(interactable, isSelected);
            }

            public override InputUIState GetUIInputState()
            {
                return inputState;
            }

            public override void OnInputSelected()
            {
                throw new NotImplementedException();
            }

            public override void OnInputDeselected()
            {
                throw new NotImplementedException();
            }

            public override void SetUIInputState(UIInputSlider<T> input, InputUIState state)
            {
                throw new NotImplementedException();
            }

            public override void OnInputPointerDownEvent()
            {
                throw new NotImplementedException();
            }

            public override void SetFieldColor(Color color)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        #endregion

        [Serializable]
        public struct StatelessChildWidgets
        {
            #region Components

            public List<GameObject> objectWidget;

            [Space(5)]
            public List<Button> buttonWidgets;

            [Space(5)]
            public List<Toggle> toggleWidgets;

            [Space(5)]
            public List<TMP_Text> textWidgets;

            [Space(5)]
            public List<ImageState> imageWidgets;

            #endregion

            #region Main

            public void SetWidgetsInteractableState(bool interactable, bool isSelected)
            {
                if (objectWidget != null)
                    foreach (var widget in objectWidget)
                        if (widget != null)
                            widget.SetActive(interactable);

                if (buttonWidgets != null)
                    foreach (var widget in buttonWidgets)
                        if (widget != null)
                            widget.interactable = interactable;

                if (toggleWidgets != null)
                    foreach (var widget in toggleWidgets)
                    {
                        if (widget)
                        {
                            widget.isOn = isSelected;
                            widget.interactable = interactable;
                        }
                        else
                            Debug.LogWarning("--> Button Stateless Child Widget Missing / Not Assigned.");
                    }

                if (textWidgets != null)
                    foreach (var widget in textWidgets)
                    {
                        if (widget)
                        {
                            widget.text = (interactable) ? "Asset Assigned." : (!string.IsNullOrEmpty(widget.text)) ? widget.text : "Assign Asset Here!";
                        }
                        else
                            Debug.LogWarning("--> Button Stateless Child Widget Missing / Not Assigned.");
                    }

                if (imageWidgets != null)
                {
                    foreach (var widget in imageWidgets)
                    {
                        if (widget.value)
                            widget.value.sprite = (interactable) ? widget.enabled : widget.disabled;
                        else
                            Debug.LogWarning("--> Image Widget Value Missing / Not Assigned In The Inspector Panel.");
                    }
                }
            }

            #endregion
        }

        [Serializable]
        public struct ImageState
        {
            public string name;

            [Space(5)]
            public Image value;

            [Space(5)]
            public Sprite enabled, disabled;
        }

        [Serializable]
        public struct ScreenText
        {
            public string name;

            [Space(5)]
            public string text;

            [Space(5)]
            public ScreenTextType textType;
        }

        [Serializable]
        public struct SceneAssetDynamicContentContainer
        {
            #region Components

            public string name;

            [Space(5)]
            public Transform value;

            [Space(5)]
            public Transform recycledContentContainer;

            [Space(5)]
            public ContentContainerType containerType;

            [Space(5)]
            public UIScreenType screenType;

            [Space(5)]
            public bool useLocalPose;

            List<GameObject> assets;

            SceneAssetPose assetPose;
            SceneAssetPose assetLocalPose;

            #endregion

            #region Main

            public void Init()
            {
                if (value)
                {
                    assetPose = new SceneAssetPose()
                    {
                        name = this.name,
                        position = value.transform.position,
                        rotation = value.transform.rotation,
                        scale = value.transform.localScale
                    };

                    assetLocalPose = new SceneAssetPose()
                    {
                        name = this.name,
                        position = value.transform.localPosition,
                        rotation = value.transform.localRotation,
                        scale = value.transform.localScale
                    };
                }
                else
                    Debug.LogWarning("--> Container Missing");
            }

            public void Add(GameObject sceneAsset, LayerMask arSceneAssetGroundLayer, bool worldPosition, bool fitInsideCollider, bool visible, float defaultContentScale, float contentScaleRatio, bool keepAssetCentered, bool isImport)
            {
                if (assets == null)
                    assets = new List<GameObject>();

                if (value)
                {
                    //sceneAsset.transform.SetParent(container, worldPosition);

                    sceneAsset.SetActive(visible);

                    if (!assets.Contains(sceneAsset))
                        assets.Add(sceneAsset);

                    sceneAsset.transform.localScale = Vector3.one;

                    if (fitInsideCollider)
                    {
                        List<MeshRenderer> assetObjectMeshRendererList = new List<MeshRenderer>();

                        if (sceneAsset.transform.childCount > 0)
                            assetObjectMeshRendererList = sceneAsset.GetComponentsInChildren<MeshRenderer>().ToList();


                        if (sceneAsset.GetComponent<MeshRenderer>())
                            assetObjectMeshRendererList.Add(sceneAsset.GetComponent<MeshRenderer>());

                        if (assetObjectMeshRendererList.Count > 0)
                        {

                            if (GetSceneAssetMaxExtent(GetSceneAssetBounds(sceneAsset, assetObjectMeshRendererList).extents) > defaultContentScale)
                            {

                                while (GetSceneAssetMaxExtent(GetSceneAssetBounds(sceneAsset, assetObjectMeshRendererList).extents) > defaultContentScale)
                                {
                                    sceneAsset.transform.localScale *= contentScaleRatio;
                                }
                            }
                            else
                            {
                                while (GetSceneAssetMaxExtent(GetSceneAssetBounds(sceneAsset, assetObjectMeshRendererList).extents) < defaultContentScale)
                                {
                                    sceneAsset.transform.localScale *= -contentScaleRatio;
                                }
                            }

                            Vector3 center = -(GetSceneAssetCenter(assetObjectMeshRendererList) - value.localPosition);


                            if (keepAssetCentered)
                            {
                                if (isImport)
                                {
                                    sceneAsset.transform.position = center;

                                    //if(SceneAssetsManager.Instance.GetCurrentSceneAsset().assetImportPosition == Vector3.zero)
                                    //    SceneAssetsManager.Instance.GetCurrentSceneAsset().assetImportPosition = center;

                                    if (SceneAssetsManager.Instance != null)
                                    {
                                        if (SceneAssetsManager.Instance.GetCurrentSceneAsset() != null)
                                        {

                                            SceneAssetsManager.Instance.GetCurrentSceneAsset().assetImportPosition = center;


                                            Debug.Log($"--------------> Imported Asset : {SceneAssetsManager.Instance.GetCurrentSceneAsset().name}'s Position Set To : {SceneAssetsManager.Instance.GetCurrentSceneAsset().assetImportPosition}");
                                        }
                                        else
                                            Debug.LogWarning("--> Add Asset To Container Failed - Scene Assets Manager Instance's Get Current Scene Asset Is Null / Not Found.");
                                    }
                                    else
                                        Debug.LogWarning("--> RG_Unity - Scene Assets Manager Not Yet Initialized.");
                                }
                                else
                                    sceneAsset.transform.position = center;
                            }
                            else
                                sceneAsset.transform.position = Vector3.zero;

                            if (sceneAsset.GetComponent<SceneAssetModelHandler>() == null)
                            {
                                SceneAssetModelHandler modelHandler = sceneAsset.AddComponent<SceneAssetModelHandler>();

                                if (modelHandler != null)
                                {
                                    modelHandler.Init(sceneAsset.transform.localScale);
                                }
                                else
                                    Debug.LogWarning($"--> RG_Unity - SceneAssetModelHandler Missing For Game Object : {sceneAsset.name}");
                            }

                            sceneAsset.transform.SetParent(value, worldPosition);

                            Debug.Log($"--> Asset Center : {GetSceneAssetCenter(assetObjectMeshRendererList)} - New Center : {center}");

                        }
                        else
                        {
                            Debug.LogWarning($"--> Failed To Get Mesh Renderer List For : {sceneAsset.name}");

                            return;
                        }
                    }
                }
                else
                    Debug.LogWarning("--> Container Missing");
            }

            Bounds GetSceneAssetBounds(GameObject sceneAssetObhect, List<MeshRenderer> renderers)
            {
                Bounds bounds = new Bounds();

                foreach (var renderer in renderers)
                {
                    bounds.Encapsulate(renderer.bounds);
                }

                return bounds;
            }

            float GetSceneAssetMaxExtent(Vector3 sceneAssetExtents)
            {
                float maxExtent = 0.0f;

                if (sceneAssetExtents.x > sceneAssetExtents.y && sceneAssetExtents.x > sceneAssetExtents.z)
                    maxExtent = sceneAssetExtents.x;
                else if (sceneAssetExtents.y > sceneAssetExtents.x && sceneAssetExtents.y > sceneAssetExtents.z)
                    maxExtent = sceneAssetExtents.y;
                else if (sceneAssetExtents.z > sceneAssetExtents.x && sceneAssetExtents.z > sceneAssetExtents.y)
                    maxExtent = sceneAssetExtents.z;
                else
                    maxExtent = sceneAssetExtents.x;

                return maxExtent;
            }

            Vector3 GetSceneAssetPivot(Bounds assetBounds, SceneAssetPivot assetPivot)
            {
                Vector3 pivot = Vector3.zero;

                switch (assetPivot)
                {
                    case SceneAssetPivot.BottomCenter:

                        pivot = assetBounds.center;
                        float y = assetBounds.extents.y / 100;
                        y /= 2;
                        pivot.y = y - y;

                        break;

                    case SceneAssetPivot.MiddleCenter:

                        pivot = assetBounds.center;

                        break;
                }

                return pivot;
            }

            Vector3 GetSceneAssetCenter(List<MeshRenderer> renderers)
            {
                Bounds bounds = new Bounds();

                foreach (var renderer in renderers)
                {
                    bounds.Encapsulate(renderer.bounds);
                }

                return bounds.center;
            }

            Vector3 GetSceneAssetBoundsSize(GameObject sceneAssetObhect, List<MeshRenderer> renderers)
            {
                Bounds bounds = new Bounds();

                foreach (var renderer in renderers)
                {
                    bounds.Encapsulate(renderer.bounds);
                }

                return bounds.size;
            }

            public void ResetPose()
            {
                if (value)
                {
                    if (useLocalPose)
                    {
                        value.localPosition = assetLocalPose.position;
                        value.localRotation = assetLocalPose.rotation;
                        value.localScale = assetLocalPose.scale;
                    }
                    else
                    {
                        value.position = assetPose.position;
                        value.rotation = assetPose.rotation;
                        value.localScale = assetPose.scale;
                    }

                }
                else
                    Debug.LogWarning("--> Container Missing");
            }

            public void Remove(GameObject sceneAsset)
            {
                if (value)
                {

                }
                else
                    Debug.LogWarning("--> Container Missing");
            }

            public void Clear(bool resetLocalScale)
            {
                if (recycledContentContainer != null)
                {
                    if (value.childCount <= 0)
                        return;

                    for (int i = 0; i < value.childCount; i++)
                    {
                        if (value.GetChild(i).gameObject.GetComponent<SceneAssetModelHandler>())
                        {
                            SceneAssetModelHandler assetHandler = value.GetChild(i).gameObject.GetComponent<SceneAssetModelHandler>();

                            if (assetHandler != null)
                            {
                                Debug.Log("-- RG_Unity - Success : Asset Scale Reseted...........................");
                                assetHandler.Reset(false, resetLocalScale, recycledContentContainer);
                            }
                            else
                                Debug.LogWarning("--> Clear Content Failed To Reset Scale : Asset Handler Is Null.");
                        }
                    }
                }
                else
                    Debug.LogWarning("--> Clear Content Failed : Recycled Content Container Is Not Assigned In The Inspector Panel.");
            }

            public void Show(bool resetPose = false, bool isLocal = false)
            {
                if (value)
                {
                    if (resetPose)
                    {
                        if (!isLocal)
                        {
                            value.position = Vector3.zero;
                            value.rotation = Quaternion.identity;
                        }
                        else
                        {
                            value.localPosition = Vector3.zero;
                            value.localRotation = Quaternion.identity;
                        }

                        value.localScale = Vector3.one;
                    }

                    value.gameObject.SetActive(true);
                }
                else
                    Debug.LogWarning("--> Container Is Null.");
            }

            public void Hide(bool resetPose = false, bool isLocal = false)
            {
                if (value)
                {
                    value.gameObject.SetActive(false);

                    if (resetPose)
                    {
                        if (!isLocal)
                        {
                            value.position = Vector3.zero;
                            value.rotation = Quaternion.identity;
                        }
                        else
                        {
                            value.localPosition = Vector3.zero;
                            value.localRotation = Quaternion.identity;
                        }

                        value.localScale = Vector3.one;
                    }

                }
                else
                    Debug.LogWarning("--> Container Is Null.");
            }

            public void ScaleContent(float value, bool scaleContent)
            {
                if (this.value)
                {
                    if (scaleContent)
                    {
                        Transform asset = this.value.GetChild(0);
                        Vector3 scale = asset.localScale;
                        scale /= value;
                        asset.localScale = scale;
                    }
                }
                else
                    Debug.LogWarning("--> Container Is Null.");
            }

            #endregion
        }

        [Serializable]
        public struct SceneContainerData
        {
            public string name;

            [Space(5)]
            public Transform value;

            [Space(5)]
            public SceneAssetModeType assetModeType;
        }

        public struct HSVColorData
        {
            public float hue;
            public float saturation;
            public float value;
        }

        [Serializable]
        public class RGBColorValue
        {
            #region Components

            public string name;


            [Space(5)]
            public Color color;

            [Space(5)]
            public ColorValueType valueType;

            #endregion

            #region Main

            public RGBColorValue()
            {

            }

            public RGBColorValue(Color Color, ColorValueType valueType)
            {
                this.color = Color;
                this.valueType = valueType;
            }

            public void SetColor(Color color) => this.color = color;

            public Color GetColor()
            {
                return color;
            }

            public void SetValueType(ColorValueType valueType) => this.valueType = valueType;

            public ColorValueType GetValueType()
            {
                return valueType;
            }

            #endregion
        }

        [Serializable]
        public struct LoadingItemData
        {
            public string name;

            [Space(5)]
            public GameObject loadingWidgetsContainer;

            [Space(5)]
            public LoadingItemType type;

            [Space(5)]
            public bool isShowing;
        }

        [Serializable]
        public struct SceneObject
        {
            public AssetInfoHandler info;
            public GameObject value;
        }

        [Serializable]
        public struct ScreenTogglableWidget<T>
        {
            #region Components

            public string name;

            [Space(5)]
            public T value;

            [Space(5)]
            public TogglableWidgetType widgetType;

            #endregion

            #region Main

            public void Show()
            {
                GameObject value = this.value as GameObject;
                value.SetActive(true);
            }

            public void Hide()
            {
                GameObject value = this.value as GameObject;
                value.SetActive(false);
            }

            public void Interactable(bool state)
            {
                Button value = this.value as Button;
                value.interactable = state;
            }

            #endregion
        }

        [Serializable]
        public struct SceneAssetLibrary
        {
            #region Components

            public Dictionary<string, SceneAsset> sceneAssetsDictionary;
            public Dictionary<string, Sprite> imageAssetsDictionary;

            #endregion

            #region Main

            public void InitializeLibrary()
            {
                sceneAssetsDictionary = new Dictionary<string, SceneAsset>();
                imageAssetsDictionary = new Dictionary<string, Sprite>();
            }

            public void AddSceneAssetObjectToLibrary(string assetName, SceneAsset sceneAsset)
            {
                if (!sceneAssetsDictionary.ContainsKey(assetName))
                    sceneAssetsDictionary.Add(assetName, sceneAsset);
                else
                    Debug.LogWarning($"--> Scene Asset : {assetName} Already Exist In The Library.");
            }

            public void AddSceneAssetObjectToLibrary(SceneAsset sceneAsset)
            {
                if (!sceneAssetsDictionary.ContainsValue(sceneAsset))
                    sceneAssetsDictionary.Add(sceneAsset.name, sceneAsset);
                else
                    Debug.LogWarning($"--> Scene Asset : {sceneAsset.name} Already Exist In The Library.");
            }

            public void RemoveSceneAssetFromLibrary(string assetName)
            {
                if (sceneAssetsDictionary.ContainsKey(assetName))
                    sceneAssetsDictionary.Remove(assetName);
                else
                    Debug.LogWarning($"--> Scene Asset : {assetName} Doesn't Exist In The Library.");
            }

            public void AddImageAsset(Sprite imageAsset, string assetPath)
            {
                if (!imageAssetsDictionary.ContainsKey(assetPath))
                    imageAssetsDictionary.Add(assetPath, imageAsset);
                else
                    Debug.LogWarning($"--> Image Asset From Path : {assetPath} Already Exist In The Library.");
            }


            public void RemoveImageeAssetFromLibrary(string assetPath)
            {
                if (imageAssetsDictionary.ContainsKey(assetPath))
                    imageAssetsDictionary.Remove(assetPath);
                else
                    Debug.LogWarning($"--> Image Asset From Path : {assetPath} Doesn't Exist In The Library.");
            }

            public Sprite GetImageAsset(string assetPath)
            {
                if (imageAssetsDictionary.ContainsKey(assetPath))
                {
                    Sprite asset;

                    imageAssetsDictionary.TryGetValue(assetPath, out asset);

                    return asset;
                }
                else
                    Debug.LogWarning($"--> Image Asset From Path : {assetPath} Doesn't Exists In The Library.");

                return null;
            }

            public Dictionary<string, Sprite> GetAllImageAssets()
            {
                return imageAssetsDictionary;
            }

            public bool HasAssets()
            {
                return sceneAssetsDictionary.Count > 0;
            }

            public Dictionary<string, SceneAsset> GetAllSceneAssets()
            {
                return sceneAssetsDictionary;
            }

            public SceneAsset GetAsset(string assetName)
            {
                if (sceneAssetsDictionary.ContainsKey(assetName))
                {
                    SceneAsset asset;

                    sceneAssetsDictionary.TryGetValue(assetName, out asset);

                    return asset;
                }
                else
                    Debug.LogWarning($"--> Scene Asset : {assetName} Doesn't Exists In The Library.");

                return null;
            }

            public SceneAsset GetAsset(SceneAsset asset)
            {
                if (sceneAssetsDictionary.ContainsKey(asset.name))
                {
                    SceneAsset sceneAsset;

                    sceneAssetsDictionary.TryGetValue(asset.name, out sceneAsset);

                    return sceneAsset;
                }
                else
                    Debug.LogWarning($"--> Scene Asset : {asset.name} Doesn't Exists In The Library.");

                return null;
            }

            public bool SceneAssetExists(string assetName)
            {
                if (sceneAssetsDictionary.ContainsKey(assetName))
                    return true;

                return false;
            }

            public bool ImageAssetExists(string assetPath)
            {
                if (imageAssetsDictionary.ContainsKey(assetPath))
                    return true;

                return false;
            }

            public bool AssetExists(SceneAsset asset)
            {
                if (sceneAssetsDictionary.ContainsValue(asset))
                    return true;

                return false;
            }

            public GameObject GetSceneAssetModel(string assetName)
            {
                if (sceneAssetsDictionary.ContainsKey(assetName))
                {
                    SceneAsset asset;

                    sceneAssetsDictionary.TryGetValue(assetName, out asset);

                    if (asset.sceneObject.value)
                        return asset.sceneObject.value;

                    if (asset.modelAsset)
                        return asset.modelAsset;


                    return asset.sceneObject.value;
                }
                else
                    Debug.LogWarning($"--> Scene Asset : {assetName} Doesn't Exists In The Library.");

                return null;
            }

            public List<GameObject> GetSceneAssetModels()
            {
                if (sceneAssetsDictionary.Count > 0)
                {
                    List<GameObject> sceneAssetModelList = new List<GameObject>();

                    foreach (var assetModel in sceneAssetModelList)
                    {
                        sceneAssetModelList.Add(assetModel);
                    }

                    if (sceneAssetModelList.Count > 0)
                        return sceneAssetModelList;
                    else
                        Debug.LogWarning("--> Failed To Get Scene Asset Models.");
                }

                Debug.LogWarning("--> Scene Assets Library Is Empty.");
                return null;
            }

            public void ClearLibrary()
            {
                sceneAssetsDictionary.Clear();
            }

            #endregion
        }

        [Serializable]
        public struct SceneAssetWidget
        {
            #region Components

            public string name;

            [Space(5)]
            public GameObject value;

            [Space(5)]
            public SceneAssetCategoryType categoryType;

            [Space(5)]
            public string creationDateTime;


            #endregion

            #region Main

            public void SetVisibilityState(bool isVisible)
            {
                if (value)
                    value.SetActive(isVisible);
                else
                    Debug.LogWarning($"--> Value For : {value} Missing / Not Assigned.");
            }

            public void SetAssetListIndex(int index)
            {
                if (value)
                    value.transform.SetSiblingIndex(index);
                else
                    Debug.LogWarning($"--> Value For : {value} Missing / Not Assigned.");
            }

            public DateTime GetModifiedDateTime()
            {
                DateTime dateTime = DateTime.Parse(creationDateTime);

                return dateTime;
            }

            #endregion
        }

        #region Callback Class

        public class Callback
        {
            #region Components

            public LogInfoType resultsCode;
            public string results;

            public UnityEngine.Object classInfo;

            #endregion

            #region Main

            public void SetClassInfo<T>(T classInfo) where T : UnityEngine.Object => this.classInfo = classInfo;

            public T GetClassInfo<T>() where T : UnityEngine.Object
            {
                return classInfo as T;
            }

            public bool Success()
            {
                return Helpers.IsSuccessCode(resultsCode);
            }

            #endregion
        }

        public class CallbackData<T> : Callback
        {
            #region Components
           
            public T data;

            #endregion
        }

        public class CallbackDatas<T> : Callback
        {
            #region Components

            public List<T> data;

            #endregion
        }

        public class CallbackTuple<T, U> : Callback
        {
            #region Components

            public List<T> tuple_A;
            public List<U> tuple_B;

            #endregion
        }

        public class CallbackTuple<T, U, V> : Callback
        {
            #region Components

            public List<T> tuple_A;
            public List<U> tuple_B;
            public List<V> tuple_C;

            #endregion
        }

        public class CallbackSizeDataTuple<T, U, V> : Callback
        {
            #region Components

            public List<T> tuple_A;
            public List<U> tuple_B;

            public V tuple_C;

            #endregion
        }

        public class CallbackSizeDataTuple<T> : Callback
        {
            #region Components

            public List<T> tuple_A;
            public List<T> tuple_B;

            public int size;

            #endregion
        }

        public class CallbackSizeDataTuple<T, U> : Callback
        {
            #region Components

            public List<T> tuple_A;
            public List<U> tuple_B;
            public int size;

            #endregion
        }

        public class CallbackDataArray<T> : Callback
        {
            #region Components

            public T[] data;

            #endregion
        }

        #endregion

        [Serializable]
        public class FocusedSelectionInfoData<T> where T : DataPackets
        {
            public string name;
            public UIScreenWidget<T> selection;

            public InputUIState state;
            public bool showSelection;
            public bool showTint;

            public FocusedSelectionType selectionInfoType;
        }

        [Serializable]
        public class FocusedSelectionStateInfo
        {
            public string name;

            [Space(5)]
            public FocusedSelectionType selectionInfoType;

            [Space(5)]
            public InputUIState state;

            [Space(5)]
            public bool showSelection;

            [Space(5)]
            public bool showTint;
        }

        [Serializable]
        public class FocusedSelectionInfo<T>
        {
            #region Components

            public string name;
            public FocusedSelectionType selectionInfoType;

            #endregion

            #region Main

            public FocusedSelectionInfo()
            {

            }

            public FocusedSelectionInfo(string name, FocusedSelectionType selectionInfoType)
            {
                this.name = name;
                this.selectionInfoType = selectionInfoType;
            }

            #endregion
        }

        [Serializable]
        public class FocusedSelectionData
        {
            #region Components

            [Space(5)]
            public List<FocusedSelectionInfo<SceneDataPackets>> selections;

            [Space(5)]
            public FocusedSelectionType selectionType;

            [Space(5)]
            public bool isActiveSelection;

            #endregion

            #region Main

            public FocusedSelectionData()
            {

            }

            public FocusedSelectionData(List<FocusedSelectionInfo<SceneDataPackets>> selections, FocusedSelectionType selectionType, bool isActiveSelection)
            {
                this.selections = selections;
                this.selectionType = selectionType;
                this.isActiveSelection = isActiveSelection;
            }

            public void Clear()
            {
                selections = new List<FocusedSelectionInfo<SceneDataPackets>>();

                selectionType = FocusedSelectionType.Default;
                isActiveSelection = false;
            }

            #endregion
        }

        [Serializable]
        public struct AssetExportData
        {
            public string name;
            public GameObject value;
            public ExportExtensionType exportExtension;
            public string exportDirectory;
        }

        [Serializable]
        public struct InfoDisplayerField
        {
            public string name;

            [Space(5)]
            public TMP_Text title;

            [Space(5)]
            public InfoDisplayerFieldType type;
        }

        [Serializable]
        public struct ScreenBlurObjectContainer
        {
            #region Components

            public string name;

            [Space(5)]
            public Transform value;

            [Space(5)]
            public ScreenBlurContainerLayerType containerLayerType;

            #endregion

            #region Main

            public Transform GetValueAssigned()
            {
                return value;
            }

            public bool HasValueAssigned()
            {
                return value != null;
            }

            #endregion
        }

        [Serializable]
        public class SceneEventCamera
        {
            public string name;

            [Space(5)]
            public Camera value;

            [Space(5)]
            public SceneEventCameraType eventCameraType;

            SceneAssetPose defaultCameraPose;

            public void Init()
            {
                if (value != null)
                {
                    SceneAssetPose assetPose = new SceneAssetPose
                    {
                        position = value.transform.position,
                        rotation = value.transform.rotation,
                        scale = value.transform.localScale
                    };

                    SetDefaultCameraPose(assetPose);
                }
                else
                    Debug.LogWarning("--> RG_Unity - Init Failed : Scene Event Camera Value Is Missing / Null.");
            }

            public Transform GetCameraTransform()
            {
                return value?.transform;
            }

            public void EnableCamera()
            {
                value.enabled = true;
            }

            public void DisableCamera()
            {
                value.enabled = false;
            }

            public void SetDefaultCameraPose(SceneAssetPose assetPose)
            {
                defaultCameraPose = assetPose;
            }

            public SceneAssetPose GetDefaultCameraPose()
            {
                return defaultCameraPose;
            }
        }

        [Serializable]
        public class AssetInfoWidgetContainer
        {
            #region Components

            [Space(5)]
            public GameObject value;

            #endregion

            #region Main

            public GameObject GetContainer()
            {
                return value;
            }

            public bool GetActive()
            {
                return value && value.activeSelf && value.activeInHierarchy;
            }

            public bool HasContainer()
            {
                return value;
            }

            public async void Show(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if(HasContainer())
                {
                    GetContainer().SetActive(true);

                    await Helpers.GetWaitUntilAsync(GetActive());

                    if(!GetActive())
                    {
                        callbackResults.results = "Container Failed To Show -Please Check Here.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                    else
                    {
                        callbackResults.results = "Container Showing.";
                        callbackResults.resultsCode = Helpers.SuccessCode;

                    }
                }
                else
                {
                    callbackResults.results = "Container Value Missing / Not Found.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public async void Hide(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (HasContainer())
                {
                    GetContainer().SetActive(false);

                    await Helpers.GetWaitUntilAsync(!GetActive());

                    if (GetActive())
                    {
                        callbackResults.results = "Container Failed To Hide -Please Check Here.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
         
                    }
                    else
                    {
                        callbackResults.results = "Container Hidden.";
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                }
                else
                {
                    callbackResults.results = "Container Value Missing / Not Found.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            #endregion
        }

        [Serializable]
        public class AssetInfoDisplayer
        {
            public List<InfoDisplayerField> infoWidgetFieldList;

            [Space(5)]
            [Header("Deprecated")]
            public GameObject widgetsContainer; // Deprecated

            [Space(5)]
            public AssetInfoWidgetContainer contentContainer = new AssetInfoWidgetContainer();

            [Space(5)]
            public bool showStats;

            bool show = false;

            public void SetAssetInfo(AssetInfoHandler info, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (infoWidgetFieldList != null)
                {
                    if (info.fields != null)
                    {
                        foreach (var field in info.fields)
                            foreach (var widget in infoWidgetFieldList)
                                if (field.type != InfoDisplayerFieldType.None && field.type.Equals(widget.type))
                                {
                                    if (field.type == InfoDisplayerFieldType.Title)
                                        widget.title.text = field.name;
                                    else
                                        widget.title.text = field.value.ToString();
                                }

                        show = true;
                    }
                    else
                        show = false;

                    if (show)
                    {
                        ShowInfo(showInfoCallback => 
                        {
                            callbackResults.results = showInfoCallback.results;
                            callbackResults.resultsCode = showInfoCallback.resultsCode;
                        });
                    }
                    else
                    {
                        HideInfo(hiddingContainerCallback => 
                        {
                            callbackResults.results = "Couldn't Show Info. Info Field Null / Empty.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        });
                    }
                }
                else
                {
                    callbackResults.results = "Couldn't Show Info. Info Widget Field List Is Null / Empty.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void ShowInfo(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                GetWidgetsContainer(containerFoundCallback => 
                {
                    if(Helpers.IsSuccessCode(containerFoundCallback.resultsCode))
                    {
                        containerFoundCallback.data.Show(showContainerCallback => 
                        {
                            callbackResults.results = showContainerCallback.results;
                            callbackResults.resultsCode = showContainerCallback.resultsCode;
                        });
                    }

                    callbackResults.results = containerFoundCallback.results;
                    callbackResults.resultsCode = containerFoundCallback.resultsCode;

                });

                callback?.Invoke(callbackResults);
            }

            public void HideInfo(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                GetWidgetsContainer(containerFoundCallback =>
                {
                    if (Helpers.IsSuccessCode(containerFoundCallback.resultsCode))
                    {
                        containerFoundCallback.data.Hide(showContainerCallback =>
                        {
                            callbackResults.results = showContainerCallback.results;
                            callbackResults.resultsCode = showContainerCallback.resultsCode;
                        });
                    }

                    callbackResults.results = containerFoundCallback.results;
                    callbackResults.resultsCode = containerFoundCallback.resultsCode;

                });

                callback?.Invoke(callbackResults);
            }

            public void GetWidgetsContainer(Action<CallbackData<AssetInfoWidgetContainer>> callback)
            {
                CallbackData<AssetInfoWidgetContainer> callbackResults = new CallbackData<AssetInfoWidgetContainer>();

                if(contentContainer.HasContainer())
                {
                    callbackResults.results = "Widgets Info Container Loaded.";
                    callbackResults.data = contentContainer;
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "Widgets Info Container Missing / Not Found - Please Check Here.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void ResetAssetInfo(AssetInfoHandler info)
            {
                if (infoWidgetFieldList == null)
                {
                    Debug.LogWarning("--> Info Widgets Null.");
                    return;
                }

                if (info.fields != null)
                {
                    foreach (var field in info.fields)
                    {
                        foreach (var widget in infoWidgetFieldList)
                        {
                            if (field.type != InfoDisplayerFieldType.None && field.type.Equals(widget.type))
                            {
                                if (field.type == InfoDisplayerFieldType.Title)
                                    widget.title.text = SceneAssetsManager.Instance.GetDefaultAssetName();
                                else
                                {
                                    widget.title.text = 0.ToString();
                                }
                            }
                        }
                    }

                    show = true;
                }
                else
                    show = false;

                if (show)
                    widgetsContainer.SetActive(showStats);
            }
        }

        #region Base Classes
        [RequireComponent(typeof(LayoutElement))]
        [RequireComponent(typeof(Button))]

        public abstract class UIScreenWidget<T> : AppMonoBaseClass, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler where T : DataPackets
        {
            #region Components

            [Space(5)]
            [SerializeField]
            List<UIButton<T>> actionButtonList = new List<UIButton<T>>();

            [Space(5)]
            [SerializeField]
            List<UIInputField<T>> actionInputFieldList = new List<UIInputField<T>>();

            [Space(5)]
            [SerializeField]
            List<UIText<T>> textDisplayerList = new List<UIText<T>>();

            [Space(5)]
            [SerializeField]
            List<UIImageDisplayer> imageDisplayerList = new List<UIImageDisplayer>();

            [Space(5)]
            [SerializeField]
            protected SelectableWidgetType selectableType;

            [Space(5)]
            [SerializeField]
            protected SelectableAssetType selectableAssetType;

            [Space(5)]
            [SerializeField]
            protected UISelectionFrame selectionFrame = new UISelectionFrame();

            [Space(5)]
            [SerializeField]
            protected float pressAndHoldDuration = 0.25f;

            [Space(5)]
            [SerializeField]
            protected float selectionStateButtonScale = 0.9f;

            [Space(5)]
            [SerializeField]
            RectTransform folderContentContainer = null;

            [Space(5)]
            [SerializeField]
            protected T dataPackets;

            [SerializeField]
            List<UIScreenWidget<SceneDataPackets>> containerFolderWidgetsReferenceList = new List<UIScreenWidget<SceneDataPackets>>();

            [Space(5)]
            [SerializeField]
            protected float dragActiveDistance = 25.0f;

            [Space(5)]
            [SerializeField]
            protected bool scaleWidgetOnDrag = false;

            [Space(5)]
            [SerializeField]
            protected bool showNotifications = false;

            [Space(5)]
            [SerializeField]
            protected Notification notification;

            protected FocusedSelectionInfoData<SceneDataPackets> focusedSelectionInfoData = new AppData.FocusedSelectionInfoData<SceneDataPackets>();

            protected DynamicWidgetsContainer container;

            protected Button buttonComponent = null;
            protected LayoutElement layout = null;

            [SerializeField]
            protected SceneAsset assetData;

            protected DefaultUIWidgetActionState defaultUIWidgetAction;

            #region UI References

            protected Image thumbnailDisplayerRef;
            protected TMP_Text titleDisplayerRef;
            protected TMP_Text descriptionDisplayerRef;
            protected TMP_Text dateTimeDisplayerRef;

            #endregion

            Vector3 selectionButtonScaleVect;
            Vector2 dragStartPosition;
            Vector2 dragOffSet;
            Vector2 dragDistance;
            protected RectTransform widgetRect;

            protected ScreenUIData widgetParentScreen;
            protected ScreenUIManager screenManager;

            protected Folder folder;

            private RectTransform parent;

            protected UIScreenWidget<SceneDataPackets> widgetComponent = null;
            protected UIScreenWidget<SceneDataPackets> hoveredWidget = null;

            bool isFingerDown = false;

            [SerializeField]
            bool isSelected = false;

            bool isDragging = false;
            bool isScrolling = false;

            [SerializeField]
            bool canDrag = false;

            bool isHovered = false;

            float currentPressDuration = 0;

            [SerializeField]
            int contentIndex = 0;

            PointerEventData currentEventData;

            #endregion

            #region Unity Callback

            void OnEnable() => OnActionEventSubscription(true);
            void OnDisable() => OnActionEventSubscription(false);
            void Update() => OnSelectionUpdate();

            #endregion

            #region Main

            protected void Init(Action<Callback> callback = null)
            {
                try
                {
                    Callback callbackResults = new Callback();

                    widgetComponent = GetComponent<UIScreenWidget<SceneDataPackets>>();
                    widgetRect = GetComponent<RectTransform>();
                    layout = GetComponent<LayoutElement>();
                    buttonComponent = GetComponent<Button>();
                    parent = GetComponentInChildren<RectTransform>();
                    Deselected();
                    contentIndex = transform.GetSiblingIndex();

                    selectionButtonScaleVect.x = selectionStateButtonScale;
                    selectionButtonScaleVect.y = selectionStateButtonScale;
                    selectionButtonScaleVect.z = selectionStateButtonScale;

                    container = GetComponentInParent<DynamicWidgetsContainer>();

                    //dragPosition = widgetRect.anchoredPosition;

                    if (actionButtonList == null)
                    {
                        callbackResults.results = "Action Buttons Required.";
                        callbackResults.resultsCode = Helpers.ErrorCode;

                        callback.Invoke(callbackResults);
                        return;
                    }

                    if (actionButtonList.Count > 0)
                    {
                        //foreach (var button in actionButtonList)
                        //{
                        //    if (button.value != null)
                        //        button.value.onClick.AddListener(() => OnActionButtonInputs(button));
                        //    else
                        //    {
                        //        callbackResults.results = "--> Action Button Value Required.";
                        //        callbackResults.success = false;
                        //        break;
                        //    }
                        //}

                        callbackResults.results = "--> Initialized Successfully.";
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = "--> Action Buttons Required.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }

                    callback.Invoke(callbackResults);
                }
                catch (Exception e)
                {
                    Debug.LogError($"--> RG_Unity - Init Failed : UIScreenWidget Initialization Failed With Results : {e}");
                    throw e;
                }
            }

            void OnActionEventSubscription(bool subscribed)
            {
                if (subscribed)
                    ActionEvents._OnScreenUIRefreshed += ActionEvents__OnScreenUIRefreshed;
                else
                    ActionEvents._OnScreenUIRefreshed -= ActionEvents__OnScreenUIRefreshed;
            }

            private void ActionEvents__OnScreenUIRefreshed()
            {
                OnScreenUIRefreshed();
            }

            public void SetFolderData(Folder folder)
            {

                this.folder = folder;
                this.folder.name = folder.name.Replace("_FolderData", "");
                OnSetFolderData(folder);
            }

            public Folder GetFolderData()
            {
                return folder;
            }

            public void SetContentSiblingIndex(int contentIndex)
            {
                this.contentIndex = contentIndex;
            }

            public int GetContentSiblingIndex()
            {
                return contentIndex;
            }

            public bool GetActive()
            {
                bool isActive = false;

                if (this)
                {
                    if (ScreenUIManager.Instance != null)
                        if (ScreenUIManager.Instance.GetCurrentScreenData().value)
                            if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == dataPackets.screenType)
                                isActive = gameObject.activeInHierarchy && gameObject.activeSelf && GetWidgetRect() != null;
                }

                return isActive;
            }

            public bool IsSelected()
            {
                bool selected = false;

                if (GetActive())
                    selected = isSelected;

                return selected;
            }

            public void SetSelected(bool isSelected) => this.isSelected = isSelected;

            public bool HasContainer()
            {
                return GetWidgetContainer() != null;
            }

            public void SetFileData() => OnSetFileData(assetData);

            public void SetFileData(SceneAsset assetData) => OnSetFileData(assetData);
            protected abstract void OnActionButtonInputs(UIButton<T> actionButton);
            protected abstract void OnSetFolderData(Folder folder);
            protected abstract void OnSetFileData(SceneAsset assetData);
            protected abstract void OnScreenUIRefreshed();

            public void SetSceneAssetData(SceneAsset assetData)
            {
                this.assetData = assetData;
            }

            public SceneAsset GetSceneAssetData()
            {
                return assetData;
            }

            public UIScreenWidget<SceneDataPackets> GetWidgetComponent()
            {
                return widgetComponent;
            }

            public void SetUIImageDisplayerValue(Sprite image, UIImageDisplayerType displayerType)
            {
                if (imageDisplayerList.Count > 0)
                {
                    foreach (var displayer in imageDisplayerList)
                        if (displayer.imageDisplayerType == displayerType)
                            displayer.value.sprite = image;
                }
                else
                    Debug.LogWarning("--> SetUIImageDisplayerValue Failed : imageDisplayerList Is Null / Empty.");
            }

            public GameObject GetSceneAssetObject()
            {
                return this.gameObject;
            }

            public void SetWidgetAssetData(SceneAsset asset)
            {
                if (thumbnailDisplayerRef != null)
                    Helpers.ShowImage(asset, thumbnailDisplayerRef);
                else
                    Debug.LogWarning("-------> SetWidgetAssetData Failed : thumbnailDisplayerRef Is Null.");

                if (titleDisplayerRef != null && !string.IsNullOrEmpty(asset.name))
                    titleDisplayerRef.text = asset.name;

                if (descriptionDisplayerRef != null && !string.IsNullOrEmpty(asset.description))
                    descriptionDisplayerRef.text = asset.description;

                if (dateTimeDisplayerRef != null && !string.IsNullOrEmpty(asset.creationDateTime))
                    dateTimeDisplayerRef.text = asset.creationDateTime;
            }

            public LayoutElement GetWidgetLayoutElement()
            {
                return layout;
            }

            public RectTransform GetWidgetRect()
            {
                if (this && widgetRect == null)
                    widgetRect = this?.GetComponent<RectTransform>();

                return widgetRect;
            }

            public Vector2 GetWidgetSizeDelta()
            {
                return GetWidgetRect().sizeDelta;
            }

            public Vector3 GetWidgetLocalScale()
            {
                return GetWidgetRect().localScale;
            }

            public Vector2 GetWidgetLocalPosition()
            {
                return GetWidgetRect().localPosition;
            }

            public Vector2 GetWidgetAnchoredPosition()
            {
                return widgetRect.anchoredPosition;
            }

            public Vector2 GetWidgetPosition()
            {
                return GetWidgetRect().position;
            }

            public void SetWidgetParentScreen(ScreenUIData screen)
            {
                widgetParentScreen = screen;
            }

            public ScreenUIData GetWidgetParentScreen()
            {
                return widgetParentScreen;
            }

            public void Show() => gameObject.SetActive(true);

            public void Hide() => gameObject.SetActive(false);

            public void SetDefaultUIWidgetActionState(DefaultUIWidgetActionState actionState)
            {
                defaultUIWidgetAction = actionState;

                switch (defaultUIWidgetAction)
                {
                    case DefaultUIWidgetActionState.Default:

                        SetUIImageDisplayerValue(SceneAssetsManager.Instance.GetImageFromLibrary(UIImageType.Null_TransparentIcon).value, UIImageDisplayerType.PinnedIcon);

                        break;

                    case DefaultUIWidgetActionState.Hidden:

                        Debug.LogError("==> Asset Hidden.");


                        SetUIImageDisplayerValue(SceneAssetsManager.Instance.GetImageFromLibrary(UIImageType.Null_TransparentIcon).value, UIImageDisplayerType.PinnedIcon);

                        break;

                    case DefaultUIWidgetActionState.Pinned:

                        SetUIImageDisplayerValue(SceneAssetsManager.Instance.GetImageFromLibrary(UIImageType.PinEnabledIcon).value, UIImageDisplayerType.PinnedIcon);

                        break;
                }

                //if (SelectableManager.Instance != null)
                //{
                //    if(SelectableManager.Instance.)
                //}
                //else
                //    LogWarning("Selectable Manager Instance Is Not Yet Initialized.", this, "SetDefaultUIWidgetActionState(DefaultUIWidgetActionState actionState)");
            }

            public DefaultUIWidgetActionState GetDefaultUIWidgetActionState()
            {
                return defaultUIWidgetAction;
            }

            protected void SetActionButtonState(InputActionButtonType buttonType, UIStateType state)
            {
                Debug.LogError($"--> Button : {buttonType} State - : {state}");
            }

            protected void SetActionButtonState(UIButton<T> button, InputUIState state)
            {
                foreach (var actionButton in actionButtonList)
                    if (actionButton.value)
                    {
                        if (actionButton == button)
                            actionButton.SetUIInputState(button, state);
                        else
                            actionButton.SetUIInputState(actionButton, InputUIState.Deselect);
                    }
                    else
                        Debug.LogError($"--> Show Action Button Failed : {actionButton.actionType} Not Found");
            }

            public List<UIButton<T>> GetActionInputUIButtonList()
            {
                return actionButtonList;
            }

            public SelectableWidgetType GetSelectableType()
            {
                return selectableType;
            }

            public SelectableAssetType GetSelectableAssetType()
            {
                return selectableAssetType;
            }

            public void SetContentContainerPositionIndex(int index) => transform.SetSiblingIndex(index);

            public RectTransform GetFolderContentContainer()
            {
                return folderContentContainer;
            }

            protected void SetUITextDisplayerValue(string value, ScreenTextType textType)
            {
                if (textDisplayerList.Count > 0)
                {
                    foreach (var displayer in textDisplayerList)
                    {
                        if (displayer.textType == textType)
                        {
                            if (displayer.value)
                            {
                                displayer.SetScreenUITextValue(value);
                                break;
                            }
                            else
                                Debug.LogWarning("--> Failed : textDisplayer Value Is Null / Empty.");
                        }
                    }
                }
                else
                    Debug.LogWarning("--> SetUITextDisplayerValue Failed : textDisplayerList Is Null / Empty.");
            }

            protected void SetUIImageDisplayerValue(UIImageDisplayerType displayerType, UIImageType imageType)
            {
                if (imageDisplayerList.Count > 0)
                {
                    foreach (var displayer in imageDisplayerList)
                    {
                        if (displayer.imageDisplayerType == displayerType)
                        {
                            if (displayer.value)
                            {
                                displayer.value.sprite = SceneAssetsManager.Instance.GetImageFromLibrary(imageType).value;
                                break;
                            }
                            else
                                Debug.LogWarning("--> Failed : textDisplayer Value Is Null / Empty.");
                        }
                    }
                }
                else
                    Debug.LogWarning("--> SetUIImageDisplayerValue Failed : imageDisplayerList Is Null / Empty.");
            }

            void OnSelectionUpdate()
            {
                if (GetActive())
                {
                    if (SelectableManager.Instance != null)
                    {
                        if (!isFingerDown || isDragging)
                            return;

                        if (currentPressDuration < pressAndHoldDuration)
                        {
                            currentPressDuration += 1 * Time.deltaTime;

                            if (currentPressDuration > 0.15f && currentPressDuration < pressAndHoldDuration)
                                canDrag = true;
                        }

                        if (currentPressDuration >= pressAndHoldDuration)
                        {
                            SetSelected(true);

                            if (!SelectableManager.Instance.SmoothTransitionToSelection)
                                SelectableManager.Instance.SmoothTransitionToSelection = true;

                            var widgetData = this as UIScreenWidget<SceneDataPackets>;
                            var dataPacket = dataPackets as SceneDataPackets;

                            SelectableManager.Instance.OnClearFocusedSelectionsInfo();
                            OnSelect(true);
                        }
                    }
                    else
                        LogWarning("Selectable Manager Instance Is Not Yet Initialized.", this, () => OnSelectionUpdate());
                }
            }

            bool OnPressAndHold()
            {
                return (currentPressDuration >= pressAndHoldDuration);
            }

            void OnReset()
            {
                isDragging = false;
                canDrag = false;
                currentPressDuration = 0.0f;

                if (layout != null)
                {
                    if (layout.ignoreLayout)
                        layout.ignoreLayout = isDragging;
                }
                else
                    LogWarning("Reset Failed - Layout Element Not Found.", this, () => OnReset());

                SetFingerDown(false);
            }

            public void OnReset(bool deselect)
            {
                if (deselect)
                    OnDeselect();

                OnReset();
            }

            void SetFingerDown(bool isFingerDown)
            {
                if (this != null)
                {
                    if (GetActive() == false)
                        return;

                    if (GetWidgetContainer())
                    {
                        this.isFingerDown = isFingerDown;
                        GetWidgetContainer().SetFingerDown(isFingerDown);
                    }
                    else
                        LogWarning("Set Finger Down Failed - Container Missing", this, () => SetFingerDown(isFingerDown));
                }
            }

            bool IsFingerDown()
            {
                return isFingerDown;
            }

            public DynamicWidgetsContainer GetWidgetContainer()
            {
                if (container == null)
                    container = GetComponentInParent<DynamicWidgetsContainer>();

                return container;
            }

            protected void OnSetActionButtonEvent(InputActionButtonType actionType, ActionEvents.ButtonAction<T> buttonAction = null, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (GetActive())
                {
                    if (actionButtonList.Count > 0)
                    {
                        UIButton<T> button = actionButtonList.Find(button => button.actionType == actionType);

                        if (button != null)
                        {
                            if (button.value)
                            {
                                button.SetButtonActionEvent(buttonAction);

                                callbackResults.results = $"Set Action Button Event Success - Action Button : {button.name} Of Type : {actionType} Found.";
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = $"Set Action Button Event Failed - Action Button : {button.name} Of Type : {actionType} Found With Missing Value - For Screen Widget : {name} With : {actionButtonList?.Count} Buttons Assigned.";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = $"Set Action Button Event Failed - Action Button Of Type : {actionType} Not Found For Screen Widget : {name} With : {actionButtonList?.Count} Buttons Assigned.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"Set Action Button Event Failed For UI Screen Widget : {name} Of Selectable Type : {selectableType} - Action Buttons For This Widget Are Missing / Null.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"Set Action Button Event Failed For UI Screen Widget : {name} Of Selectable Type : {selectableType} - This UI Screen Widget Is Not Yet Active.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void OnSetActionButtonState(InputActionButtonType actionType, InputUIState state)
            {
                if (actionButtonList.Count > 0)
                {
                    UIButton<T> button = actionButtonList.Find(button => button.actionType == actionType);

                    if (button != null)
                        button.SetUIInputState(state);
                }
                else
                    LogWarning("Action Button List Is Null / Empty.", this, () => OnSetActionButtonState(actionType, state));
            }

            void ResetWidgetOnBeginDrag(PointerEventData eventData, Vector2 pos)
            {
                buttonComponent.interactable = false;
                buttonComponent.CancelInvoke();

                layout.ignoreLayout = true;
                dragDistance = eventData.pointerCurrentRaycast.worldPosition;
                //widgetRect.localScale = selectionButtonScaleVect;

                GetWidgetContainer().SetScreenBounds(widgetRect);
                GetWidgetContainer().DeselectAllContentWidgets();

                dragOffSet = widgetRect.anchoredPosition - pos;
                dragStartPosition = pos;

                isDragging = true;
            }

            void GetFolderWidgets()
            {
                if (GetWidgetContainer().GetContentCount() > 0)
                {
                    containerFolderWidgetsReferenceList = new List<UIScreenWidget<SceneDataPackets>>();

                    if (containerFolderWidgetsReferenceList.Count == 0)
                    {
                        var ignoreWidget = this as UIScreenWidget<SceneDataPackets>;

                        GetWidgetContainer().GetContent(ignoreWidget, contentFound =>
                        {
                            if (Helpers.IsSuccessCode(contentFound.resultsCode))
                                containerFolderWidgetsReferenceList = contentFound.data;
                            else
                                Debug.LogWarning($"--> OnBeginDrag Failed With Results : {contentFound.results}");
                        });
                    }
                    else
                        Debug.LogWarning($"--> OnBeginDrag Failed : containerWidgetsReferenceList Is Not Empty / Cleared.");
                }
            }

            void ResetFolderWidgets() => containerFolderWidgetsReferenceList.Clear();

            void OnSetSiblingIndexData()
            {
                if (this != null && GetActive())
                {
                    GetWidgetContainer().GetWidgetSiblingIndex(this, foundSiblingIndex =>
                    {
                        if (Helpers.IsSuccessCode(foundSiblingIndex.resultsCode))
                            SetContentSiblingIndex(foundSiblingIndex.data);
                        else
                            Debug.LogWarning($"GetWidgetSiblindIndex Results : {foundSiblingIndex.results}");
                    });
                }
            }

            void ResetWidgetsSelectionOnBeginDrag()
            {
                if (SelectableManager.Instance != null)
                {
                    if (SelectableManager.Instance.HasActiveSelection())
                        SelectableManager.Instance.DeselectAll();

                    //widgetRect.localScale = selectionButtonScaleVect;
                    OnSelectionFrameState(true, InputUIState.Highlighted, true);
                }
                else
                    Debug.LogWarning("--> OnDrag Failed : SelectableManager.Instance Is Not Yet Initialized.");
            }

            #region Events Callbacks

            #region Drag Callbacks

            public void OnBeginDrag(PointerEventData eventData) => OnBeginDragExecuted(eventData);

            public void OnDrag(PointerEventData eventData) => OnDragExecuted(eventData);

            public void OnEndDrag(PointerEventData eventData) => OnEndDragExecuted(eventData);

            #endregion

            #region Pointer Callbacks

            public void OnPointerDown(PointerEventData eventData) => OnPointerDownExecuted(eventData);

            public void OnPointerUp(PointerEventData eventData) => OnPointerUpExecuted(eventData);

            #endregion

            #endregion

            #region Events Functions

            #region Drag Functions

            void OnBeginDragExecuted(PointerEventData eventData)
            {
                try
                {
                    if (SelectableManager.Instance != null)
                    {
                        if (SelectableManager.Instance.GetCurrentSelectionType() != FocusedSelectionType.SelectedItem)
                        {
                            if (GetActive())
                            {
                                if (GetWidgetContainer().GetScrollerDragViewPort() == null)
                                    return;

                                Vector2 pos;

                                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetWidgetContainer().GetScrollerDragViewPort(), eventData.position, eventData.pressEventCamera, out pos))
                                {
                                    if (!IsSelected())
                                    {
                                        #region Setup

                                        ResetWidgetOnBeginDrag(eventData, pos);
                                        // GetFolderWidgets();

                                        GetWidgetContainer().SetFingerDragEvent();

                                        Deselected();

                                        #endregion

                                        #region Set Sibling Index Data

                                        OnSetSiblingIndexData();

                                        #endregion

                                        #region On Disbale Selections On Drag

                                        ResetWidgetsSelectionOnBeginDrag();

                                        #endregion

                                        #region Parent Widget

                                        GetWidgetContainer().GetPlaceHolder(placeholder =>
                                        {
                                            if (Helpers.IsSuccessCode(placeholder.resultsCode))
                                            {
                                                if (!placeholder.data.IsActive())
                                                    placeholder.data.ShowPlaceHolder(GetWidgetContainer().GetContentContainer(), widgetRect.sizeDelta, GetContentSiblingIndex());

                                                widgetRect.SetParent(GetWidgetContainer().GetItemDragContainer(), false);
                                                GetFolderWidgets();
                                            }
                                            else
                                                LogWarning(placeholder.results, this, () => OnBeginDragExecuted(eventData));
                                        });

                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                    else
                        LogError("Selectable Manager Instance Is Not Yet Initialized.", this, () => OnBeginDragExecuted(eventData));
                }
                catch(Exception exception)
                {
                    LogError(exception.Message, this, () => OnBeginDragExecuted(eventData));
                    throw exception;
                }
            }

            void OnDragExecuted(PointerEventData eventData)
            {
                try
                {
                    if (SelectableManager.Instance != null)
                    {
                        if (SelectableManager.Instance.GetCurrentSelectionType() != FocusedSelectionType.SelectedItem)
                        {
                            if (this != null && GetActive())
                            {
                                if (GetWidgetContainer().GetScrollerDragViewPort() == null)
                                    return;

                                var actionType = (selectableAssetType == SelectableAssetType.Folder) ? InputActionButtonType.OpenFolderButton : InputActionButtonType.OpenSceneAssetPreview;
                                OnSetActionButtonEvent(actionType);

                                if (!IsSelected() && canDrag)
                                {
                                    #region On Drag Item

                                    OnDragWidgetEvent(eventData, GetWidgetContainer().GetScrollerDragViewPort(), currentDragEventData =>
                                    {
                                        if (Helpers.IsSuccessCode(currentDragEventData.resultsCode))
                                        {
                                            OnEdgeScrolling(currentDragEventData.data);
                                            HighlightHoveredFolderOnDrag(eventData, currentDragEventData.data);
                                        }
                                        else
                                            LogError(currentDragEventData.results, this, () => OnDragExecuted(eventData));
                                    });

                                    #endregion
                                }
                                else
                                {
                                    if (isScrolling == false)
                                        isScrolling = true;

                                    if (isScrolling)
                                    {
                                        eventData.pointerDrag = GetWidgetContainer().GetUIScroller().value.gameObject;
                                        EventSystem.current.SetSelectedGameObject(GetWidgetContainer().GetUIScroller().value.gameObject);

                                        GetWidgetContainer().GetUIScroller().value.OnInitializePotentialDrag(eventData);
                                        GetWidgetContainer().GetUIScroller().value.OnBeginDrag(eventData);

                                        OnDeselect();
                                    }
                                }
                            }
                        }
                    }
                    else
                        LogError("Selectable Manager Instance Is Not Yet Initialized.", this, () => OnDragExecuted(eventData));
                }
                catch(Exception execption)
                {
                    LogError(execption.Message, this, () => OnDragExecuted(eventData));
                    throw execption;
                }
            }

            void OnEndDragExecuted(PointerEventData eventData)
            {
                try
                {
                    if (SelectableManager.Instance != null)
                    {
                        if (SelectableManager.Instance.GetCurrentSelectionType() != FocusedSelectionType.SelectedItem)
                        {
                            if (GetActive() && GetWidgetContainer() != null && GetWidgetContainer().IsContainerActive())
                            {
                                if (!IsSelected())
                                {
                                    buttonComponent.interactable = true;
                                    buttonComponent.CancelInvoke();

                                    widgetRect.localScale = Vector3.one;
                                    //OnSelectionFrameState(false, InputUIState.Normal, false);

                                    ResetFolderWidgets();

                                    GetWidgetContainer().GetPlaceHolder(placeholder =>
                                    {
                                        if (Helpers.IsSuccessCode(placeholder.resultsCode))
                                        {
                                            if (placeholder.data.IsActive())
                                            {
                                                widgetRect.SetParent(GetWidgetContainer().GetContentContainer(), false);
                                                placeholder.data.ResetPlaceHolder(ref widgetRect);
                                            }
                                        }
                                        else
                                            Debug.LogWarning($"--> Failed With Results : {placeholder.results}");
                                    });

                                    isDragging = false;

                                    // This below here works but not desirable.
                                    //ScreenUIManager.Instance.Refresh();
                                }

                                OnReset();
                            }
                        }
                    }
                    else
                        LogError("Selectable Manager Instance Is Not Yet Initialized.", this, () => OnEndDragExecuted(eventData));
                }
                catch(Exception exception)
                {
                    LogError(exception.Message, this, () => OnEndDragExecuted(eventData));
                    throw exception;
                }
            }

            #endregion

            #region Pointer Functions

            void OnPointerDownExecuted(PointerEventData eventData)
            {
                try
                {
                    if (GetWidgetContainer())
                    {
                        if (GetActive() && GetWidgetContainer().IsContainerActive())
                        {
                            GetWidgetContainer().SetFingerDown(true);

                            if (SelectableManager.Instance.HasActiveSelection() && SelectableManager.Instance.GetCurrentSelectionType() == FocusedSelectionType.SelectedItem)
                            {
                                SelectableManager.Instance.HasFocusedSelectionInfo(name, hasSelectionCallback =>
                                {
                                    if (Helpers.IsSuccessCode(hasSelectionCallback.resultsCode))
                                    {
                                        StartCoroutine(ExecuteDeselectionStateChangedAsync());
                                    }
                                    else
                                    {
                                        buttonComponent.CancelInvoke();

                                        currentPressDuration = 0.0f;
                                        canDrag = false;
                                        isScrolling = false;

                                        SetFingerDown(true);

                                        StartCoroutine(ExecuteSelectionStateChangedAsync());
                                    }
                                });
                            }
                            else
                            {
                                if (!IsSelected())
                                {
                                    buttonComponent.CancelInvoke();

                                    currentPressDuration = 0.0f;
                                    canDrag = false;
                                    isScrolling = false;

                                    SetFingerDown(true);
                                }
                            }
                        }
                    }
                }
                catch(Exception exception)
                {
                    LogError(exception.Message, this, () => OnPointerDownExecuted(eventData));
                    throw exception;
                }
            }

            IEnumerator ExecuteSelectionStateChangedAsync()
            {
                yield return new WaitUntil(() => IsFingerDown() == false);

                if (SelectableManager.Instance.HasActiveSelection() && SelectableManager.Instance.GetCurrentSelectionType() == FocusedSelectionType.SelectedItem)
                {
                    if(OnPressAndHold())
                        OnSelect(true);
                    else
                        OnSelect();
                }
            }

            IEnumerator ExecuteDeselectionStateChangedAsync()
            {
                yield return new WaitUntil(() => IsFingerDown() == false);

                SelectableManager.Instance.GetFolderStructureSelectionData().Deselect(name, deselectionCallback =>
                {
                    if (!Helpers.IsSuccessCode(deselectionCallback.resultsCode))
                        LogError(deselectionCallback.results, this, () => ExecuteDeselectionStateChangedAsync());
                });

                yield return new WaitForEndOfFrame();

                if (!SelectableManager.Instance.HasActiveSelection())
                {
                    SelectableManager.Instance.Select(name, FocusedSelectionType.InteractedItem);

                    var widgetsContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                    if (widgetsContainer != null)
                    {
                        widgetsContainer.OnFocusedSelectionStateUpdate();
                    }
                }
            }

            void OnPointerUpExecuted(PointerEventData eventData)
            {
                try
                {
                    if (GetActive() && GetWidgetContainer() != null && GetWidgetContainer().IsContainerActive())
                    {
                        GetWidgetContainer().SetFingerUpEvent();

                        if (hoveredWidget != null)
                        {
                            if (hoveredWidget.IsHovered())
                            {
                                if (SceneAssetsManager.Instance != null)
                                {
                                    Folder hoveredFolderData = hoveredWidget.GetFolderData();

                                    if (!string.IsNullOrEmpty(hoveredFolderData.storageData.directory))
                                    {
                                        SceneAssetsManager.Instance.DirectoryFound(hoveredFolderData.storageData.directory, directoryCheckCallback =>
                                        {
                                            if (Helpers.IsSuccessCode(directoryCheckCallback.resultsCode))
                                            {
                                                StorageDirectoryData sourceDirectoryData = (selectableAssetType == SelectableAssetType.Folder) ? GetFolderData().storageData : GetSceneAssetData().storageData;
                                                StorageDirectoryData targetStorageData = GetTargetDirectoryFromSourceStorageDirectoryData(sourceDirectoryData, hoveredFolderData.storageData);

                                                OnDragInsideFolderEvent(sourceDirectoryData, targetStorageData, widgetMovedCallback =>
                                                {
                                                    if (Helpers.IsSuccessCode(widgetMovedCallback.resultsCode))
                                                    {
                                                        GetWidgetContainer().OnFocusToWidget(hoveredWidget, false, true);

                                                        if (SelectableManager.Instance != null)
                                                        {
                                                            if (SelectableManager.Instance.HasActiveSelection())
                                                            {
                                                                SelectableManager.Instance.OnClearFocusedSelectionsInfo(selectionInfoCleared =>
                                                                {
                                                                    if (Helpers.IsSuccessCode(selectionInfoCleared.resultsCode))
                                                                        SelectableManager.Instance.Select(hoveredFolderData.name, FocusedSelectionType.HoveredItem);
                                                                    else
                                                                        LogError(selectionInfoCleared.results, this, () => OnPointerUpExecuted(eventData));
                                                                });
                                                            }
                                                            else
                                                                SelectableManager.Instance.Select(hoveredFolderData.name, FocusedSelectionType.HoveredItem);
                                                        }
                                                        else
                                                            LogError("Selectable Manager Instance Not Yet Initialized.", this, () => OnPointerUpExecuted(eventData));

                                                    // Reload Screen
                                                    ScreenUIManager.Instance.Refresh();

                                                        if (showNotifications)
                                                        {
                                                            notification.message = widgetMovedCallback.results;
                                                            NotificationSystemManager.Instance.ScheduleNotification(notification);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (widgetMovedCallback.data.dataAlreadyExistsInTargetDirectory)
                                                        {
                                                            if (GetActive() && GetWidgetContainer() != null && GetWidgetContainer().IsContainerActive())
                                                            {
                                                                buttonComponent.interactable = false;
                                                                buttonComponent.CancelInvoke();

                                                                widgetRect.localScale = Vector3.one;
                                                            //OnSelectionFrameState(false, InputUIState.Normal, false);

                                                            ResetFolderWidgets();

                                                                GetWidgetContainer().GetPlaceHolder(placeholder =>
                                                                {
                                                                    if (Helpers.IsSuccessCode(placeholder.resultsCode))
                                                                    {
                                                                        if (placeholder.data.IsActive())
                                                                        {
                                                                            widgetRect.SetParent(GetWidgetContainer().GetContentContainer(), false);
                                                                            placeholder.data.ResetPlaceHolder(ref widgetRect);
                                                                        }
                                                                    }
                                                                    else
                                                                        LogWarning(placeholder.results, this, () => OnPointerUpExecuted(eventData));
                                                                });

                                                                OnReset();

                                                                string widgetTitle = (selectableAssetType == SelectableAssetType.Folder) ? $"Folder Already Exist" : "File Already Exist";

                                                                SceneDataPackets dataPackets = new SceneDataPackets
                                                                {
                                                                    widgetTitle = widgetTitle,
                                                                    widgetType = WidgetType.UIAssetActionWarningWidget,
                                                                    blurScreen = true
                                                                };

                                                                if (ScreenUIManager.Instance != null)
                                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);
                                                                else
                                                                    LogError($"Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnPointerUpExecuted(eventData));
                                                            }
                                                        }
                                                        else
                                                            LogWarning(widgetMovedCallback.results, this, () => OnPointerUpExecuted(eventData));
                                                    }
                                                });
                                            }
                                            else
                                                LogWarning(directoryCheckCallback.results, this, () => OnPointerUpExecuted(eventData));
                                        });
                                    }
                                    else
                                       LogWarning("Directory Is Null / Empty.", this, () => OnPointerUpExecuted(eventData));
                                }
                                else
                                   LogError("Scene Assets Manager Instance IS Not Yet Initialized.", this, () => OnPointerUpExecuted(eventData));
                            }
                            else
                                LogError($"Folder not Hovered, Eish!!", this, () => OnPointerUpExecuted(eventData));
                        }
                        else
                        {
                            LogInfo("Check This - Has Something To DO With Selection Ambushed Data.", this, () => OnPointerUpExecuted(eventData));

                            //if (!SelectableManager.Instance.HasFocusedWidgetInfo())
                            //{
                            //    UIWidgetInfo widgetInfo = new UIWidgetInfo
                            //    {
                            //        widgetName = name,
                            //        position = GetWidgetLocalPosition(),
                            //        selectionState = InputUIState.Highlighted
                            //    };

                            //    GetWidgetContainer().SetFocusedWidgetInfo(widgetInfo);
                            //}
                        }

                        if (isScrolling)
                        {
                            OnDeselect();
                            isScrolling = false;
                        }

                        OnReset();
                    }
                }
                catch(Exception exception)
                {
                    LogError(exception.Message, this, () => OnPointerUpExecuted(eventData));
                    throw exception;
                }
            }

            #endregion

            #endregion

            #region On Drag Functions

            void OnDragWidgetEvent(PointerEventData eventData, RectTransform dragRect, Action<CallbackData<Vector2>> callback)
            {
                CallbackData<Vector2> callbackResults = new CallbackData<Vector2>();

                Vector2 pos;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(dragRect, eventData.position, eventData.pressEventCamera, out pos))
                {
                    Vector2 dragPosition = Vector2.zero;
                    Vector2 dragPos = pos + dragOffSet;

                    if (SceneAssetsManager.Instance != null)
                    {
                        if (SceneAssetsManager.Instance.GetFolderStructureData().GetCurrentLayoutViewType() == LayoutViewType.ListView)
                        {
                            dragPosition.x = widgetRect.anchoredPosition.x;
                            dragPosition.y = dragPos.y;
                        }

                        if (SceneAssetsManager.Instance.GetFolderStructureData().GetCurrentLayoutViewType() == LayoutViewType.ItemView)
                            dragPosition = dragPos;
                    }

                    dragPosition.x = Mathf.Clamp(dragPosition.x, GetWidgetContainer().GetScreenBounds().left, GetWidgetContainer().GetScreenBounds().right);
                    dragPosition.y = Mathf.Clamp(dragPosition.y, GetWidgetContainer().GetScreenBounds().bottom, GetWidgetContainer().GetScreenBounds().top);

                    widgetRect.anchoredPosition = dragPosition;

                    callbackResults.results = $"Success - Dragging Widget At Position : {pos}";
                    callbackResults.data = dragPosition;
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "Failed - Dragging Off Screen";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            void OnEdgeScrolling(Vector2 dragPosition)
            {
                if (GetWidgetContainer().GetContainerOrientation() == OrientationType.Vertical)
                {
                    if (dragPosition.y >= GetWidgetContainer().GetScreenBounds().top)
                        GetWidgetContainer().OnEdgetScrolling(DirectionType.Up);
                    else if (dragPosition.y <= GetWidgetContainer().GetScreenBounds().bottom)
                        GetWidgetContainer().OnEdgetScrolling(DirectionType.Down);
                    else
                        GetWidgetContainer().OnEdgetScrolling(DirectionType.Default);
                }

                if (GetWidgetContainer().GetContainerOrientation() == OrientationType.Horizontal)
                {
                    if (dragPosition.x <= GetWidgetContainer().GetScreenBounds().left)
                        GetWidgetContainer().OnEdgetScrolling(DirectionType.Left);
                    else if (dragPosition.x >= GetWidgetContainer().GetScreenBounds().right)
                        GetWidgetContainer().OnEdgetScrolling(DirectionType.Right);
                    else
                        GetWidgetContainer().OnEdgetScrolling(DirectionType.Default);
                }
            }

            void HighlightHoveredFolderOnDrag(PointerEventData eventData, Vector2 dragPosition)
            {
                if (containerFolderWidgetsReferenceList.Count > 0)
                {
                    #region Drag Direction

                    for (int widgetIndex = 0; widgetIndex < containerFolderWidgetsReferenceList.Count; widgetIndex++)
                    {
                        if (containerFolderWidgetsReferenceList[widgetIndex].GetSelectableAssetType() != SelectableAssetType.PlaceHolder)
                        {
                            var canHighlightHoveredWidget = GetCurrentWidgetDragProperties(eventData, dragPosition, widgetIndex);

                            if (canHighlightHoveredWidget.dragTriggered && canHighlightHoveredWidget.highlightHoveredFolder)
                            {
                                hoveredWidget = containerFolderWidgetsReferenceList[widgetIndex];

                                if (hoveredWidget.selectableAssetType == SelectableAssetType.Folder)
                                {
                                    if (canHighlightHoveredWidget.snapToHoveredFolder && GetWidgetContainer().SnapDraggedWidgetToHoveredFolder())
                                    {
                                        Vector2 pos;

                                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hoveredWidget.GetFolderContentContainer(), eventData.position, eventData.pressEventCamera, out pos))
                                        {
                                            Vector3 position = new Vector3(hoveredWidget.GetWidgetPosition().x, hoveredWidget.GetWidgetPosition().y, widgetRect.position.z);
                                            widgetRect.position = position;
                                        }
                                    }
                                }
                                else
                                    widgetRect.SetParent(GetWidgetContainer().GetItemDragContainer(), false);
                            }
                            else
                            {
                                if (containerFolderWidgetsReferenceList[widgetIndex].GetSelectableAssetType() == SelectableAssetType.PlaceHolder)
                                {
                                    if (SnapToDefaultPose(widgetIndex))
                                    {
                                        Vector2 pos;

                                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(containerFolderWidgetsReferenceList[widgetIndex].GetFolderContentContainer(), eventData.position, eventData.pressEventCamera, out pos))
                                            widgetRect.anchoredPosition = containerFolderWidgetsReferenceList[widgetIndex].GetWidgetLocalPosition();

                                        OnWidgetScaleEvent(Vector3.one);
                                    }
                                }

                                containerFolderWidgetsReferenceList[widgetIndex].Deselected();
                            }


                            #region Highlight Items

                            if (hoveredWidget != null && (canHighlightHoveredWidget.highlightHoveredFolder || canHighlightHoveredWidget.snapToHoveredFolder))
                            {
                                OnWidgetScaleEvent(Vector3.one);
                                OnSelectionFrameState(true, InputUIState.Hovered, true);
                                SetUIImageDisplayerValue(SceneAssetsManager.Instance.GetImageFromLibrary(UIImageType.UIWidget_MoveIcon).value, UIImageDisplayerType.ActionIcon);
                                hoveredWidget.SetIsHovered(true);
                                break;
                            }
                            else
                            {

                                OnWidgetScaleEvent(selectionButtonScaleVect);
                                OnSelectionFrameState(true, InputUIState.Highlighted, true);
                                SetUIImageDisplayerValue(SceneAssetsManager.Instance.GetImageFromLibrary(UIImageType.Null_TransparentIcon).value, UIImageDisplayerType.ActionIcon);

                                if (hoveredWidget != null)
                                    hoveredWidget = null;

                                continue;
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
                else
                    LogWarning("ContainerWidgetsList Is Null.", this, () => HighlightHoveredFolderOnDrag(eventData, dragPosition));
            }

            void OnWidgetScaleEvent(Vector3 widgetScale)
            {
                if (!scaleWidgetOnDrag)
                    return;

                widgetRect.localScale = widgetScale;
            }

            (bool dragTriggered, bool highlightHoveredFolder, bool snapToHoveredFolder, DirectionAxisType directionAxis, Vector2 targetScreenPosition, Vector2 targetWorldPosition) GetCurrentWidgetDragProperties(PointerEventData eventData, Vector2 dragPosition, int comparedWidgetIndex)
            {
                #region Calculate Drag Distance From Start Position

                Vector2 dragDirectionValue = (dragPosition - dragStartPosition);
                DirectionAxisType directionAxis = DirectionAxisType.None;
                bool draggedWidgetTriggered = false;
                float distance = 0.0f;
                float targetDistance = 0.0f;

                if (Mathf.Abs(dragDirectionValue.x) > Mathf.Abs(dragDirectionValue.y))
                    directionAxis = DirectionAxisType.Horizontal;
                else
                    directionAxis = DirectionAxisType.Vertical;

                if (SceneAssetsManager.Instance.GetFolderStructureData().GetCurrentLayoutViewType() == LayoutViewType.ItemView)
                {
                    if (directionAxis == DirectionAxisType.Horizontal)
                    {
                        distance = Mathf.Abs(dragDirectionValue.x);
                        targetDistance = widgetRect.sizeDelta.x / 2;

                        if (distance > targetDistance)
                            draggedWidgetTriggered = true;
                    }

                    else if (directionAxis == DirectionAxisType.Vertical)
                    {
                        distance = Mathf.Abs(dragDirectionValue.y);
                        targetDistance = widgetRect.sizeDelta.y / 2;

                        if (distance > targetDistance)
                            draggedWidgetTriggered = true;
                    }
                }

                if (SceneAssetsManager.Instance.GetFolderStructureData().GetCurrentLayoutViewType() == LayoutViewType.ListView)
                {
                    if (directionAxis == DirectionAxisType.Vertical)
                    {
                        distance = Mathf.Abs(dragDirectionValue.y);
                        targetDistance = widgetRect.sizeDelta.y / 2;

                        if (distance > targetDistance)
                            draggedWidgetTriggered = true;
                    }
                }

                #endregion

                var onHighlightHoveredFolder = OnHighlightHoveredFolder(eventData, comparedWidgetIndex);

                return (draggedWidgetTriggered, onHighlightHoveredFolder.highlightFolder, onHighlightHoveredFolder.snapToHoveredFolder, directionAxis, onHighlightHoveredFolder.targetScreenPosition, onHighlightHoveredFolder.targetWorldPosition);
            }

            bool SnapToDefaultPose(int widgetID)
            {
                bool snap = false;

                float dragDistance = Vector2.Distance(widgetRect.anchoredPosition, containerFolderWidgetsReferenceList[widgetID].GetWidgetRect().anchoredPosition);
                float distance = dragDistance / 100.0f;
                distance = Mathf.Clamp(distance, 0.0f, 1.0f);

                snap = distance <= SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.SnapDraggedWidgetToHoveredFolderDistance).value;

                return snap;
            }

            (bool highlightFolder, bool snapToHoveredFolder, Vector2 targetScreenPosition, Vector2 targetWorldPosition) OnHighlightHoveredFolder(PointerEventData eventData, int widgetID)
            {
                bool highlight = false;
                bool snap = false;
                Vector2 targetScreenPosition = Vector2.zero;
                Vector2 targetWorldPosition = Vector2.zero;

                if (containerFolderWidgetsReferenceList != null)
                {
                    if (widgetID <= containerFolderWidgetsReferenceList.Count - 1)
                    {
                        if (containerFolderWidgetsReferenceList[widgetID] != null)
                        {
                            if (this != null && GetActive())
                            {
                                var pos = eventData.enterEventCamera.WorldToScreenPoint(widgetRect.position);
                                var targetPos = eventData.enterEventCamera.WorldToScreenPoint(containerFolderWidgetsReferenceList[widgetID].GetWidgetRect().position);

                                float targetDistance = Vector2.Distance(pos, targetPos);
                                float targetDistanceDevided = targetDistance / 100.0f;
                                float targetDistanceClampled = Mathf.Clamp(targetDistanceDevided, 0.0f, 1.0f);

                                highlight = targetDistanceClampled <= SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.HighlightHoveredFolderDistance).value;
                                snap = targetDistanceClampled <= SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.SnapDraggedWidgetToHoveredFolderDistance).value;
                                targetScreenPosition = targetPos;
                                targetWorldPosition = eventData.enterEventCamera.ScreenToWorldPoint(targetScreenPosition);
                            }
                        }
                        else
                            LogWarning($"Widget At Index : {widgetID} Is Null / Missing.", this);
                    }
                    else
                        LogError($"Widget ID : {widgetID} Is Out Of Range - Not Found In containerWidgetsList With : {containerFolderWidgetsReferenceList.Count} Widgets.", this);
                }
                else
                    LogError("Container Widgets List Is Null.", this);

                return (highlight, snap, targetScreenPosition, targetWorldPosition);
            }

            #endregion

            void OnDragInsideFolderEvent(StorageDirectoryData sourceDirectoryData, StorageDirectoryData targetDirectoryData, Action<CallbackData<DirectoryInfo>> callback = null)
            {
                CallbackData<DirectoryInfo> callbackResults = new CallbackData<DirectoryInfo>();

                bool targetStorageDataDoesntExist = (selectableAssetType == SelectableAssetType.Folder) ? !Directory.Exists(targetDirectoryData.directory) : !File.Exists(targetDirectoryData.path);

                if (targetStorageDataDoesntExist)
                {
                    UIScreenWidget<SceneDataPackets> widget = this as UIScreenWidget<SceneDataPackets>;

                    Debug.LogError($"==> Path : {targetDirectoryData.path} - Directory : {targetDirectoryData.directory} Doesn't Exist");

                    if (widget != null)
                    {
                        bool hasStorageData = !string.IsNullOrEmpty(sourceDirectoryData.directory) && !string.IsNullOrEmpty(targetDirectoryData.directory);

                        if (hasStorageData)
                        {
                            SceneAssetsManager.Instance.OnMoveToDirectory(sourceDirectoryData, targetDirectoryData, widget.GetSelectableAssetType(), fileMoveCallback =>
                            {
                                if (Helpers.IsSuccessCode(fileMoveCallback.resultsCode))
                                {
                                    if (widget != null)
                                    {
                                        if (widgetRect != null)
                                        {
                                            widgetRect.SetParent(hoveredWidget.GetFolderContentContainer(), true);
                                            widgetRect.gameObject.SetActive(false);
                                        }

                                        hoveredWidget = null;

                                        GetWidgetContainer().GetPlaceHolder(placeholder =>
                                        {
                                            if (Helpers.IsSuccessCode(placeholder.resultsCode))
                                            {
                                                if (placeholder.data.IsActive())
                                                    placeholder.data.ResetPlaceHolder();
                                            }
                                            else
                                                LogWarning(placeholder.results, this);
                                        });
                                    }
                                    else
                                        LogError("Widget Is Already Destroyed.", this);

                                    if (callback != null)
                                    {
                                        callbackResults.results = fileMoveCallback.results;
                                        callbackResults.resultsCode = fileMoveCallback.resultsCode;
                                    }
                                }
                                else
                                {
                                    if (fileMoveCallback.data.dataAlreadyExistsInTargetDirectory)
                                    {
                                        callbackResults = fileMoveCallback;
                                        callback?.Invoke(callbackResults);
                                    }
                                    else
                                    {
                                        if (callback != null)
                                        {
                                            callbackResults.results = fileMoveCallback.results;
                                            callbackResults.resultsCode = fileMoveCallback.resultsCode;
                                        }
                                        else
                                            LogWarning($"On Move To Directory Failed With Results : {fileMoveCallback.results}", this);
                                    }
                                }
                            });
                        }
                        else
                        {
                            callbackResults.results = "Source / Target Directory Data Is Missing / Null Or Invalid.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = "Widget Is Null / Couldn't Cats This To UIScreenWidget<SceneDataPackets>";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    string assetType = (selectableAssetType == SelectableAssetType.Folder) ? "Folder" : "File";
                    callbackResults.results = $"{assetType} Already Exist In Folder : {targetDirectoryData.name} ";
                    callbackResults.resultsCode = Helpers.ErrorCode;

                    callbackResults.data = new DirectoryInfo
                    {
                        assetType = selectableAssetType,
                        dataAlreadyExistsInTargetDirectory = true
                    };
                }

                callback?.Invoke(callbackResults);
            }

            StorageDirectoryData GetTargetDirectoryFromSourceStorageDirectoryData(StorageDirectoryData sourceStorageData, StorageDirectoryData targetStorageData)
            {
                // Get Source File Data
                string fileDataSourceName = Path.GetFileName(sourceStorageData.path);

                #region Target Path

                // Get Target File Data
                string fileDataTargetDirectoryPath = Path.Combine(targetStorageData.directory, fileDataSourceName);
                targetStorageData.path = Helpers.GetFormattedDirectoryPath(fileDataTargetDirectoryPath);

                #endregion

                #region Target Directory

                // Get Source Folder Directory info
                string folderTargetDirectoryPath = Path.Combine(targetStorageData.directory, SceneAssetsManager.Instance.GetAssetNameFormatted(sourceStorageData.name, selectableAssetType));
                targetStorageData.directory = Helpers.GetFormattedDirectoryPath(folderTargetDirectoryPath); ;

                #endregion

                return targetStorageData;
            }

            FocusedSelectionInfoData<T> widgetSelectionInfoData = new FocusedSelectionInfoData<T>();

            public FocusedSelectionInfoData<T> OnGetFocusedSelectionInfoData()
            {
                //if(widgetSelectionInfoData.selection == null)
                //{
                //    widgetSelectionInfoData.selection = this;
                //    widgetSelectionInfoData.name = name;
                //}

                return GetWidgetSelectionInfoData();
            }

            public void OnSetFocusedSelectionInfoData(FocusedSelectionInfoData<T> selectionInfoData) => widgetSelectionInfoData = selectionInfoData;

            public FocusedSelectionInfoData<T> GetWidgetSelectionInfoData()
            {
                FocusedSelectionInfoData<T> source = new FocusedSelectionInfoData<T>();

                source.name = name;
                source.selection = this;
                source.selectionInfoType = selectionFrame.GetCurrentSelectionState().uiStateData.selectionType;
                source.state = selectionFrame.GetCurrentSelectionState().uiStateData.state;
                source.showSelection = selectionFrame.GetCurrentSelectionState().showSelection;
                source.showTint = selectionFrame.GetCurrentSelectionState().showTint;

                return source;
            }

            public FocusedSelectionInfoData<T> GetWidgetSelectionInfoData(FocusedSelectionInfoData<T> source)
            {
                source.selectionInfoType = selectionFrame.GetCurrentSelectionState().uiStateData.selectionType;
                source.state = selectionFrame.GetCurrentSelectionState().uiStateData.state;
                source.showSelection = selectionFrame.GetCurrentSelectionState().showSelection;
                source.showTint = selectionFrame.GetCurrentSelectionState().showTint;

                return source;
            }

            public void OnSelectionFrameState(FocusedSelectionStateInfo selectionInfoData, bool async = false, Action<CallbackData<FocusedSelectionInfoData<T>>> callback = null)
            {
                CallbackData<FocusedSelectionInfoData<T>> callbackResults = new CallbackData<FocusedSelectionInfoData<T>>();

                if (selectableAssetType != SelectableAssetType.PlaceHolder)
                {
                    if (!async)
                    {
                        if (selectionFrame.IsInitialized().hasSelectionFrame && selectionFrame.IsInitialized().hasSelectionState && selectionFrame.IsInitialized().hasTint)
                        {
                            #region Selection Info Data
                            FocusedSelectionInfoData<T> newSelectionInfoData = new FocusedSelectionInfoData<T>();

                            if (selectionInfoData.showSelection)
                            {
                                newSelectionInfoData.name = name;
                                newSelectionInfoData.selection = this;

                                OnSelectionFrameState(selectionInfoData.showSelection, selectionInfoData.state, selectionInfoData.showTint, async);

                                //selectionFrame.Show(selectionInfoData.state, selectionInfoData.showTint);
                            }

                            if (newSelectionInfoData.selection != null)
                            {

                                Debug.LogError($"==> New Selection State : {selectionFrame.GetCurrentSelectionState().showSelection}");

                                callbackResults.results = $"Showing Selection Frame For [State] : {selectionInfoData.state} - (Widget) Selection For Game Object [Named] {this.name}.";
                                callbackResults.data = GetWidgetSelectionInfoData(newSelectionInfoData);
                                callbackResults.resultsCode = Helpers.SuccessCode;

                                //if (selectionFrame.GetCurrentSelectionState().showSelection)
                                //{
                                //    //if (!string.IsNullOrEmpty(widgetSelectionInfoData.name))
                                //    //     widgetSelectionInfoData.name = name;

                                //    // widgetSelectionInfoData.selection = newSelectionInfoData.selection;
                                //    // widgetSelectionInfoData.selectionInfoType = selectionFrame.GetCurrentSelectionState().uiStateData.selectionType;
                                //    // widgetSelectionInfoData.state = selectionFrame.GetCurrentSelectionState().uiStateData.state;
                                //    // widgetSelectionInfoData.showSelection = selectionFrame.GetCurrentSelectionState().showSelection;
                                //    // widgetSelectionInfoData.showTint = selectionFrame.GetCurrentSelectionState().showTint;

                                //    callbackResults.results = $"Showing Selection Frame For [State] : {selectionInfoData.state} - (Widget) Selection For Game Object [Named] {this.name}.";
                                //    callbackResults.data = GetWidgetSelectionInfoData(newSelectionInfoData);
                                //    callbackResults.success = true;
                                //}
                                //else
                                //{
                                //    callbackResults.results = $"Selection Frame For [State] : {selectionInfoData.state} - Not Showing For Game Object [Named] : {this.name}.";
                                //    callbackResults.data = default;
                                //    callbackResults.success = false;
                                //}
                            }
                            else
                            {
                                callbackResults.results = $"Selection Widget Value For State : {selectionInfoData.state} - Not Found For Game Object {this.name}.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }

                            #endregion
                        }
                        else
                        {
                            callbackResults.results = $"Not Initialized. Selection Frame For [State] : {selectionInfoData.state} is Missing / Null For Game Object [Named] : {this.name}.";
                            callbackResults.data = default;
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        StartCoroutine(OnSelectionFrameStateAsync(selectionInfoData.showSelection, selectionInfoData.state, selectionInfoData.showTint));

                        callbackResults.results = "Async Called. OnSelectionFrameState Async Called.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = Helpers.InfoCode;
                    }
                }

                callback?.Invoke(callbackResults);
            }

            public void OnSelected() => SetSelected(true);

            public void OnSelectionFrameState(bool show, InputUIState state, bool showTint, bool async = false)
            {
                if (selectableAssetType != SelectableAssetType.PlaceHolder)
                {
                    if (!async)
                    {
                        if (selectionFrame.IsInitialized().hasSelectionFrame && selectionFrame.IsInitialized().hasSelectionState && selectionFrame.IsInitialized().hasTint)
                        {
                            if (show)
                            {
                                selectionFrame.Show(state, showTint);

                                if (state == InputUIState.Selected && !IsSelected())
                                    OnSelected();
                            }
                            else
                                selectionFrame.Hide();
                        }
                        else
                            LogWarning($"SelectionFrame For State Type : {state} is Missing / Null For Game Object Named : {this.name}.", this, () => OnSelectionFrameState(show, state, showTint, async = false));
                    }
                    else
                    {
                        StartCoroutine(OnSelectionFrameStateAsync(show, state, showTint));
                    }
                }
            }

            IEnumerator OnSelectionFrameStateAsync(bool show, InputUIState state, bool showTint)
            {
                yield return new WaitForSeconds(2);

                if (selectionFrame.IsInitialized().hasSelectionFrame && selectionFrame.IsInitialized().hasSelectionState && selectionFrame.IsInitialized().hasTint)
                {
                    if (show)
                        selectionFrame.Show(state, showTint);
                    else
                        selectionFrame.Hide();
                }
                else
                    LogWarning($"Selection Frame is Missing / Null For Game Object [Named] : {this.name}.", this, () => OnSelectionFrameStateAsync(show, state, showTint));
            }

            public void OnSelectionFrameState(bool show, InputUIState state, bool showTint, Vector3 sizeDelta)
            {
                if (selectionFrame.IsInitialized().hasSelectionFrame && selectionFrame.IsInitialized().hasSelectionState && selectionFrame.IsInitialized().hasTint)
                {
                    if (show)
                        selectionFrame.Show(state, showTint);
                    else
                        selectionFrame.Hide();
                }
                else
                    LogWarning("On Selection Frame State Failed : selectionFrame is Missing / Null.", this, () => OnSelectionFrameState(show, state, showTint, sizeDelta));

                widgetRect.localScale = sizeDelta;

                #region Button Functions

                //if (show)
                //    OnSetActionButtonEvent(InputActionButtonType.OpenFolderButton);
                //else
                //    OnSetActionButtonEvent(InputActionButtonType.OpenFolderButton, OnActionButtonInputs);

                #endregion
            }

            public bool SetIsHovered(bool isHovered) => this.isHovered = isHovered;

            public bool IsHovered()
            {
                return isHovered;
            }

            public abstract void OnSelect(bool isInitialSelection = false);

            public abstract void OnDeselect();

            public void Selected() => OnButtonWidgetSelectionState(InputUIState.Selected);

            public void Deselected() => OnButtonWidgetSelectionState(InputUIState.Deselected);

            async Task OnDeselectionAsync()
            {
                await Task.Delay(1000);

                if (!IsSelected())
                    if (buttonComponent)
                    {
                        var actionType = (selectableAssetType == SelectableAssetType.Folder) ? InputActionButtonType.OpenFolderButton : InputActionButtonType.OpenSceneAssetPreview;
                        OnSetActionButtonEvent(actionType, OnActionButtonInputs);
                    }
            }

            async void OnButtonWidgetSelectionState(InputUIState selectionState)
            {
                if (this != null && GetActive())
                {
                    if (GetWidgetContainer() != null && GetWidgetContainer().IsContainerActive())
                    {
                        if (GetActive())
                        {
                            var actionType = (selectableAssetType == SelectableAssetType.Folder) ? InputActionButtonType.OpenFolderButton : InputActionButtonType.OpenSceneAssetPreview;

                            if (selectionState == InputUIState.Selected)
                            {
                                OnSetActionButtonState(actionType, InputUIState.Disabled);

                                OnSetActionButtonEvent(actionType);
                                OnSelectionFrameState(true, selectionState, true);

                                SetSelected(true);
                            }

                            if (selectionState == InputUIState.Deselected)
                            {
                                OnSelectionFrameState(false, selectionState, false);

                                OnSetActionButtonState(actionType, InputUIState.Enabled);

                                SetSelected(false);

                                await OnDeselectionAsync();
                            }

                            SetFingerDown(false);

                            GetWidgetRect().localScale = Vector3.one;
                        }
                        else
                        {
                            OnReset();
                            SetSelected(false);
                        }
                    }
                }
            }

            #endregion
        }

        public class Scene3DPreviewer : AppMonoBaseClass
        {
            //[SerializeField]
            //protected GameObject content = null;

            [Space(5)]
            [SerializeField]
            protected Transform loadedSceneAssetContainer = null;

            [Space(5)]
            [SerializeField]
            UIScreenType screenDependency;

            [Space(5)]
            [SerializeField]
            UIScreenType previousScreenType;

            [Space(5)]
            [SerializeField]
            LoadingItemType loaderType;

            [Space(5)]
            [SerializeField]
            float dummyLoadTime;

            [Space(5)]
            [SerializeField]
            protected bool initialVisibilityState;

            Coroutine showContentRoutine;

            public SceneAssetsManager assetsManager;
            ScreenUIManager screenManager;

            [SerializeField]
            SceneDataPackets currentDataPackets;


            // Fix This To Display Models
            protected void OnScreenChangeEvent(SceneDataPackets dataPackets)
            {

                // Temp Fix. Do Proper Checks


                if (previousScreenType == UIScreenType.AssetCreationScreen)
                {
                    previousScreenType = UIScreenType.None;
                    return;
                }

                if (SceneAssetsManager.Instance != null)
                {
                    if (SceneAssetsManager.Instance.GetCurrentSceneAsset() != null)
                    {
                        if (SceneAssetsManager.Instance.GetCurrentSceneAsset().modelAsset != null)
                        {
                            Debug.Log("==> Scene Asset Found");
                            //screenDependency = dataPackets.screenType;

                            //Transform contentContainer = assetsManager.GetSceneAssetsContainer(screenDependency);

                            //if (contentContainer != null)
                            //    loadedSceneAssetContainer = contentContainer;
                            //else
                            //    Debug.LogWarning($"-->OnScreenChangeEvent Content Container For Screen type : {screenDependency} Missing / Not Found ");

                            currentDataPackets = dataPackets;
                            OnSceneAssetScreenPreviewEvent(SceneAssetsManager.Instance.GetCurrentSceneAsset());
                        }
                        else
                            LogWarning("SceneAssetsManager.Instance.GetCurrentSceneAsset().modelAsset Is Null.", this, () => OnScreenChangeEvent(dataPackets));

                    }
                    else
                        LogWarning("SceneAssetsManager.Instance.GetCurrentSceneAsset() Is Null.", this, () => OnScreenChangeEvent(dataPackets));
                }
                else
                    LogWarning("SceneAssetsManager Instance Is Not Initialed!", this, () => OnScreenChangeEvent(dataPackets));
            }

            protected void OnScreenExitEvent()
            {
                Hide();
            }

            protected void OnClearPreviewedSceneAssetObjectEvent()
            {
                if (loadedSceneAssetContainer.childCount == 0)
                    return;

                for (int i = 0; i < loadedSceneAssetContainer.childCount; i++)
                {
                    //Destroy(loadedSceneAssetContainer.GetChild(i).gameObject);
                    loadedSceneAssetContainer.GetChild(i).gameObject.SetActive(false);
                    LogInfo($"Asset : {loadedSceneAssetContainer.GetChild(i).gameObject.name} Disabled", this, () => OnClearPreviewedSceneAssetObjectEvent());
                }
            }

            protected void OnSceneAssetScreenPreviewEvent(SceneAsset sceneAsset)
            {
                if (sceneAsset.modelAsset)
                {
                    if (ScreenUIManager.Instance != null)
                    {
                        if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                        {
                            if (screenDependency != UIScreenType.None && screenDependency == ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType())
                            {
                                Show();
                            }
                            else
                                LogWarning($"On New Asset Data Created Event Screen UI Manager Instance Get Current Screen Data Value Screen Type : {ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType()} - Doesn't Match The Screen Dependency : {screenDependency}.", this, () => OnSceneAssetScreenPreviewEvent(sceneAsset));
                        }
                        else
                            LogWarning("On New Asset Data Created Event Screen UI Manager Instance Get Current Screen Data Value Is Missing / Null.", this, () => OnSceneAssetScreenPreviewEvent(sceneAsset));
                    }
                    else
                        LogWarning("On New Asset Data Created Event Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnSceneAssetScreenPreviewEvent(sceneAsset));
                }
            }

            public void Show()
            {
                if (screenManager == null)
                    screenManager = ScreenUIManager.Instance;

                if (showContentRoutine != null)
                    StopCoroutine(showContentRoutine);

                showContentRoutine = StartCoroutine(OnShowPreviewContent());
            }

            public void Hide()
            {
                if (SceneAssetsManager.Instance)
                {
                    if (SceneAssetsManager.Instance.GetSceneAssetDynamicContentContainer().Count > 0)
                    {
                        foreach (var container in SceneAssetsManager.Instance.GetSceneAssetDynamicContentContainer())
                        {
                            if (container.value != null)
                                container.Hide(currentDataPackets.resetContentContainerPose, false);
                            else
                                LogWarning("Container Value Is Null.", this, () => Hide());
                        }
                    }
                    else
                        LogWarning("Assets Manager GetSceneAssetDynamicContentContainer Is Null.", this, () => Hide());
                }
                else
                    LogWarning("Assets Manager Not Yet Initialized.", this, () => Hide());
            }

            IEnumerator OnShowPreviewContent()
            {
                try
                {
                    if (assetsManager == null)
                        assetsManager = SceneAssetsManager.Instance;

                    if (screenManager != null)
                    {

                        screenManager.ShowLoadingItem(loaderType, true);

                        yield return Helpers.GetWaitForSeconds(dummyLoadTime);

                        screenManager.ShowLoadingItem(loaderType, false);

                        if (screenManager.GetCurrentScreenData().value != null)
                        {
                            SceneAssetDynamicContentContainer container = assetsManager.GetSceneAssetsContainerData(screenDependency);
                            container.Show(currentDataPackets.resetContentContainerPose, false);

                            previousScreenType = screenDependency;

                        }
                        else
                            LogWarning("Couldn't Show Preview Content - Current Screen Is Null.", this, () => OnShowPreviewContent());
                    }
                    else
                        LogWarning("Couldn't Show Preview Content - Assets Manager Not Yet Initialized.", this, () => OnShowPreviewContent());
                }
                finally
                {
                    LogInfo("Finally : On Show Preview Content.", this, () => OnShowPreviewContent());
                }
            }
        }

        #region UI Action Components

        [Serializable]
        public class UIActionButtonComponent<ComponentType, T, U, V> where ComponentType : UIInputComponent<T, U, V> where T : UnityEngine.Object where U : DataPackets
        {
            #region Components

            [Tooltip("UI Action Buttons Component")]
            [Header("Action Buttons")]

            [Space(10)]
            public string name;
   
            [Space(5)]
            public List<ComponentType> components;

            [Space(5)]
            public InputType inputType;

            [Space(5)]
            public bool initializeComponent;

            T fromClass;

            #endregion

            #region Main

            public void GetValues(Action<CallbackDatas<ComponentType>> callback)
            {
                CallbackDatas<ComponentType> callbackResults = new CallbackDatas<ComponentType>();

                HasRequiredComponents(hasComponentsCallback =>
                {
                    callbackResults = hasComponentsCallback;
                });

                callback.Invoke(callbackResults);
            }

            public void HasRequiredComponents(Action<CallbackDatas<ComponentType>> callback)
            {
                CallbackDatas<ComponentType> callbackResults = new CallbackDatas<ComponentType>();

                if (HasComponentsAssigned())
                {
                    bool assigned = true;
                    string missingValueInfo = string.Empty;

                    foreach (var uiComponent in GetUIComponentList())
                    {
                        if(!uiComponent.value)
                        {
                            assigned = false;
                            missingValueInfo = uiComponent.name;
                            break;
                        }
                    }

                    if(assigned)
                    {
                        callbackResults.results = $"UI Action Component : {name} Has Required Components";
                        callbackResults.data = GetUIComponentList();
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"UI Action Component : {name} Has Some Missing Data For Component : {missingValueInfo}. Value Is Not Assigned In The Editor IInspector";
                        callbackResults.data = default;
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"{name} Is Set To Initialize Component But However, Buttons Are Not Assigned In The Inspector Panel. Triggered From : {fromClass.name}";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public bool HasComponentsAssigned()
            {
                return components != null && components.Count > 0;
            }

            public List<ComponentType> GetUIComponentList()
            {
                return components;
            }

            #endregion
        }

        #endregion

        [Serializable]
        public class UIScreenViewerComponent
        {
            #region Component

            [Space(5)]
            [SerializeField]
            private GameObject view;

            bool isInitialized = false;

           // UIScreenViewComponent screen;

            UIScreenType screenType;

            #endregion

            #region Main

            public void Init(UIScreenType screenType, bool initialVisibilityState = false, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if(HasView())
                {
                    SetInitializedState(true);

                    this.screenType = screenType;

                    callbackResults.results = $"UI Screen View Component Has Been Initialized.";
                    callbackResults.resultsCode = Helpers.SuccessCode;

                    //if(initialVisibilityState)
                    //{
                    //    ShowScreenView(showScreenCallback => 
                    //    {
                    //        callbackResults.results = showScreenCallback.results;
                    //        callbackResults.resultsCode = showScreenCallback.resultsCode;
                    //    });
                    //}
                    //else
                    //{
                    //    callbackResults.results = $"UI Screen View Component Of Type : {GetScreenType()} Has Been Initialized.";
                    //    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    //}
                }
                else
                {
                    callbackResults.results = $"Couldn't initialize UI Screen View Component. Screen View Missing / Not Assigned In The Editor Inspector Panel.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public UIScreenType GetUIScreenType()
            {
                return screenType;
            }

            public GameObject GetView()
            {
                return view;
            }

            public bool HasView()
            {
                return view;
            }

            public async void ShowScreenView(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if(GetInitializedState())
                {
                    if(!GetActive())
                    {
                        view.SetActive(ShowView());

                        await Helpers.GetWaitUntilAsync(GetActive());

                        if(!GetActive())
                        {
                            callbackResults.results = $"Couldn't Show UI Screen View Of Type : {GetUIScreenType()}. This Is An Unexpected Error. Please See Here.";
                            callbackResults.resultsCode = LogInfoType.Error;
                        }
                        else
                        {
                            callbackResults.results = $"Showing UI Screen View Of Type : {GetUIScreenType()}.";
                            callbackResults.resultsCode = LogInfoType.Success;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"UI Screen View Of Type : {GetUIScreenType()} Is Already Active.";
                        callbackResults.resultsCode = LogInfoType.Warning;
                    }
                }
                else
                {
                    callbackResults.results = $"Couldn't Show UI Screen View Of Type : {GetUIScreenType()} Because The Screen Is Not Yet Initialized.";
                    callbackResults.resultsCode = LogInfoType.Error;
                }

                callback?.Invoke(callbackResults);
            }

            public async void HideScreenView(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (GetInitializedState())
                {
                    if (GetActive())
                    {
                        view.SetActive(HideView());

                        await Helpers.GetWaitUntilAsync(!GetActive());

                        if (GetActive())
                        {
                            callbackResults.results = $"Couldn't Hide UI Screen View Of Type : {screenType}. This Is An Unexpected Error. Please See Here.";
                            callbackResults.resultsCode = LogInfoType.Error;
                        }
                        else
                        {
                            callbackResults.results = $"Hidding UI Screen View Of Type : {screenType}.";
                            callbackResults.resultsCode = LogInfoType.Success;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"UI Screen View Of Type : {screenType} Is Already Hidden.";
                        callbackResults.resultsCode = LogInfoType.Warning;
                    }
                }
                else
                {
                    callbackResults.results = "Couldn't Hide UI Screen View Because The Screen Is Not Yet Initialized.";
                    callbackResults.resultsCode = LogInfoType.Error;
                }

                callback?.Invoke(callbackResults);
            }

            public bool GetActive()
            {
                return view && view.activeSelf && view.activeInHierarchy && GetUIScreenType() != UIScreenType.None;
            }

            void SetInitializedState(bool initialized) => isInitialized = initialized;

            bool GetInitializedState()
            {
                return isInitialized;
            }

            bool ShowView()
            {
                return true;
            }

            bool HideView()
            {
                return false;
            }

            #endregion
        }

        public class ScreenUIData : AppMonoBaseClass, IUIScreenData
        {
            [Header("Screen Info")]

            [SerializeField]
            string screenTitle;

            [Space(5)]
            [SerializeField]
            UIScreenViewerComponent screenView = new UIScreenViewerComponent();

            [Space(5)]
            [SerializeField]
            ScreenBlurObject screenBlur = new ScreenBlurObject();

            [Space(5)]
            [SerializeField]
            AssetInfoDisplayer infoDisplayer = new AssetInfoDisplayer();

            [Space(5)]
            [SerializeField]
            Transform widgetsContainer;

            [Space(5)]
            [SerializeField]
            Vector2 screenPosition = Vector2.zero;

            [Space(5)]
            [SerializeField]
            UIScreenType screenType;

            [Space(5)]
            [SerializeField]
            List<UIButton<SceneDataPackets>> screenActionButtonList = new List<UIButton<SceneDataPackets>>();

            [Space(5)]
            [SerializeField]
            List<UIDropDown<SceneDataPackets>> screenActionDropDownList = new List<UIDropDown<SceneDataPackets>>();

            [Space(5)]
            [SerializeField]
            List<UIInputField<SceneDataPackets>> screenActionInputFieldList = new List<UIInputField<SceneDataPackets>>();

            [Space(5)]
            [SerializeField]
            List<UISlider<SceneDataPackets>> screenActionSliderList = new List<UISlider<SceneDataPackets>>();

            [Space(5)]
            [SerializeField]
            List<UICheckbox<SceneDataPackets>> screenActionCheckboxList = new List<UICheckbox<SceneDataPackets>>();

            [Space(5)]
            [SerializeField]
            List<UIText<SceneDataPackets>> screenTextList = new List<UIText<SceneDataPackets>>();

            [Space(5)]
            [SerializeField]
            List<UIImageDisplayer<SceneDataPackets>> screenImageDisplayerList = new List<UIImageDisplayer<SceneDataPackets>>();

            [Space(5)]
            [Header("Screen Data")]

            [Space(5)]
            [SerializeField]
            protected List<LoadingItemData> loadingItemList = new List<LoadingItemData>();

            [Space(5)]
            [SerializeField]
            List<ScreenTogglableWidget<GameObject>> screenTogglableWidgetsList = new List<ScreenTogglableWidget<GameObject>>();

            [Space(5)]
            [SerializeField]
            protected bool includesLoadingAssets;

            [Space(5)]
            [SerializeField]
            protected List<Widget> screenWidgetsList;

            [Space(5)]
            [SerializeField]
            bool initialVisibilityState;

            [SerializeField]
            SceneDataPackets screenData = new SceneDataPackets();

            LoadingItemData currentLoadingItem = new LoadingItemData();

            bool canResetAssetPose = false;

            public void Init(Action<CallbackData<ScreenUIData>> callBack = null)
            {
                CallbackData<ScreenUIData> callbackResults = new CallbackData<ScreenUIData>();

                GetScreenView().Init(screenType, initialVisibilityState, screenViewInitializationCallback => 
                {
                    if(Helpers.IsSuccessCode(screenViewInitializationCallback.resultsCode))
                    {
                        if (includesLoadingAssets)
                        {
                            if (screenWidgetsList == null || screenActionButtonList == null || loadingItemList == null || screenActionDropDownList == null || screenActionInputFieldList == null || screenTextList == null || screenActionSliderList == null || screenActionCheckboxList == null)
                            {
                                callbackResults.results = "Missing Components - Check : Pop Up List / Screen Action Buton List / Loading Item List / Screen Action Drop Down List / Screen Action Input Field List / Screen Text List / Screen Action Slider List / Screen Action Checkbox List.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                            else
                            {
                                callbackResults.results = "Includes Loading Assets - Has Required Components";
                                callbackResults.data = default;
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                        }
                        else
                        {
                            if (screenWidgetsList == null || screenActionButtonList == null || screenActionDropDownList == null || screenActionInputFieldList == null || screenTextList == null || screenActionSliderList == null || screenActionCheckboxList == null)
                            {
                                callbackResults.results = "Missing Components - Check : Pop Up List / Screen Action Buton List / Screen Action Drop Down List / Screen Action Input Field List / Screen Text List / Screen Action Slider List / Screen Action Checkbox List.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                            else
                            {
                                callbackResults.results = "Has Required Components";
                                callbackResults.data = default;
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                        }

                        if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                        {
                            if (screenActionButtonList.Count > 0)
                            {
                                foreach (var button in screenActionButtonList)
                                {
                                    if (button.value != null)
                                    {
                                        button.value.onClick.AddListener(() => ScreenButtonInputs(button));
                                    }
                                    else
                                    {
                                        LogWarning("Button Required.", this);
                                        break;
                                    }
                                }
                            }

                            if (screenActionSliderList.Count > 0)
                            {
                                foreach (var slider in screenActionSliderList)
                                {
                                    if (slider.value != null)
                                    {
                                        slider.value.onValueChanged.AddListener((value) => OnInputSliderValueChangedAction(slider, value));
                                    }
                                    else
                                    {
                                        LogWarning("Screen Text Required.", this);
                                        break;
                                    }
                                }
                            }

                            if (screenActionCheckboxList.Count > 0)
                            {
                                foreach (var checkbox in screenActionCheckboxList)
                                {
                                    if (checkbox.value != null)
                                    {
                                        checkbox.value.onValueChanged.AddListener((value) => OnInputCheckboxValueChangedAction(checkbox, value));
                                        checkbox.SetInteractableState(checkbox.initialInteractabilityState, checkbox.initialVisibilityState);
                                        ActionEvents._OnActionCheckboxStateEvent += checkbox.SetInteractableState;
                                    }
                                    else
                                    {
                                        LogWarning("Screen Checkbox Required.", this);
                                        break;
                                    }
                                }
                            }

                            if (screenTextList.Count > 0)
                            {
                                foreach (var screenText in screenTextList)
                                {
                                    if (screenText.value != null)
                                    {
                                        screenText.SetUIInputVisibilityState(screenText.initialVisibilityState);
                                    }
                                    else
                                    {
                                        LogWarning("Screen Text Required.", this);
                                        break;
                                    }
                                }
                            }

                            if (SceneAssetsManager.Instance != null)
                            {
                                if (screenActionDropDownList.Count > 0)
                                {
                                    List<string> categoriesList = SceneAssetsManager.Instance.GetFormatedDropDownContentList(SceneAssetsManager.Instance.GetDropDownContentData(DropDownContentType.Categories).data);
                                    List<string> sortList = SceneAssetsManager.Instance.GetFormatedDropDownContentList(SceneAssetsManager.Instance.GetDropDownContentData(DropDownContentType.Sorting).data);
                                    List<string> renderModeList = SceneAssetsManager.Instance.GetFormatedDropDownContentList(SceneAssetsManager.Instance.GetDropDownContentData(DropDownContentType.RenderingModes).data);

                                    foreach (var dropDown in screenActionDropDownList)
                                    {
                                        if (dropDown.value != null)
                                        {
                                            switch (dropDown.actionType)
                                            {
                                                case InputDropDownActionType.FilterList:

                                                    if (categoriesList != null)
                                                    {
                                                        dropDown.value.ClearOptions();

                                                        List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                                                        foreach (var filter in categoriesList)
                                                        {
                                                            dropdownOption.Add(new TMP_Dropdown.OptionData() { text = (filter.Contains("None")) ? "All" : filter });
                                                        }

                                                        dropDown.value.AddOptions(dropdownOption);

                                                        dropDown.value.onValueChanged.AddListener((value) => OnDropDownFilterOptions(value));
                                                    }
                                                    else
                                                        Debug.LogWarning($"--> Filter List For Asset Screen : {gameObject.name} Value Is Null.");

                                                    break;

                                                case InputDropDownActionType.SceneAssetRenderMode:

                                                    if (renderModeList != null)
                                                    {
                                                        dropDown.value.ClearOptions();

                                                        List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                                                        foreach (var renderMode in renderModeList)
                                                        {
                                                            dropdownOption.Add(new TMP_Dropdown.OptionData() { text = renderMode });
                                                        }

                                                        dropDown.value.AddOptions(dropdownOption);

                                                        dropDown.value.onValueChanged.AddListener((value) => OnDropDownSceneAssetRenderModeOptions(value));
                                                    }
                                                    else
                                                        Debug.LogWarning($"--> Filter List For Asset Screen : {gameObject.name} Value Is Null.");

                                                    break;

                                                case InputDropDownActionType.SortingList:

                                                    if (sortList != null)
                                                    {
                                                        dropDown.value.ClearOptions();

                                                        List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                                                        foreach (var sort in sortList)
                                                        {
                                                            dropdownOption.Add(new TMP_Dropdown.OptionData() { text = sort });
                                                        }

                                                        dropDown.value.AddOptions(dropdownOption);

                                                        dropDown.value.onValueChanged.AddListener((value) => OnDropDownSortingOptions(value));
                                                    }
                                                    else
                                                        Debug.LogWarning($"--> Sort List For Asset Screen : {gameObject.name} Value Is Null.");

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogWarning("--> Drop Down Value Required.");
                                            break;
                                        }
                                    }
                                }

                                if (screenActionInputFieldList.Count > 0)
                                {
                                    foreach (var inputField in screenActionInputFieldList)
                                    {
                                        if (inputField.value != null)
                                        {
                                            inputField.value.onValueChanged.AddListener((value) => OnInputFieldAction(inputField, value));

                                            if (inputField.clearButton)
                                                inputField.clearButton.onClick.AddListener(() => OnClearInputFieldAction(inputField));
                                        }
                                        else
                                        {
                                            Debug.LogWarning("--> Button Value Required.");
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                                Debug.LogWarning("--> Asset Manager Not Initialized.");

                            if (includesLoadingAssets && loadingItemList.Count > 0)
                            {
                                foreach (var loadingItem in loadingItemList)
                                {
                                    switch (loadingItem.type)
                                    {
                                        case LoadingItemType.Spinner:

                                            if (loadingItem.loadingWidgetsContainer != null)
                                            {
                                                ShowLoadingItem(loadingItem.type, loadingItem.isShowing);
                                            }
                                            else
                                            {
                                                Debug.Log("--> Loading Item Spinner Icon Required.");
                                                return;
                                            }

                                            break;
                                    }
                                }
                            }

                            if (screenWidgetsList.Count > 0)
                            {
                                foreach (var widget in screenWidgetsList)
                                {
                                    if (widget != null && widget.widgetLayouts != null)
                                    {
                                        if (widget.buttons != null)
                                        {
                                            foreach (var button in widget.buttons)
                                            {
                                                if (button.value != null)
                                                {
                                                    button.value.onClick.AddListener(() => OnButtonClicked(widget, button.actionType, button.dataPackets));
                                                }
                                                else
                                                    Debug.LogError($"--> RG_Unity - Init Failed  : Screen Widget List Value For : {widget.name} Is Missing / Null.");
                                            }
                                        }

                                        if (widget.inputs != null)
                                        {
                                            foreach (var input in widget.inputs)
                                            {
                                                if (input.value != null)
                                                {
                                                    input.value.characterLimit = input.characterLimit;

                                                    if (input.valueType == InputFieldValueType.String)
                                                        input.value.onValueChanged.AddListener((value) => widget.SetOnInputValueChanged(value, input.dataPackets));

                                                    if (input.valueType == InputFieldValueType.Integer)
                                                        input.value.onValueChanged.AddListener((value) => widget.SetOnInputValueChanged(int.Parse(value), input.dataPackets));

                                                    input.Initialize();
                                                }
                                                else
                                                    Debug.LogError($"--> RG_Unity - Init Failed  : Screen Widget List Value For : {widget.name} Is Missing / Null.");
                                            }
                                        }

                                        foreach (var layout in widget.widgetLayouts)
                                        {
                                            if (layout.layout)
                                            {
                                                if (widget.initialVisibilityState)
                                                {
                                                    if (layout.layoutViewType == WidgetLayoutViewType.DefaultView)
                                                        layout.ShowLayout();
                                                    else
                                                        layout.HideLayout();
                                                }
                                                else
                                                    layout.HideLayout();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        callbackResults.results = $"Screen Widget : {widget.name} Widget Layouts Not Initialized.";
                                        callbackResults.data = default;
                                        callbackResults.resultsCode = Helpers.ErrorCode;
                                    }
                                }
                            }

                            if (GetScreenView().HasView())
                            {
                                if (GetUIScreenInitialVisibilityState())
                                {
                                    GetScreenView().ShowScreenView(showScreenCallback =>
                                    {
                                        if (showScreenCallback.Success())
                                        {
                                            if (screenBlur.HasBlurObject())
                                            {
                                                screenBlur.Hide();

                                                callbackResults.results = "All Screens Are Initialized";
                                                callbackResults.resultsCode = Helpers.SuccessCode;
                                            }
                                            else
                                            {
                                                callbackResults.results = $"Screen Blur Value Is Null For Screen : {screenTitle}.";
                                                callbackResults.data = default;
                                                callbackResults.resultsCode = Helpers.ErrorCode;
                                            }
                                        }
                                        else
                                            Log(showScreenCallback.resultsCode, showScreenCallback.results, this);
                                    });
                                }
                                else
                                {
                                    GetScreenView().HideScreenView(hideScreenCallback => 
                                    {
                                        if (hideScreenCallback.Success())
                                        {
                                            if (screenBlur.HasBlurObject())
                                            {
                                                screenBlur.Hide();

                                                callbackResults.results = "All Screens Are Initialized";
                                                callbackResults.resultsCode = Helpers.SuccessCode;
                                            }
                                            else
                                            {
                                                callbackResults.results = $"Screen Blur Value Is Null For Screen : {screenTitle}.";
                                                callbackResults.data = default;
                                                callbackResults.resultsCode = Helpers.ErrorCode;
                                            }
                                        }
                                        else
                                            Log(hideScreenCallback.resultsCode, hideScreenCallback.results, this);
                                    });
                                }
                            }
                            else
                            {
                                callbackResults.results = $"Screen Content Container Value Is Null For Screen : {screenTitle}.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                        else
                            LogError(callbackResults.results, this);
                    }
                    else
                    {
                        callbackResults.results = screenViewInitializationCallback.results;
                        callbackResults.resultsCode = screenViewInitializationCallback.resultsCode;
                    }
                });

                callBack?.Invoke(callbackResults);
            }

            protected bool GetUIScreenInitialVisibilityState()
            {
                return initialVisibilityState;
            }

            protected void OnWidgetsEvents(WidgetType widgetType, InputActionButtonType actionType, SceneDataPackets dataPackets)
            {
                if (screenWidgetsList.Count == 0)
                    return;

                switch (widgetType)
                {
                    case WidgetType.ConfirmationPopWidget:


                        switch (actionType)
                        {
                            case InputActionButtonType.HideScreenWidget:

                                HideScreenWidget(widgetType, dataPackets);

                                break;

                            case InputActionButtonType.Confirm:

                                //Debug.Log("--> Confirmed From Pop Up.");

                                //if (SceneAssetsManager.Instance.HasSelectedAssets())

                                //    SceneAssetsManager.Instance.DeleteSelectedSceneAssets((results) => 
                                //    {
                                //        if(results)
                                //        {
                                //            HideScreenWidget(widgetType, dataPackets);

                                //            if (ScreenUIManager.Instance != null)
                                //                ScreenUIManager.Instance.Refresh();
                                //        }
                                //        else
                                //            HideScreenWidget(widgetType, dataPackets);

                                //    });

                                break;
                        }

                        break;

                    case WidgetType.SliderValueWidget:

                        switch (actionType)
                        {
                            case InputActionButtonType.HideScreenWidget:

                                HideScreenWidget(widgetType, dataPackets);

                                break;
                        }

                        break;
                }
            }

            protected void OnScreenChangedEvent(UIScreenType screenType)
            {
                if (screenType == UIScreenType.ARViewScreen)
                {
                    ActionEvents.OnSetCurrentActiveSceneCameraEvent(SceneEventCameraType.ARViewCamera);
                    HideScreenWidget(WidgetType.SceneAssetPreviewWidget, new SceneDataPackets());
                }
                else
                    ActionEvents.OnSetCurrentActiveSceneCameraEvent(SceneEventCameraType.AssetPreviewCamera);


                OnScreenTogglableStateEvent(AppData.TogglableWidgetType.ResetAssetModelRotationButton, false);

                if (this.screenType == screenType)
                    ScreenUIManager.Instance.Refresh();
            }

            protected void OnScreenTogglableStateEvent(TogglableWidgetType widgetType, bool state = false, bool useInteractability = false)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == screenType)
                {
                    if (!state)
                    {
                        AppData.ScreenTogglableWidget<GameObject> screenTogglableWidget = screenTogglableWidgetsList.Find((x) => x.widgetType == widgetType);

                        if (screenTogglableWidget.value != null)
                        {
                            if (useInteractability)
                            {
                                screenTogglableWidget.Interactable(state);
                            }
                            else
                            {
                                if (state)
                                    screenTogglableWidget.Show();
                                else
                                    screenTogglableWidget.Hide();
                            }
                        }
                        else
                            LogWarning($"Screen Togglable Widget Value For Widget Type : {widgetType} Is Null.", this, () => OnScreenTogglableStateEvent(widgetType, state = false, useInteractability = false));

                        canResetAssetPose = false;
                    }
                    else
                    {
                        if (!canResetAssetPose)
                        {
                            ScreenTogglableWidget<GameObject> screenTogglableWidget = screenTogglableWidgetsList.Find((x) => x.widgetType == widgetType);

                            if (screenTogglableWidget.value != null)
                            {
                                if (useInteractability)
                                    screenTogglableWidget.Interactable(state);
                                else
                                {
                                    if (state)
                                        screenTogglableWidget.Show();
                                    else
                                        screenTogglableWidget.Hide();
                                }

                                canResetAssetPose = true;
                            }
                        }
                    }
                }
            }

            protected void OnAssetPoseReset()
            {
                OnScreenTogglableStateEvent(TogglableWidgetType.ResetAssetModelRotationButton, false);
                canResetAssetPose = false;
            }

            protected void OnScreenRefreshed(UIScreenViewComponent screenData)
            {
                LogInfo($"Screen : {screenData.name} - Refreshed Successfully", this, () => OnScreenRefreshed(screenData));
            }

            #region UI Widgets States

            public void SetActionButtonChildWidgetsState(InputActionButtonType actionType, bool interactable, bool isSelected)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == screenType)
                {
                    if (screenActionButtonList != null)
                    {
                        foreach (var actionButton in screenActionButtonList)
                        {
                            if (actionType != InputActionButtonType.None)
                            {
                                if (actionButton.actionType == actionType)
                                {
                                    actionButton.SetChildWidgetsState(interactable, isSelected);
                                    break;
                                }
                            }
                            else
                            {
                                actionButton.SetChildWidgetsState(interactable, isSelected);
                            }
                        }
                    }
                    else
                        LogWarning("Screen Action Button List Not Yet Initialized.", this, () => SetActionButtonChildWidgetsState(actionType, interactable, isSelected));
                }
            }

            #endregion

            #region UI Screen View

            public void SetScreenView(UIScreenViewerComponent screenView) => this.screenView = screenView;

            public UIScreenViewerComponent GetScreenView()
            {
                return screenView;
            }

            #endregion

            #region UI States

            #region UI Action Button States

            public void SetActionButtonUIImageValue(InputActionButtonType actionType, UIImageDisplayerType displayerType, UIImageType imageType, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (ScreenUIManager.Instance != null)
                {
                    if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == GetUIScreenType())
                    {
                        if (screenActionButtonList.Count > 0)
                        {
                            UIButton<SceneDataPackets> button = screenActionButtonList.Find(button => button.actionType == actionType);

                            if (button != null)
                                button.SetUIImageValue(SceneAssetsManager.Instance.GetImageFromLibrary(imageType), displayerType);
                            else
                                LogWarning($"Button Of Type : {actionType} With Displayer : {displayerType} & Image Type : {imageType} Not Found In Screen Type : {screenType} With Action Button List With : {screenActionButtonList.Count} Buttons.", this, () => SetActionButtonUIImageValue(actionType, displayerType, imageType));
                        }
                        else
                            LogWarning("ScreenActionButtonList Is Null / Empty.", this, () => SetActionButtonUIImageValue(actionType, displayerType, imageType));
                    }
                    else
                    {
                        callbackResults.results = $"Screen UI Of Type : {GetUIScreenType()} Is Not Equal To The Current Screen View Of Type : {ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType()}";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                        callbackResults.classInfo = this;
                    }
                }
                else
                {
                    callbackResults.results = "Screen UI Manager Instance Is Not Yet Initialized.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                    callbackResults.classInfo = this;
                }

                callback?.Invoke(callbackResults);
            }

            public void SetActionButtonState(InputUIState state)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == GetUIScreenType())
                {
                    if (screenActionButtonList.Count > 0)
                    {
                        foreach (var button in screenActionButtonList)
                        {
                            if (button.value != null)
                                button.SetUIInputState(state);
                            else
                                LogError($"Button Of Type : {button.actionType}'s Value Missing.", this, () => SetActionButtonState(state));
                        }
                    }
                    else
                        LogWarning("ScreenActionButtonList Is Null / Empty.", this, () => SetActionButtonState(state));
                }
            }

            public void SetActionButtonState(InputActionButtonType actionType, InputUIState state)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == screenType)
                {
                    if (screenActionButtonList.Count > 0)
                    {
                        UIButton<SceneDataPackets> button = screenActionButtonList.Find(button => button.actionType == actionType);

                        if (button != null)
                            button.SetUIInputState(state);
                        else
                            LogWarning($"Button Of Type : {actionType} Not Found In Screen Type : {screenType} With Action Button List With : {screenActionButtonList.Count} Buttons", this, () => SetActionButtonState(actionType, state));
                    }
                    else
                        LogWarning("ScreenActionButtonList Is Null / Empty.", this, () => SetActionButtonState(actionType, state));
                }
            }

            #endregion

            #region UI Action Input States

            public void SetActionInputFieldState(InputUIState state)
            {
                if (screenActionInputFieldList.Count > 0)
                {
                    foreach (var inputField in screenActionInputFieldList)
                    {
                        if (inputField.value != null)
                            inputField.SetUIInputState(state);
                        else
                            LogWarning($"Input Field Of Type : {inputField.actionType}'s Value Missing.", this, () => SetActionInputFieldState(state));
                    }
                }
                else
                    LogWarning("ScreenActionInputFieldList Is Null / Empty.", this, () => SetActionInputFieldState(state));
            }

            public void SetActionInputFieldState(InputFieldActionType actionType, InputUIState state)
            {
                if (screenActionInputFieldList.Count > 0)
                {
                    UIInputField<SceneDataPackets> inputField = screenActionInputFieldList.Find(inputField => inputField.actionType == actionType);

                    if (inputField != null)
                        inputField.SetUIInputState(state);
                    else
                        LogWarning($"Input Field Of Type : {actionType} Not Found In Screen Type : {screenType} With Input Field List With : {screenActionInputFieldList.Count} Input Fields", this, () => SetActionInputFieldState(actionType, state));
                }
                else
                    LogWarning("ScreenActionInputFieldList Is Null / Empty.", this, () => SetActionInputFieldState(actionType, state));
            }

            public void SetActionInputFieldPlaceHolderText(InputFieldActionType actionType, string placeholder)
            {
                if (screenActionInputFieldList.Count > 0)
                {
                    UIInputField<SceneDataPackets> inputField = screenActionInputFieldList.Find(inputField => inputField.actionType == actionType);

                    if (inputField != null)
                        inputField.SetPlaceHolderText(placeholder);
                    else
                        LogWarning($"Input Field Of Type : {actionType} Not Found In Screen Type : {screenType} With Input Field List With : {screenActionInputFieldList.Count} Input Fields", this, () => SetActionInputFieldPlaceHolderText(actionType, placeholder));
                }
                else
                    LogWarning("ScreenActionInputFieldList Is Null / Empty.", this, () => SetActionInputFieldPlaceHolderText(actionType, placeholder));
            }

            #endregion

            #region UI Action Dropdown States

            public void SetActionDropdownState(InputDropDownActionType actionType, InputUIState state)
            {
                if (screenActionDropDownList.Count > 0)
                {
                    UIDropDown<SceneDataPackets> dropdown = screenActionDropDownList.Find(dropdown => dropdown.actionType == actionType);

                    if (dropdown.value != null)
                        dropdown.SetUIInputState(state);
                    else
                        LogWarning($"Input Field Of Type : {actionType} Not Found In Screen Type : {screenType} With Input Field List With : {screenActionDropDownList.Count} Dropdowns", this, () => SetActionDropdownState(actionType, state));
                }
                else
                    LogWarning("ScreenActionDropDownList Is Null / Empty.", this, () => SetActionDropdownState(actionType, state));
            }

            public void SetActionDropdownState(InputDropDownActionType actionType, InputUIState state, List<string> content)
            {
                if (screenActionDropDownList.Count > 0)
                {
                    UIDropDown<SceneDataPackets> dropdown = screenActionDropDownList.Find(dropdown => dropdown.actionType == actionType);

                    if (dropdown.value != null)
                    {
                        dropdown.SetContent(content);
                        dropdown.SetUIInputState(state);
                    }
                    else
                        LogWarning($"Input Field Of Type : {actionType} Not Found In Screen Type : {screenType} With Input Field List With : {screenActionDropDownList.Count} Dropdowns", this, () => SetActionDropdownState(actionType, state, content));
                }
                else
                    LogWarning("ScreenActionDropDownList Is Null / Empty.", this, () => SetActionDropdownState(actionType, state, content));
            }

            public void SetActionDropdownState(InputUIState state)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == screenType)
                {
                    foreach (var dropdown in screenActionDropDownList)
                    {
                        if (dropdown.value != null)
                            dropdown.SetUIInputState(state);
                        else
                            LogWarning($"Dropdown Of Type : {dropdown.actionType}'s Value Missing.", this, () => SetActionDropdownState(state));
                    }
                }
            }

            public void SetActionDropdownState(InputUIState state, List<string> content)
            {
                foreach (var dropdown in screenActionDropDownList)
                {
                    if (dropdown.value != null)
                    {
                        dropdown.SetContent(content);
                        dropdown.SetUIInputState(state);
                    }
                    else
                        LogWarning($"Dropdown Of Type : {dropdown.actionType}'s Value Missing.", this, () => SetActionDropdownState(state, content));
                }
            }

            #endregion

            #region UI Action Slider States

            public void SetActionSliderState(SliderValueType valueType, InputUIState state)
            {
                if (screenActionSliderList.Count > 0)
                {
                    UISlider<SceneDataPackets> slider = screenActionSliderList.Find(slider => slider.valueType == valueType);

                    if (slider.value != null)
                        slider.SetUIInputState(state);
                    else
                        LogError($"Slider Of Type : {valueType} Not Found In Screen Type : {screenType} With Sliders List With : {screenActionSliderList.Count} Sliders", this, () => SetActionSliderState(valueType, state));
                }
                else
                    LogError("ScreenActionSliderList Is Null / Empty.", this, () => SetActionSliderState(valueType, state));
            }

            public void SetActionSliderState(InputUIState state)
            {
                if (screenActionSliderList.Count > 0)
                {
                    foreach (var slider in screenActionSliderList)
                    {
                        if (slider != null)
                            slider.SetUIInputState(state);
                        else
                            LogWarning($"Slider Of Type : {slider.valueType}'s Value Missing.", this, () => SetActionSliderState(state));
                    }
                }
                else
                    LogWarning("ScreenActionSliderList Is Null / Empty.", this, () => SetActionSliderState(state));
            }

            #endregion

            #region UI Action Checkbox States

            public void SetActionCheckboxState(CheckboxInputActionType actionType, InputUIState state)
            {
                if (screenActionCheckboxList.Count > 0)
                {
                    UICheckbox<SceneDataPackets> checkbox = screenActionCheckboxList.Find(checkbox => checkbox.actionType == actionType);

                    if (checkbox != null)
                        checkbox.SetUIInputState(state);
                    else
                        LogWarning($"Checkbox Of Type : {actionType} Not Found In Screen Type : {screenType} With Input Field List With : {screenActionCheckboxList.Count} Checkboxes", this, () => SetActionCheckboxState(actionType, state));
                }
                else
                    LogWarning("ScreenActionCheckboxList Is Null / Empty.", this, () => SetActionCheckboxState(actionType, state));
            }

            public void SetActionCheckboxState(InputUIState state)
            {
                if (screenActionCheckboxList.Count > 0)
                {
                    foreach (var checkbox in screenActionCheckboxList)
                    {
                        if (checkbox != null)
                            checkbox.SetUIInputState(state);
                        else
                            LogWarning($"Checkbox Of Type : {checkbox.actionType}'s Value Missing.", this, () => SetActionCheckboxState(state));
                    }
                }
                else
                    LogWarning("ScreenActionCheckboxList Is Null / Empty.", this, () => SetActionCheckboxState(state));
            }

            #endregion

            #region UI Action Checkbox Value

            public void SetActionCheckboxValue(CheckboxInputActionType actionType, bool value)
            {
                if (screenActionCheckboxList.Count > 0)
                {
                    UICheckbox<SceneDataPackets> checkbox = screenActionCheckboxList.Find(checkbox => checkbox.actionType == actionType);

                    if (checkbox != null)
                        checkbox.SetSelectionState(value);
                    else
                        LogWarning($"Checkbox Of Type : {actionType} Not Found In Screen Type : {screenType} With Input Field List With : {screenActionCheckboxList.Count} Checkboxes", this, () => SetActionCheckboxValue(actionType, value));
                }
                else
                    LogWarning("ScreenActionCheckboxList Is Null / Empty.", this, () => SetActionCheckboxValue(actionType, value));
            }

            public void SetActionCheckboxValue(bool value)
            {
                if (screenActionCheckboxList.Count > 0)
                {
                    foreach (var checkbox in screenActionCheckboxList)
                    {
                        if (checkbox != null)
                            checkbox.SetSelectionState(value);
                        else
                            LogWarning($"Checkbox Of Type : {checkbox.actionType}'s Value Missing.", this, () => SetActionCheckboxValue(value));
                    }
                }
                else
                    LogWarning("ScreenActionCheckboxList Is Null / Empty.", this, () => SetActionCheckboxValue(value));
            }

            #endregion

            #region UI Text Displayer Value

            public void SetUITextDisplayerValue(ScreenTextType textType, string value)
            {
                if (screenTextList.Count > 0)
                {
                    UIText<SceneDataPackets> textDisplayer = screenTextList.Find(textDisplayer => textDisplayer.textType == textType);

                    if (textDisplayer != null)
                        textDisplayer.SetScreenUITextValue(value);
                    else
                        LogWarning($"Text Displayer Of Type : {textType} Not Found In Screen Type : {screenType} With Input Field List With : {screenTextList.Count} Text Displayers", this, () => SetUITextDisplayerValue(textType, value));
                }
                else
                    LogWarning("ScreenTextList Is Null / Empty.", this, () => SetUITextDisplayerValue(textType, value));
            }

            #endregion

            #region UI Image Displayer Value

            public void SetUIImageDisplayerValue(ScreenImageType displayerType, ImageData screenCaptureData, ImageDataPackets dataPackets)
            {
                if (screenImageDisplayerList.Count > 0)
                {
                    UIImageDisplayer<SceneDataPackets> imageDisplayer = screenImageDisplayerList.Find(imageDisplayer => imageDisplayer.imageType == displayerType);

                    if (imageDisplayer != null)
                        imageDisplayer.SetImageData(screenCaptureData, dataPackets);
                    else
                        LogWarning($"Image Displayer Of Type : {displayerType} Not Found In Screen Type : {screenType} With Input Field List With : {screenImageDisplayerList.Count} Image Displayers", this, () => SetUIImageDisplayerValue(displayerType, screenCaptureData, dataPackets));
                }
                else
                    LogWarning("ScreenImageDisplayerList Is Null / Empty.", this, () => SetUIImageDisplayerValue(displayerType, screenCaptureData, dataPackets));
            }

            public void SetUIImageDisplayerValue(ScreenImageType displayerType, Texture2D imageData)
            {
                if (screenImageDisplayerList.Count > 0)
                {
                    UIImageDisplayer<SceneDataPackets> imageDisplayer = screenImageDisplayerList.Find(imageDisplayer => imageDisplayer.imageType == displayerType);

                    if (imageDisplayer != null)
                        imageDisplayer.SetImageData(imageData);
                    else
                        LogWarning($"Image Displayer Of Type : {displayerType} Not Found In Screen Type : {screenType} With Input Field List With : {screenImageDisplayerList.Count} Image Displayers", this, () => SetUIImageDisplayerValue(displayerType, imageData));
                }
                else
                    LogWarning("ScreenImageDisplayerList Is Null / Empty.", this, () => SetUIImageDisplayerValue(displayerType, imageData));
            }

            public void SetUIImageDisplayerValue(ScreenImageType displayerType, Sprite image)
            {
                if (screenImageDisplayerList.Count > 0)
                {
                    UIImageDisplayer<SceneDataPackets> imageDisplayer = screenImageDisplayerList.Find(imageDisplayer => imageDisplayer.imageType == displayerType);

                    if (imageDisplayer != null)
                        imageDisplayer.SetImageData(image);
                    else
                        LogWarning($"Image Displayer Of Type : {displayerType} Not Found In Screen Type : {screenType} With Input Field List With : {screenImageDisplayerList.Count} Image Displayers", this, () => SetUIImageDisplayerValue(displayerType, image));
                }
                else

                    LogWarning("ScreenImageDisplayerList Is Null / Empty.", this, () => SetUIImageDisplayerValue(displayerType, image));
            }

            #endregion
            #endregion

            #region Get Screen Action Inputs

            public List<UIButton<SceneDataPackets>> GetScreenActionButtonList()
            {
                return screenActionButtonList;
            }

            public List<UIInputField<SceneDataPackets>> GetScreenActionInputFieldList()
            {
                return screenActionInputFieldList;
            }

            public List<UIDropDown<SceneDataPackets>> GetScreenActionDropdownList()
            {
                return screenActionDropDownList;
            }

            public List<UISlider<SceneDataPackets>> GetScreenActionSliderList()
            {
                return screenActionSliderList;
            }

            public List<UICheckbox<SceneDataPackets>> GetScreenActionCheckboxList()
            {
                return screenActionCheckboxList;
            }

            public List<UIText<SceneDataPackets>> GetScreenUITextDisplayerList()
            {
                return screenTextList;
            }

            public List<UIImageDisplayer<SceneDataPackets>> GetScreenUIImageDisplayerList()
            {
                return screenImageDisplayerList;
            }

            #endregion

            void ScreenButtonInputs(UIButton<SceneDataPackets> actionButton)
            {
                LogInfo($"Clicked Button Of Action Type : {actionButton.actionType}", this, () => ScreenButtonInputs(actionButton));

                switch (actionButton.actionType)
                {
                    case InputActionButtonType.OpenPopUp:

                        OnOpenPopUp_ActionEvent(actionButton.dataPackets);

                        break;

                    case InputActionButtonType.BuildNewAsset:

                        OnBuildNewAsset_ActionEvent(actionButton.dataPackets);

                        break;

                    case InputActionButtonType.CreateNewAsset:

                        OnCreateNewAsset_ActionEvent(actionButton.dataPackets);


                        break;

                    case InputActionButtonType.HomeButton:

                        OnGoToHomeScreen_ActionEvent(actionButton.dataPackets);

                        break;

                    case InputActionButtonType.Return:

                        OnReturn_ActionEvent();

                        break;

                    case InputActionButtonType.ResetAssetPreviewPose:

                        OnResetAssetPreviewPose_ActionEvent(SceneAssetModeType.CreateMode);

                        break;

                    case InputActionButtonType.OpenARView:

                        OnOpenARView_ActionEvent(actionButton.dataPackets);

                        break;

                    case InputActionButtonType.GoToProfile:

                        OnGoToProfile_ActionEvent(actionButton.dataPackets);

                        break;

                    case InputActionButtonType.CreateNewFolderButton:

                        OnCreateNewFolder_ActionEvent(actionButton.dataPackets);

                        break;

                    case InputActionButtonType.ChangeLayoutViewButton:

                        OnChangeLayoutView_ActionEvent(actionButton.dataPackets);

                        break;

                    case InputActionButtonType.PaginationButton:

                        OnPagination_ActionEvent(actionButton.dataPackets);

                        break;

                    case InputActionButtonType.RefreshButton:

                        OnRefresh_ActionEvent();

                        break;
                }

                ActionEvents.OnActionButtonClicked(actionButton.actionType);
            }

            #region Action Events Callbacks

            void OnOpenPopUp_ActionEvent(SceneDataPackets dataPackets)
            {
                AssetImportContentManager.Instance.SetCurrentDataPacket(dataPackets);

                ShowWidget(dataPackets);

                if (SceneAssetsManager.Instance)
                    SceneAssetsManager.Instance.SetCurrentSceneMode(dataPackets.sceneMode);
            }

            void OnBuildNewAsset_ActionEvent(SceneDataPackets dataPackets)
            {
                #region The Police Must Investigate

                try
                {
                    if (SceneAssetsManager.Instance)
                        if (SceneAssetsManager.Instance.GetCurrentSceneAsset().modelAsset != null)
                        {
                            // This needs to be checked. Can't remember what it does.
                            //if (screenManager)
                            //    screenManager.GetCurrentScreenData().value.GetPopUpWidget(actionButton.dataPackets.popUpType).SetAlwaysShowWidget(assetsManager.GetCurrentSceneAsset().dontShowMetadataWidget);

                            if (SelectableManager.Instance)
                            {
                                if (ScreenUIManager.Instance != null)
                                {
                                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                                    {
                                        if (ScreenUIManager.Instance.GetCurrentScreenData().value.GeWidget(dataPackets.widgetType) != null)
                                        {
                                            // Clear Selection.
                                            SelectableManager.Instance.SetSelectedSceneAsset(null, false);
                                            SelectableManager.Instance.ClearSelection();
                                            SelectableManager.Instance.ClearSelectionList();

                                            if (!ScreenUIManager.Instance.GetCurrentScreenData().value.GeWidget(dataPackets.widgetType).GetAlwaysShowWidget())
                                            {
                                                if (SceneAssetsManager.Instance)
                                                    SceneAssetsManager.Instance.SetCurrentSceneMode(dataPackets.sceneMode);
                                                else
                                                    LogWarning("Scene Assets Not Yet Initialized.", this, () => OnBuildNewAsset_ActionEvent(dataPackets));

                                                if (!SelectableManager.Instance.HasAssetSelected() && !SelectableManager.Instance.HasSelection())
                                                    ActionEvents.OnTransitionSceneEventCamera(dataPackets);
                                                else
                                                    LogWarning("There's Still A Selection Active.", this, () => OnBuildNewAsset_ActionEvent(dataPackets));

                                                ShowWidget(dataPackets);
                                            }
                                            else
                                            {
                                                if (SceneAssetsManager.Instance != null)
                                                {
                                                    Folder currentFolder = SceneAssetsManager.Instance.GetCurrentFolder();

                                                    if (!string.IsNullOrEmpty(currentFolder.storageData.directory))
                                                    {
                                                        SceneAssetsManager.Instance.DirectoryFound(currentFolder.storageData.directory, directoryFoundCallback =>
                                                        {
                                                            if (Helpers.IsSuccessCode(directoryFoundCallback.resultsCode))
                                                            {
                                                                SceneAssetsManager.Instance.BuildSceneAsset(currentFolder.storageData, (assetBuiltCallback) =>
                                                                {
                                                                    if (Helpers.IsSuccessCode(assetBuiltCallback.resultsCode))
                                                                    {
                                                                        if (ScreenUIManager.Instance != null)
                                                                            ScreenUIManager.Instance.ShowScreen(dataPackets);
                                                                        else
                                                                            LogWarning("Screen Manager Missing.", this, () => OnBuildNewAsset_ActionEvent(dataPackets));
                                                                    }
                                                                    else
                                                                        LogWarning(assetBuiltCallback.results, this, () => OnBuildNewAsset_ActionEvent(dataPackets));
                                                                });
                                                            }
                                                            else
                                                                LogWarning(directoryFoundCallback.results, this, () => OnBuildNewAsset_ActionEvent(dataPackets));
                                                        });
                                                    }
                                                    else
                                                        LogWarning("Current Directory Data Is Null.", this, () => OnBuildNewAsset_ActionEvent(dataPackets));
                                                }
                                                else
                                                    LogWarning("Current Directory Not Found.", this, () => OnBuildNewAsset_ActionEvent(dataPackets));
                                            }
                                        }
                                        else
                                            LogWarning($"Screen Manager Current Screen Widget : {dataPackets.widgetType} Not Found / Missing / Not Assigned.", this, () => OnBuildNewAsset_ActionEvent(dataPackets));
                                    }
                                    else
                                        LogWarning("Screen Manager Current Screen Data Is Null/ Missing / Not Assigned.", this, () => OnBuildNewAsset_ActionEvent(dataPackets));
                                }
                                else
                                    LogWarning("Screen Manager Is Not Yet Initialized.", this, () => OnBuildNewAsset_ActionEvent(dataPackets));
                            }
                            else
                                LogWarning("Selectable Manager Not Yet Initialized.", this, () => OnBuildNewAsset_ActionEvent(dataPackets));
                        }
                        else
                        {
                            SceneDataPackets packets = new SceneDataPackets();
                            packets.widgetType = WidgetType.WarningPromptWidget;
                            packets.screenType = UIScreenType.AssetCreationScreen;
                            packets.blurScreen = true;

                            ShowWidget(packets);
                        }
                }
                catch (Exception exception)
                {
                    ThrowException(LogExceptionType.NullReference, exception, this, "ScreenButtonInputs(UIButton<SceneDataPackets> actionButton)");
                    //Debug.LogError($"--> Failed To Build Asset With Exception : {exception}");
                }

                #endregion
            }

            void OnCreateNewAsset_ActionEvent(SceneDataPackets dataPackets)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (AssetImportContentManager.Instance != null)
                    {
                        if (AssetImportContentManager.Instance.IsStoragePermissionsGranted())
                        {
                            Debug.Log($"RG_Unity : UserRequestedAppPermissions Called From Unity - Permission Granted");

                            if (ScreenUIManager.Instance != null)
                                ScreenUIManager.Instance.ShowNewAssetScreen(dataPackets);
                            else
                                LogWarning("Screen Manager Missing.", this, () => OnCreateNewAsset_ActionEvent(dataPackets));

                            if (SceneAssetsManager.Instance)
                                SceneAssetsManager.Instance.SetCurrentSceneMode(dataPackets.sceneMode);
                            else
                                LogWarning("Scene Assets Not Yet Initialized.", this, () => OnCreateNewAsset_ActionEvent(dataPackets));
                        }
                        else
                        {
                            ShowWidget(dataPackets);
                            AssetImportContentManager.Instance.SetRequestedPermissionData(dataPackets);
                        }
                    }
                    else
                        LogWarning("Asset Import Content Manager Not Yet Initialized.", this, () => OnCreateNewAsset_ActionEvent(dataPackets));
                }
                else
                {
                    if (AssetImportContentManager.Instance.ShowPermissionDialogue())
                    {
                        ShowWidget(dataPackets);

                        AssetImportContentManager.Instance.SetRequestedPermissionData(dataPackets);
                    }
                    else
                    {
                        if (ScreenUIManager.Instance != null)
                            ScreenUIManager.Instance.ShowNewAssetScreen(dataPackets);
                        else
                            LogWarning("Screen Manager Missing.", this, () => OnCreateNewAsset_ActionEvent(dataPackets));

                        if (SceneAssetsManager.Instance)
                            SceneAssetsManager.Instance.SetCurrentSceneMode(dataPackets.sceneMode);
                        else
                            LogWarning("Scene Assets Not Yet Initialized.", this, () => OnCreateNewAsset_ActionEvent(dataPackets));
                    }
                }
            }

            void OnGoToHomeScreen_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                    ScreenUIManager.Instance.ShowScreen(dataPackets);
                else
                    LogWarning("Screen Manager Missing.", this, () => OnGoToHomeScreen_ActionEvent(dataPackets));
            }

            void OnReturn_ActionEvent()
            {
                if (SceneAssetsManager.Instance != null)
                {
                    if (ScreenNavigationManager.Instance != null)
                    {
                        if (SelectableManager.Instance != null)
                        {
                            if (SelectableManager.Instance.HasActiveSelection())
                                SelectableManager.Instance.OnClearFocusedSelectionsInfo();

                            ScreenNavigationManager.Instance.ReturnFromFolder(returnFromFolder =>
                            {
                                if (!Helpers.IsSuccessCode(returnFromFolder.resultsCode))
                                {


                                    //// Check Whats Up.
                                    //if (SceneAssetsManager.Instance.GetSceneAssets().Count > 0)
                                    //    SceneAssetsManager.Instance.SetCurrentSceneAsset(SceneAssetsManager.Instance.GetSceneAssets()[0]);

                                    //if (ScreenUIManager.Instance != null)
                                    //    ScreenUIManager.Instance.ShowScreen(dataPackets);
                                    //else
                                    //    Debug.LogWarning("--> Screen Manager Missing.");
                                }
                            });
                        }
                        else
                            LogWarning("Selectable Manager Not Yet Initialized.", this, () => OnReturn_ActionEvent());
                    }
                    else
                        LogWarning("Navigation Manager Not Yet Initialized.", this, () => OnReturn_ActionEvent());
                }
                else
                    LogWarning("SceneAssetsManager.Instance Is Not Yet Initialized.", this, () => OnReturn_ActionEvent());
            }

            void OnResetAssetPreviewPose_ActionEvent(SceneAssetModeType assetMode) => ActionEvents.OnResetSceneAssetPreviewPoseEvent(assetMode);

            void OnOpenARView_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                {
                    //assetData.currentAssetMode = SceneAssetModeType.EditMode;
                    //assetsManager.OnSceneAssetEditMode(assetData);

                    //if (assetData.assetFields != null)
                    //    screenManager.ShowScreen(actionButton.dataPackets);
                    //else
                    //    Debug.LogWarning("--> Scene Asset Data Invalid.");

                    ScreenUIManager.Instance.ShowScreen(dataPackets);
                }
                else
                    LogWarning("Screen Manager Missing.", this, () => OnOpenARView_ActionEvent(dataPackets));
            }

            void OnGoToProfile_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                    ScreenUIManager.Instance.ShowScreen(dataPackets);
                else
                    LogWarning("Screen Manager Missing.", this, () => OnGoToProfile_ActionEvent(dataPackets));
            }

            void OnCreateNewFolder_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                {
                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                    {
                        if (SceneAssetsManager.Instance)
                        {
                            var widgetContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                            if (widgetContainer != null)
                            {
                                widgetContainer.GetPlaceHolder(placeholder =>
                                {
                                    if (Helpers.IsSuccessCode(placeholder.resultsCode))
                                    {
                                        widgetContainer.OnCreateNewPageWidget(onCreateNew =>
                                        {
                                            if (Helpers.IsSuccessCode(onCreateNew.resultsCode))
                                            {
                                                if (!placeholder.data.IsActive())
                                                {
                                                    placeholder.data.ShowPlaceHolder(widgetContainer.GetContentContainer(), widgetContainer.GetCurrentLayoutWidgetDimensions(), widgetContainer.GetLastContentIndex(), true);

                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(WidgetType.UITextDisplayerWidget);
                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);
                                                }
                                            }
                                            else
                                                LogWarning(onCreateNew.results, this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                                        });
                                    }
                                    else
                                        LogWarning(placeholder.results, this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                                });
                            }
                            else
                                LogWarning("Widgets Container Is Missing / Null.", this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                        }
                        else
                            LogWarning("SceneAssetsManager.Instance Is Not Yet Initialized", this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                    }
                    else
                        LogWarning("Value Is Missing / Null.", this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                }
                else
                    LogWarning("ScreenUIManager.Instance Is Not Yet Initialized", this, () => OnCreateNewFolder_ActionEvent(dataPackets));
            }

            void OnChangeLayoutView_ActionEvent(SceneDataPackets dataPackets)
            {
                if (SceneAssetsManager.Instance != null)
                {
                    SceneAssetsManager.Instance.GetDynamicWidgetsContainer(ContentContainerType.FolderStuctureContent, contentContainer =>
                    {
                        if (Helpers.IsSuccessCode(contentContainer.resultsCode))
                        {
                            if(SelectableManager.Instance != null)
                            {
                                UIImageType selectionOptionImageViewType = UIImageType.Null_TransparentIcon;

                                if (SelectableManager.Instance.HasActiveSelection())
                                {
                                    string selectionName = string.Empty;

                                    if (contentContainer.data.GetPaginationViewType() == PaginationViewType.Pager)
                                    {
                                        contentContainer.data.Pagination_CurrentPageContainsSelection(hasSelectionCallback =>
                                        {
                                            if (Helpers.IsSuccessCode(hasSelectionCallback.resultsCode))
                                                selectionName = hasSelectionCallback.data;
                                            else
                                                LogError(hasSelectionCallback.results, this, () => OnChangeLayoutView_ActionEvent(dataPackets));
                                        });
                                    }

                                    if (contentContainer.data.GetPaginationViewType() == PaginationViewType.Scroller)
                                    {

                                    }

                                    switch (SceneAssetsManager.Instance.GetLayoutViewType())
                                    {
                                        case LayoutViewType.ListView:

                                            SceneAssetsManager.Instance.ChangeFolderLayoutView(LayoutViewType.ItemView, dataPackets);

                                            break;

                                        case LayoutViewType.ItemView:

                                            SceneAssetsManager.Instance.ChangeFolderLayoutView(LayoutViewType.ListView, dataPackets);

                                            break;
                                    }

                                    if (!string.IsNullOrEmpty(selectionName))
                                        contentContainer.data.OnLayoutViewChangeSelection(selectionName);
                                }
                                else
                                {
                                    switch (SceneAssetsManager.Instance.GetLayoutViewType())
                                    {
                                        case LayoutViewType.ListView:

                                            SceneAssetsManager.Instance.ChangeFolderLayoutView(LayoutViewType.ItemView, dataPackets);

                                            break;

                                        case LayoutViewType.ItemView:

                                            SceneAssetsManager.Instance.ChangeFolderLayoutView(LayoutViewType.ListView, dataPackets);

                                            break;
                                    }
                                }

                                //ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(InputActionButtonType.SelectionOptionsButton, UIImageDisplayerType.ButtonIcon, selectionOptionImageViewType);
                            }
                            else
                                LogSuccess($"Selectable Manager Instance Is Not Yet Initialized.", this, () => OnChangeLayoutView_ActionEvent(dataPackets));
                        }
                        else
                            LogWarning(contentContainer.results, this, () => OnChangeLayoutView_ActionEvent(dataPackets));
                    });
                }
                else
                    LogWarning("SceneAssetsManager.Instance Is Not Yet Initialized.", this, () => OnChangeLayoutView_ActionEvent(dataPackets));
            }

            void OnPagination_ActionEvent(SceneDataPackets dataPackets)
            {
                if (SceneAssetsManager.Instance != null)
                {
                    SceneAssetsManager.Instance.GetDynamicWidgetsContainer(ContentContainerType.FolderStuctureContent, contentContainer =>
                    {
                        if (Helpers.IsSuccessCode(contentContainer.resultsCode))
                        {
                            LogInfo($"Pagination View Changed : {SceneAssetsManager.Instance.GetPaginationViewType()}", this, () => OnPagination_ActionEvent(dataPackets));

                            switch (SceneAssetsManager.Instance.GetPaginationViewType())
                            {
                                case PaginationViewType.Pager:

                                    SceneAssetsManager.Instance.ChangePaginationView(PaginationViewType.Scroller, dataPackets);

                                    break;

                                case PaginationViewType.Scroller:

                                    SceneAssetsManager.Instance.ChangePaginationView(PaginationViewType.Pager, dataPackets); ;

                                    break;
                            }
                        }
                        else
                            LogWarning(contentContainer.results, this, () => OnPagination_ActionEvent(dataPackets));
                    });
                }
                else
                    LogWarning("SceneAssetsManager.Instance Is Not Yet Initialized.", this, () => OnPagination_ActionEvent(dataPackets));
            }

            void OnRefresh_ActionEvent()
            {
                if (ScreenUIManager.Instance != null)
                {
                    if (SceneAssetsManager.Instance != null)
                    {
                        SceneAssetsManager.Instance.GetDynamicWidgetsContainer(ContentContainerType.FolderStuctureContent, contentContainer =>
                        {
                            if (Helpers.IsSuccessCode(contentContainer.resultsCode))
                            {
                                Debug.LogError("==> On Screen Refresh - Check Here - Has Something To Do With Ambushed Data.");

                                //if (contentContainer.data.HasCurrentFocusedWidgetInfo())
                                //{
                                //    Debug.LogError("==> HasCurrentFocusedWidgetInfo - ScreenRefresh");
                                //    ScreenUIManager.Instance.ScreenRefresh();
                                //}
                                //else
                                //{
                                //    Debug.LogError("==> HasNoCurrentFocusedWidgetInfo - Refresh");
                                //    ScreenUIManager.Instance.Refresh();
                                //}
                            }
                            else
                                LogWarning(contentContainer.results, this, () => OnRefresh_ActionEvent());
                        });
                    }
                    else
                        LogWarning("--> ScreenUIManager.Instance Is Not Yet Initialized", this, () => OnRefresh_ActionEvent());
                }
                else
                    LogWarning("ScreenUIManager.Instance Is Not Yet Initialized", this, () => OnRefresh_ActionEvent());
            }

            #endregion

            void OnDropDownFilterOptions(int dropdownIndex)
            {
                if (SceneAssetsManager.Instance != null)
                {
                    SceneAssetsManager.Instance.SetAssetSortFilterOnDropDownAction(InputDropDownActionType.FilterList, dropdownIndex);
                }
                else
                    LogWarning("Assets Manager Not Yet Initialized.", this, () => OnDropDownFilterOptions(dropdownIndex));
            }

            void OnDropDownSortingOptions(int dropdownIndex)
            {
                if (SceneAssetsManager.Instance != null)
                {
                    SceneAssetsManager.Instance.SetAssetSortFilterOnDropDownAction(InputDropDownActionType.SortingList, dropdownIndex);
                }
                else
                    LogWarning("Assets Manager Not Yet Initialized.", this, () => OnDropDownSortingOptions(dropdownIndex));
            }

            void OnDropDownSceneAssetRenderModeOptions(int dropdownIndex)
            {
                if (SceneAssetsManager.Instance != null)
                {
                    SceneAssetRenderMode renderMode = (SceneAssetRenderMode)dropdownIndex;
                    SceneAssetsManager.Instance.SetSceneAssetRenderMode(renderMode);
                }
                else
                   LogWarning("Assets Manager Not Yet Initialized.", this, () => OnDropDownSceneAssetRenderModeOptions(dropdownIndex));
            }

            void OnInputSliderValueChangedAction(UISlider<SceneDataPackets> slider, float value)
            {
                switch (slider.valueType)
                {
                    case SliderValueType.MaterialBumpScaleValue:

                        Debug.Log($"--> Setting Bump Scale To : {value}");

                        break;

                    case SliderValueType.MaterialGlossinessValue:

                        Debug.Log($"--> Setting Glossiness To : {value}");

                        break;

                    case SliderValueType.MaterialOcclusionIntensityValue:

                        Debug.Log($"--> Setting Occlusion To : {value}");

                        break;

                    case SliderValueType.SceneAssetScale:

                        Debug.Log($"--> Setting Scene Scale To : {value}");

                        break;
                }
            }

            void OnInputCheckboxValueChangedAction(UICheckbox<SceneDataPackets> checkbox, bool value)
            {
                // Update - Remove This Shit Load Here!
                switch (checkbox.actionType)
                {
                    case CheckboxInputActionType.TriangulateWireframe:

                        if (RenderingSettingsManager.Instance)
                            RenderingSettingsManager.Instance.TriangulateWireframe(value);
                        else
                            LogWarning("Rendering Msnager Not Yet Initialized.", this, () => OnInputCheckboxValueChangedAction(checkbox, value));

                        break;
                }
            }

            void OnInputFieldAction(UIInputField<SceneDataPackets> inputField, string inputValue)
            {
                switch (inputField.actionType)
                {
                    case InputFieldActionType.AssetSearchField:

                        if (SceneAssetsManager.Instance != null)
                            SceneAssetsManager.Instance.SearchScreenWidgetList(inputValue);
                        else
                            LogWarning("Assets Manager Not Yet Initialized.", this, () => OnInputFieldAction( inputField, inputValue));

                        break;
                }
            }

            void OnClearInputFieldAction(UIInputField<SceneDataPackets> inputField)
            {
                if (screenActionInputFieldList.Count > 0)
                {
                    foreach (var input in screenActionInputFieldList)
                        if (input.actionType == inputField.actionType)
                            input.value.text = string.Empty;

                    switch (inputField.actionType)
                    {
                        case InputFieldActionType.AssetSearchField:

                            if (SceneAssetsManager.Instance != null)
                                SceneAssetsManager.Instance.SearchScreenWidgetList(string.Empty);
                            else
                                LogWarning("Assets Manager Not Yet Initialized.", this,  () => OnClearInputFieldAction(inputField));

                            break;
                    }
                }
            }

            public void Show(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if(GetScreenView().HasView())
                {
                    GetScreenView().ShowScreenView(showScreenCallback => 
                    {
                        if(showScreenCallback.Success())
                        {
                            callbackResults.results = showScreenCallback.results;
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = showScreenCallback.results;
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    });
                }
                else
                {
                    callbackResults.results = "Screen Component View Missing.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void ShowLoadingItem(LoadingItemType loaderType, bool status)
            {
                if (!includesLoadingAssets || loadingItemList.Count == 0)
                    return;

                foreach (var loadingItem in loadingItemList)
                {
                    if (loadingItem.type == loaderType)
                    {
                        switch (loaderType)
                        {
                            case LoadingItemType.Bar:

                                currentLoadingItem = loadingItem;
                                currentLoadingItem.isShowing = status;

                                SetObjectVisibilityState(loadingItem.loadingWidgetsContainer, status);

                                break;

                            case LoadingItemType.Spinner:

                                currentLoadingItem = loadingItem;
                                currentLoadingItem.isShowing = status;

                                SetObjectVisibilityState(loadingItem.loadingWidgetsContainer, status);

                                break;

                            case LoadingItemType.Text:

                                currentLoadingItem = loadingItem;
                                currentLoadingItem.isShowing = status;

                                SetObjectVisibilityState(loadingItem.loadingWidgetsContainer, status);

                                break;
                        }

                        break;
                    }
                }
            }

            public void ShowScreenText(string text, ScreenTextType textType, bool setVisible)
            {
                if (screenTextList.Count > 0)
                {
                    foreach (var screenText in screenTextList)
                    {
                        if (screenText.value)
                        {
                            if (screenText.textType == textType)
                                screenText.value.text = text;

                            screenText.SetUIInputVisibilityState(setVisible);
                        }
                        else
                            LogWarning("Screen Text Value Missing / Not Assigned.", this, () => ShowScreenText(text, textType, setVisible));
                    }
                }
            }

            public void DisplaySceneAssetInfo(UIScreenViewComponent screen, Action<CallbackData<UIScreenViewComponent>> callback = null)
            {
                CallbackData<UIScreenViewComponent> callbackResults = new CallbackData<UIScreenViewComponent>();

                Helpers.GetComponentIsNotNullOrEmpty(screen.value.GetSceneAsset(), componentCheckCallback => 
                {
                    if(componentCheckCallback.Success())
                    {
                        AssetInfoHandler info = componentCheckCallback.data.GetInfo();

                        if (componentCheckCallback.data.currentAssetMode == SceneAssetModeType.CreateMode)
                        {
                            AssetInfoField titleField = info.GetInfoField(InfoDisplayerFieldType.Title);
                            titleField.name = SceneAssetsManager.Instance.GetDefaultAssetName();

                            AssetInfoField verticesField = info.GetInfoField(InfoDisplayerFieldType.VerticesCounter);
                            verticesField.value = 0;

                            AssetInfoField trianglesField = info.GetInfoField(InfoDisplayerFieldType.TriangleCounter);
                            trianglesField.value = 0;

                            info.UpdateInfoField(titleField);
                            info.UpdateInfoField(verticesField);
                            info.UpdateInfoField(trianglesField);
                        }

                        infoDisplayer.SetAssetInfo(info);
                    }
                    else
                    {
                        callbackResults.results = componentCheckCallback.results;
                        callbackResults.resultsCode = componentCheckCallback.resultsCode;
                    }
                });

                callback?.Invoke(callbackResults);
            }

            public bool GetLoadingItemState()
            {
                return currentLoadingItem.isShowing;
            }

            public Transform GetWidgetsContainer()
            {
                return widgetsContainer;
            }

            public void ShowWidget(SceneDataPackets dataPackets)
            {
                if (screenWidgetsList.Count == 0)
                    return;

                var widget = screenWidgetsList.Find(widget => widget.type.Equals(dataPackets.widgetType));

                if (widget)
                {
                    if (dataPackets.blurScreen)
                        Blur(dataPackets);

                    widget.ResetScrollPosition(scrollerResetCallback => 
                    {
                        if (scrollerResetCallback.Success())
                            widget.ShowScreenWidget(dataPackets);
                        else
                            Log(scrollerResetCallback.resultsCode, scrollerResetCallback.results, this);
                    });
                }
                else
                    LogError($"Widget Of Type : {dataPackets.widgetType} - Missing / Not Found.", this);
            }

            public void ShowWidget(SceneDataPackets dataPackets, Action<bool> callback)
            {
                if (screenWidgetsList.Count == 0)
                    return;

                foreach (var widget in screenWidgetsList)
                {
                    if (widget.type == dataPackets.widgetType)
                    {
                        if (dataPackets.blurScreen)
                            Blur(dataPackets);

                        widget.ShowScreenWidget(dataPackets);

                        callback.Invoke(true);

                        break;
                    }
                }
            }

            public void HideScreenWidget(WidgetType widgetType, bool canTransition = true)
            {
                LogInfo($"===========> Now Hide Widget Of Type : {widgetType}.................", this);

                if (screenWidgetsList.Count == 0)
                    return;

                var widget = screenWidgetsList.Find(widget => widget.type == widgetType);

                if (widget != null)
                {
                    widget.Hide(canTransition, hideCallback =>
                    {
                        if (hideCallback.Success())
                        {
                            Focus();
                        }
                        else
                            Log(hideCallback.resultsCode, hideCallback.results, this);
                    });
                }
                else
                    LogError($"Couldn't Hide Widget Of Type : {widgetType} - Widget Missing / Not Found.");
            }

            public Widget GetWidget(WidgetType widgetType)
            {
                return screenWidgetsList.Find(widget => widget.type == widgetType);
            }

            public List<Widget> GetWidgets()
            {
                return screenWidgetsList;
            }

            public void HideScreenWidget(WidgetType widgetType, SceneDataPackets dataPackets)
            {
                if (screenWidgetsList.Count == 0)
                    return;

                foreach (var widget in screenWidgetsList)
                {
                    if (widget.type == widgetType)
                    {
                        widget.Hide();

                        if (widgetType == WidgetType.ConfirmationPopWidget)
                        {
                            if (SelectableManager.Instance)
                            {
                                if (!SelectableManager.Instance.HasAssetSelected() && !SelectableManager.Instance.HasSelection())
                                    ActionEvents.OnTransitionSceneEventCamera(dataPackets);
                                else
                                    LogWarning("There Is Still A Selection Active.", this, () => HideScreenWidget(widgetType, dataPackets));
                            }
                            else
                                LogError("Selectable Manager Not Yet Initialized.", this, () => HideScreenWidget(widgetType, dataPackets));
                        }

                        if (widget.type == WidgetType.SceneAssetPreviewWidget)
                            if (SelectableManager.Instance.GetSceneAssetInteractableMode() == SceneAssetInteractableMode.Orbit)
                                ActionEvents.OnResetCameraToDefaultPoseEvent();

                        break;
                    }
                }

                Focus();
            }

            public void HideScreenWidgets()
            {
                if (screenWidgetsList.Count == 0)
                    return;

                foreach (var widget in screenWidgetsList)
                {
                    widget.Hide();
                }

                Focus();
            }

            public Widget GeWidget(WidgetType type)
            {
                Widget widget = null;

                if (screenWidgetsList != null)
                {
                    foreach (var popUpWidget in screenWidgetsList)
                    {
                        if (popUpWidget.type == type)
                        {
                            widget = popUpWidget;
                            break;
                        }
                        else
                            continue;
                    }
                }

                return widget;
            }

            public void Hide() => SetObjectVisibilityState(GetScreenView().GetView(), false);

            public void HidePopUp(WidgetType popUpType)
            {
                screenBlur.Hide();
            }

            public void Focus()
            {
                screenBlur.Hide();

                ActionEvents.OnScreenViewStateChangedEvent(ScreenViewState.Focused);
            }

            public void Blur(SceneDataPackets dataPackets)
            {
                screenBlur.Show(dataPackets.blurContainerLayerType);

                if (dataPackets.screenViewState == ScreenViewState.None)
                    dataPackets.screenViewState = ScreenViewState.Blurred;

                ActionEvents.OnScreenViewStateChangedEvent(dataPackets.screenViewState);
            }

            public GameObject GetScreenObject()
            {
                return GetScreenView().GetView();
            }

            public Vector2 GetScreenPosition()
            {
                return screenPosition;
            }

            public string GetScreenTitle()
            {
                return screenTitle;
            }

            public UIScreenType GetUIScreenType()
            {
                return screenType;
            }

            void OnButtonClicked(Widget widget, InputActionButtonType actionType, SceneDataPackets dataPackets)
            {
                widget.OnWidgetActionEvent(widget.type, actionType, dataPackets);
            }

            public void SetScreenData(SceneDataPackets dataPackets)
            {
                screenData = dataPackets;
            }

            public SceneDataPackets GetScreenData()
            {
                return screenData;
            }

            public SceneAsset GetSceneAsset()
            {
                return GetScreenData().sceneAsset;
            }

            void SetObjectVisibilityState(GameObject value, bool state) => value.SetActive(state);
        
            public SceneDataPackets GetActionButtonDataPackets(InputActionButtonType actionType)
            {
                SceneDataPackets dataPackets = new SceneDataPackets();

                if (screenActionButtonList.Count > 0)
                {
                    foreach (var actionButton in screenActionButtonList)
                    {
                        if (actionButton.value != null)
                        {
                            dataPackets = actionButton.dataPackets;
                            Debug.Log("--> Action Button Data Packets Assigned Successfully.");
                        }
                        else
                            LogWarning($"Action Button : {actionButton.name} Value Is Missing / Null.", this, () => GetActionButtonDataPackets(actionType));
                    }
                }
                else
                    LogWarning($"Action Button List Not Initialized for : {this.gameObject.name}", this, () => GetActionButtonDataPackets(actionType));

                return dataPackets;
            }
        }

        public abstract class Selectable : AppMonoBaseClass
        {
            #region Components

            #endregion

            #region Unity Callbacks

            void OnEnable()
            {
                SetupTouchSupport(true);
                OnSubscribeToActionEvents(true);
            }

            private void OnDisable() => OnSubscribeToActionEvents(false);

            private void Start() => OnInitialization();

            void Update() => UpdateSelectableStateAction();

            #endregion

            #region Main

            void SetupTouchSupport(bool subscribe)
            {
                if (subscribe)
                {
                    EnhancedTouchSupport.Enable();
                    TouchSimulation.Enable();

                    Touch.onFingerDown += OnFingerDown;
                    Touch.onFingerMove += OnFingerMoved;
                    Touch.onFingerUp += OnFingerUp;

                }
                else
                {
                    EnhancedTouchSupport.Disable();
                    TouchSimulation.Disable();

                    Touch.onFingerDown -= OnFingerDown;
                    Touch.onFingerMove -= OnFingerMoved;
                    Touch.onFingerUp -= OnFingerUp;
                }
            }

            #region Overrides

            protected abstract void OnInitialization();
            protected abstract void OnSubscribeToActionEvents(bool subscribe);
            protected abstract void OnFingerDown(Finger finger);
            protected abstract void OnFingerMoved(Finger finger);
            protected abstract void OnFingerUp(Finger finger);
            protected abstract void UpdateSelectableStateAction();

            #endregion

            #endregion
        }

        public class Interactable : AppMonoBaseClass
        {

        }

        public class SceneAssetModel : AppMonoBaseClass
        {
            #region Components

            public GameObject value;

            Transform container;

            [SerializeField]
            Vector3 defaultScale;

            bool assetInitialized = false;

            #endregion

            #region Unity Callbacks

            void OnEnable() => OnSubscribeToEvents(true);

            void OnDisable() => OnSubscribeToEvents(false);

            #endregion

            #region Main

            public void Init(Vector3 scale)
            {
                if (!assetInitialized)
                {
                    defaultScale = scale;
                    assetInitialized = true;
                }
            }

            void OnSubscribeToEvents(bool subscribed)
            {
                if (subscribed)
                    ActionEvents._OnUpdateSceneAssetDefaultRotation += SetSceneAssetModelRotation;
                else
                    ActionEvents._OnUpdateSceneAssetDefaultRotation -= SetSceneAssetModelRotation;
            }


            public void SetSceneAssetModelRotation(Quaternion rotation)
            {
                if (value)
                    value.transform.rotation = rotation;
                else
                    Debug.LogWarning("--> Scele Asset Model Value Missing / Null");
            }

            public void SetSceneAssetModelPosition(Vector3 position)
            {
                if (value)
                    value.transform.localScale = position;
                else
                    Debug.LogWarning("--> Scele Asset Model Value Missing / Null");
            }

            public void SetSceneAssetModelScale(Vector3 scale)
            {
                if (value)
                    value.transform.localScale = scale;
                else
                    Debug.LogWarning("--> Scele Asset Model Value Missing / Null");
            }

            public void Reset(bool setActive = false, bool revertTodefaultScale = true, Transform parent = null)
            {
                if (value)
                {
                    value.SetActive(setActive);

                    if (parent != null)
                        value.transform.SetParent(parent);

                    if (revertTodefaultScale)
                        value.transform.localScale = defaultScale;

                    value.transform.position = Vector3.zero;
                }
                else
                    Debug.LogWarning("--> Scele Asset Model Value Missing / Null");
            }

            public Vector3 GetSceneAssetModelScale()
            {
                return value.transform.localScale;
            }

            public void SetContainer(Transform assetContainer)
            {
                container = assetContainer;
            }

            public Transform GetContainer()
            {
                return container;
            }

            #endregion
        }

        [Serializable]
        public struct WidgetLayoutView
        {
            #region Components

            public string layoutName;

            [Space(5)]
            public GameObject layout;

            [Space(5)]
            public WidgetLayoutViewType layoutViewType;

            #endregion

            #region Main

            public void ShowLayout(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback()
 ;
                if (IsInitialized())
                {
                    layout.SetActive(true);

                    callbackResults.results = $"Layout View Of Type : {layoutViewType} Is Initialized And Now Set To Active.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Layout View Of Type : {layoutViewType} Is Not Initialized.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void HideLayout(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (IsInitialized())
                {
                    layout.SetActive(false);

                    callbackResults.results = $"Layout View Of Type : {layoutViewType} Is Initialized And Now Deactivated.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Layout View Of Type : {layoutViewType} Is Not Initialized.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public bool IsInitialized()
            {
                return layout != null;
            }

            #endregion
        }

        [Serializable]
        public abstract class Widget : AppMonoBaseClass
        {
            [Space(5)]
            public TMP_Text titleDisplayer;

            [Space(5)] // Deprecated
            public GameObject value;

            [Space(5)]
            public List<WidgetLayoutView> widgetLayouts = new List<WidgetLayoutView>();

            [Space(5)]
            public WidgetLayoutViewType defaultLayoutType;

            #region Inputs

            [Space(10)]
            [Header("Widget UI Inputs")]

            [Space(5)]
            public List<UIButton<SceneDataPackets>> buttons;

            [Space(5)]
            public List<UIInputField<SceneDataPackets>> inputs;

            [Space(5)]
            public List<UIDropDown<SceneDataPackets>> dropdowns;

            [Space(5)]
            public List<UIInputSlider<SceneDataPackets>> sliders;

            [Space(5)]
            public List<UISlider<SceneDataPackets>> uiSliders;

            [Space(5)]
            public List<UICheckbox<SceneDataPackets>> checkboxes;

            [Space(5)]
            public List<UIText<SceneDataPackets>> textDisplayerList;

            [Space(5)]
            public List<UIImageDisplayer<SceneDataPackets>> imageDisplayers;

            #endregion

            #region Scroller

            [Space(10)]
            [Header("Widget Scroller")]

            [Space(5)]
            public UIScroller<SceneDataPackets> scroller;

            #endregion

            [Space(10)]
            [Header("Widget Settings")]

            [Space(5)]
            public WidgetType type;

            [Space(5)]
            [SerializeField]
            protected bool subscribeToActionEvents = false;

            [Space(5)]
            public ScreenWidgetTransitionType transitionType;

            [Space(5)]
            public bool initialVisibilityState;

            [Space(5)]
            public float initializationDelay;

            [Space(5)]
            public Toggle dontShowAgainToggleField;

            [Space(5)]
            [SerializeField]
            protected bool onWidgetTransition;

            [Space(5)]
            [SerializeField]
            protected bool showWidget;

            [Space(5)]
            [SerializeField]
            protected UIScreenWidgetContainer widgetContainer = new UIScreenWidgetContainer();

            //[HideInInspector]
            public bool dontShowAgain;

            public bool isWidgetVisible = false;

            protected bool isTransitionState;

            protected RectTransform widgetRect;

            //[HideInInspector]
            public SceneDataPackets currentDataPackets;

            Coroutine showWidgetAsyncRoutine;

            #region Widgets 

            protected SliderValuePopUpWidget sliderWidget = null;
            protected ConfirmationPopUpWidget confirmationWidget = null;
            protected CreateAssetConfirmationPopUpWidget createAssetConfirmationWidget;
            protected BuildWarningPromptPopUpWidget buildWarningPromptWidget;
            protected SelectedSceneAssetPreviewWidget selectedSceneAssetPreviewWidget;
            protected SceneAssetPropertiesWidget assetPropertiesWidget;
            protected AssetImportWidget assetImportWidget;
            protected AppPermissionsRequestWidget permissionsRequestWidget;
            protected LoadingScreenWidget loadingScreenWidget;
            protected SceneAssetExportWidget sceneAssetExportWidget;
            protected RenderSettingsWidget renderSettingsWidget;
            protected AssetPublishingWidget assetPublishingWidget;
            protected NetworkNotificationWidget networkNotificationWidget;
            protected ColorPickerWidget colorPickerWidget;
            protected SnapShotWidget snapShotWidget;
            protected UIScreenFolderCreationWidget folderCreationWidget;
            protected FileSelectionOptionsWidget fileSelectionOptionsWidget;
            protected SrollerNavigationWidget srollerNavigationWidget;
            protected UITextDisplayerWidget textDisplayerWidget;
            protected PagerNavigationWidget pagerNavigationWidget;
            protected UIAssetActionWarningWidget uiAssetWarningWidget;
            protected SelectionOptionsWidget selectionOptionsWidget;

            #endregion

            #region Unity Callbacks

            void OnEnable()
            {
                if (subscribeToActionEvents)
                    OnSubscribeToActionEvents(true);
            }

            void OnDisable()
            {
                if (subscribeToActionEvents)
                    OnSubscribeToActionEvents(false);
            }

            void Update() => OnWidgetTransition();

            #endregion

            protected void Init()
            {
                WidgetLayoutView layoutView = GetLayoutView();

                if (layoutView.layout)
                    if (layoutView.layout.GetComponent<RectTransform>())
                        widgetRect = layoutView.layout.GetComponent<RectTransform>();
                    else
                        LogWarning("Value Doesn't Have A Rect Transform Component.", this, () => Init());
                else
                    LogWarning("Value Is Null.", this, () => Init());


                if (dontShowAgainToggleField != null)
                    dontShowAgainToggleField.onValueChanged.AddListener((value) => SetAlwaysShowWidget(value));

                #region Initialize Checkbox

                if(checkboxes != null && checkboxes.Count > 0)
                {
                    foreach (var checkbox in checkboxes)
                    {
                        if(checkbox.value != null)
                            checkbox.value.onValueChanged.AddListener((value) => SetOnCheckboxValueChanged(checkbox.actionType, value, checkbox.dataPackets));
                        else
                        {
                            LogError($"Checkbox : {checkbox.name} Value Is Null.", this, () => Init());
                            break;
                        }
                    }
                }

                #endregion
            }

            protected abstract void OnSubscribeToActionEvents(bool subscribe);

            public void OnWidgetActionEvent(WidgetType popUpType, InputActionButtonType actionType, SceneDataPackets dataPackets)
            {
                if (SceneAssetsManager.Instance != null && ScreenUIManager.Instance != null)
                {
                    switch (actionType)
                    {
                        case InputActionButtonType.OpenPopUp:

                            break;

                        case InputActionButtonType.SelectionOptionsButton:

                            OnSelectionOptions_ActionEvents(dataPackets);

                            break;

                        case InputActionButtonType.SelectionButton:

                            OnSelection_ActionEvents(dataPackets);

                            break;

                        case InputActionButtonType.Undo:

                            UndoChanges();

                            break;

                        case InputActionButtonType.Delete:

                            OnDelete_ActionEvent(dataPackets);

                            break;

                        case InputActionButtonType.Info:

                            ShowInfo();

                            break;

                        case InputActionButtonType.PinButton:

                            OnPinItem_ActionEvent(dataPackets);

                            break;

                        case InputActionButtonType.BuildNewAsset:

                            OnBuildNewAsset_ActionEvent(popUpType, dataPackets);

                            break;

                        case InputActionButtonType.HideScreenWidget:

                            OnHideScreenWidget_ActionEvent(popUpType, dataPackets);

                            break;

                        case InputActionButtonType.OpenFilePicker_OBJ:

                            OnOpenFilePicker_ActionEvent(popUpType, dataPackets);

                            break;

                        case InputActionButtonType.OpenARView:

                            OnOpenARView_ActionEvent(dataPackets);

                            break;

                        case InputActionButtonType.Confirm:

                            OnConfirm_ActionEvent(popUpType, dataPackets);

                            break;

                        case InputActionButtonType.Cancel:

                            OnCancel_ActionEvent(dataPackets);

                            break;

                        case InputActionButtonType.ResetAssetPreviewPose:

                            OnResetAssetPreviewPose_ActionEvent(SceneAssetModeType.PreviewMode);

                            break;

                        case InputActionButtonType.ExportAsset:

                            OnExportAsset_ActionEvent(dataPackets);

                            break;

                        case InputActionButtonType.OpenRenderSettings:

                            OnOpenRendererSettings_ActionEvent(dataPackets);

                            break;

                        case InputActionButtonType.PublishAsset:

                            OnPublishAsset_ActionEvent(dataPackets);

                            break;

                        case InputActionButtonType.CaptureSnapShot:

                            OnCaptureSnapShot_ActionEvent(dataPackets);

                            break;

                        case InputActionButtonType.ScrollToTopButton:

                            OnScrollToTop_ActionEvent();

                            break;

                        case InputActionButtonType.ScrollToBottomButton:

                            OnScrollToBottom_ActionEvent();

                            break;

                        case InputActionButtonType.NextNavigationButton:

                            OnPaginationNavigation_ActionEvent(PaginationNavigationActionType.GoToNextPage);

                            break;

                        case InputActionButtonType.PreviousNavigationButton:

                            OnPaginationNavigation_ActionEvent(PaginationNavigationActionType.GoToPreviousPage);

                            break;

                        case InputActionButtonType.FolderActionButton:

                            OnFolderActions_ActionEvent(dataPackets);

                            break;

                        case InputActionButtonType.DeselectButton:

                            OnDeselect_ActionEvent(dataPackets);

                            break;
                    }
                }
                else
                    LogWarning("Scene Assets Manager / Screen UI Manager Instance Not Found.", this, () => OnWidgetActionEvent(popUpType, actionType, dataPackets));

                ActionEvents.OnPopUpActionEvent(popUpType, actionType, dataPackets);
                ActionEvents.OnActionButtonClicked(actionType);
            }

            #region Action Inputs Callbacks

            void OnSelectionOptions_ActionEvents(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                {
                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                    {
                        if (SceneAssetsManager.Instance != null)
                        {
                            SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.HasAllWidgetsSelected(allWidgetsSelectedCallback => 
                            {
                                if(allWidgetsSelectedCallback.Success())
                                {
                                    if(SelectableManager.Instance)
                                        SelectableManager.Instance.DeselectAll();
                                    else
                                        LogError("Selectable Manager Instance Is Not Yet Initialized.", this, () => OnSelectionOptions_ActionEvents(dataPackets));
                                }
                                else
                                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);
                            });
                        }
                        else
                            LogError("Scene Assets Manager Instance Is Not Yet Initialized.", this, () => OnSelectionOptions_ActionEvents(dataPackets));
                    }
                    else
                        LogWarning("On Widget Action Event Screen Manager Get Current Screen Data Value Is Null", this, () => OnSelectionOptions_ActionEvents(dataPackets));
                }
                else
                    LogError("Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnSelectionOptions_ActionEvents(dataPackets));
            }

            void OnSelection_ActionEvents(SceneDataPackets dataPackets)
            {
                if (SceneAssetsManager.Instance != null && ScreenUIManager.Instance != null)
                {
                    var widgetsContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                    if (widgetsContainer != null)
                    {
                        switch (dataPackets.selectionOption)
                        {
                            case SelectionOption.Default:

                                break;

                            case SelectionOption.SelectPage:

                                widgetsContainer.SelectAllWidgets(true, selectionCallback =>
                                {
                                    if (Helpers.IsSuccessCode(selectionCallback.resultsCode))
                                    {
                                        if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                                            ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(WidgetType.SelectionOptionsWidget);
                                        else
                                            LogWarning("On Widget Action Event Screen Manager Get Current Screen Data Value Is Null", this, () => OnSelection_ActionEvents(dataPackets));


                                        //Check This Man And Below
                                        widgetsContainer.OnFocusedSelectionStateUpdate();
                                    }
                                    else
                                        LogError(selectionCallback.results, this, () => OnSelection_ActionEvents(dataPackets));
                                });

                                break;

                            case SelectionOption.SelectAll:

                                widgetsContainer.SelectAllWidgets(false, selectionCallback =>
                                {
                                    if (selectionCallback.Success())
                                    {
                                        if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                                            ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(WidgetType.SelectionOptionsWidget);
                                        else
                                            LogWarning("On Widget Action Event Screen Manager Get Current Screen Data Value Is Null", this, () => OnSelection_ActionEvents(dataPackets));

                                        if (widgetsContainer.GetContentCount() == SelectableManager.Instance.GetFocusedSelectionDataCount())
                                        {
                                            switch (SceneAssetsManager.Instance.GetLayoutViewType())
                                            {
                                                case LayoutViewType.ItemView:

                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(InputActionButtonType.SelectionOptionsButton, UIImageDisplayerType.ButtonIcon, UIImageType.ItemViewDeselectionIcon);

                                                    break;

                                                case LayoutViewType.ListView:

                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(InputActionButtonType.SelectionOptionsButton, UIImageDisplayerType.ButtonIcon, UIImageType.ListViewDeselectionIcon);

                                                    break;
                                            }
                                        }

                                        //Check This Man And Above
                                        widgetsContainer.OnFocusedSelectionStateUpdate();
                                    }
                                    else
                                        LogError(selectionCallback.results, this, () => OnSelection_ActionEvents(dataPackets));
                                });

                                break;
                        }
                    }
                    else
                        LogError("Widgets Container Not Found", this, () => OnSelection_ActionEvents(dataPackets));
                }
                else
                    LogError("Scene Assets Manager / Screen UI Manager Instance Is Not Yet Initialized", this, () => OnSelection_ActionEvents(dataPackets));
            }

            void OnDelete_ActionEvent(SceneDataPackets dataPackets)
            {
                int selectionCount = SelectableManager.Instance.GetFocusedSelectionDataCount();

                if (selectionCount > 0)
                {
                    if (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetPaginationViewType() == PaginationViewType.Pager)
                    {
                        List<UIScreenWidget<SceneDataPackets>> currentPage = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.Pagination_GetCurrentPage();

                        if (currentPage != null && currentPage.Count > 0)
                        {
                            List<UIScreenWidget<SceneDataPackets>> selectedWidgets = new List<UIScreenWidget<SceneDataPackets>>();

                            foreach (var selectedWidget in currentPage)
                                if (selectedWidget.IsSelected())
                                    selectedWidgets.Add(selectedWidget);

                            if (selectedWidgets.Count == 0)
                            {
                                SceneAssetsManager.Instance.GetSortedWidgetsFromList(SelectableManager.Instance.GetFolderStructureSelectionData().GetCurrentSelections(), getFolderStructureSelectionData =>
                                {
                                    if (Helpers.IsSuccessCode(getFolderStructureSelectionData.resultsCode))
                                    {
                                        int lastSelectionIndex = getFolderStructureSelectionData.data.Count - 1;
                                        UIScreenWidget<SceneDataPackets> lastSelectedWidget = getFolderStructureSelectionData.data[lastSelectionIndex];

                                        if (lastSelectedWidget != null)
                                            SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.Pagination_GoToItemPage(lastSelectedWidget);
                                        else
                                            LogWarning("Show Delete Asset Widget Failed - Last Selected Widget Not Found.", this, () => OnDelete_ActionEvent(dataPackets));
                                    }
                                    else
                                        LogWarning(getFolderStructureSelectionData.results, this, () => OnDelete_ActionEvent(dataPackets));
                                });
                            }
                        }
                        else
                            LogWarning("Show Delete Asset Widget Failed - Current Page Not Found.", this, () => OnDelete_ActionEvent(dataPackets));

                        if (selectionCount == 1)
                        {
                            string assetName = SelectableManager.Instance.GetFolderStructureSelectionData().GetCurrentSelections()[0].name;
                            string formattedAssetName = assetName.Replace("_FolderData", "");
                            bool hasContent = false;
                            SelectableAssetType assetType = SelectableManager.Instance.GetFolderStructureSelectionData().GetCurrentSelections()[0].GetSelectableAssetType();

                            if (assetType == SelectableAssetType.Folder)
                            {
                                int contentCount = SelectableManager.Instance.GetFolderStructureSelectionData().GetCurrentSelections()[0].GetFolderData().GetFileCount();
                                hasContent = contentCount > 0;
                            }

                            string fileType = (assetType == SelectableAssetType.Folder) ? "folder" : "asset";
                            string hasContentInfo = (hasContent) ? "with its content" : "";
                            dataPackets.widgetTitle = (assetType == SelectableAssetType.Folder) ? "Delete Folder" : "Delete Asset";
                            dataPackets.popUpMessage = $"Are you sure you want to remove {fileType} { "'" + formattedAssetName + "'"} {hasContentInfo} permanently?";
                        }

                        if (selectionCount > 1)
                        {
                            int fileCount = 0;
                            int folderCount = 0;

                            bool hasMultipleAssetTypes = false;

                            string messageTitle = string.Empty;
                            string messageInfo = string.Empty;

                            foreach (var item in SelectableManager.Instance.GetFolderStructureSelectionData().GetCurrentSelections())
                            {
                                if (item.GetSelectableAssetType() == SelectableAssetType.Folder)
                                    folderCount++;

                                if (item.GetSelectableAssetType() == SelectableAssetType.File)
                                    fileCount++;
                            }

                            if (fileCount > 0 && folderCount > 0)
                                hasMultipleAssetTypes = true;

                            if (hasMultipleAssetTypes)
                            {
                                string files = (fileCount > 1) ? "Files" : "File";
                                string folders = (folderCount > 1) ? "Folders" : "Folder";

                                messageTitle = "Delete Selections";
                                messageInfo = $"Are you sure you want to remove {fileCount} {files} and {folderCount} {folders} permanently?";
                            }
                            else
                            {
                                if (folderCount > 0 && fileCount == 0)
                                {
                                    messageTitle = "Delete Folders";
                                    messageInfo = $"Are you sure you want to remove {folderCount} folders permanently?";
                                }

                                if (fileCount > 0 && folderCount == 0)
                                {
                                    messageTitle = $"Delete Assets";
                                    messageInfo = $"Are you sure you want to remove {fileCount} assets permanently?";
                                }
                            }


                            dataPackets.widgetTitle = messageTitle;
                            dataPackets.popUpMessage = messageInfo;
                        }
                    }

                    if (showWidgetAsyncRoutine != null)
                    {
                        StopCoroutine(showWidgetAsyncRoutine);
                        showWidgetAsyncRoutine = null;
                    }

                    if (showWidgetAsyncRoutine == null)
                        showWidgetAsyncRoutine = StartCoroutine(ShowWidgetAsync(dataPackets));
                }
                else
                    LogWarning("Show Delete Asset Widget Failed - There Are No Selections Found.", this, () => OnDelete_ActionEvent(dataPackets));
            }

            void OnOpenFilePicker_ActionEvent(WidgetType popUpType, SceneDataPackets dataPackets) => ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(popUpType, dataPackets);

            void OnPinItem_ActionEvent(SceneDataPackets dataPackets)
            {
                var pinData = SelectableManager.Instance.HasPinnedSelection(SelectableManager.Instance.GetFolderStructureSelectionData().GetCurrentSelections());
                DefaultUIWidgetActionState widgetActionState = DefaultUIWidgetActionState.Default;

                if (pinData.pinned)
                    widgetActionState = DefaultUIWidgetActionState.Default;
                else
                    widgetActionState = DefaultUIWidgetActionState.Pinned;

                int pinItemsCount = SelectableManager.Instance.GetFocusedSelectionDataCount();

                if (pinItemsCount > 0)
                {
                    var currentSelection = SelectableManager.Instance.GetFolderStructureSelectionData().GetCurrentSelections();

                    SceneAssetsManager.Instance.SetDefaultUIWidgetActionState(currentSelection, widgetActionState, assetsPinnedCallback =>
                    {
                        if (Helpers.IsSuccessCode(assetsPinnedCallback.resultsCode))
                        {
                            ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(InputActionButtonType.PinButton, UIImageDisplayerType.ButtonIcon, (widgetActionState == DefaultUIWidgetActionState.Pinned) ? UIImageType.PinDisabledIcon : UIImageType.PinEnabledIcon);

                            if (dataPackets.showNotification)
                            {
                                if (pinItemsCount == 1)
                                {
                                    string assetName = SceneAssetsManager.Instance.GetFormattedName(currentSelection[0].name, currentSelection[0].GetSelectableAssetType());
                                    dataPackets.notification.message = (widgetActionState == DefaultUIWidgetActionState.Pinned) ? $"{assetName} Pinned" : $"{assetName} Removed From Pinned Items";
                                }

                                if (pinItemsCount > 1)
                                    dataPackets.notification.message = (widgetActionState == DefaultUIWidgetActionState.Pinned) ? $"{pinItemsCount} Items Pinned" : $"{pinItemsCount} Items Removed From Pinned";

                                SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetContent<SceneDataPackets>(contentLoadedCallback =>
                                {
                                    if (Helpers.IsSuccessCode(contentLoadedCallback.resultsCode))
                                    {
                                        SceneAssetsManager.Instance.SortScreenWidgets(contentLoadedCallback.data);

                                        var lastSelectionWidget = currentSelection.FindLast(x => x.GetActive());

                                        if (lastSelectionWidget != null)
                                            SelectableManager.Instance.Select(lastSelectionWidget.name, FocusedSelectionType.SelectedItem);
                                        else
                                            LogError("Last Selected Widget Missing / Not Found", this, () => OnPinItem_ActionEvent(dataPackets));

                                        ScreenUIManager.Instance.Refresh();

                                        if (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetPaginationViewType() == PaginationViewType.Pager)
                                            StartCoroutine(GoToItemPageAsync(currentSelection[pinItemsCount - 1].name));

                                        if (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetPaginationViewType() == PaginationViewType.Scroller)
                                            StartCoroutine(SctollToItemAsync(currentSelection[pinItemsCount - 1].name));

                                        NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);
                                    }
                                    else
                                        LogWarning(contentLoadedCallback.results, this, () => OnPinItem_ActionEvent(dataPackets));
                                });
                            }
                        }
                        else
                            LogWarning(assetsPinnedCallback.results, this, () => OnPinItem_ActionEvent(dataPackets));
                    });
                }
                else
                    LogWarning("Pin Button Failed - There Are No Selections Found.", this, () => OnPinItem_ActionEvent(dataPackets));
            }

            void OnBuildNewAsset_ActionEvent(WidgetType popUpType, SceneDataPackets dataPackets)
            {
                #region Double Check

                Folder currentFolder = SceneAssetsManager.Instance.GetCurrentFolder();

                if (!string.IsNullOrEmpty(currentFolder.storageData.directory))
                {
                    SceneAssetsManager.Instance.DirectoryFound(currentFolder.storageData.directory, directoryFoundCallback =>
                    {
                        if (Helpers.IsSuccessCode(directoryFoundCallback.resultsCode))
                        {
                            SceneAssetsManager.Instance.BuildSceneAsset(currentFolder.storageData, (assetBuiltCallback) =>
                            {
                                if (Helpers.IsSuccessCode(assetBuiltCallback.resultsCode))
                                {
                                    if (ScreenUIManager.Instance != null)
                                    {
                                        ActionEvents.OnPopUpActionEvent(popUpType, InputActionButtonType.HideScreenWidget, dataPackets);
                                        ScreenUIManager.Instance.ShowScreen(currentDataPackets);
                                    }
                                    else
                                        LogWarning("Screen Manager Missing.", this, () => OnBuildNewAsset_ActionEvent(popUpType, dataPackets));
                                }
                                else
                                    LogWarning(assetBuiltCallback.results, this, () => OnBuildNewAsset_ActionEvent(popUpType, dataPackets));
                            });
                        }
                        else
                            LogWarning(directoryFoundCallback.results, this, () => OnBuildNewAsset_ActionEvent(popUpType, dataPackets));
                    });
                }
                else
                    LogWarning("Build New Asset Failed - Current Directory Data Is Null.", this, () => OnBuildNewAsset_ActionEvent(popUpType, dataPackets));

                #endregion
            }

            void OnHideScreenWidget_ActionEvent(WidgetType popUpType, SceneDataPackets dataPackets)
            {
                try
                {
                    if (popUpType == WidgetType.SceneAssetPreviewWidget)
                    {
                        SceneAssetsManager.Instance.OnClearPreviewedContent(false, contentClrearedCallback =>
                        {
                            if (Helpers.IsSuccessCode(contentClrearedCallback.resultsCode))
                            {
                                SceneAssetsManager.Instance.SetCurrentSceneAsset(SceneAssetsManager.Instance.GetSceneAssets()[0]);
                                ScreenUIManager.Instance.ShowScreen(dataPackets);

                                ScreenUIManager.Instance.Refresh();
                            }
                            else
                                LogError(contentClrearedCallback.results, this);
                        });

                        //dataPackets.sceneAsset = null;
                        //SceneAssetsManager.Instance.OnSceneAssetPreviewMode(dataPackets);
                    }

                    if (popUpType == WidgetType.PermissionsRequestWidget)
                    {
                        if (dataPackets.canTransitionSceneAsset)
                        {
                            if (!SelectableManager.Instance.HasAssetSelected() && !SelectableManager.Instance.HasSelection())
                                ActionEvents.OnTransitionSceneEventCamera(dataPackets);
                            else
                                LogWarning("There's Still A Selection Active.", this, () => OnHideScreenWidget_ActionEvent(popUpType, dataPackets));
                        }
                    }

                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(popUpType, dataPackets);
                    else
                        LogWarning("Failed To Close Pop Up Because Screen Manager's Get Current Screen Data Value Is Null.", this, () => OnHideScreenWidget_ActionEvent(popUpType, dataPackets));

                    if (popUpType == WidgetType.SnapShotWidget)
                    {
                        // Show Notification
                        Notification notification = new Notification
                        {
                            message = "Picture Discarded Successfully",
                            notificationType = NotificationType.Info,
                            screenType = UIScreenType.ProjectViewScreen,
                            screenPosition = SceneAssetPivot.TopCenter,
                            blurScreen = true,
                            delay = SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.NotificationDelay).value,
                            duration = SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.NotificationDuration).value // Get From Value List In Scene Assets Manager.
                        };

                        NotificationSystemManager.Instance.ScheduleNotification(notification);
                    }

                    if (popUpType == WidgetType.FolderCreationWidget)
                    {
                        if (SceneAssetsManager.Instance.GetLoadedSceneAssetsList().Count == 0)
                            ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(ScreenNavigationManager.Instance.GetEmptyFolderDataPackets());
                    }
                }
                catch (Exception exception)
                {
                    LogError($"Failed To Close Widget With Exception : {exception.Message}", this, () => OnHideScreenWidget_ActionEvent(popUpType, dataPackets));
                    throw exception;
                }
            }

            void OnOpenARView_ActionEvent(SceneDataPackets dataPackets)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (AssetImportContentManager.Instance != null)
                    {
                        Debug.Log($"RG_Unity : UserRequestedAppPermissions Called From Unity - Instance Initialized - Not Null.");

                        if (AssetImportContentManager.Instance.IsCameraPermissionsGranted())
                        {
                            Debug.Log($"RG_Unity : UserRequestedAppPermissions Called From Unity - Permission Granted");

                            ScreenUIManager.Instance.ShowScreen(dataPackets);

                            if (SceneAssetsManager.Instance)
                                SceneAssetsManager.Instance.SetCurrentSceneMode(dataPackets.sceneMode);
                            else
                                LogWarning("Scene Assets Not Yet Initialized.", this, () => OnOpenARView_ActionEvent(dataPackets));
                        }
                        else
                        {
                            if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                                ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);
                            else
                                LogWarning("On Widget Action Event Screen Manager Get Current Screen Data Value Is Null", this, () => OnOpenARView_ActionEvent(dataPackets));

                            AssetImportContentManager.Instance.SetRequestedPermissionData(dataPackets);
                        }
                    }
                    else
                        LogWarning("Asset Import Content Manager Not Yet Initialized.", this, () => OnOpenARView_ActionEvent(dataPackets));
                }
                else
                {
                    if (AssetImportContentManager.Instance != null)
                    {
                        if (AssetImportContentManager.Instance.ShowPermissionDialogue())
                        {
                            SceneAssetsManager.Instance.SetCurrentSceneMode(dataPackets.sceneMode);

                            if (!SelectableManager.Instance.HasAssetSelected() && !SelectableManager.Instance.HasSelection())
                                ActionEvents.OnTransitionSceneEventCamera(dataPackets);
                            else
                                LogWarning("There's Still A Selection Active.", this, () => OnOpenARView_ActionEvent(dataPackets));

                            if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                                ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);
                            else
                                LogWarning("On Widget Action Event Screen Manager Get Current Screen Data Value Is Null", this, () => OnOpenARView_ActionEvent(dataPackets));

                            AssetImportContentManager.Instance.SetRequestedPermissionData(dataPackets);
                        }
                        else
                        {
                            ScreenUIManager.Instance.ShowScreen(dataPackets);
                            SceneAssetsManager.Instance.SetCurrentSceneMode(dataPackets.sceneMode);
                        }
                    }
                    else
                        LogWarning("Asset Import Content Manager Not Yet Initialized.", this, () => OnOpenARView_ActionEvent(dataPackets));
                }
            }

            void OnConfirm_ActionEvent(WidgetType popUpType, SceneDataPackets dataPackets)
            {
                switch (dataPackets.widgetType)
                {
                    case WidgetType.PermissionsRequestWidget:

                        OnPermissionsReques_ActionEvents(popUpType, dataPackets);

                        break;

                    case WidgetType.DeleteAssetWidget:

                        OnDeleteAssetWidget_ActionEvent(dataPackets);

                        break;

                    case WidgetType.SceneAssetExportWidget:

                        OnSceneAssetExport_ActionEvent(popUpType, dataPackets);

                        break;

                    case WidgetType.AssetPublishingWidget:

                        OnAssetPublishing_ActionEvent(dataPackets);

                        break;

                    case WidgetType.NetworkNotificationWidget:

                        OnNetworkNotification_ActionEvent(dataPackets);

                        break;

                    case WidgetType.FolderCreationWidget:

                        OnCreateNewFolder_ActionEvent(dataPackets);

                        break;
                }
            }

            void OnAssetPublishing_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(dataPackets.widgetType, dataPackets);

                    if (PublishingManager.Instance != null)
                        PublishingManager.Instance.Publish();
                    else
                        LogWarning("Asset Publishing Failed : Publishing Manager Instance Is Not Yet Initialized.", this, () => OnAssetPublishing_ActionEvent(dataPackets));
                }
                else
                    LogError("Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnAssetPublishing_ActionEvent(dataPackets));
            }

            void OnNetworkNotification_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(dataPackets.widgetType, dataPackets);

                    if (PublishingManager.Instance != null)
                        PublishingManager.Instance.Publish();
                    else
                        LogWarning("Asset Publishing Failed : Publishing Manager Instance Is Not Yet Initialized.", this, () => OnNetworkNotification_ActionEvent(dataPackets));
                }
                else
                    LogWarning("Asset Publishing Failed : Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnNetworkNotification_ActionEvent(dataPackets));
            }

            void OnSceneAssetExport_ActionEvent(WidgetType popUpType, SceneDataPackets dataPackets)
            {
                if (AssetImportContentManager.Instance != null)
                    if (SceneAssetsManager.Instance != null)
                        if (SceneAssetsManager.Instance.GetCurrentAssetExportData().value != null)
                            AssetImportContentManager.Instance.ExportAsset(SceneAssetsManager.Instance.GetCurrentAssetExportData());
                        else
                            LogWarning("Export Asset Failed : Scene Assets Manager Instance's Get Current Asset Export Data Value Is Missing / Null.", this, () => OnSceneAssetExport_ActionEvent(popUpType, dataPackets));
                    else
                        LogWarning("Export Asset Failed : Scene Assets Manager Instance Is Not Yet Initialized.", this, () => OnSceneAssetExport_ActionEvent(popUpType, dataPackets));
                else
                    LogWarning("Export Asset Failed : Asset Import Content Manager Instance Is Not Yet Initialized.", this, () => OnSceneAssetExport_ActionEvent(popUpType, dataPackets));


                if (ScreenUIManager.Instance != null)
                    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(popUpType, dataPackets);
                else
                    LogWarning("Export Asset Failed : Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnSceneAssetExport_ActionEvent(popUpType, dataPackets));
            }

            void OnPermissionsReques_ActionEvents(WidgetType popUpType, SceneDataPackets dataPackets)
            {
                try
                {
                    // Requesting Permissions
                    if (AssetImportContentManager.Instance)
                        AssetImportContentManager.Instance.UserRequestedAppPermissions(AssetImportContentManager.Instance.GetRequestedPermissionData());
                    else
                        LogWarning("Asset Import Content Manager Not Yet initialized.", this, () => OnPermissionsReques_ActionEvents(popUpType, dataPackets));

                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                        ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(popUpType, AssetImportContentManager.Instance.GetRequestedPermissionData());
                    else
                        LogWarning("Failed To Close Pop Up Because Screen Manager Is Not Yet Initialized.", this, () => OnPermissionsReques_ActionEvents(popUpType, dataPackets));

                    SceneAssetsManager.Instance.SetCurrentSceneAsset(SceneAssetsManager.Instance.GetSceneAssets()[0]);

                    ScreenUIManager.Instance.ShowScreen(AssetImportContentManager.Instance.GetRequestedPermissionData());

                    SceneAssetsManager.Instance.SetCurrentSceneMode(AssetImportContentManager.Instance.GetRequestedPermissionData().sceneMode);

                    #if UNITY_EDITOR

                    if (AssetImportContentManager.Instance.ShowPermissionDialogue())
                    {
                        SceneAssetsManager.Instance.SetCurrentSceneAsset(SceneAssetsManager.Instance.GetSceneAssets()[0]);
                        ScreenUIManager.Instance.ShowScreen(AssetImportContentManager.Instance.GetRequestedPermissionData());
                        SceneAssetsManager.Instance.SetCurrentSceneMode(AssetImportContentManager.Instance.GetRequestedPermissionData().sceneMode);
                    }

                    #endif
                }
                catch (Exception exception)
                {
                    LogError($"Failed To Close Widget With Exception : {exception.Message}", this, () => OnPermissionsReques_ActionEvents(popUpType, dataPackets));
                    throw exception;
                }
            }

            void OnDeleteAssetWidget_ActionEvent(SceneDataPackets dataPackets)
            {
                try
                {
                    if (SelectableManager.Instance.HasActiveSelection())
                    {
                        int deletedFileCount = 0;
                        var selectedWidgets = SelectableManager.Instance.GetFolderStructureSelectionData().GetCurrentSelections();

                        deletedFileCount = selectedWidgets.Count;

                        if (deletedFileCount > 0)
                            SceneAssetsManager.Instance.OnDelete(selectedWidgets, deletedAssetsCallback =>
                            {
                                if (Helpers.IsSuccessCode(deletedAssetsCallback.resultsCode))
                                {
                                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                                        ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(dataPackets.widgetType, dataPackets);
                                    else
                                        LogWarning("Failed To Close Pop Up Because Screen Manager Is Not Yet Initialized.", this, () => OnDeleteAssetWidget_ActionEvent(dataPackets));

                                    ScreenUIManager.Instance.Refresh();

                                    if (dataPackets.showNotification)
                                    {
                                        if (deletedFileCount == 1)
                                        {
                                            string assetName = SceneAssetsManager.Instance.GetAssetNameFormatted(selectedWidgets[0].name, selectedWidgets[0].GetSelectableAssetType());
                                            dataPackets.notification.message = $"{assetName} Deleted";
                                        }

                                        if (deletedFileCount > 1)
                                            dataPackets.notification.message = $"{deletedFileCount} Files Deleted";

                                        NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);
                                    }
                                }
                                else
                                    LogWarning(deletedAssetsCallback.results, this, () => OnDeleteAssetWidget_ActionEvent(dataPackets));
                            });
                        else
                            LogWarning("Delete Assets Failed - No Assets To Delete Found.", this, () => OnDeleteAssetWidget_ActionEvent(dataPackets));
                    }
                    else
                        LogWarning("Delete Assets Failed - There Are No Selections Found.", this, () => OnDeleteAssetWidget_ActionEvent(dataPackets));

                }
                catch (Exception exception)
                {
                    LogError($"Failed To Delete Asset With Exception : {exception.Message}", this, () => OnDeleteAssetWidget_ActionEvent(dataPackets));
                    throw exception;
                }
            }

            void OnCreateNewFolder_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(dataPackets.widgetType, dataPackets);

                    SceneAssetsManager.Instance.CreateNewFolderStructureData((folderCreated) =>
                    {
                        if (Helpers.IsSuccessCode(folderCreated.resultsCode))
                        {
                            if (SelectableManager.Instance)
                            {
                                SelectableManager.Instance.OnSetFocusedWidgetSelectionInfo(folderCreated.data, true, selectionInfoSet =>
                                {
                                    if (Helpers.IsSuccessCode(selectionInfoSet.resultsCode))
                                    {
                                        var widgetsContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                                        if(widgetsContainer != null)
                                        {
                                            // Reload Screen
                                            ScreenUIManager.Instance.Refresh();

                                            if(widgetsContainer.GetPaginationViewType() == PaginationViewType.Scroller)
                                            {
                                                if (dataPackets.showNotification)
                                                    NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);
                                            }

                                            if (widgetsContainer.GetPaginationViewType() == PaginationViewType.Pager)
                                            {
                                                var widget = widgetsContainer.GetWidgetNamed(folderCreated.data.name);

                                                if (widget != null)
                                                {
                                                    widgetsContainer.Pagination_GoToItemPageAsync(widget, goToPageCallback =>
                                                    {
                                                        if (Helpers.IsSuccessCode(goToPageCallback.resultsCode))
                                                        {
                                                            if (dataPackets.showNotification)
                                                                NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);
                                                        }
                                                        else
                                                            LogError(goToPageCallback.results, this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                                                    });
                                                }
                                                else
                                                    LogError($"Widget Named : {folderCreated.data.name} Not Found.", this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                                            }
                                        }
                                        else
                                            LogError($"Widgets Container Not Found.", this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                                    }
                                    else
                                        LogWarning(selectionInfoSet.results, this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                                });
                            }
                            else
                                LogError($"Couldn't Create New Folder - Selectable Manager Instance Is Not Yet Initialized", this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                        }
                        else
                            LogWarning(folderCreated.results, this, () => OnCreateNewFolder_ActionEvent(dataPackets));
                    });
                }
                else
                    LogWarning("Asset Publishing Failed : Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnCreateNewFolder_ActionEvent(dataPackets));
            }

            void OnCancel_ActionEvent(SceneDataPackets dataPackets)
            {
                switch (dataPackets.widgetType)
                {
                    case WidgetType.FileSelectionOptionsWidget:

                        if (SelectableManager.Instance.HasActiveSelection())
                            SelectableManager.Instance.OnDeselectAll();

                        if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                            ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(dataPackets.widgetType, dataPackets);
                        else
                            LogWarning("Failed To Close Pop Up Because Screen Manager Is Not Yet Initialized.", this, () => OnCancel_ActionEvent(dataPackets));

                        break;
                }
            }

            void OnResetAssetPreviewPose_ActionEvent(SceneAssetModeType modeType) => ActionEvents.OnResetSceneAssetPreviewPoseEvent(modeType);

            void OnExportAsset_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);
                }
                else
                    LogWarning("Asset Export Failed : Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnExportAsset_ActionEvent(dataPackets));
            }

            void OnOpenRendererSettings_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);
                }
                else
                    LogWarning("Asset Export Failed : Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnOpenRendererSettings_ActionEvent(dataPackets));
            }

            void OnPublishAsset_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);
                }
                else
                    LogWarning("Asset Export Failed : Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnPublishAsset_ActionEvent(dataPackets));
            }

            void OnCaptureSnapShot_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenCaptureManager.Instance != null)
                {
                    ScreenCaptureManager.Instance.CaptureScreen((onScreenCaptured) =>
                    {
                        if (Helpers.IsSuccessCode(onScreenCaptured.resultsCode))
                        {
                            if (ScreenUIManager.Instance != null)
                            {
                                ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);
                            }
                            else
                                LogWarning("Asset Export Failed : Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnCaptureSnapShot_ActionEvent(dataPackets));
                        }
                        else
                            LogWarning(onScreenCaptured.results, this, () => OnCaptureSnapShot_ActionEvent(dataPackets));
                    });
                }
                else
                    LogWarning("Asset Export Failed : Screen UI Manager Instance Is Not Yet Initialized.", this, () => OnCaptureSnapShot_ActionEvent(dataPackets));
            }

            void OnScrollToTop_ActionEvent()
            {
                if (ScreenUIManager.Instance != null)
                {
                    if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == UIScreenType.ProjectViewScreen)
                    {
                        SceneAssetsManager.Instance.GetDynamicWidgetsContainer(ContentContainerType.FolderStuctureContent, dynamicWidgetsContainer =>
                        {
                            if (Helpers.IsSuccessCode(dynamicWidgetsContainer.resultsCode))
                                dynamicWidgetsContainer.data.ScrollToTop();
                            else
                                LogWarning(dynamicWidgetsContainer.results, this, () => OnScrollToTop_ActionEvent());
                        });
                    }
                    else
                        LogWarning("Not Asset Screen.", this, () => OnScrollToTop_ActionEvent());
                }
                else
                    LogError("Screen UI Manager Instance Is Not Yet Initialized", this, () => OnScrollToTop_ActionEvent());
            }

            void OnScrollToBottom_ActionEvent()
            {
                if (ScreenUIManager.Instance != null)
                {
                    if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == UIScreenType.ProjectViewScreen)
                    {
                        SceneAssetsManager.Instance.GetDynamicWidgetsContainer(ContentContainerType.FolderStuctureContent, dynamicWidgetsContainer =>
                        {
                            if (Helpers.IsSuccessCode(dynamicWidgetsContainer.resultsCode))
                                dynamicWidgetsContainer.data.ScrollToBottom();
                            else
                                LogWarning(dynamicWidgetsContainer.results, this, () => OnScrollToBottom_ActionEvent());
                        });
                    }
                    else
                        LogWarning("Not Asset Screen.", this, () => OnScrollToBottom_ActionEvent());
                }
                else
                    LogError("Screen UI Manager Instance Is Not Yet Initialized", this, () => OnScrollToBottom_ActionEvent());
            }

            void OnPaginationNavigation_ActionEvent(PaginationNavigationActionType actionType) => SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.OnPaginationActionButtonPressed(actionType);

            void OnFolderActions_ActionEvent(SceneDataPackets dataPackets)
            {
                if (ScreenUIManager.Instance != null)
                {
                    if (dataPackets.folderStructureType == FolderStructureType.MainFolder)
                    {
                        ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(WidgetType.UITextDisplayerWidget);

                        SceneDataPackets packets = dataPackets;
                        packets.widgetType = WidgetType.FolderCreationWidget;

                        if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                        {
                            if (SceneAssetsManager.Instance)
                            {
                                var widgetContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                                if (widgetContainer != null)
                                {
                                    widgetContainer.GetPlaceHolder(placeholder =>
                                    {
                                        if (Helpers.IsSuccessCode(placeholder.resultsCode))
                                        {
                                            if (!placeholder.data.IsActive())
                                            {
                                                placeholder.data.ShowPlaceHolder(widgetContainer.GetContentContainer(), widgetContainer.GetCurrentLayoutWidgetDimensions(), widgetContainer.GetLastContentIndex(), true);

                                                ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(packets);
                                                //StartCoroutine(OnShowWidgetAsync(WidgetType.UITextDisplayerWidget, actionButton.dataPackets));
                                            }
                                        }
                                        else
                                            LogWarning(placeholder.results, this, () => OnFolderActions_ActionEvent(dataPackets));
                                    });
                                }
                                else
                                    LogWarning("Get Place Holder Failed : Widgets Container Is Missing / Null.", this, () => OnFolderActions_ActionEvent(dataPackets));
                            }
                            else
                                LogWarning("Get Place holder Failed : Scene Assets Manager Instance Is Not Yet Initialized", this, () => OnFolderActions_ActionEvent(dataPackets));
                        }
                        else
                            LogWarning("Screen UI Manager Instance Get Current Screen Data Failed : Value Is Missing / Null.", this, () => OnFolderActions_ActionEvent(dataPackets));
                    }
                    else
                    {
                        LogError("Delete Folder Action - Delete Folder Widget", this, () => OnFolderActions_ActionEvent(dataPackets));
                    }
                }
                else
                    LogError("Screen UI Manager Instance Is Not Yet Initialized", this, () => OnFolderActions_ActionEvent(dataPackets));
            }

            void OnDeselect_ActionEvent(SceneDataPackets dataPackets)
            {
                if (SelectableManager.Instance)
                {
                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                    {
                        SelectableManager.Instance.DeselectAll();
                        ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(dataPackets.widgetType);
                    }
                    else
                        LogWarning("On Widget Action Event Screen Manager Get Current Screen Data Value Is Null", this);
                }
                else
                    LogError("Selectable Manager Instance Is Not Yet Initialized.", this);
            }

            #endregion

            #region Events

            void OnWidgetSelectionEvent()
            {
                switch (SceneAssetsManager.Instance.GetLayoutViewType())
                {
                    case LayoutViewType.ItemView:

                        ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(InputActionButtonType.SelectionOptionsButton, UIImageDisplayerType.ButtonIcon, UIImageType.ItemViewDeselectionIcon);

                        break;

                    case LayoutViewType.ListView:

                        ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(InputActionButtonType.SelectionOptionsButton, UIImageDisplayerType.ButtonIcon, UIImageType.ListViewDeselectionIcon);

                        break;
                }
            }

            #endregion

            IEnumerator ShowWidgetAsync(SceneDataPackets dataPackets)
            {
                yield return new WaitForEndOfFrame();
                //yield return new WaitForSeconds(1.0f);
                ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);
            }

            void UndoChanges()
            {
                if (type == WidgetType.SliderValueWidget)
                {
                    if (sliderWidget.slider != null)
                    {
                        sliderWidget.slider.value = sliderWidget.defaultFieldValue;
                    }
                    else
                    {
                        LogWarning("Slider Value Pop Up Handler Component Required.", this, () => UndoChanges());
                    }
                }
            }

            void ShowInfo()
            {
                LogInfo("Show Info.", this, () => ShowInfo());
            }

            protected WidgetLayoutView GetLayoutView()
            {
                return widgetLayouts.Find(layout => layout.layoutViewType == defaultLayoutType);
            }

            protected WidgetLayoutView GetLayoutView(WidgetLayoutViewType layoutViewType)
            {
                return widgetLayouts.Find(layout => layout.layoutViewType == layoutViewType);
            }

            public void OnScrollerValueChangedEvent(Vector2 value) => OnScrollerValueChanged(value);

            public void SetOnInputValueChanged(string value, SceneDataPackets dataPackets) => OnInputFieldValueChanged(value, dataPackets);

            public void SetOnInputValueChanged(int value, SceneDataPackets dataPackets) => OnInputFieldValueChanged(value, dataPackets);

            public void SetOnCheckboxValueChanged(CheckboxInputActionType actionType, bool value, SceneDataPackets dataPackets) => OnCheckboxValueChanged(actionType, value, dataPackets);

            public void ShowScreenWidget(SceneDataPackets dataPackets)
            {
                if (widgetLayouts.Count > 0)
                {
                    if (titleDisplayer != null)
                        titleDisplayer.text = dataPackets.widgetTitle;

                    switch (dataPackets.widgetType)
                    {
                        case WidgetType.ConfirmationPopWidget:

                            Debug.Log($"---> Showing Widget For : {dataPackets.sceneAsset.name}");

                            break;

                        case WidgetType.SliderValueWidget:

                            if (sliderWidget.slider)
                            {
                                if (SceneAssetsManager.Instance != null)
                                {
                                    if (SceneAssetsManager.Instance.GetCurrentSceneAsset().modelAsset)
                                    {
                                        switch (dataPackets.assetFieldConfiguration)
                                        {
                                            case AssetFieldSettingsType.MainTextureSettings:

                                                Debug.Log($"---> Material Properties : {dataPackets.assetFieldConfiguration.ToString()} - Value : {SceneAssetsManager.Instance.GetCurrentSceneAsset().GetMaterialProperties().glossiness}");
                                                sliderWidget.SetSliderValue(SceneAssetsManager.Instance.GetCurrentSceneAsset().GetMaterialProperties().glossiness, SliderValueType.MaterialGlossinessValue);

                                                break;

                                            case AssetFieldSettingsType.NormalMapSettings:

                                                Debug.Log($"---> Material Properties : {dataPackets.assetFieldConfiguration.ToString()} - Value : {SceneAssetsManager.Instance.GetCurrentSceneAsset().GetMaterialProperties().bumpScale}");
                                                sliderWidget.SetSliderValue(SceneAssetsManager.Instance.GetCurrentSceneAsset().GetMaterialProperties().bumpScale, SliderValueType.MaterialBumpScaleValue);

                                                break;

                                            case AssetFieldSettingsType.AOMapSettings:

                                                Debug.Log($"---> Material Properties : {dataPackets.assetFieldConfiguration.ToString()} - Value : {SceneAssetsManager.Instance.GetCurrentSceneAsset().GetMaterialProperties().aoStrength}");
                                                sliderWidget.SetSliderValue(SceneAssetsManager.Instance.GetCurrentSceneAsset().GetMaterialProperties().aoStrength, SliderValueType.MaterialOcclusionIntensityValue);

                                                break;
                                        }
                                    }
                                    else
                                        LogWarning("Current Scene Asset Model Is Null / Invalid.", this, () => ShowScreenWidget(dataPackets));
                                }
                                else
                                    LogError("Scene Assets Manager Not Yet Initialized.", this, () => ShowScreenWidget(dataPackets));
                            }
                            else
                                LogInfo($"Slider Value For : {gameObject.name} Is Null.", this, () => ShowScreenWidget(dataPackets));

                            break;

                        case WidgetType.WarningPromptWidget:

                            // Check If Data Exists.
                            //Debug.Log($"---> Showing Warning Prompt Pop Up For : {dataPackets.sceneAsset.name}");

                            break;

                        case WidgetType.SceneAssetPreviewWidget:

                            LogInfo($"Showing Scene Asset Preview Widget For : {dataPackets.sceneAsset.name}", this, () => ShowScreenWidget(dataPackets));

                            break;

                        case WidgetType.UITextDisplayerWidget:



                            break;
                    }

                    currentDataPackets = dataPackets;

                    OnScreenWidget();

                    ShowWidget(transitionType, dataPackets);
                }
                else
                    LogWarning("Pop Up Widgets Layouts Not Assigned - Required.", this, () => ShowScreenWidget(dataPackets));
            }

            protected void ShowSelectedLayout(WidgetLayoutViewType layoutViewType, bool hideAll = true)
            {
                if (hideAll)
                {
                    if (widgetLayouts.Count > 0)
                    {
                        foreach (var layout in widgetLayouts)
                        {
                            if (layout.layoutViewType == layoutViewType)
                                layout.ShowLayout();
                            else
                                layout.HideLayout();
                        }
                    }
                    else
                        LogWarning("Show Selected Layout Failed - Widget Layouts Missing / Not Assigned In The Inspector.", this, () => ShowSelectedLayout(layoutViewType, hideAll = true));
                }
                else
                {
                    WidgetLayoutView layoutView = widgetLayouts.Find(widget => widget.layoutViewType == layoutViewType);

                    if (layoutView.layout)
                        layoutView.ShowLayout();
                }
            }

            protected void HideSelectedLayout(WidgetLayoutViewType layoutViewType, bool hideAll = true)
            {
                if (hideAll)
                {
                    if (widgetLayouts.Count > 0)
                    {
                        foreach (var layout in widgetLayouts)
                            layout.HideLayout();
                    }
                    else
                        LogWarning("Show Selected Layout Failed - Widget Layouts Missing / Not Assigned In The Inspector.", this, () => HideSelectedLayout(layoutViewType, hideAll = true));
                }
                else
                {
                    WidgetLayoutView layoutView = widgetLayouts.Find(widget => widget.layoutViewType == layoutViewType);

                    if (layoutView.layout)
                        layoutView.HideLayout();
                }
            }

            protected void ShowWidget(ScreenWidgetTransitionType transitionType, SceneDataPackets dataPackets)
            {
                if (transitionType == this.transitionType)
                {
                    switch (transitionType)
                    {
                        case ScreenWidgetTransitionType.PopUp:

                            if (dataPackets.widgetScreenPosition != Vector2.zero)
                                if (widgetRect != null)
                                    widgetRect.anchoredPosition = dataPackets.widgetScreenPosition;
                                else
                                    LogWarning("Screen Rect Is Null.", this, () => ShowWidget(transitionType, dataPackets));

                            OnShowScreenWidget(dataPackets);

                            break;

                        case ScreenWidgetTransitionType.Slide:

                            if (widgetRect)
                            {
                                onWidgetTransition = true;
                                showWidget = true;
                            }
                            else
                                LogWarning($"Widget Rect Is Missing / Not Found For Game Object : {gameObject.name}", this, () => ShowWidget(transitionType, dataPackets));

                            break;
                    }
                }
            }

            #region Async Functions

            IEnumerator GoToItemPageAsync(string widgetName)
            {
                yield return new WaitForEndOfFrame();

                int widgetPageIndex = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.Pagination_GetItemPageIndex(widgetName);
                SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.Pagination_GoToPage(widgetPageIndex);
            }

            IEnumerator SctollToItemAsync(string widgetName)
            {
                yield return new WaitForEndOfFrame();

                UIScreenWidget<SceneDataPackets> widget = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetWidgetNamed(widgetName);

                if (widget != null)
                    SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.OnFocusToWidget(widget, true);
                else
                    LogWarning("Widget Is Null.", this, () => SctollToItemAsync(widgetName));
            }

            #endregion

            #region Overrides

            protected abstract void OnScrollerValueChanged(Vector2 value);
            protected abstract void OnInputFieldValueChanged(string value, SceneDataPackets dataPackets);
            protected abstract void OnInputFieldValueChanged(int value, SceneDataPackets dataPackets);
            protected abstract void OnCheckboxValueChanged(CheckboxInputActionType actionType, bool value, SceneDataPackets dataPackets);

            protected abstract void OnScreenWidget();

            protected abstract void OnShowScreenWidget(SceneDataPackets dataPackets);

            protected abstract void OnHideScreenWidget();

            #endregion

            #region Set Action Values

            #region Input

            protected void SetInputFieldValue(string value, InputFieldActionType actionType)
            {
                if (inputs.Count > 0)
                {
                    UIInputField<SceneDataPackets> input = inputs.Find((input) => input.actionType == actionType);

                    if (input.value != null)
                    {
                        if (!string.IsNullOrEmpty(value))
                            input.SetValue(value);
                        else
                            input.OnClearField();
                    }
                    else
                        LogWarning($"Couldn't Find Input Field Of Type : {actionType}", this, () => SetInputFieldValue(value, actionType));
                }
                else
                    LogWarning("Set Input Field Value Failed : No Input Fields Found.", this, () => SetInputFieldValue(value, actionType));
            }

            #endregion

            #region Checkbox

            protected void SetCheckboxValue(bool value, CheckboxInputActionType actionType)
            {
                if (checkboxes.Count > 0)
                {
                    UICheckbox<SceneDataPackets> checkbox = checkboxes.Find((checkbox) => checkbox.actionType == actionType);

                    if (checkbox.value != null)
                        checkbox.SetSelectionState(value);
                    else
                        LogWarning($"Couldn't Find Input Field Of Type : {actionType}", this, () => SetCheckboxValue(value, actionType));
                }
                else
                    LogWarning("Set Input Field Value Failed : No Input Fields Found.", this, () => SetCheckboxValue(value, actionType));
            }

            #endregion

            #endregion

            protected void HighlightInputFieldValue(InputFieldActionType actionType, bool highlight = true)
            {
                if (inputs.Count > 0)
                {
                    UIInputField<SceneDataPackets> input = inputs.Find((input) => input.actionType == actionType);
                    input.OnSelect();
                }
                else
                    LogWarning("Set Input Field Value Failed : No Input Fields Found.", this, () => HighlightInputFieldValue(actionType, highlight = true));
            }

            public void Hide(bool canTransition = true, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                switch (transitionType)
                {
                    case ScreenWidgetTransitionType.PopUp:

                        OnHideScreenWidget();

                        callbackResults.results = "Widget Hidden.";
                        callbackResults.resultsCode = Helpers.SuccessCode;

                        break;

                    case ScreenWidgetTransitionType.Slide:

                        if (widgetRect)
                        {
                            if (canTransition)
                            {
                                onWidgetTransition = true;
                                showWidget = false;

                                if (onWidgetTransition == false)
                                {
                                    callbackResults.results = "Widget Hidden.";
                                    callbackResults.resultsCode = Helpers.SuccessCode;
                                }
                                else
                                {
                                    callbackResults.results = "Widget Not Hidden.";
                                    callbackResults.resultsCode = Helpers.WarningCode;
                                }
                            }
                            else
                            {
                                widgetRect.anchoredPosition = widgetContainer.hiddenScreenPoint.anchoredPosition;
                            }
                        }

                        break;
                }

                callback?.Invoke(callbackResults);
            }

            void OnWidgetTransition()
            {
                if (onWidgetTransition)
                {
                    if (showWidget)
                    {
                        if (widgetRect)
                        {
                            if (!isTransitionState)
                                isTransitionState = true;

                            if (widgetContainer.hiddenScreenPoint != null && widgetContainer.visibleScreenPoint != null)
                            {
                                if (SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.ScreenWidgetTransitionalSpeed).value > 0)
                                {
                                    Vector2 screenPoint = widgetRect.anchoredPosition;

                                    screenPoint = Vector2.Lerp(screenPoint, widgetContainer.visibleScreenPoint.anchoredPosition, SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.ScreenWidgetTransitionalSpeed).value * Time.smoothDeltaTime);
                                    widgetRect.anchoredPosition = screenPoint;

                                    float distance = (widgetRect.anchoredPosition - widgetContainer.visibleScreenPoint.anchoredPosition).sqrMagnitude;

                                    if (distance <= 0.1f)
                                    {
                                        onWidgetTransition = false;
                                        isTransitionState = false;

                                        OnSetWidgetShowingState(true);
                                    }
                                }
                                else
                                    LogWarning($"Scene Assets Manager Instance Get Default Execution Times Is Not Set - Currently {SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.ScreenWidgetTransitionalSpeed).value}.", this, () => OnWidgetTransition());

                            }
                            else
                                LogWarning($"Widget Container Hidden Screen Point | Widget Container visible Screen Point Is Null / Not Assigned In The Editor For Game Object : {gameObject.name}.", this, () => OnWidgetTransition());
                        }
                        else
                            LogWarning("Widget Rect Is Null.", this, () => OnWidgetTransition());

                    }
                    else
                    {
                        if (widgetRect)
                        {
                            if (!isTransitionState)
                                isTransitionState = true;

                            if (widgetContainer.hiddenScreenPoint != null && widgetContainer.visibleScreenPoint != null)
                            {
                                Vector2 screenPoint = widgetRect.anchoredPosition;
                                screenPoint = Vector2.Lerp(screenPoint, widgetContainer.hiddenScreenPoint.anchoredPosition, SceneAssetsManager.Instance.GetDefaultExecutionValue(RuntimeValueType.ScreenWidgetTransitionalSpeed).value * Time.smoothDeltaTime);

                                widgetRect.anchoredPosition = screenPoint;

                                float distance = (widgetRect.anchoredPosition - widgetContainer.hiddenScreenPoint.anchoredPosition).sqrMagnitude;

                                if (distance <= 0.1f)
                                {
                                    onWidgetTransition = false;
                                    isTransitionState = false;

                                    OnSetWidgetShowingState(false);
                                }
                            }
                            else
                                LogWarning("Widget Container Hidden Screen Point | Widget Container Visible Screen Point Is Null / Not Assigned In The Editor.", this, () => OnWidgetTransition());
                        }
                        else
                            LogWarning("Widget Rect Is Null.", this, () => OnWidgetTransition());
                    }
                }
                else
                    return;
            }

            void OnSetWidgetShowingState(bool visible) => isWidgetVisible = visible;

            public bool GetWidgetShowing()
            {
                return isWidgetVisible;
            }

            public void SetAlwaysShowWidget(bool state)
            {
                dontShowAgain = state;
                dontShowAgainToggleField.isOn = state;

                if (SceneAssetsManager.Instance != null)
                {
                    var updatedCurrentAsset = SceneAssetsManager.Instance.GetCurrentSceneAsset();
                    updatedCurrentAsset.dontShowMetadataWidget = state;
                    SceneAssetsManager.Instance.SetCurrentSceneAsset(updatedCurrentAsset);
                }
            }

            public bool GetAlwaysShowWidget()
            {
                return dontShowAgain;
            }

            public bool IsTransitionState()
            {
                return isTransitionState;
            }

            #region UI States

            #region Widget Positions

            public void SetWidgetPosition(Vector3 position) => widgetRect.position = position;

            public void SetWidgetLocalPosition(Vector3 position) => widgetRect.localPosition = position;

            public void SetWidgetAnchoredPosition(Vector2 position) => widgetRect.anchoredPosition = position;

            #endregion

            #region Widget Screen Dimensions

            public void SetWidgetSizeDelta(Vector2 sizeDelta) => widgetRect.sizeDelta = sizeDelta;

            public void SetWidgetSizeDelta(int width, int height)
            {
                Vector2 sizeDelta = new Vector2(width, height);
                widgetRect.sizeDelta = sizeDelta;
            }

            public void SetWidgetSizeDelta(UIScreenDimensions dimensions)
            {
                Vector2 sizeDelta = new Vector2(dimensions.width, dimensions.height);
                widgetRect.sizeDelta = sizeDelta;
            }

            public void SetWidgetLocalScale(Vector3 scale) => widgetRect.localScale = scale;

            #endregion

            #region UI Accessors

            public void GetActionButtonOfType(InputActionButtonType actionType, Action<CallbackData<List<UIButton<SceneDataPackets>>>> callback)
            {
                CallbackData<List<UIButton<SceneDataPackets>>> callbackResults = new CallbackData<List<UIButton<SceneDataPackets>>>();

                if(buttons != null && buttons.Count > 0)
                {
                    List<UIButton<SceneDataPackets>> foundButtons = buttons.FindAll(x => x.actionType == actionType);

                    if(foundButtons.Count > 0)
                    {
                        callbackResults.results = $"{foundButtons.Count} Button(s) Matching Action Type : {actionType} Found For Widget Type : {type} - Named : {name}";
                        callbackResults.data = foundButtons;
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"No Buttons Matching Action Type : {actionType} Found For Widget Type : {type} - Named : {name}";
                        callbackResults.data = default;
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"No Buttons Found For Widget Type : {type} - Named : {name}";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            #endregion

            #region UI Action Button States

            public void SetActionButtonUIImageValue(InputActionButtonType actionType, UIImageDisplayerType displayerType, UIImageType imageType)
            {
                if (buttons.Count > 0)
                {
                    UIButton<SceneDataPackets> button = buttons.Find(button => button.actionType == actionType);

                    if (button != null)
                        button.SetUIImageValue(SceneAssetsManager.Instance.GetImageFromLibrary(imageType), displayerType);
                    else
                        LogWarning($"Button Of Type : {actionType} With Displayer : {displayerType} & Image Type : {imageType} Not Found In Widget Type : {type} With Action Button List With : {buttons.Count} Buttons.", this, () => SetActionButtonUIImageValue(actionType, displayerType, imageType));
                }
                else
                    LogWarning("Screen Action Button List Is Null / Empty.", this, () => SetActionButtonUIImageValue(actionType, displayerType, imageType));
            }

            public void SetActionButtonState(InputUIState state)
            {
                if (buttons.Count > 0)
                {
                    foreach (var button in buttons)
                    {
                        if (button.value != null)
                            button.SetUIInputState(state);
                        else
                            LogWarning($"Button Of Type : {button.actionType}'s Value Missing.", this, () => SetActionButtonState(state));
                    }
                }
                else
                    LogWarning("Screen Action Button List Is Null / Empty.", this, () => SetActionButtonState(state));
            }

            public void SetActionButtonTitle(InputActionButtonType actionType, string title)
            {
                if (buttons.Count > 0)
                {
                    UIButton<SceneDataPackets> button = buttons.Find(button => button.actionType == actionType);

                    if (button != null)
                        button.SetTitle(title);
                    else
                        LogWarning($"To Set Title {title} For Button Of Type : {actionType} - Button Missing / Not Found.", this, () => SetActionButtonTitle(actionType, title));
                }
                else
                   LogWarning("--> Screen Action Button List Is Null / Empty.", this, () => SetActionButtonTitle(actionType, title));
            }

            public void SetActionButtonState(InputActionButtonType actionType, InputUIState state)
            {
                if (buttons.Count > 0)
                {
                    UIButton<SceneDataPackets> button = buttons.Find(button => button.actionType == actionType);

                    if (button != null)
                        button.SetUIInputState(state);
                    else
                        LogWarning($"Button Of Type : {actionType} Not Found In Widgt Type : {type} With Action Button List With : {buttons.Count} Buttons", this, () => SetActionButtonState(actionType, state));
                }
                else
                    LogWarning("Screen Action Button List Is Null / Empty.", this, () => SetActionButtonState(actionType, state));
            }

            #endregion

            #region UI Action Input States

            public void SetActionInputFieldState(InputUIState state)
            {
                if (inputs.Count > 0)
                {
                    foreach (var inputField in inputs)
                    {
                        if (inputField.value != null)
                            inputField.SetUIInputState(state);
                        else
                            Debug.LogWarning($"--> Failed : Input Field Of Type : {inputField.actionType}'s Value Missing.");
                    }
                }
                else
                    Debug.LogWarning("--> SetActionInputState Failed : screenActionInputFieldList Is Null / Empty.");
            }

            public void SetActionInputFieldState(InputFieldActionType actionType, InputUIState state)
            {
                if (inputs.Count > 0)
                {
                    UIInputField<SceneDataPackets> inputField = inputs.Find(inputField => inputField.actionType == actionType);

                    if (inputField != null)
                        inputField.SetUIInputState(state);
                    else
                        Debug.LogWarning($"--> Failed : Input Field Of Type : {actionType} Not Found In Widget Type : {type} With Input Field List With : {inputs.Count} Input Fields");
                }
                else
                    Debug.LogWarning("--> SetActionInputState Failed : screenActionInputFieldList Is Null / Empty.");
            }

            public void SetActionInputFieldValueText(InputFieldActionType actionType, string value)
            {
                if (inputs.Count > 0)
                {
                    UIInputField<SceneDataPackets> inputField = inputs.Find(inputField => inputField.actionType == actionType);

                    if (inputField != null)
                        inputField.SetValue(value);
                    else
                        Debug.LogWarning($"--> Failed : Input Field Of Type : {actionType} Not Found In Widget Type : {type} With Input Field List With : {inputs.Count} Input Fields");
                }
                else
                    Debug.LogWarning("--> SetActionInputState Failed : screenActionInputFieldList Is Null / Empty.");
            }

            public void SetActionInputFieldValueText(InputFieldActionType actionType, int value)
            {
                if (inputs.Count > 0)
                {
                    UIInputField<SceneDataPackets> inputField = inputs.Find(inputField => inputField.actionType == actionType);

                    if (inputField != null)
                        inputField.SetValue(value.ToString());
                    else
                        Debug.LogWarning($"--> Failed : Input Field Of Type : {actionType} Not Found In Widget Type : {type} With Input Field List With : {inputs.Count} Input Fields");
                }
                else
                    Debug.LogWarning("--> SetActionInputState Failed : screenActionInputFieldList Is Null / Empty.");
            }

            public void SetActionInputFieldPlaceHolderText(InputFieldActionType actionType, string placeholder)
            {
                if (inputs.Count > 0)
                {
                    UIInputField<SceneDataPackets> inputField = inputs.Find(inputField => inputField.actionType == actionType);

                    if (inputField != null)
                        inputField.SetPlaceHolderText(placeholder);
                    else
                        Debug.LogWarning($"--> Failed : Input Field Of Type : {actionType} Not Found In Widget Type : {type} With Input Field List With : {inputs.Count} Input Fields");
                }
                else
                    Debug.LogWarning("--> SetActionInputState Failed : screenActionInputFieldList Is Null / Empty.");
            }

            public void SetActionInputFieldPlaceHolderText(InputFieldActionType actionType, int placeholder)
            {
                if (inputs.Count > 0)
                {
                    UIInputField<SceneDataPackets> inputField = inputs.Find(inputField => inputField.actionType == actionType);

                    if (inputField != null)
                        inputField.SetPlaceHolderText(placeholder);
                    else
                        Debug.LogWarning($"--> Failed : Input Field Of Type : {actionType} Not Found In Widget Type : {type} With Input Field List With : {inputs.Count} Input Fields");
                }
                else
                    Debug.LogWarning("--> SetActionInputState Failed : screenActionInputFieldList Is Null / Empty.");
            }

            #endregion

            #region UI Action Dropdown States

            public void SetActionDropdownState(InputDropDownActionType actionType, InputUIState state)
            {
                if (dropdowns.Count > 0)
                {
                    UIDropDown<SceneDataPackets> dropdown = dropdowns.Find(dropdown => dropdown.actionType == actionType);

                    if (dropdown.value != null)
                        dropdown.SetUIInputState(state);
                    else
                        Debug.LogWarning($"--> Failed : Input Field Of Type : {actionType} Not Found In Widget Type : {type} With Input Field List With : {dropdowns.Count} Dropdowns");
                }
                else
                    Debug.LogWarning("--> SetActionDropdownState Failed : screenActionDropDownList Is Null / Empty.");
            }

            public void SetActionDropdownState(InputDropDownActionType actionType, InputUIState state, List<string> content)
            {
                if (dropdowns.Count > 0)
                {
                    UIDropDown<SceneDataPackets> dropdown = dropdowns.Find(dropdown => dropdown.actionType == actionType);

                    if (dropdown.value != null)
                    {
                        dropdown.SetContent(content);
                        dropdown.SetUIInputState(state);
                    }
                    else
                        Debug.LogWarning($"--> Failed : Input Field Of Type : {actionType} Not Found In Widget Type : {type} With Input Field List With : {dropdowns.Count} Dropdowns");
                }
                else
                    Debug.LogWarning("--> SetActionDropdownState Failed : screenActionDropDownList Is Null / Empty.");
            }

            public void SetActionDropdownState(InputUIState state)
            {
                foreach (var dropdown in dropdowns)
                {
                    if (dropdown.value != null)
                        dropdown.SetUIInputState(state);
                    else
                        Debug.LogWarning($"--> Failed : Dropdown Of Type : {dropdown.actionType}'s Value Missing.");
                }
            }

            public void SetActionDropdownState(InputUIState state, List<string> content)
            {
                foreach (var dropdown in dropdowns)
                {
                    if (dropdown.value != null)
                    {
                        dropdown.SetContent(content);
                        dropdown.SetUIInputState(state);
                    }
                    else
                        Debug.LogWarning($"--> Failed : Dropdown Of Type : {dropdown.actionType}'s Value Missing.");
                }
            }

            #endregion

            #region UI Action Slider States

            public void SetActionSliderState(SliderValueType valueType, InputUIState state)
            {
                if (uiSliders.Count > 0)
                {
                    UISlider<SceneDataPackets> slider = uiSliders.Find(slider => slider.valueType == valueType);

                    if (slider.value != null)
                        slider.SetUIInputState(state);
                    else
                        Debug.LogWarning($"--> Failed : Slider Of Type : {valueType} Not Found In Widget Type : {type} With Sliders List With : {uiSliders.Count} Sliders");
                }
                else
                    Debug.LogWarning("--> SetActionSliderState Failed : screenActionSliderList Is Null / Empty.");
            }


            public void SetActionSliderState(InputUIState state)
            {
                if (uiSliders.Count > 0)
                {
                    foreach (var slider in uiSliders)
                    {
                        if (slider != null)
                            slider.SetUIInputState(state);
                        else
                            Debug.LogWarning($"--> Failed : Slider Of Type : {slider.valueType}'s Value Missing.");
                    }
                }
                else
                    Debug.LogWarning("--> SetActionSliderState Failed : screenActionSliderList Is Null / Empty.");
            }

            #endregion

            #region UI Action Checkbox States

            public void SetActionCheckboxState(CheckboxInputActionType actionType, InputUIState state)
            {
                if (checkboxes.Count > 0)
                {
                    UICheckbox<SceneDataPackets> checkbox = checkboxes.Find(checkbox => checkbox.actionType == actionType);

                    if (checkbox != null)
                        checkbox.SetUIInputState(state);
                    else
                        Debug.LogWarning($"--> Failed :Checkbox Of Type : {actionType} Not Found In Widget Type : {type} With Input Field List With : {checkboxes.Count} Checkboxes");
                }
                else
                    Debug.LogWarning("--> SetActionCheckboxState Failed : screenActionCheckboxList Is Null / Empty.");
            }

            public void SetActionCheckboxState(InputUIState state)
            {
                if (checkboxes.Count > 0)
                {
                    foreach (var checkbox in checkboxes)
                    {
                        if (checkbox != null)
                            checkbox.SetUIInputState(state);
                        else
                            Debug.LogWarning($"--> Failed : Checkbox Of Type : {checkbox.actionType}'s Value Missing.");
                    }
                }
                else
                    Debug.LogWarning("--> SetActionCheckboxState Failed : screenActionCheckboxList Is Null / Empty.");
            }

            #endregion

            #region UI Action Checkbox Value

            public void SetActionCheckboxValue(CheckboxInputActionType actionType, bool value)
            {
                if (checkboxes.Count > 0)
                {
                    UICheckbox<SceneDataPackets> checkbox = checkboxes.Find(checkbox => checkbox.actionType == actionType);

                    if (checkbox != null)
                        checkbox.SetSelectionState(value);
                    else
                        Debug.LogWarning($"--> Failed : Checkbox Of Type : {actionType} Not Found In Widget Type : {type} With Input Field List With : {checkboxes.Count} Checkboxes");
                }
                else
                    Debug.LogWarning("--> SetActionCheckboxValue Failed : screenActionCheckboxList Is Null / Empty.");
            }

            public void SetActionCheckboxValue(bool value)
            {
                if (checkboxes.Count > 0)
                {
                    foreach (var checkbox in checkboxes)
                    {
                        if (checkbox != null)
                            checkbox.SetSelectionState(value);
                        else
                            Debug.LogWarning($"--> Failed : Checkbox Of Type : {checkbox.actionType}'s Value Missing.");
                    }
                }
                else
                    Debug.LogWarning("--> SetActionCheckboxValue Failed : screenActionCheckboxList Is Null / Empty.");
            }

            #endregion

            #region UI Text Displayer Value

            public void SetUITextDisplayerValue(ScreenTextType textType, string value)
            {
                if (textDisplayerList.Count > 0)
                {
                    UIText<SceneDataPackets> textDisplayer = textDisplayerList.Find(textDisplayer => textDisplayer.textType == textType);

                    if (textDisplayer != null)
                        textDisplayer.SetScreenUITextValue(value);
                    else
                        Debug.LogWarning($"--> Failed : Text Displayer Of Type : {textType} Not Found In Widget Type : {type} With Input Field List With : {textDisplayerList.Count} Text Displayers");
                }
                else
                    Debug.LogWarning("--> SetUITextDisplayerValue Failed : screenTextList Is Null / Empty.");
            }

            public void SetUITextDisplayerValue(ScreenTextType textType, int value)
            {
                if (textDisplayerList.Count > 0)
                {
                    UIText<SceneDataPackets> textDisplayer = textDisplayerList.Find(textDisplayer => textDisplayer.textType == textType);

                    if (textDisplayer != null)
                        textDisplayer.SetScreenUITextValue(value.ToString());
                    else
                        Debug.LogWarning($"--> Failed : Text Displayer Of Type : {textType} Not Found In Widget Type : {type} With Input Field List With : {textDisplayerList.Count} Text Displayers");
                }
                else
                    Debug.LogWarning("--> SetUITextDisplayerValue Failed : screenTextList Is Null / Empty.");
            }

            #endregion

            #region UI Image Displayer Value

            public void SetUIImageDisplayerValue(ScreenImageType displayerType, ImageData screenCaptureData, ImageDataPackets dataPackets)
            {
                if (imageDisplayers.Count > 0)
                {
                    UIImageDisplayer<SceneDataPackets> imageDisplayer = imageDisplayers.Find(imageDisplayer => imageDisplayer.imageType == displayerType);

                    if (imageDisplayer != null)
                        imageDisplayer.SetImageData(screenCaptureData, dataPackets);
                    else
                        Debug.LogWarning($"--> Failed : Image Displayer Of Type : {displayerType} Not Found In Widget Type : {type} With Input Field List With : {imageDisplayers.Count} Image Displayers");
                }
                else
                    Debug.LogWarning("--> SetUIImageDisplayerValue Failed : screenImageDisplayerList Is Null / Empty.");
            }

            public void SetUIImageDisplayerValue(ScreenImageType displayerType, Texture2D imageData)
            {
                if (imageDisplayers.Count > 0)
                {
                    UIImageDisplayer<SceneDataPackets> imageDisplayer = imageDisplayers.Find(imageDisplayer => imageDisplayer.imageType == displayerType);

                    if (imageDisplayer != null)
                        imageDisplayer.SetImageData(imageData);
                    else
                        Debug.LogWarning($"--> Failed : Image Displayer Of Type : {displayerType} Not Found In Widget Type : {type} With Input Field List With : {imageDisplayers.Count} Image Displayers");
                }
                else
                    Debug.LogWarning("--> SetUIImageDisplayerValue Failed : screenImageDisplayerList Is Null / Empty.");
            }

            public void SetUIImageDisplayerValue(ScreenImageType displayerType, Sprite image)
            {
                if (imageDisplayers.Count > 0)
                {
                    UIImageDisplayer<SceneDataPackets> imageDisplayer = imageDisplayers.Find(imageDisplayer => imageDisplayer.imageType == displayerType);

                    if (imageDisplayer != null)
                        imageDisplayer.SetImageData(image);
                    else
                        Debug.LogWarning($"--> Failed : Image Displayer Of Type : {displayerType} Not Found In Widget Type : {type} With Input Field List With : {imageDisplayers.Count} Image Displayers");
                }
                else
                    Debug.LogWarning("--> SetUIImageDisplayerValue Failed : screenImageDisplayerList Is Null / Empty.");
            }

            #endregion

            #region Scroller 

            public void ResetScrollPosition(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (scroller.resetScrollerPositionOnHide)
                    scroller.ResetPosition(resetCallback => { callbackResults = resetCallback; });
                else
                {
                    callbackResults.results = "Reset Scroller Disabled.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }

                callback?.Invoke(callbackResults);
            }

            #endregion

            #endregion
        }

        [Serializable]
        public abstract class SettingsWidget : AppMonoBaseClass, ISettingsWidget
        {
            #region Components

            [Header("Widget Info")]
            [Space(5)]
            [SerializeField]
            protected GameObject value = null;

            [Space(5)]
            [SerializeField]
            protected SettingsWidgetType widgetType;

            [Space(5)]
            [SerializeField]
            protected SceneDataPackets dataPackets;

            [Space(5)]
            [SerializeField]
            protected bool initialVisiblityState;

            [Space(5)]
            [SerializeField]
            protected bool isChildWidget;

            [Space(5)]
            [SerializeField]
            protected bool includeChildWidgets;

            [Space(5)]
            [SerializeField]
            protected List<SettingsWidget> subWidgetsList = new List<SettingsWidget>();

            [Space(10)]
            [Header("Button")]

            [Space(5)]
            [SerializeField]
            protected List<UIButton<ButtonDataPackets>> actionButtonList = new List<UIButton<ButtonDataPackets>>();

            [SerializeField]
            protected bool initializeActionButtonList = false;

            [Space(10)]
            [Header("Input Field")]

            [Space(5)]
            [SerializeField]
            protected List<UIInputField<InputFieldDataPackets>> actionInputFieldList = new List<UIInputField<InputFieldDataPackets>>();

            [SerializeField]
            protected bool initializeActionInputFieldList = false;

            [Space(10)]
            [Header("Slider")]

            [Space(5)]
            [SerializeField]
            protected List<UISlider<SliderDataPackets>> actionSliderList = new List<UISlider<SliderDataPackets>>();

            [SerializeField]
            protected bool initializeActionSliderList = false;

            [Space(10)]
            [Header("Input Slider")]

            [Space(5)]
            [SerializeField]
            protected List<UIInputSlider<InputSliderDataPackets>> actionInputSliderList = new List<UIInputSlider<InputSliderDataPackets>>();

            [SerializeField]
            protected bool initializeActionInputSliderList = false;

            [Space(10)]
            [Header("Dropdown")]

            [Space(5)]
            [SerializeField]
            protected List<UIDropDown<DropdownDataPackets>> actionDropdownList = new List<UIDropDown<DropdownDataPackets>>();

            [SerializeField]
            protected bool initializeActionDropDownList = false;

            [Space(10)]
            [Header("Checkbox")]

            [Space(5)]
            [SerializeField]
            protected List<UICheckbox<CheckboxDataPackets>> actionCheckboxList = new List<UICheckbox<CheckboxDataPackets>>();

            [SerializeField]
            protected bool initializeActionCheckboxList = false;

            [Space(10)]
            [Header("Text")]

            [Space(5)]
            [SerializeField]
            protected List<UIText<TextDataPackets>> screenUITextList = new List<UIText<TextDataPackets>>();

            [SerializeField]
            protected bool initializeUITextList = false;

            [Space(10)]
            [Header("Settings Screen Widgets")]

            [Space(5)]
            [SerializeField]
            public List<SettingsDataWidget<SettingsDataPackets>> settingsDataScreenWidgetsList = new List<SettingsDataWidget<SettingsDataPackets>>();

            [SerializeField]
            protected bool initializeSettingsDataScreenWidgetsList = false;

            [Space(10)]
            [Header("Setting Storage Directory Data")]

            [Space(5)]
            [SerializeField]
            protected StorageDirectoryData storageDirectoryData = new StorageDirectoryData();

            [SerializeField]
            protected SettingsWidget parentWidget;

            protected SwatchData swatchData = new SwatchData();

            #endregion

            #region Unity Callbacks

            #region Unity Callbacks

            void OnEnable()
            {
                if (ScreenUIManager.Instance != null)
                {
                    if (GetActive())
                    {
                        //RegisterEventListensers(true);
                        //OnWidgetOpened();
                    }
                    else
                        return;
                }
                else
                    LogError("Screen UI Manager Instance Is Not Yet Initialized", this, () => OnEnable());
            }

            protected bool GetActive()
            {
                bool isActive = false;

                if (ScreenUIManager.Instance)
                {
                    if (ScreenUIManager.Instance?.GetCurrentScreenData()?.value != null)
                    {
                        if (ScreenUIManager.Instance?.GetCurrentScreenData()?.value.GetUIScreenType() == dataPackets.screenType)
                            if (this && gameObject)
                                isActive = gameObject.activeSelf && gameObject.activeInHierarchy;
                    }
                    else
                        LogWarning("Get Current Screen Data Value Is Missing / Null.", this, () => GetActive());
                }
                else
                    LogError("Is Not Yet Initialized.", this, () => GetActive());

                return isActive;
            }

            void OnDisable()
            {
                RegisterEventListensers(false);
                OnWidgetClosed();
            }

            void Start() => Initialize();

            #endregion

            #endregion

            #region Main

            protected void Initialize()
            {
                if (ScreenUIManager.Instance)
                {
                    if (GetActive())
                    {
                        #region Action List Initialization

                        #region Action Button List

                        if (initializeActionButtonList)
                            OnActionButtonInitialized((callbackResults) =>
                            {
                                if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    foreach (var button in actionButtonList)
                                        button.value.onClick.AddListener(() => OnActionButtonClickedEvent(button.dataPackets));
                                else
                                    LogWarning(callbackResults.results, this, () => Initialize());
                            });

                        #endregion

                        #region Action Input Field List

                        if (initializeActionInputFieldList)
                            OnActionInputFieldInitialized((callbackResults) =>
                            {
                                if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    foreach (var inputField in actionInputFieldList)
                                    {
                                        inputField.Initialize();
                                        inputField.value.onValueChanged.AddListener((value) => OnActionInputFieldValueChangedEvent(value, inputField.dataPackets));
                                    }
                                else
                                    LogWarning(callbackResults.results, this, () => Initialize());
                            });

                        #endregion

                        #region Action Slider List

                        if (initializeActionSliderList)
                            OnActionSliderInitialized((callbackResults) =>
                            {
                                if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    foreach (var slider in actionSliderList)
                                    {
                                        slider.Initialize();
                                        slider.value.onValueChanged.AddListener((value) => OnActionSliderValueChangedEvent(value, slider.dataPackets));
                                    }
                                else
                                    LogWarning(callbackResults.results, this, () => Initialize());
                            });

                        #endregion

                        #region Action Input Slider List

                        if (initializeActionInputSliderList)
                            OnActionInputSliderInitialized((callbackResults) =>
                            {
                                if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    foreach (var inputSlider in actionInputSliderList)
                                    {
                                        inputSlider.Initialize();
                                        inputSlider.slider.onValueChanged.AddListener((value) => OnInputSliderValueChangedEvent(value, inputSlider.dataPackets));
                                        inputSlider.inputField.onValueChanged.AddListener((value) => OnInputSliderValueChangedEvent(value, inputSlider.dataPackets));
                                    }
                                else
                                    LogWarning(callbackResults.results, this, () => Initialize());
                            });

                        #endregion

                        #region Action Checkbox List

                        if (initializeActionCheckboxList)
                            OnActionChecboxInitialized((callbackResults) =>
                            {
                                if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    foreach (var checkbox in actionCheckboxList)
                                    {
                                        checkbox.Initialize();
                                        checkbox.value.onValueChanged.AddListener((value) => OnActionCheckboxValueChangedEvent(value, checkbox.dataPackets));
                                    }
                                else
                                    LogWarning(callbackResults.results, this, () => Initialize());
                            });

                        #endregion

                        #region Screen Widgets List

                        if (initializeSettingsDataScreenWidgetsList)
                            OnSettingsWidgetInitialized((callbackResults) =>
                            {
                                if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    foreach (var widget in settingsDataScreenWidgetsList)
                                        widget.Initialize();
                                else
                                    LogWarning(callbackResults.results, this, () => Initialize());
                            });

                        #endregion

                        #endregion

                        if (includeChildWidgets)
                        {
                            OnSettingsSubWidgetsInitialized((subWidgetsCallbackResults) =>
                            {
                                if (!Helpers.IsSuccessCode(subWidgetsCallbackResults.resultsCode))
                                    LogWarning(subWidgetsCallbackResults.results, this, () => Initialize());
                            });
                        }

                        if (initialVisiblityState)
                            ShowWidget();
                        else
                            HideWidget();

                        if (isChildWidget)
                        {
                            if (gameObject.GetComponentInParent<SettingsWidget>())
                                parentWidget = gameObject.GetComponentInParent<SettingsWidget>();
                            else
                                LogWarning("Parent Doesn't Contain A Setiings Widget Component.", this, () => Initialize());
                        }

                        Init();
                    }
                }
                else
                    LogError("Screen UI Manager Instance Is Not Yet Initialized.", this, () => Initialize());
            }

            protected void ShowWidgetOnDropDownSelection(SettingsWidgetTabID widgetTabID, InputDropDownActionType actionType)
            {
                OnSettingsWidgetInitialized((callbackResults) =>
                {
                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                    {
                        foreach (var widgetScreen in settingsDataScreenWidgetsList)
                            if (widgetScreen.dataPackets.widgetTabID == widgetTabID)
                                widgetScreen.Show();
                            else
                                widgetScreen.Hide();
                    }
                    else
                        LogWarning(callbackResults.results, this, () => Initialize());
                });
            }

            public void SetActionButtonState(InputActionButtonType buttonType, InputUIState buttonState)
            {
                foreach (var actionButton in actionButtonList)
                    if (actionButton.value)
                    {
                        if (actionButton.dataPackets.actionType == buttonType)
                        {
                            actionButton.SetUIInputState(buttonState);
                            break;
                        }
                        else
                            continue;
                    }
                    else
                        LogWarning($"Action Button : {actionButton.actionType} Not Found", this, () => SetActionButtonState(buttonType, buttonState));
            }

            protected void SetActionDropdownState(InputDropDownActionType dropdownType, InputUIState dropdownState)
            {
                foreach (var dropdown in actionDropdownList)
                    if (dropdown.value)
                    {
                        if (dropdown.dataPackets.actionType == dropdownType)
                        {
                            dropdown.SetUIInputState(dropdownState);
                            break;
                        }
                        else
                            continue;
                    }
                    else
                        LogError($"Action Dropdown : {dropdown.actionType} Not Found", this, () => SetActionDropdownState(dropdownType, dropdownState));
            }

            protected void SetActionButtons(InputUIState buttonState)
            {
                OnActionButtonInitialized((callbackResults) =>
                {
                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                        foreach (var actionButton in actionButtonList)
                        {
                            if (actionButton.value)
                                actionButton.SetUIInputState(buttonState);
                            else
                                LogError("Action Button Value Missing", this, () => SetActionButtons(buttonState));
                        }
                    else
                        LogError(callbackResults.results, this, () => SetActionButtons(buttonState));
                });
            }

            protected void SetAcionInputFieldValue(InputFieldActionType actionType, string value)
            {
                OnActionInputFieldInitialized((callbackResults) =>
                {
                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                    {
                        UIInputField<InputFieldDataPackets> inputField = actionInputFieldList.Find((x) => x.dataPackets.actionType == actionType);

                        if (inputField.value)
                        {
                            inputField.value.text = value;
                        }
                        else
                            LogError($"Input Field : {actionType} Value Is Not Found / Null.", this, () => SetAcionInputFieldValue(actionType, value));
                    }
                    else
                        LogError(callbackResults.results, this, () => SetAcionInputFieldValue(actionType, value));
                });
            }

            protected void SetAcionInputSliderValue(InputSliderActionType actionType, float sliderValue, string inputValue)
            {
                OnActionInputSliderInitialized((callbackResults) =>
                {
                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                    {
                        UIInputSlider<InputSliderDataPackets> inputSlider = actionInputSliderList.Find((x) => x.dataPackets.actionType == actionType);

                        if (inputSlider.IsInitialized())
                        {
                            inputSlider.slider.value = sliderValue;
                            inputSlider.inputField.text = inputValue;
                        }
                        else
                            LogError($"Input Slider : {actionType} Is Not Initialized.", this, () => SetAcionInputSliderValue(actionType, sliderValue, inputValue));
                    }
                    else
                        LogWarning(callbackResults.results, this, () => SetAcionInputSliderValue(actionType, sliderValue, inputValue));
                });
            }

            protected void SetAcionInputSliderValue(InputSliderActionType actionType, float sliderValue)
            {
                OnActionInputSliderInitialized((callbackResults) =>
                {
                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                    {
                        UIInputSlider<InputSliderDataPackets> inputSlider = actionInputSliderList.Find((x) => x.dataPackets.actionType == actionType);

                        if (inputSlider.IsInitialized())
                            inputSlider.slider.value = sliderValue;
                        else
                            LogError($"Input Slider : {actionType} Is Not Initialized.", this, () => SetAcionInputSliderValue(actionType, sliderValue));
                    }
                    else
                        LogWarning(callbackResults.results, this, () => SetAcionInputSliderValue(actionType, sliderValue));
                });
            }

            protected void SetAcionInputSliderValue(InputSliderActionType actionType, string inputValue)
            {
                OnActionInputSliderInitialized((callbackResults) =>
                {
                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                    {
                        UIInputSlider<InputSliderDataPackets> inputSlider = actionInputSliderList.Find((x) => x.dataPackets.actionType == actionType);

                        if (inputSlider.IsInitialized())
                            inputSlider.inputField.text = inputValue;
                        else
                            LogError($"Input Slider : {actionType} Is Not Initialized.", this, () => SetAcionInputSliderValue(actionType, inputValue));
                    }
                    else
                        LogWarning(callbackResults.results, this, () => SetAcionInputSliderValue(actionType, inputValue));
                });
            }

            #region Initialization

            protected void OnActionButtonInitialized(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (actionButtonList.Count > 0)
                {
                    foreach (var button in actionButtonList)
                    {
                        if (button.value != null)
                        {
                            callbackResults.results = "ActionButtonsList Buttons Initialized.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"ActionButtonsList Value At Index : {actionButtonList.IndexOf(button)} Is Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;

                            callback?.Invoke(callbackResults);
                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"ActionButtonsList Is Null For Widgets Type : {widgetType} On Game Object : {this.name}.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void OnActionInputFieldInitialized(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (actionInputFieldList.Count > 0)
                {
                    foreach (var inputField in actionInputFieldList)
                    {
                        if (inputField.value != null)
                        {
                            callbackResults.results = "ActionInputFieldList Buttons Initialized.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"ActionInputFieldList Value At Index : {actionInputFieldList.IndexOf(inputField)} Is Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;

                            callback?.Invoke(callbackResults);
                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"ActionInputFieldList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void OnActionDropdownInitialized(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (actionDropdownList.Count > 0)
                {
                    foreach (var dropdown in actionDropdownList)
                    {
                        if (dropdown.value != null)
                        {
                            dropdown.Initialize();
                            dropdown._AddInputEventListener += OnInputDropdownSelectedEvent;

                            callbackResults.results = "OnActionDropdownInitialized Initialized Sucess : actionDropdownList Buttons Initialized.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"OnActionDropdownInitialized Initialized Failed : actionDropdownList Value At Index : {actionDropdownList.IndexOf(dropdown)} Is Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;

                            callback?.Invoke(callbackResults);
                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"OnActionDropdownInitialized Initialized Failed : actionDropdownList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void OnInputDropdownSelectedEvent(InputDropDownActionType actionType)
            {
                foreach (var dropdown in actionDropdownList)
                    if (dropdown.dataPackets.actionType != actionType && dropdown.selectableInput)
                        dropdown.value.Hide();
            }

            protected void OnActionSliderInitialized(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (actionSliderList.Count > 0)
                {
                    foreach (var slider in actionSliderList)
                    {
                        if (slider.value != null)
                        {
                            callbackResults.results = "ActionSliderList Slider Initialized.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"ActionSliderList Value At Index : {actionSliderList.IndexOf(slider)} Is Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;

                            callback?.Invoke(callbackResults);
                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"ActionSliderList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void OnActionInputSliderInitialized(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (actionInputSliderList.Count > 0)
                {
                    foreach (var inputSlider in actionInputSliderList)
                    {
                        if (inputSlider.IsInitialized())
                        {
                            callbackResults.results = "ActionInputSliderList Input Slider Initialized.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"ActionInputSliderList Value At Index : {actionInputSliderList.IndexOf(inputSlider)} Is Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;

                            callback?.Invoke(callbackResults);
                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"AcionInputSliderList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void OnUpdateDropdownSelection(InputDropDownActionType dropdownType, List<string> contentList, bool isUpdate = true, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (actionDropdownList.Count > 0)
                {
                    foreach (var dropdown in actionDropdownList)
                    {
                        if (dropdown.value != null)
                        {
                            if (dropdown.dataPackets.actionType == dropdownType)
                            {
                                if (contentList != null)
                                {
                                    dropdown.value.ClearOptions();
                                    dropdown.value.onValueChanged.RemoveAllListeners();

                                    List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                                    foreach (var content in contentList)
                                        dropdownOption.Add(new TMP_Dropdown.OptionData() { text = content });

                                    dropdown.value.AddOptions(dropdownOption);

                                    dropdown.value.onValueChanged.AddListener((value) => OnActionDropdownValueChangedEvent(contentList[value], dropdown.dataPackets));

                                    // Select Initial Content
                                    if (isUpdate)
                                        dropdown.value.value = contentList.IndexOf(contentList.FirstOrDefault());
                                    else
                                        dropdown.value.value = contentList.IndexOf(contentList.Last());

                                    callbackResults.results = $"ActionDropdownList's : {dropdown.name} Has Been Created Successfully.";
                                    callbackResults.resultsCode = Helpers.SuccessCode;
                                }
                                else
                                {
                                    callbackResults.results = "Dropdown Content List Is Null.";
                                    callbackResults.resultsCode = Helpers.ErrorCode;
                                }

                                break;
                            }
                            else
                                continue;
                        }
                        else
                        {
                            callbackResults.results = $"ActionDropdownList Value At Index : {actionDropdownList.IndexOf(dropdown)} Is Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;

                            callback?.Invoke(callbackResults);
                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"ActionDropdownList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void OnActionChecboxInitialized(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (actionCheckboxList.Count > 0)
                {
                    foreach (var checkbox in actionCheckboxList)
                    {
                        if (checkbox.value != null)
                        {
                            callbackResults.results = "ActionCheckboxList Buttons Initialized.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"ActionCheckboxList Value At Index : {actionCheckboxList.IndexOf(checkbox)} Is Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;

                            callback?.Invoke(callbackResults);
                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"ActionCheckboxList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void OnScreenUITextInitialized(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (screenUITextList.Count > 0)
                {
                    foreach (var uiText in screenUITextList)
                    {
                        if (uiText.value != null)
                        {
                            callbackResults.results = "ScreenUITextList Buttons Initialized.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"ScreenUITextList Value At Index : {screenUITextList.IndexOf(uiText)} Is Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;

                            callback?.Invoke(callbackResults);
                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"ScreenUITextList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void OnSettingsWidgetInitialized(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (settingsDataScreenWidgetsList.Count > 0)
                {
                    foreach (var panelWidget in settingsDataScreenWidgetsList)
                    {
                        if (panelWidget.value != null)
                        {
                            callbackResults.results = "PanelWidgetsList Buttons Initialized.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"PanelWidgetsList Value At Index : {settingsDataScreenWidgetsList.IndexOf(panelWidget)} Is Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;

                            callback?.Invoke(callbackResults);
                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"PanelWidgetsList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void OnSettingsSubWidgetsInitialized(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (subWidgetsList.Count == 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).GetComponent<SettingsWidget>())
                        {
                            SettingsWidget widget = transform.GetChild(i).GetComponent<SettingsWidget>();
                            subWidgetsList.Add(widget);
                        }
                        else
                            continue;
                    }

                    if (subWidgetsList != null && subWidgetsList.Count > 0)
                    {
                        callbackResults.results = $"Sub Widgets Found In Childrens.";
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"PanelWidgetsList Is Null.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"Sub Widgets List Already Initialized.";
                    callbackResults.resultsCode = Helpers.WarningCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void SetInputFieldValue(InputFieldActionType type, string value, Action<Callback> callback)
            {
                Callback callbackResults = new Callback();

                if (actionInputFieldList.Count > 0)
                {
                    UIInputField<InputFieldDataPackets> inputField = actionInputFieldList.Find((x) => x.dataPackets.actionType == type);

                    if (inputField.value)
                    {
                        inputField.SetValue(value);

                        callbackResults.results = $"ActionInputFieldList Value : {value} Set For Type : {type}.";
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"ActionInputFieldList Set Value : {value} For Type : {type} Is Missing / Null.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "ActionInputFieldList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            protected void UpdateInputFieldInfo(InputSliderActionType actionType, string fieldName, Color fieldColor)
            {
                OnActionInputSliderInitialized((callbackResults) =>
                {
                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                        foreach (var inputSlider in actionInputSliderList)
                        {
                            if (inputSlider.dataPackets.actionType == actionType)
                            {
                                inputSlider.SetTitle(fieldName);
                                inputSlider.SetFieldColor(fieldColor);
                                break;
                            }
                            else
                                continue;
                        }
                    else
                        LogWarning(callbackResults.results, this, () => UpdateInputFieldInfo(actionType, fieldName, fieldColor));
                });
            }

            public void UpdateInputActionCheckbox(CheckboxInputActionType actionType, bool isSelected)
            {
                OnActionChecboxInitialized((checkboxInitializedCallbackResults) =>
                {
                    if (Helpers.IsSuccessCode(checkboxInitializedCallbackResults.resultsCode))
                    {
                        foreach (var checkbox in actionCheckboxList)
                        {
                            if (checkbox.dataPackets.actionType == actionType)
                            {
                                if (checkbox.value != null)
                                    checkbox.SetSelectionState(isSelected);
                                else
                                    LogWarning("Checkbox Value Missing / Null.", this, () => UpdateInputActionCheckbox(actionType, isSelected));
                            }
                        }
                    }
                    else
                        LogWarning(checkboxInitializedCallbackResults.results, this, () => UpdateInputActionCheckbox(actionType, isSelected));
                });
            }

            protected void UpdateInputFieldUIInputState(InputSliderActionType actionType, InputUIState state)
            {
                OnActionInputSliderInitialized((callbackResults) =>
                {
                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                        foreach (var inputSlider in actionInputSliderList)
                        {

                            if (inputSlider.dataPackets.actionType == actionType)
                            {
                                inputSlider.SetUIInputState(state);
                                break;
                            }
                            else
                                continue;
                        }
                    else
                        LogWarning(callbackResults.results, this, () => UpdateInputFieldUIInputState(actionType, state));
                });
            }

            protected void SetScreenUITextValue(string value, ScreenUITextType textType)
            {
                OnScreenUITextInitialized((screenTextCallbackResults) =>
                {
                    var screenText = screenUITextList.Find((x) => x.dataPackets.textType == textType);

                    if (screenText != null)
                        screenText.SetScreenUITextValue(value);
                    else
                        LogWarning($"Settings Text : {value} To ScreenText Of Type : {textType} Missing / Not Found.", this, ()  => SetScreenUITextValue(value, textType));
                });
            }

            protected void ResetInputFieldValue(InputSliderActionType actionType, float value = 0)
            {
                OnActionInputSliderInitialized((callbackResults) =>
                {
                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                        foreach (var inputSlider in actionInputSliderList)
                        {

                            if (inputSlider.dataPackets.actionType == actionType)
                            {
                                inputSlider.SetValue(value.ToString(), value);
                                break;
                            }
                            else
                                continue;
                        }
                    else
                        LogWarning(callbackResults.results, this, () => ResetInputFieldValue(actionType, value = 0));
                });
            }

            public void ShowChildWidget(SettingsWidgetType widgetType)
            {
                OnSettingsSubWidgetsInitialized((subWidgetCallbackResults) =>
                {
                    if (Helpers.IsSuccessCode(subWidgetCallbackResults.resultsCode))
                    {
                        SettingsWidget widget = subWidgetsList.Find((x) => x.widgetType == widgetType);

                        if (widget != null)
                            widget.ShowWidget();
                        else
                            LogWarning("Loaded Sub Widget Not Found / Null.", this, () => ShowChildWidget(widgetType));
                    }
                    else
                        LogWarning(subWidgetCallbackResults.results, this, () => ShowChildWidget(widgetType));
                });
            }

            public void ShowChildWidget(SettingsWidgetType widgetType, string message, ScreenUITextType textType)
            {
                OnSettingsSubWidgetsInitialized((subWidgetCallbackResults) =>
                {
                    if (Helpers.IsSuccessCode(subWidgetCallbackResults.resultsCode))
                    {
                        SettingsWidget widget = subWidgetsList.Find((x) => x.widgetType == widgetType);

                        if (widget != null)
                            widget.ShowWidget(message, textType);
                        else
                            LogWarning("Loaded Sub Widget Not Found / Null.", this, () => ShowChildWidget(widgetType, message, textType));
                    }
                    else
                        LogWarning(subWidgetCallbackResults.results, this, () => ShowChildWidget(widgetType, message, textType));
                });
            }

            public void HideChildWidget(SettingsWidgetType widgetType)
            {
                OnSettingsSubWidgetsInitialized((subWidgetCallbackResults) =>
                {
                    if (Helpers.IsSuccessCode(subWidgetCallbackResults.resultsCode))
                    {
                        SettingsWidget widget = subWidgetsList.Find((x) => x.widgetType == widgetType);

                        if (widget != null && widget.value != null)
                            widget.HideWidget();
                        else
                            LogWarning("Loaded Widget Is Null Or Value Is Empty In Sub Widgets list.", this, () => HideChildWidget(widgetType));
                    }
                    else
                        LogWarning(subWidgetCallbackResults.results, this, () => HideChildWidget(widgetType));
                });
            }

            public void ResetChildWidget(SettingsWidgetType widgetType)
            {
                OnSettingsSubWidgetsInitialized((subWidgetCallbackResults) =>
                {
                    if (Helpers.IsSuccessCode(subWidgetCallbackResults.resultsCode))
                    {
                        SettingsWidget widget = subWidgetsList.Find((x) => x.widgetType == widgetType);

                        if (widget != null && widget.value != null)
                            widget.ResetWidgetData(widgetType);
                        else
                            LogWarning("Loaded Widget Is Null Or Value Is Empty In Sub Widgets list.", this, () => ResetChildWidget(widgetType));
                    }
                    else
                        LogWarning(subWidgetCallbackResults.results, this, () => ResetChildWidget(widgetType));
                });
            }

            protected void CloseWidget(SettingsWidget widget) => widget.HideWidget();

            #endregion

            #endregion

            #region Abstract Functions

            protected abstract void Init();

            protected abstract void RegisterEventListensers(bool register);

            protected abstract void OnWidgetOpened();

            protected abstract void OnWidgetClosed();

            protected abstract void OnActionButtonClickedEvent(ButtonDataPackets dataPackets);

            protected abstract void OnActionInputFieldValueChangedEvent(string value, InputFieldDataPackets dataPackets);

            protected abstract void OnActionSliderValueChangedEvent(float value, SliderDataPackets dataPackets);

            protected abstract void OnActionDropdownValueChangedEvent(string value, DropdownDataPackets dataPackets);
            protected abstract void OnActionDropdownValueChangedEvent(int value, DropdownDataPackets dataPackets);
            protected abstract void OnActionDropdownValueChangedEvent(int value, List<string> contentList, AppData.DropdownDataPackets dataPackets);

            protected abstract void OnInputSliderValueChangedEvent(float value, InputSliderDataPackets dataPackets);
            protected abstract void OnInputSliderValueChangedEvent(string value, InputSliderDataPackets dataPackets);

            protected abstract void OnActionCheckboxValueChangedEvent(bool value, CheckboxDataPackets dataPackets);

            protected abstract void OnResetWidgetData(SettingsWidgetType widgetType);

            public void ShowWidget()
            {
                if (value)
                    value.SetActive(true);
                else
                    LogError("Widget Value Missing / Null.", this, () => ShowWidget());
            }

            public void ShowWidget(string messsage, ScreenUITextType textType)
            {
                if (value)
                {
                    SetScreenUITextValue(messsage, textType);
                    value.SetActive(true);
                }
                else
                    LogError("Widget Value Missing / Null.", this, () => ShowWidget(messsage, textType));
            }

            public void HideWidget()
            {
                if (value)
                    value.SetActive(false);
                else
                    LogError("Widget Value Missing / Null.", this, () => HideWidget());
            }

            public void ResetWidgetData(SettingsWidgetType widgetType) => OnResetWidgetData(widgetType);

            #endregion

            #region Events

            public delegate void EventsListeners();

            #endregion
        }


        [Serializable]
        public class SettingsDataWidget<T>
        {
            #region Components

            public string name;

            [Space(5)]
            public GameObject value;

            [Space(5)]
            public bool initialVisibilityState;

            [Space(5)]
            public T dataPackets;

            string panelID;

            #endregion

            #region Main

            public void Initialize()
            {
                if (initialVisibilityState)
                    Show();
                else
                    Hide();
            }

            public void Show()
            {
                if (IsInitialized())
                    value.SetActive(true);
                else
                    Debug.LogWarning("--> PanelWidget Show Panel Failed : Value Is Missing / Null.");
            }

            public void Hide()
            {
                if (IsInitialized())
                    value.SetActive(false);
                else
                    Debug.LogWarning("--> PanelWidget Hide Panel Failed : Value Is Missing / Null.");
            }

            public void SetPanelID(string id)
            {
                if (IsInitialized())
                {
                    if (!string.IsNullOrEmpty(id))
                        panelID = id;
                    else
                        Debug.LogWarning("--> PanelWidget Set Panel ID Failed : Panel ID Parameter Value Is Null.");
                }
                else
                    Debug.LogWarning("--> PanelWidget Init Failed : Value Is Missing / Null.");
            }

            public string GetPanelID()
            {
                return panelID;
            }

            public bool IsInitialized()
            {
                return value;
            }

            #endregion
        }

        [Serializable]
        public class ScreenLoadingInitializationData
        {
            #region Components

            public float duration;

            [Space(5)]
            public string title;

            [Space(5)]
            public string content;

            [Space(5)]
            public bool autoHide;

            [Space(5)]
            public bool isLargeFileSize;

            bool hasCompleted;

            #endregion

            #region Main

            public ScreenLoadingInitializationData()
            {

            }

            public ScreenLoadingInitializationData(float duration, string title, string content, bool autoHide)
            {
                this.duration = duration;
                this.title = title;
                this.content = content;
                this.autoHide = autoHide;

                hasCompleted = false;
            }


            public void SetCompleted() => hasCompleted = true;

            public bool Completed()
            {
                return hasCompleted;
            }

            #endregion
        }

        #endregion

        #region Class Data

        #region Color Data

        [Serializable]
        public class SerializableData
        {
            public string name;

            [HideInInspector]
            public StorageDirectoryData storageData;
        }

        [Serializable]
        public struct ColorInfo
        {
            public string hexadecimal;

            public Color color;
        }

        [Serializable]
        public struct ColorSwatch
        {
            public string name;

            [Space(5)]
            public int colorSpectrumSize;

            [Space(5)]
            public List<ColorInfo> colorIDList;
        }

        [Serializable]
        public class ColorSwatchPallet
        {
            #region Components
            public string name;

            // Hide This
            public List<ColorSwatchButtonHandler> swatchButtonList = new List<ColorSwatchButtonHandler>();

            #endregion

            #region Main

            public void AddSwatchButton(ColorSwatchButtonHandler swatchButton)
            {
                if (!swatchButtonList.Contains(swatchButton))
                    swatchButtonList.Add(swatchButton);
            }

            public void Show(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (swatchButtonList.Count > 0)
                {
                    foreach (var colorIconButton in swatchButtonList)
                        colorIconButton.Show();

                    callbackResults.results = "Show Color Swatch Success : Swatch Color Showing.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "Show Color Swatch Failed : Swatch Color ID List Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void Hide(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (swatchButtonList.Count > 0)
                {
                    foreach (var colorIconButton in swatchButtonList)
                        colorIconButton.Hide();

                    callbackResults.results = "Hide Color Swatch Success : Swatch Color Showing.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "Hide Color Swatch Failed : Swatch Color ID List Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            #endregion
        }

        [Serializable]
        public class ColorSwatchData
        {
            #region Components

            [Space(5)]
            public List<ColorSwatch> swatches = new List<ColorSwatch>();

            [HideInInspector]
            public List<string> swatchDropDownList = new List<string>();

            //[HideInInspector]
            public List<ColorInfo> colorInfoLibrary = new List<ColorInfo>();

            public List<ColorSwatchPallet> swatchPalletList = new List<ColorSwatchPallet>();

            //[HideInInspector]
            public SwatchData loadedSwatchData = new SwatchData();

            #endregion

            #region Main

            public void Init(string fileName, Action<CallbackData<string>> callback)
            {
                CallbackData<string> callbackResults = new CallbackData<string>();

                HasSwatchDataContent((callbackDataResults) =>
                {
                    if (Helpers.IsSuccessCode(callbackDataResults.resultsCode))
                    {
                        CreateSwatchDropDownList(fileName, (createCallbackResults) =>
                        {
                            callbackResults.results = createCallbackResults.results;
                            callbackResults.resultsCode = createCallbackResults.resultsCode;

                            if (Helpers.IsSuccessCode(createCallbackResults.resultsCode))
                                callbackResults.data = createCallbackResults.data.First();
                            else
                                Debug.LogError($"--> Init Failed With Results : {createCallbackResults.results}");
                        });
                    }
                    else
                        Debug.LogError($"--> Init Failed With Results : {callbackDataResults.results}");
                });

                callback?.Invoke(callbackResults);
            }

            public void CreateSwatchDropDownList(string fileName, Action<CallbackDatas<string>> callback = null)
            {
                CallbackDatas<string> callbackResults = new CallbackDatas<string>();

                if (SceneAssetsManager.Instance != null)
                {
                    StorageDirectoryData directoryData = SceneAssetsManager.Instance.GetAppDirectoryData(DirectoryType.Settings_Storage);

                    if (SceneAssetsManager.Instance.DirectoryFound(directoryData))
                    {
                        SceneAssetsManager.Instance.LoadData<SwatchData>(fileName, directoryData, (loadedDataResults) =>
                        {
                            if (Helpers.IsSuccessCode(loadedDataResults.resultsCode))
                            {
                                foreach (var swatch in loadedDataResults.data.swatches)
                                    if (!swatchDropDownList.Contains(swatch.name))
                                        swatchDropDownList.Add(swatch.name);

                                HasSwatchDropDownContent((callbackDataResults) =>
                                {
                                    callbackResults.results = callbackDataResults.results;
                                    callbackResults.data = swatchDropDownList;
                                    callbackResults.resultsCode = callbackDataResults.resultsCode;
                                });

                                loadedSwatchData = loadedDataResults.data;

                                callbackResults.results = $"CreateSwatchDropDownList Sucess : {loadedDataResults.data.name} Loaded.Successfully.";
                                callbackResults.data = swatchDropDownList;
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                            else
                            {
                                if (swatches.Count > 0)
                                {
                                    SwatchData swatchData = new SwatchData(fileName, swatches);

                                    SceneAssetsManager.Instance.CreateData(swatchData, directoryData, (createDataCallback) =>
                                 {
                                        if (Helpers.IsSuccessCode(createDataCallback.resultsCode))
                                        {
                                            loadedSwatchData = createDataCallback.data;

                                            foreach (var swatch in swatches)
                                                if (!swatchDropDownList.Contains(swatch.name))
                                                    swatchDropDownList.Add(swatch.name);

                                            HasSwatchDropDownContent((callbackDataResults) =>
                                            {
                                                callbackResults.results = callbackDataResults.results;
                                                callbackResults.data = swatchDropDownList;
                                                callbackResults.resultsCode = callbackDataResults.resultsCode;
                                            });

                                        }
                                        else
                                        {
                                            callbackResults.results = createDataCallback.results;
                                            callbackResults.data = default;
                                            callbackResults.resultsCode = createDataCallback.resultsCode;
                                        }
                                    });
                                }
                                else
                                {
                                    callbackResults.results = $"CreateSwatchDropDownList Failed : Swatches List Is Empty.";
                                    callbackResults.data = default;
                                    callbackResults.resultsCode = Helpers.ErrorCode;
                                }
                            }
                        });

                    }
                    else
                    {
                        callbackResults.results = $"CreateSwatchDropDownList Failed : Storage Directory : {directoryData.directory} Not Found.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "CreateSwatchDropDownList Failed : SceneAssetsManager.Instance Is Not Yet initialized.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void GetSwatchDropDownList(Action<CallbackDatas<string>> callback)
            {
                CallbackDatas<string> callbackResults = new CallbackDatas<string>();

                HasSwatchDropDownContent((callbackDataResults) =>
                {
                    callbackResults.results = callbackDataResults.results;
                    callbackResults.resultsCode = callbackDataResults.resultsCode;

                    if (Helpers.IsSuccessCode(callbackDataResults.resultsCode))
                        callbackResults.data = swatchDropDownList;
                    else
                        callbackResults.data = default;
                });

                callback?.Invoke(callbackResults);
            }

            void HasSwatchDropDownContent(Action<Callback> callback)
            {
                Callback callbackResults = new Callback();

                if (swatchDropDownList.Count > 0)
                {
                    callbackResults.results = "Has Swatch Drop Down Content Success : Swatch Drop Down List Initialized";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "Has Swatch Drop Down Content Failed : Swatch Drop Down List Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            void HasSwatchDataContent(Action<Callback> callback)
            {
                Callback callbackResults = new Callback();

                if (swatches.Count > 0)
                {
                    #region Default Swatch

                    foreach (var swatch in swatches)
                    {
                        if (swatch.colorSpectrumSize > 0)
                        {
                            var data = SceneAssetsManager.Instance.GetColorInfoSpectrum(swatch.colorSpectrumSize);

                            foreach (var colorInfo in data)
                            {
                                swatch.colorIDList.Add(colorInfo);
                            }
                        }

                        swatch.colorIDList.Add(new ColorInfo());
                    }

                    #endregion

                    callbackResults.results = "Has Swatch Data Content Success : Swatches Initialized";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "Has Swatch Data Content Failed : Swatches Are Not Initialized In The Inspector Panel.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void CreateColorInCustomSwatch(string fileName, string swatchName, ColorInfo colorInfo, DirectoryType directoryType, Action<CallbackDatas<string>> callback = null)
            {
                CallbackDatas<string> callbackResults = new CallbackDatas<string>();

                if (SceneAssetsManager.Instance != null)
                {
                    SceneAssetsManager.Instance.GetColorSwatchData((swatchDataResults) =>
                    {
                        if (Helpers.IsSuccessCode(swatchDataResults.resultsCode))
                        {
                            if (swatchDataResults.data.SwatchDataLoadedSuccessfully())
                            {
                                SwatchData swatchData = swatchDataResults.data.GetLoadedSwatchData();

                                swatchDataResults.data.GetColorSwatchInLoadedData(swatchName, (loadedSwatchDataResults) =>
                                {
                                    if (Helpers.IsSuccessCode(loadedSwatchDataResults.resultsCode))
                                    {
                                    #region Test

                                        swatchDataResults.data.CreateSwatchDropDownList(fileName, (dropDownCreated) =>
                                        {
                                            callbackResults = dropDownCreated;

                                            if (Helpers.IsSuccessCode(dropDownCreated.resultsCode))
                                            {
                                                ColorSwatch swatch = loadedSwatchDataResults.data;

                                                SwatchExistsInSwatches(swatchName, (swatchExistsCallbackResults) =>
                                                {
                                                    if (Helpers.IsSuccessCode(swatchExistsCallbackResults.resultsCode))
                                                    {
                                                        if (swatchExistsCallbackResults.data.colorIDList.Count > 0)
                                                        {
                                                            swatch = swatchExistsCallbackResults.data;

                                                            if (!swatch.colorIDList.Contains(colorInfo))
                                                                swatch.colorIDList.Add(colorInfo);
                                                            else
                                                            {
                                                                callbackResults.results = $"CreateColorInCustomSwatch Failed : Color Info : {colorInfo.hexadecimal} Already Exists In : {swatch.name} swatch.colorIDList Is Null.";
                                                                callbackResults.resultsCode = Helpers.ErrorCode;
                                                            }

                                                            SceneAssetsManager.Instance.CreateColorInfoContent(colorInfo, swatchName, ContentContainerType.ColorSwatches, OrientationType.HorizontalGrid, (callbackDataResults) =>
                                                            {
                                                                if (Helpers.IsSuccessCode(callbackDataResults.resultsCode))
                                                                {
                                                                    SceneAssetsManager.Instance.GetColorSwatchData((colorSwatchDataResults) =>
                                                                    {
                                                                        if (Helpers.IsSuccessCode(colorSwatchDataResults.resultsCode))
                                                                        {
                                                                            colorSwatchDataResults.data.AddColorToSwatch(colorInfo, swatch, (addSwatchCallback) =>
                                                                            {
                                                                                callbackResults.results = addSwatchCallback.results;
                                                                                callbackResults.resultsCode = addSwatchCallback.resultsCode;

                                                                                if (Helpers.IsSuccessCode(addSwatchCallback.resultsCode))
                                                                                {
                                                                                    SwatchData swatchData = new SwatchData(fileName, swatches);

                                                                                    StorageDirectoryData directoryData = SceneAssetsManager.Instance.GetAppDirectoryData(directoryType);

                                                                                    SceneAssetsManager.Instance.CreateData(swatchData, directoryData, (createDataCallback) =>
                                                                                    {
                                                                                        if (Helpers.IsSuccessCode(createDataCallback.resultsCode))
                                                                                        {
                                                                                            swatchDropDownList = new List<string>();

                                                                                            foreach (var colorSwatch in swatchData.swatches)
                                                                                                if (!swatchDropDownList.Contains(colorSwatch.name))
                                                                                                    swatchDropDownList.Add(colorSwatch.name);

                                                                                            HasSwatchDropDownContent((callbackDataResults) =>
                                                                                            {
                                                                                                callbackResults.results = createDataCallback.results;
                                                                                                callbackResults.data = swatchDropDownList;
                                                                                                callbackResults.resultsCode = createDataCallback.resultsCode;
                                                                                            });

                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            callbackResults.results = "CreateColorInCustomSwatch Failed :  SceneAssetsManager.Instance.CreateData Couldn't Load.";
                                                                                            callbackResults.data = default;
                                                                                            callbackResults.resultsCode = Helpers.ErrorCode;
                                                                                        }
                                                                                    });
                                                                                }
                                                                                else
                                                                                    Debug.LogError($"--> CreateColorInCustomSwatch Failed With Results : {addSwatchCallback.results}");

                                                                            });
                                                                        }
                                                                        else
                                                                            Debug.LogError($"--> Adding Color To Swatch Failed With Error Results : {callbackDataResults.results}");
                                                                    });
                                                                }
                                                                else
                                                                    Debug.LogError($"--> Create Color Failed With Error Results : {callbackDataResults.results}");

                                                            });
                                                        }
                                                        else
                                                        {
                                                            callbackResults.results = $"CreateColorInCustomSwatch Failed : Swtach : {swatch.name} swatch.colorIDList Is Null.";
                                                            callbackResults.resultsCode = Helpers.ErrorCode;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        swatches.Add(swatch);

                                                        if (swatch.colorIDList.Count > 0)
                                                        {
                                                            if (!swatch.colorIDList.Contains(colorInfo))
                                                                swatch.colorIDList.Add(colorInfo);
                                                            else
                                                            {
                                                                callbackResults.results = $"CreateColorInCustomSwatch Failed : Color Info : {colorInfo.hexadecimal} Already Exists In : {swatch.name} swatch.colorIDList Is Null.";
                                                                callbackResults.resultsCode = Helpers.ErrorCode;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            callbackResults.results = $"CreateColorInCustomSwatch Failed : Swtach : {swatch.name} swatch.colorIDList Is Null.";
                                                            callbackResults.resultsCode = Helpers.ErrorCode;
                                                        }

                                                        SwatchData swatchData = new SwatchData(fileName, swatches);

                                                        StorageDirectoryData directoryData = SceneAssetsManager.Instance.GetAppDirectoryData(directoryType);

                                                        SceneAssetsManager.Instance.CreateData(swatchData, directoryData, (createDataCallback) =>
                                                        {
                                                            if (Helpers.IsSuccessCode(createDataCallback.resultsCode))
                                                            {
                                                                swatchDropDownList = new List<string>();

                                                                foreach (var colorSwatch in swatchData.swatches)
                                                                    if (!swatchDropDownList.Contains(colorSwatch.name))
                                                                        swatchDropDownList.Add(colorSwatch.name);

                                                                HasSwatchDropDownContent((callbackDataResults) =>
                                                                {
                                                                    callbackResults.results = createDataCallback.results;
                                                                    callbackResults.data = swatchDropDownList;
                                                                    callbackResults.resultsCode = createDataCallback.resultsCode;
                                                                });

                                                            }
                                                            else
                                                            {
                                                                callbackResults.results = "CreateColorInCustomSwatch Failed :  SceneAssetsManager.Instance.CreateData Couldn't Load.";
                                                                callbackResults.data = default;
                                                                callbackResults.resultsCode = Helpers.ErrorCode;
                                                            }
                                                        });
                                                    }
                                                });

                                            }
                                            else
                                                Debug.LogError($"---> On Dropdown Create Failed With Resuts : {dropDownCreated.results}.");
                                        });

                                    #endregion

                                }
                                    else
                                    {
                                        if (!SwatchPalletExist(swatchName))
                                        {

                                            Debug.LogError($"---> On Dropdown Create New Swatch / Pallet");

                                        //ColorSwatchPallet swatchPallet = new ColorSwatchPallet();
                                        //swatchPallet.name = swatchName;

                                        //swatchDataResults.data.AddPallet(swatchPallet);

                                        // --Create New Swatch
                                        ColorSwatch swatch = new ColorSwatch { name = swatchName, colorIDList = new List<ColorInfo>() };
                                            swatch.colorIDList.Add(colorInfo);

                                            if (!swatches.Contains(swatch))
                                                swatches.Add(swatch);

                                            SwatchData swatchData = new SwatchData(fileName, swatches);

                                            StorageDirectoryData directoryData = SceneAssetsManager.Instance.GetAppDirectoryData(directoryType);

                                            SceneAssetsManager.Instance.CreateData(swatchData, directoryData, (createDataCallback) =>
                                            {
                                                if (Helpers.IsSuccessCode(createDataCallback.resultsCode))
                                                {
                                                    swatchDropDownList = new List<string>();

                                                    foreach (var colorSwatch in swatchData.swatches)
                                                        if (!swatchDropDownList.Contains(colorSwatch.name))
                                                            swatchDropDownList.Add(colorSwatch.name);

                                                    HasSwatchDropDownContent((callbackDataResults) =>
                                                    {
                                                        callbackResults.results = createDataCallback.results;
                                                        callbackResults.data = swatchDropDownList;
                                                        callbackResults.resultsCode = createDataCallback.resultsCode;
                                                    });

                                                }
                                                else
                                                {
                                                    callbackResults.results = "CreateColorInCustomSwatch Failed :  SceneAssetsManager.Instance.CreateData Couldn't Load.";
                                                    callbackResults.data = default;
                                                    callbackResults.resultsCode = Helpers.ErrorCode;
                                                }
                                            });
                                        }
                                        else
                                        {
                                            callbackResults.results = $"CreateColorInCustomSwatch Failed : Swatch : {swatchName} Already Exists In swatchPalletList.";
                                            callbackResults.resultsCode = Helpers.ErrorCode;
                                        }
                                    }
                                });
                            }
                            else
                            {
                                callbackResults.results = "CreateColorInCustomSwatch Failed : swatchDataResults Couldn't Load For Some Reason.";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = $"CreateColorInCustomSwatch GetColorSwatchData Failed With Results : {swatchDataResults.results}";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    });
                }
                else
                {
                    callbackResults.results = "CreateColorInCustomSwatch Failed : SceneAssetsManager.Instance Is Not Yet Initialized.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void GetColorSwatchInLoadedData(string swatchName, Action<CallbackData<ColorSwatch>> callback)
            {
                CallbackData<ColorSwatch> callbackResults = new CallbackData<ColorSwatch>();

                if (loadedSwatchData != null && loadedSwatchData.swatches.Count > 0)
                {
                    ColorSwatch swatch = loadedSwatchData.swatches.Find((x) => x.name == swatchName);

                    if (swatch.colorIDList != null && swatch.colorIDList.Count > 0)
                    {
                        callbackResults.results = $"GetColorSwatchInLoadedData Success : Swatch : {swatchName} Data Loaded";
                        callbackResults.data = swatch;
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = "GetColorSwatchInLoadedData Failed : Swatch Data Couldn't Load For Some Reason.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "GetColorSwatchInLoadedData Failed : Swatch Data Couldn't Load For Some Reason.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public SwatchData GetLoadedSwatchData()
            {
                return loadedSwatchData;
            }

            public void SetLoadedSwatchData(SwatchData swatchData)
            {
                loadedSwatchData = swatchData;
            }

            public bool SwatchDataLoadedSuccessfully()
            {
                bool results = false;

                if (loadedSwatchData != null && loadedSwatchData.swatches.Count > 0)
                    results = true;

                return results;
            }

            public void AddColorToSwatch(ColorInfo colorInfo, ColorSwatch swatch, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                HasSwatchDataContent((callbackDataResults) =>
                {
                    callbackResults = callbackDataResults;

                    if (Helpers.IsSuccessCode(callbackDataResults.resultsCode))
                    {
                        if (swatch.colorIDList.Count > 0)
                        {
                            if (!swatch.colorIDList.Contains(colorInfo))
                            {
                                swatch.colorIDList.Add(colorInfo);

                                callbackDataResults.results = "AddColorToSwatch Success : Color Info Added To swatch.colorIDList.";
                                callbackDataResults.resultsCode = Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackDataResults.results = "AddColorToSwatch Failed : Color Info Already Exists In swatch.colorIDList.";
                                callbackDataResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackDataResults.results = "AddColorToSwatch Failed : swatch.colorIDList Is Null.";
                            callbackDataResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                    else
                        Debug.LogError($"--> AddColorToSwatch Failed With Results : {callbackDataResults.results}");
                });

                callback?.Invoke(callbackResults);
            }

            public void AddPallet(ColorSwatchPallet pallet)
            {
                if (!swatchPalletList.Contains(pallet))
                    swatchPalletList.Add(pallet);
            }

            public ColorSwatchPallet GetColorSwatchPallet(string name)
            {
                ColorSwatchPallet swatchPallet = swatchPalletList.Find((x) => x.name == name);

                return swatchPallet;
            }

            public List<ColorSwatchPallet> GetColorSwatchPallets()
            {
                return swatchPalletList;
            }

            public void OnSwatchColorSelection(ColorInfo colorInfo)
            {
                for (int i = 0; i < swatchPalletList.Count; i++)
                    for (int j = 0; j < swatchPalletList[i].swatchButtonList.Count; j++)
                        if (swatchPalletList[i].swatchButtonList[j].GetColorInfo().hexadecimal == colorInfo.hexadecimal)
                            swatchPalletList[i].swatchButtonList[j].SetInputUIButtonState(InputActionButtonType.ColorPickerButton, InputUIState.Selected);
                        else
                            swatchPalletList[i].swatchButtonList[j].SetInputUIButtonState(InputActionButtonType.ColorPickerButton, InputUIState.Deselect);
            }

            public void SwatchExistsInSwatches(string swatchName, Action<CallbackData<ColorSwatch>> callback)
            {
                CallbackData<ColorSwatch> callbackResults = new CallbackData<ColorSwatch>();

                foreach (var swatch in swatches)
                {
                    if (swatch.name == swatchName)
                    {
                        callbackResults.results = $"SwatchExistsInSwatches Success : Color Swatch : {swatchName} Exists In Swatches List.";
                        callbackResults.data = swatch;
                        callbackResults.resultsCode = Helpers.SuccessCode;

                        break;
                    }
                    else
                    {
                        callbackResults.results = $"SwatchExistsInSwatches Failed : Color Swatch : {swatchName} Doesn't Exist In Swatches List.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }

                callback?.Invoke(callbackResults);
            }

            public bool SwatchPalletExist(string nameID)
            {
                return swatchPalletList.Contains(swatchPalletList.Find((x) => x.name == nameID));
            }

            public bool SwatchPalletExist(ColorSwatchPallet pallet)
            {
                return swatchPalletList.Contains(pallet);
            }

            public void ShowPallet(string key)
            {
                foreach (var item in swatchPalletList)
                    item.Hide();

                GetColorSwatchPallet(key).Show();
            }

            public void AddColorInfoToLibrary(ColorInfo colorInfo, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (!colorInfoLibrary.Contains(colorInfo))
                {
                    colorInfo.hexadecimal = Helpers.TrimStringValue(colorInfo.hexadecimal, 6);

                    colorInfoLibrary.Add(colorInfo);

                    callbackResults.results = $"AddColorInfoToLibrary Success : Color : {colorInfo.hexadecimal} Added To Library.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"AddColorInfoToLibrary Failed : Color : {colorInfo.hexadecimal} Already Exists.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void ColorInfoExistsInLibrary(ColorInfo colorInfo, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (colorInfoLibrary.Count > 0)
                {
                    foreach (var color in colorInfoLibrary)
                    {
                        if (color.hexadecimal == Helpers.TrimStringValue(colorInfo.hexadecimal, 6))
                        {
                            callbackResults.results = "ColorInfoExistsInLibrary Success : Color Exists In Library.";
                            callbackResults.resultsCode = Helpers.SuccessCode;

                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = "ColorInfoExistsInLibrary Failed : colorInfoLibrary Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void ColorInfoExistsInSwatch(ColorInfo colorInfo, string swatchName, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (colorInfoLibrary.Count > 0)
                {
                    foreach (var color in colorInfoLibrary)
                    {
                        if (color.hexadecimal == Helpers.TrimStringValue(colorInfo.hexadecimal, 6))
                        {
                            callbackResults.results = "ColorInfoExistsInLibrary Success : Color Exists In Library.";
                            callbackResults.resultsCode = Helpers.SuccessCode;

                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = "ColorInfoExistsInLibrary Failed : colorInfoLibrary Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            #endregion
        }

        [Serializable]
        public class SwatchData : SerializableData
        {
            #region Components

            public List<ColorSwatch> swatches = new List<ColorSwatch>();

            #endregion

            #region Main

            public SwatchData()
            {

            }

            public SwatchData(string name, List<ColorSwatch> swatches)
            {
                this.swatches = swatches;
                this.name = name;
            }

            #endregion
        }

        #endregion

        #region Navigation

        [Serializable]
        public class NavigationWidget
        {
            #region Components

            public List<NavigationTab> navigationTabsList = new List<NavigationTab>();

            public List<NavigationButton> navigationButtonsList = new List<NavigationButton>();

            #endregion

            #region Main

            public void Init(Action<Callback> callback = null)
            {
                try
                {
                    Callback callbackResults = new Callback();

                    // Tabs
                    if (navigationTabsList != null)
                    {
                        foreach (var navigationTab in navigationTabsList)
                        {
                            if (navigationTab.value != null)
                            {
                                navigationTab.Init((navigationTabCallback) =>
                                {
                                    if (Helpers.IsSuccessCode(navigationTabCallback.resultsCode))
                                    {
                                        callbackResults.results = navigationTabCallback.results;
                                        callbackResults.resultsCode = Helpers.SuccessCode;
                                    }
                                    else
                                    {
                                        callbackResults.results = navigationTabCallback.results;
                                        callbackResults.resultsCode = Helpers.ErrorCode;
                                    }
                                });
                            }
                            else
                            {
                                callbackResults.results = "Navigation Button Value Is Missing / Null.";
                                callbackResults.resultsCode = Helpers.ErrorCode;

                                break;
                            }
                        }
                    }
                    else
                    {
                        callbackResults.results = "Navigation Buttons List Is Null / Empty.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }

                    // Buttons
                    if (navigationButtonsList != null)
                    {
                        foreach (var navigationButton in navigationButtonsList)
                        {
                            if (navigationButton.value != null)
                            {
                                navigationButton.Init();
                                navigationButton.value.onClick.AddListener(() => NavigationIDInput(navigationButton.navigationID));
                            }
                            else
                            {
                                callbackResults.results = "Navigation Button Value Is Missing / Null.";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                    }
                    else
                    {
                        callbackResults.results = "Navigation Buttons List Is Null / Empty.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }

                    callback.Invoke(callbackResults);
                }
                catch (Exception e)
                {
                    Debug.LogError($"--> RG_Unity : Init Failed With Exception : {e.Message}");
                    throw e;
                }
            }

            void NavigationIDInput(NavigationTabID navigationID)
            {
                OnNavigationTabChangedEvent(navigationID);
            }

            void OnNavigationTabChangedEvent(NavigationTabID navigationID)
            {
                if (navigationTabsList != null)
                {
                    for (int i = 0; i < navigationTabsList.Count; i++)
                    {
                        if (navigationTabsList[i].navigationID == navigationID)
                        {
                            navigationTabsList[i].Show();
                            UpdateButtonState(navigationID);
                        }
                        else
                            navigationTabsList[i].Hide();
                    }
                }
                else
                    Debug.LogWarning("--> RG_Unity - OnNavigationTabChangedEvent Failed : Navigation Tabs List Is Null / Empty.");
            }

            void UpdateButtonState(NavigationTabID navigationID)
            {
                if (navigationButtonsList != null)
                {
                    for (int i = 0; i < navigationButtonsList.Count; i++)
                        if (navigationButtonsList[i].IsSelected())
                            navigationButtonsList[i].SetButtonState(false);

                    NavigationButton navButton = navigationButtonsList.Find((x) => x.navigationID == navigationID);

                    if (navButton != null)
                        navButton.SetButtonState(true);
                    else
                        Debug.LogWarning($"--> RG_Unity - UpdateButtonState : Navigation Button For ID : {navigationID} Not Found.");
                }
                else
                    Debug.LogWarning("--> RG_Unity - OnNavigationTabChangedEvent Failed : Navigation Tabs List Is Null / Empty.");
            }

            #endregion
        }

        [Serializable]
        public class NavigationSubWidget
        {
            #region Components

            public NavigationTabID navigationTabID;

            [Space(5)]
            public List<NavigationSubTab> navigationSubTabsList = new List<NavigationSubTab>();

            #endregion

            #region Main

            public void Init(NavigationTabID navigationTabID, Action<Callback> callback = null)
            {

                this.navigationTabID = navigationTabID;

                Callback callbackResults = new Callback();

                // Tabs
                if (navigationSubTabsList != null)
                {
                    foreach (var navigationTab in navigationSubTabsList)
                    {
                        if (navigationTab.value != null)
                        {
                            navigationTab.Init();

                            callbackResults.results = "Success.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = "Navigation Button Value Is Missing / Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                }
                else
                {
                    callbackResults.results = "Navigation Buttons List Is Null / Empty.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            public void OnNavigationTabChangedEvent(NavigationRenderSettingsProfileID selectionTypedID)
            {
                if (navigationSubTabsList != null)
                {
                    for (int i = 0; i < navigationSubTabsList.Count; i++)
                    {
                        if (navigationSubTabsList[i].selectionTypedID == selectionTypedID)
                        {
                            navigationSubTabsList[i].Show();
                        }
                        else
                            navigationSubTabsList[i].Hide();
                    }
                }
                else
                    Debug.LogWarning("--> RG_Unity - OnNavigationTabChangedEvent Failed : Navigation Tabs List Is Null / Empty.");
            }

            public void SetNavigationTabID(NavigationTabID tabID)
            {
                navigationTabID = tabID;
            }

            public NavigationTabID GetNavigationTabID()
            {
                return navigationTabID;
            }

            #endregion
        }

        [Serializable]
        public class NavigationTab
        {
            #region Components

            public string name;

            [Space(5)]
            public GameObject value;

            [Space(5)]
            public NavigationTabID navigationID;

            [Space(5)]
            public List<NavigationTabWidget> navigationTabWidgetsList = new List<NavigationTabWidget>();

            [Space(5)]
            public List<UIButton<ButtonDataPackets>> actionButtonsList = new List<UIButton<ButtonDataPackets>>();

            [Space(5)]
            public List<UISlider<SliderDataPackets>> actionSlidersList = new List<UISlider<SliderDataPackets>>();

            [Space(5)]
            public bool initialVisibilityState;

            [Space(5)]
            public bool hasSubNavigationTabs;

            [Space(5)]
            public NavigationSubWidget navigationSubWidget = new NavigationSubWidget();

            #endregion

            #region Main

            public void Init(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (value)
                {
                    if (actionButtonsList.Count > 0)
                    {
                        foreach (var button in actionButtonsList)
                        {
                            if (button.value != null)
                            {
                                button.value.onClick.AddListener(() => OnActionButtonPressedEvent(button.dataPackets, button.dataPackets.actionType));

                                callbackResults.results = "Initialized Successfully";
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = "Button Value Is Null.";
                                callbackResults.resultsCode = Helpers.ErrorCode;

                                break;
                            }
                        }
                    }

                    if (actionSlidersList.Count > 0)
                    {
                        foreach (var slider in actionSlidersList)
                        {
                            if (slider.value != null)
                            {
                                slider.value.onValueChanged.AddListener((value) => OnActionSliderValueChangedEvent(slider.dataPackets, value));

                                callbackResults.results = "Initialized Successfully";
                                callbackResults.resultsCode = Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = "Slider Value Is Null.";
                                callbackResults.resultsCode = Helpers.ErrorCode;

                                break;
                            }
                        }
                    }

                    if (navigationTabWidgetsList.Count > 0)
                    {
                        foreach (var widget in navigationTabWidgetsList)
                        {
                            if (widget.value != null)
                            {
                                widget.Init((callback) =>
                                {
                                    callbackResults.results = callback.results;
                                    callbackResults.resultsCode = callback.resultsCode;
                                });
                            }
                            else
                            {
                                callbackResults.results = "Widget Value Is Null.";
                                callbackResults.resultsCode = Helpers.ErrorCode;

                                break;
                            }
                        }
                    }
                    else
                    {
                        callbackResults.results = $"-----> Nav Tab Widgets For Tab : {name} Missing : {navigationTabWidgetsList.Count}";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }

                    if (hasSubNavigationTabs)
                    {
                        if (navigationSubWidget != null)
                        {
                            navigationSubWidget.Init(navigationID, (navigationCallback) =>
                            {
                                if (Helpers.IsSuccessCode(navigationCallback.resultsCode))
                                {
                                    if (navigationSubWidget.navigationSubTabsList != null)
                                    {
                                        foreach (var subTab in navigationSubWidget.navigationSubTabsList)
                                            subTab.Init();
                                    }
                                    else
                                    {
                                        callbackResults.results = navigationCallback.results;
                                        callbackResults.resultsCode = navigationCallback.resultsCode;
                                    }
                                }
                                else
                                {
                                    callbackResults.results = navigationCallback.results;
                                    callbackResults.resultsCode = navigationCallback.resultsCode;
                                }

                            });
                        }
                        else
                        {
                            callbackResults.results = "Show Navigation Tab Failed: Subwidget Is Missing / Null.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }

                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                        if (initialVisibilityState)
                            Show();
                        else
                            Hide();
                }
                else
                {
                    callbackResults.results = "Show Navigation Tab Failed: Value Is Missing / Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            public void Show()
            {
                if (value)
                    value.SetActive(true);
                else
                    Debug.LogWarning("--> Show Navigation Tab Failed : Value Is Missing / Null.");
            }

            public void Hide()
            {
                if (value)
                    value.SetActive(false);
                else
                    Debug.LogWarning("--> Hide Navigation Tab Failed : Value Is Missing / Null.");
            }

            public void ShowWidget(NavigationTabType widgetType, Action<Callback> callback)
            {
                Callback callbackResults = new Callback();

                if (navigationTabWidgetsList != null)
                {
                    NavigationTabWidget tabWidget = navigationTabWidgetsList.Find((x) => x.tabType == widgetType);

                    if (tabWidget != null)
                    {
                        if (tabWidget.IsInitialized())
                        {
                            tabWidget.Show();

                            callbackResults.results = $"ShowWidget Success : NavigationTabWidget : {tabWidget.name} Is Initialized.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"ShowWidget Failed : NavigationTabWidget : {tabWidget.name} Is Not Initialized.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = "ShowWidget Failed : NavigationTabWidget Not Found In navigationTabWidgetsList.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "ShowWidget Failed : navigationTabWidgetsList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            public void ShowWidget(NavigationTabType widgetType, NavigationTabID navigationID, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (navigationTabWidgetsList != null)
                {
                    if (this.navigationID == navigationID)
                    {
                        NavigationTabWidget tabWidget = navigationTabWidgetsList.Find((x) => x.tabType == widgetType);

                        if (tabWidget != null)
                        {
                            if (tabWidget.IsInitialized())
                            {
                                tabWidget.Show();
                            }
                            else
                            {
                                callbackResults.results = $"ShowWidget Failed : NavigationTabWidget : {tabWidget.name} Is Not Initialized.";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = "ShowWidget Failed : NavigationTabWidget Not Found In navigationTabWidgetsList.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"ShowWidget Failed : Selected Navigation Tab ID : {navigationID} Doesn't Match The Current Navigation ID : {this.navigationID}.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "ShowWidget Failed : navigationTabWidgetsList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            public void HideWidget(NavigationTabType widgetType, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (navigationTabWidgetsList != null)
                {
                    NavigationTabWidget tabWidget = navigationTabWidgetsList.Find((x) => x.tabType == widgetType);

                    if (tabWidget != null)
                    {
                        if (tabWidget.IsInitialized())
                        {
                            callbackResults.results = $"HideWidget Success : NavigationTabWidget : {tabWidget.name} Initialized.";
                            callbackResults.resultsCode = Helpers.SuccessCode;

                            tabWidget.Hide();
                        }
                        else
                        {
                            callbackResults.results = $"HideWidget Failed : NavigationTabWidget : {tabWidget.name} Is Not Initialized.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = "HideWidget Failed : NavigationTabWidget Not Found In navigationTabWidgetsList.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "HideWidget Failed : navigationTabWidgetsList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            public void HideWidget(NavigationTabType widgetType, NavigationTabID navigationID, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (navigationTabWidgetsList != null)
                {
                    if (this.navigationID == navigationID)
                    {

                        NavigationTabWidget tabWidget = navigationTabWidgetsList.Find((x) => x.tabType == widgetType);

                        if (tabWidget != null)
                        {
                            if (tabWidget.IsInitialized())
                            {
                                tabWidget.Hide();
                            }
                            else
                            {
                                callbackResults.results = $"HideWidget Failed : NavigationTabWidget : {tabWidget.name} Is Not Initialized.";
                                callbackResults.resultsCode = Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = "HideWidget Failed : NavigationTabWidget Not Found In navigationTabWidgetsList.";
                            callbackResults.resultsCode = Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"HideWidget Failed : Selected Navigation Tab ID : {navigationID} Doesn't Match The Current Navigation ID : {this.navigationID}.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "HideWidget Failed : navigationTabWidgetsList Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback.Invoke(callbackResults);
            }

            void OnActionButtonPressedEvent(ButtonDataPackets dataPackets, InputActionButtonType actionType, Action<Callback> callback = null)
            {
                Debug.LogError($"==> Open Settings : {actionType}");

                switch (actionType)
                {
                    case InputActionButtonType.CreateSkyboxButton:

                        RenderingSettingsManager.Instance.UpdateScreenWidgetInfo = false;
                        ActionEvents.OnNavigationTabWidgetEvent(dataPackets);

                        break;

                    case InputActionButtonType.Edit:

                        RenderingSettingsManager.Instance.UpdateScreenWidgetInfo = true;
                        ActionEvents.OnNavigationTabWidgetEvent(dataPackets);

                        break;

                    case InputActionButtonType.OpenColorPicker:

                        ActionEvents.OnNavigationTabWidgetEvent(dataPackets);

                        if (RenderingSettingsManager.Instance != null)
                        {
                            if (SceneAssetsManager.Instance != null)
                            {
                                SceneAssetsManager.Instance.GetHexidecimalFromColor(RenderingSettingsManager.Instance.GetRenderingSettingsData().GetLightingSettingsData().GetLightColor(), (callbackResults) =>
                                {
                                    if (Helpers.IsSuccessCode(callbackResults.resultsCode))
                                        ActionEvents.OnSwatchColorPickedEvent(callbackResults.data, false, true);
                                    else
                                        Debug.LogError($"--> OnActionButtonPressedEvent OpenColorPicker Failed With Results : {callbackResults.results}");
                                });
                            }
                            else
                                Debug.LogWarning("OnActionButtonPressedEvent OpenColorPicker Failed : SceneAssetsManager Instance Is Not Yet Initialized.");
                        }
                        else
                            Debug.LogWarning("OnActionButtonPressedEvent OpenColorPicker Failed : RenderingSettingsManager Instance Is Not Yet Initialized.");

                        break;

                    case InputActionButtonType.CreateNewProfileButton:

                        ActionEvents.OnNavigationTabWidgetEvent(dataPackets);

                        break;

                    case InputActionButtonType.DuplicateButton:

                        if (ScreenUIManager.Instance != null)
                        {
                            if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                            {
                                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == dataPackets.screenType)
                                {
                                    if (dataPackets.tabID == navigationID)
                                    {
                                        if (SceneAssetsManager.Instance != null)
                                        {
                                            SceneAssetsManager.Instance.Duplicate((duplicateCallback) =>
                                            {
                                                if (Helpers.IsSuccessCode(duplicateCallback.resultsCode))
                                                    Debug.Log($"-------------------> RG_Unity: {duplicateCallback.results}");
                                                else
                                                    Debug.LogWarning($"--> Failed To Create With Results : {duplicateCallback.results}");

                                            });
                                        }
                                        else
                                            Debug.LogWarning("--> RG_Unity - OnActionButtonPressedEvent : Scene Assets Manager Instance Not Yet Initialized.");
                                    }
                                    else
                                        return;
                                }
                                else
                                    return;
                            }
                            else
                                Debug.LogWarning("--> RG_Unity - OnActionButtonPressedEvent : Screen UI Manager Instance's  Get Current Screen Data Value Is Missing / Null.");
                        }
                        else
                            Debug.LogWarning("--> RG_Unity - OnActionButtonPressedEvent : Screen UI Manager Instance Not Yet Initialized.");

                        break;

                    case InputActionButtonType.ClearAllButton:

                        if (ScreenUIManager.Instance != null)
                        {
                            if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                            {
                                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == dataPackets.screenType)
                                {
                                    if (dataPackets.tabID == navigationID)
                                    {
                                        if (SceneAssetsManager.Instance != null)
                                        {
                                            SceneAssetsManager.Instance.ClearAllRenderProfiles((clearAllCallback) =>
                                            {
                                                if (Helpers.IsSuccessCode(clearAllCallback.resultsCode))
                                                    Debug.Log($"-------------------> RG_Unity: {clearAllCallback.results}");
                                                else
                                                    Debug.LogWarning($"--> Failed To Create With Results : {clearAllCallback.results}");

                                            });
                                        }
                                        else
                                            Debug.LogWarning("--> RG_Unity - OnActionButtonPressedEvent : Scene Assets Manager Instance Not Yet Initialized.");
                                    }
                                    else
                                        return;
                                }
                                else
                                    return;
                            }
                            else
                                Debug.LogWarning("--> RG_Unity - OnActionButtonPressedEvent : Screen UI Manager Instance's  Get Current Screen Data Value Is Missing / Null.");
                        }
                        else
                            Debug.LogWarning("--> RG_Unity - OnActionButtonPressedEvent : Screen UI Manager Instance Not Yet Initialized.");

                        break;
                }
            }

            void OnActionSliderValueChangedEvent(SliderDataPackets dataPackets, float value)
            {
                switch (dataPackets.valueType)
                {
                    case SliderValueType.LightIntensity:

                        RenderingSettingsManager.Instance.GetRenderingSettingsData().CurrentSkyboxSettings.SetLightIntensity(value);

                        break;

                    case SliderValueType.SkyboxExposure:

                        float exposure = GetValueFormatted(value, 8);

                        Debug.LogError($"==> Exposure : {exposure}");

                        RenderingSettingsManager.Instance.GetRenderingSettingsData().CurrentSkyboxSettings.SetSkyBoxExposure(exposure);

                        break;

                    case SliderValueType.SkyboxRotationSpeed:

                        RenderingSettingsManager.Instance.GetRenderingSettingsData().CurrentSkyboxSettings.SkyboxRotationSpeed = GetValueFormatted(value, 100);

                        break;
                }
            }

            float GetValueFormatted(float value, float multiplier)
            {
                return value * multiplier;
            }

            #endregion
        }

        [Serializable]
        public class NavigationSubTab
        {
            #region Components

            public string name;

            [Space(5)]
            public GameObject value;

            [Space(5)]
            public NavigationRenderSettingsProfileID selectionTypedID;

            [Space(5)]
            public bool initialVisibilityState;

            #endregion

            #region Main

            public void Init()
            {
                if (value)
                {
                    if (initialVisibilityState)
                        Show();
                    else
                        Hide();

                    Debug.Log($"---------> Initialized Sub Tab : {name}");
                }
                else
                    Debug.LogWarning("--> Show Navigation Tab Failed : Value Is Missing / Null.");
            }

            public void Show()
            {
                if (value)
                    value.SetActive(true);
                else
                    Debug.LogWarning("--> Show Navigation Tab Failed : Value Is Missing / Null.");
            }

            public void Hide()
            {
                if (value)
                    value.SetActive(false);
                else
                    Debug.LogWarning("--> Hide Navigation Tab Failed : Value Is Missing / Null.");
            }

            #endregion
        }

        [Serializable]
        public class NavigationButton
        {
            #region Components

            public string name;

            [Space(5)]
            public Button value;

            [Space(5)]
            public NavigationTabID navigationID;


            [Space(5)]
            public bool initialSelectionState;

            bool isSelected;

            #endregion

            #region Main

            public void Init()
            {
                isSelected = initialSelectionState;
            }

            public void SetButtonState(bool isSelected) => this.isSelected = isSelected;

            public bool IsSelected()
            {
                return isSelected;
            }

            #endregion
        }

        [Serializable]
        public class NavigationTabWidget
        {
            #region Components

            public string name;

            [Space(5)]
            public GameObject value;

            [Space(5)]
            public List<UIButton<ButtonDataPackets>> actionButtonsList = new List<UIButton<ButtonDataPackets>>();

            [Space(5)]
            public List<UIDropDown<ButtonDataPackets>> actionDropDownList = new List<UIDropDown<ButtonDataPackets>>();

            [Space(5)]
            public NavigationTabType tabType;

            [Space(5)]
            public bool initialVisibilityState;

            [SerializeField]
            bool isInitialized = false;

            #endregion

            #region Main

            public void Init(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (actionButtonsList.Count > 0)
                {
                    foreach (var button in actionButtonsList)
                    {
                        if (button.value)
                        {
                            button.value.onClick.AddListener(() => OnActionButtonClickedEvent(button.dataPackets));

                            callbackResults.results = "Initialized Successfully.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = "Init Failed : Button Value Is Null.";
                            callbackResults.resultsCode = Helpers.SuccessCode;

                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = "Initialized Successfully.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }

                if (actionDropDownList.Count > 0)
                {
                    foreach (var dropDown in actionDropDownList)
                    {
                        if (dropDown.value)
                        {
                            switch (dropDown.actionType)
                            {
                                case InputDropDownActionType.RenderingProfileType:

                                    if (SceneAssetsManager.Instance != null)
                                    {

                                        List<string> profileTypeList = SceneAssetsManager.Instance.GetFormatedDropDownContentList(SceneAssetsManager.Instance.GetDropDownContentData(AppData.DropDownContentType.RenderProfiles).data);

                                        if (profileTypeList != null)
                                        {
                                            dropDown.value.ClearOptions();

                                            List<TMP_Dropdown.OptionData> dropdownOption = new List<TMP_Dropdown.OptionData>();

                                            foreach (var profile in profileTypeList)
                                                dropdownOption.Add(new TMP_Dropdown.OptionData() { text = profile });

                                            dropDown.value.AddOptions(dropdownOption);

                                            dropDown.value.onValueChanged.AddListener((value) => OnDropDownExtensionsOptions(value));
                                        }
                                        else
                                            Debug.LogWarning("--> SelectedSceneAssetPreviewWidget : Export Extension Drop Down Extensions List Not Found In Scene Assets Manager.");
                                    }
                                    else
                                        Debug.LogWarning("--> Init Failed : Scene Assets Manager Instance Is Not Yet Initialized.");

                                    break;
                            }

                            callbackResults.results = "Initialized Successfully.";
                            callbackResults.resultsCode = Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = "Init Failed : Drop Down Value Is Null.";
                            callbackResults.resultsCode = Helpers.SuccessCode;

                            break;
                        }
                    }
                }
                else
                {
                    callbackResults.results = "Initialized Successfully.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }

                isInitialized = (callbackResults.resultsCode == Helpers.SuccessCode)? true : false;

                if (isInitialized)
                    if (initialVisibilityState)
                        Show();
                    else
                        Hide();

                callback.Invoke(callbackResults);
            }

            void OnActionButtonClickedEvent(ButtonDataPackets dataPackets)
            {
                Debug.Log($"----> Clicked On Button Type : {dataPackets.actionType}");

                switch (dataPackets.actionType)
                {
                    case InputActionButtonType.Confirm:

                        switch (dataPackets.navigationTabWidgetType)
                        {
                            case NavigationTabType.CreateRenderProfileWidget:

                                if (ScreenUIManager.Instance != null)
                                {
                                    if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                                    {
                                        if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == dataPackets.screenType)
                                        {
                                            if (dataPackets.tabID == NavigationTabID.PostProcessingSettings)
                                            {
                                                if (SceneAssetsManager.Instance != null)
                                                {
                                                    SceneAssetsManager.Instance.CreateNewRenderProfile(dataPackets, (createRendererCallback) =>
                                                    {
                                                        if (Helpers.IsSuccessCode(createRendererCallback.resultsCode))
                                                            ActionEvents.OnNavigationTabWidgetEvent(dataPackets);
                                                        else
                                                            Debug.LogWarning($"--> Failed To Create With Results : {createRendererCallback.results}");
                                                    });
                                                }
                                                else
                                                    Debug.LogWarning("--> RG_Unity - OnActionButtonPressedEvent : Rendering Settings Manager Instance Not Yet Initialized.");
                                            }
                                            else
                                                return;
                                        }
                                        else
                                            return;
                                    }
                                    else
                                        Debug.LogWarning("--> RG_Unity - OnActionButtonPressedEvent : Screen UI Manager Instance's  Get Current Screen Data Value Is Missing / Null.");
                                }
                                else
                                    Debug.LogWarning("--> RG_Unity - OnActionButtonPressedEvent : Screen UI Manager Instance Not Yet Initialized.");

                                break;
                        }

                        break;

                    case InputActionButtonType.Cancel:

                        ActionEvents.OnNavigationTabWidgetEvent(dataPackets);

                        break;

                    case InputActionButtonType.HideNavigationScreenWidget:

                        Debug.LogError("-----------> Hide Widget");

                        ActionEvents.OnNavigationTabWidgetEvent(dataPackets);

                        break;
                }
            }

            void OnDropDownExtensionsOptions(int dropdownIndex)
            {
                if (SceneAssetsManager.Instance)
                    SceneAssetsManager.Instance.SetNewRenderProfileID((NavigationRenderSettingsProfileID)dropdownIndex);
                else
                    Debug.LogWarning("--> RG_Unity - OnDropDownExtensionsOptions Failed : Scene Assets Manager Instance Not Yet Initialized.");
            }

            public void Show()
            {
                if (value != null)
                    value.SetActive(true);
                else
                    Debug.LogWarning("--> RG_Unity - Init Failed : NavigationTabWidget Value Is Missing / Null.");
            }

            public void Hide()
            {
                if (value != null)
                    value.SetActive(false);
                else
                    Debug.LogWarning("--> RG_Unity - Init Failed : NavigationTabWidget Value Is Missing / Null.");
            }

            public NavigationTabType GetNavigationTabWidgetType()
            {
                return tabType;
            }

            public bool IsInitialized()
            {
                return isInitialized;
            }

            #endregion
        }

        #endregion

        [Serializable]
        public class ScreenBlurObject
        {
            #region Components

            [Space(5)]
            public string name;

            [Space(5)]
            public GameObject value;

            [Space(5)]
            public List<ScreenBlurObjectContainer> displayLayerContainerList = new List<ScreenBlurObjectContainer>();

            [Space(5)]
            public bool initialVisibilityState = false;

            CanvasGroup canvasGroup;

            #endregion

            #region Main

            public void Init<T>(T fromClass, Action<Callback> callback = null) where T : AppMonoBaseClass
            {
                Callback callbackResults = new Callback();

                if (HasBlurObject())
                {
                    if (initialVisibilityState)
                        Show(ScreenBlurContainerLayerType.Default);

                    callbackResults.results = "Screen Blur Initialized.";
                    callbackResults.resultsCode = Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Initialize Screen Blur : {name} From Class : {fromClass.name} Value Missing / Not Assigned In The Editor Inspector.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void Show(ScreenBlurContainerLayerType layerType, bool fade = false, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (HasBlurObject())
                {
                    AddToSelectedContainer(layerType, addedToContainerCallback => 
                    {
                        bool isVisible = addedToContainerCallback.resultsCode == Helpers.SuccessCode;

                        OnSetBlurObjectVisibilityState(isVisible);
                        callbackResults = addedToContainerCallback;
                    });
                }
                else
                {
                    callbackResults.results = $"Show Blur Object Of Layer Type : {layerType} Failed : Screen Blur Object Value Is Missing / Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public void SetBlurValue(float value)
            {
                if (HasBlurObject())
                    OnSetBlurObjectAlphaValue(value);
                else
                    Debug.LogWarning("Show Blur Object Failed : Screen Blur Object Canvas Group Is Missing / Null.");
            }

            public void Hide(bool resetDisplayLayer = true, bool fade = false)
            {
                if (HasBlurObject())
                {
                    OnSetBlurObjectVisibilityState(false);

                    if (resetDisplayLayer)
                        AddToSelectedContainer(ScreenBlurContainerLayerType.Default);
                }
                else
                    Debug.LogWarning("--> RG_Unity - Show Blur Object Failed : Screen Blur Object Value Is Missing / Null.");
            }

            void AddToSelectedContainer(ScreenBlurContainerLayerType layerType, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                if (displayLayerContainerList != null)
                {
                    ScreenBlurObjectContainer container = displayLayerContainerList.Find((x) => x.containerLayerType == layerType);

                    if (container.HasValueAssigned())
                    {
                        OnSetBlurObjectContainer(container.GetValueAssigned(), false);

                        callbackResults.results = $"Container Of Type : {layerType} Found.";
                        callbackResults.resultsCode = Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"Container Of Type : {layerType} Value Is Missing Null.";
                        callbackResults.resultsCode = Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"Add To Selected Container Of Type : {layerType} Failed : Display Layer Container List Is Null.";
                    callbackResults.resultsCode = Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public float GetAlphaValue()
            {
                return canvasGroup.alpha;
            }

            public bool HasBlurObject()
            {
                if (value != null)
                {
                    if (canvasGroup == null)
                        if(value?.GetComponent<CanvasGroup>() == null)
                            canvasGroup = value?.AddComponent<CanvasGroup>();
                        else
                            canvasGroup = value?.GetComponent<CanvasGroup>();
                }

                return value && canvasGroup;
            }

            public bool GetActive()
            {
                if (!HasBlurObject())
                    return false;

                return value.activeSelf && value.activeInHierarchy;
            }

            public void OnSetBlurObjectVisibilityState(bool isVisible) => value.SetActive(isVisible);

            public void OnSetBlurObjectAlphaValue(float value) => canvasGroup.alpha = value;

            public void OnSetBlurObjectContainer(Transform value, bool keepWorldPos) => value.transform.SetParent(value, keepWorldPos);

            #endregion
        }

        [Serializable]
        public class RenderingSettingsData
        {
            #region Components

            [Space(5)]
            public LightingSettings lightingSettings = new LightingSettings();

            [Space(5)]
            public Material defaultMaterial;

            [Space(5)]
            public List<SkyboxSettings> skyboxDataList = new List<SkyboxSettings>();

            public LightingSettings CurrentSkyboxSettings { get { return lightingSettings; } set { lightingSettings = value; } }


            #endregion

            #region Main

            public LightingSettings GetLightingSettingsData()
            {
                return CurrentSkyboxSettings;
            }

            public void SetCurrentSkybox(Material skybox) => RenderSettings.skybox = skybox;
            public void SetCurrentSkybox(Texture2D skyboxTexture) => RenderSettings.skybox.SetTexture("_MainTex", skyboxTexture);

            public Material GetCurrentSkybox()
            {
                return RenderSettings.skybox;
            }

            public Texture2D GetCurrentSkyboxTexture()
            {
                return (Texture2D)RenderSettings.skybox.mainTexture;
            }

            public void SetSkyboxRotation(float rotationAngle) => RenderSettings.skybox.SetFloat("_Rotation", rotationAngle);

            public void RotateSkybox(float rotationSpreed) => RenderSettings.skybox.SetFloat("_Rotation", rotationSpreed * Time.time);

            public Material GetDefaultMaterial()
            {
                return defaultMaterial;
            }

            #endregion
        }

        [Serializable]
        public class SkyboxSettings
        {
            #region Components

            public string name;

            [Space(5)]
            public LightingSettingsData lightingSettings = new LightingSettingsData();

            public Material skybox;

            #endregion

            #region Main

            public void Initialize()
            {

            }

            public void SetSkybox(Material skybox) => this.skybox = skybox;

            public Material GetSkybox()
            {
                return skybox;
            }

            #endregion
        }


        [Serializable]
        public class SkyboxSettingsData
        {
            #region Components

            public string name;


            #endregion

            #region Main

            #endregion
        }


        // Guard Code Properly For Errors.
        [Serializable]
        public class LightingSettings
        {
            #region Components

            public Light sceneLight;

            [Space(5)]
            public Material sceneSkybox;

            public float LightRotationSpeed { get; set; }

            public float SkyboxRotationSpeed { get { return RenderSettings.skybox.GetFloat("_Rotation"); } set { RenderSettings.skybox.SetFloat("_Rotation", value); } }

            #endregion

            #region Main

            public Light GetSceneLight()
            {
                return sceneLight;
            }

            public void SetLightIntensity(float intensity)
            {
                if (sceneLight != null)
                    sceneLight.intensity = intensity;
                else
                    Debug.LogWarning("--> SetLightColor Failed : Light Value Missing");
            }

            public float GetLightIntensity()
            {
                if (sceneLight != null)
                    return sceneLight.intensity;
                else
                {
                    Debug.LogWarning("--> SetLightColor Failed : Light Value Missing");
                    return 0.0f;
                }
            }

            public void SetLightColor(Color color)
            {
                if (sceneLight != null)
                    sceneLight.color = color;
                else
                    Debug.LogWarning("--> SetLightColor Failed : Light Value Missing");
            }

            public void SetLightColorInfo(ColorInfo colorInfo)
            {
                if (sceneLight != null)
                    sceneLight.color = colorInfo.color;
                else
                    Debug.LogWarning("--> SetLightColor Failed : Light Value Missing");
            }

            public Color GetLightColor()
            {
                if (sceneLight != null)
                    return sceneLight.color;
                else
                {
                    Debug.LogError("==> RG_Unity - Scene Light Missing / Null. Adress This.................");
                    return Color.clear;
                }
            }

            public void SetSkyBoxTexture(Texture2D skyboxTexture = null)
            {
                if (sceneSkybox != null)
                {
                    Debug.LogError($"==> RG_Unity - Setting Skybox To Texture : {skyboxTexture.name}");

                    if (skyboxTexture != null)
                    {
                        sceneSkybox.SetTexture("_MainTex", skyboxTexture);
                        RenderSettings.skybox = sceneSkybox;
                        DynamicGI.UpdateEnvironment();
                    }
                    else
                        Debug.LogWarning("--> SetSkyBoxTexture Failed : Skybox Texture Missing / Null.");
                }
                else
                    Debug.LogWarning("--> SetSkyBoxTexture Failed : Scene Skybox Is Missing / Null.");
            }

            public void SetSkyBoxExposure(float exposure)
            {
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Custom;
                RenderSettings.skybox.SetFloat("_Exposure", exposure);
                RenderSettings.skybox.SetFloat("_Rotation", 0);
            }

            public float GetSkyBoxValue(string valueName)
            {
                return RenderSettings.skybox.GetFloat(valueName);
            }

            #endregion
        }

        [Serializable]
        public struct LightingSettingsData
        {
            #region Components

            public LightingData lightingData;

            [Space(5)]
            public StorageDirectoryData hdrTextureDirectoryData;

            //[HideInInspector]
            public Texture2D hdrTexture;

            #endregion

            #region Main

            public Texture2D GetSkyboxHDRTexture()
            {
                if (hdrTexture != null)
                    return hdrTexture;
                else
                {
                    hdrTexture = Helpers.LoadTextureFile(hdrTextureDirectoryData.directory);
                    return hdrTexture;
                }
            }

            #endregion
        }

        [Serializable]
        public struct LightingData
        {
            #region Components

            public float lightIntensity;

            [Space(5)]
            public float skyboxExposure;

            [Space(5)]
            public ColorInfo lightColor;

            [Space(5)]
            public ColorInfo lightTemperature;

            #endregion
        }

        #endregion

        #region Struct Data

        [Serializable]
        public class UIScreenViewComponent
        {
            public string name;

            [Space(5)]
            public UIScreenHandler value;
        }

        [Serializable]
        public class ColorData
        {
            #region Components

            public Color color;
            public ScreenCoordinates coordinates;

            #endregion
        }

        [Serializable]
        public class ScreenCoordinates
        {
            #region Components

            public int x;
            public int y;

            #endregion

            #region Main

            public ScreenCoordinates()
            {

            }

            public ScreenCoordinates(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            #endregion
        }

        [Serializable]
        public struct UIScreenDimensions
        {
            public int width;

            [Space(5)]
            public int height;
        }

        [Serializable]
        public struct UILayoutDimensions
        {
            public string name;

            [Space(5)]
            public UIScreenDimensions containerDimensions;

            [Space(5)]
            public LayoutViewType layoutView;
        }

        [Serializable]
        public struct ScreenBounds
        {
            public int left;

            [Space(5)]
            public int right;

            [Space(5)]
            public int top;

            [Space(5)]
            public int bottom;
        }

        [Serializable]
        public struct ReferencedActionButtonData
        {
            public string title;
            public InputActionButtonType type;
            public InputUIState state;
        }

        #region Data Packets

        [Serializable]
        public class DataPackets
        {
            [Space(5)]
            [Header("Default Data Packet")]

            [Space(5)]
            public InputType inputType;

            [Space(5)]
            public SelectionOption selectionOption;

            [Space(5)]
            public UIScreenType screenType;

            [Space(5)]
            public ContentContainerType containerType;

            [Space(5)]
            public UIStateType stateType;

            [Space(5)]
            public bool showNotification;

            [Space(5)]
            public Notification notification;
        }

        [Serializable]
        public class SceneDataPackets : DataPackets
        {
            [Space(5)]
            [Header("Scene Data")]

            [Space(5)]
            public UIScreenWidget<SceneDataPackets> screenWidgetPrefab;

            [Space(5)]
            public DynamicWidgetsContainer dynamicWidgetsContainer;

            [Space(5)]
            public bool clearContentContainer;

            [Space(5)]
            public bool resetContentContainerPose;

            [Space(5)]
            public bool scaleSceneAsset;

            [Space(5)]
            public bool keepAssetWorldPose;

            [Space(5)]
            public bool keepAssetCentered;

            [Space(5)]
            public SceneAsset sceneAsset;

            [Space(5)]
            public RuntimeValueType sceneAssetScaleValueType;

            [Space(5)]
            public OrientationType dynamicWidgetsContainerOrientation;

            [Space(5)]
            public WidgetType widgetType;

            [Space(5)]
            public Vector2 widgetScreenPosition;

            [Space(5)]
            public AssetFieldSettingsType assetFieldConfiguration;

            [Space(5)]
            public DirectoryType storageDirectoryType;

            [Space(5)]
            public FolderStructureType folderStructureType;

            [Space(5)]
            public PermissionType requiredPermission;

            [Space(5)]
            public LoadingItemType screenRefreshLoadingItemType;

            [Space(5)]
            public SceneMode sceneMode;

            [Space(5)]
            public EventCameraState sceneAssetMode;

            [Space(5)]
            public bool blurScreen;

            [Space(5)]
            public ScreenBlurContainerLayerType blurContainerLayerType;

            [Space(5)]
            public ScreenViewState screenViewState;

            [Space(5)]
            public bool refreshScreenOnLoad;

            [Space(5)]
            public bool refreshSceneAssets;

            [Space(5)]
            public float refreshDuration;

            [Space(5)]
            public string widgetTitle;

            [Space(5)]
            public int widgetTitleCharacterLimit;

            [Space(5)]
            public string popUpMessage;

            [Space(5)]
            public float sliderValue;

            [Space(5)]
            public bool canTransitionScreen;

            [Space(5)]
            public float screenTransitionSpeed;

            [Space(5)]
            public bool canTransitionSceneAsset;

            [Space(5)]
            public bool overrideSceneAssetTargetPosition;

            [Space(5)]
            public Vector3 sceneAssetTransitionalTargetPosition;

            [Space(5)]
            public bool overrideSceneAssetTransitionSpeed;

            [Space(5)]
            public float sceneAssetTransitionSpeed;

            [HideInInspector]
            public bool isRootFolder;

            [HideInInspector]
            public List<ReferencedActionButtonData> referencedActionButtonDataList;

            public new string ToString()
            {
                string results = $"Screen Type {screenType.ToString()},  Pop Up Type : {widgetType.ToString()}, asse Field Config : {assetFieldConfiguration.ToString()}, Directory Type : {storageDirectoryType.ToString()}";

                return results;
            }
        }

        [Serializable]
        public class ButtonDataPackets : DataPackets
        {
            [Space(5)]
            [Header("Button Data Packet")]

            [Space(5)]
            public InputActionButtonType actionType;

            [Space(5)]
            public NavigationTabID tabID;

            [Space(5)]
            public NavigationTabType navigationTabWidgetType;

            [Space(5)]
            public NavigationWidgetVisibilityState navigationWidgetVisibilityState;

            [Space(5)]
            public OrientationType containerContentOrientation;

            [Space(5)]
            public FocusedWidgetOrderType contentFocusedWidgetOrderType;
        }

        [Serializable]
        public class InputFieldDataPackets : DataPackets
        {
            [Space(5)]
            [Header("Input Field Data Packet")]

            [Space(5)]
            public InputFieldActionType actionType;

            [Space(5)]
            public NavigationTabID tabID;

            [Space(5)]
            public NavigationWidgetVisibilityState visibilityState;
        }

        [Serializable]
        public class DropdownDataPackets : DataPackets
        {
            [Space(5)]
            [Header("Dropdown Data Packet")]

            [Space(5)]
            public InputDropDownActionType actionType;

            [Space(5)]
            public NavigationTabID tabID;

            [Space(5)]
            public NavigationWidgetVisibilityState visibilityState;
        }

        [Serializable]
        public class SliderDataPackets : DataPackets
        {
            [Space(5)]
            [Header("Slider Data Packet")]

            [Space(5)]
            public SliderValueType valueType;

            [Space(5)]
            public ColorValueType colorValue;

            [Space(5)]
            public NavigationTabID tabID;

            [Space(5)]
            public NavigationWidgetVisibilityState visibilityState;

            [Space(5)]
            public bool initialVisibilityState;
        }

        [Serializable]
        public class InputSliderDataPackets : DataPackets
        {
            [Space(5)]
            [Header("Input Slider Data Packet")]

            [Space(5)]
            public InputSliderActionType actionType;

            [Space(5)]
            public NavigationTabID tabID;

            [Space(5)]
            public NavigationWidgetVisibilityState visibilityState;
        }

        [Serializable]
        public class CheckboxDataPackets : DataPackets
        {
            [Space(5)]
            [Header("Checkbox Data Packet")]

            [Space(5)]
            public CheckboxInputActionType actionType;

            [Space(5)]
            public NavigationTabID tabID;

            [Space(5)]
            public NavigationWidgetVisibilityState visibilityState;
        }

        [Serializable]
        public class TextDataPackets : DataPackets
        {
            [Space(5)]
            [Header("Text Data Packet")]

            [Space(5)]
            public ScreenUITextType textType;

            [Space(5)]
            public NavigationTabID tabID;

            [Space(5)]
            public NavigationWidgetVisibilityState visibilityState;
        }

        [Serializable]
        public class ImageDataPackets : DataPackets
        {
            [Space(5)]
            [Header("Image Data Packet")]

            [Space(5)]
            public ScreenImageType imageType;

            [Space(5)]
            public UIScreenDimensions resolution;

            [Space(5)]
            public bool preserveAspectRatio;

            [Space(5)]
            public bool useData;
        }

        [Serializable]
        public class SettingsDataPackets : DataPackets
        {
            [Space(5)]
            [Header("Panel Widget Data Packet")]

            [Space(5)]
            public InputDropDownActionType actionType;

            [Space(5)]
            public SettingsWidgetTabID widgetTabID;

            [Space(5)]
            public NavigationTabID tabID;

            [Space(5)]
            public NavigationWidgetVisibilityState visibilityState;
        }

        #endregion

        #endregion

        #region Static Classess

        public static class Helpers
        {
            public static WaitForSeconds GetWaitForSeconds(float seconds)
            {
                return new WaitForSeconds(seconds);
            }

            public static Vector2 GetScreenToWorldPosition(Vector2 screenPoint, Camera eventCamera)
            {
                return eventCamera.ScreenToWorldPoint(screenPoint);
            }

            public static Vector2 GetWorldToScreenPosition(Vector3 worldPosition, Camera eventCamera)
            {
                return eventCamera.WorldToScreenPoint(worldPosition);
            }

            public static Sprite Texture2DToSprite(Texture2D texture)
            {
                Rect newRect = new Rect(Vector2.zero, new Vector2(texture.width, texture.height));
                return Sprite.Create(texture, newRect, Vector2.zero);
            }

            /// <summary>
            /// Returns A Position To Scroll To.
            /// </summary>
            /// <param name="value">Takes In The Scroller</param>
            /// <param name="childWidget">Takes In The Dynamic Content Container Child Widget.</param>
            /// <returns></returns>
            public static Vector2 GetScrollerSnapPosition(ScrollRect value, RectTransform childWidget)
            {
                Canvas.ForceUpdateCanvases();

                Vector2 viewPortPosition = value.viewport.localPosition;
                Vector2 childWidgetPosition = childWidget.localPosition;

                Vector2 position = new Vector2
                (
                    0 - (viewPortPosition.x + childWidgetPosition.x),
                    0 - (viewPortPosition.y + childWidgetPosition.y)
                );

                return position;
            }

            public static Vector2 GetScrollerSnapPosition(ScrollRect value, Vector2 snapPosition)
            {
                Canvas.ForceUpdateCanvases();

                Vector2 viewPortPosition = value.viewport.localPosition;

                Vector2 position = new Vector2
                (
                    0 - (viewPortPosition.x + snapPosition.x),
                    0 - (viewPortPosition.y + snapPosition.y)
                );

                return position;
            }

            public static void ConvertNameToColor(string colorName, Action<CallbackData<Color>> callback)
            {
                CallbackData<Color> callbackResults = new CallbackData<Color>();

                System.Drawing.ColorConverter converter = new System.Drawing.ColorConverter();

                if (converter.IsValid(colorName))
                {
                    var color = (System.Drawing.Color)converter.ConvertFromString(colorName);
                    Color newColor = new Color(color.R, color.G, color.B, color.A);

                    callbackResults.results = "Success : Color Found.";
                    callbackResults.data = newColor;
                    callbackResults.resultsCode = SuccessCode;
                }
                else
                {
                    callbackResults.results = "Failed : Color Not Found.";
                    callbackResults.data = Color.clear;
                    callbackResults.resultsCode = ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public static Color ConvertNameToColor(string colorName)
            {
                System.Drawing.ColorConverter converter = new System.Drawing.ColorConverter();

                if (converter.IsValid(colorName))
                {
                    var color = (System.Drawing.Color)converter.ConvertFromString(colorName);
                    return new Color(color.R, color.G, color.B, color.A);
                }
                else
                {
                    Debug.LogWarning($"--> RG_Unity - Failed To ConvertNameToColor - Color : {colorName} Is Invalid.");
                    return Color.clear;
                }
            }

            public static Color BlendColors(string colorA, string colorB, float blendAmount)
            {
                Color color_A = ConvertNameToColor(colorA);
                Color color_B = ConvertNameToColor(colorB);

                float r = (color_A.r * blendAmount + color_B.r * (1 - blendAmount));
                float g = (color_A.g * blendAmount + color_B.g * (1 - blendAmount));
                float b = (color_A.b * blendAmount + color_B.b * (1 - blendAmount));

                return new Color(r, g, b, 1.0f);
            }

            public static Color BlendColors(List<string> colors)
            {
                float r = 0.0f;
                float g = 0.0f;
                float b = 0.0f;

                foreach (var color in colors)
                {
                    Color tempColor = ConvertNameToColor(color);

                    r += tempColor.r;
                    g += tempColor.g;
                    b += tempColor.b;
                }

                return new Color(r / colors.Count, g / colors.Count, b / colors.Count, 1.0f);
            }

            public static MaterialProperties GetMaterialProperties(GameObject asset, SceneAsset assetData)
            {
                if (asset == null)
                {
                    Debug.LogWarning("--> Failed To Get Mesh Info Because Scene Asset Is Null.");
                    return null;
                }

                List<MeshRenderer> renderers = new List<MeshRenderer>();
                MeshRenderer meshRenderer = asset.GetComponent<MeshRenderer>();
                MaterialProperties materialProperties = new MaterialProperties();

                if (meshRenderer)
                    renderers.Add(meshRenderer);
                else
                {
                    renderers = asset.GetComponentsInChildren<MeshRenderer>().ToList();
                }

                if (renderers.Count > 0)
                {
                    foreach (var renderer in renderers)
                    {
                        if (renderer.sharedMaterials.Length > 0)
                        {
                            foreach (var material in renderer.sharedMaterials)
                            {
                                materialProperties.mainTexturePath = assetData.GetAssetField(AssetFieldType.MainTexture).path;
                                materialProperties.normalMapTexturePath = assetData.GetAssetField(AssetFieldType.NormalMap).path;
                                materialProperties.aoMapTexturePath = assetData.GetAssetField(AssetFieldType.AmbientOcclusionMap).path;

                                materialProperties.glossiness = material.GetFloat("_Glossiness");
                                materialProperties.bumpScale = material.GetFloat("_BumpScale");
                                materialProperties.aoStrength = material.GetFloat("_OcclusionStrength");
                            }
                        }
                    }
                }
                else
                    Debug.LogWarning($"--> Failed To Get Mesh Renderer(s) From Game Object : {asset.name}");

                return materialProperties;
            }

            public static MaterialProperties GetMaterialProperties(GameObject asset, int selectedMeshIndex)
            {
                List<MeshRenderer> renderers = new List<MeshRenderer>();
                MeshRenderer meshRenderer = asset.GetComponent<MeshRenderer>();
                MaterialProperties materialProperties = new MaterialProperties();

                if (meshRenderer)
                    renderers.Add(meshRenderer);
                else
                {
                    renderers = asset.GetComponentsInChildren<MeshRenderer>().ToList();
                }

                return materialProperties;
            }

            public static void ShowImage(SceneAsset asset, Image imageDisplayer)
            {
                if (imageDisplayer != null && asset.assetFields != null)
                {
                    if (FileIsValid(asset.GetAssetField(AssetFieldType.Thumbnail).path))
                    {
                        if (SceneAssetsManager.Instance != null)
                        {
                            if (SceneAssetsManager.Instance.GetAssetsLibrary().ImageAssetExists(asset.GetAssetField(AssetFieldType.Thumbnail).path))
                                imageDisplayer.sprite = SceneAssetsManager.Instance.GetAssetsLibrary().GetImageAsset(asset.GetAssetField(AssetFieldType.Thumbnail).path);
                            else
                            {
                                imageDisplayer.sprite = Texture2DToSprite(LoadTextureFile(asset.GetAssetField(AssetFieldType.Thumbnail).path));
                                SceneAssetsManager.Instance.GetAssetsLibrary().AddImageAsset(imageDisplayer.sprite, asset.GetAssetField(AssetFieldType.Thumbnail).path);
                            }
                        }
                        else
                            Debug.LogWarning("--> Assets Manager Not Yet Initialized.");
                    }
                    else
                    {
                        if (SceneAssetsManager.Instance != null)
                        {
                            if (imageDisplayer != null)
                                imageDisplayer.sprite = SceneAssetsManager.Instance.GetDefaultFallbackSceneAssetIcon();
                            else
                                Debug.LogWarning("--> Show Image Failed : Image Displayer Is Null.");

                        }
                        else
                            Debug.LogWarning("--> Assets Manager Not Yet Initialized.");
                    }
                }
                else
                {
                    if (SceneAssetsManager.Instance != null)
                    {
                        if (imageDisplayer != null)
                            imageDisplayer.sprite = SceneAssetsManager.Instance.GetDefaultFallbackSceneAssetIcon();
                        else
                            Debug.LogWarning("--> Show Image Failed : Image Displayer Is Null.");

                    }
                    else
                        Debug.LogWarning("--> Assets Manager Not Yet Initialized.");
                }
            }

            public static string GetColorGradientHexadecimal(Int32 numberOfSteps, Int32 step)
            {
                var r = 0.0;
                var g = 0.0;
                var b = 0.0;

                var h = (Double)step / numberOfSteps;
                var i = (Int32)(h * 6);
                var f = h * 6.0f - i;
                var q = 1 - f;

                switch (i % 6)
                {
                    case 0:

                        r = 1;
                        g = f;
                        b = 0;

                        break;

                    case 1:

                        r = q;
                        g = 1;
                        b = 0;

                        break;

                    case 2:

                        r = 0;
                        g = 1;
                        b = f;

                        break;

                    case 3:

                        r = 0;
                        g = q;
                        b = 1;

                        break;

                    case 4:

                        r = f;
                        g = 0;
                        b = 1;

                        break;

                    case 5:

                        r = 1;
                        g = 0;
                        b = q;

                        break;
                }

                return "#" + ((Int32)(r * 255)).ToString("X2") + ((Int32)(g * 255)).ToString("X2") + ((Int32)(b * 255)).ToString("X2");
            }

            public static GameObject LoadSceneAssetModelFile(string path)
            {
                return new OBJLoader().Load(path);
            }

            public static GameObject LoadSceneAssetModelFile(string path, string mtlPath)
            {
                if (!string.IsNullOrEmpty(mtlPath))
                    return new OBJLoader().Load(path, mtlPath);

                return new OBJLoader().Load(path);
            }

            #region Load Formatted Scene Asset Model 

            public static SceneObject LoadFormattedSceneAssetModel(string path, string mtlPath = null, bool addColliders = true)
            {
                Debug.Log("--> RG_Unity - Creating New Asset Model In Data");

                GameObject loadedAsset = null;

                SceneObject sceneObject = new SceneObject();

                if (!string.IsNullOrEmpty(mtlPath))
                    loadedAsset = new OBJLoader().Load(path, mtlPath);
                else
                    loadedAsset = new OBJLoader().Load(path);

                if (loadedAsset)
                {
                    if (addColliders)
                    {
                        List<MeshRenderer> assetObjectMeshRendererList = new List<MeshRenderer>();

                        if (loadedAsset.transform.childCount > 0)
                            assetObjectMeshRendererList = loadedAsset.GetComponentsInChildren<MeshRenderer>().ToList();

                        if (loadedAsset.GetComponent<MeshRenderer>())
                            assetObjectMeshRendererList.Add(loadedAsset.GetComponent<MeshRenderer>());

                        if (assetObjectMeshRendererList.Count > 0)
                        {
                            foreach (var renderer in assetObjectMeshRendererList)
                            {
                                renderer.gameObject.AddComponent<MeshCollider>();
                            }
                        }
                    }

                    sceneObject.value = loadedAsset;

                }
                else
                    Debug.LogWarning("--> Loaded Scene Asset Parent Not Assigned.");


                return sceneObject;
            }

            static Vector3 GetImportedAssetPosition(Vector3 pos, Bounds assetBounds)
            {
                pos -= (assetBounds.center * 2);

                return pos;
            }

            static void OnImportedAssetSetupScale(Renderer assetRenderer, Transform assetTransform, float shrinkVectorScale, float defaultAssetImportScale, SceneAssetScaleDirection scaleDirection, Action<bool> callBack)
            {
                Vector3 scaleVect = new Vector3(shrinkVectorScale, shrinkVectorScale, shrinkVectorScale);

                switch (scaleDirection)
                {
                    case SceneAssetScaleDirection.Up:

                        assetTransform.localScale = Vector3.zero;

                        while (GetImportedAssetMaxExtent(assetRenderer.bounds) < defaultAssetImportScale)
                        {
                            assetTransform.localScale += scaleVect;
                        }

                        if (GetImportedAssetMaxExtent(assetRenderer.bounds) >= defaultAssetImportScale)
                        {
                            callBack.Invoke(true);
                        }
                        else
                        {
                            callBack.Invoke(false);
                        }

                        break;

                    case SceneAssetScaleDirection.Down:

                        assetTransform.localScale = Vector3.one;

                        while (GetImportedAssetMaxExtent(assetRenderer.bounds) > defaultAssetImportScale)
                        {
                            assetTransform.localScale -= scaleVect;
                        }

                        if (GetImportedAssetMaxExtent(assetRenderer.bounds) <= defaultAssetImportScale)
                        {
                            callBack.Invoke(true);
                        }
                        else
                        {
                            callBack.Invoke(false);
                        }

                        break;
                }
            }

            static SceneAssetScaleDirection GetImportedAssetScaleDirection(float assetScaleDirection, float defaultAssetImportScale)
            {
                SceneAssetScaleDirection scaleDirection = SceneAssetScaleDirection.None;

                if (assetScaleDirection > defaultAssetImportScale)
                {
                    scaleDirection = SceneAssetScaleDirection.Down;
                }

                if (assetScaleDirection < defaultAssetImportScale)
                {
                    scaleDirection = SceneAssetScaleDirection.Up;
                }

                return scaleDirection;
            }

            static float GetImportedAssetMaxExtent(Bounds assetBounds)
            {
                float[] extentsList = new float[] { assetBounds.extents.x, assetBounds.extents.y, assetBounds.extents.z };
                float assetMaxExtent = extentsList.Max();

                return assetMaxExtent;
            }

            #endregion

            public static bool FileIsValid(string path)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    if (File.Exists(path))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }

            public static Texture2D LoadTextureFile(string path)
            {
                return ImageLoader.LoadTexture(path);
            }

            public static List<string> GetEnumToStringList<T>()
            {
                return Enum.GetNames(typeof(T)).ToList();
            }

            public static T GetStringToEnum<T>(string value) where T : struct
            {
                T results;

                Enum.TryParse(value, out results);

                return results;
            }

            public static string TrimStringValue(string value, int targetLength)
            {
                if (value.Length >= targetLength)
                {
                    char[] valueCharArray = new char[targetLength];

                    for (int i = 0; i < targetLength; i++)
                        valueCharArray[i] = value[i];

                    string newValue = new string(valueCharArray);

                    return newValue;
                }
                else
                    return value;
            }

            public static string GetFormattedDirectoryPath(string path)
            {
                return path.Replace("\\", "/");
            }

            #region Async Timers

            static int cachedApplicationFrameRates = 0;

            public static void CacheApplicationFrameRate(int fps) => cachedApplicationFrameRates = fps;

            public static int GetCachedApplicationFrameRate()
            {
                return cachedApplicationFrameRates;
            }

            public static async Task GetWaitForSecondAsync(int value = 100) => await Task.Delay(value);

            public static async Task GetWaitForSecondAsync(int value = 100, Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                await Task.Delay(value);

                callbackResults.resultsCode = SuccessCode;

                callback?.Invoke(callbackResults);
            }

            public static async Task GetWaitForEndOfFrameAsync() => await Task.Delay(GetCachedApplicationFrameRate());

            public static async Task GetWaitForEndOfFrameAsync(Action<Callback> callback = null)
            {
                Callback callbackResults = new Callback();

                await Task.Delay(GetCachedApplicationFrameRate());

                callbackResults.resultsCode = SuccessCode;
                callback?.Invoke(callbackResults);
            }

            public static async Task GetWaitUntilAsync(bool action)
            {
                while(!action)
                {
                    await Task.Delay(100);
                }
            }

            #endregion

            #region Data Comparison

            public static bool CompareEnumValue<T>(T value_A, T value_B) where T : Enum
            {
                return value_A.Equals(value_B);
            }


            public static void CompareEnumTypeValue<T>(T value_A, T value_B, Action<Callback> callback) where T : Enum
            {
                Callback callbackResults = new Callback();

                if(value_A.Equals(value_B))
                {
                    callbackResults.results = $"Value_A Of Type : {value_A.GetType()} Is Equal To Value_B Of Type : {value_B.GetType()}";
                    callbackResults.resultsCode = SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Value_A Of Type : {value_A.GetType()} Is Not Equal To Value_B Of Type : {value_B.GetType()}";
                    callbackResults.resultsCode = ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public static void ListComponentHasEqualDataSize<T, U>(List<T> valueA, List<U> valueB, Action<CallbackSizeDataTuple<T, U>> callback)
            {
                CallbackSizeDataTuple<T, U> callbackResults = new CallbackSizeDataTuple<T, U>();
;
                if (valueA != null && valueA.Count > 0)
                {
                    if (valueB != null && valueB.Count > 0)
                    {
                        string valueA_ID = valueA[0].GetType().ToString();
                        string valueB_ID = valueB[0].GetType().ToString();

                        if (valueA.Count.Equals(valueB.Count))
                        {
                            callbackResults.results = $"List Component Value_A Of ID Type : {valueA_ID} Is Equal To List Component Value_B Of ID Type : {valueB_ID} With : {valueA.Count} Item(s).";

                            callbackResults.tuple_A = valueA;
                            callbackResults.tuple_B = valueB;
                            callbackResults.size = valueA.Count;

                            callbackResults.resultsCode = SuccessCode;
                        }
                        else
                        {
                            string greaterValue = (valueA.Count > valueB.Count) ? $"Value_A : {valueA_ID} Is Greater Than Value_B" : $"Value_B : {valueB_ID} Is Greater Than Value_A";

                            callbackResults.results = $"List Components Don't Have Equal Values : {greaterValue}. - Value_A : {valueA.Count} - Value B : {valueB.Count}";

                            callbackResults.tuple_A = default;
                            callbackResults.tuple_B = default;
                            callbackResults.size = default;

                            callbackResults.resultsCode = ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = "List Component Value_B Is Null / Empty.";

                        callbackResults.tuple_A = default;
                        callbackResults.tuple_B = default;
                        callbackResults.size = default;

                        callbackResults.resultsCode = ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "List Component Value_A Is Null / Empty.";

                    callbackResults.tuple_A = default;
                    callbackResults.tuple_B = default;
                    callbackResults.size = default;

                    callbackResults.resultsCode = ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public static void ListComponentHasEqualDataSize<T, U>(List<T> valueA, List<U> valueB, Action<CallbackSizeDataTuple<T, U, int>> callback)
            {
                CallbackSizeDataTuple<T, U, int> callbackResults = new CallbackSizeDataTuple<T, U, int>();
 
                if (valueA != null && valueA.Count > 0)
                {
                    if (valueB != null && valueB.Count > 0)
                    {
                        string valueA_ID = valueA[0].GetType().ToString();
                        string valueB_ID = valueB[0].GetType().ToString();

                        if (valueA.Count.Equals(valueB.Count))
                        {
                            callbackResults.results = $"List Component Value_A Of ID Type : {valueA_ID} Is Equal To List Component Value_B Of ID Type : {valueB_ID} With : {valueA.Count} Item(s).";

                            callbackResults.tuple_A = valueA;
                            callbackResults.tuple_B = valueB;
                            callbackResults.tuple_C = valueA.Count;

                            callbackResults.resultsCode = SuccessCode;
                        }
                        else
                        {
                            string greaterValue = (valueA.Count > valueB.Count) ? $"Value_A : {valueA_ID} Is Greater Than Value_B" : $"Value_B : {valueB_ID} Is Greater Than Value_A";

                            callbackResults.results = $"List Components Don't Have Equal Values : {greaterValue}. - Value_A : {valueA.Count} - Value B : {valueB.Count}";

                            callbackResults.tuple_A = default;
                            callbackResults.tuple_B = default;
                            callbackResults.tuple_C = default;

                            callbackResults.resultsCode = ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = "List Component Value_B Is Null / Empty.";

                        callbackResults.tuple_A = default;
                        callbackResults.tuple_B = default;
                        callbackResults.tuple_C = default;

                        callbackResults.resultsCode = ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "List Component Value_A Is Null / Empty.";


                    callbackResults.tuple_A = default;
                    callbackResults.tuple_B = default;
                    callbackResults.tuple_C = default;

                    callbackResults.resultsCode = ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public static void ListComponentHasEqualDataSize<T>(List<T> valueA, List<T> valueB, Action<CallbackSizeDataTuple<T>> callback)
            {
                CallbackSizeDataTuple<T> callbackResults = new CallbackSizeDataTuple<T>();
;
                if (valueA != null && valueA.Count > 0)
                {
                    if (valueB != null && valueB.Count > 0)
                    {
                        string values_ID = valueA[0].GetType().ToString();

                        if (valueA.Count.Equals(valueB.Count))
                        {
                            callbackResults.results = $"List Component Value_A Of ID Type : {values_ID} Is Equal To List Component Value_B Of ID Type : {values_ID} With : {valueA.Count} Item(s).";

                            callbackResults.tuple_A = valueA;
                            callbackResults.tuple_B = valueB;
                            callbackResults.size = valueA.Count;

                            callbackResults.resultsCode = SuccessCode;
                        }
                        else
                        {
                            string greaterValue = (valueA.Count > valueB.Count) ? $"Value_A : {values_ID} Is Greater Than Value_B" : $"Value_B : {values_ID} Is Greater Than Value_A";

                            callbackResults.results = $"List Components Don't Have Equal Values : {greaterValue}. - Value_A : {valueA.Count} - Value B : {valueB.Count}";

                            callbackResults.tuple_A = default;
                            callbackResults.tuple_B = default;
                            callbackResults.size = default;

                            callbackResults.resultsCode = ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = "List Component Value_B Is Null / Empty.";

                        callbackResults.tuple_A = default;
                        callbackResults.tuple_B = default;
                        callbackResults.size = default;

                        callbackResults.resultsCode = ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "List Component Value_A Is Null / Empty.";

                    callbackResults.tuple_A = default;
                    callbackResults.tuple_B = default;
                    callbackResults.size = default;

                    callbackResults.resultsCode = ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public static void ListComponentHasData<T>(List<T> source, Action<CallbackDatas<T>> callback)
            {
                CallbackDatas<T> callbackResults = new CallbackDatas<T>();

                if (source != null && source.Count > 0)
                {
                    callbackResults.results = $"List Component Has {source.Count} Items.";
                    callbackResults.data = source;
                    callbackResults.resultsCode = SuccessCode;
                }
                else
                {
                    callbackResults.results = "List Component Has No Data.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public static bool ComponentIsNotNullOrEmpty<T>(T component)
            {
                return component != null;
            }

            public static void GetComponentsNotNullOrEmpty<T>(List<T> component, Action<CallbackDatas<T>> callback = null)
            {
                CallbackDatas<T> callbackResults = new CallbackDatas<T>();

                if (component != null && component.Count > 0)
                {
                    callbackResults.results = "Component Value Is Not Null Or Empty.";
                    callbackResults.data = component;
                    callbackResults.resultsCode = SuccessCode;
                }
                else
                {
                    callbackResults.results = "Component Value Is Null Or Empty.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public static void GetComponentIsNotNullOrEmpty<T>(T component, Action<CallbackData<T>> callback)
            {
                CallbackData<T> callbackResults = new CallbackData<T>();

                if(component != null)
                {
                    callbackResults.results = $"Component : {component.GetType().Name} Is Valid.";
                    callbackResults.data = component;
                    callbackResults.resultsCode = SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Component : {component.GetType().Name} Is Null / Empty.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public static bool IsNotNullOrEmpty<T>(T component) where T : AppMonoBaseClass
            {
                return component != null;
            }

            public static void GetIsNotNullOrEmpty<T>(T component, Action<CallbackData<T>> callback) where T : AppMonoBaseClass
            {
                CallbackData<T> callbackResults = new CallbackData<T>();

                if (component != null)
                {
                    callbackResults.results = $"Component : {component.GetType().Name} Is Valid.";
                    callbackResults.data = component;
                    callbackResults.resultsCode = SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Component : {component.GetType().Name} Is Null / Empty.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            public static bool ObjectIsNotNullOrEmpty<T>(T component) where T : UnityEngine.Object
            {
                return component != null;
            }

            public static void GetObjectIsNotNullOrEmpty<T>(T component, Action<CallbackData<T>> callback) where T : UnityEngine.Object
            {
                CallbackData<T> callbackResults = new CallbackData<T>();

                if (component != null)
                {
                    callbackResults.results = $"Component : {component.GetType().Name} Is Valid.";
                    callbackResults.data = component;
                    callbackResults.resultsCode = SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Component : {component.GetType().Name} Is Null / Empty.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }

            #endregion

            #region Log Information 

            public static LogInfoType InfoCode { get { return LogInfoType.Info; } }

            public static LogInfoType SuccessCode { get { return LogInfoType.Success; } }

            public static LogInfoType WarningCode { get { return LogInfoType.Warning; } }

            public static LogInfoType ErrorCode { get { return LogInfoType.Error; } }


            public static bool IsInfoCode(LogInfoType resultCode)
            {
                return resultCode == InfoCode;
            }

            public static bool IsSuccessCode(LogInfoType resultCode)
            {
                return resultCode == SuccessCode;
            }

            public static bool IsWarningCode(LogInfoType resultCode)
            {
                return resultCode == WarningCode;
            }


            public static bool IsErrorCode(LogInfoType resultCode)
            {
                return resultCode == ErrorCode;
            }

            #endregion
        }

        #endregion

        #region Interfaces

        public interface IUIScreenData
        {
            void Init(Action<CallbackData<ScreenUIData>> callBack = null);

            void Show(Action<Callback> callback = null);

            void Hide();

            void Focus();

            void Blur(SceneDataPackets dataPackets);

            string GetScreenTitle();
            GameObject GetScreenObject();
            UIScreenType GetUIScreenType();
        }

        public interface IUIInputComponent<V>
        {
            void SetTitle(string title);

            void SetFieldColor(Color color);

            void SetInteractableState(bool interactable);

            bool GetInteractableState();

            void SetUIInputVisibilityState(bool visible);

            bool GetUIInputVisibilityState();

            void SetUIInputState(InputUIState state);

            void SetUIInputState(V input, InputUIState state);

            InputUIState GetUIInputState();

            void SetChildWidgetsState(bool interactable, bool isSelected);

            void OnInputSelected();

            void OnInputDeselected();

            void OnInputPointerDownEvent();
        }

        public interface ISettingsWidget
        {
            void ShowWidget();
            void HideWidget();
        }

        #region Commands

        public interface ICommand
        {
            void Execute();

            void Undo();
        }

        #endregion

        #endregion

        #region Events

        public static class ActionEvents
        {
            #region Delegates

            // Void Delegates
            public delegate void Void();
            public delegate void ParamVoid<T>(T tValue);
            public delegate void ParamVoid<T, U>(T tValue, U uValue);
            public delegate void ParamVoid<T, U, V>(T tValue, U uValue, V vValue);
            public delegate void ParamVoid<T, U, V, W>(T tValue, U uValue, V vValue, W wValue);
            public delegate void ButtonAction<T>(UIButton<T> value);

            // Value Delegates
            public delegate Transform TransformNoParam();
            public delegate Transform TransformParam<T>(T tValue);

            #endregion

            #region Events

            public static event Void _OnClearPreviewedSceneAssetObjectEvent;
            public static event Void _OnSceneAssetsRemoved;
            public static event Void _OnAssetDeleteRefresh;
            public static event Void _OnScreenUIRefreshed;
            public static event Void _OnScreenExitEvent;
            public static event Void _OnScreenPressAndHoldInput;
            public static event Void _OnScreenDoubleTapInput;
            public static event Void _OnClearAllAssetSelectionEvent;
            public static event Void _OnSceneModelPoseResetEvent;
            public static event Void _OnResetContaineToDefaultrPoseEvent;
            public static event Void _OnResetCameraToDefaultrPoseEvent;
            public static event Void _OnAppScreensInitializedEvent;
            public static event Void _OnDropDownContentDataInitializedEvent;
            public static event Void _OnscrollToTopEvent;
            public static event Void _OnWidgetSelectionEvent;
            public static event Void _OnWidgetSelectionAdded;
            public static event Void _OnWidgetDeselectionEvent;
            public static event Void _OnWidgetSelectionRemoved;

            public static event ParamVoid<ButtonDataPackets> _OnNavigationTabWidgetEvent;
            public static event ParamVoid<ScreenViewState> _OnScreenViewStateChangedEvent;
            public static event ParamVoid<NavigationTabID, NavigationRenderSettingsProfileID> _OnNavigationSubTabChangedEvent;
            public static event ParamVoid<SceneAsset> _OnCreatedAssetDataEditEvent;
            public static event ParamVoid<SceneDataPackets> _OnScreenChangeEvent;
            public static event ParamVoid<UIScreenType> _OnScreenChangedEvent;
            public static event ParamVoid<UIScreenViewComponent> _OnScreenRefreshed;
            public static event ParamVoid<WidgetType, InputActionButtonType, SceneDataPackets> _OnPopUpActionEvent;
            public static event ParamVoid<SceneAssetModeType> _OnResetSceneAssetPreviewPoseEvent;
            public static event ParamVoid<ARSceneContentState> _OnARSceneAssetStateEvent;
            public static event ParamVoid<SceneEventCameraType> _OnSetCurrentActiveSceneCameraEvent;
            public static event ParamVoid<InputActionButtonType, bool, bool> _OnActionButtonFieldUploadedEvent;
            public static event ParamVoid<TogglableWidgetType, bool, bool> _OnScreenTogglableStateEvent;
            public static event ParamVoid<Quaternion> _OnUpdateSceneAssetDefaultRotation;
            public static event ParamVoid<SceneDataPackets> _OnTransitionSceneEventCamera;
            public static event ParamVoid<InputActionButtonType> _OnActionButtonClickedEvent;
            public static event ParamVoid<CheckboxInputActionType, bool> _OnActionCheckboxToggledEvent;
            public static event ParamVoid<bool, bool> _OnActionCheckboxStateEvent;
            public static event ParamVoid<bool> _OnPermissionGrantedResults;
            public static event ParamVoid<ColorInfo, bool, bool> _OnSwatchColorPickedEvent;
            public static event ParamVoid<bool> _OnToggleColorDropPickerEvent;
            public static event ParamVoid<AssetFieldType, StorageDirectoryData> _OnFilePickerDirectoryFieldSelectedEvent;
            public static event ParamVoid<string[]> _OnVoiceCommandResultsEvent;
            public static event ParamVoid<AudioType> _OnPlayAudioEvent;
            public static event ParamVoid<Vector2, bool> _OnScrollAndFocusToSelectionEvent;
            public static event ParamVoid<string> _OnNavigateAndFocusToSelectionEvent;
            public static event ParamVoid<bool> _OnAllWidgetsSelectionEvent;
            public static event ParamVoid<FocusedSelectionData> _OnWidgetsSelectionDataEvent;
            public static event ParamVoid<FocusedSelectionInfo<SceneDataPackets>> _OnWidgetSelectionDataEvent;

            public static event TransformNoParam _OnGetContentPreviewContainer;

            #endregion

            #region Callbacks

            public static void OnScreenDoubleTapInput() => _OnScreenDoubleTapInput?.Invoke();
            public static void OnAppScreensInitializedEvent() => _OnAppScreensInitializedEvent?.Invoke();
            public static void OnSceneModelPoseResetEvent() => _OnSceneModelPoseResetEvent?.Invoke();
            public static void OnResetContaineToDefaultPoseEvent() => _OnResetContaineToDefaultrPoseEvent?.Invoke();
            public static void OnResetCameraToDefaultPoseEvent() => _OnResetCameraToDefaultrPoseEvent?.Invoke();
            public static void OnDropDownContentDataInitializedEvent() => _OnDropDownContentDataInitializedEvent?.Invoke();
            public static void OnScreenPressAndHoldInput() => _OnScreenPressAndHoldInput?.Invoke();
            public static void OnClearAllAssetSelectionEvent() => _OnClearAllAssetSelectionEvent?.Invoke();
            public static void OnSceneAssetsRemoved() => _OnSceneAssetsRemoved?.Invoke();
            public static void OnAssetDeleteRefresh() => _OnAssetDeleteRefresh?.Invoke();
            public static void OnScreenUIRefreshed() => _OnScreenUIRefreshed?.Invoke();
            public static void OnScreenExitEvent() => _OnScreenExitEvent?.Invoke();
            public static void OnscrollToTopEvent() => _OnscrollToTopEvent?.Invoke();
            public static void OnWidgetSelectionEvent() => _OnWidgetSelectionEvent?.Invoke();
            public static void OnWidgetSelectionAdded() => _OnWidgetSelectionAdded?.Invoke();
            public static void OnWidgetDeselectionEvent() => _OnWidgetDeselectionEvent?.Invoke();
            public static void OnWidgetSelectionRemoved() => _OnWidgetSelectionRemoved?.Invoke();

            public static void OnNavigationTabWidgetEvent(ButtonDataPackets dataPackets) => _OnNavigationTabWidgetEvent?.Invoke(dataPackets);
            public static void OnNavigationSubTabChangedEvent(NavigationTabID navigationTab, NavigationRenderSettingsProfileID selectionTypedID) => _OnNavigationSubTabChangedEvent?.Invoke(navigationTab, selectionTypedID);
            public static void OnScreenViewStateChangedEvent(ScreenViewState state) => _OnScreenViewStateChangedEvent?.Invoke(state);
            public static void OnScreenTogglableStateEvent(AppData.TogglableWidgetType widgetType, bool state = false, bool useInteractability = false) => _OnScreenTogglableStateEvent?.Invoke(widgetType, state, useInteractability);
            public static void OnARSceneAssetStateEvent(ARSceneContentState contentState) => _OnARSceneAssetStateEvent?.Invoke(contentState);
            public static void OnPermissionGrantedResults(bool isGranted) => _OnPermissionGrantedResults?.Invoke(isGranted);
            public static void OnSetCurrentActiveSceneCameraEvent(SceneEventCameraType eventCameraType) => _OnSetCurrentActiveSceneCameraEvent?.Invoke(eventCameraType);
            public static void OnResetSceneAssetPreviewPoseEvent(SceneAssetModeType assetModeType) => _OnResetSceneAssetPreviewPoseEvent?.Invoke(assetModeType);
            public static void OnUpdateSceneAssetDefaultRotation(Quaternion rotation) => _OnUpdateSceneAssetDefaultRotation?.Invoke(rotation);
            public static void OnActionButtonClicked(InputActionButtonType actionType) => _OnActionButtonClickedEvent?.Invoke(actionType);
            public static void OnActionCheckboxToggledEvent(CheckboxInputActionType actionType, bool value) => _OnActionCheckboxToggledEvent?.Invoke(actionType, value);
            public static void OnCreatedAssetDataEditEvent(SceneAsset asset) => _OnCreatedAssetDataEditEvent?.Invoke(asset);
            public static void OnScreenChangeEvent(SceneDataPackets dataPackets) => _OnScreenChangeEvent?.Invoke(dataPackets);
            public static void OnScreenChangedEvent(UIScreenType screenType) => _OnScreenChangedEvent?.Invoke(screenType);
            public static void OnScreenRefreshed(UIScreenViewComponent screenData) => _OnScreenRefreshed?.Invoke(screenData);
            public static void OnActionButtonFieldUploadedEvent(InputActionButtonType actionType = InputActionButtonType.None, bool interactable = false, bool isSelected = false) => _OnActionButtonFieldUploadedEvent?.Invoke(actionType, interactable, isSelected);
            public static void OnClearPreviewedSceneAssetObjectEvent() => _OnClearPreviewedSceneAssetObjectEvent?.Invoke();
            public static void OnPopUpActionEvent(WidgetType popUpType, InputActionButtonType actionType, SceneDataPackets dataPackets) => _OnPopUpActionEvent?.Invoke(popUpType, actionType, dataPackets);
            public static void OnTransitionSceneEventCamera(SceneDataPackets dataPackets) => _OnTransitionSceneEventCamera?.Invoke(dataPackets);
            public static void OnActionCheckboxStateEvent(bool interactable, bool visible) => _OnActionCheckboxStateEvent?.Invoke(interactable, visible);
            public static void OnSwatchColorPickedEvent(ColorInfo colorID, bool fromButtonPress, bool onOpenColorSettings) => _OnSwatchColorPickedEvent?.Invoke(colorID, fromButtonPress, onOpenColorSettings);
            public static void OnToggleColorDropPickerEvent(bool enabled) => _OnToggleColorDropPickerEvent?.Invoke(enabled);
            public static void OnFilePickerDirectoryFieldSelectedEvent(AssetFieldType fieldType, StorageDirectoryData directoryData) => _OnFilePickerDirectoryFieldSelectedEvent?.Invoke(fieldType, directoryData);
            public static void OnVoiceCommandResultsEvent(string[] commands) => _OnVoiceCommandResultsEvent?.Invoke(commands);
            public static void OnPlayAudioEvent(AudioType audioType) => _OnPlayAudioEvent?.Invoke(audioType);
            public static void ScrollAndFocusToSelectionEvent(Vector2 position, bool transition = false) => _OnScrollAndFocusToSelectionEvent?.Invoke(position, transition);
            public static void OnNavigateAndFocusToSelectionEvent(string widgetName) => _OnNavigateAndFocusToSelectionEvent?.Invoke(widgetName);
            public static void OnAllWidgetsSelectionEvent(bool currentPage = false) => _OnAllWidgetsSelectionEvent?.Invoke(currentPage);
            public static void OnWidgetSelectionEvent(FocusedSelectionInfo<SceneDataPackets> selectionInfo) => _OnWidgetSelectionDataEvent?.Invoke(selectionInfo);
            public static void OnWidgetsSelectionEvent(FocusedSelectionData selectionData) => _OnWidgetsSelectionDataEvent?.Invoke(selectionData);

            public static Transform OnGetContentPreviewContainer()
            {
                return _OnGetContentPreviewContainer?.Invoke();
            }

            #endregion
        }

        #endregion
    }


    public static class AppDataExtensions
    {
        public static List<List<T>> GetSubList<T>(this List<T> source, int subListSize)
        {
            return source.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / subListSize).Select(x => x.Select(v => v.Value).ToList()).ToList();
        }
    }
}