using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class PostsWidget : AppData.Widget
    {
        #region Components

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            postsWidget = this;
            base.Init();
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
                                case AppData.InputActionButtonType.ShowPostsButton:

                                    screen.value.GetWidget(this).SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Hidden);
                                    screen.value.GetWidget(this).SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Shown);

                                    screen.value.ShowWidget(this);

                                    break;

                                case AppData.InputActionButtonType.HidePostsButton:

                                    screen.value.GetWidget(this).SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Hidden);
                                    screen.value.GetWidget(this).SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Shown);

                                    await screen.value.HideScreenWidgetAsync(this);

                                    break;
                            }
                        }
                        else
                            Log(currentScreenCallbackResults.ResultCode, currentScreenCallbackResults.Result, this);

                    });
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