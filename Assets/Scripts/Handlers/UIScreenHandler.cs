using System;

namespace Com.RedicalGames.Filar
{
    public class UIScreenHandler : AppData.ScreenUIData
    {
        #region Unity Callbacks

        void OnEnable() => ActionEventsSubscription(true);

        void OnDisable() => ActionEventsSubscription(false);

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.UIScreenType, AppData.WidgetType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.UIScreenType, AppData.WidgetType>>(GetType());

            if (callbackResults.Success())
            {
                Init(initializationCallbackResults => 
                {
                    callbackResults.SetResult(initializationCallbackResults);
                });
            }

            callback.Invoke(callbackResults);
        }

        //public void Init(Action<AppData.Callback> callback = null)
        //{
        //    var callbackResults = new AppData.Callback();

        //    if (initializeScreenWidgets)
        //    {
        //        callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(screenWidgetsList, "Screen Widgets List", $""));

        //        if (callbackResults.UnSuccessful())
        //        {
        //            AppData.Widget[] widgetComponents = this.GetComponentsInChildren<AppData.Widget>();

        //            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(widgetComponents, "Widget Components", $""));

        //            if (callbackResults.Success())
        //            {
        //                screenWidgetsList = new List<AppData.Widget>();

        //                foreach (var widget in widgetComponents)
        //                {
        //                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(widget, "Screen Widget", $"Screen Widget Null At Index : {widgetComponents.ToList().IndexOf(widget)} For Screen : {GetName()} - Of Type : {GetType().GetData()}"));

        //                    if (callbackResults.Success())
        //                    {
        //                        widget.Initilize(initializationCallbackResults =>
        //                        {
        //                            callbackResults.SetResult(initializationCallbackResults);

        //                            if (callbackResults.Success())
        //                            {
        //                                AddScreenWidget(widget, screenWidgetAddedCallbackResults =>
        //                                {
        //                                    callbackResults.SetResult(screenWidgetAddedCallbackResults);

        //                                    if (callbackResults.UnSuccessful())
        //                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

        //                                });
        //                            }
        //                        });
        //                    }
        //                    else
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        callbackResults.result = "";
        //        callbackResults.resultCode = AppData.Helpers.SuccessCode;
        //    }

        //    callback?.Invoke(callbackResults);
        //}

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.UIScreenType, AppData.WidgetType>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.UIScreenType, AppData.WidgetType>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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

        #endregion
    }
}
