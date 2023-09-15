using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SignInWidget : AppData.Widget
    {
        #region Components

        AppData.TransitionableUI transitionable;

        bool isInitialView = true;

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            signInWidget = this;

            AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, databaseManagerCallbackResults => 
            {
                if (databaseManagerCallbackResults.Success())
                {
                    var widgetView = GetLayoutView(AppData.WidgetLayoutViewType.DefaultView);

                    transitionable = new AppData.TransitionableUI(widgetContainer.value);
                    transitionable.SetSpeed(databaseManagerCallbackResults.data.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetTransitionalSpeed).value);

                    base.Init();
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
                                        transitionable.SetTarget(widgetContainer.visibleScreenPoint);
                                    else
                                        transitionable.SetTarget(widgetContainer.hiddenScreenPoint);

                                    SwitchPage();

                                    break;
                            }
                        }
                        else
                            Log(currentScreenCallbackResults.ResultCode, currentScreenCallbackResults.Result, this);

                    });
                }

            }, "Screen UI Manager Instance Is Not Yet Initialized.");

        }

        async void SwitchPage()
        {

            LogInfo(" <+++++++++++++> Go To Login Screen", this);

            SetActionButtonState(AppData.InputActionButtonType.SignInViewChangeButton, AppData.InputUIState.Disabled);

            await transitionable.TransitionAsync();

            string buttonTitle = (isInitialView) ? "Sign In" : "Sign Up";

            SetActionButtonTitle(AppData.InputActionButtonType.SignInViewChangeButton, buttonTitle);
            SetActionButtonState(AppData.InputActionButtonType.SignInViewChangeButton, AppData.InputUIState.Enabled);

            LogInfo(" <+++++++++++++> Went To Login Screen", this);
        }

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