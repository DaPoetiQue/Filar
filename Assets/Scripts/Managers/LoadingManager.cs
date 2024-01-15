using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class LoadingManager : AppData.SingletonBaseComponent<LoadingManager>
    {
        #region Components

        [Space(5)]
        [SerializeField]
        AppData.LoadingSequence loadingSequence = new AppData.LoadingSequence();

        public bool OnInitialLoad { get; private set; }

        bool OnShowSplashScreen { get; set; } = true;

        #endregion

        #region Main

        protected override void Init()
        {
            
        }

        public async Task LoadScreen(AppData.ScreenLoadInfoInstance screenLoadInfoInstance, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (CanLoad().Success())
            {
                callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Scene Assets Manager instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    OnInitialLoad = screenLoadInfoInstance.InitialScreen();

                    var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                    if (callbackResults.Success())
                    {
                        var screenUIManagerInstanceCallbackResults = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, "Screen UI Manager instance Is Not Yet Initialized.");

                        callbackResults.SetResults(screenUIManagerInstanceCallbackResults);

                        if (callbackResults.Success())
                        {
                            var screenUIManager = screenUIManagerInstanceCallbackResults.data;

                            if(OnShowSplashScreen && !screenLoadInfoInstance.InitialScreen())
                            {
                                callbackResults.SetResults(screenLoadInfoInstance.GetScreenConfigDataPacket());

                                #region Show Splash Screen

                                if (callbackResults.Success())
                                {
                                    #region Screen Setup

                                    var showScreenTaskResults = await screenUIManager.ShowScreenAsync(screenLoadInfoInstance.GetScreenConfigDataPacket().GetData());

                                    callbackResults.SetResults(showScreenTaskResults);

                                    if (callbackResults.Success())
                                    {
                                        var screenLoadedDelayTimeTaskResults = await screenLoadInfoInstance.OnScreenLoadExecutionTime(AppData.RuntimeExecution.OnSplashScreenExitDelay);

                                        callbackResults.SetResults(screenLoadedDelayTimeTaskResults);

                                        if (callbackResults.Success())
                                        {
                                            var hideScreenTaskResults = await screenUIManager.HideScreenAsync(screenLoadInfoInstance.GetScreenConfigDataPacket().GetData());

                                            callbackResults.SetResults(hideScreenTaskResults);

                                            if (callbackResults.Success())
                                            {
                                                var screenExitDelayTimeTaskResults = await screenLoadInfoInstance.OnScreenLoadExecutionTime(AppData.RuntimeExecution.OnScreenChangedExitDelay);
                                                callbackResults.SetResults(screenExitDelayTimeTaskResults);

                                                if (callbackResults.Success())
                                                {
                                                    OnShowSplashScreen = false;
                                                    //OnInitialLoad = true;
                                                }
                                            }
                                        }
                                    }

                                    #endregion

                                    callback.Invoke(callbackResults);
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                #endregion
                            }
                            else if (screenLoadInfoInstance.InitialScreen() && !OnShowSplashScreen)
                            {
                                #region Loading Data Initialization

                                AppData.CallbackData<AppData.ScreenConfigDataPacket> loadingScreenDataPacketsCallbackResults = new AppData.CallbackData<AppData.ScreenConfigDataPacket>();

                                AppDatabaseManager.Instance.GetDataPacketsLibrary().GetDataPacket(AppData.ScreenType.LoadingScreen, getLoadingScreenDataPacketsCallbackResults =>
                                {
                                    loadingScreenDataPacketsCallbackResults.result = getLoadingScreenDataPacketsCallbackResults.result;
                                    loadingScreenDataPacketsCallbackResults.data = getLoadingScreenDataPacketsCallbackResults.GetData().screenConfigDataPacket;
                                    loadingScreenDataPacketsCallbackResults.resultCode = getLoadingScreenDataPacketsCallbackResults.resultCode;
                                });

                                callbackResults.SetResults(loadingScreenDataPacketsCallbackResults);

                                #endregion

                                #region Setup Loading Screen Splash Displayer

                                screenUIManager.GetScreen(loadingScreenDataPacketsCallbackResults.GetData().GetType().GetData(), loadingScreenCallbackResults =>
                                {
                                    callbackResults.SetResult(loadingScreenCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        var loadingScreen = loadingScreenCallbackResults.GetData();

                                        callbackResults.SetResult(loadingScreen.GetWidget(AppData.WidgetType.ImageDisplayerWidget));

                                        if (callbackResults.Success())
                                        {
                                            var imageDisplayerWidget = loadingScreen.GetWidget(AppData.WidgetType.ImageDisplayerWidget).GetData();

                                            var splashImage = databaseManager.GetRandomSplashImage().GetData();

                                            imageDisplayerWidget.SetUIImageDisplayer(AppData.ScreenImageType.Splash, splashImage, true);

                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });

                                #endregion

                                if (callbackResults.Success())
                                {
                                    if(screenLoadInfoInstance.HasSequenceInstances())
                                    {
                                        #region Staging Sequences

                                        GetLoadingSequence().StageSequence(async sequencesStagedCallbackResults =>
                                        {
                                            callbackResults.SetResult(sequencesStagedCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                await GetLoadingSequence().Process(loadingSequenceCallbackResults =>
                                                {
                                                    callbackResults.SetResult(loadingSequenceCallbackResults);

                                                    if (!callbackResults.Success())
                                                        Log(callbackResults.resultCode, callbackResults.result, this);
                                                });
                                            }

                                        }, screenLoadInfoInstance.GetSequenceInstanceArray());

                                        #endregion

                                        #region Processing Loading Sequence

                                        if (callbackResults.Success())
                                        {
                                            await ScreenUIManager.Instance.ShowScreenAsync(loadingScreenDataPacketsCallbackResults.data);

                                            while (GetLoadingSequence().IsRunning())
                                                await Task.Yield();

                                            var referencedScreen = screenLoadInfoInstance.GetReferencedScreen().data;

                                            AppData.ActionEvents.OnInitializationCompletedEvent();

                                            referencedScreen.HideScreenWidget(AppData.WidgetType.LoadingWidget);

                                            await ScreenUIManager.Instance.HideScreenAsync(loadingScreenDataPacketsCallbackResults.data);

                                            int loadingScreenExitDelay = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(AppDatabaseManager.Instance.GetDefaultExecutionValue(AppData.RuntimeExecution.OnScreenChangedExitDelay).value);
                                            await Task.Delay(loadingScreenExitDelay);

                                            callbackResults.SetResult(screenLoadInfoInstance.GetReferencedScreen());

                                            if (callbackResults.Success())
                                            {
                                                referencedScreen.HideScreenWidget(AppData.WidgetType.ImageDisplayerWidget);
                                                await ScreenUIManager.Instance.ShowScreenAsync(screenLoadInfoInstance.GetScreenConfigDataPacket().GetData());
                                            }
                                        }
                                        else
                                            Log(callbackResults.resultCode, callbackResults.result, this);

                                        #endregion

                                    }
                                    else
                                        LogWarning("Initial Load Screen Sequence Data Missing / Not Found.", this);
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                            Log(callbackResults.resultCode, callbackResults.result, this);
                    }
                    else
                        Log(callbackResults.resultCode, callbackResults.result, this);
                }
                else
                    Log(callbackResults.resultCode, callbackResults.result, this);
            }
            else
                Log(CanLoad().resultCode, CanLoad().result, this);

            callback?.Invoke(callbackResults);
        }

        public AppData.LoadingSequence GetLoadingSequence()
        {
            return loadingSequence;
        }

        public AppData.Callback CanLoad()
        {
            AppData.Callback callbackResults = new AppData.Callback();

            callbackResults.result = GetLoadingSequence().IsInitialized().result;
            callbackResults.resultCode = GetLoadingSequence().IsInitialized().resultCode;

            return callbackResults;
        }

        #endregion
    }
}