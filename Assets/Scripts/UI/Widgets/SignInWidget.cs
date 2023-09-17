using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SignInWidget : AppData.Widget
    {
        #region Components

        AppData.TransitionableUIComponent transitionableUIComponent;

        bool isInitialView = true;

        #endregion

        #region Main

        protected override void Initialize()
        {
            signInWidget = this;

            AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, databaseManagerCallbackResults =>
            {
                if (databaseManagerCallbackResults.Success())
                {
                    var databaseManager = databaseManagerCallbackResults.data;

                    var widgetView = GetLayoutView(AppData.WidgetLayoutViewType.DefaultView);

                    transitionableUIComponent = new AppData.TransitionableUIComponent(widgetContainer.value, AppData.UITransitionType.Translate, AppData.UITransitionStateType.Once);
                    transitionableUIComponent.SetTransitionSpeed(databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetTransitionalSpeed).value);
                }
                else
                    Log(databaseManagerCallbackResults.resultCode, databaseManagerCallbackResults.result, this);

            }, "App Database Manager Instance Is Not Yet Initialized.");
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenWidget()
        {

        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            LogInfo(" <============================================> Show Widget Now!", this);

            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
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

                                    await screen.value.HideScreenWidgetAsync(this);

                                    break;

                                case AppData.InputActionButtonType.SignInViewChangeButton:

                                    isInitialView = !isInitialView;

                                    if (isInitialView)
                                        GetTransitionableUIComponent().SetTarget(widgetContainer.visibleScreenPoint);
                                    else
                                        GetTransitionableUIComponent().SetTarget(widgetContainer.hiddenScreenPoint);

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

        async void SwitchPage()
        {

            LogInfo(" <+++++++++++++> Go To Login Screen", this);

            SetActionButtonState(AppData.InputActionButtonType.SignInViewChangeButton, AppData.InputUIState.Disabled);

            await GetTransitionableUIComponent().InvokeTransitionAsync();

            string buttonTitle = (isInitialView) ? "Sign In" : "Sign Up";

            SetActionButtonTitle(AppData.InputActionButtonType.SignInViewChangeButton, buttonTitle);
            SetActionButtonState(AppData.InputActionButtonType.SignInViewChangeButton, AppData.InputUIState.Enabled);

            LogInfo(" <+++++++++++++> Went To Login Screen", this);
        }

        AppData.TransitionableUIComponent GetTransitionableUIComponent() => transitionableUIComponent;

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}