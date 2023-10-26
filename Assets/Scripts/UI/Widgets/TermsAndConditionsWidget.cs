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

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
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
            
        }

        protected override void OnShowScreenWidget(Action<AppData.Callback> callback = null)
        {
            var confirmationButtonState = (confirmationButtonEnabled) ? AppData.InputUIState.Enabled : AppData.InputUIState.Disabled;
            SetActionButtonState(AppData.InputActionButtonType.ConfirmationButton, confirmationButtonState);

            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, screenManagerComponentCallbackResults => 
            {
                if(screenManagerComponentCallbackResults.Success())
                {
                    var screenManager = screenManagerComponentCallbackResults.data;

                    screenManager.GetScreen(screenManager.GetCurrentScreenType().GetData(), loadedScreenCallbacResults =>
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
                                                AppData.SceneConfigDataPacket networkDataPackets = new AppData.SceneConfigDataPacket();

                                                dataPackets.SetReferencedScreenType(AppData.ScreenType.LandingPageScreen);
                                                dataPackets.SetReferencedWidgetType(AppData.WidgetType.NetworkNotificationWidget);
                                                dataPackets.SetScreenBlurState(true);
                                                dataPackets.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.Default);

                                                loadedScreen.ShowWidget(networkDataPackets);
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

                                    loadedScreen.HideScreenWidget(type);

                                    break;
                            }
                        }
                    });
                }
                else
                    Log(screenManagerComponentCallbackResults.resultCode, screenManagerComponentCallbackResults.result, this);

            }, "Screen UI Manager Instance Is Not Yet Initialized.");
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
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