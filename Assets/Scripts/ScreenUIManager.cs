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

        void OnAppInit(Action<AppData.CallbackDataList<AppData.UIScreenViewComponent>> callback = null)
        {
            AppData.CallbackDataList<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackDataList<AppData.UIScreenViewComponent>();

            if (!HasRequiredComponentsAssigned())
            {
                List<UIScreenHandler> screenComponents = GetComponentsInChildren<UIScreenHandler>().ToList();

                AppData.Helpers.ListComponentHasData(screenComponents, hasDataCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(hasDataCallback.resultsCode))
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
                            if (compareDataCallback.Success())
                            {
                                callbackResults.results = $"{compareDataCallback.size} Screen(s) Has Been Initialized Successfully.";
                                callbackResults.data = compareDataCallback.tuple_A;
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;

                                foreach (var screenView in callbackResults.data)
                                {
                                    screenView.value.Init(screenInitializationCallback =>
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
                                    //SetCurrentScreenData(GetScreenData(AppManager.Instance.GetInitialLoadDataPackets()));
                                    SetScreensInitialized(callbackResults.Success(), callbackResults.results);
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

            Log(callbackResults.resultsCode, callbackResults.results, this);

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
                AppData.UIScreenViewComponent screen = (first) ? GetScreens()[0] : GetScreens().FindLast(screen => screen.value);

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
                                    Log(selectionSystemCallback.resultsCode, selectionSystemCallback.results, this);
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

            OnAppInit(initializationCallbackResults =>
            {
                callbackResults.results = initializationCallbackResults.results;
                callbackResults.resultsCode = initializationCallbackResults.resultsCode;

                if (callbackResults.Success())
                {
                    AppData.LogInfoType bootScreenCallbackResultsCode = (bootScreenType != AppData.UIScreenType.None) ? AppData.Helpers.SuccessCode : AppData.Helpers.WarningCode;
                    string bootScreenCallbackResults = (bootScreenCallbackResultsCode == AppData.LogInfoType.Success) ? $"On App Boot State - Initial Boot Screen : {bootScreenType}" : $"On App Boot Failed : Boot Screen Type Is Not Yet Initialized / Set Tpo Default : {bootScreenType}.";

                    callbackResults.results = bootScreenCallbackResults;
                    callbackResults.resultsCode = bootScreenCallbackResultsCode;

                    if (callbackResults.Success())
                    {
                        var bootScreen = initializationCallbackResults.data.Find(screen => screen.value.GetUIScreenType() == bootScreenType);

                        AppData.Helpers.GetAppComponentValid(bootScreen, bootScreen.name, bootScreenValidCallbackResults =>
                        {
                            callbackResults.results = bootScreenValidCallbackResults.results;
                            callbackResults.resultsCode = bootScreenValidCallbackResults.resultsCode;

                            if (callbackResults.Success())
                            {
                                AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name, hasAssetsManagerCallbackResults =>
                                {
                                    callbackResults.results = hasAssetsManagerCallbackResults.results;
                                    callbackResults.resultsCode = hasAssetsManagerCallbackResults.resultsCode;

                                    if (callbackResults.Success())
                                    {
                                        SceneAssetsManager.Instance.GetDataPacketsLibrary().GetDataPacket(bootScreenType, bootScreenDataPacketsCallbackResults =>
                                        {
                                            callbackResults.results = bootScreenDataPacketsCallbackResults.results;
                                            callbackResults.resultsCode = bootScreenDataPacketsCallbackResults.resultsCode;

                                            if (callbackResults.Success())
                                            {
                                                callbackResults.data = bootScreenDataPacketsCallbackResults.data.dataPackets;
                                                callbackResults.results = $"App Initial Boot Screen : {bootScreenType} Data Packets Have Been Loaded Successfully - Show Boot Screen : {callbackResults.data.screenType} Now.";
                                            }
                                            else
                                                callbackResults.results = $"Failed To Load App Initial Boot Screen : {bootScreenType}'s Data Packets - Results : {callbackResults.results}";
                                        });
                                    }
                                    else
                                        callbackResults.results = $"On App Boot Screen Failed With Results : {callbackResults.results}.";

                                }, $"On App Boot Screen Failed - Scene Assets Manager Instance Is Not Yet Initialized.");
                            }
                            else
                                callbackResults.results = $"On App Boot Screen Failed With Results : {callbackResults.results}.";

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
                        AppData.ActionEvents.OnScreenExitEvent(GetCurrentUIScreenType());

                        callbackResults.data.value.SetScreenData(dataPackets);
                        SetCurrentScreenData(callbackResults.data);

                        OnCheckIfScreenLoadedAsync(dataPackets, screenLoadedCallbackResults =>
                        {
                            callbackResults.results = screenLoadedCallbackResults.results;
                            callbackResults.resultsCode = screenLoadedCallbackResults.resultsCode;

                            if (screenLoadedCallbackResults.data != null)
                            {
                                callbackResults.results = $"Screen : {dataPackets.screenType} Has Been Loaded Successfully.";
                                callbackResults.data = screenLoadedCallbackResults.data;
                            }
                            else
                            {
                                callbackResults.results = $"Failed To Load Screen : {dataPackets.screenType}";
                                callbackResults.data = default;
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }

                            if (callbackResults.Success())
                                callbackResults.data.value.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, callbackResults.data.value.GetScreenTitle());
                            else
                                Log(callbackResults.resultsCode, callbackResults.results, this);
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
                    Log(callbackResults.resultsCode, callbackResults.results, this);
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

        void CachePreviousScreenData(AppData.SceneDataPackets screenDataPackets) => previousScreenData = screenDataPackets;

        AppData.CallbackData<AppData.SceneDataPackets> GetPreviousCachedScreenData()
        {
            AppData.CallbackData<AppData.SceneDataPackets> callbackResults = new AppData.CallbackData<AppData.SceneDataPackets>();

            if(previousScreenData != null)
            {
                callbackResults.results = "Previous Screen : Data Loaded Successfully.";
                callbackResults.data = previousScreenData;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Previous Screen Data Not Initialized Yet - Get Previous Loaded Screen Data Failed.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            return callbackResults;
        }

        void OnCheckIfScreenLoadedAsync(AppData.SceneDataPackets dataPackets, Action<AppData.CallbackData<AppData.UIScreenViewComponent>> callback = null)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            if (GetScreenData(dataPackets) != null && GetScreenData(dataPackets).value != null && dataPackets.screenType == GetCurrentUIScreenType())
            {
                callbackResults.results = $"Screen Of Type : {GetCurrentUIScreenType()} Loaded Successfully.";
                callbackResults.data = GetScreenData(dataPackets);
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = $"Screen : {dataPackets.screenType} Failed To Load.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
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
                        case AppData.UIScreenType.ProjectViewScreen:

                            if(SceneAssetsManager.Instance.GetWidgetsContentCount() == 0)
                                screen.value.ShowWidget(AppData.WidgetType.LoadingWidget);

                            SceneAssetsManager.Instance.GetFolderContentCount(SceneAssetsManager.Instance.GetCurrentFolder(), folderFound =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(folderFound.resultsCode))
                                    SceneAssetsManager.Instance.DisableUIOnScreenEnter(screen.value.GetUIScreenType());
                            });

                            if (SceneAssetsManager.Instance.GetProjectStructureData().Success())
                            {
                                if (SceneAssetsManager.Instance.GetProjectStructureData().data.GetLayoutViewType() == AppData.LayoutViewType.ItemView)
                                {
                                    screen.value.SetActionButtonUIImageValue(AppData.InputActionButtonType.LayoutViewButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ListViewIcon, setUIStateCallback =>
                                    {
                                        callbackResults.results = setUIStateCallback.results;
                                        callbackResults.resultsCode = setUIStateCallback.resultsCode;
                                    });
                                }

                                if (SceneAssetsManager.Instance.GetProjectStructureData().data.GetLayoutViewType() == AppData.LayoutViewType.ListView)
                                {
                                    screen.value.SetActionButtonUIImageValue(AppData.InputActionButtonType.LayoutViewButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ItemViewIcon, setUIStateCallback =>
                                    {
                                        callbackResults.results = setUIStateCallback.results;
                                        callbackResults.resultsCode = setUIStateCallback.resultsCode;
                                    });
                                }
                            }
                            else
                                Log(SceneAssetsManager.Instance.GetProjectStructureData().resultsCode, SceneAssetsManager.Instance.GetProjectStructureData().results, this);

                            if (SceneAssetsManager.Instance.GetProjectStructureData().Success())
                                screen.value.SetUITextDisplayerValue(AppData.ScreenTextType.NavigationRootTitleDisplayer, SceneAssetsManager.Instance.GetProjectStructureData().data.GetRootFolder().name);
                            else
                                Log(SceneAssetsManager.Instance.GetProjectStructureData().resultsCode, SceneAssetsManager.Instance.GetProjectStructureData().results, this);

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

                        case AppData.UIScreenType.ContentImportExportScreen:

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
                                        case AppData.UIScreenType.ProjectSelectionScreen:

                                            break;

                                        case AppData.UIScreenType.ProjectViewScreen:

                                            SceneAssetsManager.Instance.GetDynamicWidgetsContainer(AppData.ContentContainerType.FolderStuctureContent, widgetsContentContainer =>
                                            {
                                                if (AppData.Helpers.IsSuccessCode(widgetsContentContainer.resultsCode))
                                                {
                                                    LogError("Check Here -Has Something To Do Regarding Ambushed Selection Data.", this);

                                                    //widgetsContentContainer.data.DeselectAllContentWidgets();

                                                    //widgetsContentContainer.data.ClearAllFocusedWidgetInfo();
                                                }
                                                else
                                                    LogError(widgetsContentContainer.results, this, () => ScreenRefresh());
                                            });

                                            break;
                                    }
                                }
                                else
                                    Log(hasContentCallbackResults.resultsCode, hasContentCallbackResults.results, this);
                            });
                        }
                        else
                            Log(SceneAssetsManager.Instance.GetRootFolder(HasCurrentScreen().data.value.GetUIScreenType()).resultsCode, SceneAssetsManager.Instance.GetRootFolder(HasCurrentScreen().data.value.GetUIScreenType()).results, this);
                    }
                    else
                        Log(HasCurrentScreen().resultsCode, HasCurrentScreen().results, this);
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
                    if (SceneAssetsManager.Instance.GetProjectRootStructureData().Success())
                    {
                        if (SceneAssetsManager.Instance.GetProjectStructureData().Success())
                        {
                            var rootFolder = (GetCurrentUIScreenType() == AppData.UIScreenType.ProjectSelectionScreen) ? SceneAssetsManager.Instance.GetProjectRootStructureData().data.GetProjectStructureData().rootFolder : SceneAssetsManager.Instance.GetProjectStructureData().data.rootFolder;
                            var container = containerCallbackResults.data;

                            SceneAssetsManager.Instance.SetWidgetsRefreshData(rootFolder, container, dataSetupCallbackResults =>
                            {
                                if (dataSetupCallbackResults.Success())
                                {
                                    SceneAssetsManager.Instance.Init(rootFolder, container, assetsInitializedCallback =>
                                    {
                                        Log(assetsInitializedCallback.resultsCode, assetsInitializedCallback.results, this);
                                    });
                                }
                            });
                        }
                        else
                            Log(SceneAssetsManager.Instance.GetProjectStructureData().resultsCode, SceneAssetsManager.Instance.GetProjectStructureData().results, this);
                    }
                    else
                        Log(SceneAssetsManager.Instance.GetProjectRootStructureData().resultsCode, SceneAssetsManager.Instance.GetProjectRootStructureData().results, this);
                }
                else
                    Log(containerCallbackResults.resultsCode, containerCallbackResults.results, this);
            });

            var container = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

            if (container == null)
            {
                SceneAssetsManager.Instance.GetContentContainer(containerCallbackResults =>
                {
                    if (containerCallbackResults.Success())
                        container = containerCallbackResults.data;
                    else
                        Log(containerCallbackResults.resultsCode, containerCallbackResults.results, this);
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

        public bool ScreenInitialized(AppData.UIScreenViewComponent screen)
        {
            return screen?.value != null;
        }

        public AppData.CallbackData<AppData.UIScreenViewComponent> HasCurrentScreen()
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            if(currentScreen != null && currentScreen?.value != null)
            {
                callbackResults.results = $"Current Screen : {currentScreen.name} Of Type : {currentScreen.value.GetUIScreenType()} Found.";
                callbackResults.data = currentScreen;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Current Screen Not Yet Initialized / Missing / Not Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            return callbackResults;
        }

        public void HasCurrentScreen(Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if(currentScreen != null && currentScreen?.value != null)
            {
                callbackResults.results = $"Screen Data : {currentScreen.name} Loaded.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Has No Current Screen Data";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void GetCurrentScreen(Action<AppData.CallbackData<AppData.UIScreenViewComponent>> callback)
        {
            AppData.CallbackData<AppData.UIScreenViewComponent> callbackResults = new AppData.CallbackData<AppData.UIScreenViewComponent>();

            HasCurrentScreen(currentScreenCallbackResults => 
            {
                callbackResults.results = currentScreenCallbackResults.results;
                callbackResults.resultsCode = currentScreenCallbackResults.resultsCode;

                if (callbackResults.Success())
                    callbackResults.data = currentScreen;
            });

            callback?.Invoke(callbackResults);
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
