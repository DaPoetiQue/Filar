using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using static TMPro.TMP_Dropdown;

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

        [Space(5)]
        [SerializeField]
        List<AppData.ProjectCategoryInfo> projectCategoryInfoList = new List<AppData.ProjectCategoryInfo>();

        #region Remove

        List<AppData.ColorSwatch> colorSwatchLibrary = new List<AppData.ColorSwatch>();

        [Space(5)]
        [SerializeField]
        AppData.UIScreenWidgetsPrefabDataLibrary screenWidgetPrefabLibrary = new AppData.UIScreenWidgetsPrefabDataLibrary();

        [Space(5)]
        [SerializeField]
        List<AppData.UIImageData> imageDataLibrary = new List<AppData.UIImageData>();

        #endregion

        List<AppData.UIScreenWidget> loadedWidgets = new List<AppData.UIScreenWidget>();

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
        AppData.StorageDirectoryData rootStructureStorageData = new AppData.StorageDirectoryData();

        [Space(5)]
        [SerializeField]
        AppData.ProjectRootStructureData rootProjectStructureData = new AppData.ProjectRootStructureData();

        [Space(5)]
        [SerializeField]
        List<AppData.FileData> fileDatas = new List<AppData.FileData>();

        AppData.SceneAsset currentSceneAsset;

        AppData.SceneAssetLibrary sceneAssetLibrary = new AppData.SceneAssetLibrary();

        [Space(5)]
        [SerializeField]
        List<AppData.SceneAsset> sceneAssetList = new List<AppData.SceneAsset>();

        List<AppData.SceneAssetWidget> screenWidgetList = new List<AppData.SceneAssetWidget>();
        AppData.AssetExportData currentAssetExportData = new AppData.AssetExportData();

        List<AppData.Project> loadedProjectData = new List<AppData.Project>();

        [SerializeField]
        List<AppData.DropDownContentData> dropDownContentDataList = new List<AppData.DropDownContentData>();

        RenderProfileUIHandler renderProfileUIHandlerPrefab = null;
        ColorSwatchButtonHandler colorSwatchButtonHandlerPrefab = null;

        AppData.Folder<UIScreenHandler> folderList = new AppData.Folder<UIScreenHandler>();

        [SerializeField] // Hide
        AppData.ProjectStructureData currentProjectStructureData;

        AppData.FolderStructureType currentViewedFolderStructure;

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

        public bool IsInitialized { get; private set; }

        #endregion

        #endregion

        #region Unity Callbacks

        void Awake() => SetupInstance();

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

        public void InitializeStorage(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            #region Directories

            if (defaultDirectories == null)
            {
                callbackResults.results = "App Default Directories Missing.";
                callbackResults.resultsCode = AppData.Helpers.WarningCode;

                callback?.Invoke(callbackResults);
            }

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
                        projectDirectory = formattedDirectory,
                        type = directory.type
                    };

                    if (!Directory.Exists(formattedDirectory))
                    {
                        CreateDirectory(appDirectory, (directoryCreatedCallbackResults) =>
                        {
                            callbackResults.results = directoryCreatedCallbackResults.results;
                            callbackResults.resultsCode = directoryCreatedCallbackResults.resultsCode;

                            if (callbackResults.Success())
                            {
                                if (!appDirectories.Contains(appDirectory))
                                {
                                    appDirectories.Add(appDirectory);
                                    callbackResults.results = $"Created And Added Directory : {directory} To App Directories.";
                                }
                                else
                                    callbackResults.results = $"Directory : {directory} Already Exist In App Directories.";
                            }
                            else
                            {
                                callbackResults.results = $"Failed To Create Directory : {directory}";
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        });
                    }
                    else
                    {
                        if (!appDirectories.Contains(appDirectory))
                        {
                            appDirectories.Add(appDirectory);
                            callbackResults.results = $"Added Existing Directory : {appDirectory} To App Directories.";
                        }
                    }
                }
                else
                {
                    callbackResults.results = "App Data Path Set To None.";
                    callbackResults.resultsCode = AppData.Helpers.WarningCode;
                }
            }

            #endregion

            callback?.Invoke(callbackResults);
        }

        public void Init(AppData.Folder rootFolder, DynamicWidgetsContainer container, Action<AppData.Callback> callback = null)
        {
            try
            {
                AppData.Callback callbackResults = new AppData.Callback();

                if (!IsInitialized)
                {
                    #region Initialization

                    LogInfo("Initializing Assets Manager.", this);

                    #region Asset Container

                    if (assetContainerList.Count > 0)
                    {
                        foreach (var assetContainer in assetContainerList)
                        {
                            AppData.Helpers.UnityComponentValid(assetContainer.value, "Asset Container Value", objectValidCallbackResults =>
                            {
                                callbackResults.results = objectValidCallbackResults.results;
                                callbackResults.resultsCode = objectValidCallbackResults.resultsCode;

                                if (!callbackResults.Success())
                                {
                                    callbackResults.results = $"Value For Asset Container : {assetContainer.name} Is Null / Missing / Not Assigned In The Editor Inspector.";
                                    callbackResults.resultsCode = AppData.Helpers.WarningCode;
                                }
                            });

                            if (!callbackResults.Success())
                            {
                                callback?.Invoke(callbackResults);
                                break;
                            }
                        }
                    }

                    #endregion

                    #region Content Container

                    if (dynamicWidgetsContainersList.Count > 0)
                    {
                        foreach (var assetContainer in dynamicWidgetsContainersList)
                        {
                            AppData.Helpers.ComponentValid(assetContainer, objectValidCallbackResults =>
                            {
                                callbackResults.results = objectValidCallbackResults.results;
                                callbackResults.resultsCode = objectValidCallbackResults.resultsCode;

                                if (!callbackResults.Success())
                                {
                                    callbackResults.results = $"Container For Screen : {assetContainer.name} Missing / Null / Not Assigned In The Inspector Panel.";
                                    callbackResults.resultsCode = AppData.Helpers.WarningCode;
                                }
                            });

                            if (!callbackResults.Success())
                            {
                                callback?.Invoke(callbackResults);
                                break;
                            }
                        }
                    }

                    #endregion

                    #region Assets Library Initialization

                    sceneAssetLibrary.InitializeLibrary(libraryInitializationCallbackResults =>
                    {
                        callbackResults = libraryInitializationCallbackResults;

                        if (callbackResults.Success())
                        {
                            #region Layout Data

                            GetLayoutViewType(layoutViewCallbackResults =>
                                {
                                    callbackResults.results = layoutViewCallbackResults.results;
                                    callbackResults.resultsCode = layoutViewCallbackResults.resultsCode;

                                    if (callbackResults.Success())
                                        InitializeFolderLayoutView(layoutViewCallbackResults.data);
                                    else
                                    {
                                        Log(callbackResults.resultsCode, callbackResults.results, this);
                                        callback?.Invoke(callbackResults);
                                    }
                                });

                            #endregion

                            UnloadUnusedAssets();

                            IsInitialized = callbackResults.Success();
                        }
                        else
                            callback?.Invoke(callbackResults);
                    });

                    #endregion

                    #region Rmove This

                    // Move This To Streaming Assets Or Addressables (Prefered).

                    //if (renderProfileUIHandlerPrefab == null)
                    //    if (!string.IsNullOrEmpty(profileWidgetPrefabDirectory))
                    //        renderProfileUIHandlerPrefab = Resources.Load<RenderProfileUIHandler>(profileWidgetPrefabDirectory);
                    //    else
                    //        LogWarning("Couldn't Load Asset From Resources - Directory Missing.", this);

                    //if (colorSwatchButtonHandlerPrefab == null)
                    //    if (!string.IsNullOrEmpty(profileWidgetPrefabDirectory))
                    //        colorSwatchButtonHandlerPrefab = Resources.Load<ColorSwatchButtonHandler>(colorSwatchButtonHandlerPrefabDirectory);
                    //    else
                    //        LogWarning("Couldn't Load Asset From Resources - Directory Missing.", this);

                    #endregion

                    #endregion
                }
                else
                {
                    callbackResults.results = "Scene Assets Manager Has Already Been Initialized.";
                    callbackResults.resultsCode = AppData.Helpers.WarningCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void UnloadUnusedAssets() => Resources.UnloadUnusedAssets();

        #region Drop Down Content

        public AppData.DropDownContentData GetDropdownContent<T>() where T : Enum
        {
            return new AppData.DropDownContentData
            {
                data = AppData.Helpers.GetEnumToStringList<T>()
            };
        }

        public AppData.DropDownContentData GetDropdownContent<T>(params string[] args) where T : Enum
        {
            var datas = AppData.Helpers.GetEnumToStringList<T>();

            List<string> contentDataList = new List<string>();
            List<string> validContentDataList = new List<string>();

            if (args.Length > 0 && datas.Count > 0)
            {
                foreach (var item in args)
                {
                    foreach (var data in datas)
                    {
                        if (data.Contains(item) && data != item)
                        {
                            string content = data.Replace(item, "");

                            if(!contentDataList.Contains(content))
                                contentDataList.Add(content);
                        }
                        
                        if(data.Contains(item) && data == item)
                        {
                            if (contentDataList.Contains(data))
                                contentDataList.Remove(data);
                        }
                    }
                }

                if(contentDataList.Count > 0)
                {
                    foreach (var item in args)
                    {
                        for (int i = 0; i < datas.Count; i++)
                        {
                            if (!validContentDataList.Contains(contentDataList[i]) && !contentDataList[i].Contains(item) && contentDataList[i] != item)
                                validContentDataList.Add(contentDataList[i]);
                        }

                        if (validContentDataList.Contains(item))
                            validContentDataList.Remove(item);
                    }
                }
                else
                    LogError("Failed To Get Dropdown Content", this);
            }
            else
                LogError("Failed There Are No Args Or Data Content Is Null.", this);

            return new AppData.DropDownContentData
            {
                data = validContentDataList
            };
        }

        public AppData.DropDownContentData GetFormattedDropdownContent(List<string> datas, params string[] args)
        {
            List<string> contentDataList = new List<string>();
            List<string> validContentDataList = new List<string>();

            if (args.Length > 0 && datas.Count > 0)
            {
                foreach (var item in args)
                {
                    foreach (var data in datas)
                    {
                        if (data.Contains(item) && data != item)
                        {
                            string content = data.Replace(item, "");

                            if (!contentDataList.Contains(content))
                                contentDataList.Add(content);
                        }

                        if (data.Contains(item) && data == item)
                        {
                            if (contentDataList.Contains(data))
                                contentDataList.Remove(data);
                        }
                    }
                }

                if (contentDataList.Count > 0)
                {
                    foreach (var item in args)
                    {
                        for (int i = 0; i < datas.Count; i++)
                        {
                            if (!validContentDataList.Contains(contentDataList[i]) && !contentDataList[i].Contains(item) && contentDataList[i] != item)
                                validContentDataList.Add(contentDataList[i]);
                        }

                        if (validContentDataList.Contains(item))
                            validContentDataList.Remove(item);
                    }
                }
                else
                    LogError("Failed To Get Dropdown Content", this);
            }
            else
                LogError("Failed There Are No Args Or Data Content Is Null.", this);

            return new AppData.DropDownContentData
            {
                data = validContentDataList
            };
        }

        public int GetDropdownContentCount<T>() where T : Enum
        {
            var datas = AppData.Helpers.GetEnumToStringList<T>();

            return datas.Count;
        }

        public int GetDropdownContentCount<T>(params string[] args) where T : Enum
        {
            var datas = AppData.Helpers.GetEnumToStringList<T>();

            List<string> contentDataList = new List<string>();
            List<string> validContentDataList = new List<string>();

            if (args.Length > 0 && datas.Count > 0)
            {
                foreach (var item in args)
                {
                    foreach (var data in datas)
                    {
                        if (data.Contains(item) && data != item)
                        {
                            string content = data.Replace(item, "");

                            if (!contentDataList.Contains(content))
                                contentDataList.Add(content);
                        }

                        if (data.Contains(item) && data == item)
                        {
                            if (contentDataList.Contains(data))
                                contentDataList.Remove(data);
                        }
                    }
                }

                if (contentDataList.Count > 0)
                {
                    foreach (var item in args)
                    {
                        for (int i = 0; i < datas.Count; i++)
                        {
                            if (!validContentDataList.Contains(contentDataList[i]) && !contentDataList[i].Contains(item) && contentDataList[i] != item)
                                validContentDataList.Add(contentDataList[i]);
                        }

                        if (validContentDataList.Contains(item))
                            validContentDataList.Remove(item);
                    }
                }
                else
                    LogError("Failed To Get Dropdown Content", this);
            }
            else
                LogError("Failed There Are No Args Or Data Content Is Null.", this);

            return validContentDataList.Count;
        }

        public int GetDropdownContentIndex(int contentA, int ContentB)
        {
            return contentA - ContentB;
        }

        public void GetDropdownContentIndex<T>(string content, Action<AppData.CallbackData<int>> callback) where T : Enum
        {
            AppData.CallbackData<int> callbackResults = new AppData.CallbackData<int>();

            var datas = AppData.Helpers.GetEnumToStringList<T>();

            AppData.Helpers.StringListValueValid(datas, hasComponentsCallbackResults => 
            {
                callbackResults.resultsCode = hasComponentsCallbackResults.resultsCode;
            
                if(callbackResults.Success())
                {
                    var contentType = datas.Find(x => x.Contains(content));

                    AppData.Helpers.StringValueValid(contentType, valueValidCallbackResults => 
                    {
                        callbackResults.resultsCode = valueValidCallbackResults.resultsCode;

                        if(callbackResults.Success())
                        {
                            int index = datas.IndexOf(contentType);

                            callbackResults.results = $"Content : {content} Is Found At Index : {index}.";
                            callbackResults.data = index;
                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;

                            LogSuccess($"========================>> Results: {callbackResults.results}", this);
                        }
                        else
                        {
                            callbackResults.results = $"Couldn't Find Type That Matches / Contains Content {content}.";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    });
                }
                else
                {
                    callbackResults.results = $"Failed To Get Data For Enum : Couldn't Get Content : {content}'s Index.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            });

            callback.Invoke(callbackResults);
        }

        public int GetDropdownContentTypeIndex<T>(T type) where T : Enum
        {
            var data = AppData.Helpers.GetEnumToStringList<T>();

            return data.IndexOf(type.ToString());
        }

        public void GetDropdownContentTypeFromIndex<T>(int index, Action<AppData.CallbackData<Enum>> callback) where T : struct
        {
            AppData.CallbackData<Enum> callbackResults = new AppData.CallbackData<Enum>();

            var data = AppData.Helpers.GetEnumToStringList<T>();

            var type = data[index];

            AppData.Helpers.GetStringToEnumData<T>(type, convertedEnumDataCallbackResults => 
            {
                callbackResults.results = convertedEnumDataCallbackResults.results;
                callbackResults.resultsCode = convertedEnumDataCallbackResults.resultsCode;

                if (convertedEnumDataCallbackResults.Success())
                {
                    callbackResults.data = convertedEnumDataCallbackResults.data;
                }
                else
                    Log(convertedEnumDataCallbackResults.resultsCode, convertedEnumDataCallbackResults.results, this);
            });

            callback.Invoke(callbackResults);
        }

        public void GetSortedDropdownContent<T>(List<string> contents, Action<AppData.CallbackDataList<string>> callback) where T : Enum
        {
            AppData.CallbackDataList<string> callbackResults = new AppData.CallbackDataList<string>();

            AppData.Helpers.StringListValueValid(contents, hasValueCallbackResults => 
            {
                callbackResults.resultsCode = hasValueCallbackResults.resultsCode;

                if (callbackResults.Success())
                {
                    var datas = AppData.Helpers.GetEnumToStringList<T>();

                    AppData.Helpers.StringListValueValid(contents, hasValueCallbackResults => 
                    {
                        callbackResults.resultsCode = hasValueCallbackResults.resultsCode;

                        if(callbackResults.Success())
                        {
                            List<string> sortedList = new List<string>();

                            foreach (var data in datas)
                            {
                                foreach (var content in contents)
                                {
                                    if (data.Contains(content))
                                    {
                                        if (!sortedList.Contains(content))
                                            sortedList.Insert(datas.IndexOf(data), content);
                                    }
                                }
                            }

                            AppData.Helpers.StringListValueValid(sortedList, hasValueCallbackResults => 
                            {
                                callbackResults.resultsCode = hasValueCallbackResults.resultsCode;

                                if(callbackResults.Success())
                                {
                                    callbackResults.results = $"{sortedList.Count} Content Sorted.";
                                    callbackResults.data = sortedList;
                                }
                                else
                                {
                                    callbackResults.results = "Couldn't Find Any Matching Contents To Sort.";
                                    callbackResults.data = default;
                                }
                            });
                        }
                        else
                        {
                            callbackResults.results = "Couldn't Get Content From Type - Please Check Here.";
                            callbackResults.data = default;
                        }
                    });
                }
                else
                {
                    callbackResults.results = "There Are No Content Assigned To Sort.";
                    callbackResults.data = default;
                }
            });

            callback.Invoke(callbackResults);
        }

        public int GetDropdownContentOptionRelativeIndex(Enum option, List<OptionData> options)
        {
            int index = 0;

            if (options != null && options.Count > 0)
            {
                var optionString = option.ToString();

                foreach (var optn in options)
                {
                    if (optionString.Contains(optn.text))
                    {
                        index = options.IndexOf(optn);
                        break;
                    }
                    else
                        continue;
                }
            }
            else
                LogError("Get Dropdown Content Option Relative Index Failed : Options Are Not Initialized / Null.", this);

            return index;
        }

        #endregion

        void OnActionEventSubscription(bool subscribe = false)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnActionButtonFieldUploadedEvent += OnActionButtonFieldUploadedEvent;
                AppData.ActionEvents._OnScreenRefreshed += OnScreenRefreshedEvent;
                AppData.ActionEvents._OnUpdateSceneAssetDefaultRotation += OnUpdateSceneAssetDefaultRotationEvent;
                AppData.ActionEvents._OnScreenExitEvent += ActionEvents__OnScreenExitEvent;
            }
            else
            {
                AppData.ActionEvents._OnActionButtonFieldUploadedEvent -= OnActionButtonFieldUploadedEvent;
                AppData.ActionEvents._OnScreenRefreshed -= OnScreenRefreshedEvent;
                AppData.ActionEvents._OnUpdateSceneAssetDefaultRotation -= OnUpdateSceneAssetDefaultRotationEvent; 
                AppData.ActionEvents._OnScreenExitEvent -= ActionEvents__OnScreenExitEvent;
            }
        }

        private void ActionEvents__OnScreenExitEvent(AppData.UIScreenType screenType)
        {
            GetDynamicWidgetsContainerForScreen(screenType, foundContainersCallbackResults => 
            {
                if (foundContainersCallbackResults.Success())
                {
                    foundContainersCallbackResults.data.ClearWidgets(true, widgetsClearedCallbackResults =>
                    {
                        if(widgetsClearedCallbackResults.Success())
                        {
                            ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(ScreenNavigationManager.Instance.GetEmptyFolderDataPackets().widgetType);
                            ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(AppData.WidgetType.LoadingWidget);
                        }
                    });
                }
                else
                    Log(foundContainersCallbackResults.resultsCode, foundContainersCallbackResults.results, this);
            });
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

        public void OnNewAssetDataCreated(AppData.AssetData assetData, Action<AppData.SceneAsset, bool> callback)
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
            LogSuccess($"==============> Get Scene Asset Container Of Type : {dataPackets.containerType} - For Screen : {dataPackets.screenType}", this);

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

                if (assetContainerList.Count == 0)
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

        public async void CreateNewAsset(AppData.AssetData assetData, Action<AppData.SceneAsset, bool> callback)
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

        async Task LoadSceneAsset(AppData.AssetData assetData, AppData.SceneAsset sceneAsset, Action<AppData.SceneAsset, bool> callback)
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

        public void GetProjectCategoryInfo(AppData.ProjectCategoryType categoryType, Action<AppData.CallbackData<AppData.ProjectCategoryInfo>> callback)
        {
            AppData.CallbackData<AppData.ProjectCategoryInfo> callbackResults = new AppData.CallbackData<AppData.ProjectCategoryInfo>();

            AppData.Helpers.ProjectDataComponentValid(projectCategoryInfoList, hasDataCallbackResults => 
            {
                callbackResults.resultsCode = hasDataCallbackResults.resultsCode;

                if (callbackResults.Success())
                {
                    var projectInfo = projectCategoryInfoList.Find(info => info.GetType() == categoryType);

                    if(projectInfo != null)
                    {
                        callbackResults.results = $"Project Info : {projectInfo.name} For Category : {categoryType} Found / Initialized.";
                        callbackResults.data = projectInfo;
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"Project Info For Category : {categoryType} Not Found / Initialized.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "Project Category List Has Not Been Initialized.";
                    callbackResults.data = default;
                }
            });

            callback.Invoke(callbackResults);
        }

        public AppData.CallbackData<AppData.StorageDirectoryData>  GetAppDirectoryData(AppData.DirectoryType directoryType)
        {
            AppData.CallbackData<AppData.StorageDirectoryData> callbackResults = new AppData.CallbackData<AppData.StorageDirectoryData>();

            AppData.StorageDirectoryData directoryData = new AppData.StorageDirectoryData();

            if (appDirectories.Count > 0)
            {
                directoryData = appDirectories.Find(data => data.type == directoryType);

                if(directoryData != null)
                {
                    callbackResults.results = $"App Directory Data Of Type : {directoryType} Found.";
                    callbackResults.data = directoryData;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"App Directory Data Of Type : {directoryType} Not Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "There Are No App Directories Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            return callbackResults;
        }

        public void BuildSceneAsset(AppData.StorageDirectoryData directoryData, Action<AppData.CallbackData<AppData.StorageDirectoryData>> callback = null)
        {
            AppData.CallbackData<AppData.StorageDirectoryData> callbackResults = new AppData.CallbackData<AppData.StorageDirectoryData>();

            if (currentSceneAsset.modelAsset != null)
            {
                DirectoryFound(directoryData.projectDirectory, directoryFoundCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(directoryFoundCallback.resultsCode))
                    {
                        AppData.AssetData assetData = currentSceneAsset.ToSceneAssetData();

                        if (assetData.mode == AppData.AssetModeType.CreateMode)
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

                                            string validPath = GetAppDirectory(field.directoryType).projectDirectory;
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
                        string filePath = Path.Combine(directoryData.projectDirectory, fileNameWithJSONExtension);
                        string formattedFilePath = AppData.Helpers.GetFormattedDirectoryPath(filePath);

                        AppData.StorageDirectoryData storageDirectory = new AppData.StorageDirectoryData
                        {
                            name = validAssetName,
                            type = AppData.DirectoryType.Sub_Folder_Structure,
                            path = formattedFilePath,
                            projectDirectory = directoryData.projectDirectory
                        };

                        assetData.creationDateTime = new AppData.DateTimeComponent(DateTime.Now);
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

        public AppData.ProjectStructureData GetFolderStructureData()
        {
            return GetCurrentProjectStructureData();
        }

        public string GetCreateNewFolderTempName()
        {
            // Do Some Calculations And Get A Tem Name.
            string tempName = string.Empty;
            AppData.Folder currentFolder = GetCurrentFolder();

            DirectoryFound(currentFolder.storageData.projectDirectory, directoryFoundCallback =>
            {
                if (AppData.Helpers.IsSuccessCode(directoryFoundCallback.resultsCode))
                {
                    string[] folderDataPathList = Directory.GetFiles(currentFolder.storageData.projectDirectory, "*_FolderData.json", SearchOption.TopDirectoryOnly);
                    string newTempName = "New Folder";

                    if (folderDataPathList.Length > 0)
                    {
                        List<string> folderNameList = new List<string>();
                        List<string> matchingFolderNameList = new List<string>();
                        string folderName = string.Empty;

                        foreach (var folderDataPath in folderDataPathList)
                        {
                            folderName = GetAssetNameFormatted(Path.GetFileNameWithoutExtension(folderDataPath), AppData.SelectableWidgetType.Folder);

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

        public void SetCurrentProjectStructureData(AppData.ProjectStructureData structureData = null)
        {
            currentProjectStructureData = structureData;
            SetCurrentFolder(currentProjectStructureData.rootFolder);
        }

        public AppData.ProjectStructureData GetCurrentProjectStructureData()
        {
            return currentProjectStructureData;
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

        public string GetFileDataName(string name, AppData.SelectableWidgetType assetType)
        {
            return ((assetType == AppData.SelectableWidgetType.Folder) ? name.Replace("_FolderData.json", "") : name.Replace("_FileData.json", ""));
        }

        public void GetDataNameWithExtension(string name, AppData.SelectableWidgetType type, Action<AppData.CallbackData<string>> callback)
        {
            AppData.CallbackData<string> callbackResults = new AppData.CallbackData<string>();

            GetFileData(type, dataFoundCallbackResults => 
            {
                callbackResults.results = dataFoundCallbackResults.results;
                callbackResults.resultsCode = dataFoundCallbackResults.resultsCode;

                if(callbackResults.Success())
                {
                    string dataType = $"_{type}Data.{dataFoundCallbackResults.data.extension.ToString().ToLower()}";
                    callbackResults.data = name + dataType;
                }
            });

            callback.Invoke(callbackResults);
        }

        public string GetDataNameWithoutExtension(string name, AppData.SelectableWidgetType type)
        {
            string dataName = string.Empty;

            if (name.Contains(".json"))
                dataName = name.Replace(".json", "");
            else
                dataName = name;

            return dataName;
        }

        #endregion

        public void SetDefaultUIWidgetActionState(List<AppData.UIScreenWidget> widgets, AppData.DefaultUIWidgetActionState widgetActionState, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (widgets != null)
            {
                foreach (var widget in widgets)
                {
                    if (widget.GetSelectableWidgetType() == AppData.SelectableWidgetType.Folder)
                    {
                        AppData.Folder folder = widget.GetFolderData();
                        string formattedName = GetFormattedName(folder.name, AppData.SelectableWidgetType.Folder, false);
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

                    if (widget.GetSelectableWidgetType() == AppData.SelectableWidgetType.Asset)
                    {
                        AppData.SceneAsset sceneAsset = widget.GetAssetData();
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

            if (DirectoryFound(currentFolder.storageData.projectDirectory))
            {
                CreateData(folder, currentFolder.storageData, (folderDataCreated) =>
                {
                    callbackResults.results = folderDataCreated.results;
                    callbackResults.resultsCode = folderDataCreated.resultsCode;

                    if (AppData.Helpers.IsSuccessCode(folderDataCreated.resultsCode))
                    {
                        if (!DirectoryFound(folder.storageData.projectDirectory))
                            CreateDirectory(folder.storageData.projectDirectory, (folderCreated) => { });
                    }
                    else
                        Debug.LogWarning($"--> Failed To Create DIrectory With Results : {folderDataCreated.results}");
                });
            }
            else
            {
                callbackResults.results = $"Directory : {currentFolder.storageData.projectDirectory} Doesn't Exist.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void SaveAssetWidget(AppData.SceneAsset sceneAsset, Action<AppData.Callback> callback = null)
        {

        }

        public string GetAssetNameFormatted(string assetName, AppData.SelectableWidgetType assetType)
        {
            string assetFormattedName = (assetType == AppData.SelectableWidgetType.Folder) ? assetName.Replace("_FolderData", "") : assetName.Replace("_FileData", "");

            return assetFormattedName;
        }

        public void ChangeFolderLayoutView(AppData.LayoutViewType viewType, AppData.SceneDataPackets dataPackets)
        {
            GetFolderStructureData().SetLayoutViewType(viewType);


            if (SelectableManager.Instance != null)
                SelectableManager.Instance.SmoothTransitionToSelection = false;
            else
                Debug.LogWarning("--> OpenUIFolderStructure Failed : SelectableManager.Instance Is Not Yet Initialized.");

            ScreenUIManager.Instance.Refresh();

            if (dataPackets.notification.showNotifications)
                NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);

            //SaveData(GetFolderStructureData(), (folderStructureDataSaved) =>
            //{
            //    if (AppData.Helpers.IsSuccessCode(folderStructureDataSaved.resultsCode))
            //    {
            //        if (SelectableManager.Instance != null)
            //            SelectableManager.Instance.SmoothTransitionToSelection = false;
            //        else
            //            Debug.LogWarning("--> OpenUIFolderStructure Failed : SelectableManager.Instance Is Not Yet Initialized.");

            //        ScreenUIManager.Instance.Refresh();

            //        if (dataPackets.notification.showNotifications)
            //            NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);
            //    }
            //    else
            //        Debug.LogWarning($"--> Save Data Failed With Results : {folderStructureDataSaved.results}");
            //});
        }

        public void ChangePaginationView(AppData.PaginationViewType paginationView, AppData.SceneDataPackets dataPackets)
        {
            GetFolderStructureData().SetPaginationViewType(paginationView);

            SaveData(GetFolderStructureData(), (folderStructureDataSaved) =>
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

        public void InitializeFolderLayoutView(AppData.LayoutViewType viewType)
        {
            var widgetsContainer = GetWidgetsRefreshData().widgetsContainer;

            if (widgetsContainer != null)
                GetWidgetsRefreshData().widgetsContainer.SetViewLayout(rootProjectStructureData.GetProjectStructureData().GetFolderLayoutView(viewType));
            else
                LogError("Widgets Container Not Found. Please Initialize First.", this);
        }

        public void GetLayoutViewType(Action<AppData.CallbackData<AppData.LayoutViewType>> callback)
        {
            AppData.CallbackData<AppData.LayoutViewType> callbackResults = new AppData.CallbackData<AppData.LayoutViewType>();

            var container = GetWidgetsRefreshData().widgetsContainer;

            if(container != null)
            {
                callbackResults.results = $"Content Container Found With Layout View Type : {container.GetLayout().viewType}.";
                callbackResults.data = container.GetLayout().viewType; ;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "There Is No Content Container Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public AppData.PaginationViewType GetPaginationViewType()
        {
            return GetFolderStructureData().GetPaginationViewType();
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

        public void GetDynamicWidgetsContainerForScreen(AppData.UIScreenType screenType, Action<AppData.CallbackData<DynamicWidgetsContainer>> callback)
        {
            AppData.CallbackData<DynamicWidgetsContainer> callbackResults = new AppData.CallbackData<DynamicWidgetsContainer>();

            if (dynamicWidgetsContainersList.Count > 0)
            {
                DynamicWidgetsContainer container = dynamicWidgetsContainersList.Find(container => container.GetUIScreenType() == screenType);

                if (container != null)
                {
                    callbackResults.results = $"Container For Screen Type : {screenType} Found.";
                    callbackResults.data = container;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Container For Screen Type : {screenType} Doesn't Exist In The Dynamic Widgets Container List - Not Found.";
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

        public void GetAllDynamicWidgetsContainers(Action<AppData.CallbackDataList<DynamicWidgetsContainer>> callback)
        {
            AppData.CallbackDataList<DynamicWidgetsContainer> callbackResults = new AppData.CallbackDataList<DynamicWidgetsContainer>();

            if (dynamicWidgetsContainersList.Count > 0)
            {
                callbackResults.results = $"{dynamicWidgetsContainersList.Count} Containers Found.";
                callbackResults.data = dynamicWidgetsContainersList;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Failed : dynamicWidgetsContainersList Is Null / Empty.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void GetAllDynamicWidgetsContainerExcludingFromScreen(AppData.UIScreenType screenType, Action<AppData.CallbackDataList<DynamicWidgetsContainer>> callback)
        {
            AppData.CallbackDataList<DynamicWidgetsContainer> callbackResults = new AppData.CallbackDataList<DynamicWidgetsContainer>();

            if (dynamicWidgetsContainersList.Count > 0)
            {
                List<DynamicWidgetsContainer> containers = dynamicWidgetsContainersList.FindAll(container => container.GetUIScreenType() != screenType);

                if (containers != null && containers.Count > 0)
                {
                    callbackResults.results = $"{containers.Count} Containers Found.";
                    callbackResults.data = containers;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Failed : There Are No Containers Found.";
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

        public List<AppData.Folder> GetFolders()
        {
            return folders;
        }

        #region On Create Functions

        #region Create UI

        public void CreateUIScreenProjectSelectionWidgets(AppData.UIScreenType screenType, List<AppData.ProjectStructureData> projectData, DynamicWidgetsContainer contentContainer, Action<AppData.CallbackDataList<AppData.Project>> callback)
        {
            try
            {
                AppData.CallbackDataList<AppData.Project> callbackResults = new AppData.CallbackDataList<AppData.Project>();

                if (contentContainer != null && contentContainer.IsContainerActive())
                {
                    contentContainer.InitializeContainer();

                    if (screenType == AppData.UIScreenType.ProjectSelectionScreen)
                    {
                        GetSortedProjectWidgetList(projectData, sortedListCallbackResults =>
                        {
                            callbackResults.results = sortedListCallbackResults.results;
                            callbackResults.resultsCode = sortedListCallbackResults.resultsCode;

                            if (callbackResults.Success())
                            {
                                GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                                {
                                    callbackResults.results = widgetsCallback.results;
                                    callbackResults.resultsCode = widgetsCallback.resultsCode;

                                    LogSuccess($"================> We Are Winning : {callbackResults.results}", this);

                                    if (callbackResults.Success())
                                    {
                                        var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                        if (widgetPrefabData != null)
                                        {
                                            widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableWidgetType.Project, AppData.LayoutViewType.ListView, prefabCallbackResults =>
                                            {
                                                callbackResults.results = prefabCallbackResults.results;
                                                callbackResults.resultsCode = prefabCallbackResults.resultsCode;

                                                if (prefabCallbackResults.Success())
                                                {
                                                    AppData.Helpers.UnityComponentValid(prefabCallbackResults.data.gameObject, "Project Widget Prefab Value", hasComponentCallbackResults =>
                                                   {
                                                       callbackResults.results = hasComponentCallbackResults.results;
                                                       callbackResults.resultsCode = hasComponentCallbackResults.resultsCode;

                                                       if (callbackResults.Success())
                                                       {
                                                           List<AppData.Project> projects = new List<AppData.Project>();

                                                           foreach (var project in sortedListCallbackResults.data)
                                                           {
                                                               GameObject projectWidget = Instantiate(hasComponentCallbackResults.data);

                                                               if (projectWidget != null)
                                                               {
                                                                   AppData.UIScreenWidget widgetComponent = projectWidget.GetComponent<AppData.UIScreenWidget>();

                                                                   if (widgetComponent != null)
                                                                   {
                                                                       //if (folderStructureData.currentPaginationViewType == AppData.PaginationViewType.Pager)
                                                                       //    widgetComponent.Hide();

                                                                       widgetComponent.SetProjectData(project);

                                                                       projectWidget.name = project.name;
                                                                       contentContainer.AddDynamicWidget(widgetComponent, contentContainer.GetContainerOrientation(), false);

                                                                       AppData.Project projectData = new AppData.Project
                                                                       {
                                                                           name = project.name,
                                                                           widget = widgetComponent,
                                                                           structureData = project
                                                                       };

                                                                       projects.Add(projectData);

                                                                       //if(!loadedProjectData.ContainsKey(project))
                                                                       //    loadedProjectData.Add(project, widgetComponent);

                                                                       callbackResults.results = $"Project Widget : { projectWidget.name} Created.";
                                                                       callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                                                   }
                                                                   else
                                                                   {
                                                                       callbackResults.results = "Project Widget Component Is Null.";
                                                                       callbackResults.data = default;
                                                                       callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                                   }
                                                               }
                                                               else
                                                               {
                                                                   callbackResults.results = "Project Widget Prefab Data Is Null.";
                                                                   callbackResults.data = default;
                                                                   callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                               }
                                                           }

                                                           if (callbackResults.Success())
                                                           {
                                                               callbackResults.results = "Project Widgets Loaded.";
                                                               callbackResults.data = projects;
                                                               callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                                           }
                                                           else
                                                           {
                                                               callbackResults.results = "Project Widgets Counldn't Load.";
                                                               callbackResults.data = default;
                                                               callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                           }
                                                       }
                                                   });
                                                }
                                                else
                                                    Log(prefabCallbackResults.resultsCode, prefabCallbackResults.results, this);
                                            });
                                        }
                                        else
                                        {
                                            callbackResults.results = $"DWidget Prefab For Screen Type : {screenType} Missing / Null.";
                                            callbackResults.data = default;
                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                        }
                                    }
                                    else
                                        Log(callbackResults.resultsCode, callbackResults.results, this);
                                });
                            }
                            else
                                Log(sortedListCallbackResults.resultsCode, sortedListCallbackResults.results, this);
                        });
                    }
                    else
                    {
                        callbackResults.results = "Current Screen Type Is Not Project Selection.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.WarningCode;
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

        public void CreateNewProjectStructureData(Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>> callback)
        {
            try
            {
                AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>();

                string newFolderDataFileName = !string.IsNullOrEmpty(CreateNewFolderName) ? CreateNewFolderName : GetCreateNewFolderTempName();

                DirectoryFound(GetCurrentFolder().storageData.projectDirectory, currentDirectoryFoundCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(currentDirectoryFoundCallback.resultsCode))
                    {
                        #region Get Folder File Storage Data

                        // Folder Storage Info
                        string newFolderDirectoryInfo = Path.Combine(GetCurrentFolder().storageData.projectDirectory, newFolderDataFileName);
                        string newFolderDirectory = newFolderDirectoryInfo.Replace("\\", "/");

                        // Folder Storage File Path

                        // Folder File Storage Data
                        string newFolderFileDataName = GetDataNameWithoutExtension(newFolderDataFileName, AppData.SelectableWidgetType.Folder);
                        string newStorageDataName = GetFormattedName(newFolderFileDataName, AppData.SelectableWidgetType.Folder, true);

                        GetDataNameWithExtension(newFolderDataFileName, AppData.SelectableWidgetType.Folder, dataFoundCallbackResults => 
                        {
                            callbackResults.results = dataFoundCallbackResults.results;
                            callbackResults.resultsCode = dataFoundCallbackResults.resultsCode;

                            if(callbackResults.Success())
                            {
                                string newFolderFileDataDirectoryInfo = Path.Combine(GetCurrentFolder().storageData.projectDirectory, dataFoundCallbackResults.data);
                                string newFolderFileDataDirectory = newFolderFileDataDirectoryInfo.Replace("\\", "/");

                                AppData.StorageDirectoryData newFolderFileDataStorageData = new AppData.StorageDirectoryData
                                {
                                    name = newFolderFileDataName,
                                    projectDirectory = newFolderDirectory
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
                        });                         
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

        public void CreateUIScreenFolderWidgets(AppData.UIScreenType screenType, AppData.Folder folder, DynamicWidgetsContainer contentContainer, Action<AppData.CallbackDataList<AppData.UIScreenWidget>> callback)
        {
            try
            {
                AppData.CallbackDataList<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDataList<AppData.UIScreenWidget>();

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
                                        callbackResults.results = sortedList.results;
                                        callbackResults.resultsCode = sortedList.resultsCode;

                                        if (AppData.Helpers.IsSuccessCode(sortedList.resultsCode))
                                        {
                                            GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                                            {
                                                callbackResults.results = widgetsCallback.results;
                                                callbackResults.resultsCode = widgetsCallback.resultsCode;

                                                if (widgetsCallback.Success())
                                                {
                                                    var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                                    if (widgetPrefabData != null)
                                                    {
                                                        widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableWidgetType.Folder, GetCurrentProjectStructureData().GetLayoutViewType(), prefabCallbackResults =>
                                                        {
                                                            callbackResults.results = prefabCallbackResults.results;
                                                            callbackResults.resultsCode = prefabCallbackResults.resultsCode;

                                                            if (prefabCallbackResults.Success())
                                                            {
                                                                pinnedFolders = sortedList.data;

                                                                LogSuccess($"====> Loaded - {pinnedFolders.Count} Pinned Items - Prefab : {prefabCallbackResults.data.gameObject.name}", this);

                                                                foreach (var folder in pinnedFolders)
                                                                {
                                                                    GameObject folderWidget = Instantiate(prefabCallbackResults.data.gameObject);

                                                                    if (folderWidget != null)
                                                                    {
                                                                        AppData.UIScreenWidget widgetComponent = folderWidget.GetComponent<AppData.UIScreenWidget>();

                                                                        if (widgetComponent != null)
                                                                        {
                                                                            widgetComponent.SetDefaultUIWidgetActionState(folder.defaultWidgetActionState);

                                                                            if (GetCurrentProjectStructureData().paginationViewType == AppData.PaginationViewType.Pager)
                                                                                widgetComponent.Hide();

                                                                            folderWidget.name = folder.name;
                                                                            widgetComponent.SetFolderData(folder);
                                                                            contentContainer.AddDynamicWidget(widgetComponent, contentContainer.GetContainerOrientation(), false);

                                                                            if (!loadedWidgetsList.Contains(widgetComponent))
                                                                                loadedWidgetsList.Add(widgetComponent);

                                                                            callbackResults.results = "Folder Widget Loaded.";
                                                                            callbackResults.data = loadedWidgetsList;
                                                                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                                                        }
                                                                        else
                                                                        {
                                                                            callbackResults.results = "Folder Widget Component Missing.";
                                                                            callbackResults.data = default;
                                                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        callbackResults.results = "Folder Widget Failed To Instantiate.";
                                                                        callbackResults.data = default;
                                                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
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

        public void CreateUIScreenFolderWidgets(AppData.UIScreenType screenType, List<AppData.StorageDirectoryData> foldersDirectoryList, DynamicWidgetsContainer contentContainer, Action<AppData.CallbackDataList<AppData.UIScreenWidget>> callback)
        {
            try
            {
                AppData.CallbackDataList<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDataList<AppData.UIScreenWidget>();

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
                                                        widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableWidgetType.Folder, GetFolderStructureData().GetLayoutViewType(), prefabCallbackResults =>
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

                                                                            if (GetFolderStructureData().paginationViewType == AppData.PaginationViewType.Pager)
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

        public void CreateUIScreenFileWidgets(AppData.UIScreenType screenType, AppData.Folder folder, DynamicWidgetsContainer contentContainer, Action<AppData.CallbackDataList<AppData.UIScreenWidget>> callback)
        {
            try
            {
                AppData.CallbackDataList<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDataList<AppData.UIScreenWidget>();

                if (contentContainer != null && contentContainer.IsContainerActive())
                {
                    switch (screenType)
                    {
                        case AppData.UIScreenType.ProjectViewScreen:

                            LoadSceneAssets(folder, (loadedAssetsResults) =>
                            {
                                callbackResults.results = loadedAssetsResults.results;
                                callbackResults.resultsCode = loadedAssetsResults.resultsCode;

                                if (AppData.Helpers.IsSuccessCode(loadedAssetsResults.resultsCode))
                                {
                                    sceneAssetList = new List<AppData.SceneAsset>();

                                    if (loadedAssetsResults.data.Count > 0)
                                    {
                                        List<AppData.UIScreenWidget> loadedWidgetsList = new List<AppData.UIScreenWidget>();

                                        GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                                        {
                                            callbackResults.results = widgetsCallback.results;
                                            callbackResults.resultsCode = widgetsCallback.resultsCode;

                                            if (widgetsCallback.Success())
                                            {
                                                var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                                if (widgetPrefabData != null)
                                                {
                                                    widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableWidgetType.Asset, GetFolderStructureData().GetLayoutViewType(), prefabCallbackResults =>
                                                    {
                                                        callbackResults.results = prefabCallbackResults.results;
                                                        callbackResults.resultsCode = prefabCallbackResults.resultsCode;

                                                        if (prefabCallbackResults.Success())
                                                        {
                                                            foreach (AppData.SceneAsset asset in loadedAssetsResults.data)
                                                            {
                                                                if (!sceneAssetList.Contains(asset))
                                                                {
                                                                    GameObject newWidget = Instantiate(prefabCallbackResults.data.gameObject);

                                                                    if (newWidget != null)
                                                                    {
                                                                        AppData.UIScreenWidget widgetComponent = newWidget.GetComponent<AppData.UIScreenWidget>();

                                                                        if (widgetComponent != null)
                                                                        {
                                                                            widgetComponent.SetDefaultUIWidgetActionState(asset.defaultWidgetActionState);

                                                                            if (GetFolderStructureData().paginationViewType == AppData.PaginationViewType.Pager)
                                                                                widgetComponent.Hide();

                                                                            newWidget.name = asset.name;

                                                                            widgetComponent.SetAssetData(asset);
                                                                            widgetComponent.SetWidgetParentScreen(ScreenUIManager.Instance.GetCurrentScreenData().value);
                                                                            widgetComponent.SetWidgetAssetData(asset);

                                                                            contentContainer.AddDynamicWidget(widgetComponent, contentContainer.GetContainerOrientation(), false);

                                                                            sceneAssetList.Add(asset);

                                                                            AppData.SceneAssetWidget assetWidget = new AppData.SceneAssetWidget();
                                                                            assetWidget.name = widgetComponent.GetAssetData().name;
                                                                            assetWidget.value = newWidget;
                                                                            assetWidget.categoryType = widgetComponent.GetAssetData().categoryType;
                                                                            assetWidget.creationDateTime = widgetComponent.GetAssetData().creationDateTime.dateTime;

                                                                            screenWidgetList.Add(assetWidget);

                                                                            //widgetComponent.SetFileData();

                                                                            if (!loadedWidgetsList.Contains(widgetComponent))
                                                                                loadedWidgetsList.Add(widgetComponent);

                                                                            callbackResults.results = "Widget Prefab Loaded.";
                                                                            callbackResults.data = loadedWidgetsList;
                                                                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                                                        }
                                                                        else
                                                                        {
                                                                            callbackResults.results = "Widget Prefab Component Missing.";
                                                                            callbackResults.data = default;
                                                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        callbackResults.results = "Widget Prefab Failed To Instantiate.";
                                                                        callbackResults.data = default;
                                                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                                    }
                                                                }
                                                                else
                                                                    Debug.LogWarning($"--> Widget : {asset.modelAsset.name} Already Exists.");
                                                            }
                                                        }
                                                        else
                                                        {

                                                            LogError($"==================> Widget Prefab Not Found Results : {prefabCallbackResults.results}", this);
                                                            Log(prefabCallbackResults.resultsCode, prefabCallbackResults.results, this);
                                                        }
                                                    });
                                                }
                                                else
                                                    LogError("Widget Prefab Data Missing.", this);
                                            }
                                        });

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

        public void CreateUIScreenFileWidgets(AppData.UIScreenType screenType, List<AppData.StorageDirectoryData> filesDirectoryList, DynamicWidgetsContainer contentContainer, Action<AppData.CallbackDataList<AppData.UIScreenWidget>> callback)
        {
            try
            {
                AppData.CallbackDataList<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDataList<AppData.UIScreenWidget>();

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

                                        GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                                        {
                                            callbackResults.results = widgetsCallback.results;
                                            callbackResults.resultsCode = widgetsCallback.resultsCode;

                                            if (widgetsCallback.Success())
                                            {
                                                var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                                if (widgetPrefabData != null)
                                                {
                                                    widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableWidgetType.Folder, GetFolderStructureData().GetLayoutViewType(), prefabCallbackResults =>
                                                    {
                                                        callbackResults.results = prefabCallbackResults.results;
                                                        callbackResults.resultsCode = prefabCallbackResults.resultsCode;

                                                        if (prefabCallbackResults.Success())
                                                        {
                                                            foreach (AppData.SceneAsset asset in loadedAssetsResults.data)
                                                            {
                                                                if (!sceneAssetList.Contains(asset))
                                                                {
                                                                    GameObject newWidget = Instantiate(prefabCallbackResults.data.gameObject);

                                                                    if (newWidget != null)
                                                                    {
                                                                        AppData.UIScreenWidget widgetComponent = newWidget.GetComponent<AppData.UIScreenWidget>();

                                                                        LogSuccess($"=================> hecking Component For Asset Data : {asset.name}.", this);

                                                                        if (widgetComponent != null)
                                                                        {

                                                                            LogSuccess($"=================> Setting File : {widgetComponent.name} Asset Data : {asset.name}.", this);

                                                                            widgetComponent.SetDefaultUIWidgetActionState(asset.defaultWidgetActionState);

                                                                            if (GetFolderStructureData().paginationViewType == AppData.PaginationViewType.Pager)
                                                                                widgetComponent.Hide();

                                                                            newWidget.name = asset.name;

                                                                            widgetComponent.SetAssetData(asset);
                                                                            widgetComponent.SetWidgetParentScreen(ScreenUIManager.Instance.GetCurrentScreenData().value);
                                                                            widgetComponent.SetWidgetAssetData(asset);

                                                                            contentContainer.AddDynamicWidget(widgetComponent, contentContainer.GetContainerOrientation(), false);

                                                                            sceneAssetList.Add(asset);

                                                                            AppData.SceneAssetWidget assetWidget = new AppData.SceneAssetWidget();
                                                                            assetWidget.name = widgetComponent.GetAssetData().name;
                                                                            assetWidget.value = newWidget;
                                                                            assetWidget.categoryType = widgetComponent.GetAssetData().categoryType;
                                                                            assetWidget.creationDateTime = widgetComponent.GetAssetData().creationDateTime.dateTime;

                                                                            screenWidgetList.Add(assetWidget);

                                                                            if (!loadedWidgetsList.Contains(widgetComponent))
                                                                                loadedWidgetsList.Add(widgetComponent);

                                                                            callbackResults.results = "Widget Prefab Component Loaded.";
                                                                            callbackResults.data = loadedWidgetsList;
                                                                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                                                        }
                                                                        else
                                                                        {
                                                                            callbackResults.results = "Widget Prefab Component Missing.";
                                                                            callbackResults.data = default;
                                                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        callbackResults.results = $"Failed To Instantiate Prefab For Screen Widget : {asset.modelAsset.name}";
                                                                        callbackResults.data = default;
                                                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                                    }
                                                                }
                                                                else
                                                                    Debug.LogWarning($"--> Widget : {asset.modelAsset.name} Already Exists.");
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

        public void CreateNewProjectStructureData(AppData.ProjectStructureData newProject, Action<AppData.CallbackData<AppData.ProjectStructureData>> callback = null)
        {
            try
            {
                AppData.CallbackData<AppData.ProjectStructureData> callbackResults = new AppData.CallbackData<AppData.ProjectStructureData>();

                AppData.Helpers.SerializableComponentValid(newProject, componentIsValidCallbackResults => 
                {
                    callbackResults.results = componentIsValidCallbackResults.results;
                    callbackResults.resultsCode = componentIsValidCallbackResults.resultsCode;

                    if (callbackResults.Success())
                    {
                        if (GetAppDirectoryData(newProject.rootFolder.directoryType).Success())
                        {
                            AppData.StorageDirectoryData directoryData = GetAppDirectoryData(newProject.rootFolder.directoryType).data;

                            if (DirectoryFound(directoryData))
                            {
                                GetDataNameWithExtension(newProject.name, AppData.SelectableWidgetType.Project, fileNameCallbackResults =>
                                {
                                    callbackResults.results = fileNameCallbackResults.results;
                                    callbackResults.resultsCode = fileNameCallbackResults.resultsCode;

                                    if (callbackResults.Success())
                                    {
                                        string fileNameWithoutExtension = GetDataNameWithoutExtension(fileNameCallbackResults.data, AppData.SelectableWidgetType.Project);

                                        var storageData = GetAppDirectoryData(newProject.rootFolder.directoryType).data;

                                        string path = Path.Combine(storageData.projectDirectory, fileNameCallbackResults.data);
                                        string validPath = path.Replace("\\", "/");

                                        //string directory = Path.Combine(storageData.directory, mainFolder.name);
                                        string projectDirectory = storageData.projectDirectory.Replace("\\", "/");
                                        string folderDirectory = Path.Combine(projectDirectory, newProject.name);
                                        string rootDirectory = folderDirectory.Replace("\\", "/");

                                        storageData.name = fileNameWithoutExtension;
                                        storageData.path = validPath;
                                        storageData.projectDirectory = projectDirectory;
                                        storageData.rootDirectory = rootDirectory;

                                        newProject.projectInfo.name = newProject.name;
                                        newProject.storageData = storageData;
                                        newProject.rootFolder.name = newProject.name;
                                        newProject.rootFolder.isRootFolder = true;
                                        newProject.rootFolder.storageData = storageData;

                                        CreateData(newProject, directoryData, (folderStructureCreated) =>
                                        {
                                            callbackResults = folderStructureCreated;

                                            if (folderStructureCreated.Success())
                                            {
                                                CreateDirectory(rootDirectory, directoryCreatedCallback =>
                                                {
                                                    callbackResults.resultsCode = directoryCreatedCallback.resultsCode;

                                                    if (directoryCreatedCallback.Success())
                                                        callbackResults.results = $"A New Project Titled : {newProject.name} Has Been Created.";
                                                    else
                                                        LogWarning(directoryCreatedCallback.results, this);
                                                });
                                            }
                                            else
                                                Log(folderStructureCreated.resultsCode, folderStructureCreated.results, this);

                                        });
                                    }
                                });
                            }
                            else
                            {
                                callbackResults.results = $"Directory : {directoryData.projectDirectory} Not Found.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        }
                        else
                            Log(GetAppDirectoryData(newProject.rootFolder.directoryType).resultsCode, GetAppDirectoryData(newProject.rootFolder.directoryType).results, this);
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
                        directoryData.projectDirectory = overrideActivity.Call<string>("GetAppDataDirectory", directoryData.projectDirectory);
                    else
                        Debug.LogWarning("--> RG_Unity - Asset Import Content Manager Referenced Plugin Instance Is Null.");

                    //if (AssetImportContentManager.Instance.GetReferencedPluginInstance() != null)
                    //    directoryData.directory = AssetImportContentManager.Instance.GetReferencedPluginInstance().Call<string>("GetAppDataDirectory", directoryData.directory);
                    //else
                    //    Debug.LogWarning("--> RG_Unity - Asset Import Content Manager Referenced Plugin Instance Is Null.");

                    if (Directory.Exists(directoryData.projectDirectory))
                    {
                        if (!appDirectories.Contains(directoryData))
                            appDirectories.Add(directoryData);

                        callbackResults.results = "Success : Directory Exists.";
                        callbackResults.data = directoryData;
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"--> Failed To Create Directory : {directoryData.projectDirectory}";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    Directory.CreateDirectory(directoryData.projectDirectory);

                    if (Directory.Exists(directoryData.projectDirectory))
                    {
                        if (!appDirectories.Contains(directoryData))
                            appDirectories.Add(directoryData);

                        callbackResults.results = "Success : Directory Exists.";
                        callbackResults.data = directoryData;
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"--> Failed To Create Directory : {directoryData.projectDirectory}";
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
                        if (Directory.Exists(directoryData.projectDirectory))
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
                            if (Directory.Exists(appDirectory.projectDirectory))
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

                if (!string.IsNullOrEmpty(folder.storageData.projectDirectory))
                {
                    if (DirectoryFound(folder.storageData.projectDirectory))
                    {
                        string[] files = Directory.GetFiles(folder.storageData.projectDirectory);

                        if (files.Length > 0)
                        {
                            List<string> validFiles = new List<string>();

                            for (int i = 0; i < files.Length; i++)
                                if (files[i].Contains(".json") && !files[i].Contains(".meta"))
                                    validFiles.Add(files[i]);

                            if (validFiles.Count > 0)
                            {
                                callbackResults.results = $"GetFolderContentCount Success - Directory : {folder.storageData.projectDirectory} Found.";
                                callbackResults.data = validFiles.Count;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = $"GetFolderContentCount Failed - There Were No Valid Files Found In Directory : {folder.storageData.projectDirectory}.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = $"GetFolderContentCount Failed - There Were No Files Found In Directory : {folder.storageData.projectDirectory}.";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"GetFolderContentCount Failed - Directory : {folder.storageData.projectDirectory} Not Found.";
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

        public string GetFormattedName(string source, AppData.SelectableWidgetType assetType, bool isDisplayName = true, int length = 0)
        {
            try
            {
                string formattedName = string.Empty;

                if (isDisplayName)
                {
                    bool isValid = (assetType == AppData.SelectableWidgetType.Folder) ? !source.Contains("_FolderData") : !source.Contains("_FileData");

                    if (isValid)
                        formattedName = source;
                    else
                        formattedName = (assetType == AppData.SelectableWidgetType.Folder) ? source.Replace("_FolderData", "") : source.Replace("_FileData", "");
                }
                else
                {
                    if (source.Contains("_FolderData") || source.Contains("_FileData"))
                        formattedName = source;
                    else
                        formattedName = (assetType == AppData.SelectableWidgetType.Folder) ? source + "_FolderData" : source + "_FileData";
                }

                return formattedName;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void SetWidgetsRefreshData(AppData.Folder folder, DynamicWidgetsContainer widgetsContainer, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            AppData.Helpers.ComponentValid(widgetsContainer, componentValidCallbackResults => 
            {
                callbackResults.results = componentValidCallbackResults.results;
                callbackResults.resultsCode = componentValidCallbackResults.resultsCode;

                if (callbackResults.Success())
                    widgetsRefreshDynamicContainer = widgetsContainer;
            });

            widgetsRefreshFolder = folder;

            //AppData.Helpers.StringValueValid(folder.GetDirectoryData().directory, folderValidCallbackResults => 
            //{
            //    callbackResults.results = folderValidCallbackResults.results;
            //    callbackResults.resultsCode = folderValidCallbackResults.resultsCode;

            //    if(callbackResults.Success())
            //    {
            //        if (DirectoryFound(folder.GetDirectoryData().directory))
            //        {
            //            widgetsRefreshFolder = folder;

            //            callbackResults.results = $"Root Folder Directory : {folder.GetDirectoryData().directory} Found.";
            //            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            //        }
            //        else
            //        {
            //            callbackResults.results = $"Root Folder Directory : {folder.GetDirectoryData().directory} Doesn't Exist.";
            //            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            //        }
            //    }
            //});

            callback?.Invoke(callbackResults);
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

        public void GetContentContainer(Action<AppData.CallbackData<DynamicWidgetsContainer>> callback)
        {
            AppData.CallbackData<DynamicWidgetsContainer> callbackResults = new AppData.CallbackData<DynamicWidgetsContainer>();

            var container = GetWidgetsRefreshData().widgetsContainer;

            if(container != null && container.IsContainerActive())
            {
                callbackResults.results = $"Widgets Container : {container.name} Found.";
                callbackResults.data = container;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Couldn't Get Widgets Container Or Container Is Inative.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void GetProjectStructure(Action<AppData.CallbackData<AppData.ProjectStructureData>> callback)
        {
            AppData.CallbackData<AppData.ProjectStructureData> callbackResults = new AppData.CallbackData<AppData.ProjectStructureData>();

            if(GetCurrentProjectStructureData() != null)
            {
                callbackResults.results = $"Project Structure : {GetCurrentProjectStructureData().name} Assigned.";
                callbackResults.data = GetCurrentProjectStructureData();
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Couldn't Get Current Project Structure Data - Value Is Null / Not Yet Initialized.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        #region On Refresh Functions

        public bool Refreshed(AppData.Folder folder, DynamicWidgetsContainer widgetsContainer, AppData.SceneDataPackets dataPackets)
        {
            try
            {
                bool isRefreshed = false;

                if (IsInitialized)
                {
                    if (ScreenUIManager.Instance != null)
                    {
                        widgetsContainer.SetAssetsLoaded(isRefreshed);

                        if (dataPackets.refreshScreenOnLoad)
                        {
                            switch (dataPackets.screenType)
                            {
                                case AppData.UIScreenType.ProjectSelectionScreen:

                                    widgetsContainer.ClearWidgets(false, widgetsClearedCallback =>
                                    {
                                        if (widgetsClearedCallback.Success())
                                        {
                                            loadedProjectData = new List<AppData.Project>();

                                            LoadProjectStructureData((structureLoader) =>
                                            {
                                                #region Screen UI Params

                                                var paginationButtonParam = GetUIScreenGroupContentTemplate("Pagination View Button", AppData.InputType.Button, buttonActionType: AppData.InputActionButtonType.PaginationButton, state: AppData.InputUIState.Disabled);
                                                var searchFieldParam = GetUIScreenGroupContentTemplate("Search Field", AppData.InputType.InputField, inputFieldActionType: AppData.InputFieldActionType.AssetSearchField, placeHolder: "Search", state: AppData.InputUIState.Disabled);
                                                var filterListParam = GetUIScreenGroupContentTemplate("Filter Content", AppData.InputType.DropDown, dropdownActionType: AppData.InputDropDownActionType.FilterList, placeHolder: "Filter", state: AppData.InputUIState.Disabled);
                                                var sortingListParam = GetUIScreenGroupContentTemplate("Sorting Content", AppData.InputType.DropDown, dropdownActionType: AppData.InputDropDownActionType.SortingList, placeHolder: "Sort", state: AppData.InputUIState.Disabled);
                                     
                                                #endregion

                                                if (structureLoader.Success())
                                                {
                                                    if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == AppData.UIScreenType.ProjectSelectionScreen)
                                                    {
                                                        CreateUIScreenProjectSelectionWidgets(ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType(), structureLoader.data, widgetsContainer, createProjectWidgetCallback =>
                                                        {
                                                            if (createProjectWidgetCallback.Success())
                                                            {
                                                                loadedProjectData = createProjectWidgetCallback.data;

                                                                GetFilterTypesFromContent(structureLoader.data, filterContentCallbackResults =>
                                                                {
                                                                    if (filterContentCallbackResults.Success())
                                                                    {
                                                                        var sortedFilterList = filterContentCallbackResults.data;
                                                                        sortedFilterList.Sort((x, y) => x.CompareTo(y));
                                                                        sortedFilterList.Insert(0, "All");
                                                                        filterListParam.SetContent(sortedFilterList);
                                                                    }
                                                                    else
                                                                        Log(filterContentCallbackResults.resultsCode, filterContentCallbackResults.results, this);
                                                                }, "Project_");

                                                                if (GetProjectRootStructureData().Success())
                                                                {
                                                                    var filterType = GetProjectRootStructureData().data.GetProjectStructureData().GetProjectInfo().GetCategoryType();
                                                                    var sortingContents = GetDropdownContent<AppData.SortType>().data;

                                                                    if (filterType != AppData.ProjectCategoryType.Project_All)
                                                                    {
                                                                        AppData.Helpers.StringListValueValid(sortingContents, isValidCallbackResults =>
                                                                        {
                                                                            if (isValidCallbackResults.Success())
                                                                                sortingContents.Remove(sortingContents.Find(content => content.Contains("Category")));
                                                                            else
                                                                                Log(isValidCallbackResults.resultsCode, isValidCallbackResults.results, this);
                                                                        });
                                                                    }

                                                                    sortingListParam.SetContent(sortingContents);
                                                                }
                                                                else
                                                                    Log(GetProjectRootStructureData().resultsCode, GetProjectRootStructureData().results, this);

                                                                #region Enable UI Screen Group COntent

                                                                paginationButtonParam.SetUIInputState(widgetsContainer.CanPaginate()? AppData.InputUIState.Enabled : AppData.InputUIState.Disabled);
                                                                searchFieldParam.SetUIInputState(AppData.InputUIState.Enabled);
                                                                filterListParam.SetUIInputState(AppData.InputUIState.Enabled);
                                                                sortingListParam.SetUIInputState(AppData.InputUIState.Enabled);

                                                                #endregion

                                                                SetContentScreenUIStatesEvent(paginationButtonParam, searchFieldParam, filterListParam, sortingListParam);
                                                            }
                                                            else
                                                                Log(createProjectWidgetCallback.resultsCode, createProjectWidgetCallback.results, this);
                                                        });
                                                    }
                                                    else
                                                        LogError($"Folder Structure Screen : {ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType()}", this);
                                                }
                                                else
                                                    SetContentScreenUIStatesEvent(paginationButtonParam, searchFieldParam, filterListParam, sortingListParam);
                                            });


                                            isRefreshed = true;
                                        }
                                        else
                                            Log(widgetsClearedCallback.resultsCode, widgetsClearedCallback.results, this);
                                    });

                                    break;

                                case AppData.UIScreenType.ProjectViewScreen:

                                    widgetsContainer.ClearWidgets(false, widgetsClearedCallback =>
                                    {
                                        if (widgetsClearedCallback.Success())
                                        {
                                            if (GetCurrentProjectStructureData() != null)
                                            {
                                                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == AppData.UIScreenType.ProjectViewScreen)
                                                {
                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, GetCurrentProjectStructureData().projectInfo.name);

                                                    SetCurrentFolder(folder);

                                                    GetWidgetsRefreshData().widgetsContainer.SetViewLayout(GetFolderStructureData().GetFolderLayoutView(GetFolderStructureData().GetLayoutViewType()));

                                                    RefreshLayoutViewButtonIcon();

                                                    FolderHasContentToLoad(folder, hasContentCallbackResults =>
                                                    {
                                                        if (hasContentCallbackResults.Success())
                                                        {
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

                                                                GetLayoutViewType(layoutViewCallbackResults =>
                                                                {
                                                                    if (layoutViewCallbackResults.Success())
                                                                    {
                                                                        switch (layoutViewCallbackResults.data)
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
                                                                    }
                                                                    else
                                                                        Log(layoutViewCallbackResults.resultsCode, layoutViewCallbackResults.results, this);
                                                                });

                                                                ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.ButtonIcon, selectionOptionImageViewType);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.LoadingWidget);

                                                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.InputUIState.Disabled);
                                                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Disabled);
                                                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ClipboardButton, AppData.InputUIState.Disabled);

                                                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputUIState.Disabled);
                                                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldState(AppData.InputFieldActionType.AssetSearchField, AppData.InputUIState.Disabled);
                                                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldPlaceHolderText(AppData.InputFieldActionType.AssetSearchField, string.Empty);

                                                            isRefreshed = true;

                                                            StartCoroutine(RefreshAssetsAsync());

                                                            Log(hasContentCallbackResults.resultsCode, hasContentCallbackResults.results, this);
                                                        }

                                                    });

                                                }
                                            }
                                            else
                                                LogWarning("Project Structure Data Is Null", this);
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
                }
                else
                    LogError("Assets Manager Is Not Initialized Yet.", this);

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
            switch (GetFolderStructureData().GetLayoutViewType())
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

        public AppData.UIScreenGroupContent GetUIScreenGroupContentTemplate(string name, AppData.InputType inputType, AppData.InputUIState state = AppData.InputUIState.Normal,  
            string placeHolder = null, string content = null, List<string> contents = null, bool value = false, AppData.InputActionButtonType buttonActionType = AppData.InputActionButtonType.None, 
            AppData.InputFieldActionType inputFieldActionType = AppData.InputFieldActionType.None, AppData.InputDropDownActionType dropdownActionType = AppData.InputDropDownActionType.None, 
            AppData.CheckboxInputActionType checkboxActionType = AppData.CheckboxInputActionType.None)
        {
            var groupContent = new AppData.UIScreenGroupContent
            {
                name = name,
                inputType = inputType,
                state = state,
                placeHolder = placeHolder,
                content = content,
                contents = contents,
                value = value,

                buttonActionType = buttonActionType,
                inputFieldActionType = inputFieldActionType,
                dropDownActionType = dropdownActionType,
                checkboxActionType = checkboxActionType
            };

            return groupContent;
        }

        public void SetContentScreenUIStatesEvent(params AppData.UIScreenGroupContent[] actions)
        {
            if (actions != null && actions.Length > 0)
            {
                foreach (var action in actions)
                {
                    switch (action.inputType)
                    {
                        case AppData.InputType.Button:

                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(action.buttonActionType, action.state);

                            break;

                        case AppData.InputType.InputField:

                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldState(action.inputFieldActionType, action.state);
                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldPlaceHolderText(action.inputFieldActionType, action.placeHolder);

                            break;

                        case AppData.InputType.DropDown:

                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownOptions(action.dropDownActionType, action);

                            break;
                    }
                }
            }
        }

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
            switch (GetFolderStructureData().GetPaginationViewType())
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

            widgetsContainer.SetPaginationView(GetFolderStructureData().GetPaginationViewType());
        }

        #region Content Load

        public void FolderHasContentToLoad(AppData.Folder folder, Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (DirectoryFound(folder.storageData.projectDirectory))
            {
                var folders = Directory.GetFiles(folder.storageData.projectDirectory, "*_FolderData.json", SearchOption.TopDirectoryOnly).ToList();
                var files = Directory.GetFiles(folder.storageData.projectDirectory, "*_FileData.json", SearchOption.TopDirectoryOnly).ToList();

                if(folders.Count == 0 && files.Count == 0)
                {
                    callbackResults.results = $"Directory : {folder.storageData.projectDirectory} Has No Content.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
                else
                {
                    callbackResults.results = $"Directory : {folder.storageData.projectDirectory} Has Content.";
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
            }
            else
            {
                callbackResults.results = $"Directory : {folder.storageData.projectDirectory} Not Found.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void LoadFolderData(AppData.Folder folder, Action<AppData.CallbackDataList<AppData.Folder>> callback)
        {
            AppData.CallbackDataList<AppData.Folder> callbackResults = new AppData.CallbackDataList<AppData.Folder>();

            if (DirectoryFound(folder.storageData.projectDirectory))
            {
                List<AppData.Folder> loadedFolders = new List<AppData.Folder>();

                var folderDataList = Directory.GetFiles(folder.storageData.projectDirectory, "*_FolderData.json", SearchOption.TopDirectoryOnly).ToList();

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
                        callbackResults.results = $"Failed - No Valid Files Loaded In Directory : {folder.storageData.projectDirectory} Not Found.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"Failed - No Files Found In Directory : {folder.storageData.projectDirectory} Not Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = $"Failed - Directory : {folder.storageData.projectDirectory} Not Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void LoadFolderData(List<AppData.StorageDirectoryData> folderDirectoryDataList, Action<AppData.CallbackDataList<AppData.Folder>> callback)
        {
            AppData.CallbackDataList<AppData.Folder> callbackResults = new AppData.CallbackDataList<AppData.Folder>();

            if (folderDirectoryDataList != null && folderDirectoryDataList.Count > 0)
            {
                List<AppData.Folder> loadedFolders = new List<AppData.Folder>();

                foreach (var folderDirectory in folderDirectoryDataList)
                {
                    FileFound(folderDirectory.projectDirectory, folderFoundCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(folderFoundCallback.resultsCode))
                        {
                            if (folderDirectory.projectDirectory.Contains("_FolderData.json"))
                            {
                                string directory = folderDirectory.projectDirectory.Replace("_FolderData.json", "");

                                DirectoryFound(directory, directoryexistCallack =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(directoryexistCallack.resultsCode))
                                    {
                                        string JSONString = File.ReadAllText(folderDirectory.projectDirectory);
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

        public void LoadSceneAssets(AppData.Folder folder, Action<AppData.CallbackDataList<AppData.SceneAsset>> callback)
        {
            AppData.CallbackDataList<AppData.SceneAsset> callbackResults = new AppData.CallbackDataList<AppData.SceneAsset>();

            DirectoryFound(folder.storageData.projectDirectory, foundDirectoryCallback =>
            {
                if (AppData.Helpers.IsSuccessCode(foundDirectoryCallback.resultsCode))
                {
                    List<AppData.SceneAsset> loadedAssetsList = new List<AppData.SceneAsset>();

                    string[] fileDataList = Directory.GetFiles(folder.storageData.projectDirectory, "*.json");

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
                                AppData.AssetData sceneAssetData = JsonUtility.FromJson<AppData.AssetData>(JSONString);

                                AppData.SceneAsset sceneAsset = sceneAssetData.ToSceneAsset();

                                if (!loadedAssetsList.Contains(sceneAsset))
                                    loadedAssetsList.Add(sceneAsset);
                            }

                            if (loadedAssetsList.Count > 0)
                            {
                                callbackResults.results = $"Success - {loadedAssetsList.Count} : Files Found In Directory : {folder.storageData.projectDirectory}";
                                callbackResults.data = loadedAssetsList;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                            else
                            {
                                callbackResults.results = $"No Loaded Assets Files Found In Directory : {folder.storageData.projectDirectory}";
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = $"No Valid Files Found In Directory : {folder.storageData.projectDirectory}";
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

        public void LoadSceneAssets(List<AppData.StorageDirectoryData> fileDirectoryDataList, Action<AppData.CallbackDataList<AppData.SceneAsset>> callback)
        {
            AppData.CallbackDataList<AppData.SceneAsset> callbackResults = new AppData.CallbackDataList<AppData.SceneAsset>();

            if (fileDirectoryDataList != null && fileDirectoryDataList.Count > 0)
            {
                List<AppData.SceneAsset> loadedFiles = new List<AppData.SceneAsset>();

                foreach (var fileDirectory in fileDirectoryDataList)
                {
                    FileFound(fileDirectory.projectDirectory, fileFoundCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(fileFoundCallback.resultsCode))
                        {
                            string JSONString = File.ReadAllText(fileDirectory.projectDirectory);
                            AppData.AssetData fileData = JsonUtility.FromJson<AppData.AssetData>(JSONString);

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

        public void LoadProjectStructureData(Action<AppData.CallbackDataList<AppData.ProjectStructureData>> callback)
        {
            try
            {
                AppData.CallbackDataList<AppData.ProjectStructureData> callbackResults = new AppData.CallbackDataList<AppData.ProjectStructureData>();

                LoadRootStructureData(rootProjectCallbackResults => 
                {
                    callbackResults.results = rootProjectCallbackResults.results;
                    callbackResults.resultsCode = rootProjectCallbackResults.resultsCode;

                    if(callbackResults.Success())
                    {
                        if (GetAppDirectoryData(rootProjectCallbackResults.data.GetProjectStructureData().rootFolder.directoryType).Success())
                        {
                            AppData.StorageDirectoryData directoryData = GetAppDirectoryData(rootProjectCallbackResults.data.GetProjectStructureData().rootFolder.directoryType).data;

                            if (DirectoryFound(directoryData))
                            {
                                var projectFiles = Directory.GetFileSystemEntries(directoryData.projectDirectory);

                                if (projectFiles != null && projectFiles.Length > 0)
                                {
                                    List<AppData.StorageDirectoryData> validEntries = new List<AppData.StorageDirectoryData>();

                                    foreach (var item in projectFiles)
                                    {
                                        if (item.Contains(".json") && !item.Contains(".meta"))
                                        {
                                            AppData.StorageDirectoryData validEntry = new AppData.StorageDirectoryData
                                            {
                                                name = Path.GetFileName(item).Replace(".json", ""),
                                                path = item,
                                                projectDirectory = directoryData.projectDirectory
                                            };

                                            validEntries.Add(validEntry);
                                        }
                                    }

                                    if (validEntries.Count > 0)
                                    {
                                        List<AppData.ProjectStructureData> loadedEntries = new List<AppData.ProjectStructureData>();

                                        foreach (var entry in validEntries)
                                        {
                                            LoadData<AppData.ProjectStructureData>(entry, loadedResults =>
                                            {
                                                callbackResults.results = loadedResults.results;
                                                callbackResults.resultsCode = loadedResults.resultsCode;

                                                if (callbackResults.Success())
                                                    loadedEntries.Add(loadedResults.data);
                                            });

                                            if (!callbackResults.Success())
                                                break;
                                        }

                                        LogSuccess($"===========> Found : {loadedEntries.Count} Loaded Entries.", this);

                                        if (loadedEntries.Count > 0)
                                        {
                                            callbackResults.results = $"Directory : {directoryData.projectDirectory} Found.";
                                            callbackResults.data = loadedEntries;
                                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                        }
                                        else
                                        {

                                            callbackResults.results = $" Failed To Load Project Structure From Directory : {directoryData.projectDirectory} - Please Check Here For Details.";
                                            callbackResults.data = default;
                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                        }
                                    }
                                    else
                                    {
                                        callbackResults.results = $" There Are No Valid Project Data Files Found In Directory : {directoryData.projectDirectory}.";
                                        callbackResults.data = default;
                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                    }
                                }
                                else
                                {
                                    callbackResults.results = $" There Are No Valid Project Data Files Found In Directory : {directoryData.projectDirectory}.";
                                    callbackResults.data = default;
                                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                }
                            }
                            else
                            {
                                callbackResults.results = $"Directory : {directoryData.projectDirectory} Of Type : {rootStructureStorageData.type} Not Found.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        }
                        else
                            Log(GetAppDirectoryData(rootProjectCallbackResults.data.GetProjectStructureData().rootFolder.directoryType).resultsCode, GetAppDirectoryData(rootProjectCallbackResults.data.GetProjectStructureData().rootFolder.directoryType).results, this);
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

        public void OnMoveToDirectory(AppData.StorageDirectoryData sourceStorageData, AppData.StorageDirectoryData targetStorageData, AppData.SelectableWidgetType type, Action<AppData.CallbackData<AppData.DirectoryInfo>> callback = null)
        {
            try
            {
                AppData.CallbackData<AppData.DirectoryInfo> callbackResults = new AppData.CallbackData<AppData.DirectoryInfo>();

                if (type == AppData.SelectableWidgetType.Asset)
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

                if (type == AppData.SelectableWidgetType.Folder)
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

                    DirectoryFound(sourceStorageData.projectDirectory, checkDirectoryCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(checkDirectoryCallback.resultsCode))
                        {
                            if (!Directory.Exists(targetStorageData.projectDirectory))
                            {
                                MoveDirectory(sourceStorageData.projectDirectory, targetStorageData.projectDirectory, checkDirectoryMoveCallback =>
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
                                            projectDirectory = targetStorageData.projectDirectory
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
                                callbackResults.results = $"Directory : {sourceStorageData.projectDirectory} Already Exists In : {targetStorageData.name}";
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
                string targetDirectory = Path.Combine(targetStorageData.projectDirectory, sourceFileName);
                string formattedDirectory = targetDirectory.Replace("\\", "/");

                File.Move(sourceStorageData.projectDirectory, formattedDirectory);

                FileFound(formattedDirectory, fileCheckCallback =>
                {
                    callbackResults.results = fileCheckCallback.results;
                    callbackResults.resultsCode = fileCheckCallback.resultsCode;

                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                        callbackResults.data = new AppData.StorageDirectoryData
                        {
                            name = sourceStorageData.name,
                            projectDirectory = formattedDirectory
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

        public void MoveFile(string sourceDirectory, string targetDirectory, AppData.SelectableWidgetType assetType, Action<AppData.CallbackData<AppData.StorageDirectoryData>> callback)
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
                        projectDirectory = fileDataDirectory
                    };

                #region File Data Update

                if (assetType == AppData.SelectableWidgetType.Asset)
                    {
                        LoadData<AppData.AssetData>(newStorageData, fileLoaderCallback =>
                        {
                            callbackResults.results = fileLoaderCallback.results;
                            callbackResults.resultsCode = fileLoaderCallback.resultsCode;

                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                            {
                                AppData.AssetData loadedFileData = fileLoaderCallback.data;
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
                                        callbackResults.results = $"Couldn't Save File Data : {fileLoaderCallback.data.name} At Directory : {newStorageData.projectDirectory}";
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

                if (assetType == AppData.SelectableWidgetType.Folder)
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
                                        callbackResults.results = $"Couldn't Save Folder Data : {fileLoaderCallback.data.name} At Directory : {newStorageData.projectDirectory}";
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

            Debug.LogError($"==> Moving Directory From : {sourceStorageData.projectDirectory} To : {targetStorageData.projectDirectory}");

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
                            string formattedFolderName = GetFormattedName(formattedName, AppData.SelectableWidgetType.Folder, true);
                            string formattedDirectory = AppData.Helpers.GetFormattedDirectoryPath(Path.GetDirectoryName(formattedPath));
                            string newDirectory = Path.Combine(formattedDirectory, formattedFolderName);
                            string newFormattedDirectory = AppData.Helpers.GetFormattedDirectoryPath(newDirectory);

                            AppData.SelectableWidgetType assetType = GetAssetTypeFromAssetDataName(Path.GetFileNameWithoutExtension(formattedPath));

                            AppData.StorageDirectoryData storageData = new AppData.StorageDirectoryData
                            {
                                name = formattedName,
                                path = formattedPath,
                                projectDirectory = newFormattedDirectory,
                            };

                            if (assetType == AppData.SelectableWidgetType.Asset)
                            {
                                if (!formattedFileSorageDataList.Contains(storageData))
                                    formattedFileSorageDataList.Add(storageData);
                            }

                            if (assetType == AppData.SelectableWidgetType.Folder)
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
                                LoadData<AppData.AssetData>(fileSorageData, (fileLoaderCallback) =>
                                {
                                    callbackResults.results = fileLoaderCallback.results;
                                    callbackResults.resultsCode = fileLoaderCallback.resultsCode;

                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                    {
                                        AppData.AssetData loadedFileData = fileLoaderCallback.data;
                                        loadedFileData.storageData.path = fileSorageData.path;
                                        loadedFileData.storageData.projectDirectory = fileSorageData.projectDirectory;

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
                                                callbackResults.results = $"Couldn't Save Asset File Data : {fileLoaderCallback.data.name} At Directory : {fileSorageData.projectDirectory}";
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
                                        loadedFoldereData.storageData.projectDirectory = folderStorageData.projectDirectory;

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
                                                callbackResults.results = $"Couldn't Save Folder Data : {fileLoaderCallback.data.name} At Directory : {folderStorageData.projectDirectory}";
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
                switch (asset.GetSelectableWidgetType())
                {
                    case AppData.SelectableWidgetType.Asset:

                        AppData.SceneAsset assetToDelete = asset.GetAssetData();

                        Delete(assetToDelete, assetDeletedCallback =>
                        {
                            if (AppData.Helpers.IsSuccessCode(assetDeletedCallback.resultsCode))
                                assetsToDeleteCount--;
                            else
                                Debug.LogWarning($"--> Delete Failed With Results : {assetDeletedCallback.results}");
                        });

                        break;

                    case AppData.SelectableWidgetType.Folder:

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

            Debug.LogError($"==> Deleting Folder Directory : {folder.storageData} : Folder Content Path : {folder.storageData.projectDirectory}");

            if (File.Exists(folder.storageData.projectDirectory))
                File.Delete(folder.storageData.projectDirectory);

            if (Directory.Exists(folder.storageData.projectDirectory))
                Directory.Delete(folder.storageData.projectDirectory, true);

            if (!File.Exists(folder.storageData.projectDirectory) && !Directory.Exists(folder.storageData.projectDirectory))
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

            if (File.Exists(asset.storageData.projectDirectory))
                File.Delete(asset.storageData.projectDirectory);

            if (!File.Exists(asset.storageData.projectDirectory))
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

        public AppData.SelectableWidgetType GetAssetTypeFromAssetDataName(string assetName)
        {
            if (assetName.Contains("File"))
                return AppData.SelectableWidgetType.Asset;

            if (assetName.Contains("Folder"))
                return AppData.SelectableWidgetType.Folder;

            return AppData.SelectableWidgetType.PlaceHolder;
        }

        public bool HasSelectedAssets()
        {
            return selectedSceneAssetList.Count > 0;
        }

        public void GetFilteredWidgetList<T>(T data, Action<AppData.CallbackDataList<T>> callback) where T : AppData.SerializableData
        {
            AppData.CallbackDataList<T> callbackResults = new AppData.CallbackDataList<T>();

            callback?.Invoke(callbackResults);
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
            try
            {
                AppData.CallbackData<List<string>> callbackResults = new AppData.CallbackData<List<string>>();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    if (GetWidgetsRefreshData().widgetsContainer && GetWidgetsRefreshData().widgetsContainer.IsContainerActive())
                    {
                        switch (ScreenUIManager.Instance.GetCurrentUIScreenType())
                        {
                            case AppData.UIScreenType.ProjectSelectionScreen:

                                #region Serach For Projects Files

                                if (GetProjectRootStructureData().Success())
                                {
                                    if (GetAppDirectoryData(GetProjectRootStructureData().data.GetProjectStructureData().rootFolder.directoryType).Success())
                                    {
                                        var searchDirectory = GetAppDirectoryData(GetProjectRootStructureData().data.GetProjectStructureData().rootFolder.directoryType).data;

                                        if (DirectoryFound(searchDirectory))
                                        {
                                            GetWidgetsRefreshData().widgetsContainer.ClearWidgets();

                                            var searchedProjects = Directory.GetFileSystemEntries(searchDirectory.projectDirectory, "*.json", SearchOption.TopDirectoryOnly);

                                            #region Get System Files

                                            List<string> validProjectsfound = new List<string>();
                                            List<string> projectsDataBlackList = new List<string>();

                                            bool projectsFound = false;

                                            foreach (var searchedProject in searchedProjects)
                                            {
                                                if (GetFolderStructureData().GetExcludedSystemFileData() != null)
                                                {
                                                    foreach (var excludedFile in GetProjectRootStructureData().data.GetProjectStructureData().GetExcludedSystemFileData())
                                                    {
                                                        if (!searchedProject.Contains(excludedFile) && !projectsDataBlackList.Contains(searchedProject))
                                                        {
                                                            if (!validProjectsfound.Contains(searchedProject))
                                                                validProjectsfound.Add(searchedProject);
                                                        }
                                                        else
                                                            projectsDataBlackList.Add(searchedProject);
                                                    }
                                                }
                                                else
                                                    Debug.LogWarning($"==> LoadFolderData's GetExcludedSystemFolders Failed - GetFolderStructureData().GetExcludedSystemFileData() Returned Null.");
                                            }

                                            #endregion

                                            #region Projects

                                            if (validProjectsfound.Count > 0)
                                            {
                                                List<AppData.ProjectStructureData> validProjectsfoundDirectories = new List<AppData.ProjectStructureData>();
                                                List<AppData.ProjectStructureData> projectsSearchResults = new List<AppData.ProjectStructureData>();

                                                foreach (var validProject in validProjectsfound)
                                                {
                                                    var fileName = Path.GetFileName(validProject);

                                                    AppData.StorageDirectoryData directoryData = new AppData.StorageDirectoryData
                                                    {
                                                        name = fileName,
                                                        path = validProject,
                                                        projectDirectory = searchDirectory.projectDirectory,
                                                        type = searchDirectory.type
                                                    };

                                                    LoadData<AppData.ProjectStructureData>(directoryData, loadedProjectCallbackResults =>
                                                    {
                                                        if (loadedProjectCallbackResults.Success())
                                                            validProjectsfoundDirectories.Add(loadedProjectCallbackResults.data);
                                                        else
                                                            LogError($"====> Project Data Failed To Load : {fileName} From Path : {directoryData.path} In Directory Directory : {directoryData.projectDirectory} With Results : {loadedProjectCallbackResults.results}", this);
                                                    });
                                                }

                                                if (validProjectsfoundDirectories.Count > 0)
                                                {
                                                    #region Project Search Filter

                                                    foreach (var validDirectory in validProjectsfoundDirectories)
                                                    {
                                                        string folderName = validDirectory.name.ToLower();

                                                        if (strictValidateAssetSearch)
                                                        {
                                                            if (folderName.Contains(searchValue.ToLower()) && folderName.StartsWith(searchValue[0].ToString().ToLower()))
                                                            {
                                                                if (!projectsSearchResults.Contains(validDirectory))
                                                                    projectsSearchResults.Add(validDirectory);
                                                            }
                                                            else
                                                            {
                                                                if (projectsSearchResults.Contains(validDirectory))
                                                                    projectsSearchResults.Remove(validDirectory);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (folderName.Contains(searchValue.ToLower()))
                                                            {
                                                                if (!projectsSearchResults.Contains(validDirectory))
                                                                    projectsSearchResults.Add(validDirectory);
                                                            }
                                                            else
                                                            {
                                                                if (projectsSearchResults.Contains(validDirectory))
                                                                    projectsSearchResults.Remove(validDirectory);
                                                            }
                                                        }
                                                    }

                                                    #endregion

                                                    #region Create Project Widgets

                                                    if (projectsSearchResults.Count > 0)
                                                    {
                                                        CreateUIScreenProjectSelectionWidgets(ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType(), projectsSearchResults, GetWidgetsRefreshData().widgetsContainer, (widgetsCreated) =>
                                                        {
                                                            projectsFound = widgetsCreated.resultsCode == AppData.Helpers.SuccessCode;
                                                        });
                                                    }

                                                    #endregion
                                                }
                                            }

                                            #endregion

                                            #region No Results Found

                                            if (ScreenUIManager.Instance)
                                            {

                                                LogSuccess($"==========> Search Results Found : {projectsFound}", this);

                                                if (!projectsFound)
                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetUITextDisplayerValue(AppData.ScreenTextType.ResultsNotFound, $"No Search Results Found For {searchValue}");
                                                else
                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetUITextDisplayerValue(AppData.ScreenTextType.ResultsNotFound, "");

                                                ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputDropDownActionType.SortingList, AppData.InputUIState.Disabled);
                                                ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputDropDownActionType.FilterList, AppData.InputUIState.Disabled);

                                                ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewProjectButton, AppData.InputUIState.Disabled);
                                                ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.OpenProjectFolderButton, AppData.InputUIState.Disabled);
                                                ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Disabled);
                                            }
                                            else
                                                Debug.LogWarning("--> Screen Manager Not Yet Initialized.");

                                            #endregion

                                        }
                                        else
                                            LogError($"Directory Of Type {rootProjectStructureData.GetProjectStructureData().rootFolder.directoryType} Not Found ", this);
                                    }
                                    else
                                        Log(GetAppDirectoryData(GetProjectRootStructureData().data.GetProjectStructureData().rootFolder.directoryType).resultsCode, GetAppDirectoryData(GetProjectRootStructureData().data.GetProjectStructureData().rootFolder.directoryType).results, this);
                                }
                                else
                                    Log(GetProjectRootStructureData().resultsCode, GetProjectRootStructureData().results, this);

                                #endregion

                                break;

                            case AppData.UIScreenType.ProjectViewScreen:

                                #region Search For Files And Folders

                                var searchFolder = GetCurrentFolder();

                                if (!string.IsNullOrEmpty(searchFolder.storageData.projectDirectory))
                                {
                                    DirectoryFound(searchFolder.storageData.projectDirectory, foundDirectoriesCallback =>
                                    {
                                        GetWidgetsRefreshData().widgetsContainer.ClearWidgets();

                                        if (foundDirectoriesCallback.Success())
                                        {
                                            var searchedItems = Directory.GetFileSystemEntries(searchFolder.storageData.projectDirectory, "*.json", SearchOption.AllDirectories);

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
                                                            projectDirectory = validFolder,
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
                                                            projectDirectory = validFileDirectory,
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

                                                                Debug.LogError($"==> Found File : {file.name} : Directory : {file.projectDirectory}");
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
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetUITextDisplayerValue(AppData.ScreenTextType.ResultsNotFound, $"No Results Found For : {searchValue}");
                                                    else
                                                        ScreenUIManager.Instance.GetCurrentScreenData().value.SetUITextDisplayerValue(AppData.ScreenTextType.ResultsNotFound, "");

                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputDropDownActionType.SortingList, AppData.InputUIState.Enabled);
                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputDropDownActionType.FilterList, AppData.InputUIState.Enabled);

                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.InputUIState.Disabled);
                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Disabled);
                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewFolderButton, AppData.InputUIState.Disabled);
                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewAsset, AppData.InputUIState.Disabled);
                                                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ClipboardButton, AppData.InputUIState.Disabled);
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

                                break;
                        }
                    }
                }
                else
                {
                    switch (ScreenUIManager.Instance.GetCurrentUIScreenType())
                    {
                        case AppData.UIScreenType.ProjectSelectionScreen:

                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewProjectButton, AppData.InputUIState.Enabled);
                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.OpenProjectFolderButton, AppData.InputUIState.Enabled);
                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Enabled);

                            break;

                        case AppData.UIScreenType.ProjectViewScreen:

                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.InputUIState.Enabled);
                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.PaginationButton, AppData.InputUIState.Enabled);

                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewFolderButton, AppData.InputUIState.Enabled);
                            ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewAsset, AppData.InputUIState.Enabled);

                            break;
                    }

                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputDropDownActionType.SortingList, AppData.InputUIState.Enabled);
                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputDropDownActionType.FilterList, AppData.InputUIState.Enabled);

                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowLoadingItem(AppData.LoadingItemType.Spinner, false);
                    ScreenUIManager.Instance.Refresh();
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

        #region Sort Functions

        public void OnSetFilterAndSortActionEvent(AppData.InputDropDownActionType actionType, int dropDownIndex)
        {
            if (ScreenUIManager.Instance != null)
            {
                GetContentContainer(widgetContainerCallbackResults =>
                {
                    if (widgetContainerCallbackResults.Success())
                    {
                        switch (actionType)
                        {
                            case AppData.InputDropDownActionType.FilterList:

                                OnFilterScreenWidgets(dropDownIndex, widgetContainerCallbackResults.data, filteredProjectCallbackResults =>
                                {
                                    Log(filteredProjectCallbackResults.resultsCode, filteredProjectCallbackResults.results, this);
                                });

                                break;

                            case AppData.InputDropDownActionType.SortingList:

                                OnSortScreenWidgets(dropDownIndex, assetSortedCallbackResults =>
                                {
                                    if (assetSortedCallbackResults.Success())
                                    {
                                        if (GetProjectRootStructureData().Success())
                                        {
                                            var rootStructureData = GetProjectRootStructureData().data;
                                            rootStructureData.rootProjectStructure.GetProjectInfo().SetSortType(assetSortedCallbackResults.data);

                                            SaveModifiedData(rootStructureData, dataSavedCallbackResults =>
                                            {
                                                Log(dataSavedCallbackResults.resultsCode, dataSavedCallbackResults.results, this);
                                            });
                                        }
                                        else
                                            Log(GetProjectRootStructureData().resultsCode, GetProjectRootStructureData().results, this);
                                    }
                                    else
                                        Log(assetSortedCallbackResults.resultsCode, assetSortedCallbackResults.results, this);
                                });

                                break;
                        }
                    }
                    else
                        Log(widgetContainerCallbackResults.resultsCode, widgetContainerCallbackResults.results, this);
                });
            }
            else
                LogError("Screen UI Manager Instance Is Not Yet Initialized.");
        }

        public void OnSortScreenWidgets(int sortIndex, Action<AppData.CallbackData<AppData.SortType>> callback)
        {
            try
            {
                AppData.CallbackData<AppData.SortType> callbackResults = new AppData.CallbackData<AppData.SortType>();

                GetDropdownContentTypeFromIndex<AppData.SortType>(sortIndex, enumCallbackResults =>
                {
                    callbackResults.results = enumCallbackResults.results;
                    callbackResults.resultsCode = enumCallbackResults.resultsCode;

                    if (callbackResults.Success())
                    {
                        GetContentContainer(containerCallbackResults => 
                        {
                            callbackResults.results = containerCallbackResults.results;
                            callbackResults.resultsCode = containerCallbackResults.resultsCode;

                            if (callbackResults.Success())
                            {
                                var sortType = (AppData.SortType)enumCallbackResults.data;

                                containerCallbackResults.data.GetContent(contentCallbackResults => 
                                {
                                    callbackResults.results = contentCallbackResults.results;
                                    callbackResults.resultsCode = contentCallbackResults.resultsCode;

                                    if (callbackResults.Success())
                                    {
                                        switch (sortType)
                                        {
                                            case AppData.SortType.Ascending:

                                                contentCallbackResults.data.Sort((firstWidget, secondWidget) => firstWidget.name.CompareTo(secondWidget.name));

                                                break;

                                            case AppData.SortType.Category:

                                                containerCallbackResults.data.GetSelectableWidgetType(contentCallbackResults.data, selectableWidgetTypeCallbackResults => 
                                                {
                                                    callbackResults.results = selectableWidgetTypeCallbackResults.results;
                                                    callbackResults.resultsCode = selectableWidgetTypeCallbackResults.resultsCode;

                                                    if(callbackResults.Success())
                                                    {
                                                        switch (selectableWidgetTypeCallbackResults.data)
                                                        {
                                                            case AppData.SelectableWidgetType.Project:

                                                                contentCallbackResults.data.Sort((firstWidget, secondWidget) => firstWidget.GetData<AppData.ProjectStructureData>(selectableWidgetTypeCallbackResults.data).projectInfo.GetCategoryType().CompareTo(secondWidget.GetData<AppData.ProjectStructureData>(selectableWidgetTypeCallbackResults.data).projectInfo.GetCategoryType()));

                                                                break;

                                                            case AppData.SelectableWidgetType.Folder:

                                                                contentCallbackResults.data.Sort((firstWidget, secondWidget) => firstWidget.GetData<AppData.Folder>(selectableWidgetTypeCallbackResults.data).GetCategoryType().CompareTo(secondWidget.GetData<AppData.Folder>(selectableWidgetTypeCallbackResults.data).GetCategoryType()));

                                                                break;

                                                            case AppData.SelectableWidgetType.Asset:

                                                                contentCallbackResults.data.Sort((firstWidget, secondWidget) => firstWidget.GetData<AppData.AssetData>(selectableWidgetTypeCallbackResults.data).GetCategoryType().CompareTo(secondWidget.GetData<AppData.AssetData>(selectableWidgetTypeCallbackResults.data).GetCategoryType()));

                                                                break;
                                                        }
                                                    }
                                                });

                                                break;

                                            case AppData.SortType.Descending:

                                                contentCallbackResults.data.Sort((firstWidget, secondWidget) => secondWidget.name.CompareTo(firstWidget.name));

                                                break;

                                            case AppData.SortType.DateModified:

                                                contentCallbackResults.data.Sort((firstWidget, secondWidget) => secondWidget.GetData<AppData.SerializableData>(secondWidget.GetSelectableWidgetType()).GetModifiedDateTime().CompareTo(firstWidget.GetData<AppData.SerializableData>(firstWidget.GetSelectableWidgetType()).GetModifiedDateTime()));

                                                break;
                                        }

                                        if (contentCallbackResults.data.Count > 0)
                                        {
                                            for (int i = 0; i < contentCallbackResults.data.Count; i++)
                                            {
                                                containerCallbackResults.data.SetWidgetListIndex(contentCallbackResults.data[i], i, setIndexCallbackResults => 
                                                {
                                                    callbackResults.results = setIndexCallbackResults.results;
                                                    callbackResults.resultsCode = setIndexCallbackResults.resultsCode;

                                                });

                                                if (callbackResults.Success())
                                                    callbackResults.data = sortType;
                                                else
                                                {
                                                    callbackResults.data = default;
                                                    break;
                                                }
                                            }

                                            if(callbackResults.Success())
                                                callbackResults.results = $"{contentCallbackResults.data.Count } : Assets Sorted To : {sortType}.";
                                        }
                                        else
                                        {
                                            callbackResults.results = "Screen Widget List Is Null / Not Initialized.";
                                            callbackResults.data = default;
                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                        }
                                    }
                                });
                            }
                        });
                    }
                });

                callback.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void GetSortedWidgetList<T>(List<T> serializableDataList, List<T> pinnedList, Action<AppData.CallbackDataList<T>> callback) where T : AppData.SerializableData
        {
            AppData.CallbackDataList<T> callbackResults = new AppData.CallbackDataList<T>();

            if (serializableDataList != null)
            {
                GetCurrentProjectStructureData().GetProjectInfo().GetSortType(sortCallBackResults =>
                {
                    callbackResults.results = sortCallBackResults.results;
                    callbackResults.resultsCode = sortCallBackResults.resultsCode;

                    if(callbackResults.Success())
                    {
                        switch (sortCallBackResults.data)
                        {
                            case AppData.SortType.Ascending:

                                serializableDataList.Sort((firstWidget, secondWidget) => firstWidget.name.CompareTo(secondWidget.name));

                                break;

                            case AppData.SortType.Category:

                                //serializableDataList.Sort((firstWidget, secondWidget) => firstWidget.categoryType.CompareTo(secondWidget.categoryType));

                                break;


                            case AppData.SortType.Descending:

                                serializableDataList.Sort((firstWidget, secondWidget) => secondWidget.name.CompareTo(firstWidget.name));

                                break;

                            case AppData.SortType.DateModified:

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
                });
            }
            else
            {
                callbackResults.results = "GetSortedWidgetList Failed : serializableDataList Is Null / Empty.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void GetSortedProjectWidgetList<T>(List<T> serializableDataList, Action<AppData.CallbackDataList<T>> callback) where T : AppData.ProjectStructureData
        {
            try
            {
                AppData.CallbackDataList<T> callbackResults = new AppData.CallbackDataList<T>();

                AppData.Helpers.SerializableComponentValid<T>(serializableDataList, validDataCallbackResults =>
                {
                    callbackResults.results = validDataCallbackResults.results;
                    callbackResults.resultsCode = validDataCallbackResults.resultsCode;

                    if (callbackResults.Success())
                    {
                        GetProjectStructure(projectStructureCallbackResults =>
                        {
                            callbackResults.results = projectStructureCallbackResults.results;
                            callbackResults.resultsCode = projectStructureCallbackResults.resultsCode;

                            if (callbackResults.Success())
                            {
                                projectStructureCallbackResults.data.GetProjectInfo(projectInfoCallbackResults => 
                                {
                                    callbackResults.results = projectInfoCallbackResults.results;
                                    callbackResults.resultsCode = projectInfoCallbackResults.resultsCode;

                                    if (callbackResults.Success())
                                    {
                                        projectStructureCallbackResults.data.GetProjectInfo().GetSortType(sortCallbackResults =>
                                        {
                                            callbackResults.results = sortCallbackResults.results;
                                            callbackResults.resultsCode = sortCallbackResults.resultsCode;

                                            if (callbackResults.Success())
                                            {
                                                switch (sortCallbackResults.data)
                                                {
                                                    case AppData.SortType.Ascending:

                                                        serializableDataList.Sort((firstWidget, secondWidget) => firstWidget.name.CompareTo(secondWidget.name));

                                                        break;

                                                    case AppData.SortType.Category:

                                                        serializableDataList.Sort((firstWidget, secondWidget) => firstWidget.GetProjectInfo().GetCategoryType().CompareTo(secondWidget.GetProjectInfo().GetCategoryType()));

                                                        break;


                                                    case AppData.SortType.Descending:

                                                        serializableDataList.Sort((firstWidget, secondWidget) => secondWidget.name.CompareTo(firstWidget.name));

                                                        break;

                                                    case AppData.SortType.DateModified:

                                                        serializableDataList.Sort((firstWidget, secondWidget) => secondWidget.GetCreationDateTime().GetDateTime().CompareTo(firstWidget.GetCreationDateTime().GetDateTime()));

                                                        break;
                                                }

                                                if (serializableDataList != null && serializableDataList.Count > 0)
                                                {
                                                    callbackResults.results = "GetSortedWidgetList Success";
                                                    callbackResults.data = serializableDataList;
                                                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                                }
                                                else
                                                {
                                                    callbackResults.results = "Something Sinister Happaned. What's Going On Here. Huh!";
                                                    callbackResults.data = default;
                                                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                }
                                            }
                                            else
                                                callbackResults.data = default;
                                        });
                                    }
                                });
                            }
                            else
                                callbackResults.data = default;
                        });
                    }
                    else
                        callbackResults.data = default;
                });

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public void SortScreenWidgets(List<AppData.UIScreenWidget> widgets, Action<AppData.CallbackDataList<AppData.SceneAssetWidget>> callback = null)
        {
            try
            {
                AppData.CallbackDataList<AppData.SceneAssetWidget> callbackResults = new AppData.CallbackDataList<AppData.SceneAssetWidget>();

                GetProjectStructure(folderStructureCallbackResults =>
                {
                    callbackResults.results = folderStructureCallbackResults.results;
                    callbackResults.resultsCode = folderStructureCallbackResults.resultsCode;

                    if (callbackResults.Success())
                    {
                        folderStructureCallbackResults.data.GetProjectInfo().GetSortType(sortTypeCallbackResults => 
                        {
                            callbackResults.results = sortTypeCallbackResults.results;
                            callbackResults.resultsCode = sortTypeCallbackResults.resultsCode;

                            if (callbackResults.Success())
                            {
                                switch (sortTypeCallbackResults.data)
                                {
                                    case AppData.SortType.Ascending:

                                        screenWidgetList.Sort((firstWidget, secondWidget) => firstWidget.name.CompareTo(secondWidget.name));

                                        break;

                                    case AppData.SortType.Category:

                                        screenWidgetList.Sort((firstWidget, secondWidget) => firstWidget.categoryType.CompareTo(secondWidget.categoryType));

                                        break;

                                    case AppData.SortType.Descending:

                                        screenWidgetList.Sort((firstWidget, secondWidget) => secondWidget.name.CompareTo(firstWidget.name));

                                        break;

                                    case AppData.SortType.DateModified:

                                        screenWidgetList.Sort((firstWidget, secondWidget) => secondWidget.GetModifiedDateTime().CompareTo(firstWidget.GetModifiedDateTime()));

                                        break;
                                }

                                callbackResults.results = $"Sorted Widgets Using : {sortTypeCallbackResults.data} Sort Type.";
                                callbackResults.data = screenWidgetList;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                        });
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

        public void GetSortedWidgetsFromList(List<AppData.UIScreenWidget> widgets, AppData.SelectableWidgetType assetType, Action<AppData.CallbackData<List<AppData.UIScreenWidget>>> callback)
        {
            AppData.CallbackData<List<AppData.UIScreenWidget>> callbackResults = new AppData.CallbackData<List<AppData.UIScreenWidget>>();

            AppData.Helpers.ComponentValid(widgets, componentCallbackResults =>
            {
                callbackResults.results = componentCallbackResults.results;
                callbackResults.resultsCode = componentCallbackResults.resultsCode;

                if (callbackResults.Success())
                {
                    GetProjectStructure(folderStructureCallbackResults =>
                    {
                        callbackResults.results = folderStructureCallbackResults.results;
                        callbackResults.resultsCode = folderStructureCallbackResults.resultsCode;

                        if (callbackResults.Success())
                        {
                            folderStructureCallbackResults.data.GetProjectInfo().GetSortType(sortedCallbackResults =>
                            {
                                callbackResults.results = sortedCallbackResults.results;
                                callbackResults.resultsCode = sortedCallbackResults.resultsCode;

                                if (callbackResults.Success())
                                {
                                    switch (sortedCallbackResults.data)
                                    {
                                        case AppData.SortType.Ascending:

                                            widgets.Sort((firstWidget, secondWidget) => firstWidget.name.CompareTo(secondWidget.name));

                                            break;

                                        case AppData.SortType.Category:

                                            switch (assetType)
                                            {
                                                case AppData.SelectableWidgetType.Project:

                                                    widgets.Sort((firstWidget, secondWidget) => firstWidget.GetData<AppData.ProjectStructureData>(assetType).projectInfo.GetCategoryType().CompareTo(secondWidget.GetData<AppData.ProjectStructureData>(assetType).projectInfo.GetCategoryType()));

                                                    break;

                                                case AppData.SelectableWidgetType.Folder:

                                                    widgets.Sort((firstWidget, secondWidget) => firstWidget.GetData<AppData.Folder>(assetType).GetCategoryType().CompareTo(secondWidget.GetData<AppData.Folder>(assetType).GetCategoryType()));

                                                    break;

                                                case AppData.SelectableWidgetType.Asset:

                                                    widgets.Sort((firstWidget, secondWidget) => firstWidget.GetData<AppData.AssetData>(assetType).GetCategoryType().CompareTo(secondWidget.GetData<AppData.AssetData>(assetType).GetCategoryType()));

                                                    break;
                                            }

                                            break;

                                        case AppData.SortType.Descending:

                                            widgets.Sort((firstWidget, secondWidget) => secondWidget.name.CompareTo(firstWidget.name));

                                            break;

                                        case AppData.SortType.DateModified:

                                            switch (assetType)
                                            {
                                                case AppData.SelectableWidgetType.Project:

                                                    widgets.Sort((firstWidget, secondWidget) => secondWidget.GetData<AppData.ProjectStructureData>(assetType).GetModifiedDateTime().CompareTo(firstWidget.GetData<AppData.ProjectStructureData>(assetType).GetModifiedDateTime()));

                                                    break;

                                                case AppData.SelectableWidgetType.Folder:

                                                    widgets.Sort((firstWidget, secondWidget) => secondWidget.GetData<AppData.Folder>(assetType).GetModifiedDateTime().CompareTo(firstWidget.GetData<AppData.Folder>(assetType).GetModifiedDateTime()));

                                                    break;

                                                case AppData.SelectableWidgetType.Asset:

                                                    widgets.Sort((firstWidget, secondWidget) => secondWidget.GetData<AppData.AssetData>(assetType).GetModifiedDateTime().CompareTo(firstWidget.GetData<AppData.AssetData>(assetType).GetModifiedDateTime()));

                                                    break;
                                            }

                                            break;
                                    }

                                    callbackResults.results = "Widgets Sorted Successfully.";
                                    callbackResults.data = widgets;
                                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                }
                            });
                        }
                    });
                }
            });

            callback.Invoke(callbackResults);
        }

        #endregion

        #region Filtering

        public void OnFilterScreenWidgets(int filterIndex, DynamicWidgetsContainer container, Action<AppData.CallbackData<Enum>> callback = null)
        {
            try
            {
                AppData.CallbackData<Enum> callbackResults = new AppData.CallbackData<Enum>();

                AppData.Helpers.ComponentValid(ScreenUIManager.Instance, hasScreenManagerCallbackResults =>
                {
                    callbackResults.resultsCode = hasScreenManagerCallbackResults.resultsCode;

                    if (callbackResults.Success())
                    {
                        ScreenUIManager.Instance.GetCurrentScreen(currentScreenCallbackResults => 
                        {
                            callbackResults.results = currentScreenCallbackResults.results;
                            callbackResults.resultsCode = currentScreenCallbackResults.resultsCode;

                            if(callbackResults.Success())
                            {
                                switch (currentScreenCallbackResults.data.value.GetUIScreenType())
                                {
                                    case AppData.UIScreenType.ProjectSelectionScreen:

                                        GetDropdownContentTypeFromIndex<AppData.ProjectCategoryType>(filterIndex, enumCallbackResults =>
                                        {
                                            if (enumCallbackResults.Success())
                                            {
                                                var filterType = (AppData.ProjectCategoryType)enumCallbackResults.data;

                                                if (filterType != AppData.ProjectCategoryType.Project_All)
                                                {
                                                    if (GetAppDirectoryData(rootProjectStructureData.GetProjectStructureData().rootFolder.directoryType).Success())
                                                    {
                                                        var filterDirectory = GetAppDirectoryData(rootProjectStructureData.GetProjectStructureData().rootFolder.directoryType).data;

                                                        if (DirectoryFound(filterDirectory))
                                                        {
                                                            var filteredProjectFiles = Directory.GetFileSystemEntries(filterDirectory.projectDirectory, "*.json", SearchOption.TopDirectoryOnly);

                                                            AppData.Helpers.StringArrayValueValid(filteredProjectFiles, valueIsValidCallbackResults =>
                                                            {
                                                                callbackResults.resultsCode = valueIsValidCallbackResults.resultsCode;

                                                                if (callbackResults.Success())
                                                                {
                                                                    container.ClearWidgets(false, widgetsClearedCallbackResults =>
                                                                    {
                                                                        #region Filter Content 

                                                                        if (widgetsClearedCallbackResults.Success())
                                                                        {
                                                                            #region Get System Files

                                                                            List<string> validProjectsfound = new List<string>();

                                                                            List<string> projectsDataBlackList = new List<string>();

                                                                            bool projectsFound = false;

                                                                            foreach (var validData in filteredProjectFiles)
                                                                            {
                                                                                if (GetFolderStructureData().GetExcludedSystemFileData() != null)
                                                                                {
                                                                                    foreach (var excludedFile in rootProjectStructureData.GetProjectStructureData().GetExcludedSystemFileData())
                                                                                    {
                                                                                        if (!validData.Contains(excludedFile) && !projectsDataBlackList.Contains(validData))
                                                                                        {
                                                                                            if (!validProjectsfound.Contains(validData))
                                                                                                validProjectsfound.Add(validData);
                                                                                            else
                                                                                            {
                                                                                                callbackResults.results = $"Found Valid Project Data Contains Excluded File : {excludedFile} Or Project Data Contains Already Contains Project : {validData}";
                                                                                                callbackResults.data = default;
                                                                                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                                                                                                break;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                            projectsDataBlackList.Add(validData);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    callbackResults.results = "Couldn't Get Excluded File Data.";
                                                                                    callbackResults.data = default;
                                                                                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                                                }
                                                                            }

                                                                            AppData.Helpers.StringListValueValid(validProjectsfound, hasValidDataCallbackResults =>
                                                                            {
                                                                                callbackResults.resultsCode = hasValidDataCallbackResults.resultsCode;

                                                                                if (callbackResults.Success())
                                                                                {
                                                                                    List<AppData.ProjectStructureData> validProjectsfoundDirectories = new List<AppData.ProjectStructureData>();
                                                                                    List<AppData.ProjectStructureData> projectsFilteredResults = new List<AppData.ProjectStructureData>();

                                                                                    foreach (var validProject in validProjectsfound)
                                                                                    {
                                                                                        var fileName = Path.GetFileName(validProject);

                                                                                        AppData.StorageDirectoryData directoryData = new AppData.StorageDirectoryData
                                                                                        {
                                                                                            name = fileName,
                                                                                            path = validProject,
                                                                                            projectDirectory = filterDirectory.projectDirectory,
                                                                                            type = filterDirectory.type
                                                                                        };

                                                                                        LoadData<AppData.ProjectStructureData>(directoryData, loadedProjectCallbackResults =>
                                                                                        {
                                                                                            if (loadedProjectCallbackResults.Success())
                                                                                                validProjectsfoundDirectories.Add(loadedProjectCallbackResults.data);
                                                                                            else
                                                                                                LogError($"Project Data Failed To Load : {fileName} From Path : {directoryData.path} In Directory Directory : {directoryData.projectDirectory} With Results : {loadedProjectCallbackResults.results}", this);
                                                                                        });
                                                                                    }

                                                                                    AppData.Helpers.SerializableComponentValid(validProjectsfoundDirectories, hasComponentsCallbackResults =>
                                                                                    {
                                                                                        callbackResults.resultsCode = hasComponentsCallbackResults.resultsCode;

                                                                                        if (callbackResults.Success())
                                                                                        {
                                                                                            #region Project Search Filter

                                                                                            foreach (var validDirectory in validProjectsfoundDirectories)
                                                                                            {
                                                                                                if (validDirectory.GetProjectInfo().GetCategoryType() == filterType)
                                                                                                {
                                                                                                    if (!projectsFilteredResults.Contains(validDirectory))
                                                                                                        projectsFilteredResults.Add(validDirectory);
                                                                                                    else
                                                                                                    {
                                                                                                        callbackResults.results = $"Projects Filtered Results Already Contains Valid File Data Directory : {validDirectory}.";
                                                                                                        callbackResults.data = default;
                                                                                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                                                                                                        break;
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if (projectsFilteredResults.Contains(validDirectory))
                                                                                                        projectsFilteredResults.Remove(validDirectory);

                                                                                                    if (projectsFilteredResults.Contains(validDirectory))
                                                                                                    {
                                                                                                        callbackResults.results = $"Failed To Remove Valid Directory Data : {validDirectory} From Projects Filtered Results.";
                                                                                                        callbackResults.data = default;
                                                                                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                                                                    }
                                                                                                }
                                                                                            }

                                                                                            AppData.Helpers.SerializableComponentValid(projectsFilteredResults, hasComponentsCallbackResults =>
                                                                                            {
                                                                                                callbackResults.resultsCode = hasComponentsCallbackResults.resultsCode;

                                                                                                if (callbackResults.Success())
                                                                                                {
                                                                                                    CreateUIScreenProjectSelectionWidgets(ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType(), projectsFilteredResults, GetWidgetsRefreshData().widgetsContainer, (widgetsCreated) =>
                                                                                                    {
                                                                                                        callbackResults.results = widgetsCreated.results;
                                                                                                        callbackResults.resultsCode = widgetsCreated.resultsCode;

                                                                                                        if (callbackResults.Success())
                                                                                                        {
                                                                                                            callbackResults.data = filterType;
                                                                                                            projectsFound = widgetsCreated.resultsCode == AppData.Helpers.SuccessCode;

                                                                                                            if (GetProjectRootStructureData().Success())
                                                                                                            {
                                                                                                                var rootStructureData = GetProjectRootStructureData().data;
                                                                                                                rootStructureData.rootProjectStructure.GetProjectInfo().SetCategoryType(filterType);

                                                                                                                SaveModifiedData(rootStructureData, dataSavedCallbackResults =>
                                                                                                                {
                                                                                                                    callbackResults.results = dataSavedCallbackResults.results;
                                                                                                                    callbackResults.resultsCode = dataSavedCallbackResults.resultsCode;

                                                                                                                    if(callbackResults.Success())
                                                                                                                    {
                                                                                                                        var sortingContents = GetDropdownContent<AppData.SortType>().data;

                                                                                                                        AppData.Helpers.StringListValueValid(sortingContents, isValidCallbackResults =>
                                                                                                                        {
                                                                                                                            if (isValidCallbackResults.Success())
                                                                                                                                sortingContents.Remove(sortingContents.Find(content => content.Contains("Category")));
                                                                                                                            else
                                                                                                                                Log(isValidCallbackResults.resultsCode, isValidCallbackResults.results, this);
                                                                                                                        });

                                                                                                                        var sortingListParam = GetUIScreenGroupContentTemplate("Sorting Contents", AppData.InputType.DropDown, placeHolder: "Sort", contents: sortingContents, dropdownActionType: AppData.InputDropDownActionType.SortingList);
                                                                                                                        SetContentScreenUIStatesEvent(sortingListParam);

                                                                                                                        //ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownOptions(AppData.InputDropDownActionType.SortingList, sortingContents);
                                                                                                                    }

                                                                                                                    Log(callbackResults.resultsCode, callbackResults.results, this);
                                                                                                                });
                                                                                                            }
                                                                                                            else
                                                                                                                Log(GetProjectRootStructureData().resultsCode, GetProjectRootStructureData().results, this);
                                                                                                        }
                                                                                                    });
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    callbackResults.results = $"Couldn't Find Widgets For Filter Type : {filterType}";
                                                                                                    callbackResults.data = default;
                                                                                                }
                                                                                            });

                                                                                            #endregion
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            callbackResults.results = "Couldn't Get Valid Projects File Data.";
                                                                                            callbackResults.data = default;
                                                                                        }
                                                                                    });
                                                                                }
                                                                                else
                                                                                {
                                                                                    callbackResults.results = $"Couldn't Find Any Valid Project Files In Directory : {filterDirectory.projectDirectory}";
                                                                                    callbackResults.data = default;
                                                                                }
                                                                            });

                                                                            #endregion
                                                                        }
                                                                        else
                                                                        {
                                                                            callbackResults.results = widgetsClearedCallbackResults.results;
                                                                            callbackResults.data = default;
                                                                            callbackResults.resultsCode = widgetsClearedCallbackResults.resultsCode;
                                                                        }

                                                                        #endregion
                                                                    });
                                                                }
                                                                else
                                                                {
                                                                    callbackResults.results = $"Couldn't Find Project Directory Data From Directory : {filterDirectory.projectDirectory}.";
                                                                    callbackResults.data = default;
                                                                }
                                                            });
                                                        }
                                                        else
                                                        {
                                                            callbackResults.results = $"Couldn't Filter Project Widgets - Directory : {filterDirectory.projectDirectory} Not Found.";
                                                            callbackResults.data = default;
                                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                        }
                                                    }
                                                    else
                                                        Log(GetAppDirectoryData(rootProjectStructureData.GetProjectStructureData().rootFolder.directoryType).resultsCode, GetAppDirectoryData(rootProjectStructureData.GetProjectStructureData().rootFolder.directoryType).results, this);
                                                }
                                                else
                                                {
                                                    if (GetProjectRootStructureData().Success())
                                                    {
                                                        var rootStructureData = GetProjectRootStructureData().data;
                                                        rootStructureData.rootProjectStructure.GetProjectInfo().SetCategoryType(filterType);

                                                        SaveModifiedData(rootStructureData, dataSavedCallbackResults =>
                                                        {
                                                            callbackResults.results = dataSavedCallbackResults.results;
                                                            callbackResults.resultsCode = dataSavedCallbackResults.resultsCode;

                                                            if(callbackResults.Success())
                                                                ScreenUIManager.Instance.Refresh();

                                                            Log(callbackResults.resultsCode, callbackResults.results, this);
                                                        });
                                                    }
                                                    else
                                                        Log(GetProjectRootStructureData().resultsCode, GetProjectRootStructureData().results, this);
                                                }
                                            }
                                            else
                                                Log(enumCallbackResults.resultsCode, enumCallbackResults.results, this);
                                        });

                                        break;

                                    case AppData.UIScreenType.ProjectViewScreen:

                                        GetDropdownContentTypeFromIndex<AppData.AssetCategoryType>(filterIndex, enumCallbackResults =>
                                        {
                                            if (enumCallbackResults.Success())
                                            {
                                                var filterType = (AppData.AssetCategoryType)enumCallbackResults.data;

                                                if (filterType != AppData.AssetCategoryType.None)
                                                {
                                                    if (GetAppDirectoryData(rootProjectStructureData.GetProjectStructureData().rootFolder.directoryType).Success())
                                                    {
                                                        var filterDirectory = GetAppDirectoryData(rootProjectStructureData.GetProjectStructureData().rootFolder.directoryType).data;

                                                        if (DirectoryFound(filterDirectory))
                                                        {
                                                            var filteredAssetFiles = Directory.GetFileSystemEntries(filterDirectory.projectDirectory, "*.json", SearchOption.TopDirectoryOnly);
                                                        }
                                                        else
                                                        {
                                                            callbackResults.results = $"Couldn't Filter Project Widgets - Directory : {filterDirectory.projectDirectory} Not Found.";
                                                            callbackResults.data = default;
                                                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                                        }
                                                    }
                                                    else
                                                        Log(GetAppDirectoryData(rootProjectStructureData.GetProjectStructureData().rootFolder.directoryType).resultsCode, GetAppDirectoryData(rootProjectStructureData.GetProjectStructureData().rootFolder.directoryType).results, this);
                                                }
                                                else
                                                    ScreenUIManager.Instance.Refresh();
                                            }
                                            else
                                                Log(enumCallbackResults.resultsCode, enumCallbackResults.results, this);
                                        });

                                        break;
                                }
                            }
                        });
                    }
                    else
                        callbackResults.results = "Screen UI Manager Instance Is Not Yet Initialized";
                });

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        void GetFilterTypesFromContent(List<AppData.ProjectStructureData> contents, Action<AppData.CallbackDataList<string>> callback = null, params string[] args)
        {
            AppData.CallbackDataList<string> callbackResults = new AppData.CallbackDataList<string>();

            AppData.Helpers.SerializableComponentValid(contents, hasComponentsCallbackResults => 
            {
                callbackResults.resultsCode = hasComponentsCallbackResults.resultsCode;

                if(callbackResults.Success())
                {
                    List<string> filterContent = new List<string>();

                    foreach (var content in contents)
                        if (!filterContent.Contains(content.GetProjectInfo().categoryType.ToString()))
                            filterContent.Add(content.GetProjectInfo().categoryType.ToString());

                    AppData.Helpers.StringListValueValid(filterContent, hasComponentsCallbackResults => 
                    {
                        callbackResults.resultsCode = hasComponentsCallbackResults.resultsCode;

                        if(callbackResults.Success())
                        {
                            List<string> contentDataList = new List<string>();
                            List<string> validContentDataList = new List<string>();

                            if (args.Length > 0 && filterContent.Count > 0)
                            {
                                foreach (var item in args)
                                {
                                    foreach (var data in filterContent)
                                    {
                                        if (data.Contains(item) && data != item)
                                        {
                                            string content = data.Replace(item, "");

                                            if (!contentDataList.Contains(content))
                                                contentDataList.Add(content);
                                        }

                                        if (data.Contains(item) && data == item)
                                        {
                                            if (contentDataList.Contains(data))
                                                contentDataList.Remove(data);
                                        }
                                    }
                                }

                                AppData.Helpers.StringListValueValid(contentDataList, hasValuesComponentsCallbackResults =>
                                {
                                    callbackResults.resultsCode = hasValuesComponentsCallbackResults.resultsCode;

                                    if (callbackResults.Success())
                                    {
                                        foreach (var item in args)
                                        {
                                            for (int i = 0; i < filterContent.Count; i++)
                                                if (!validContentDataList.Contains(contentDataList[i]) && !contentDataList[i].Contains(item) && contentDataList[i] != item)
                                                    validContentDataList.Add(contentDataList[i]);

                                            if (validContentDataList.Contains(item))
                                                validContentDataList.Remove(item);
                                        }

                                        AppData.Helpers.StringListValueValid(validContentDataList, hasValuesComponentsCallbackResults =>
                                        {
                                            callbackResults.resultsCode = hasValuesComponentsCallbackResults.resultsCode;

                                            if (callbackResults.Success())
                                            {
                                                callbackResults.results = $"Found : {hasValuesComponentsCallbackResults.data.Count} Widget Type(s).";
                                                callbackResults.data = hasValuesComponentsCallbackResults.data;
                                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                            }
                                            else
                                            {
                                                callbackResults.results = "Couldn't Get Any Filter Types - Please Check Here.";
                                                callbackResults.data = default;
                                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                            }
                                        });
                                    }
                                    else
                                    {
                                        callbackResults.results = "Failed To Get Dropdown Content";
                                        callbackResults.data = default;
                                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                    }
                                });
                            }
                            else
                            {
                                callbackResults.results = "Failed There Are No Args Or Data Content Is Null.";
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = "There Were No Filter Contents Found.";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.WarningCode;
                        }
                    });
                }
                else
                {
                    callbackResults.results = "There Are No Contents Assigned To Get Filter Types From.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            });


            if(callbackResults.Success())
                Log(callbackResults.resultsCode, $"===========================>> Get Filter Types For :{callbackResults.data.Count} Content.", this);
            else
                Log(callbackResults.resultsCode, $"===========================>> Get Filter Content Failed.", this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Root Project Structure

        public AppData.CallbackData<AppData.ProjectRootStructureData> GetProjectRootStructureData()
        {
            AppData.CallbackData<AppData.ProjectRootStructureData> callbackResults = new AppData.CallbackData<AppData.ProjectRootStructureData>(); 

            LoadRootStructureData(loadedRootStructureCallbackResults => 
            {
                callbackResults.results = loadedRootStructureCallbackResults.results;
                callbackResults.data = default;
                callbackResults.resultsCode = loadedRootStructureCallbackResults.resultsCode;

                if (loadedRootStructureCallbackResults.Success())
                    callbackResults.data = loadedRootStructureCallbackResults.data;
            });

            return callbackResults;
        }

        #endregion

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
                        widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableWidgetType.Folder, GetFolderStructureData().GetLayoutViewType(), prefabCallbackResults =>
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

        void CreateDynamicScreenContents<U>(AppData.UIScreenWidget contentPrefab, List<U> contents, AppData.ContentContainerType containerType, AppData.OrientationType containerOrientation, Action<AppData.CallbackDataList<AppData.UIScreenWidget>> callback = null)
        {
            AppData.CallbackDataList<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDataList<AppData.UIScreenWidget>();

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

        #region Data

        public void GetFileData(AppData.SelectableWidgetType dataType, Action<AppData.CallbackData<AppData.FileData>> callback)
        {
            AppData.CallbackData<AppData.FileData> callbackResults = new AppData.CallbackData<AppData.FileData>();

            AppData.Helpers.ProjectDataComponentValid(fileDatas, componentsValidCallback => 
            {
                callbackResults.results = componentsValidCallback.results;
                callbackResults.resultsCode = componentsValidCallback.resultsCode;

                if(callbackResults.Success())
                {
                    var fileData = fileDatas.Find(data => data.dataType == dataType);

                    AppData.Helpers.ProjectDataComponentValid(fileData, componentsValidCallback => 
                    {
                        callbackResults.results = componentsValidCallback.results;
                        callbackResults.resultsCode = componentsValidCallback.resultsCode;

                        if (callbackResults.Success())
                            callbackResults.data = fileData;
                    });
                }
            });

            callback.Invoke(callbackResults);
        }

        #endregion

        #region Data Serialization

        #region Create Data

        public void CreateData<T>(T data, AppData.StorageDirectoryData directoryData, Action<AppData.CallbackData<T>> callback) where T : AppData.SerializableData
        {
            AppData.CallbackData<T> callbackResults = new AppData.CallbackData<T>();

            DirectoryFound(directoryData.projectDirectory, directoryCheckCallback =>
            {
                if (directoryCheckCallback.Success())
                {
                    callbackResults.results = directoryCheckCallback.results;
                    callbackResults.resultsCode = directoryCheckCallback.resultsCode;

                    if (string.IsNullOrEmpty(data.name))
                        data.name = data.GetType().ToString();

                    data.SetCreationDateTime(DateTime.Now);

                    string storageDirectory = data.storageData.projectDirectory;

                    string fileNameWithJSONExtension = data.storageData.name + ".json";
                    string filePath = Path.Combine(directoryData.projectDirectory, fileNameWithJSONExtension);
                    string formattedFilePath = filePath.Replace("\\", "/");

                    data.storageData.path = formattedFilePath;
                    data.storageData.projectDirectory = storageDirectory;

                    string JSONString = JsonUtility.ToJson(data);

                    if (!string.IsNullOrEmpty(JSONString))
                    {
                        if (!File.Exists(formattedFilePath))
                        {
                            File.WriteAllText(formattedFilePath, JSONString);

                            callbackResults.results = $"Created New Data Success : : {data.name} As : {formattedFilePath}";
                            callbackResults.data = data;
                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                        }
                        else
                        {
                            File.Delete(formattedFilePath);

                            if (!File.Exists(formattedFilePath))
                                File.WriteAllText(formattedFilePath, JSONString);

                            callbackResults.results = $"Created New Data Success : Replaced Asset : {data.name} At Path : {formattedFilePath}";
                            callbackResults.data = data;
                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = "Failed To Create A JSON File.";
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

        #endregion

        #region Save Data

        public void SaveData<T>(T data, Action<AppData.Callback> callback = null) where T : AppData.SerializableData
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (File.Exists(data.storageData.path))
            {
                data.creationDateTime = new AppData.DateTimeComponent(DateTime.Now);

                string JSONString = JsonUtility.ToJson(data);

                if (!string.IsNullOrEmpty(JSONString))
                {
                    if (!File.Exists(data.storageData.path))
                    {
                        File.WriteAllText(data.storageData.path, JSONString);

                        callbackResults.results = $"-->  Save Data Success : : {data.name} As : {data.storageData.path}";
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        File.Delete(data.storageData.path);

                        if (!File.Exists(data.storageData.path))
                            File.WriteAllText(data.storageData.path, JSONString);

                        callbackResults.results = $"--> Create New Data Success : Replaced Asset : {data.name} At Path : {data.storageData.path}";
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                }
                else
                {
                    callbackResults.results = $"--> Failed To Create A JSON File.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callbackResults.results = $"Success - File Saved in Directory : {data.storageData.path}";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = $"Save data Failed : File Not found In Directory : {data.storageData.path}";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void SaveModifiedData<T>(T data, Action<AppData.CallbackData<T>> callback) where T : AppData.SerializableData
        {
            AppData.CallbackData<T> callbackResults = new AppData.CallbackData<T>();

            SaveData(data, dataSavedCallbackResults =>
            {
                callbackResults.results = dataSavedCallbackResults.results;
                callbackResults.resultsCode = dataSavedCallbackResults.resultsCode;
            });

            callback.Invoke(callbackResults);
        }

        #endregion

        #region Load Data

        public void LoadRootStructureData(Action<AppData.CallbackData<AppData.ProjectRootStructureData>> callback)
        {
            AppData.CallbackData<AppData.ProjectRootStructureData> callbackResults = new AppData.CallbackData<AppData.ProjectRootStructureData>();

            if (GetAppDirectoryData(rootStructureStorageData.type).Success())
            {
                var appStorageData = GetAppDirectoryData(rootStructureStorageData.type).data;

                if (DirectoryFound(appStorageData))
                {
                    LoadData<AppData.ProjectRootStructureData>(rootProjectStructureData.name, appStorageData, (rootStructureLoadedCallbackResults) =>
                    {
                        callbackResults.results = rootStructureLoadedCallbackResults.results;
                        callbackResults.resultsCode = rootStructureLoadedCallbackResults.resultsCode;

                        if (callbackResults.Success())
                            callbackResults.data = rootStructureLoadedCallbackResults.data;
                        else
                        {
                            var projectInfo = new AppData.ProjectInfo
                            {
                                name = rootProjectStructureData.name,
                                sortType = AppData.SortType.Ascending,
                                categoryType = AppData.ProjectCategoryType.Project_All
                            };

                            rootProjectStructureData.GetProjectStructureData().projectInfo = projectInfo;
                            rootProjectStructureData.storageData.name = projectInfo.name;
                            string storageName = projectInfo.name + "_RootStructureData";
                            appStorageData.name = storageName;

                            CreateData(rootProjectStructureData, appStorageData, (rootStructureCreatedCallbackResults) =>
                            {
                                callbackResults.results = rootStructureCreatedCallbackResults.results;
                                callbackResults.data = default;
                                callbackResults.resultsCode = rootStructureCreatedCallbackResults.resultsCode;

                                if (callbackResults.Success())
                                    callbackResults.data = rootStructureCreatedCallbackResults.data;
                            });
                        }
                    });
                }
                else
                {
                    callbackResults.results = "Root Project Storage Data Directory Not Found.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                    LogError($"=========>>>>>>> Directory Not Found For {rootStructureStorageData.type}", this);
                }
            }
            else
            {
                callbackResults.results = GetAppDirectoryData(rootStructureStorageData.type).results;
                callbackResults.resultsCode = GetAppDirectoryData(rootStructureStorageData.type).resultsCode;
            }

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
                string[] files = Directory.GetFiles(directoryData.projectDirectory, "*.json");

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
                            callbackResults.results = $"--> File : {fileName} Not Loaded From Directory : {directoryData.projectDirectory} - Loaded Data Is Null / Empty.";
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            callbackResults.data = default;
                        }
                    }
                }
                else
                {
                    callbackResults.results = $"--> No Files Found In Directory : {directoryData.projectDirectory}";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    callbackResults.data = default;
                }
            }
            else
            {
                callbackResults.results = $"Load Data Failed : Directory : {directoryData.projectDirectory} Not Found.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                callbackResults.data = default;
            }

            callback.Invoke(callbackResults);
        }

        #endregion

        #endregion
    }
}
