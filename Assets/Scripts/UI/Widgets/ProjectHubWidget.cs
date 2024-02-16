using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ProjectHubWidget : AppData.Widget
    {
        #region Components

        private AppData.ActionButtonListener onConfirmationButtonEvent = new AppData.ActionButtonListener();

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetType());

                if (callbackResults.Success())
                {
                    var widgetType = GetType().data;

                    callbackResults.SetResult(GetStatePacket().Initialized(widgetType));

                    if (callbackResults.Success())
                    {
                        callbackResults.result = $"Widget : {GetStatePacket().GetName()} Of Type : {GetStatePacket().GetType()} State Is Set To : {GetStatePacket().GetStateType()}";
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

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {

        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            
        }

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget));

                    if (callbackResults.Success())
                    {
                        switch (actionType)
                        {
                            case AppData.InputActionButtonType.GoToHomeButton:

                                var confirmationWidget = screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget).GetData() as ConfirmationPopUpWidget;

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(confirmationWidget, "Confirmation Widget", "Failed To Get Confirmation Widget - Invalid Operation."));

                                if (callbackResults.Success())
                                {
                                    confirmationWidget.UnRegisterActionButtonListeners(eventsUnregisteredCallbackResults => 
                                    {
                                        callbackResults.SetResult(eventsUnregisteredCallbackResults);

                                        if(callbackResults.Success())
                                        {
                                            onConfirmationButtonEvent.SetMethod(GoToHomeScreen, methodSetCallbackResults =>
                                            {
                                                callbackResults.SetResult(methodSetCallbackResults);

                                                if (callbackResults.Success())
                                                {
                                                    onConfirmationButtonEvent.SetAction(AppData.InputActionButtonType.ConfirmationButton, actionSetCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(actionSetCallbackResults);

                                                        if (callbackResults.Success())
                                                        {
                                                            confirmationWidget.RegisterActionButtonListeners(onConfirmRegisteredCallbackResults =>
                                                            {
                                                                callbackResults.SetResult(onConfirmRegisteredCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    var confirmationWidgetConfig = new AppData.SceneConfigDataPacket();

                                                                    confirmationWidgetConfig.SetReferencedWidgetType(AppData.WidgetType.ConfirmationPopUpWidget);
                                                                    confirmationWidgetConfig.blurScreen = true;

                                                                    screen.ShowWidget(confirmationWidgetConfig);
                                                                }
                                                                else
                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                            }, onConfirmationButtonEvent);
                                                        }
                                                        else
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                                else
                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            });
                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    });
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                break;

                            case AppData.InputActionButtonType.CreateNewProjectButton:

                                var newProjectWidgetConfig = new AppData.SceneConfigDataPacket();

                                newProjectWidgetConfig.SetReferencedWidgetType(AppData.WidgetType.ProjectCreationWidget);
                                newProjectWidgetConfig.blurScreen = true;

                                screen.ShowWidget(newProjectWidgetConfig);

                                break;
                        }
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


        private void GoToHomeScreen()
        {
            var callbackResults = new AppData.Callback();

           callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance", "Loading Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var loadingManagerInstance = AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance").GetData();

                loadingManagerInstance.LoadSelectedScreen(AppData.ScreenType.LandingPageScreen, loadedScreenCallbackResults =>
                {
                    callbackResults.SetResult(loadedScreenCallbackResults);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }


        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
         
        }

        protected override void ScrollerPosition(Vector2 position)
        {

        }

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
            
        }

        protected override void OnScreenWidgetShownEvent()
        {
         
        }

        protected override void OnScreenWidgetHiddenEvent()
        {
     
        }

        protected override void OnScreenWidgetTransitionInProgressEvent()
        {

        }

        protected override void OnScreenWidget(Action<AppData.Callback> callback = null)
        {
            
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            
        }

        #endregion

    }
}