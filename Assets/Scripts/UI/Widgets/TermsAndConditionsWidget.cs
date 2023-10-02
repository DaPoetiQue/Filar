using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class TermsAndConditionsWidget : AppData.Widget
    {
        #region Components

        bool confirmationButtonEnabled = false;

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket>> callback)
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>();

            callbackResults.SetResult(GetType());

            if (callbackResults.Success())
            {
                OnRegisterWidget(this, onRegisterWidgetCallbackResults =>
                {
                    callbackResults.SetResult(GetType());

                    if (callbackResults.Success())
                    {
                        var widgetStatePacket = new AppData.WidgetStatePacket(name: GetName(), type: GetType().data, stateType: AppData.WidgetStateType.Initialized, value: this);

                        callbackResults.result = $"Widget : {GetName()} Of Type : {GetType().data}'s State Packet Has Been Initialized Successfully.";
                        callbackResults.data = widgetStatePacket;
                    }
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback.Invoke(callbackResults);
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

        protected override AppData.CallbackData<AppData.WidgetStatePacket> OnGetState()
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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

        #endregion
    }
}