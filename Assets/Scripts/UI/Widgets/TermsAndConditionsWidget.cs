using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class TermsAndConditionsWidget : AppData.Widget
    {
        #region Components

        bool confirmationButtonEnabled = false;

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            termsAndConditionsWidget = this;
            base.Init();
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
            var confirmationButtonState = (confirmationButtonEnabled) ? AppData.InputUIState.Enabled : AppData.InputUIState.Disabled;
            SetActionButtonState(AppData.InputActionButtonType.ConfirmationButton, confirmationButtonState);

            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, screenManagerComponentCallbackResults => 
            {
                if(screenManagerComponentCallbackResults.Success())
                {
                    var screenManager = screenManagerComponentCallbackResults.data;

                    screenManager.GetScreen(screenManager.GetCurrentUIScreenType(), loadedScreenCallbacResults =>
                    {
                        if (loadedScreenCallbacResults.Success())
                        {
                            var loadedScreen = loadedScreenCallbacResults.data;

                            switch (actionType)
                            {
                                case AppData.InputActionButtonType.GoToWebsiteLinkButton:

                                    AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, NetworkManager.Instance.name, async networkManagerCallbackResults =>
                                    {
                                        if (networkManagerCallbackResults.Success())
                                        {
                                            var success = await networkManagerCallbackResults.data.CheckConnectionStatus();

                                            if (success.Success())
                                            {
                                                if (!string.IsNullOrEmpty(dataPackets.externalLinkURL))
                                                    Application.OpenURL(dataPackets.externalLinkURL);
                                                else
                                                    LogError("Open Terms And Conditions Website Failed - External URL Is Not Assigned.", this);
                                            }
                                            else
                                            {
                                                AppData.SceneDataPackets networkDataPackets = new AppData.SceneDataPackets
                                                {
                                                    screenType = AppData.UIScreenType.LandingPageScreen,
                                                    widgetType = AppData.WidgetType.NetworkNotificationWidget,
                                                    blurScreen = true,
                                                    blurContainerLayerType = AppData.ScreenBlurContainerLayerType.Default
                                                };

                                                loadedScreen.value.ShowWidget(networkDataPackets);
                                                LogError("Network Connection Failed : Show Network Error Pop-Up", this);
                                            }
                                        }
                                        else
                                            Log(networkManagerCallbackResults.resultCode, networkManagerCallbackResults.result, this);

                                    }, "Network Manager Instance Is Not Yet Initialized");

                                    break;

                                case AppData.InputActionButtonType.ConfirmationButton:

                                    AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, ProfileManager.Instance.name, profileManagerCallbackResults =>
                                    {
                                        if(profileManagerCallbackResults.Success())
                                        {
                                            var profileManager = profileManagerCallbackResults.data;
                                            profileManager.AcceptTermsAndConditions();
                                        }
                                        else
                                            Log(profileManagerCallbackResults.resultCode, profileManagerCallbackResults.result, this);

                                    }, "Profile Manager Instance Is Not Yet Initialized");

                                    break;

                                case AppData.InputActionButtonType.CloseButton:

                                    loadedScreen.value.HideScreenWidget(widgetType);

                                    break;
                            }
                        }
                    });
                }
                else
                    Log(screenManagerComponentCallbackResults.resultCode, screenManagerComponentCallbackResults.result, this);

            }, "Screen UI Manager Instance Is Not Yet Initialized.");
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            if(confirmationButtonEnabled)
                return;

            if (scroller.value.verticalNormalizedPosition == 1.0f)
            {
                confirmationButtonEnabled = true;

                var confirmationButtonState = (confirmationButtonEnabled) ? AppData.InputUIState.Enabled : AppData.InputUIState.Disabled;
                SetActionButtonState(AppData.InputActionButtonType.ConfirmationButton, confirmationButtonState);
            }
        }

        #endregion
    }
}