using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class PostsWidget : AppData.Widget
    {
        #region Components

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

        protected override void OnActionButtonEvent(AppData.WidgetType screenWidgetType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

            if (screenWidgetType == AppData.WidgetType.PostsWidget)
            {
                if (callbackResults.Success())
                {
                    var screenUIManager = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                    screenUIManager.GetCurrentScreen(currentScreenCallbackResults =>
                    {
                        callbackResults.SetResult(currentScreenCallbackResults);

                        if (currentScreenCallbackResults.Success())
                        {
                            var screen = currentScreenCallbackResults.GetData();

                            switch (actionType)
                            {
                                case AppData.InputActionButtonType.ShowPostsButton:

                                    if (screen.IsFocusedWidget(AppData.WidgetType.HomeMenuWidget).Success())
                                    {
                                        SetActionButtonTitle(AppData.InputActionButtonType.ShowPostsButton, "Posts");
                                        screen.HideScreenWidget(AppData.WidgetType.HomeMenuWidget);
                                    }
                                    else
                                    {
                                        SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Shown);
                                        SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Hidden);

                                        screen.ShowWidget(this);
                                    }

                                    break;

                                case AppData.InputActionButtonType.HidePostsButton:

                                    SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Hidden);
                                    SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Shown);

                                    SetActionButtonTitle(AppData.InputActionButtonType.ShowPostsButton, "Posts");

                                    screen.HideScreenWidget(this);

                                    break;
                            }
                        }
                        else
                            Log(currentScreenCallbackResults.GetResultCode, currentScreenCallbackResults.GetResult, this);

                    });
                }
            }
        }

        public void OnPostsInitializationCompletedEvent()
        {
            var callbackResults = new AppData.Callback();

            LogInfo($" **********-*******-** Show Widget Type : {GetType().GetData()}", this);

            //ShowWidget(GetType().GetData(), showWidgetCallbackResults => 
            //{
            
            //});
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

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetShownEvent()
        {    
            //SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Shown);
            SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Hidden);
        }

        protected override void OnScreenWidgetHiddenEvent()
        {
            SetActionButtonState(AppData.InputActionButtonType.ShowPostsButton, AppData.InputUIState.Shown);
            //SetActionButtonState(AppData.InputActionButtonType.HidePostsButton, AppData.InputUIState.Hidden);
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