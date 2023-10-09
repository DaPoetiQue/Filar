using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.RedicalGames.Filar
{
    public class UIScreenHandler : AppData.ScreenUIData
    {
        #region Unity Callbacks

        void OnEnable() => ActionEventsSubscription(true);

        void OnDisable() => ActionEventsSubscription(false);

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.UIScreenType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.UIScreenType>>(GetType());

            if (callbackResults.Success())
            {
                //GetScreenView().Init(this, screenViewInitializationCallbackResults =>
                //{
                //    callbackResults.SetResult(screenViewInitializationCallbackResults);
                //});
            }

            LogInfo($" _____________________+++++++++++ Callback Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult}", this);

            callback.Invoke(callbackResults);
        }

        public void Init(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            if (initializeScreenWidgets)
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(screenWidgetsList, "Screen Widgets List", $""));

                if (callbackResults.UnSuccessful())
                {
                    AppData.Widget[] widgetComponents = this.GetComponentsInChildren<AppData.Widget>();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(widgetComponents, "Widget Components", $""));

                    if (callbackResults.Success())
                    {
                        screenWidgetsList = new List<AppData.Widget>();

                        foreach (var widget in widgetComponents)
                        {
                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(widget, "Screen Widget", $"Screen Widget Null At Index : {widgetComponents.ToList().IndexOf(widget)} For Screen : {GetName()} - Of Type : {GetType().GetData()}"));

                            if (callbackResults.Success())
                            {
                                widget.Initilize(initializationCallbackResults =>
                                {
                                    callbackResults.SetResult(initializationCallbackResults);

                                    LogInfo($" _________________________+++++++++++++++++++++++++++++++++++++__________ Screen : {GetName()} - Of Type : {GetType().GetData()} - Code : {callbackResults.GetResultCode} - Result : {callbackResults.GetResult}", this);

                                    if(callbackResults.Success())
                                    {
                                        AddScreenWidget(widget, screenWidgetAddedCallbackResults => 
                                        {
                                            callbackResults.SetResult(screenWidgetAddedCallbackResults);

                                            if (callbackResults.UnSuccessful())
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        
                                        });
                                    }
                                });
                            }
                            else
                                break;
                        }
                    }
                }
            }
            else
            {
                callbackResults.result = "";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }

            callback?.Invoke(callbackResults);
        }

        private void AddScreenWidget(AppData.Widget widget, Action<AppData.Callback> callback = null)
        {
            var callbacResults = new AppData.Callback();

            if(!screenWidgetsList.Contains(widget))
            {
                screenWidgetsList.Add(widget);

                if(screenWidgetsList.Contains(widget))
                {
                    callbacResults.result = $"Screen Widget : {widget.GetName()} - Has Been Added Successfully To Screen Widgets List For Screen : {GetName()} - Of Type : {GetType().GetData()}.";
                    callbacResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbacResults.result = $"Failed To Add Screen Widget : {widget.GetName()} - To Screen Widgets List For Screen : {GetName()} - Of Type : {GetType().GetData()} - Invalid Operation -Please Check Here.";
                    callbacResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbacResults.result = $"Screen Widget : {widget.GetName()} Already Exists In Screen Widgets List For Screen : {GetName()} - Of Type : {GetType().GetData()}.";
                callbacResults.resultCode = AppData.Helpers.WarningCode;
            }

            callback?.Invoke(callbacResults);
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

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.UIScreenType>> OnGetState()
        {
            AppData.CallbackData<AppData.WidgetStatePacket<AppData.UIScreenType>> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.UIScreenType>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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

        #endregion
    }
}
