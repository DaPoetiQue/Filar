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
<<<<<<< HEAD
                    transitionable.SetSpeed(sceneAssetsManagerCallbackResults.data.GetDefaultExecutionValue(AppData.RuntimeValueType.ScreenWidgetTransitionalSpeed).value);
=======
                    transitionable.SetSpeed(sceneAssetsManagerCallbackResults.data.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetTransitionalSpeed).value);
>>>>>>> Initialization

                    base.Init();
                }
                else
<<<<<<< HEAD
                    Log(sceneAssetsManagerCallbackResults.resultsCode, sceneAssetsManagerCallbackResults.results, this);
=======
                    Log(sceneAssetsManagerCallbackResults.resultCode, sceneAssetsManagerCallbackResults.result, this);
>>>>>>> Initialization
            
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
<<<<<<< HEAD
            if (actionType == AppData.InputActionButtonType.SignUp_SignIn_SelectionButton)
=======
            if (actionType == AppData.InputActionButtonType.SignInViewChangeButton)
>>>>>>> Initialization
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

<<<<<<< HEAD
            SetActionButtonState(AppData.InputActionButtonType.SignUp_SignIn_SelectionButton, AppData.InputUIState.Disabled);
=======
            SetActionButtonState(AppData.InputActionButtonType.SignInViewChangeButton, AppData.InputUIState.Disabled);
>>>>>>> Initialization

            await transitionable.TransitionAsync();

            string buttonTitle = (isInitialView) ? "Sign In" : "Sign Up";

<<<<<<< HEAD
            SetActionButtonTitle(AppData.InputActionButtonType.SignUp_SignIn_SelectionButton, buttonTitle);
            SetActionButtonState(AppData.InputActionButtonType.SignUp_SignIn_SelectionButton, AppData.InputUIState.Enabled);
=======
            SetActionButtonTitle(AppData.InputActionButtonType.SignInViewChangeButton, buttonTitle);
            SetActionButtonState(AppData.InputActionButtonType.SignInViewChangeButton, AppData.InputUIState.Enabled);
>>>>>>> Initialization

            LogInfo(" <+++++++++++++> Went To Login Screen", this);
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

<<<<<<< HEAD
=======
        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

>>>>>>> Initialization
        #endregion
    }
}