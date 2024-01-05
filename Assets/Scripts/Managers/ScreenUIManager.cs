using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ScreenUIManager : AppData.SingletonBaseComponent<ScreenUIManager>
    {
        #region Components

        [SerializeField]
        List<Screen> screens = new List<Screen>();

        [Space(5)]
        [SerializeField]
        Screen currentScreen = new Screen();

        [Space(5)]
        [SerializeField]
        RectTransform screenWidgetsContainer = null;

        AppData.SceneConfigDataPacket previousScreenData;

        float screenTransitionSpeed = 0.0f;

        AppData.ScreenLoadTransitionType transitionType = AppData.ScreenLoadTransitionType.None;
        bool clearInputsOnTransition;
        bool screensInitialized = false;

        Vector2 targetScreenPoint = Vector2.zero;

        Coroutine pageRefreshRoutine;

        #region Manager References

        AppDatabaseManager appDatabaseManagerInstance;

        #endregion

        #endregion

        #region Unity Callbacks

        void Update() => OnScreenTransition();

        #endregion

        #region Main

        public async Task<AppData.CallbackDataList<Screen>> OnScreenInitAsync()
        {
            var callbackResults = new AppData.CallbackDataList<Screen>(
                AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, 
                "App Database Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).GetData();

                callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                if (callbackResults.Success())
                {
                    var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                    var awaitLoadedScreensTaskCallbackResults = await assetBundlesLibrary.OnAwaitAssetsInitialization(AppData.AssetBundleResourceLocatorType.Screen);

                    callbackResults.SetResult(awaitLoadedScreensTaskCallbackResults);

                    if(callbackResults.Success())
                    {
                        await Task.Delay(1000);

                        callbackResults.SetResult(assetBundlesLibrary.GetLoadedScreens());

                        if (callbackResults.Success())
                        {
                            var loadedScreens = assetBundlesLibrary.GetLoadedScreens().GetData();

                            for (int i = 0; i < loadedScreens.Count; i++)
                            {
                                var screenComponent = Instantiate(loadedScreens[i].gameObject).GetComponent<Screen>();
                                screenComponent.gameObject.SetName(loadedScreens[i].GetName());

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(screenComponent, "Screen Component", $"On Screen Init Async Failed - Screen Component Not Found From Instantiated Object For Screen : {loadedScreens[i].GetName()} - Of Type : {loadedScreens[i].GetType().GetData()} - Invalid Operation, Please Check Here."));

                                if(callbackResults.Success())
                                {
                                    screenComponent.Initilize(initializationCallbackResults =>
                                    {
                                        callbackResults.SetResult(initializationCallbackResults);

                                        if (callbackResults.Success())
                                        {
                                            AddScreen(screenComponent, screenAddCallback =>
                                            {
                                                callbackResults.SetDataResults(screenAddCallback);

                                                if(callbackResults.Success())
                                                {
                                                    callbackResults.SetResult(screenComponent.GetDynamicContainerList());

                                                    if (callbackResults.Success())
                                                    {
                                                        assetBundlesLibrary.AddLoadedDynamicContainersToLibrary(containerAddedCallbackResults =>
                                                        {
                                                            callbackResults.SetResult(containerAddedCallbackResults);

                                                        }, AppData.Helpers.GetArray(screenComponent.GetDynamicContainerList().GetData()));
                                                    }
                                                }
                                            });
                                        }
                                        else
                                            Log(callbackResults.resultCode, callbackResults.result, this);
                                    });
                                }

                                if (callbackResults.UnSuccessful())
                                {
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    break;
                                }
                            }

                            SetScreensInitialized(callbackResults.Success());
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.resultCode, callbackResults.result, this);
            }
            else
                Log(callbackResults.resultCode, callbackResults.result, this);

            return callbackResults;
        }

        public void AddScreen(Screen screen, Action<AppData.CallbackDataList<Screen>> callback = null)
        {
            AppData.CallbackDataList<Screen> callbackResults = new AppData.CallbackDataList<Screen>();

            if (!screens.Contains(screen))
            {
                screens.Add(screen);

                if (screens.Contains(screen))
                {
                    callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                    if (callbackResults.Success())
                    {
                        var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                        assetBundlesLibrary.GetDynamicContainer<DynamicScreenContainer>(AppData.ScreenType.Default, AppData.ContentContainerType.AppScreenContainer, AppData.ContainerViewSpaceType.Screen, containerCallbackResults =>
                        {
                            callbackResults.SetResult(containerCallbackResults);

                            if (callbackResults.Success())
                            {
                                var screenContainer = containerCallbackResults.GetData();

                                callbackResults.SetResult(screen.GetInitialVisibility());

                                if (callbackResults.Success())
                                {
                                    screenContainer.AddContent<Screen, AppData.ScreenType, AppData.WidgetType>(uiScreenWidgetComponent: screen, keepWorldPosition: false, isActive: true, overrideContainerActiveState: true, updateContainer: true, screenAddedCallbackResults =>
                                    {
                                        callbackResults.SetResult(screenAddedCallbackResults);

                                        if (callbackResults.Success())
                                        {
                                            callbackResults.result = $"Screen : {screen.name} Of Type : {screen.GetType().GetData()} Has Been Added To Screen List.";
                                            callbackResults.data = screens;
                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    });
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        });
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                {
                    callbackResults.result = $"Couldn't Add Screen : {screen.name} Of Type : {screen.GetType().GetData()} For Unknown Reasons - Please Check Here.";
                    callbackResults.data = default;
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.result = $"Screen : {screen.name} Of Type : {screen.GetType().GetData()} Already Exists In The Screen List.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackData<AppData.ScreenType> GetCurrentScreenType()
        {
            var callbackResults = new AppData.CallbackData<AppData.ScreenType>(HasCurrentScreen());

            if (callbackResults.Success())
                callbackResults.data = currentScreen.GetType().GetData();
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.Callback ScreensInitialized()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(screens, "Screens", "Screen Have Not Been Initialized Yet."));

            if (callbackResults.Success())
            {
                if (screensInitialized)
                    callbackResults.result = $"Screens Have Been Initialized Successfully With Results : {callbackResults.result}.";
                else
                {
                    callbackResults.result = "Screens Are Not Yet Initialized.";
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }

            return callbackResults;
        }

        private void SetScreensInitialized(bool isInitialized) => screensInitialized = isInitialized;

        public bool IsInitialized() => screensInitialized;

        void OnScreenTransition()
        {
            //if (transitionType == AppData.ScreenLoadTransitionType.Translate)
            //{
            //    if (ScreensInitialized())
            //    {
            //        Vector2 screenPoint = screenWidgetsContainer.anchoredPosition;
            //        screenPoint = Vector2.Lerp(screenPoint, targetScreenPoint, screenTransitionSpeed * Time.smoothDeltaTime);

            //        screenWidgetsContainer.anchoredPosition = screenPoint;
            //        float distance = (screenWidgetsContainer.anchoredPosition - targetScreenPoint).sqrMagnitude;

            //        if (distance <= 0.1f)
            //        {
            //            AppData.ActionEvents.OnScreenChangedEvent(currentScreen.value.GetUIScreenType());
            //            transitionType = AppData.ScreenLoadTransitionType.None;

            //            if (clearInputsOnTransition)
            //            {
            //                SelectableManager.Instance.GetProjectStructureSelectionSystem(selectionSystemCallback =>
            //                {
            //                    if (selectionSystemCallback.Success())
            //                        selectionSystemCallback.data.OnClearInputSelection();
            //                    else
            //                        Log(selectionSystemCallback.resultCode, selectionSystemCallback.result, this);
            //                });
            //            }
            //        }
            //    }
            //    else
            //        LogWarning($"Couldn't Transition Screen : {currentScreen?.name} Of Type : {currentScreen?.value?.GetUIScreenType()} - Possible Issue - Screens Are Missing / Not Found.", this, () => OnScreenTransition());
            //}
        }

        public void ShowNewAssetScreen(AppData.SceneConfigDataPacket dataPackets)
        {
            try
            {
                if (ScreensInitialized().Success())
                {
                    if (AppDatabaseManager.Instance)
                    {
                        foreach (var screen in screens)
                        {
                            if (screen.GetType().GetData() == dataPackets.GetReferencedScreenType().GetData().GetValue().GetData())
                            {
                                //if (assetsManager == null)
                                //    assetsManager = SceneAssetsManager.Instance;

                                AppData.SceneAsset sceneAsset = new AppData.SceneAsset();
                                //sceneAsset.name = "Create New Asset";

                                AppData.AssetInfoHandler info = new AppData.AssetInfoHandler();

                                info.fields = new List<AppData.AssetInfoField>();

                                // Title Info
                                AppData.AssetInfoField titleInfoField = new AppData.AssetInfoField();
                                titleInfoField.name = AppDatabaseManager.Instance.GetDefaultAssetName();
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
                                AppDatabaseManager.Instance.SetCurrentSceneAsset(sceneAsset);
                                AppDatabaseManager.Instance.UpdateCurrentSceneAsset(sceneAsset);

                                //if (SceneAssetsManager.Instance.GetCurrentSceneAsset().info.fields != null)
                                //    screen.value.DisplaySceneAssetInfo(SceneAssetsManager.Instance.GetCurrentSceneAsset());
                                //else
                                //    Debug.LogWarning("--> Info Fields Not Initialized.");

                                //AppData.ActionEvents.OnClearPreviewedSceneAssetObjectEvent();


                                //screen.value.DisplaySceneAssetInfo(sceneAsset);

                                //screen.value.ShowViewAsync();

                                currentScreen = screen;

                                //if (currentScreen.value != null)
                                //    currentScreen.value.SetScreenData(dataPackets);
                                //else
                                //    Debug.LogWarning("--> Unity - Current Screen Value Is Null.");

                                AppData.ActionEvents.OnScreenChangeEvent(dataPackets);

                                if (dataPackets.screenTransition == AppData.ScreenLoadTransitionType.Default)
                                {
                                    if (screenWidgetsContainer != null)
                                    {
                                        targetScreenPoint = screen.GetScreenPosition();
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
                        Log(ScreensInitialized().GetResultCode, ScreensInitialized().GetResult, this);
                }
                else
                    LogWarning($"Couldn't Show New Asset Screen Of Type : {dataPackets.referencedScreenType} - Possible Issue - Screens Are Missing / Not Found.", this, () => ShowNewAssetScreen(dataPackets));
            }
            catch (Exception exception)
            {
                throw new Exception($"--> Unity - Failed To Show New Asset Screen  - With Exception : {exception}");
            }
        }

        #region On Show Screen Async

        public async Task<AppData.CallbackData<Screen>> ShowScreenAsync(AppData.ScreenConfigDataPacket dataPacket) { return await OnTriggerShowScreenAsync(dataPacket); }

        async Task<AppData.CallbackData<Screen>> OnTriggerShowScreenAsync(AppData.ScreenConfigDataPacket dataPacket)
        {
            return await OnShowScreenAsync(dataPacket);
        }

        async Task<AppData.CallbackData<Screen>> OnShowScreenAsync(AppData.ScreenConfigDataPacket dataPacket)
        {
            AppData.CallbackData<Screen> callbackResults = new AppData.CallbackData<Screen>(ScreensInitialized());

            if (callbackResults.Success())
            {
                GetScreen(dataPacket.GetType().GetData(), screenFoundCallback =>
                {
                    callbackResults = screenFoundCallback;

                    if (callbackResults.Success())
                    {
                        if(GetCurrentScreenType().GetData() != AppData.ScreenType.None && GetCurrentScreenType().GetData() != AppData.ScreenType.SplashScreen && GetCurrentScreenType().GetData() != AppData.ScreenType.LoadingScreen)
                             AppData.ActionEvents.OnScreenExitEvent(GetCurrentScreenType().GetData());

                        //callbackResults.GetData().GetValue().GetData().SetScreenData(dataPackets);
                        SetCurrentScreen(callbackResults.GetData());

                        OnCheckIfScreenLoadedAsync(dataPacket, screenLoadedCallbackResults =>
                        {
                            callbackResults.result = screenLoadedCallbackResults.result;
                            callbackResults.resultCode = screenLoadedCallbackResults.resultCode;

                            if (screenLoadedCallbackResults.data != null)
                            {
                                callbackResults.result = $"Screen : {dataPacket.GetType().GetData()} Has Been Loaded Successfully.";
                                callbackResults.data = screenLoadedCallbackResults.data;
                            }
                            else
                            {
                                callbackResults.result = $"Failed To Load Screen : {dataPacket.GetType().GetData()}";
                                callbackResults.data = default;
                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                            }

                            //if (callbackResults.Success())
                            //{
                            //    if (GetCurrentUIScreenType() != AppData.UIScreenType.None && GetCurrentUIScreenType() != AppData.UIScreenType.SplashScreen && GetCurrentUIScreenType() != AppData.UIScreenType.LoadingScreen)
                            //        callbackResults.data.value.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, callbackResults.data.value.GetScreenTitle());
                            //}
                            //else
                            //    Log(callbackResults.resultCode, callbackResults.result, this);
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

                await OnShowSelectedScreenViewAsync(callbackResults.GetData());

                if (callbackResults.Success())
                {
                    LogWarning(" _______________+++++++++++ Please Fix Commented Out Code.", this);

                    //callbackResults.SetResult(callbackResults.data.value.GetDataPackets());

                    //if(callbackResults.Success())
                    //    AppData.ActionEvents.OnScreenChangeEvent(callbackResults.data.value.GetDataPackets().GetData());
                    //else
                    //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }

            return callbackResults;

        }

        public async Task<AppData.CallbackData<Screen>> OnShowSelectedScreenViewAsync(Screen screen)
        {
            return await screen.ShowViewAsync();
        }

        #endregion

        #region On Hide Screen Async

        public async Task<AppData.CallbackData<Screen>> HideScreenAsync(AppData.ScreenConfigDataPacket dataPacket) { return await OnTriggerHideScreenAsync(dataPacket); }

        async Task<AppData.CallbackData<Screen>> OnTriggerHideScreenAsync(AppData.ScreenConfigDataPacket dataPacket)
        {
            return await OnHideScreenAsync(dataPacket);
        }

        async Task<AppData.CallbackData<Screen>> OnHideScreenAsync(AppData.ScreenConfigDataPacket dataPacket)
        {
            AppData.CallbackData<Screen> callbackResults = new AppData.CallbackData<Screen>();

            GetScreen(dataPacket, screenFoundCallback =>
            {
                callbackResults = screenFoundCallback;
            });

            await OnHideSelectedScreenViewAsync(callbackResults.data);

            return callbackResults;
        }

        public async Task<AppData.CallbackData<Screen>> OnHideSelectedScreenViewAsync(Screen screen)
        {
            return await screen.HideViewSync();
        }

        #endregion

        #region Go To Screen

        public async Task GoToSelectedScreenAsync(AppData.ScreenConfigDataPacket dataPacket, Action<AppData.CallbackData<AppData.ScreenConfigDataPacket>> callback = null)
        {
            try
            {
                AppData.CallbackData<AppData.ScreenConfigDataPacket> callbackResults = new AppData.CallbackData<AppData.ScreenConfigDataPacket>(dataPacket.Initialized());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(ScreenOfTypeExists(dataPacket.GetType().GetData()));

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Scene Assets Manager Instance Is Not Yet Initialized."));

                        if (callbackResults.Success())
                        {
                            var sceneAssetsManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).GetData();

                            callbackResults.SetResults(sceneAssetsManager.GetScreenLoadInfoInstanceFromLibrary(dataPacket.GetType().GetData()));

                            if (callbackResults.Success())
                            {
                                var screenLoadInfo = sceneAssetsManager.GetScreenLoadInfoInstanceFromLibrary(dataPacket.GetType().GetData()).GetData();

                                callbackResults.SetResult(dataPacket.GetScreenLoadTransitionType());

                                if (callbackResults.Success())
                                {
                                    #region Load Screen

                                    if (dataPacket.GetScreenLoadTransitionType().GetData() == AppData.ScreenLoadTransitionType.LoadingScreen)
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

                                    if (dataPacket.GetScreenLoadTransitionType().GetData() == AppData.ScreenLoadTransitionType.Translate)
                                    {

                                    }

                                    #endregion
                                }
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

        public AppData.Callback ScreenOfTypeExists(AppData.ScreenType screenType)
        {
            AppData.Callback callbackResults = new AppData.Callback(GetScreenScreenOfType(screenType));
            return callbackResults;
        }

        void CachePreviousScreenData(AppData.SceneConfigDataPacket screenDataPackets) => previousScreenData = screenDataPackets;

        AppData.CallbackData<AppData.SceneConfigDataPacket> GetPreviousCachedScreenData()
        {
            AppData.CallbackData<AppData.SceneConfigDataPacket> callbackResults = new AppData.CallbackData<AppData.SceneConfigDataPacket>();

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

        void OnCheckIfScreenLoadedAsync(AppData.ScreenConfigDataPacket dataPacket, Action<AppData.CallbackData<Screen>> callback = null)
        {
            AppData.CallbackData<Screen> callbackResults = new AppData.CallbackData<Screen>(GetScreenScreenOfType(dataPacket.GetType().GetData()));

            if (dataPacket.GetType().GetData() == GetCurrentScreenType().GetData())
            {
                callbackResults.result = $"Screen Of Type : {GetCurrentScreenType()} Loaded Successfully.";
                callbackResults.data = GetScreenScreenOfType(dataPacket.GetType().GetData()).GetData();
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Screen : {dataPacket.GetType().GetData()} Failed To Load.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        void OnUpdateUIScreenOnEnter(Screen screen, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback(ScreensInitialized());

            if (callbackResults.Success())
            {
                if (AppDatabaseManager.Instance != null)
                {
                    switch(screen.GetType().GetData())
                    {
                        case AppData.ScreenType.ProjectDashboardScreen:

                            if(AppDatabaseManager.Instance.GetWidgetsContentCount() == 0)
                                screen.ShowWidget(AppData.WidgetType.LoadingWidget, showLoadingWidgetCallbackResults => { });

                            AppDatabaseManager.Instance.GetFolderContentCount(AppDatabaseManager.Instance.GetCurrentFolder(), folderFound =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(folderFound.resultCode))
                                    AppDatabaseManager.Instance.DisableUIOnScreenEnter(screen.GetType().GetData());
                            });

                            if (AppDatabaseManager.Instance.GetProjectStructureData().Success())
                            {
                                if (AppDatabaseManager.Instance.GetProjectStructureData().GetData().GetLayoutViewType() == AppData.LayoutViewType.ItemView)
                                {
                                    //screen.value.SetActionButtonUIImageValue(AppData.InputActionButtonType.LayoutViewButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ListViewIcon, setUIStateCallback =>
                                    //{
                                    //    callbackResults.result = setUIStateCallback.result;
                                    //    callbackResults.resultCode = setUIStateCallback.resultCode;
                                    //});
                                }

                                if (AppDatabaseManager.Instance.GetProjectStructureData().GetData().GetLayoutViewType() == AppData.LayoutViewType.ListView)
                                {
                                    //screen.value.SetActionButtonUIImageValue(AppData.InputActionButtonType.LayoutViewButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ItemViewIcon, setUIStateCallback =>
                                    //{
                                    //    callbackResults.result = setUIStateCallback.result;
                                    //    callbackResults.resultCode = setUIStateCallback.resultCode;
                                    //});
                                }
                            }
                            else
                                Log(AppDatabaseManager.Instance.GetProjectStructureData().resultCode, AppDatabaseManager.Instance.GetProjectStructureData().result, this);

                            //if (AppDatabaseManager.Instance.GetProjectStructureData().Success())
                            //    screen.value.SetUITextDisplayerValue(AppData.ScreenTextType.NavigationRootTitleDisplayer, AppDatabaseManager.Instance.GetProjectStructureData().data.GetRootFolder().name);
                            //else
                            //    Log(AppDatabaseManager.Instance.GetProjectStructureData().resultCode, AppDatabaseManager.Instance.GetProjectStructureData().result, this);

                            //screen.value.SetActionButtonState(AppData.InputActionButtonType.Return, AppData.InputUIState.Hidden);

                            AppDatabaseManager.Instance.GetFolderContentCount(AppDatabaseManager.Instance.GetCurrentFolder(), folderFound =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(folderFound.resultCode))
                                {
                                    AppDatabaseManager.Instance.DisableUIOnScreenEnter(screen.GetType().GetData());

                                    callbackResults.result = $"UI Scene View Of Type {screen.GetType().GetData()} Has Been Updated - UI Disabled On Enter.";
                                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                }
                                else
                                {
                                    callbackResults.result = $"UI Scene View Of Type {screen.GetType().GetData()} Has Been Updated.";
                                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                }
                            });

                            break;

                        case AppData.ScreenType.ContentImportExportScreen:

                            screen.DisplaySceneAssetInfo(screen, displaySeneAssetInfoCallback => 
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
                callbackResults.result = $"Couldn't Update Screen : {screen.name} Of Type : {screen.GetType().GetData()} on Enter - Possible Issue - Screens Are Missing / Not Found.";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void ShowLoadingItem(AppData.LoadingItemType loadingItemType, bool status)
        {
            if (GetCurrentScreen().Success())
                GetCurrentScreen().GetData().ShowLoadingItem(loadingItemType, status);
            else
                Log(GetCurrentScreen().GetResultCode, GetCurrentScreen().GetResult, this);
        }

        #region Screen Refresh

        public async Task<AppData.Callback> RefreshAsync(int refreshDuration = 0)
        {
            AppData.Callback callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appDatabaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager Instance Is Not Yet Initialized.").GetData();

                var refreshTask = await appDatabaseManager.RefreshedAsync(currentScreen, null, null, null);
            }

            return callbackResults;
        }

        //public void ScreenRefresh()
        //{
        //    if (AppDatabaseManager.Instance != null)
        //    {
        //        if (SelectableManager.Instance != null)
        //        {
        //            if (SelectableManager.Instance.HasFocusedWidgetInfo())
        //                SelectableManager.Instance.OnClearFocusedSelectionsInfo();

        //            if (HasCurrentScreen().Success())
        //            {
        //                if (AppDatabaseManager.Instance.GetRootFolder(HasCurrentScreen().data.value.GetUIScreenType()).Success())
        //                {
        //                    AppDatabaseManager.Instance.HasContentToLoadForSelectedScreen(AppDatabaseManager.Instance.GetRootFolder(HasCurrentScreen().data.value.GetUIScreenType()).data, hasContentCallbackResults =>
        //                    {
        //                        if (hasContentCallbackResults.Success())
        //                        {
        //                            appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData().GetDynamicContainers<DynamicWidgetsContainer>(GetCurrentUIScreenType(), widgetsContentContainers =>
        //                            {
        //                                if (widgetsContentContainers.Success())
        //                                {
        //                                    LogError("Check Here -Has Something To Do Regarding Ambushed Selection Data.", this);

        //                                    foreach (var container in widgetsContentContainers.data)
        //                                        container.DeselectAllContentWidgets();
        //                                }
        //                                else
        //                                    Log(widgetsContentContainers.GetResultCode, widgetsContentContainers.GetResult, this);
        //                            });
        //                        }
        //                        else
        //                            Log(hasContentCallbackResults.resultCode, hasContentCallbackResults.result, this);
        //                    });
        //                }
        //                else
        //                    Log(AppDatabaseManager.Instance.GetRootFolder(HasCurrentScreen().data.value.GetUIScreenType()).resultCode, AppDatabaseManager.Instance.GetRootFolder(HasCurrentScreen().data.value.GetUIScreenType()).result, this);
        //            }
        //            else
        //                Log(HasCurrentScreen().resultCode, HasCurrentScreen().result, this);
        //        }
        //        else
        //            LogWarning("Refresh Button Failed : SelectableManager.Instance Is Not Yet Initialized", this);

        //        AppData.ActionEvents.OnScreenUIRefreshed();

        //        Canvas.ForceUpdateCanvases();
        //    }
        //    else
        //        LogError("Refresh Button Failed : Scene Assets Manager Instance Is Not Yet Initialized", this);
        //}

        async Task<AppData.Callback> OnScreenRefreshAsync(AppData.SceneConfigDataPacket dataPackets, int refreshDuration = 0)
        {
            AppData.Callback callbackResults = new AppData.Callback(appDatabaseManagerInstance.GetAssetBundlesLibrary());

            if (dataPackets.blurScreen)
                currentScreen.Blur(dataPackets);

            if (currentScreen != null)
                currentScreen.ShowLoadingItem(dataPackets.screenRefreshLoadingItemType, true);

            if (callbackResults.Success())
            {
                callbackResults.SetResult(dataPackets.GetReferencedScreenType());

                if (callbackResults.Success())
                {
                    switch (dataPackets.GetReferencedScreenType().GetData().GetValue().GetData())
                    {
                        case AppData.ScreenType.LandingPageScreen:

                            #region Get Content Container

                            if (dataPackets.GetScreenContainerData().GetContainerType() != AppData.ContentContainerType.None && dataPackets.GetScreenContainerData().GetContainerViewSpaceType() != AppData.ContainerViewSpaceType.None)
                            {
                                appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData().GetDynamicContainer<DynamicWidgetsContainer>(dataPackets.GetReferencedScreenType().GetData().GetValue().GetData(), dataPackets.GetScreenContainerData(), screenContainerCallbackResults =>
                                {
                                    callbackResults.SetResult(screenContainerCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        var screenContainer = screenContainerCallbackResults.GetData();

                                        #region Get Scene Content Container

                                        appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData().GetDynamicContainer<DynamicContentContainer>(dataPackets.GetReferencedScreenType().GetData().GetValue().GetData(), dataPackets.GetSceneContainerData().GetContainerType(), dataPackets.GetSceneContainerData().GetContainerViewSpaceType(), sceneContainerCallbackResults =>
                                        {
                                            callbackResults.SetResult(sceneContainerCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                #region Set Refresh Data

                                                appDatabaseManagerInstance.SetRefreshData(folder: null, screenContainer: screenContainer, sceneContainer: sceneContainerCallbackResults.GetData(), callback: dataSetupCallbackResults =>
                                                {
                                                    Log(dataSetupCallbackResults.GetResultCode, dataSetupCallbackResults.GetResult, this);
                                                });

                                                #endregion
                                            }
                                            else
                                            {
                                                #region Set Refresh Data

                                                appDatabaseManagerInstance.SetRefreshData(folder: null, screenContainer: screenContainerCallbackResults.data, sceneContainer: null, callback: dataSetupCallbackResults =>
                                                {
                                                    Log(dataSetupCallbackResults.GetResultCode, dataSetupCallbackResults.GetResult, this);
                                                });

                                                #endregion
                                            }
                                        });

                                        #endregion
                                    }
                                });
                            }

                            #endregion

                            break;

                        case AppData.ScreenType.ProjectCreationScreen:

                            #region Get Content Container

                            if (dataPackets.GetScreenContainerData().GetContainerType() != AppData.ContentContainerType.None && dataPackets.GetScreenContainerData().GetContainerViewSpaceType() != AppData.ContainerViewSpaceType.None)
                            {
                                appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData().GetDynamicContainer<DynamicWidgetsContainer>(dataPackets.GetReferencedScreenType().GetData().GetValue().GetData(), dataPackets.GetScreenContainerData(), screenContainerCallbackResults =>
                                {
                                    callbackResults.SetResult(screenContainerCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        if (appDatabaseManagerInstance.GetProjectRootStructureData().Success())
                                        {
                                            if (appDatabaseManagerInstance.GetProjectStructureData().Success())
                                            {
                                                callbackResults.SetResult(GetCurrentScreenType());

                                                if (callbackResults.Success())
                                                {
                                                    var rootFolder = (GetCurrentScreenType().GetData() == AppData.ScreenType.ProjectCreationScreen) ? appDatabaseManagerInstance.GetProjectRootStructureData().GetData().GetProjectStructureData().rootFolder : appDatabaseManagerInstance.GetProjectStructureData().GetData().rootFolder;
                                                    var container = screenContainerCallbackResults.GetData();

                                                    appDatabaseManagerInstance.SetRefreshData(rootFolder, container, null, dataSetupCallbackResults =>
                                                    {
                                                        if (dataSetupCallbackResults.Success())
                                                        {
                                                            appDatabaseManagerInstance.Init(rootFolder, container, assetsInitializedCallback =>
                                                            {
                                                                Log(assetsInitializedCallback.resultCode, assetsInitializedCallback.result, this);
                                                            });
                                                        }
                                                    });
                                                }
                                            }
                                            else
                                                Log(AppDatabaseManager.Instance.GetProjectStructureData().resultCode, AppDatabaseManager.Instance.GetProjectStructureData().result, this);
                                        }
                                        else
                                            Log(AppDatabaseManager.Instance.GetProjectRootStructureData().resultCode, AppDatabaseManager.Instance.GetProjectRootStructureData().result, this);
                                    }
                                });
                            }

                            #endregion

                            break;

                        case AppData.ScreenType.ProjectDashboardScreen:

                            break;

                        case AppData.ScreenType.ContentImportExportScreen:

                            break;
                    }

                    #region On Screen Refresh

                    var refreshTask = await appDatabaseManagerInstance.RefreshedAsync(GetCurrentScreen().GetData(), appDatabaseManagerInstance?.GetCurrentFolder(), appDatabaseManagerInstance?.GetRefreshData().screenContainer, appDatabaseManagerInstance?.GetRefreshData().sceneContainer, dataPackets, refreshDuration); // Wait For Assets To Be Refreshed.

                    if (currentScreen != null)
                        currentScreen.ShowLoadingItem(dataPackets.screenRefreshLoadingItemType, false);

                    currentScreen.Focus();

                    AppData.ActionEvents.OnScreenRefreshed(currentScreen);

                    #endregion
                }
            }

            return callbackResults;
        }

        #endregion

        public void SetCurrentScreen(Screen screenData) => currentScreen = screenData;

        public AppData.CallbackData<Screen> GetScreenScreenOfType(AppData.ScreenType screenType)
        {
            var callbackResults = new AppData.CallbackData<Screen>(AppData.Helpers.GetAppComponentsValid(screens, "Screens", "GetScreenScreenOfType Failed - There Are No Screens Initialized Yet - Invalid Operation - Please Investigate Screen UI Manager Screens List Initialization."));

            if (callbackResults.Success())
            {
                var screen = screens.Find((screen) => screen.GetType().GetData() == screenType);

                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(screen, "Screen", $"Screen Of Type : {screenType} Not Found In Screen List - Invalid Operation."));

                if(callbackResults.Success())
                {
                    callbackResults.result = $"Screen : {screen.GetName()} Of Type : {screen.GetType().GetData()} Has Been Successfully Found.";
                    callbackResults.data = screen;
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        //public Transform GetScreenWidgetsContainer(AppData.UIScreenType screenType)
        //{
        //    Transform widgetsContainer = null;

        //    if (ScreensInitialized())
        //    {
        //        foreach (var screen in screens)
        //        {
        //            if (screen.value.GetUIScreenType() == screenType)
        //            {
        //                widgetsContainer = screen.value.GetWidgetsContainer();
        //                break;
        //            }
        //        }
        //    }

        //    return widgetsContainer;
        //}

        public AppData.CallbackData<Screen> GetCurrentScreen()
        {
            var callbackResults = new AppData.CallbackData<Screen>(HasCurrentScreen());

            if(callbackResults.Success())
                callbackResults.data = currentScreen;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.Callback HasCurrentScreen()
        {
            AppData.Callback callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(currentScreen, "Current Screen", "Current Screen Missing / Not Found. Invalid Operation - Please Make Sure Current Screen Value Is AssignedOn Screen Selection / Initialization."));

            if (callbackResults.Success())
                callbackResults.result = $"Current Screen : {currentScreen.name} Of Type : {currentScreen.GetType().GetData()} Found.";
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public void GetCurrentScreen(Action<AppData.CallbackData<Screen>> callback)
        {
            AppData.CallbackData<Screen> callbackResults = new AppData.CallbackData<Screen>(HasCurrentScreen());

            if (callbackResults.Success())
                callbackResults.data = currentScreen;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void GetScreen(AppData.ScreenType screenType, Action<AppData.CallbackData<Screen>> callback)
        {
            AppData.CallbackData<Screen> callbackResults = new AppData.CallbackData<Screen>(GetScreenScreenOfType(screenType));

            if (callbackResults.Success())
                callbackResults.data = GetScreenScreenOfType(screenType).GetData();
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback.Invoke(callbackResults);
        }

        public void GetScreen(AppData.ScreenConfigDataPacket dataPacket, Action<AppData.CallbackData<Screen>> callback)
        {
            AppData.CallbackData<Screen> callbackResults = new AppData.CallbackData<Screen>(GetScreenScreenOfType(dataPacket.GetType().GetData()));

            if (callbackResults.Success())
                callbackResults.data = GetScreenScreenOfType(dataPacket.GetType().GetData()).GetData();
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback.Invoke(callbackResults);
        }

        public AppData.CallbackData<Screen> GetScreen(AppData.ScreenType screenType)
        {
            AppData.CallbackData<Screen> callbackResults = new AppData.CallbackData<Screen>(GetScreenScreenOfType(screenType));

            AppData.Helpers.GetAppComponentsValid(screens, "App Screen List", hasScreensCallbackResults =>
            {
                callbackResults.result = hasScreensCallbackResults.result;
                callbackResults.resultCode = hasScreensCallbackResults.resultCode;

                if (callbackResults.Success())
                {
                    var screen = screens.Find(screen => screen.GetType().GetData() == screenType);

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

        public void SetScreenSleepTime(bool neverSleep)
        {
            if (neverSleep)
                UnityEngine.Screen.sleepTimeout = SleepTimeout.NeverSleep;
            else
                UnityEngine.Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

        public AppData.CallbackDataList<Screen> GetScreens()
        {
            var callbackResults = new AppData.CallbackDataList<Screen>(AppData.Helpers.GetAppComponentsValid(screens, "Screens", "Get Screens Failed, Screens Are Not Initialized Yet - Invalid Operation"));

            if (callbackResults.Success())
            {
                callbackResults.result = $"{screens.Count} : Screens Found.";
                callbackResults.data = screens;
            }
            else
                Log(callbackResults.resultCode, callbackResults.result, this);

            return callbackResults;
        }

        #region Screen UI Widgets Creation

        public async Task<AppData.CallbackDataList<AppData.Post>> CreateUIScreenPostWidgetAsync(AppData.ScreenType screenType, List<AppData.Post> posts, DynamicWidgetsContainer screenContentContainer)
        {
            try
            {
                AppData.CallbackDataList<AppData.Post> callbackResults = new AppData.CallbackDataList<AppData.Post>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    if (screenType == GetCurrentScreenType().GetData())
                    {
                        var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                        if (posts != null && posts.Count > 0)
                        {
                            if (screenContentContainer != null && screenContentContainer.GetActive().Success())
                            {
                                callbackResults.SetResult(databaseManager.GetWidgetsPrefabDataLibrary(screenType));

                                if (callbackResults.Success())
                                {
                                    var widgetsLibraryData = databaseManager.GetWidgetsPrefabDataLibrary(screenType).data;

                                    var widgetPrefabData = widgetsLibraryData.Find(x => x.screenType == screenType);

                                    if (widgetPrefabData != null)
                                    {
                                        callbackResults.SetResult(widgetPrefabData.GetUIScreenWidgetData(screenContentContainer.GetSelectableWidgetType(), screenContentContainer.GetLayout().viewType));

                                        if (callbackResults.Success())
                                        {
                                            var prefabData = widgetPrefabData.GetUIScreenWidgetData(screenContentContainer.GetSelectableWidgetType(), screenContentContainer.GetLayout().viewType).data;
                                            var prefab = prefabData.gameObject;

                                            callbackResults.SetResult(AppData.Helpers.UnityComponentValid(prefab, "Post Widget Prefab Value"));

                                            if (callbackResults.Success())
                                            {
                                                var widget = AppData.Helpers.UnityComponentValid(prefab, "Post Widget Prefab Value").data;

                                                List<AppData.Post> postDatas = new List<AppData.Post>();

                                                callbackResults.SetResult(AppData.Helpers.ListComponentHasEqualDataSize(postDatas, posts));

                                                foreach (var post in posts)
                                                {
                                                    GameObject postWidget = Instantiate(widget);

                                                    if (postWidget != null)
                                                    {
                                                        AppData.UIScreenWidget widgetComponent = postWidget.GetComponent<AppData.UIScreenWidget>();

                                                        if (widgetComponent != null)
                                                        {
                                                            widgetComponent.SetPost(post);

                                                            postWidget.name = post.GetIdentifier();

                                                            var addContentAsyncTaskResults = await screenContentContainer.AddContentAsync(content: widgetComponent, keepWorldPosition: false, updateContainer: true);

                                                            callbackResults.SetResult(addContentAsyncTaskResults);

                                                            postDatas.Add(post);

                                                            callbackResults.result = $"Post Widget : { postWidget.name} Created.";
                                                        }
                                                        else
                                                        {
                                                            callbackResults.result = "Post Widget Component Is Null.";
                                                            callbackResults.data = default;
                                                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        callbackResults.result = "Post Widget Prefab Data Is Null.";
                                                        callbackResults.data = default;
                                                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                    }

                                                    await Task.Yield();
                                                }

                                                while (screenContentContainer.GetContentCount().data != posts.Count)
                                                    await Task.Yield();

                                                callbackResults.result = "Posts Widgets Loaded.";
                                                callbackResults.data = postDatas;
                                                callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        callbackResults.result = $"Widget Prefab For Screen Type : {screenType} Missing / Null.";
                                        callbackResults.data = default;
                                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                    }
                                }
                                else
                                    Log(callbackResults.resultCode, callbackResults.result, this);

                                LogInfo($" ================++++++ Widgets Loading Completed With Results : {callbackResults.GetResult}.", this);
                            }
                            else
                            {
                                callbackResults.result = "Dynamic Widgets Content Container Missing / Null.";
                                callbackResults.data = default;
                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                            }
                        }
                        else
                        {
                            callbackResults.result = "No Posts Found To Create Widgets - Posts Missing / Null.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.WarningCode;
                        }
                    }
                    else
                    {
                        callbackResults.result = $"Current Screen Type : {GetCurrentScreenType()} Dosen't Match Requested Screen Type : {screenType} - Operation Is Not Valid.";
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.WarningCode;
                    }
                }

                return callbackResults;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public async Task<AppData.CallbackDataList<AppData.Project>> CreateUIScreenProjectSelectionWidgetsAsync(AppData.ScreenType screenType, List<AppData.ProjectStructureData> projectData, DynamicWidgetsContainer screenContentContainer)
        {
            try
            {
                AppData.CallbackDataList<AppData.Project> callbackResults = new AppData.CallbackDataList<AppData.Project>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                    if (screenContentContainer != null && screenContentContainer.GetActive().Success())
                    {
                        if (screenType == GetCurrentScreenType().GetData())
                        {
                            bool loadTaskIsRunning = true;

                            while (loadTaskIsRunning)
                            {
                                databaseManager.GetSortedProjectWidgetList(projectData, sortedListCallbackResults =>
                                {
                                    callbackResults.result = sortedListCallbackResults.result;
                                    callbackResults.resultCode = sortedListCallbackResults.resultCode;

                                    if (callbackResults.Success())
                                    {
                                        databaseManager.GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                                        {
                                            callbackResults.result = widgetsCallback.result;
                                            callbackResults.resultCode = widgetsCallback.resultCode;

                                            if (callbackResults.Success())
                                            {
                                                var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                                if (widgetPrefabData != null)
                                                {
                                                    widgetPrefabData.GetUIScreenWidgetData(screenContentContainer.GetSelectableWidgetType(), screenContentContainer.GetLayout().viewType, prefabCallbackResults =>
                                                    {
                                                        callbackResults.result = prefabCallbackResults.result;
                                                        callbackResults.resultCode = prefabCallbackResults.resultCode;

                                                        if (prefabCallbackResults.Success())
                                                        {
                                                            AppData.Helpers.UnityComponentValid(prefabCallbackResults.data.gameObject, "Project Widget Prefab Value", hasComponentCallbackResults =>
                                                            {
                                                                callbackResults.result = hasComponentCallbackResults.result;
                                                                callbackResults.resultCode = hasComponentCallbackResults.resultCode;

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
                                                                                widgetComponent.SetProjectData(project);

                                                                                projectWidget.name = project.name;
                                                                                screenContentContainer.AddContent(content: widgetComponent, keepWorldPosition: false, overrideActiveState: false, updateContainer: true);

                                                                                AppData.Project projectData = new AppData.Project
                                                                                {
                                                                                    name = project.name,
                                                                                    widget = widgetComponent,
                                                                                    structureData = project
                                                                                };

                                                                                projects.Add(projectData);

                                                                                callbackResults.result = $"Project Widget : { projectWidget.name} Created.";
                                                                                callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                                            }
                                                                            else
                                                                            {
                                                                                callbackResults.result = "Project Widget Component Is Null.";
                                                                                callbackResults.data = default;
                                                                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            callbackResults.result = "Project Widget Prefab Data Is Null.";
                                                                            callbackResults.data = default;
                                                                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                                        }
                                                                    }

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        loadTaskIsRunning = false;

                                                                        callbackResults.result = "Project Widgets Loaded.";
                                                                        callbackResults.data = projects;
                                                                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                                    }
                                                                    else
                                                                    {
                                                                        callbackResults.result = "Project Widgets Counldn't Load.";
                                                                        callbackResults.data = default;
                                                                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                                    }
                                                                }
                                                            });
                                                        }
                                                        else
                                                            Log(prefabCallbackResults.resultCode, prefabCallbackResults.result, this);
                                                    });
                                                }
                                                else
                                                {
                                                    callbackResults.result = $"DWidget Prefab For Screen Type : {screenType} Missing / Null.";
                                                    callbackResults.data = default;
                                                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                }
                                            }
                                            else
                                                Log(callbackResults.resultCode, callbackResults.result, this);
                                        });
                                    }
                                    else
                                        Log(sortedListCallbackResults.resultCode, sortedListCallbackResults.result, this);
                                });

                                await Task.Yield();
                            }
                        }
                        else
                        {
                            callbackResults.result = $"Current Screen Type : {GetCurrentScreenType()} Dosen't Match Requested Screen Type : {screenType} - Operation Is Not Valid.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.WarningCode;
                        }
                    }
                    else
                    {
                        callbackResults.result = "Dynamic Widgets Content Container Missing / Null.";
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }

                return callbackResults;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public async Task<AppData.CallbackDataList<AppData.UIScreenWidget>> CreateUIScreenFolderWidgetsAsync(AppData.ScreenType screenType, List<AppData.StorageDirectoryData> foldersDirectoryList, DynamicWidgetsContainer screenContentContainer)
        {
            try
            {
                AppData.CallbackDataList<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDataList<AppData.UIScreenWidget>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                    if (screenContentContainer != null && screenContentContainer.GetActive().Success())
                    {
                        switch (screenType)
                        {
                            case AppData.ScreenType.ProjectDashboardScreen:

                                bool loadTaskIsRunning = true;

                                while (loadTaskIsRunning)
                                {
                                    databaseManager.LoadFolderData(foldersDirectoryList, (foldersLoaded) =>
                                    {
                                        if (AppData.Helpers.IsSuccessCode(foldersLoaded.resultCode))
                                        {
                                            List<AppData.UIScreenWidget> loadedWidgetsList = new List<AppData.UIScreenWidget>();

                                            List<AppData.Folder> pinnedFolders = new List<AppData.Folder>();

                                            foreach (var folder in foldersLoaded.data)
                                                if (folder.defaultWidgetActionState == AppData.DefaultUIWidgetActionState.Pinned)
                                                    pinnedFolders.Add(folder);

                                            databaseManager.GetSortedWidgetList(foldersLoaded.data, pinnedFolders, sortedList =>
                                            {
                                                if (AppData.Helpers.IsSuccessCode(sortedList.resultCode))
                                                {
                                                    databaseManager.GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                                                    {
                                                        if (widgetsCallback.Success())
                                                        {
                                                            var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                                            if (widgetPrefabData != null)
                                                            {
                                                                callbackResults.SetResult(databaseManager.GetProjectStructureData());

                                                                if (callbackResults.Success())
                                                                {
                                                                    widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableWidgetType.Folder, databaseManager.GetProjectStructureData().data.GetLayoutViewType(), prefabCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(prefabCallbackResults);

                                                                        if (callbackResults.Success())
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

                                                                                        if (databaseManager.GetProjectStructureData().data.paginationViewType == AppData.PaginationViewType.Pager)
                                                                                            widgetComponent.Hide();

                                                                                        folderWidget.name = folder.name;
                                                                                        widgetComponent.SetFolderData(folder);
                                                                                        screenContentContainer.AddContent(content: widgetComponent, keepWorldPosition: false, overrideActiveState: false, updateContainer: true);
                                                                                    }

                                                                                    if (!loadedWidgetsList.Contains(widgetComponent))
                                                                                        loadedWidgetsList.Add(widgetComponent);
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                            Log(prefabCallbackResults.resultCode, prefabCallbackResults.result, this);
                                                                    });
                                                                }
                                                                else
                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                            }
                                                            else
                                                                LogError("Widget Prefab Data Missing.", this);
                                                        }
                                                    });
                                                }
                                                else
                                                    Debug.LogWarning($"--> GetSortedWidgetList Failed With Results : {sortedList.result}");
                                            });

                                            if (loadedWidgetsList.Count > 0)
                                            {
                                                loadTaskIsRunning = false;

                                                callbackResults.data = loadedWidgetsList;
                                            }
                                            else
                                                callbackResults.data = default;
                                        }
                                        else
                                            Debug.LogWarning($"--> LoadFolderData Failed With Results : {foldersLoaded.result}");

                                        callbackResults.result = foldersLoaded.result;
                                        callbackResults.resultCode = foldersLoaded.resultCode;
                                    });

                                    await Task.Yield();
                                }

                                break;
                        }
                    }
                    else
                    {
                        callbackResults.result = "Dynamic Widgets Content Container Missing / Null.";
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }

                return callbackResults;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public async Task<AppData.CallbackDataList<AppData.UIScreenWidget>> CreateUIScreenFileWidgetsAsync(AppData.ScreenType screenType, AppData.Folder folder, DynamicWidgetsContainer screenContentContainer)
        {
            try
            {
                AppData.CallbackDataList<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDataList<AppData.UIScreenWidget>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                    if (screenContentContainer != null && screenContentContainer.GetActive().Success())
                    {
                        switch (screenType)
                        {
                            case AppData.ScreenType.ProjectDashboardScreen:

                                bool loadTaskIsRunning = true;

                                while (loadTaskIsRunning)
                                {
                                    databaseManager.LoadSceneAssets(folder, (loadedAssetsResults) =>
                                {
                                    callbackResults.result = loadedAssetsResults.result;
                                    callbackResults.resultCode = loadedAssetsResults.resultCode;

                                    if (AppData.Helpers.IsSuccessCode(loadedAssetsResults.resultCode))
                                    {
                                        var sceneAssetList = new List<AppData.SceneAsset>();

                                        if (loadedAssetsResults.data.Count > 0)
                                        {
                                            List<AppData.UIScreenWidget> loadedWidgetsList = new List<AppData.UIScreenWidget>();

                                            databaseManager.GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                                            {
                                                callbackResults.result = widgetsCallback.result;
                                                callbackResults.resultCode = widgetsCallback.resultCode;

                                                if (widgetsCallback.Success())
                                                {
                                                    var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                                    if (widgetPrefabData != null)
                                                    {
                                                        if (databaseManager.GetProjectStructureData().Success())
                                                        {
                                                            widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableWidgetType.Asset, databaseManager.GetProjectStructureData().data.GetLayoutViewType(), prefabCallbackResults =>
                                                            {
                                                                callbackResults.result = prefabCallbackResults.result;
                                                                callbackResults.resultCode = prefabCallbackResults.resultCode;

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
                                                                                    if (databaseManager.GetProjectStructureData().Success())
                                                                                    {
                                                                                        widgetComponent.SetDefaultUIWidgetActionState(asset.defaultWidgetActionState);

                                                                                        if (databaseManager.GetProjectStructureData().GetData().paginationViewType == AppData.PaginationViewType.Pager)
                                                                                            widgetComponent.Hide();

                                                                                        newWidget.name = asset.name;

                                                                                        widgetComponent.SetAssetData(asset);
                                                                                        widgetComponent.SetWidgetParentScreen(GetCurrentScreen().GetData());
                                                                                        widgetComponent.SetWidgetAssetData(asset);

                                                                                        screenContentContainer.AddContent(content: widgetComponent, keepWorldPosition: false, overrideActiveState: false, updateContainer: true);

                                                                                        sceneAssetList.Add(asset);

                                                                                        AppData.SceneAssetWidget assetWidget = new AppData.SceneAssetWidget();
                                                                                        assetWidget.name = widgetComponent.GetAssetData().name;
                                                                                        assetWidget.value = newWidget;
                                                                                        assetWidget.categoryType = widgetComponent.GetAssetData().categoryType;
                                                                                        assetWidget.creationDateTime = widgetComponent.GetAssetData().creationDateTime.dateTime;

                                                                                        //screenWidgetList.Add(assetWidget);

                                                                                        //widgetComponent.SetFileData();

                                                                                        if (!loadedWidgetsList.Contains(widgetComponent))
                                                                                            loadedWidgetsList.Add(widgetComponent);

                                                                                        callbackResults.result = "Widget Prefab Loaded.";
                                                                                        callbackResults.data = loadedWidgetsList;
                                                                                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        callbackResults.result = databaseManager.GetProjectStructureData().result;
                                                                                        callbackResults.resultCode = databaseManager.GetProjectStructureData().resultCode;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    callbackResults.result = "Widget Prefab Component Missing.";
                                                                                    callbackResults.data = default;
                                                                                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                callbackResults.result = "Widget Prefab Failed To Instantiate.";
                                                                                callbackResults.data = default;
                                                                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                                            }
                                                                        }
                                                                        else
                                                                            Debug.LogWarning($"--> Widget : {asset.modelAsset.name} Already Exists.");
                                                                    }
                                                                }
                                                                else
                                                                    Log(prefabCallbackResults.resultCode, prefabCallbackResults.result, this);
                                                            });
                                                        }
                                                        else
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    }
                                                    else
                                                        LogError("Widget Prefab Data Missing.", this);
                                                }
                                            });

                                            if (loadedWidgetsList.Count >= loadedAssetsResults.data.Count)
                                            {
                                                loadTaskIsRunning = false;

                                                callbackResults.result = "Created Screen Widgets";
                                                callbackResults.data = loadedWidgetsList;
                                                callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                            }
                                            else
                                            {
                                                callbackResults.result = "Failed To Create Screen Widgets";
                                                callbackResults.data = default;
                                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                            }
                                        }
                                        else
                                        {
                                            callbackResults.result = "Failed To Create Screen Widgets";
                                            callbackResults.data = default;
                                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                        }
                                    }
                                    else
                                    {
                                        callbackResults.result = loadedAssetsResults.result;
                                        callbackResults.data = default;
                                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                    }

                                });

                                    await Task.Yield();
                                }

                                break;
                        }
                    }
                    else
                    {
                        callbackResults.result = "Dynamic Widgets Content Container Missing / Null.";
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }

                return callbackResults;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public async Task<AppData.CallbackDataList<AppData.UIScreenWidget>> CreateUIScreenFileWidgetsAsync(AppData.ScreenType screenType, List<AppData.StorageDirectoryData> filesDirectoryList, DynamicWidgetsContainer screenContentContainer)
        {
            try
            {
                AppData.CallbackDataList<AppData.UIScreenWidget> callbackResults = new AppData.CallbackDataList<AppData.UIScreenWidget>(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Database Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                    if (screenContentContainer != null && screenContentContainer.GetActive().Success())
                    {
                        switch (screenType)
                        {
                            case AppData.ScreenType.ProjectDashboardScreen:

                                bool loadTaskIsRunning = true;

                                while (loadTaskIsRunning)
                                {
                                    databaseManager.LoadSceneAssets(filesDirectoryList, (loadedAssetsResults) =>
                                {
                                    if (AppData.Helpers.IsSuccessCode(loadedAssetsResults.resultCode))
                                    {
                                        var sceneAssetList = new List<AppData.SceneAsset>();

                                        if (loadedAssetsResults.data.Count > 0)
                                        {
                                            List<AppData.UIScreenWidget> loadedWidgetsList = new List<AppData.UIScreenWidget>();

                                            databaseManager.GetWidgetsPrefabDataLibrary().GetAllUIScreenWidgetsPrefabDataForScreen(screenType, widgetsCallback =>
                                            {
                                                callbackResults.result = widgetsCallback.result;
                                                callbackResults.resultCode = widgetsCallback.resultCode;

                                                if (widgetsCallback.Success())
                                                {
                                                    var widgetPrefabData = widgetsCallback.data.Find(x => x.screenType == screenType);

                                                    if (widgetPrefabData != null)
                                                    {
                                                        if (databaseManager.GetProjectStructureData().Success())
                                                        {
                                                            widgetPrefabData.GetUIScreenWidgetData(AppData.SelectableWidgetType.Folder, databaseManager.GetProjectStructureData().data.GetLayoutViewType(), prefabCallbackResults =>
                                                            {
                                                                callbackResults.result = prefabCallbackResults.result;
                                                                callbackResults.resultCode = prefabCallbackResults.resultCode;

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

                                                                                    if (databaseManager.GetProjectStructureData().data.paginationViewType == AppData.PaginationViewType.Pager)
                                                                                        widgetComponent.Hide();

                                                                                    newWidget.name = asset.name;

                                                                                    widgetComponent.SetAssetData(asset);
                                                                                    widgetComponent.SetWidgetParentScreen(GetCurrentScreen().GetData());
                                                                                    widgetComponent.SetWidgetAssetData(asset);

                                                                                    screenContentContainer.AddContent(content: widgetComponent, keepWorldPosition: false, overrideActiveState: false, updateContainer: true);

                                                                                    sceneAssetList.Add(asset);

                                                                                    AppData.SceneAssetWidget assetWidget = new AppData.SceneAssetWidget();
                                                                                    assetWidget.name = widgetComponent.GetAssetData().name;
                                                                                    assetWidget.value = newWidget;
                                                                                    assetWidget.categoryType = widgetComponent.GetAssetData().categoryType;
                                                                                    assetWidget.creationDateTime = widgetComponent.GetAssetData().creationDateTime.dateTime;

                                                                                    //screenWidgetList.Add(assetWidget);

                                                                                    if (!loadedWidgetsList.Contains(widgetComponent))
                                                                                        loadedWidgetsList.Add(widgetComponent);

                                                                                    callbackResults.result = "Widget Prefab Component Loaded.";
                                                                                    callbackResults.data = loadedWidgetsList;
                                                                                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                                                }
                                                                                else
                                                                                {
                                                                                    callbackResults.result = "Widget Prefab Component Missing.";
                                                                                    callbackResults.data = default;
                                                                                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                callbackResults.result = $"Failed To Instantiate Prefab For Screen Widget : {asset.modelAsset.name}";
                                                                                callbackResults.data = default;
                                                                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            callbackResults.result = $"Widget : {asset.modelAsset.name} Already Exists.";
                                                                            callbackResults.data = default;
                                                                            callbackResults.resultCode = AppData.Helpers.WarningCode;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                    Log(prefabCallbackResults.resultCode, prefabCallbackResults.result, this);
                                                            });
                                                        }
                                                        else
                                                        {
                                                            callbackResults.result = databaseManager.GetProjectStructureData().result;
                                                            callbackResults.resultCode = databaseManager.GetProjectStructureData().resultCode;
                                                        }
                                                    }
                                                    else
                                                        LogError("Widget Prefab Data Missing.", this);
                                                }
                                            });

                                            if (loadedWidgetsList.Count >= loadedAssetsResults.data.Count)
                                            {
                                                loadTaskIsRunning = false;

                                                callbackResults.result = "Created Screen Widgets";
                                                callbackResults.data = loadedWidgetsList;
                                                callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                            }
                                            else
                                            {
                                                callbackResults.result = "Failed To Create Screen Widgets";
                                                callbackResults.data = default;
                                                callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                            }
                                        }
                                        else
                                        {
                                            callbackResults.result = "Failed To Create Screen Widgets";
                                            callbackResults.data = default;
                                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                        }
                                    }
                                    else
                                    {
                                        callbackResults.result = loadedAssetsResults.result;
                                        callbackResults.data = default;
                                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                    }

                                });

                                    await Task.Yield();
                                }

                                break;
                        }
                    }
                    else
                    {
                        callbackResults.result = "Dynamic Widgets Content Container Missing / Null.";
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }

                return callbackResults;
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        #endregion

        #endregion
    }
}
