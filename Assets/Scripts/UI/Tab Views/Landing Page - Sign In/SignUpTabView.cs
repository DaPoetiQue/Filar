using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SignUpTabView : AppData.TabView<AppData.WidgetType>
    {
        #region Components

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.TabViewType, AppData.TabViewType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.TabViewType, AppData.TabViewType, AppData.Widget>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override void OnActionButtonEvent(AppData.TabViewType screenWidgetType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            var callbackResults = new AppData.Callback();

            switch (actionType)
            {
                case AppData.InputActionButtonType.ReadButton:

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

                    if (callbackResults.Success())
                    {
                        var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                        callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                        if (callbackResults.Success())
                        {
                            var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                            screen.HideScreenWidget(AppData.WidgetType.SignInWidget);

                            var readTermsAndConditionsWidgetConfig = new AppData.SceneConfigDataPacket();

                            readTermsAndConditionsWidgetConfig.SetReferencedWidgetType(AppData.WidgetType.TermsAndConditionsWidget);
                            readTermsAndConditionsWidgetConfig.blurScreen = true;

                            screen.ShowWidget(readTermsAndConditionsWidgetConfig);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }

                    break;
            }
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
           
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.TabViewType, AppData.TabViewType, AppData.Widget>> OnGetState()
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

        #endregion
    }
}