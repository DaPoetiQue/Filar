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

        AppData.SceneDataPackets previousScreenData;

        float screenTransitionSpeed = 0.0f;

        AppData.ScreenLoadTransitionType transitionType = AppData.ScreenLoadTransitionType.None;
        bool clearInputsOnTransition;
        bool screensInitialized = false;

        Vector2 targetScreenPoint = Vector2.zero;

        Coroutine pageRefreshRoutine;

        #endregion

        #region Unity Callbacks

        void Awake() => SetupInstance();

        //void Start()
        //{
        //    Init(initializationCallback => 
        //    {
        //        if (initializationCallback.Success())
        //            AppData.ActionEvents.OnAppScreensInitializedEvent();
        //        else
        //            Log(initializationCallback.resultsCode, initializationCallback.results, this, () => Start());
        //    });
        //}

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

        public void OnScreenInit(Action<AppData.CallbackDataList<AppData.UIScreenViewComponent>> callback = null)
        {
            AppData.CallbackDataList<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackDataList<AppData.UIScreenViewComponent>();

            if (!HasRequiredComponentsAssigned())
            {
                List<UIScreenHandler> screenComponents = GetComponentsInChildren<UIScreenHandler>().ToList();

                AppData.Helpers.ListComponentHasData(screenComponents, hasDataCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(hasDataCallback.resultCode))
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

                            if (AppData.Helpers.IsSuccessCode(callbackResults.resultCode))
                                continue;
                            else
                                break;
                        }

                        AppData.Helpers.ListComponentHasEqualDataSize(callbackResults.data, screenComponents, compareDataCallback =>
                        {
                            if (compareDataCallback.Success())
                            {
                                callbackResults.result = $"{compareDataCallback.size} Screen(s) Has Been Initialized Successfully.";
                                callbackResults.data = compareDataCallback.tuple_A;
                                callbackResults.resultCode = AppData.Helpers.SuccessCode;

                                foreach (var screenView in callbackResults.data)
                                {
                                    screenView.value.Init(screenInitializationCallback =>
                                    {
                                        callbackResults.result = screenInitializationCallback.result;
                                        callbackResults.resultCode = screenInitializationCallback.resultCode;
                                    });

                                    if (AppData.Helpers.IsSuccessCode(callbackResults.resultCode))
                                        continue;
                                    else
                                        break;
                                }

                                if (callbackResults.Success())
                                {
                                    //SetCurrentScreenData(GetScreenData(AppManager.Instance.GetInitialLoadDataPackets()));
                                    SetScreensInitialized(callbackResults.Success(), callbackResults.result);
                                }
                                else
                                    Log(callbackResults.resultCode, callbackResults.result, this);
                            }
                            else
                            {
                                callbackResults.result = compareDataCallback.result;
                                callbackResults.data = default;
                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                            }
                        });
                    }
                    else
                    {
                        callbackResults.result = hasDataCallback.result;
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                });
            }
            else
            {
                callbackResults.result = $"{GetScreenCount()} Screen(s) Have Already Been Initialized";
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            Log(callbackResults.resultCode, callbackResults.result, this);

            callback?.Invoke(callbackResults);
        }

        public void AddScreen(AppData.UIScreenViewComponent screen, Action<AppData.CallbackDataList<AppData.UIScreenViewComponent>> callback = null)
        {
            AppData.CallbackDataList<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackDataList<AppData.UIScreenViewComponent>();

            if (screen.value)
            {
                if (!screens.Contains(screen))
                {
                    screens.Add(screen);

                    if (screens.Contains(screen))
                    {
                        callbackResults.result = $"Screen : {screen.name} Of Type : {screen.value.GetUIScreenType()} Has Been Added To Screen List.";
                        callbackResults.data = screens;
                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.result = $"Couldn't Add Screen : {screen.name} Of Type : {screen.value.GetUIScreenType()} For Unknown Reasons - Please Check Here.";
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.result = $"Screen : {screen.name} Of Type : {screen.value.GetUIScreenType()} Already Exists In The Screen List.";
                    callbackResults.data = default;
                    callbackResults.resultCode = AppData.Helpers.WarningCode;
                }
            }
            else
            {
                callbackResults.result = $"Screen : {screen.name}'s Value Is Missing / Null.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void GetUIScreen(bool first, Action<AppData.CallbackData<AppData.UIScreenViewComponent>> callback)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            string screenPos = (first) ? "First" : "Last";

            if (HasRequiredComponentsAssigned())
            {
                AppData.UIScreenViewComponent screen = (first) ? GetScreens()[0] : GetScreens().FindLast(screen => screen.value);

                callbackResults.result = $"{screenPos} Screen Found.";
                callbackResults.data = screen;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {

                callbackResults.result = $"Couldn't Get {screenPos} Screen - Possible Issue - Screens Are Missing / Not Found.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public AppData.UIScreenType GetCurrentUIScreenType()
        {
            AppData.UIScreenType currentScreenType = AppData.UIScreenType.None;

            if (currentScreen?.value)
                currentScreenType = currentScreen.value.GetUIScreenType();

            return currentScreenType;
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

        public bool IsInitialized() => screensInitialized;

        void OnScreenTransition()
        {
            if (transitionType == AppData.ScreenLoadTransitionType.Translate)
            {
                if (HasRequiredComponentsAssigned())
                {
                    Vector2 screenPoint = screenWidgetsContainer.anchoredPosition;
                    screenPoint = Vector2.Lerp(screenPoint, targetScreenPoint, screenTransitionSpeed * Time.smoothDeltaTime);

                    screenWidgetsContainer.anchoredPosition = screenPoint;
                    float distance = (screenWidgetsContainer.anchoredPosition - targetScreenPoint).sqrMagnitude;

                    if (distance <= 0.1f)
                    {
                        AppData.ActionEvents.OnScreenChangedEvent(currentScreen.value.GetUIScreenType());
                        transitionType = AppData.ScreenLoadTransitionType.None;

                        if (clearInputsOnTransition)
                        {
                            SelectableManager.Instance.GetProjectStructureSelectionSystem(selectionSystemCallback =>
                            {
                                if (selectionSystemCallback.Success())
                                    selectionSystemCallback.data.OnClearInputSelection();
                                else
                                    Log(selectionSystemCallback.resultCode, selectionSystemCallback.result, this);
                            });
                        }
                    }
                }
                else
                    LogWarning($"Couldn't Transition Screen : {currentScreen?.name} Of Type : {currentScreen?.value?.GetUIScreenType()} - Possible Issue - Screens Are Missing / Not Found.", this, () => OnScreenTransition());
            }
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

                                sceneAsset.assetMode = AppData.AssetModeType.CreateMode;
                                SceneAssetsManager.Instance.SetCurrentSceneAsset(sceneAsset);
                                SceneAssetsManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

                                //if (SceneAssetsManager.Instance.GetCurrentSceneAsset().info.fields != null)
                                //    screen.value.DisplaySceneAssetInfo(SceneAssetsManager.Instance.GetCurrentSceneAsset());
                                //else
                                //    Debug.LogWarning("--> Info Fields Not Initialized.");

                                //AppData.ActionEvents.OnClearPreviewedSceneAssetObjectEvent();


                                //screen.value.DisplaySceneAssetInfo(sceneAsset);

                                //screen.value.ShowViewAsync();

                                currentScreen = screen;

                                if (currentScreen.value != null)
                                    currentScreen.value.SetScreenData(dataPackets);
                                else
                                    Debug.LogWarning("--> Unity - Current Screen Value Is Null.");

                                AppData.ActionEvents.OnScreenChangeEvent(dataPackets);

                                if (dataPackets.screenTransition == AppData.ScreenLoadTransitionType.Default)
                                {
                                    if (screenWidgetsContainer != null)
                                    {
                                        targetScreenPoint = screen.value.GetScreenPosition();
                                        transitionType = dataPackets.screenTransition;
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

        public void OnAppBootScreen(AppData.UIScreenType bootScreenType, Action<AppData.CallbackData<AppData.SceneDataPackets>> callback = null)
        {
            AppData.CallbackData<AppData.SceneDataPackets> callbackResults = new AppData.CallbackData<AppData.SceneDataPackets>();

            OnScreenInit(initializationCallbackResults =>
            {
                callbackResults.result = initializationCallbackResults.result;
                callbackResults.resultCode = initializationCallbackResults.resultCode;

                if (callbackResults.Success())
                {
                    AppData.LogInfoChannel bootScreenCallbackResultsCode = (bootScreenType != AppData.UIScreenType.None) ? AppData.Helpers.SuccessCode : AppData.Helpers.WarningCode;
                    string bootScreenCallbackResults = (bootScreenCallbackResultsCode == AppData.LogInfoChannel.Success) ? $"On App Boot State - Initial Boot Screen : {bootScreenType}" : $"On App Boot Failed : Boot Screen Type Is Not Yet Initialized / Set Tpo Default : {bootScreenType}.";

                    callbackResults.result = bootScreenCallbackResults;
                    callbackResults.resultCode = bootScreenCallbackResultsCode;

                    if (callbackResults.Success())
                    {
                        var bootScreen = initializationCallbackResults.data.Find(screen => screen.value.GetUIScreenType() == bootScreenType);

                        AppData.Helpers.GetAppComponentValid(bootScreen, bootScreen.name, bootScreenValidCallbackResults =>
                        {
                            callbackResults.result = bootScreenValidCallbackResults.result;
                            callbackResults.resultCode = bootScreenValidCallbackResults.resultCode;

                            if (callbackResults.Success())
                            {
                                AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name, hasAssetsManagerCallbackResults =>
                                {
                                    callbackResults.result = hasAssetsManagerCallbackResults.result;
                                    callbackResults.resultCode = hasAssetsManagerCallbackResults.resultCode;

                                    if (callbackResults.Success())
                                    {
                                        SceneAssetsManager.Instance.GetDataPacketsLibrary().GetDataPacket(bootScreenType, bootScreenDataPacketsCallbackResults =>
                                        {
                                            callbackResults.result = bootScreenDataPacketsCallbackResults.result;
                                            callbackResults.resultCode = bootScreenDataPacketsCallbackResults.resultCode;

                                            if (callbackResults.Success())
                                            {
                                                callbackResults.data = bootScreenDataPacketsCallbackResults.data.dataPackets;
                                                callbackResults.result = $"App Initial Boot Screen : {bootScreenType} Data Packets Have Been Loaded Successfully - Show Boot Screen : {callbackResults.data.screenType} Now.";
                                            }
                                            else
                                                callbackResults.result = $"Failed To Load App Initial Boot Screen : {bootScreenType}'s Data Packets - Results : {callbackResults.result}";
                                        });
                                    }
                                    else
                                        callbackResults.result = $"On App Boot Screen Failed With Results : {callbackResults.result}.";

                                }, $"On App Boot Screen Failed - Scene Assets Manager Instance Is Not Yet Initialized.");
                            }
                            else
                                callbackResults.result = $"On App Boot Screen Failed With Results : {callbackResults.result}.";

                        }, $"On App Boot Screen Failed - Boot Screen : {bootScreenType} Is Not Found / Missing / Null / Not Initialized In The Unity Inspector Panel.");

                        AppData.ActionEvents.OnAppScreensInitializedEvent();
                    }
                }
            });

            callback?.Invoke(callbackResults);
        }

        #region On Show Screen Async

        public async Task<AppData.CallbackData<AppData.UIScreenViewComponent>> ShowScreenAsync(AppData.SceneDataPackets dataPackets) { return await OnTriggerShowScreenAsync(dataPackets); }

        async Task<AppData.CallbackData<AppData.UIScreenViewComponent>> OnTriggerShowScreenAsync(AppData.SceneDataPackets dataPackets)
        {
            return await OnShowScreenAsync(dataPackets);
        }

        async Task<AppData.CallbackData<AppData.UIScreenViewComponent>> OnShowScreenAsync(AppData.SceneDataPackets dataPackets)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            if (HasRequiredComponentsAssigned())
            {
                GetScreenData(dataPackets, screenFoundCallback =>
                {
                    callbackResults = screenFoundCallback;

                    if (callbackResults.Success())
                    {
                        if(GetCurrentUIScreenType() != AppData.UIScreenType.None && GetCurrentUIScreenType() != AppData.UIScreenType.SplashScreen && GetCurrentUIScreenType() != AppData.UIScreenType.LoadingScreen)
                             AppData.ActionEvents.OnScreenExitEvent(GetCurrentUIScreenType());

                        callbackResults.data.value.SetScreenData(dataPackets);
                        SetCurrentScreenData(callbackResults.data);

                        OnCheckIfScreenLoadedAsync(dataPackets, screenLoadedCallbackResults =>
                        {
                            callbackResults.result = screenLoadedCallbackResults.result;
                            callbackResults.resultCode = screenLoadedCallbackResults.resultCode;

                            if (screenLoadedCallbackResults.data != null)
                            {
                                callbackResults.result = $"Screen : {dataPackets.screenType} Has Been Loaded Successfully.";
                                callbackResults.data = screenLoadedCallbackResults.data;
                            }
                            else
                            {
                                callbackResults.result = $"Failed To Load Screen : {dataPackets.screenType}";
                                callbackResults.data = default;
                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                            }

                            if (callbackResults.Success())
                            {
                                if (GetCurrentUIScreenType() != AppData.UIScreenType.None && GetCurrentUIScreenType() != AppData.UIScreenType.SplashScreen && GetCurrentUIScreenType() != AppData.UIScreenType.LoadingScreen)
                                    callbackResults.data.value.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, callbackResults.data.value.GetScreenTitle());
                            }
                            else
                                Log(callbackResults.resultCode, callbackResults.result, this);
                        });

                        if (callbackResults.Success())
                        {
                            //OnUpdateUIScreenOnEnter(callbackResults.data, uiScreenUpdatedCallback =>
                            //{
                            //    callbackResults.results = uiScreenUpdatedCallback.results;
                            //    callbackResults.resultsCode = uiScreenUpdatedCallback.resultsCode;

                            //    if (callbackResults.Success())
                            //    {
                            //        screenFoundCallback.data.value.SetScreenData(dataPackets);
                            //        SetCurrentScreenData(screenFoundCallback.data);

                            //        if (dataPackets.screenTransition == AppData.ScreenLoadTransitionType.Translate)
                            //        {
                            //            if (screenWidgetsContainer != null)
                            //            {
                            //                targetScreenPoint = screenFoundCallback.data.value.GetScreenPosition();
                            //                transitionType = dataPackets.screenTransition;
                            //                screenTransitionSpeed = dataPackets.screenTransitionSpeed;
                            //            }
                            //            else
                            //                LogWarning("Screen Widgets Container Required.", this);
                            //        }
                            //    }
                            //});
                        }
                    }
                });

                await OnShowSelectedScreenViewAsync(callbackResults.data);

                if (callbackResults.Success())
                    AppData.ActionEvents.OnScreenChangeEvent(callbackResults.data.value.GetScreenData());
                else
                    Log(callbackResults.resultCode, callbackResults.result, this);
            }

            return callbackResults;

        }

        public async Task<AppData.CallbackData<AppData.UIScreenViewComponent>> OnShowSelectedScreenViewAsync(AppData.UIScreenViewComponent screen)
        {
            return await screen.value.ShowViewAsync();
        }

        #endregion

        #region On Hide Screen Async

        public async Task<AppData.CallbackData<AppData.UIScreenViewComponent>> HideScreenAsync(AppData.SceneDataPackets dataPackets) { return await OnTriggerHideScreenAsync(dataPackets); }

        async Task<AppData.CallbackData<AppData.UIScreenViewComponent>> OnTriggerHideScreenAsync(AppData.SceneDataPackets dataPackets)
        {
            return await OnHideScreenAsync(dataPackets);
        }

        async Task<AppData.CallbackData<AppData.UIScreenViewComponent>> OnHideScreenAsync(AppData.SceneDataPackets dataPackets)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            GetScreenData(dataPackets, screenFoundCallback =>
            {
                callbackResults = screenFoundCallback;
            });

            await OnHideSelectedScreenViewAsync(callbackResults.data);

            return callbackResults;
        }

        public async Task<AppData.CallbackData<AppData.UIScreenViewComponent>> OnHideSelectedScreenViewAsync(AppData.UIScreenViewComponent screen)
        {
            return await screen.value.HideViewSync();
        }

        #endregion

        #region Go To Screen

        public async Task GoToSelectedScreenAsync(AppData.SceneDataPackets dataPackets, Action<AppData.CallbackData<AppData.SceneDataPackets>> callback = null)
        {
            try
            {
                AppData.CallbackData<AppData.SceneDataPackets> callbackResults = new AppData.CallbackData<AppData.SceneDataPackets>(AppData.Helpers.GetScreenDataPacketsValid(dataPackets, $"Go To Screen : {dataPackets.screenType}'s Data Packets Validation"));

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(ScreenOfTypeExists(dataPackets));

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name, "Scene Assets Manager Instance Is Not Yet Initialized."));

                        if (callbackResults.Success())
                        {
                            var sceneAssetsManager = AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name).data;

                            callbackResults.SetResults(sceneAssetsManager.GetScreenLoadInfoInstanceFromLibrary(dataPackets.screenType));

                            if (callbackResults.Success())
                            {
                                var screenLoadInfo = sceneAssetsManager.GetScreenLoadInfoInstanceFromLibrary(dataPackets.screenType).data;

                                #region Load Screen

                                if (dataPackets.screenTransition == AppData.ScreenLoadTransitionType.LoadingScreen)
                                {
                                    callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, LoadingManager.Instance.name, "Loading Manager Instance Is Not Yet Initialized."));

                                    if (callbackResults.Success())
                                    {
                                        var loadingManager = AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, LoadingManager.Instance.name).data;

                                        await loadingManager.LoadScreen(screenLoadInfo, showSplashScreenCallbackResults =>
                                        {
                                            callbackResults.SetResult(showSplashScreenCallbackResults);

                                            LogInfo($" <==++> Show Spash Screen Results", this);

                                            if (callbackResults.Success())
                                            {

                                            }
                                        });
                                    }
                                }

                                #endregion

                                #region Transition To Screen

                                if (dataPackets.screenTransition == AppData.ScreenLoadTransitionType.Translate)
                                {

                                }

                                #endregion
                            }
                        }
                    }
                }

                callback?.Invoke(callbackResults);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        #endregion

        public AppData.Callback ScreenOfTypeExists(AppData.UIScreenType screenType)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            AppData.Helpers.GetAppComponentsValid(GetScreens(), "Screens", validScreensCallbackResults =>
            {
                callbackResults.SetResults(validScreensCallbackResults);

                if(callbackResults.Success())
                {
                    var screens = validScreensCallbackResults.data;

                    bool success = false;

                    for (int i = 0; i < screens.Count; i++)
                    {
                        if (screens[i].value.GetUIScreenType() == screenType)
                        {
                            callbackResults.result = $"Screen Of Type : {screenType} Exists And Has Been Loaded Successfully.";
                            callbackResults.resultCode = AppData.Helpers.SuccessCode;

                            success = true;

                            break;
                        }
                        else
                            continue;
                    }

                    if(!success)
                    {
                        callbackResults.result = $"Screen Of Type : {screenType} Doesn't Exists In The Loaded Screens List.";
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }

            }, failedOperationFallbackResults: $"Checking For Screen Of Type : {screenType} Failed - Get Screens Failed Because Screens Are Invalid.",  successOperationFallbackResults: $"Checking For Screen Of Type : {screenType} Success - Get Screens Valid - Found : {GetScreens().Count} Valid Loaded Screens");

            return callbackResults;
        }

        public AppData.Callback ScreenOfTypeExists(AppData.SceneDataPackets dataPackets)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            AppData.Helpers.GetAppComponentsValid(GetScreens(), "Screens", validScreensCallbackResults =>
            {
                callbackResults.SetResults(validScreensCallbackResults);

                if (callbackResults.Success())
                {
                    var screens = validScreensCallbackResults.data;

                    bool success = false;

                    for (int i = 0; i < screens.Count; i++)
                    {
                        if (screens[i].value.GetUIScreenType() == dataPackets.screenType)
                        {
                            callbackResults.result = $"Screen Of Type : {dataPackets.screenType} Exists And Has Been Loaded Successfully.";
                            callbackResults.resultCode = AppData.Helpers.SuccessCode;

                            success = true;

                            break;
                        }
                        else
                            continue;
                    }

                    if (!success)
                    {
                        callbackResults.result = $"Screen Of Type : {dataPackets.screenType} Doesn't Exists In The Loaded Screens List.";
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }

            }, failedOperationFallbackResults: $"Checking For Screen Of Type : {dataPackets.screenType} Failed - Get Screens Failed Because Screens Are Invalid.", successOperationFallbackResults: $"Checking For Screen Of Type : {dataPackets.screenType} Success - Get Screens Valid - Found : {GetScreens().Count} Valid Loaded Screens");

            return callbackResults;
        }

        public AppData.Callback CheckIfScreensAreValid(AppData.UIScreenType screenType)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            AppData.Helpers.GetAppComponentsValid(GetScreens(), "Screens", validScreensCallbackResults =>
            {
                callbackResults.SetResults(validScreensCallbackResults);

                if (callbackResults.Success())
                {
                    var screens = validScreensCallbackResults.data;

                    for (int i = 0; i < screens.Count; i++)
                    {
                        if(screens[i].value == null)
                        {
                            int breakIndex = (i + 1) - GetScreens().Count;
                            callbackResults.result = $"Checking If Screens Are Valid Failed - Screen At Index : {i}'s Value Is Missing / Not Assigned : Varified {breakIndex} Screens Of : {GetScreens().Count}.";
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;

                            break;
                        }
                        else
                        {
                            if (screens[i].value.GetUIScreenType() == AppData.UIScreenType.None)
                            {
                                callbackResults.result = $"Checking If Screens Are Valid Failed - Screen At Index : {i} Type Is Set To Default : {GetScreens().Count} Loaded Valid Screens.";
                                callbackResults.resultCode = AppData.Helpers.ErrorCode;

                                break;
                            }
                        }
                    }

                    if (callbackResults.Success())
                        callbackResults.result = $"Checking If Screens Are Valid Success - Found {GetScreens().Count} Loaded Valid Screens.";
                }

            }, failedOperationFallbackResults: $"Checking For Screen Of Type : {screenType} Failed - Get Screens Failed Because Screens Are Invalid.", successOperationFallbackResults: $"Checking For Screen Of Type : {screenType} Success - Get Screens Valid - Found : {GetScreens().Count} Valid Loaded Screens");

            return callbackResults;
        }

        public void ScreenOfTypeExists(AppData.UIScreenType screenType, Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            AppData.Helpers.GetAppComponentsValid(GetScreens(), "Screens", validScreensCallbackResults =>
            {
                callbackResults.SetResults(validScreensCallbackResults);

                if (callbackResults.Success())
                {
                    var screens = validScreensCallbackResults.data;

                    bool success = false;

                    for (int i = 0; i < screens.Count; i++)
                    {
                        if (screens[i].value.GetUIScreenType() == screenType)
                        {
                            callbackResults.result = $"Screen Of Type : {screenType} Exists And Has Been Loaded Successfully.";
                            callbackResults.resultCode = AppData.Helpers.SuccessCode;

                            success = true;

                            break;
                        }
                        else
                            continue;
                    }

                    if (!success)
                    {
                        callbackResults.result = $"Screen Of Type : {screenType} Doesn't Exists In The Loaded Screens List.";
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }

            }, failedOperationFallbackResults: $"Checking For Screen Of Type : {screenType} Failed - Get Screens Failed Because Screens Are Invalid.", successOperationFallbackResults: $"Checking For Screen Of Type : {screenType} Success - Get Screens Valid - Found : {GetScreens().Count} Valid Loaded Screens");

            callback.Invoke(callbackResults);
        }

        void CachePreviousScreenData(AppData.SceneDataPackets screenDataPackets) => previousScreenData = screenDataPackets;

        AppData.CallbackData<AppData.SceneDataPackets> GetPreviousCachedScreenData()
        {
            AppData.CallbackData<AppData.SceneDataPackets> callbackResults = new AppData.CallbackData<AppData.SceneDataPackets>();

            if(previousScreenData != null)
            {
                callbackResults.result = "Previous Screen : Data Loaded Successfully.";
                callbackResults.data = previousScreenData;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Previous Screen Data Not Initialized Yet - Get Previous Loaded Screen Data Failed.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            return callbackResults;
        }

        void OnCheckIfScreenLoadedAsync(AppData.SceneDataPackets dataPackets, Action<AppData.CallbackData<AppData.UIScreenViewComponent>> callback = null)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            if (GetScreenData(dataPackets) != null && GetScreenData(dataPackets).value != null && dataPackets.screenType == GetCurrentUIScreenType())
            {
                callbackResults.result = $"Screen Of Type : {GetCurrentUIScreenType()} Loaded Successfully.";
                callbackResults.data = GetScreenData(dataPackets);
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Screen : {dataPackets.screenType} Failed To Load.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        #region Delete Data

        //async Task ShowLoadingScreenAsync(AppData.SceneDataPackets dataPackets, Action<AppData.Callback> callback = null)
        //{
        //    AppData.Callback callbackResults = new AppData.Callback();

        //    try
        //    {
        //        if (SceneAssetsManager.Instance)
        //        {
        //            SceneAssetsManager.Instance.GetDataPacketsLibrary().GetDataPacket(AppData.UIScreenType.LoadingScreen, async dataPacketsCallbackResults => 
        //            {
        //                callbackResults.results = dataPacketsCallbackResults.results;
        //                callbackResults.resultsCode = dataPacketsCallbackResults.resultsCode;

        //                if(callbackResults.Success())
        //                {
        //                    await OnCheckIfScreenLoadedAsync(dataPacketsCallbackResults.data.dataPackets, async screenLoadedCallbackResults =>
        //                    {
        //                        callbackResults.results = screenLoadedCallbackResults.results;
        //                        callbackResults.resultsCode = screenLoadedCallbackResults.resultsCode;

        //                        if (callbackResults.Success())
        //                        {
        //                            await AppData.Helpers.GetWaitUntilAsync(HasRequiredComponentsAssigned());

        //                            AppData.ActionEvents.OnScreenExitEvent(GetCurrentUIScreenType());

        //                            if (HasRequiredComponentsAssigned())
        //                            {
        //                                GetScreenData(dataPacketsCallbackResults.data.dataPackets, async screenFoundCallbackResults =>
        //                                {
        //                                    callbackResults.results = screenFoundCallbackResults.results;
        //                                    callbackResults.resultsCode = screenFoundCallbackResults.resultsCode;

        //                                    if (callbackResults.Success())
        //                                    {
        //                                        callbackResults = await HideScreen(GetCurrentUIScreenType());

        //                                        LogInfo($" <<<< Hidding Screen Ended : Code {callbackResults.resultsCode} - .", this);

        //                                        if (callbackResults.Success())
        //                                        {
        //                                            //await OnShowSelectedScreenView(screenFoundCallbackResults.data, showScreenViewCallbackResults =>
        //                                            //{
        //                                            //    callbackResults.results = showScreenViewCallbackResults.results;
        //                                            //    callbackResults.resultsCode = showScreenViewCallbackResults.resultsCode;

        //                                            //    if (callbackResults.Success())
        //                                            //    {
        //                                            //        screenFoundCallbackResults.data.value.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, screenFoundCallbackResults.data.value.GetScreenTitle());

        //                                            //    #region Trigger Loading Manager

        //                                            //    AppData.Helpers.GetComponent(ContentLoadingManager.Instance, validComponentCallback =>
        //                                            //        {
        //                                            //            callbackResults.resultsCode = validComponentCallback.resultsCode;

        //                                            //            if (callbackResults.Success())
        //                                            //            {
        //                                            //                SetCurrentScreenData(screenFoundCallbackResults.data);
        //                                            //                ContentLoadingManager.Instance.LoadScreen(dataPackets);
        //                                            //                callbackResults.results = $" Triggered Loading Screen For : {dataPackets.screenType}";
        //                                            //            }
        //                                            //            else
        //                                            //                callbackResults.results = "Content Loading Manager Is Not Yet Initialized.";
        //                                            //        });

        //                                            //    #endregion
        //                                            //}
        //                                            //});
        //                                        }
        //                                    }
        //                                });
        //                            }
        //                            else
        //                            {
        //                                callbackResults.results = $"Couldn't Show Screen Of Type : {dataPackets.screenType} - Possible Issue - Screens Are Missing / Not Found.";
        //                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
        //                            }

        //                            callbackResults.results = $"Screen Of Type : {dataPackets.screenType} Has Been Loaded Successfully";
        //                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
        //                        }
        //                    });
        //                }
        //            });
        //        }
        //        else
        //        {
        //            callbackResults.results = $"Couldn't Show Screen Of Type : {dataPackets.screenType} - Possible Issue - Screens Are Missing / Not Found.";
        //            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        ThrowException(AppData.LogExceptionType.NullReference, exception, this, "ShowScreen(AppData.SceneDataPackets dataPackets)");
        //        //throw new Exception($"--> RG_Unity - Unity - Failed To Show Screen Type : {dataPackets.ToString()} - With Exception Results : {exception}");
        //    }

        //    callback?.Invoke(callbackResults);
        //}

        #endregion

        void OnUpdateUIScreenOnEnter(AppData.UIScreenViewComponent screen, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (HasRequiredComponentsAssigned())
            {
                if (SceneAssetsManager.Instance != null)
                {
                    switch(screen.value.GetUIScreenType())
                    {
                        case AppData.UIScreenType.ProjectDashboardScreen:

                            if(SceneAssetsManager.Instance.GetWidgetsContentCount() == 0)
                                screen.value.ShowWidget(AppData.WidgetType.LoadingWidget);

                            SceneAssetsManager.Instance.GetFolderContentCount(SceneAssetsManager.Instance.GetCurrentFolder(), folderFound =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(folderFound.resultCode))
                                    SceneAssetsManager.Instance.DisableUIOnScreenEnter(screen.value.GetUIScreenType());
                            });

                            if (SceneAssetsManager.Instance.GetProjectStructureData().Success())
                            {
                                if (SceneAssetsManager.Instance.GetProjectStructureData().data.GetLayoutViewType() == AppData.LayoutViewType.ItemView)
                                {
                                    screen.value.SetActionButtonUIImageValue(AppData.InputActionButtonType.LayoutViewButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ListViewIcon, setUIStateCallback =>
                                    {
                                        callbackResults.result = setUIStateCallback.result;
                                        callbackResults.resultCode = setUIStateCallback.resultCode;
                                    });
                                }

                                if (SceneAssetsManager.Instance.GetProjectStructureData().data.GetLayoutViewType() == AppData.LayoutViewType.ListView)
                                {
                                    screen.value.SetActionButtonUIImageValue(AppData.InputActionButtonType.LayoutViewButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ItemViewIcon, setUIStateCallback =>
                                    {
                                        callbackResults.result = setUIStateCallback.result;
                                        callbackResults.resultCode = setUIStateCallback.resultCode;
                                    });
                                }
                            }
                            else
                                Log(SceneAssetsManager.Instance.GetProjectStructureData().resultCode, SceneAssetsManager.Instance.GetProjectStructureData().result, this);

                            if (SceneAssetsManager.Instance.GetProjectStructureData().Success())
                                screen.value.SetUITextDisplayerValue(AppData.ScreenTextType.NavigationRootTitleDisplayer, SceneAssetsManager.Instance.GetProjectStructureData().data.GetRootFolder().name);
                            else
                                Log(SceneAssetsManager.Instance.GetProjectStructureData().resultCode, SceneAssetsManager.Instance.GetProjectStructureData().result, this);

                            screen.value.SetActionButtonState(AppData.InputActionButtonType.Return, AppData.InputUIState.Hidden);

                            SceneAssetsManager.Instance.GetFolderContentCount(SceneAssetsManager.Instance.GetCurrentFolder(), folderFound =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(folderFound.resultCode))
                                {
                                    SceneAssetsManager.Instance.DisableUIOnScreenEnter(screen.value.GetUIScreenType());

                                    callbackResults.result = $"UI Scene View Of Type {screen.value.GetUIScreenType()} Has Been Updated - UI Disabled On Enter.";
                                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                }
                                else
                                {
                                    callbackResults.result = $"UI Scene View Of Type {screen.value.GetUIScreenType()} Has Been Updated.";
                                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                }
                            });

                            break;

                        case AppData.UIScreenType.ContentImportExportScreen:

                            screen.value.DisplaySceneAssetInfo(screen, displaySeneAssetInfoCallback => 
                            {
                                callbackResults.result = displaySeneAssetInfoCallback.result;
                                callbackResults.resultCode = displaySeneAssetInfoCallback.resultCode;
                            });

                            break;
                    }
                }
                else
                {
                    callbackResults.result = "Scene Assets Manager Instance Is Not Yet Initialized.";
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.result = $"Couldn't Update Screen : {screen.name} Of Type : {screen.value.GetUIScreenType()} on Enter - Possible Issue - Screens Are Missing / Not Found.";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
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
            AppData.Helpers.GetComponent(SceneAssetsManager.Instance, hasComponentCallbackResults =>
            {
                if (hasComponentCallbackResults.Success())
                {
                    if (pageRefreshRoutine != null)
                        StopCoroutine(pageRefreshRoutine);

                    pageRefreshRoutine = StartCoroutine(OnScreenRefresh(currentScreen.value.GetScreenData()));
                }
                else
                    LogError("Scene Assets Manager Instance Is Not Yet Initialized.", this);
            });
        }

        public void ScreenRefresh()
        {
            if (SceneAssetsManager.Instance != null)
            {
                if (SelectableManager.Instance != null)
                {
                    if (SelectableManager.Instance.HasFocusedWidgetInfo())
                        SelectableManager.Instance.OnClearFocusedSelectionsInfo();

                    if (HasCurrentScreen().Success())
                    {
                        if (SceneAssetsManager.Instance.GetRootFolder(HasCurrentScreen().data.value.GetUIScreenType()).Success())
                        {
                            SceneAssetsManager.Instance.HasContentToLoadForSelectedScreen(SceneAssetsManager.Instance.GetRootFolder(HasCurrentScreen().data.value.GetUIScreenType()).data, hasContentCallbackResults =>
                            {
                                if (hasContentCallbackResults.Success())
                                {
                                    switch (hasContentCallbackResults.data)
                                    {
                                        case AppData.UIScreenType.LandingPageScreen:

                                            SceneAssetsManager.Instance.GetDynamicWidgetsContainer(AppData.ContentContainerType.LandingPageSelectionContent, widgetsContentContainer =>
                                            {
                                                if (widgetsContentContainer.Success())
                                                {
                                                    LogError("Check Here -Has Something To Do Regarding Ambushed Selection Data.", this);

                                                    //widgetsContentContainer.data.DeselectAllContentWidgets();

                                                    //widgetsContentContainer.data.ClearAllFocusedWidgetInfo();
                                                }
                                                else
                                                    Log(widgetsContentContainer.ResultCode, widgetsContentContainer.Result, this);
                                            });

                                            break;

                                        case AppData.UIScreenType.ProjectCreationScreen:

                                            break;

                                        case AppData.UIScreenType.ProjectDashboardScreen:

                                            SceneAssetsManager.Instance.GetDynamicWidgetsContainer(AppData.ContentContainerType.FolderStuctureContent, widgetsContentContainer =>
                                            {
                                                if (AppData.Helpers.IsSuccessCode(widgetsContentContainer.resultCode))
                                                {
                                                    LogError("Check Here -Has Something To Do Regarding Ambushed Selection Data.", this);

                                                    //widgetsContentContainer.data.DeselectAllContentWidgets();

                                                    //widgetsContentContainer.data.ClearAllFocusedWidgetInfo();
                                                }
                                                else
                                                    LogError(widgetsContentContainer.result, this, () => ScreenRefresh());
                                            });

                                            break;
                                    }
                                }
                                else
                                    Log(hasContentCallbackResults.resultCode, hasContentCallbackResults.result, this);
                            });
                        }
                        else
                            Log(SceneAssetsManager.Instance.GetRootFolder(HasCurrentScreen().data.value.GetUIScreenType()).resultCode, SceneAssetsManager.Instance.GetRootFolder(HasCurrentScreen().data.value.GetUIScreenType()).result, this);
                    }
                    else
                        Log(HasCurrentScreen().resultCode, HasCurrentScreen().result, this);
                }
                else
                    LogWarning("Refresh Button Failed : SelectableManager.Instance Is Not Yet Initialized", this);

                AppData.ActionEvents.OnScreenUIRefreshed();

                Canvas.ForceUpdateCanvases();
            }
            else
                LogError("Refresh Button Failed : Scene Assets Manager Instance Is Not Yet Initialized", this);
        }

        IEnumerator OnScreenRefresh(AppData.SceneDataPackets dataPackets)
        {
            if (dataPackets.blurScreen)
                currentScreen.value.Blur(dataPackets);

            if (currentScreen.value != null)
                currentScreen.value.ShowLoadingItem(dataPackets.screenRefreshLoadingItemType, true);

            SceneAssetsManager.Instance.GetDynamicWidgetsContainer(SceneAssetsManager.Instance.GetContainerType(GetCurrentUIScreenType()), containerCallbackResults =>
            {
                if (containerCallbackResults.Success())
                {
                    switch(GetCurrentUIScreenType())
                    {
                        case AppData.UIScreenType.LandingPageScreen:

                            LogInfo($" *==============* Getting Container.", this);

                            SceneAssetsManager.Instance.SetWidgetsRefreshData(null, containerCallbackResults.data, dataSetupCallbackResults =>
                            {
                                if (dataSetupCallbackResults.Success())
                                {
                                    //SceneAssetsManager.Instance.Init(rootFolder, container, assetsInitializedCallback =>
                                    //{
                                    //    Log(assetsInitializedCallback.resultCode, assetsInitializedCallback.result, this);
                                    //});
                                }
                            });

                            break;

                        case AppData.UIScreenType.ProjectCreationScreen:

                            if (SceneAssetsManager.Instance.GetProjectRootStructureData().Success())
                            {
                                if (SceneAssetsManager.Instance.GetProjectStructureData().Success())
                                {
                                    var rootFolder = (GetCurrentUIScreenType() == AppData.UIScreenType.ProjectCreationScreen) ? SceneAssetsManager.Instance.GetProjectRootStructureData().data.GetProjectStructureData().rootFolder : SceneAssetsManager.Instance.GetProjectStructureData().data.rootFolder;
                                    var container = containerCallbackResults.data;

                                    SceneAssetsManager.Instance.SetWidgetsRefreshData(rootFolder, container, dataSetupCallbackResults =>
                                    {
                                        if (dataSetupCallbackResults.Success())
                                        {
                                            SceneAssetsManager.Instance.Init(rootFolder, container, assetsInitializedCallback =>
                                            {
                                                Log(assetsInitializedCallback.resultCode, assetsInitializedCallback.result, this);
                                            });
                                        }
                                    });
                                }
                                else
                                    Log(SceneAssetsManager.Instance.GetProjectStructureData().resultCode, SceneAssetsManager.Instance.GetProjectStructureData().result, this);
                            }
                            else
                                Log(SceneAssetsManager.Instance.GetProjectRootStructureData().resultCode, SceneAssetsManager.Instance.GetProjectRootStructureData().result, this);

                            break;

                        case AppData.UIScreenType.ProjectDashboardScreen:

                            break;

                        case AppData.UIScreenType.ContentImportExportScreen:

                            break;
                    }
                }
                else
                    Log(containerCallbackResults.resultCode, containerCallbackResults.result, this);
            });

            var container = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

            if (container == null)
            {
                SceneAssetsManager.Instance.GetContentContainer(containerCallbackResults =>
                {
                    if (containerCallbackResults.Success())
                        container = containerCallbackResults.data;
                    else
                        Log(containerCallbackResults.resultCode, containerCallbackResults.result, this);
                });
            }

            if (container != null)
            {
                if (currentScreen.value.GetScreenData().refreshSceneAssets)
                    yield return new WaitUntil(() => SceneAssetsManager.Instance.Refreshed(SceneAssetsManager.Instance.GetCurrentFolder(), container, dataPackets)); // Wait For Assets To Be Refreshed.
                else
                    yield return AppData.Helpers.GetWaitForSeconds(dataPackets.refreshDuration);

                if (currentScreen.value != null)
                    currentScreen.value.ShowLoadingItem(dataPackets.screenRefreshLoadingItemType, false);

                currentScreen.value.Focus();

                AppData.ActionEvents.OnScreenRefreshed(currentScreen);
            }
            else
                LogError("Failed To Refresh Screen - Content Container Null / Missing.", this);
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
            return currentScreen?.value?.GetScreenData();
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
                callbackResults.result = $"UI Screen Of Type : {dataPackets.screenType} Not Found / Missing / Null. Possible Bug - Screen Initialization Might Have Failed.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
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

        public bool ScreenInitialized(AppData.UIScreenViewComponent screen)
        {
            return screen?.value != null;
        }

        public AppData.CallbackData<AppData.UIScreenViewComponent> HasCurrentScreen()
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            if(currentScreen != null && currentScreen?.value != null)
            {
                callbackResults.result = $"Current Screen : {currentScreen.name} Of Type : {currentScreen.value.GetUIScreenType()} Found.";
                callbackResults.data = currentScreen;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Current Screen Not Yet Initialized / Missing / Not Found.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            return callbackResults;
        }

        public void HasCurrentScreen(Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if(currentScreen != null && currentScreen?.value != null)
            {
                callbackResults.result = $"Screen Data : {currentScreen.name} Loaded.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Has No Current Screen Data";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void GetCurrentScreen(Action<AppData.CallbackData<AppData.UIScreenViewComponent>> callback)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            HasCurrentScreen(currentScreenCallbackResults => 
            {
                callbackResults.result = currentScreenCallbackResults.result;
                callbackResults.resultCode = currentScreenCallbackResults.resultCode;

                if (callbackResults.Success())
                    callbackResults.data = currentScreen;
            });

            callback?.Invoke(callbackResults);
        }

        public void GetScreen(AppData.UIScreenType screenType, Action<AppData.CallbackData<AppData.UIScreenViewComponent>> callback)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            AppData.Helpers.GetAppComponentsValid(screens, "App Screen List", hasScreensCallbackResults => 
            {
                callbackResults.result = hasScreensCallbackResults.result;
                callbackResults.resultCode = hasScreensCallbackResults.resultCode;

                if(callbackResults.Success())
                {
                    var screen = screens.Find(screen => screen.value.GetUIScreenType() == screenType);

                    if (screen != null)
                    {
                        callbackResults.result = $"Successfully Found Screen Of Type : {screenType}.";
                        callbackResults.data = screen;
                    }
                    else
                    {
                        callbackResults.result = $"Failed To Find Screen Of Type : {screenType} - Screen Not Loaded - Please Check Screen UI Manager In The Inspector Panel.";
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }

            }, "There Are No App Screens Initialized. Plsease Check Screen UI Manager In The Inspector Panel.", $"{screens.Count} Screens Are Scuccessfully Loaded And Initialized.");

            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackData<AppData.UIScreenViewComponent> GetScreen(AppData.UIScreenType screenType)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            AppData.Helpers.GetAppComponentsValid(screens, "App Screen List", hasScreensCallbackResults =>
            {
                callbackResults.result = hasScreensCallbackResults.result;
                callbackResults.resultCode = hasScreensCallbackResults.resultCode;

                if (callbackResults.Success())
                {
                    var screen = screens.Find(screen => screen.value.GetUIScreenType() == screenType);

                    AppData.Helpers.GetAppComponentValid(screen, screen?.name, hasScreenCallbackResults => 
                    {
                        callbackResults.result = hasScreenCallbackResults.result;
                        callbackResults.result = hasScreenCallbackResults.result;

                        if(callbackResults.Success())
                            callbackResults.data = screen;

                    }, $"Screen Of Type : {screenType} Not Fount", $"Screen : {screen.name} Of Type : {screenType} Loaded Successfully.");
                }

            }, "There Are No App Screens Initialized. Plsease Check Screen UI Manager In The Inspector Panel.", $"{screens.Count} Screens Are Successfully Loaded And Initialized.");

            return callbackResults;
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
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionButtonUIImageValue(actionType, displayerType, imageType);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        public void SetScreenActionButtonState(AppData.UIScreenType screenType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionButtonState(state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        public void SetScreenActionButtonState(AppData.UIScreenType screenType, AppData.InputActionButtonType actionType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionButtonState(actionType, state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        #endregion

        #region Input States

        public void SetScreenActionInputFieldState(AppData.UIScreenType screenType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionInputFieldState(state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        public void SetScreenActionInputFieldState(AppData.UIScreenType screenType, AppData.InputFieldActionType actionType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionInputFieldState(actionType, state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        public void SetScreenActionInputFieldPlaceHolderText(AppData.UIScreenType screenType, AppData.InputFieldActionType actionType, string placeholder)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionInputFieldPlaceHolderText(actionType, placeholder);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        #endregion

        #region Dropdown States

        public void SetScreenActionDropdownState(AppData.UIScreenType screenType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionDropdownState(state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }


        public void SetScreenActionDropdownState(AppData.UIScreenType screenType, AppData.InputUIState state, List<string> content)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionDropdownState(state, content);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        public void SetScreenActionDropdownState(AppData.UIScreenType screenType, AppData.InputDropDownActionType actionType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionDropdownState(actionType, state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        public void SetScreenActionDropdownState(AppData.UIScreenType screenType, AppData.InputDropDownActionType actionType, AppData.InputUIState state, List<string> content)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionDropdownState(actionType, state, content);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        #endregion

        #region Slider States

        public void SetScreenActionSliderState(AppData.UIScreenType screenType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionSliderState(state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        public void SetScreenActionSliderState(AppData.UIScreenType screenType, AppData.SliderValueType valueType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionSliderState(valueType, state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        #endregion

        #region Checkbox States


        public void SetScreenActionCheckboxState(AppData.UIScreenType screenType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionCheckboxState(state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
            });
        }

        public void SetScreenActionCheckboxState(AppData.UIScreenType screenType, AppData.CheckboxInputActionType actionType, AppData.InputUIState state)
        {
            OnFindScreenOfType(screenType, screenFound =>
            {
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionCheckboxState(actionType, state);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
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
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionCheckboxValue(value);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
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
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetActionCheckboxValue(actionType, value);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
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
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetUITextDisplayerValue(textType, value);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
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
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetUIImageDisplayerValue(displayerType, screenCaptureData, dataPackets);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
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
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetUIImageDisplayerValue(displayerType, imageData);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
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
                if (AppData.Helpers.IsSuccessCode(screenFound.resultCode))
                    screenFound.data.value.SetUIImageDisplayerValue(displayerType, image);
                else
                    Debug.LogWarning($"--> OnFindScreenOfType Failed With Results : {screenFound.result}");
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
                    callbackResults.result = $"Screen Found.";
                    callbackResults.data = screen;
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.result = $"OnFindScreenOfType Failed : Screen Of Type : {screenType} Not Found - Value Missing.";
                    callbackResults.data = default;
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.result = "OnFindScreenOfType Failed : Screens Are Missing / Null.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
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
