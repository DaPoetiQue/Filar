using System;
using System.Collections.Generic;
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

        /// <summary>
        /// This Functions Shows A Loading Screen Then Progress To Selected Screen.
        /// </summary>
        /// <param name="screenType"></param>
        /// <param name="callback"></param>
        public async void LoadSelectedScreen(AppData.ScreenType screenType, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "App Database Manager Instance Is Not Initialized Yet."));

            if (callbackResults.Success())
            {
                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                callbackResults.SetResult(appDatabaseManagerInstance.GetScreenLoadInfoInstanceFromLibrary(screenType));

                if (callbackResults.Success())
                {
                    var screenInfo = appDatabaseManagerInstance.GetScreenLoadInfoInstanceFromLibrary(screenType).GetData();
                    screenInfo.initialScreen = false;

                    await LoadScreen(screenInfo, loadInfoCallbackResults =>
                    {
                        callbackResults.SetResult(loadInfoCallbackResults);
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }


        /// <summary>
        /// This Functions Shows A Selected Screen.
        /// </summary>
        /// <param name="screenType"></param>
        /// <param name="callback"></param>
        public void GoToScreen(AppData.ScreenType screenType, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "App Database Manager Instance Is Not Initialized Yet."));

            if (callbackResults.Success())
            {
                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                callbackResults.SetResult(appDatabaseManagerInstance.GetScreenLoadInfoInstanceFromLibrary(screenType));

                if (callbackResults.Success())
                {
                    var screenInfo = appDatabaseManagerInstance.GetScreenLoadInfoInstanceFromLibrary(screenType).GetData();
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        /// <summary>
        /// This Is The Base Loader For Initialization And Loading Screens Through A Loading Screen
        /// </summary>
        /// <param name="screenLoadInfoInstance"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
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
                            var screenUIManager = screenUIManagerInstanceCallbackResults.GetData();

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

                                databaseManager.GetDataPacketsLibrary().GetDataPacket(AppData.ScreenType.LoadingScreen, getLoadingScreenDataPacketsCallbackResults =>
                                {
                                    loadingScreenDataPacketsCallbackResults.result = getLoadingScreenDataPacketsCallbackResults.result;
                                    loadingScreenDataPacketsCallbackResults.data = getLoadingScreenDataPacketsCallbackResults.GetData().screenConfigDataPacket;
                                    loadingScreenDataPacketsCallbackResults.resultCode = getLoadingScreenDataPacketsCallbackResults.resultCode;
                                });

                                callbackResults.SetResults(loadingScreenDataPacketsCallbackResults);

                                if (callbackResults.Success())
                                {
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

                                                AppData.ActionEvents.OnInitializationStartedEvent();
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
                                        if (screenLoadInfoInstance.HasSequenceInstances())
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
                                                await screenUIManager.ShowScreenAsync(loadingScreenDataPacketsCallbackResults.GetData());

                                                while (GetLoadingSequence().IsRunning())
                                                    await Task.Yield();

                                                var referencedScreen = screenLoadInfoInstance.GetReferencedScreen().GetData();

                                                AppData.ActionEvents.OnInitializationCompletedEvent();

                                                referencedScreen.HideWidget(AppData.WidgetType.LoadingWidget);

                                                await screenUIManager.HideScreenAsync(loadingScreenDataPacketsCallbackResults.GetData());

                                                int loadingScreenExitDelay = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(AppDatabaseManager.Instance.GetDefaultExecutionValue(AppData.RuntimeExecution.OnScreenChangedExitDelay).value);
                                                await Task.Delay(loadingScreenExitDelay);

                                                callbackResults.SetResult(screenLoadInfoInstance.GetReferencedScreen());

                                                if (callbackResults.Success())
                                                {
                                                    referencedScreen.HideWidget(AppData.WidgetType.ImageDisplayerWidget);
                                                    await screenUIManager.ShowScreenAsync(screenLoadInfoInstance.GetScreenConfigDataPacket().GetData());
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
                            }
                            else
                            {
                                #region Loading Data Initialization

                                AppData.CallbackData<AppData.ScreenConfigDataPacket> loadingScreenDataPacketsCallbackResults = new AppData.CallbackData<AppData.ScreenConfigDataPacket>();

                                databaseManager.GetDataPacketsLibrary().GetDataPacket(AppData.ScreenType.LoadingScreen, getLoadingScreenDataPacketsCallbackResults =>
                                {
                                    loadingScreenDataPacketsCallbackResults.result = getLoadingScreenDataPacketsCallbackResults.result;
                                    loadingScreenDataPacketsCallbackResults.data = getLoadingScreenDataPacketsCallbackResults.GetData().screenConfigDataPacket;
                                    loadingScreenDataPacketsCallbackResults.resultCode = getLoadingScreenDataPacketsCallbackResults.resultCode;
                                });

                                callbackResults.SetResults(loadingScreenDataPacketsCallbackResults);

                                if (callbackResults.Success())
                                {
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

                                                AppData.ActionEvents.OnScreenLoadStartedEvent();
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
                                        callbackResults.SetResult(screenUIManager.GetCurrentScreenType());

                                        if (callbackResults.Success())
                                        {
                                            var currentScreenType = screenUIManager.GetCurrentScreenType().GetData();

                                            AppData.CallbackData<AppData.ScreenConfigDataPacket> currentScreenDataPacketsCallbackResults = new AppData.CallbackData<AppData.ScreenConfigDataPacket>();

                                            databaseManager.GetDataPacketsLibrary().GetDataPacket(currentScreenType, getcurrentScreenDataPacketsCallbackResults =>
                                            {
                                                currentScreenDataPacketsCallbackResults.result = getcurrentScreenDataPacketsCallbackResults.result;
                                                currentScreenDataPacketsCallbackResults.data = getcurrentScreenDataPacketsCallbackResults.GetData().screenConfigDataPacket;
                                                currentScreenDataPacketsCallbackResults.resultCode = getcurrentScreenDataPacketsCallbackResults.resultCode;
                                            });

                                            callbackResults.SetResults(currentScreenDataPacketsCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                var onHideCurrentScreenCallbackResultsTask = await screenUIManager.HideScreenAsync(currentScreenDataPacketsCallbackResults.GetData());

                                                callbackResults.SetResult(onHideCurrentScreenCallbackResultsTask);

                                                if(callbackResults.Success())
                                                {
                                                    await Task.Delay(1000);

                                                    if (screenLoadInfoInstance.HasSequenceInstances())
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
                                                            else
                                                                Log(callbackResults.resultCode, callbackResults.result, this);

                                                        }, screenLoadInfoInstance.GetSequenceInstanceArray());

                                                        #endregion

                                                        #region Processing Loading Sequence

                                                        if (callbackResults.Success())
                                                        {
                                                            var showScreenCallbackResultsTask = await screenUIManager.ShowScreenAsync(loadingScreenDataPacketsCallbackResults.GetData());

                                                            callbackResults.SetResult(showScreenCallbackResultsTask);

                                                            if (callbackResults.Success())
                                                            {
                                                                callbackResults.SetResult(screenUIManager.GetCurrentScreen());

                                                                if (callbackResults.Success())
                                                                {
                                                                    var screen = screenUIManager.GetCurrentScreen().GetData();

                                                                    screenLoadInfoInstance.SetReferencedScreen(screen);

                                                                    while (GetLoadingSequence().IsRunning())
                                                                        await Task.Yield();

                                                                    callbackResults.SetResult(screenLoadInfoInstance.GetReferencedScreen());

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        var referencedScreen = screenLoadInfoInstance.GetReferencedScreen().GetData();

                                                                        AppData.ActionEvents.OnInitializationCompletedEvent();

                                                                        referencedScreen.HideWidget(AppData.WidgetType.LoadingWidget);

                                                                        await screenUIManager.HideScreenAsync(loadingScreenDataPacketsCallbackResults.GetData());

                                                                        int loadingScreenExitDelay = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(AppDatabaseManager.Instance.GetDefaultExecutionValue(AppData.RuntimeExecution.OnScreenChangedExitDelay).value);
                                                                        await Task.Delay(loadingScreenExitDelay);

                                                                        callbackResults.SetResult(screenLoadInfoInstance.GetReferencedScreen());

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            referencedScreen.HideWidget(AppData.WidgetType.ImageDisplayerWidget);
                                                                            await screenUIManager.ShowScreenAsync(screenLoadInfoInstance.GetScreenConfigDataPacket().GetData());
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
                                                                Log(callbackResults.resultCode, callbackResults.result, this);
                                                        }
                                                        else
                                                            Log(callbackResults.resultCode, callbackResults.result, this);

                                                        #endregion

                                                    }
                                                    else
                                                        LogWarning("Initial Load Screen Sequence Data Missing / Not Found.", this);
                                                }
                                                else
                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                }
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

        public async Task<AppData.Callback> ProcessLoadingSequence(List<AppData.LoadingSequenceState> sequences, IProgress<int> progressReport)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValuesValid(sequences, "Sequences", "Process Loading Sequence Failed - There Are No Sequences Assigned - Invalid Operation."));

            if (callbackResults.Success())
            {
                for (int i = 0; i < sequences.Count; i++)
                {
                    callbackResults.SetResult(GetSequenceFunction(sequences[i]));

                    if (callbackResults.Success())
                    {
                        var task = GetSequenceFunction(sequences[i]).GetData();
                        var processSequenceCallbackResultsTask = await ProcessSequence(task, sequences.Count, i, progressReport);

                        callbackResults.SetResult(processSequenceCallbackResultsTask);

                        if (callbackResults.UnSuccessful())
                            Log(callbackResults.resultCode, callbackResults.result, this);
                    }
                    else
                    {
                        Log(callbackResults.resultCode, callbackResults.result, this);
                        break;
                    }
                }
            }
            else
                Log(callbackResults.resultCode, callbackResults.result, this);

            return callbackResults;
        }

        private async Task<AppData.Callback> ProcessSequence(Func<Task> sequence, int processCount, int processIndex,  IProgress<int> progressReport = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(sequence, "Sequence", "Process Sequence Failed - Sequence Parameter Value Is null - Invalid Operation."));

            if(callbackResults.Success())
            {
                await sequence.Invoke();

                if(progressReport != null)
                {
                    processIndex += 1;

                    var progressValue = (double)processIndex / processCount;
                    progressValue *= 100;

                    var progress = (int)Math.Round(progressValue, 0);

                    progressReport.Report(progress);
                }
            }
            else
                Log(callbackResults.resultCode, callbackResults.result, this);

            return callbackResults;
        }

        private AppData.CallbackData<Func<Task>> GetSequenceFunction(AppData.LoadingSequenceState sequenceType)
        {
            var callbackResults = new AppData.CallbackData<Func<Task>>(AppData.Helpers.GetAppEnumValueValid(sequenceType, "Sequence Type", $"Get Sequence Function Failed - Sequence Type Parameter Value Is Set To Default : {sequenceType} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, "Network Manager Instance", "Get Sequence Function Failed - Network Manager Instance Is Not Yet Initialized - Invalid Operation."));

                if (callbackResults.Success())
                {
                    var networkManagerInstance = AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, "Network Manager Instance").GetData();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppManager.Instance, "App Manager Instance", "Get Sequence Function Failed - App Manager Instance Is Not Yet Initialized - Invalid Operation."));

                    if (callbackResults.Success())
                    {
                        var appManagerInstance = AppData.Helpers.GetAppComponentValid(AppManager.Instance, "App Database Manager Instance").GetData();

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "Get Sequence Function Failed - Profile Manager Instance Is Not Yet Initialized - Invalid Operation."));

                        if (callbackResults.Success())
                        {
                            var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                            switch (sequenceType)
                            {
                                case AppData.LoadingSequenceState.NetworkConnection:

                                    callbackResults.data = networkManagerInstance.OnCheckNetworkConnectionStatus;

                                    break;

                                case AppData.LoadingSequenceState.CompatibilityStatus:

                                    callbackResults.data = appManagerInstance.GetCompatibilityStatusAsync;

                                    break;

                                case AppData.LoadingSequenceState.SynchronizingProfile:

                                    callbackResults.data = profileManagerInstance.SynchronizingProfile;

                                    break;

                                case AppData.LoadingSequenceState.AppSignIn:

                                    callbackResults.data = profileManagerInstance.AppSignInAsync;

                                    break;

                                case AppData.LoadingSequenceState.ServerConnection:

                                    callbackResults.data = networkManagerInstance.ServerConnected;

                                    break;

                                case AppData.LoadingSequenceState.DownloadPostEntry:

                                    callbackResults.data = appManagerInstance.DownloadPostEntryDataAsync;

                                    break;
                            }

                            callbackResults.result = $"Get Sequence Function Success - Sequence Type : {sequenceType} - Has Been Successfully Found.";
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
                Log(callbackResults.resultCode, callbackResults.result, this);

            return callbackResults;
        }

        #endregion
    }
}