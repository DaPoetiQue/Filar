using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SignInWidget : AppData.Widget
    {
        #region Components

        bool isInitialView = true;

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

        void ReferenceAnddelete()
        {

            //AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, databaseManagerCallbackResults =>
            //{
            //    callbackResults.SetResult(databaseManagerCallbackResults);

            //    if (databaseManagerCallbackResults.Success())
            //    {
            //        callbackResults.SetResult(GetType());

            //        if (callbackResults.Success())
            //        {
            //            OnRegisterWidget(this, onRegisterWidgetCallbackResults =>
            //            {
            //                callbackResults.SetResult(GetType());

            //                if (callbackResults.Success())
            //                {
            //                    var databaseManager = databaseManagerCallbackResults.data;
            //                    var widgetView = GetLayoutView(AppData.WidgetLayoutViewType.DefaultView);

            //                    transitionableUIComponent = new AppData.TransitionableUIComponent(widgetContainer.value, AppData.UITransitionType.Translate, AppData.UITransitionStateType.Once);
            //                    transitionableUIComponent.SetTransitionSpeed(databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetTransitionalSpeed).value);

            //                    var widgetStatePacket = new AppData.WidgetStatePacket(name: GetName(), type: GetType().data, stateType: AppData.WidgetStateType.Initialized, value: this);

            //                    callbackResults.result = $"Widget : {GetName()} Of Type : {GetType().data}'s State Packet Has Been Initialized Successfully.";
            //                    callbackResults.data = widgetStatePacket;
            //                }
            //                else
            //                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //            });
            //        }
            //        else
            //            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //    }
            //    else
            //        Log(databaseManagerCallbackResults.resultCode, databaseManagerCallbackResults.result, this);

            //}, "App Database Manager Instance Is Not Yet Initialized.");
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
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenWidget(Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, screenUIManagerCallbackResults =>
            {
                if (screenUIManagerCallbackResults.Success())
                {
                    var screenUIManager = screenUIManagerCallbackResults.data;

                    screenUIManager.GetCurrentScreen(async currentScreenCallbackResults =>
                    {
                        if (currentScreenCallbackResults.Success())
                        {
                            var screen = currentScreenCallbackResults.data;

                            switch (actionType)
                            {
                                case AppData.InputActionButtonType.Cancel:

                                    screen.HideScreenWidget(this);

                                    break;

                                case AppData.InputActionButtonType.SignInViewChangeButton:

                                    isInitialView = !isInitialView;

                                    //if (isInitialView)
                                    //    GetTransitionableUIComponent().SetTransitionDestination(widgetContainer.visibleScreenPoint);
                                    //else
                                    //    GetTransitionableUIComponent().SetTransitionDestination(widgetContainer.hiddenScreenPoint);

                                    SwitchPage();

                                    break;
                            }
                        }
                        else
                            Log(currentScreenCallbackResults.GetResultCode, currentScreenCallbackResults.GetResult, this);

                    });
                }

            }, "Screen UI Manager Instance Is Not Yet Initialized.");

        }

        void SwitchPage()
        {

            LogInfo(" <+++++++++++++> Go To Login Screen", this);

            SetActionButtonState(AppData.InputActionButtonType.SignInViewChangeButton, AppData.InputUIState.Disabled);

            //await GetTransitionableUIComponent().InvokeTransition();

            string buttonTitle = (isInitialView) ? "Sign In" : "Sign Up";

            SetActionButtonTitle(AppData.InputActionButtonType.SignInViewChangeButton, buttonTitle);
            SetActionButtonState(AppData.InputActionButtonType.SignInViewChangeButton, AppData.InputUIState.Enabled);

            LogInfo(" <+++++++++++++> Went To Login Screen", this);
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
            throw new NotImplementedException();
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

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
           
        }

        #endregion
    }
}