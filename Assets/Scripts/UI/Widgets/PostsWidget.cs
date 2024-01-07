using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class PostsWidget : AppData.Widget
    {
        #region Components

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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
                                case AppData.InputActionButtonType.ShowPostsButton:

                                    screen.GetWidget(this).SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Hidden);
                                    screen.GetWidget(this).SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Shown);

                                    screen.ShowWidget(this);

                                    break;

                                case AppData.InputActionButtonType.HidePostsButton:

                                    screen.GetWidget(this).SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Hidden);
                                    screen.GetWidget(this).SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Shown);

                                    await screen.HideScreenWidgetAsync(this);

                                    break;
                            }
                        }
                        else
                            Log(currentScreenCallbackResults.GetResultCode, currentScreenCallbackResults.GetResult, this);

                    });
                }

            }, "Screen UI Manager Instance Is Not Yet Initialized.");
        }

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {
            HideSelectedLayout(defaultLayoutType);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket)
        {
            ShowSelectedLayout(defaultLayoutType);
        }

        protected override void OnShowScreenWidget(Action<AppData.Callback> callback = null) => ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}