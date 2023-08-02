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

            AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name, sceneAssetsManagerCallbackResults => 
            {
                if (sceneAssetsManagerCallbackResults.Success())
                {
                    var widgetView = GetLayoutView(AppData.WidgetLayoutViewType.DefaultView);

                    transitionable = new AppData.TransitionableUI(widgetContainer.value);
                    transitionable.SetSpeed(sceneAssetsManagerCallbackResults.data.GetDefaultExecutionValue(AppData.RuntimeValueType.ScreenWidgetTransitionalSpeed).value);

                    base.Init();
                }
                else
                    Log(sceneAssetsManagerCallbackResults.resultsCode, sceneAssetsManagerCallbackResults.results, this);
            
            }, "Scene Assets Manager Instance Is Not Yet Initialized.");
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
            if (actionType == AppData.InputActionButtonType.SignUp_SignIn_SelectionButton)
            {
                isInitialView = !isInitialView;

                if (isInitialView)
                    transitionable.SetTarget(widgetContainer.visibleScreenPoint);
                else
                    transitionable.SetTarget(widgetContainer.hiddenScreenPoint);

                SwitchPage();
            }
        }

        async void SwitchPage()
        {

            LogInfo(" <+++++++++++++> Go To Login Screen", this);

            SetActionButtonState(AppData.InputActionButtonType.SignUp_SignIn_SelectionButton, AppData.InputUIState.Disabled);

            await transitionable.TransitionAsync();

            string buttonTitle = (isInitialView) ? "Sign In" : "Sign Up";

            SetActionButtonTitle(AppData.InputActionButtonType.SignUp_SignIn_SelectionButton, buttonTitle);
            SetActionButtonState(AppData.InputActionButtonType.SignUp_SignIn_SelectionButton, AppData.InputUIState.Enabled);

            LogInfo(" <+++++++++++++> Went To Login Screen", this);
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}