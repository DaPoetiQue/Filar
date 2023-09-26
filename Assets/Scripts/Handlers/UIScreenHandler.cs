using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.RedicalGames.Filar
{
    public class UIScreenHandler : AppData.ScreenUIData
    {
        #region Main

        public void Init(Action<AppData.CallbackParams<AppData.EventActionHandler>> callback = null)
        {
            AppData.CallbackParams<AppData.EventActionHandler> callbackResults = new AppData.CallbackParams<AppData.EventActionHandler>();

            AppData.Helpers.GetAppComponentsValid(screenWidgetsList, "Screen Widgets List", async componentsValidCallbackResults => 
            {
                callbackResults.SetResult(componentsValidCallbackResults);

                if(callbackResults.Success())
                {
                    AppData.Widget[] widgetComponents = GetComponentsInChildren<AppData.Widget>();

                    AppData.Helpers.GetAppComponentsValid(widgetComponents, "Widget Components", async componentsValidCallbackResults => 
                    {
                        callbackResults.SetResult(componentsValidCallbackResults);

                        if (callbackResults.Success())
                        {
                            screenWidgetsList = new List<AppData.Widget>();

                            foreach (var widget in widgetComponents)
                            {
                                if (widget != null && !screenWidgetsList.Contains(widget))
                                {
                                    await Task.Yield();

                                    widget.Init(this, initializationCallbackResults =>
                                    {
                                        callbackResults.SetResult(initializationCallbackResults);

                                        if (callbackResults.Success())
                                        {
                                            var widgetEventActionData = initializationCallbackResults.GetData();

                                            RegisterEventAction(eventRegisteredCallbackResults =>
                                            {
                                                callbackResults.SetResult(eventRegisteredCallbackResults);

                                                if (callbackResults.Success())
                                                {
                                                    screenWidgetsList.Add(widget);

                                                    var onScreenChangedEvent = new AppData.EventAction<AppData.UIScreenType>("On Screen Changed Event", AppData.EventType.OnScreenChangedEvent, OnScreenChangedEvent);

                                                    var onWidgetsEvent = new AppData.EventAction<AppData.WidgetType, AppData.InputActionButtonType, AppData.SceneDataPackets>("On Widgets Event", AppData.EventType.OnWidgetActionEvent, OnWidgetsEvent);

                                                    var onScreenTogglableStateEvent = new AppData.EventAction<AppData.TogglableWidgetType, bool, bool>("On Screen Togglable State Event", AppData.EventType.OnScreenTogglableStateEvent, OnScreenTogglableStateEvent);

                                                    var onAssetPoseResetEvent = new AppData.EventAction("On Asset Pose Reset", AppData.EventType.OnSceneModelPoseResetEvent, OnAssetPoseReset);

                                                    callbackResults.SetData(onScreenChangedEvent, onWidgetsEvent, onScreenTogglableStateEvent, onAssetPoseResetEvent);

                                                }
                                                else
                                                    Log(eventRegisteredCallbackResults.GetResultCode, eventRegisteredCallbackResults.GetResult, this);

                                            }, widgetEventActionData);
                                        }
                                        else
                                            Log(initializationCallbackResults.GetResultCode, initializationCallbackResults.GetResult, this);
                                    });
                                }
                                else
                                {
                                    callbackResults.result = $"Widget / Screen Widgets List Is Null / Missing - Invalid Operation - Please See Here.";
                                    callbackResults.data = default;
                                    callbackResults.resultCode = AppData.Helpers.ErrorCode;

                                    break;
                                }
                            }
                        }
                    
                    }, $"Widget Components Not Found For Screen : {name}");
                }
            
            }, "Screen Widgets List Is Not Yet Initialized.");

            callback?.Invoke(callbackResults);
        }

        void ActionEventsSubscription(bool subscribe)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnScreenChangedEvent += OnScreenChangedEvent;
                AppData.ActionEvents._OnWidgetActionEvent += OnWidgetsEvent;
                AppData.ActionEvents._OnScreenTogglableStateEvent += OnScreenTogglableStateEvent;
                AppData.ActionEvents._OnSceneModelPoseResetEvent += OnAssetPoseReset;
            }
            else
            {
                AppData.ActionEvents._OnWidgetActionEvent -= OnWidgetsEvent;
                AppData.ActionEvents._OnScreenChangedEvent -= OnScreenChangedEvent;
                AppData.ActionEvents._OnScreenTogglableStateEvent -= OnScreenTogglableStateEvent;
                AppData.ActionEvents._OnSceneModelPoseResetEvent -= OnAssetPoseReset;
            }
        }

        #endregion
    }
}
