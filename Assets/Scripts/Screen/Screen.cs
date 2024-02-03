using System;

namespace Com.RedicalGames.Filar
{
    public class Screen : AppData.ScreenUIData
    {
        #region Unity Callbacks

        void OnEnable() => ActionEventsSubscription(true);

        void OnDisable() => ActionEventsSubscription(false);

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.ScreenType, AppData.WidgetType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.ScreenType, AppData.WidgetType, AppData.Widget>>(GetType());

            if (callbackResults.Success())
            {
                Init(initializationCallbackResults => 
                {
                    callbackResults.SetResult(initializationCallbackResults);

                    if (callbackResults.Success())
                    {
                        #region Screen Initializations

                        switch(GetType().GetData())
                        {
                            case AppData.ScreenType.SplashScreen:

                                InitializeSplashScreen(initializationCallbackResults => 
                                {
                                    callbackResults.SetResult(initializationCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });

                                break;

                            case AppData.ScreenType.LoadingScreen:

                                InitializeLoadingScreen(initializationCallbackResults =>
                                {

                                });

                                break;
                        }

                        #endregion

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "App Events Manager Instance Is Not Yet Initialized."));

                            if (callbackResults.Success())
                            {
                                var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                                appEventsManagerInstance.OnEventSubscription<Screen>(OnScreenEntered, AppData.EventType.OnScreenShownEvent, true);
                                appEventsManagerInstance.OnEventSubscription<Screen>(OnScreenExited, AppData.EventType.OnScreenHiddenEvent, true);

                                appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnWidgetFocused, AppData.EventType.OnWidgetShownEvent, true);
                                appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnWidgetUnfocused, AppData.EventType.OnWidgetHiddenEvent, true);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }

            callback.Invoke(callbackResults);
        }

        #region Screens Initialization

        private void InitializeSplashScreen(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppManager.Instance, AppManager.Instance.name, "App Manager Instance Is Not Yet Initialized - Invalid Operation."));

            if (callbackResults.Success())
            {
                var appManagerInstance = AppData.Helpers.GetAppComponentValid(AppManager.Instance, AppManager.Instance.name).GetData();

                callbackResults.SetResult(GetWidgetOfType(AppData.WidgetType.TitleDisplayerWidget));

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(appManagerInstance.GetApplicationName());

                    if (callbackResults.Success())
                    {
                        var titleWidget = GetWidgetOfType(AppData.WidgetType.TitleDisplayerWidget).GetData();
                        titleWidget.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, appManagerInstance.GetApplicationName().GetData());
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

        private void InitializeLoadingScreen(Action<AppData.Callback> callback = null)
        {

        }

        #endregion

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.ScreenType, AppData.WidgetType, AppData.Widget>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.ScreenType, AppData.WidgetType, AppData.Widget>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetType());

                if (callbackResults.Success())
                {
                    var widgetType = GetType().data;

                    callbackResults.SetResult(GetStatePacket().Initialized(widgetType));

                    if (callbackResults.Success())
                    {
                        callbackResults.result = $"Screen : {GetStatePacket().GetName()} Of Type : {GetStatePacket().GetType()} State Is Set To : {GetStatePacket().GetStateType()}";
                        callbackResults.data = GetStatePacket();
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        void ActionEventsSubscription(bool subscribe)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnScreenChangedEvent += OnScreenChangedEvent;
                AppData.ActionEvents._OnPopUpActionEvent += OnWidgetsEvents;
                AppData.ActionEvents._OnScreenTogglableStateEvent += OnScreenTogglableStateEvent;
                AppData.ActionEvents._OnSceneModelPoseResetEvent += OnAssetPoseReset;
            }
            else
            {
                AppData.ActionEvents._OnPopUpActionEvent -= OnWidgetsEvents;
                AppData.ActionEvents._OnScreenChangedEvent -= OnScreenChangedEvent;
                AppData.ActionEvents._OnScreenTogglableStateEvent -= OnScreenTogglableStateEvent;
                AppData.ActionEvents._OnSceneModelPoseResetEvent -= OnAssetPoseReset;
            }
        }

        protected override void OnScreenWidgetShownEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetHiddenEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetTransitionInProgressEvent()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
