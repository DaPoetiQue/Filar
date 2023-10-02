using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.RedicalGames.Filar
{
    public class UIScreenHandler : AppData.ScreenUIData
    {
        #region Unity Callbacks

        void OnEnable() => ActionEventsSubscription(true);

        void OnDisable() => ActionEventsSubscription(false);

        #endregion

        #region Main

        public void Init(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            if (initializeWidgets)
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
                            if (widget != null && !screenWidgetsList.Contains(widget))
                            {
                                widget.Initilize(initializationCallbackResults =>
                                {
                                    callbackResults.SetResult(initializationCallbackResults);
                                });

                                //widget.Init(this, initializationCallbackResults => 
                                //{
                                //    if (initializationCallbackResults.Success())
                                //    {
                                //        var widgetEventActionData = initializationCallbackResults.GetData();

                                //        RegisterEventAction(eventRegisteredCallbackResults => 
                                //        {
                                //            if (eventRegisteredCallbackResults.Success())
                                //                screenWidgetsList.Add(widget);
                                //            else
                                //                Log(eventRegisteredCallbackResults.GetResultCode, eventRegisteredCallbackResults.GetResult, this);

                                //        }, widgetEventActionData);
                                //    }
                                //    else
                                //        Log(initializationCallbackResults.GetResultCode, initializationCallbackResults.GetResult, this);
                                //});
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
