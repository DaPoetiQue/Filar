using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class LoadingManager : AppMonoBaseClass
    {
        #region Static

        private static LoadingManager _instance;

        public static LoadingManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<LoadingManager>();

                return _instance;
            }
        }

        #endregion

        #region Components

        [Space(5)]
        [SerializeField]
        AppData.LoadingSequence loadingSequence = new AppData.LoadingSequence();

        [Space(5)]
        [SerializeField]
        float tempLoadDuration = 3.0f;

        public bool OnInitialLoad { get; private set; }

        bool OnShowSplashScreen { get; set; } = true;

        float loadingDuration = 0.0f;

        #endregion

        #region Main

        public void Init(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            callbackResults.result = GetLoadingSequence().IsInitialized().result;
            callbackResults.resultCode = GetLoadingSequence().IsInitialized().resultCode;

            if (callbackResults.Success())
            {
                callbackResults.result = "Loading Manager Has Been Initialized Successfully";
            }
            else
                callbackResults.result = "Loading Manager's Initialization Failed";

            callback?.Invoke(callbackResults);
        }

        public async Task LoadScreen(AppData.ScreenLoadInfoInstance screenLoadInfoInstance, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (CanLoad().Success())
            {
                callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name, "Scene Assets Manager instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    OnInitialLoad = screenLoadInfoInstance.InitialScreen();

                    var sceneAssetsManager = AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name).data;

                    if (callbackResults.Success())
                    {
                        var screenUIManagerInstanceCallbackResults = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, "Screen UI Manager instance Is Not Yet Initialized.");

                        callbackResults.SetResults(screenUIManagerInstanceCallbackResults);

                        if (callbackResults.Success())
                        {
                            var screenUIManager = screenUIManagerInstanceCallbackResults.data;

                            if(OnShowSplashScreen && !screenLoadInfoInstance.InitialScreen())
                            {
                                #region Show Splash Screen

                                var showScreenTaskResults = await screenUIManager.ShowScreenAsync(screenLoadInfoInstance.GetScreenData());

                                callbackResults.SetResults(showScreenTaskResults);

                                if (callbackResults.Success())
                                {
                                    var screenLoadedDelayTimeTaskResults = await screenLoadInfoInstance.OnScreenLoadExecutionTime(AppData.RuntimeExecution.OnSplashScreenExitDelay);

                                    callbackResults.SetResults(screenLoadedDelayTimeTaskResults);


                                    if (callbackResults.Success())
                                    {
                                        var hideScreenTaskResults =  await screenUIManager.HideScreenAsync(screenLoadInfoInstance.GetScreenData());

                                        callbackResults.SetResults(screenLoadedDelayTimeTaskResults);

                                        if (callbackResults.Success())
                                        {
                                            var screenExitDelayTimeTaskResults = await screenLoadInfoInstance.OnScreenLoadExecutionTime(AppData.RuntimeExecution.OnScreenChangedExitDelay);
                                            callbackResults.SetResults(screenLoadedDelayTimeTaskResults);

                                            if (callbackResults.Success())
                                            {
                                                OnShowSplashScreen = false;
                                                //OnInitialLoad = true;
                                            }
                                        }
                                    }
                                }

                                callback.Invoke(callbackResults);

                                #endregion

                            }
                            else if (screenLoadInfoInstance.InitialScreen() && !OnShowSplashScreen)
                            {
                                #region Loading Data Initialization

                                AppData.CallbackData<AppData.SceneDataPackets> loadingScreenDataPacketsCallbackResults = new AppData.CallbackData<AppData.SceneDataPackets>();

                                SceneAssetsManager.Instance.GetDataPacketsLibrary().GetDataPacket(AppData.UIScreenType.LoadingScreen, getLoadingScreenDataPacketsCallbackResults =>
                                {
                                    loadingScreenDataPacketsCallbackResults.result = getLoadingScreenDataPacketsCallbackResults.result;
                                    loadingScreenDataPacketsCallbackResults.data = getLoadingScreenDataPacketsCallbackResults.data.dataPackets;
                                    loadingScreenDataPacketsCallbackResults.resultCode = getLoadingScreenDataPacketsCallbackResults.resultCode;
                                });

                                callbackResults.SetResults(loadingScreenDataPacketsCallbackResults);

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
                                            else
                                                Log(callbackResults.resultCode, callbackResults.result, this);

                                        }, screenLoadInfoInstance.GetSequenceInstanceArray());

                                        #endregion

                                        #region Processing Loading Sequence

                                        if (callbackResults.Success())
                                        {
                                            await ScreenUIManager.Instance.ShowScreenAsync(loadingScreenDataPacketsCallbackResults.data);

                                            while (GetLoadingSequence().IsRunning())
                                                await Task.Yield();

                                            await ScreenUIManager.Instance.HideScreenAsync(loadingScreenDataPacketsCallbackResults.data);

                                            int loadingScreenExitDelay = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeExecution.OnScreenChangedExitDelay).value);
                                            await Task.Delay(loadingScreenExitDelay);

                                            await ScreenUIManager.Instance.ShowScreenAsync(screenLoadInfoInstance.GetScreenData());
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

                                //var messagingSystemCallbackResults = AppData.Helpers.GetAppComponentValid(MessagingSystemManager.Instance, MessagingSystemManager.Instance.name, "Messaging System Manager Instance Is Not Yet Initialized.");

                                //callbackResults.SetResults(messagingSystemCallbackResults);

                                //if (callbackResults.Success())
                                //{
                                //    var messageGroupCallbackResults = new AppData.CallbackData<AppData.MessageGroup>();

                                //    messagingSystemCallbackResults.data.GetMessageGroup(dataPackets.screenType, getMessageGroupCallbackResults =>
                                //    {
                                //        messageGroupCallbackResults = getMessageGroupCallbackResults;

                                //    });

                                //    callbackResults.SetResults(messageGroupCallbackResults);

                                //    if (callbackResults.Success())
                                //    {
                                //        var randomMessageCallbackResults = new AppData.CallbackData<AppData.Message>();
                                //        messageGroupCallbackResults.data.GetRandomMessage(getRandomMessageCallbackResults => { randomMessageCallbackResults = getRandomMessageCallbackResults; });

                                //        callbackResults.SetResults(randomMessageCallbackResults);

                                //        if (callbackResults.Success())
                                //        {
                                //            callbackResults.SetResults(screenUIManager.GetScreen(AppData.UIScreenType.LoadingScreen));

                                //            if (callbackResults.Success())
                                //            {
                                //                var messageDisplayerWidget = screenUIManager.GetScreen(AppData.UIScreenType.LoadingScreen).data.value.GetWidget(AppData.WidgetType.MessageDisplayerWidget);

                                //                if (messageDisplayerWidget != null)
                                //                {
                                //                    messageDisplayerWidget.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, randomMessageCallbackResults.data.GetHeader());
                                //                    messageDisplayerWidget.SetUITextDisplayerValue(AppData.ScreenTextType.MessageDisplayer, randomMessageCallbackResults.data.GetBody());
                                //                    messageDisplayerWidget.SetUITextDisplayerValue(AppData.ScreenTextType.InfoDisplayer, randomMessageCallbackResults.data.GetFooter());

                                //                    if (screenUIManager.GetCurrentUIScreenType() == dataPackets.screenType)
                                //                    {
                                //                        callbackResults.results = $"Screen : {screenUIManager.GetCurrentUIScreenType()} Has Been Loaded Successfully.";
                                //                        //callbackResults.data = dataPackets;

                                //                        await ScreenUIManager.Instance.ShowScreenAsync(loadingScreenDataPacketsCallbackResults.data);

                                //                        loadingDuration = tempLoadDuration;

                                //                        while (loadingDuration > 0.0f)
                                //                        {
                                //                            loadingDuration -= 1.0f * 5 * Time.deltaTime;
                                //                            await Task.Yield();
                                //                        }

                                //                        if (loadingDuration <= 0)
                                //                        {
                                //                            await ScreenUIManager.Instance.HideScreenAsync(loadingScreenDataPacketsCallbackResults.data);

                                //                            int loadingScreenExitDelay = AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.OnScreenChangedExitDelay).value);
                                //                            await Task.Delay(loadingScreenExitDelay);

                                //                            await ScreenUIManager.Instance.ShowScreenAsync(dataPackets);
                                //                        }
                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    callbackResults.results = $"Message Displayer Widget Not Found For Loading Screen.";
                                //                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                //                }
                                //            }
                                //            else
                                //            {
                                //                Log(callbackResults.resultsCode, callbackResults.results, this);
                                //                throw new ArgumentNullException("Load Screen Failed", $"Load Screen : {dataPackets.screenType} Failed With Results : {callbackResults.results}.");
                                //            }
                                //        }
                                //        else
                                //        {
                                //            Log(callbackResults.resultsCode, callbackResults.results, this);
                                //            throw new ArgumentNullException("Loading Screen Message", $"Loading Screen Message For Screen : {dataPackets.screenType} Missing / Not Found - Failed With Results : {callbackResults.results}.");
                                //        }
                                //    }
                                //}
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