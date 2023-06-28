using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ScreenUIManager : AppMonoBaseClass
    {
        #region Static

        private static ScreenUIManager _instance;

        public static ScreenUIManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<ScreenUIManager>();

                return _instance;
            }
        }

        #endregion

        #region UI Screens

        [SerializeField]
        List<AppData.UIScreenViewComponent> screens = new List<AppData.UIScreenViewComponent>();

        [Space(5)]
        [SerializeField]
        AppData.UIScreenViewComponent currentScreen = new AppData.UIScreenViewComponent();

        [Space(5)]
        [SerializeField]
        RectTransform screenWidgetsContainer = null;

        float screenTransitionSpeed = 0.0f;

        bool canTransitionScreen = false;
        bool screensInitialized = false;

        Vector2 targetScreenPoint = Vector2.zero;

        Coroutine pageRefreshRoutine;

        string screenInitializationResults;

        #endregion

        #region Unity Callbacks

        void Awake() => SetupInstance();

        void Start()
        {
            Init(initializationCallback => 
            {
                if (initializationCallback.Success())
                    AppData.ActionEvents.OnAppScreensInitializedEvent();
                else
                    Log(initializationCallback.resultsCode, initializationCallback.results, this, () => Start());
            });
        }

        void Update() => OnScreenTransition();

        #endregion

        #region Main

        void SetupInstance()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(_instance.gameObject);
            }

            _instance = this;
        }

        void Init(Action<AppData.CallbackDatas<AppData.UIScreenViewComponent>> callback = null)
        {
            AppData.CallbackDatas<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackDatas<AppData.UIScreenViewComponent>();

            if (!HasRequiredComponentsAssigned())
            {
                List<UIScreenHandler> screenComponents = GetComponentsInChildren<UIScreenHandler>().ToList();

                AppData.Helpers.ListComponentHasData(screenComponents, hasDataCallback => 
                {
                    if(AppData.Helpers.IsSuccessCode(hasDataCallback.resultsCode))
                    {
                        screens = new List<AppData.UIScreenViewComponent>();

                        foreach (var screenComponent in screenComponents)
                        {
                            AppData.UIScreenViewComponent newScreen = new AppData.UIScreenViewComponent
                            {
                                name = screenComponent.GetScreenTitle(),
                                value = screenComponent
                            };

                            AddScreen(newScreen, screenAddCallback => { callbackResults = screenAddCallback; });

                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                continue;
                            else
                                break;
                        }

                        AppData.Helpers.ListComponentHasEqualDataSize(callbackResults.data, screenComponents, compareDataCallback => 
                        {
                            if(compareDataCallback.Success())
                            {
                                callbackResults.results = $"{compareDataCallback.size} Screen(s) Has Been Initialized Successfully.";
                                callbackResults.data = compareDataCallback.tuple_A;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;

                                foreach (var screenData in callbackResults.data)
                                {
                                    screenData.value.Init(screenInitializationCallback =>
                                    {
                                        callbackResults.results = screenInitializationCallback.results;
                                        callbackResults.resultsCode = screenInitializationCallback.resultsCode;
                                    });

                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultsCode))
                                        continue;
                                    else
                                        break;
                                }

                                if (callbackResults.Success())
                                {
                                    SetCurrentScreenData(GetScreenData(AppManager.Instance.GetInitialLoadDataPackets()));
                                    SetScreensInitialized(AppData.Helpers.IsSuccessCode(callbackResults.resultsCode), callbackResults.results);
                                }
                                else
                                    Log(callbackResults.resultsCode, callbackResults.results, this);
                            }
                            else
                            {
                                callbackResults.results = compareDataCallback.results;
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        });
                    }
                    else
                    {
                        callbackResults.results = hasDataCallback.results;
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                });
            }
            else
            {
                callbackResults.results = $"{GetScreenCount()} Screen(s) Have Already Been Initialized";
                callbackResults.resultsCode = AppData.Helpers.WarningCode;
            }

            Log(callbackResults.resultsCode, callbackResults.results, this, () => Init(callback));

            callback?.Invoke(callbackResults);
        }

        public void AddScreen(AppData.UIScreenViewComponent screen, Action<AppData.CallbackDatas<AppData.UIScreenViewComponent>> callback = null)
        {
            AppData.CallbackDatas<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackDatas<AppData.UIScreenViewComponent>();

            if (screen.value)
            {
                if (!screens.Contains(screen))
                {
                    screens.Add(screen);

                    if(screens.Contains(screen))
                    {
                        callbackResults.results = $"Screen : {screen.name} Of Type : {screen.value.GetUIScreenType()} Has Been Added To Screen List.";
                        callbackResults.data = screens;
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"Couldn't Add Screen : {screen.name} Of Type : {screen.value.GetUIScreenType()} For Unknown Reasons - Please Check Here.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = $"Screen : {screen.name} Of Type : {screen.value.GetUIScreenType()} Already Exists In The Screen List.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.WarningCode;
                }
            }
            else
            {
                callbackResults.results = $"Screen : {screen.name}'s Value Is Missing / Null.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void GetUIScreen(bool first, Action<AppData.CallbackData<AppData.UIScreenViewComponent>> callback)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            string screenPos = (first) ? "First" : "Last";

            if (HasRequiredComponentsAssigned())
            {
                AppData.UIScreenViewComponent screen = (first)? GetScreens()[0] : GetScreens().FindLast(screen => screen.value);

                callbackResults.results = $"{screenPos} Screen Found.";
                callbackResults.data = screen;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
               
                callbackResults.results = $"Couldn't Get {screenPos} Screen - Possible Issue - Screens Are Missing / Not Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public AppData.UIScreenType GetCurrentUIScreenType()
        {
            return currentScreen.value.GetUIScreenType();
        }

        public bool HasRequiredComponentsAssigned()
        {
            return screensInitialized;
        }

        public void SetScreensInitialized(bool isInitialized, string results)
        {
           screensInitialized = isInitialized;

            if (isInitialized)
                LogSuccess(results, this, () => SetScreensInitialized(isInitialized, results));
            else
                LogWarning(results, this, () => SetScreensInitialized(isInitialized, results));
        }

        void OnScreenTransition()
        {
            if (HasRequiredComponentsAssigned())
            {
                if (canTransitionScreen)
                {
                    Vector2 screenPoint = screenWidgetsContainer.anchoredPosition;
                    screenPoint = Vector2.Lerp(screenPoint, targetScreenPoint, screenTransitionSpeed * Time.smoothDeltaTime);

                    screenWidgetsContainer.anchoredPosition = screenPoint;
                    float distance = (screenWidgetsContainer.anchoredPosition - targetScreenPoint).sqrMagnitude;

                    if (distance <= 0.1f)
                    {
                        AppData.ActionEvents.OnScreenChangedEvent(currentScreen.value.GetUIScreenType());
                        HideScreens(currentScreen.value.GetUIScreenType());
                        canTransitionScreen = false;
                    }
                }
            }
            else
                LogWarning($"Couldn't Transition Screen : {currentScreen?.name} Of Type : {currentScreen?.value?.GetUIScreenType()} - Possible Issue - Screens Are Missing / Not Found.", this, () => OnScreenTransition());
        }

        public void ShowNewAssetScreen(AppData.SceneDataPackets dataPackets)
        {
            try
            {
                if (HasRequiredComponentsAssigned())
                {
                    if (SceneAssetsManager.Instance)
                    {
                        foreach (var screen in screens)
                        {
                            if (screen.value.GetUIScreenType() == dataPackets.screenType)
                            {
                                //if (assetsManager == null)
                                //    assetsManager = SceneAssetsManager.Instance;

                                AppData.SceneAsset sceneAsset = new AppData.SceneAsset();
                                //sceneAsset.name = "Create New Asset";

                                AppData.AssetInfoHandler info = new AppData.AssetInfoHandler();

                                info.fields = new List<AppData.AssetInfoField>();

                                // Title Info
                                AppData.AssetInfoField titleInfoField = new AppData.AssetInfoField();
                                titleInfoField.name = SceneAssetsManager.Instance.GetDefaultAssetName();
                                titleInfoField.type = AppData.InfoDisplayerFieldType.Title;

                                // Vertices Info
                                AppData.AssetInfoField verticeCounterInfoField = new AppData.AssetInfoField();
                                verticeCounterInfoField.value = 0;
                                verticeCounterInfoField.type = AppData.InfoDisplayerFieldType.VerticesCounter;

                                // Triangles Info
                                AppData.AssetInfoField trianglesCounterInfoField = new AppData.AssetInfoField();
                                trianglesCounterInfoField.value = 0;
                                trianglesCounterInfoField.type = AppData.InfoDisplayerFieldType.TriangleCounter;

                                // Add Fields
                                info.fields.Add(titleInfoField);
                                info.fields.Add(verticeCounterInfoField);
                                info.fields.Add(trianglesCounterInfoField);

                                sceneAsset.info = info;

                                sceneAsset.currentAssetMode = AppData.SceneAssetModeType.CreateMode;
                                SceneAssetsManager.Instance.SetCurrentSceneAsset(sceneAsset);
                                SceneAssetsManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

                                //if (SceneAssetsManager.Instance.GetCurrentSceneAsset().info.fields != null)
                                //    screen.value.DisplaySceneAssetInfo(SceneAssetsManager.Instance.GetCurrentSceneAsset());
                                //else
                                //    Debug.LogWarning("--> Info Fields Not Initialized.");

                                //AppData.ActionEvents.OnClearPreviewedSceneAssetObjectEvent();


                                //screen.value.DisplaySceneAssetInfo(sceneAsset);

                                screen.value.Show();

                                currentScreen = screen;

                                if (currentScreen.value != null)
                                    currentScreen.value.SetScreenData(dataPackets);
                                else
                                    Debug.LogWarning("--> Unity - Current Screen Value Is Null.");

                                AppData.ActionEvents.OnScreenChangeEvent(dataPackets);

                                if (dataPackets.canTransitionScreen)
                                {
                                    if (screenWidgetsContainer != null)
                                    {
                                        targetScreenPoint = screen.value.GetScreenPosition();
                                        canTransitionScreen = dataPackets.canTransitionScreen;
                                        screenTransitionSpeed = dataPackets.screenTransitionSpeed;
                                    }
                                    else
                                    {
                                        Debug.LogWarning("--> Screen Widgets Container Required.");
                                    }
                                }

                                break;
                            }
                        }
                    }
                    else
                        LogWarning("Scene Assets Manager Not Yet Initialized To Show New Asset Screen.", this, () => ShowNewAssetScreen(dataPackets));
                }
                else
                    LogWarning($"Couldn't Show New Asset Screen Of Type : {dataPackets.screenType} - Possible Issue - Screens Are Missing / Not Found.", this, () => ShowNewAssetScreen(dataPackets));
            }
            catch (Exception exception)
            {
                throw new Exception($"--> Unity - Failed To Show New Asset Screen  - With Exception : {exception}");
            }
        }

        public void ShowScreen(AppData.SceneDataPackets dataPackets) => OnTriggerShowScreenAsync(dataPackets, showScreenAsyncCallback => 
        {
            Log(showScreenAsyncCallback.resultsCode, showScreenAsyncCallback.results, this);
        });

        async void OnTriggerShowScreenAsync(AppData.SceneDataPackets dataPackets, Action<AppData.Callback> callback = null)
        {
            await ShowScreenAsync(dataPackets, showScreenCallback => 
            {
                callback?.Invoke(showScreenCallback);
            });
        }

        async Task OnCheckIfScreenLoadedAsync(AppData.SceneDataPackets dataPackets, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (GetScreenData(dataPackets) != null && GetScreenData(dataPackets).value != null)
            {
                callbackResults.results = $"Screen Of Type : {dataPackets.screenType} Found.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
                await AppData.Helpers.GetWaitForSecondAsync(100);

            callback?.Invoke(callbackResults);
        }

        async Task ShowScreenAsync(AppData.SceneDataPackets dataPackets, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            try
            {
                if (SceneAssetsManager.Instance)
                {
                    await OnCheckIfScreenLoadedAsync(dataPackets, async screenLoadedCallback =>
                    {
                        if (screenLoadedCallback.Success())
                        {
                            //if (currentScreen.value != null)
                            //    if (currentScreen.value.GetUIScreenType() == dataPackets.screenType)
                            //    {
                            //        LogWarning($"Current Screen Type : {currentScreen.value.GetUIScreenType()} Is Equals To Requested Screen Type : {dataPackets.screenType}. On Screen Exit Event Called.", this, () => ShowScreenAsync(dataPackets, callback = null));
                            //        AppData.ActionEvents.OnScreenExitEvent();
                            //    }

                            await AppData.Helpers.GetWaitUntilAsync(HasRequiredComponentsAssigned());

                            LogInfo($"Has Required Components : {HasRequiredComponentsAssigned()} So If True - Broer Please Show Screen Of Type : {dataPackets.screenType}", this);

                            if (HasRequiredComponentsAssigned())
                            {
                                GetScreenData(dataPackets, screenFoundCallback =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(screenFoundCallback.resultsCode))
                                    {
                                        OnShowSelectedScreenView(screenFoundCallback.data, showScreenViewCallback => 
                                        {

                                            LogInfo($"Show Selected Screen : {screenFoundCallback.data.name} Of Type : {dataPackets.screenType} Results : {showScreenViewCallback.results}", this);

                                            if (showScreenViewCallback.Success())
                                            {
                                                OnUpdateUIScreenOnEnter(screenFoundCallback.data, uiScreenUpdatedCallback =>
                                                {
                                                    if (AppData.Helpers.IsSuccessCode(uiScreenUpdatedCallback.resultsCode))
                                                    {
                                                        screenFoundCallback.data.value.SetScreenData(dataPackets);
                                                        SetCurrentScreenData(screenFoundCallback.data);

                                                        AppData.ActionEvents.OnScreenChangeEvent(dataPackets);

                                                        if (dataPackets.canTransitionScreen)
                                                        {
                                                            if (screenWidgetsContainer != null)
                                                            {
                                                                targetScreenPoint = screenFoundCallback.data.value.GetScreenPosition();
                                                                canTransitionScreen = dataPackets.canTransitionScreen;
                                                                screenTransitionSpeed = dataPackets.screenTransitionSpeed;
                                                            }
                                                            else
                                                                LogWarning("Screen Widgets Container Required.", this);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        callbackResults.results = uiScreenUpdatedCallback.results;
                                                        callbackResults.resultsCode = uiScreenUpdatedCallback.resultsCode;
                                                    }
                                                });

                                                screenFoundCallback.data.value.SetScreenData(dataPackets);
                                                SetCurrentScreenData(screenFoundCallback.data);

                                                screenFoundCallback.data.value.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, "");

                                                AppData.ActionEvents.OnScreenChangeEvent(dataPackets);

                                                if (dataPackets.canTransitionScreen)
                                                {
                                                    if (screenWidgetsContainer != null)
                                                    {
                                                        targetScreenPoint = screenFoundCallback.data.value.GetScreenPosition();
                                                        canTransitionScreen = dataPackets.canTransitionScreen;
                                                        screenTransitionSpeed = dataPackets.screenTransitionSpeed;
                                                    }
                                                    else
                                                        LogWarning("Screen Widgets Container Required.", this, () => { ShowScreen(dataPackets); });
                                                }
                                            }
                                            else
                                            {
                                                callbackResults.results = showScreenViewCallback.results;
                                                callbackResults.resultsCode = showScreenViewCallback.resultsCode;
                                            }
                                        });
                                    }
                                    else
                                    {
                                        callbackResults.results = screenFoundCallback.results;
                                        callbackResults.resultsCode = screenFoundCallback.resultsCode;
                                    }
                                });
                            }
                            else
                            {
                                callbackResults.results = $"Couldn't Show Screen Of Type : {dataPackets.screenType} - Possible Issue - Screens Are Missing / Not Found.";
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }

                            callbackResults.results = $"Screen Of Type : {dataPackets.screenType} Has Been Loaded Successfully";
                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                        }
                        else
                            callbackResults = screenLoadedCallback;
                    });
                }
                else
                {
                    callbackResults.results = $"Couldn't Show Screen Of Type : {dataPackets.screenType} - Possible Issue - Screens Are Missing / Not Found.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            catch (Exception exception)
            {
                ThrowException(AppData.LogExceptionType.NullReference, exception, this, "ShowScreen(AppData.SceneDataPackets dataPackets)");
                //throw new Exception($"--> RG_Unity - Unity - Failed To Show Screen Type : {dataPackets.ToString()} - With Exception Results : {exception}");
            }

            callback?.Invoke(callbackResults);
        }

        void OnUpdateUIScreenOnEnter(AppData.UIScreenViewComponent screen, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (HasRequiredComponentsAssigned())
            {
                if (SceneAssetsManager.Instance != null)
                {
                    switch(screen.value.GetUIScreenType())
                    {
                        case AppData.UIScreenType.ProjectViewScreen:

                            if(SceneAssetsManager.Instance.GetWidgetsContentCount() == 0)
                                screen.value.ShowWidget(AppData.WidgetType.LoadingWidget);

                            SceneAssetsManager.Instance.GetFolderContentCount(SceneAssetsManager.Instance.GetCurrentFolder(), folderFound =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(folderFound.resultsCode))
                                    SceneAssetsManager.Instance.DisableUIOnScreenEnter(screen.value.GetUIScreenType());
                            });

                            if (SceneAssetsManager.Instance.GetFolderStructureData().GetCurrentLayoutViewType() == AppData.LayoutViewType.ItemView)
                            {
                                screen.value.SetActionButtonUIImageValue(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ListViewIcon, setUIStateCallback =>
                                {
                                    callbackResults.results = setUIStateCallback.results;
                                    callbackResults.resultsCode = setUIStateCallback.resultsCode;
                                });
                            }

                            if (SceneAssetsManager.Instance.GetFolderStructureData().GetCurrentLayoutViewType() == AppData.LayoutViewType.ListView)
                            {
                                screen.value.SetActionButtonUIImageValue(AppData.InputActionButtonType.ChangeLayoutViewButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ItemViewIcon, setUIStateCallback =>
                                {
                                    callbackResults.results = setUIStateCallback.results;
                                    callbackResults.resultsCode = setUIStateCallback.resultsCode;
                                });
                            }

                            screen.value.SetUITextDisplayerValue(AppData.ScreenTextType.NavigationRootTitleDisplayer, SceneAssetsManager.Instance.GetFolderStructureData().GetRootFolder().name);
                            screen.value.SetActionButtonState(AppData.InputActionButtonType.Return, AppData.InputUIState.Hidden);

                            SceneAssetsManager.Instance.GetFolderContentCount(SceneAssetsManager.Instance.GetCurrentFolder(), folderFound =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(folderFound.resultsCode))
                                {
                                    SceneAssetsManager.Instance.DisableUIOnScreenEnter(screen.value.GetUIScreenType());

                                    callbackResults.results = $"UI Scene View Of Type {screen.value.GetUIScreenType()} Has Been Updated - UI Disabled On Enter.";
                                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                }
                                else
                                {
                                    callbackResults.results = $"UI Scene View Of Type {screen.value.GetUIScreenType()} Has Been Updated.";
                                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                }
                            });

                            break;

                        case AppData.UIScreenType.AssetCreationScreen:

                            screen.value.DisplaySceneAssetInfo(screen, displaySeneAssetInfoCallback => 
                            {
                                callbackResults.results = displaySeneAssetInfoCallback.results;
                                callbackResults.resultsCode = displaySeneAssetInfoCallback.resultsCode;
                            });

                            break;
                    }
                }
                else
                {
                    callbackResults.results = "Scene Assets Manager Instance Is Not Yet Initialized.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = $"Couldn't Update Screen : {screen.name} Of Type : {screen.value.GetUIScreenType()} on Enter - Possible Issue - Screens Are Missing / Not Found.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void HideScreen(AppData.UIScreenType screenType)
        {
            if (HasRequiredComponentsAssigned())
            {
                AppData.UIScreenViewComponent screenData = screens.Find((x) => x.value.GetUIScreenType() == screenType);

                if (screenData.value)
                    screenData.value.Hide();
                else
                    Debug.LogWarning("--> RG_Unity - Hide Screen : Screen Data Value Is Null");
            }
            else
                LogWarning($"Couldn't Hide Screen Of Type : {screenType} - Possible Issue - Screens Are Missing / Not Found.", this, () => HideScreen(screenType));
        }

        public void HideScreens()
        {
            if (HasRequiredComponentsAssigned())
            {
                foreach (var screen in screens)
                {
                    if (screen.value)
                        screen.value.Hide();
                    else
                        Debug.LogWarning("--> RG_Unity - Hide Screen : Screen Data Value Is Null");
                }
            }
            else
                LogWarning($"Couldn't Hide Screens. Possible Issue - Screens Are Missing / Not Found.", this, () => HideScreens());
        }

        public void HideScreens(AppData.UIScreenType excludeScreenType)
        {
            if (HasRequiredComponentsAssigned())
            {
                foreach (var screen in GetScreens())
                {
                    if (screen.value)
                        if (screen.value.GetUIScreenType() != excludeScreenType)
                            screen.value.Hide();
                }
            }
            else
                LogWarning($"Couldn't Exclude Screen Of Type : {excludeScreenType} On Hide Screens - Screens Are Missing / Not Found.", this, () => HideScreens(excludeScreenType));
        }

        public void OnShowSelectedScreenView(AppData.UIScreenViewComponent screen, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if(AppData.Helpers.ComponentIsNotNullOrEmpty(screen))
            {
                screen.value.Show(screenViewShowCallback => 
                {
                    callbackResults.results = screenViewShowCallback.results;
                    callbackResults.resultsCode = screenViewShowCallback.resultsCode;
                });
            }
            else
            {
                callbackResults.results = "Couldn't Show UI Screen, Screen Data Is Null / Invalid.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void ShowLoadingItem(AppData.LoadingItemType loadingItemType, bool status)
        {
            if (currentScreen.value != null)
                currentScreen.value.ShowLoadingItem(loadingItemType, status);
            else
                LogWarning($"Couldn't Execute : {loadingItemType}. Current Screen Missing.", this, () => ShowLoadingItem(loadingItemType, status));
        }

        public void Refresh()
        {
            if (pageRefreshRoutine != null)
                StopCoroutine(pageRefreshRoutine);

            pageRefreshRoutine = StartCoroutine(OnScreenRefresh(currentScreen.value.GetScreenData()));
        }

        public void ScreenRefresh()
        {
            if (SelectableManager.Instance != null)
            {
                if (SelectableManager.Instance.HasFocusedWidgetInfo())
                    SelectableManager.Instance.OnClearFocusedSelectionsInfo();

                if (SceneAssetsManager.Instance != null)
                {
                    SceneAssetsManager.Instance.GetDynamicWidgetsContainer(AppData.ContentContainerType.FolderStuctureContent, widgetsContentContainer =>
                    {
                        if (AppData.Helpers.IsSuccessCode(widgetsContentContainer.resultsCode))
                        {
                            Debug.LogError("==> Check Heere -Has Something To Do Regarding Ambushed Selection Data.");

                            //widgetsContentContainer.data.DeselectAllContentWidgets();

                            //widgetsContentContainer.data.ClearAllFocusedWidgetInfo();
                        }
                        else
                            LogError(widgetsContentContainer.results, this, () => ScreenRefresh());
                    });
                }
                else
                    LogError("Refresh Button Failed : SelectableManager.Instance Is Not Yet Initialized", this, () => ScreenRefresh());
            }
            else
                LogWarning("Refresh Button Failed : SelectableManager.Instance Is Not Yet Initialized", this, () => ScreenRefresh());

            AppData.ActionEvents.OnScreenUIRefreshed();

            Canvas.ForceUpdateCanvases();
        }

        IEnumerator OnScreenRefresh(AppData.SceneDataPackets dataPackets)
        {
            if (SceneAssetsManager.Instance)
            {
                if(dataPackets.blurScreen)
                    currentScreen.value.Blur(dataPackets);

                if (currentScreen.value != null)
                    currentScreen.value.ShowLoadingItem(dataPackets.screenRefreshLoadingItemType, true);

                SceneAssetsManager.Instance.GetDynamicWidgetsContainer(SceneAssetsManager.Instance.GetContainerType(GetCurrentUIScreenType()), containerResults => 
                {
                    if(containerResults.Success())
                    {
                        var rootFolder = (GetCurrentUIScreenType() == AppData.UIScreenType.ProjectSelectionScreen)? AppManager.Instance.GetInitialStructureData().rootFolder : SceneAssetsManager.Instance.GetFolderStructureData().rootFolder;
                        var container = containerResults.data;

                        SceneAssetsManager.Instance.SetWidgetsRefreshData(rootFolder, container);
                    }
                });

                if (currentScreen.value.GetScreenData().refreshSceneAssets)
                    yield return new WaitUntil(() => SceneAssetsManager.Instance.Refreshed(SceneAssetsManager.Instance.GetCurrentFolder(), SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer, dataPackets)); // Wait For Assets To Be Refreshed.
                else
                    yield return AppData.Helpers.GetWaitForSeconds(dataPackets.refreshDuration);

                if (currentScreen.value != null)
                    currentScreen.value.ShowLoadingItem(dataPackets.screenRefreshLoadingItemType, false);

                currentScreen.value.Focus();

                AppData.ActionEvents.OnScreenRefreshed(currentScreen);
            }
            else
                LogError("On Screen Refresh Failed : Scene Assets Manager Not Initialized.", this);
        }

        public void UpdateInfoDisplayer(AppData.UIScreenViewComponent screen)
        {
            if (AppData.Helpers.ComponentIsNotNullOrEmpty(screen.value.GetSceneAsset().info.fields))
            {
                if (GetCurrentScreenData().value != null)
                    GetCurrentScreenData().value.DisplaySceneAssetInfo(screen);
                else
                    LogWarning("Current Screen Data Not Found.", this, () => UpdateInfoDisplayer(screen));
            }
            else
                LogWarning("The Current Scene Asset Info Fields Not Initialized.", this, () => UpdateInfoDisplayer(screen));
        }

        public AppData.SceneDataPackets GetScreenData()
        {
            return currentScreen.value.GetScreenData();
        }

        public void SetCurrentScreenData(AppData.UIScreenViewComponent screenData)
        {
            currentScreen = screenData;
        }

        public AppData.UIScreenViewComponent GetScreenData(AppData.SceneDataPackets dataPackets)
        {
            AppData.UIScreenViewComponent screenData = screens.Find((screen) => screen.value.GetUIScreenType() == dataPackets.screenType);
            return screenData;
        }

        public void GetScreenData(AppData.SceneDataPackets dataPackets, Action<AppData.CallbackData<AppData.UIScreenViewComponent>> callback)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            if (HasRequiredComponentsAssigned())
            {
                AppData.Helpers.GetComponentIsNotNullOrEmpty(GetScreens().Find(screen => screen?.value.GetUIScreenType() == dataPackets.screenType), componentCheckCallback => 
                {
                    callbackResults = componentCheckCallback;
                });
            }
            else
            {
                callbackResults.results = $"UI Screen Of Type : {dataPackets.screenType} Not Found / Missing / Null. Possible Bug - Screen Initialization Might Have Failed.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public Transform GetScreenWidgetsContainer()
        {
            return currentScreen.value?.GetWidgetsContainer();
        }

        public Transform GetScreenWidgetsContainer(AppData.UIScreenType screenType)
        {
            Transform widgetsContainer = null;

            if (HasRequiredComponentsAssigned())
            {
                foreach (var screen in screens)
                {
                    if (screen.value.GetUIScreenType() == screenType)
                    {
                        widgetsContainer = screen.value.GetWidgetsContainer();
                        break;
                    }
                }
            }

            return widgetsContainer;
        }

        public AppData.UIScreenViewComponent GetCurrentScreenData()
        {
            return currentScreen;
        }

        #region UI States

        #region Screen States

        public void SetScreenUIState(AppData.UIScreenType screenType, AppData.InputUIState state, params Enum[] parameters)
        {

        }

        #endregion

        #region Button States

        public void SetScreenActionButtonUIImageValue(AppData.UIScreenType screenType, AppData.InputActionButtonType actionType, AppData.UIImageDisplayerType displayerType, AppData.UIImageType imageType)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionButtonUIImageValue(actionType, displayerType, imageType);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        public void SetScreenActionButtonState(AppData.UIScreenType screenType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionButtonState(state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        public void SetScreenActionButtonState(AppData.UIScreenType screenType, AppData.InputActionButtonType actionType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionButtonState(actionType, state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        #endregion

        #region Input States

        public void SetScreenActionInputFieldState(AppData.UIScreenType screenType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionInputFieldState(state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        public void SetScreenActionInputFieldState(AppData.UIScreenType screenType, AppData.InputFieldActionType actionType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionInputFieldState(actionType, state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        public void SetScreenActionInputFieldPlaceHolderText(AppData.UIScreenType screenType, AppData.InputFieldActionType actionType, string placeholder)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionInputFieldPlaceHolderText(actionType, placeholder);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        #endregion

        #region Dropdown States

        public void SetScreenActionDropdownState(AppData.UIScreenType screenType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionDropdownState(state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }


        public void SetScreenActionDropdownState(AppData.UIScreenType screenType, AppData.InputUIState state, List<string> content)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionDropdownState(state, content);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        public void SetScreenActionDropdownState(AppData.UIScreenType screenType, AppData.InputDropDownActionType actionType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionDropdownState(actionType, state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        public void SetScreenActionDropdownState(AppData.UIScreenType screenType, AppData.InputDropDownActionType actionType, AppData.InputUIState state, List<string> content)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionDropdownState(actionType, state, content);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        #endregion

        #region Slider States

        public void SetScreenActionSliderState(AppData.UIScreenType screenType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionSliderState(state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        public void SetScreenActionSliderState(AppData.UIScreenType screenType, AppData.SliderValueType valueType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionSliderState(valueType, state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        #endregion

        #region Checkbox States


        public void SetScreenActionCheckboxState(AppData.UIScreenType screenType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionCheckboxState(state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        public void SetScreenActionCheckboxState(AppData.UIScreenType screenType, AppData.CheckboxInputActionType actionType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionCheckboxState(actionType, state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        #endregion

        #region Checkbox Value

        /// <summary>
        /// This Functions Sets Checkbox Selection Value State On The Specific Screen.
        /// </summary>
        /// <param name="screenType"></param>
        /// <param name="value">The State Of The Checkbox</param>
        public void SetScreenActionCheckboxValue(AppData.UIScreenType screenType, bool value)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionCheckboxValue(value);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        /// <summary>
        /// This Functions Sets Checkbox Selection Value State On The Specific Screen.
        /// </summary>
        /// <param name="screenType"></param>
        /// <param name="actionType">The Type Of Checkbox.</param>
        /// <param name="value">The State Of The Checkbox.</param>
        public void SetScreenActionCheckboxValue(AppData.UIScreenType screenType, AppData.CheckboxInputActionType actionType, bool value)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetActionCheckboxValue(actionType, value);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        #endregion

        #region UI Text Value

        /// <summary>
        /// This Functions Sets Text String To The Selected Text Displayer Type On The Specific Screen.
        /// </summary>
        /// <param name="screenType"></param>
        /// <param name="textType"></param>
        /// <param name="value"></param>
        public void SetScreenUITextValue(AppData.UIScreenType screenType, AppData.ScreenTextType textType, string value)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetUITextDisplayerValue(textType, value);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        #endregion


        #region UI Image Value

        /// <summary>
        ///  This Functions Sets A Screen Captured Data To The Selected Displayer Type On The Specific Screen.
        /// </summary>
        /// <param name="screenType"></param>
        /// <param name="displayerType"></param>
        /// <param name="screenCaptureData"></param>
        /// <param name="dataPackets"></param>
        public void SetScreenUIImageValue(AppData.UIScreenType screenType, AppData.ScreenImageType displayerType, AppData.ImageData screenCaptureData, AppData.ImageDataPackets dataPackets)
        {
            // Find Screen
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetUIImageDisplayerValue(displayerType, screenCaptureData, dataPackets);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }


        /// <summary>
        ///  This Functions Sets A UI Texture 2D To The Selected Displayer Type On The Specific Screen.
        /// </summary>
        /// <param name="screenType"></param>
        /// <param name="displayerType"></param>
        /// <param name="imageData"></param>
        public void SetScreenUIImageValue(AppData.UIScreenType screenType, AppData.ScreenImageType displayerType, Texture2D imageData)
        {
            // Find Screen
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetUIImageDisplayerValue(displayerType, imageData);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        /// <summary>
        /// This Functions Sets A UI Sprite To The Selected Displayer Type On The Specific Screen.
        /// </summary>
        /// <param name="screenType">The Screen To Set Image To.</param>
        /// <param name="actionType"></param>
        /// <param name="displayerType"></param>
        /// <param name="image"></param>
        public void SetScreenUIImageValue(AppData.UIScreenType screenType, AppData.ScreenImageType displayerType, Sprite image)
        {
            // Find Screen
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultsCode))
                    screenFound.data.value.SetUIImageDisplayerValue(displayerType, image);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.results}");
            });
        }

        #endregion

        #endregion

        public void OnFindScreenOfType(AppData.UIScreenType screenType, Action<AppData.CallbackData<AppData.UIScreenViewComponent>> callback)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            // Check For Screens
            if (screens.Count > 0)
            {
                AppData.UIScreenViewComponent screen = screens.Find(screen => screen.value.GetUIScreenType() == screenType);

                if (screen.value != null)
                {
                    callbackResults.results = $"Screen Found.";
                    callbackResults.data = screen;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"OnFindScreenOfType Failed : Screen Of Type : {screenType} Not Found - Value Missing.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "OnFindScreenOfType Failed : Screens Are Missing / Null.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void SetScreenSleepTime(bool neverSleep)
        {
            if (neverSleep)
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            else
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

        public List<AppData.UIScreenViewComponent> GetScreens()
        {
            return screens;
        }

        public int GetScreenCount()
        {
            return screens.Count;
        }

        #endregion
    }
}
