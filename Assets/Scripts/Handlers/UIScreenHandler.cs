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

        public AppData.CallbackData<AppData.EventActionData> Init()
        {
            AppData.CallbackData<AppData.EventActionData> callbackResults = new AppData.CallbackData<AppData.EventActionData>(InitializeScreenWidgets());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(screenWidgetsList, "Screen Widgets List", $"Screen Widgets Are Not Yet Initialized For Screen : {GetName()} - Of Type : {GetUIScreenType()}", $"{screenWidgetsList.Count} : Screen Widgets Have Been Found For Screen : {GetName()} - Of Type : {GetUIScreenType()}"));

                if (callbackResults.UnSuccessful())
                {
                    AppData.Widget[] widgetComponents = widgetComponents = GetComponentsInChildren<AppData.Widget>();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(widgetComponents, "Widget Components", $"Widget Components Were Not Found For Screen : {GetName()} - Of Type : {GetUIScreenType()}", $"{widgetComponents.Length} : Widget Component(s) Found For Screen : {GetName()} - Of Type : {GetUIScreenType()}"));

                    if (callbackResults.Success())
                    {
                        screenWidgetsList = new List<AppData.Widget>();

                        foreach (var widget in widgetComponents)
                        {
                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(widget, "Widget", $"Widget Not Found / Missing At Index : {widgetComponents.ToList().IndexOf(widget)} - For Screen : {GetName()} - Of Type : {GetUIScreenType()}"));

                            if (callbackResults.Success())
                            {
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
                                                AddScreenWidget(widget, widgetAddedCallbackResults => 
                                                {
                                                    callbackResults.SetResult(widgetAddedCallbackResults);

                                                    if (callbackResults.UnSuccessful())
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });
                                            }
                                            else
                                                Log(eventRegisteredCallbackResults.GetResultCode, eventRegisteredCallbackResults.GetResult, this);

                                        }, widgetEventActionData);
                                    }
                                    else
                                        Log(initializationCallbackResults.GetResultCode, initializationCallbackResults.GetResult, this);
                                });
                            }
                        }
                    }

                    LogInfo($" _______________________+++++++ Initialized {screenWidgetsList.Count} : Widgets Out Of : {widgetComponents.Length} Found Child Widgets.", this);
                }
            }
            else
            {
                callbackResults.result = "Screen Has Been Initialized Without Widgets.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }

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
