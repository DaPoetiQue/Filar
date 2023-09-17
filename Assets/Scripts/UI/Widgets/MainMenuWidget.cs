using UnityEngine;

namespace Com.RedicalGames.Filar
{

    public class MainMenuWidget : AppData.Widget
    {
        #region Components

        #endregion

        #region Main

        protected override void Initialize() => mainMenuWidget = this;

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, screenUIManagerCallbackResults =>
            {
                if (screenUIManagerCallbackResults.Success())
                {
                    var screenUIManager = screenUIManagerCallbackResults.data;

                    switch (actionType)
                    {
                        case AppData.InputActionButtonType.OpenProfileButton:

                            AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, ProfileManager.Instance.name, profileManagerCallbackResults =>
                            {
                                if (profileManagerCallbackResults.Success())
                                {
                                    var profileManager = profileManagerCallbackResults.data;

                                    if (profileManager.SignedIn)
                                    {
                                        LogInfo(" <==========================> Open Profile", this);
                                    }
                                    else
                                    {
                                        screenUIManager.GetCurrentScreen(async currentScreenCallbackResults =>
                                        {
                                            if (currentScreenCallbackResults.Success())
                                            {
                                                var currentScreen = currentScreenCallbackResults.data;

                                                currentScreen.value.GetWidget(AppData.WidgetType.PostsWidget).SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Hidden);

                                                currentScreen.value.GetWidget(AppData.WidgetType.PostsWidget).SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Shown);

                                                await currentScreen.value.HideScreenWidgetAsync(AppData.WidgetType.PostsWidget);


                                                AppData.SceneDataPackets dataPackets = new AppData.SceneDataPackets
                                                {
                                                    screenType = AppData.UIScreenType.LandingPageScreen,
                                                    widgetType = AppData.WidgetType.SignInWidget,
                                                    blurScreen = true,
                                                    blurContainerLayerType = AppData.ScreenBlurContainerLayerType.Background
                                                };

                                                currentScreen.value.ShowWidget(dataPackets);
                                            }
                                        });
                                    }
                                }

                            }, "Profile Manager Instance Is Not Yet Initialized.");

                            break;
                    }
                }

            }, "Screen UI Manager Instance Is Not Yet Initialized.");
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(defaultLayoutType);
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
            ShowSelectedLayout(defaultLayoutType);
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets) => ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            LogInfo($"===============> Subscribe : {subscribe}", this);
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
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