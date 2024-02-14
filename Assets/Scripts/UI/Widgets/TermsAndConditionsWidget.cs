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
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "Profile Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                profileManagerInstance.GetUserProfile(userProfileCallbackResults =>
                {
                    callbackResults.SetResult(userProfileCallbackResults);

                    if(callbackResults.Success())
                    {
                        var userProfile = userProfileCallbackResults.GetData();

                        callbackResults.SetResult(userProfile.GetTermsAndConditionsRead());

                        if(callbackResults.Success())
                        {
                            confirmationButtonEnabled = true;
                            SetActionButtonState(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, AppData.InputUIState.Enabled);

                            callbackResults.SetResult(userProfile.GetTermsAndConditionsAccepted());

                            if(callbackResults.Success())
                            {
                                SetActionButtonTitle(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, "Decline Terms", buttonTitleSetCallbackResults => 
                                {
                                    callbackResults.SetResult(buttonTitleSetCallbackResults);

                                    if(callbackResults.Success())
                                    {
                                        // Change Button Color
                                        SetActionButtonState(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, AppData.InputUIState.Selected, buttonStateCallbackResults => 
                                        {
                                            callbackResults.SetResult(buttonStateCallbackResults);

                                            if(callbackResults.Success())
                                                confirmationButtonEnabled = true;
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        });
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }
                            else
                            {
                                SetActionButtonTitle(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, "Accept Terms", buttonTitleSetCallbackResults => 
                                {
                                    callbackResults.SetResult(buttonTitleSetCallbackResults);

                                    if(callbackResults.Success())
                                    {
                                        // Change Button Color
                                        SetActionButtonState(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, AppData.InputUIState.Enabled, buttonStateCallbackResults => 
                                        {
                                            callbackResults.SetResult(buttonStateCallbackResults);

                                            if(callbackResults.Success())
                                                confirmationButtonEnabled = true;
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        });
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }
                        }
                        else
                        {
                            SetActionButtonTitle(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, "Accept Terms", buttonTitleSetCallbackResults => 
                            {
                                callbackResults.SetResult(buttonTitleSetCallbackResults);

                                if(callbackResults.Success())
                                {
                                    SetActionButtonState(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, AppData.InputUIState.Disabled, buttonStateCallbackResults => 
                                    {
                                        callbackResults.SetResult(buttonStateCallbackResults);

                                        if(callbackResults.Success())
                                            confirmationButtonEnabled = false;
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    });
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            });
                        }
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
            
        }

        //protected override void OnShowScreenWidget(Action<AppData.Callback> callback = null)
        //{
        //    var confirmationButtonState = (confirmationButtonEnabled) ? AppData.InputUIState.Enabled : AppData.InputUIState.Disabled;
        //    SetActionButtonState(AppData.InputActionButtonType.ConfirmationButton, confirmationButtonState);

        //    ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        //}

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            if (OnScrollEnded(value))
            {
                var callbackResults = new AppData.Callback();

                callbackResults.SetResult(GetActionButtonOfType(AppData.InputActionButtonType.AcceptTermsAndConditionsButton));

                if (callbackResults.Success())
                {
                    var button = GetActionButtonOfType(AppData.InputActionButtonType.AcceptTermsAndConditionsButton).GetData();

                    if (button.GetInputUIState() == AppData.InputUIState.Disabled)
                        button.SetUIInputState(AppData.InputUIState.Enabled);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                return;

            //scroller.Update();
        }

        private bool OnScrollEnded(Vector2 value) => value.y <= 0.0f;

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            var callbackResults = new AppData.Callback();

            callbackResults.SetResult(AppData.Helpers.GetAppEnumValueValid(actionType, "Action Type", $"On Action Button Event Failed - Action Type Parameter Value Is Set To Default : {actionType} - Invalid Operation"));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                    if (callbackResults.Success())
                    {
                        var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                        var signInWidgetConfig = new AppData.SceneConfigDataPacket();

                        signInWidgetConfig.SetReferencedWidgetType(AppData.WidgetType.SignInWidget);
                        signInWidgetConfig.blurScreen = true;

                        switch (actionType)
                        {
                            case AppData.InputActionButtonType.GoToWebsiteLinkButton:

                                break;

                            case AppData.InputActionButtonType.AcceptTermsAndConditionsButton:

                                callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.SignInWidget));

                                if (callbackResults.Success())
                                {
                                    var widget = screen.GetWidget(AppData.WidgetType.SignInWidget).GetData();

                                    callbackResults.SetResult(widget.GetTabViewComponent());

                                    if (callbackResults.Success())
                                    {
                                        callbackResults.SetResult(widget.GetTabViewComponent().GetData().GetTabView(AppData.TabViewType.SignUpView));

                                        if (callbackResults.Success())
                                        {
                                            var tabView = widget.GetTabViewComponent().GetData().GetTabView(AppData.TabViewType.SignUpView).GetData() as SignUpTabView;

                                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(tabView, "Tab View", "Accept Terms And Conditions Failed - Sign Up Tab View Couldn't Be Found - Invalid Operation."));

                                            if (callbackResults.Success())
                                            {
                                                tabView.ReadAndAcceptTermsAndConditions(termsAndConditionsAcceptedCallbackResults => 
                                                {
                                                    callbackResults.SetResult(termsAndConditionsAcceptedCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        screen.HideScreenWidget(this, widgetHiddenCallbackResults =>
                                                        {
                                                            callbackResults.SetResult(widgetHiddenCallbackResults);

                                                            if (callbackResults.Success())
                                                            {
                                                                screen.ShowWidget(signInWidgetConfig, widgetShownCallbackResults =>
                                                                {
                                                                    callbackResults.SetResult(widgetShownCallbackResults);
                                                                });
                                                            }
                                                            else
                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                        });
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                });                                            
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                break;

                            case AppData.InputActionButtonType.CloseButton:

                                screen.HideScreenWidget(this);

                                screen.ShowWidget(signInWidgetConfig);

                                break;
                        }
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            //AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, screenManagerComponentCallbackResults => 
            //{
            //    if(screenManagerComponentCallbackResults.Success())
            //    {
            //        var screenManager = screenManagerComponentCallbackResults.data;

            //        screenManager.GetScreen(screenManager.GetCurrentScreenType().GetData(), loadedScreenCallbacResults =>
            //        {
            //            if (loadedScreenCallbacResults.Success())
            //            {
            //                var loadedScreen = loadedScreenCallbacResults.data;

            //                switch (actionType)
            //                {
            //                    case AppData.InputActionButtonType.GoToWebsiteLinkButton:

            //                        AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, NetworkManager.Instance.name, async networkManagerCallbackResults =>
            //                        {
            //                            if (networkManagerCallbackResults.Success())
            //                            {
            //                                var success = await networkManagerCallbackResults.data.CheckConnectionStatus();

            //                                if (success.Success())
            //                                {
            //                                    if (!string.IsNullOrEmpty(dataPackets.externalLinkURL))
            //                                        Application.OpenURL(dataPackets.externalLinkURL);
            //                                    else
            //                                        LogError("Open Terms And Conditions Website Failed - External URL Is Not Assigned.", this);
            //                                }
            //                                else
            //                                {
            //                                    AppData.SceneConfigDataPacket networkDataPackets = new AppData.SceneConfigDataPacket();

            //                                    dataPackets.SetReferencedScreenType(AppData.ScreenType.LandingPageScreen);
            //                                    dataPackets.SetReferencedWidgetType(AppData.WidgetType.NetworkNotificationWidget);
            //                                    dataPackets.SetScreenBlurState(true);
            //                                    dataPackets.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.Default);

            //                                    loadedScreen.ShowWidget(networkDataPackets);
            //                                    LogError("Network Connection Failed : Show Network Error Pop-Up", this);
            //                                }
            //                            }
            //                            else
            //                                Log(networkManagerCallbackResults.resultCode, networkManagerCallbackResults.result, this);

            //                        }, "Network Manager Instance Is Not Yet Initialized");

            //                        break;

            //                    case AppData.InputActionButtonType.ConfirmationButton:

            //                        AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, ProfileManager.Instance.name, profileManagerCallbackResults =>
            //                        {
            //                            if(profileManagerCallbackResults.Success())
            //                            {
            //                                var profileManager = profileManagerCallbackResults.data;
            //                                profileManager.AcceptTermsAndConditions();
            //                            }
            //                            else
            //                                Log(profileManagerCallbackResults.resultCode, profileManagerCallbackResults.result, this);

            //                        }, "Profile Manager Instance Is Not Yet Initialized");

            //                        break;

            //                    case AppData.InputActionButtonType.CloseButton:

            //                        loadedScreen.HideScreenWidget(type);

            //                        break;
            //                }
            //            }
            //        });
            //    }
            //    else
            //        Log(screenManagerComponentCallbackResults.resultCode, screenManagerComponentCallbackResults.result, this);

            //}, "Screen UI Manager Instance Is Not Yet Initialized.");
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

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
           
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

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
         
        }

        #endregion
    }
}