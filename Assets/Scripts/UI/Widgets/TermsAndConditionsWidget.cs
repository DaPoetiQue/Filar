using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class TermsAndConditionsWidget : AppData.Widget
    {
        #region Components

        private bool confirmationButtonEnabled = false;

        private AppData.ActionButtonListener confirmationButtonEvent = new AppData.ActionButtonListener();
        private AppData.ActionButtonListener cancelationButtonEvent = new AppData.ActionButtonListener();

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

                        if (callbackResults.Success())
                        {
                            confirmationButtonEnabled = true;
                            SetActionButtonState(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, AppData.InputUIState.Enabled);

                            callbackResults.SetResult(userProfile.GetTermsAndConditionsAccepted());

                            if (callbackResults.Success())
                            {
                                SetActionButtonTitle(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, "Decline Terms", buttonTitleSetCallbackResults =>
                                {
                                    callbackResults.SetResult(buttonTitleSetCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        // Change Button Color
                                        SetActionButtonState(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, AppData.InputUIState.Selected, buttonStateCallbackResults =>
                                        {
                                            callbackResults.SetResult(buttonStateCallbackResults);

                                            if (callbackResults.Success())
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

                                    if (callbackResults.Success())
                                    {
                                        HighlightButton(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, callback: highlightedButtonCallbackResults =>
                                        {
                                            callbackResults.SetResult(highlightedButtonCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                // Change Button Color
                                                SetActionButtonState(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, AppData.InputUIState.Enabled, buttonStateCallbackResults =>
                                                {
                                                    callbackResults.SetResult(buttonStateCallbackResults);

                                                    if (callbackResults.Success())
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
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }
                        }
                        else
                        {
                            callbackResults.SetResult(userProfile.GetTermsAndConditionsAccepted());

                            if (callbackResults.Success())
                            {
                                SetActionButtonTitle(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, "Decline Terms", buttonTitleSetCallbackResults =>
                                {
                                    callbackResults.SetResult(buttonTitleSetCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        SetActionButtonState(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, AppData.InputUIState.Disabled, buttonStateCallbackResults =>
                                        {
                                            callbackResults.SetResult(buttonStateCallbackResults);

                                            if (callbackResults.Success())
                                                confirmationButtonEnabled = false;
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

                                    if (callbackResults.Success())
                                    {
                                        SetActionButtonState(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, AppData.InputUIState.Disabled, buttonStateCallbackResults =>
                                        {
                                            callbackResults.SetResult(buttonStateCallbackResults);

                                            if (callbackResults.Success())
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
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
            
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            if (OnScrollEnded(value))
            {
                var callbackResults = new AppData.Callback();

                callbackResults.SetResult(GetActionButtonOfType(AppData.InputActionButtonType.AcceptTermsAndConditionsButton));

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "Profile Manager Instance Is Not Yet Initialized."));

                    if(callbackResults.Success())
                    {
                        var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                        profileManagerInstance.GetUserProfile(userProfileCallbackResults =>
                        {
                            callbackResults.SetResult(userProfileCallbackResults);

                            if(callbackResults.Success())
                            {
                                var userProfile = userProfileCallbackResults.GetData();
                        
                                var button = GetActionButtonOfType(AppData.InputActionButtonType.AcceptTermsAndConditionsButton).GetData();

                                callbackResults.SetResult(userProfile.GetTermsAndConditionsAccepted());

                                if(callbackResults.Success())
                                {
                                    SetActionButtonTitle(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, "Decline Terms", buttonTitleSetCallbackResults => 
                                    {
                                        callbackResults.SetResult(buttonTitleSetCallbackResults);

                                        if(callbackResults.Success())
                                        {
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
                                            HighlightButton(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, callback: highlightedButtonCallbackResults =>
                                            {
                                                callbackResults.SetResult(highlightedButtonCallbackResults);

                                                if (callbackResults.Success())
                                                {
                                                    SetActionButtonState(AppData.InputActionButtonType.AcceptTermsAndConditionsButton, AppData.InputUIState.Enabled, buttonStateCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(buttonStateCallbackResults);

                                                        if (callbackResults.Success())
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
                                                        screen.HideWidget(this, widgetHiddenCallbackResults =>
                                                        {
                                                            callbackResults.SetResult(widgetHiddenCallbackResults);

                                                            if (callbackResults.Success())
                                                            {
                                                                screen.ShowWidget(AppData.WidgetType.SignInWidget, widgetShownCallbackResults =>
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





                                callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.GetName(), "App Database Manager Instance Is Not Yet Initialized."));

                                if (callbackResults.Success())
                                {
                                    var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.GetName()).GetData();

                                    callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                                    if (callbackResults.Success())
                                    {
                                        var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                                        callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget));

                                        if (callbackResults.Success())
                                        {
                                            callbackResults.SetResult(assetBundlesLibrary.GetLoadedConfigMessageDataPacket(AppData.SurfacingContentType.OnClosePopUpWarningMessage));

                                            if (callbackResults.Success())
                                            {
                                                var closePopUpnMessageDataObject = assetBundlesLibrary.GetLoadedConfigMessageDataPacket(AppData.SurfacingContentType.OnClosePopUpWarningMessage).GetData();

                                                var confirmationPopUpWidget = screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget).GetData();

                                                callbackResults.SetResult(closePopUpnMessageDataObject.GetTitle());

                                                if (callbackResults.Success())
                                                {
                                                    confirmationPopUpWidget.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, closePopUpnMessageDataObject.GetTitle().GetData(), verificationTitleSetCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(verificationTitleSetCallbackResults);

                                                        if (callbackResults.Success())
                                                        {
                                                            callbackResults.SetResult(closePopUpnMessageDataObject.GetMessage());

                                                            if (callbackResults.Success())
                                                            {
                                                            string closePopUpnMessage = closePopUpnMessageDataObject.GetMessage("App Terms And Conditions Screen").GetData();

                                                                confirmationPopUpWidget.SetUITextDisplayerValue(AppData.ScreenTextType.MessageDisplayer, closePopUpnMessage, verificationMessageSetCallbackResults =>
                                                                {
                                                                    callbackResults.SetResult(verificationMessageSetCallbackResults);

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        confirmationPopUpWidget.SetActionButtonTitle(AppData.InputActionButtonType.ConfirmationButton, "Close", closeButtonCallbackResults => 
                                                                        {
                                                                            callbackResults.SetResult(closeButtonCallbackResults);

                                                                            if(callbackResults.Success())
                                                                            {
                                                                                confirmationPopUpWidget.SetActionButtonTitle(AppData.InputActionButtonType.Cancel, "Cancel", cancelButtonCallbackResults =>
                                                                                {
                                                                                    callbackResults.SetResult(cancelButtonCallbackResults);

                                                                                    if (callbackResults.Success())
                                                                                    {
                                                                                        confirmationButtonEvent.SetAction(AppData.InputActionButtonType.ConfirmationButton, confirmationButtonActionCallbackResults =>
                                                                                        {
                                                                                            callbackResults.SetResult(confirmationButtonActionCallbackResults);

                                                                                            if (callbackResults.Success())
                                                                                            {
                                                                                                confirmationButtonEvent.SetMethod(OnConfirmButtonPressedEvent, confirmationButtonMethodCallbackResults =>
                                                                                                {
                                                                                                    callbackResults.SetResult(confirmationButtonMethodCallbackResults);

                                                                                                    if (callbackResults.Success())
                                                                                                    {
                                                                                                        cancelationButtonEvent.SetAction(AppData.InputActionButtonType.Cancel, cancelButtonActionCallbackResults =>
                                                                                                        {
                                                                                                            callbackResults.SetResult(cancelButtonActionCallbackResults);

                                                                                                            if (callbackResults.Success())
                                                                                                            {
                                                                                                                cancelationButtonEvent.SetMethod(OnCancelButtonPressedEvent, cancelButtonMethodCallbackResults =>
                                                                                                                {
                                                                                                                    callbackResults.SetResult(cancelButtonMethodCallbackResults);

                                                                                                                    if (callbackResults.Success())
                                                                                                                    {
                                                                                                                        confirmationPopUpWidget.RegisterActionButtonListeners(buttonEventsRegisteredCallbackResults => 
                                                                                                                        {
                                                                                                                            callbackResults.SetResult(buttonEventsRegisteredCallbackResults);

                                                                                                                            if(callbackResults.Success())
                                                                                                                            {
                                                                                                                                screen.ShowWidget(confirmationPopUpWidget, confirmationPopUpWidgetShownCallbackResults =>
                                                                                                                                {
                                                                                                                                    callbackResults.SetResult(confirmationPopUpWidgetShownCallbackResults);

                                                                                                                                    if (callbackResults.UnSuccessful())
                                                                                                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                                                                                });
                                                                                                                            }
                                                                                                                            else
                                                                                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                                                                                        }, confirmationButtonEvent, cancelationButtonEvent);
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
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                break;
                        }
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnConfirmButtonPressedEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On Cancel Button Pressed Event Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    screen.HideWidget(AppData.WidgetType.ConfirmationPopUpWidget, confirmationPopUpWidgetHiddenCallbackResults => 
                    {
                        callbackResults.SetResult(confirmationPopUpWidgetHiddenCallbackResults);
                    
                        if(callbackResults.Success())
                        {
                            screen.HideWidget(AppData.WidgetType.TermsAndConditionsWidget, termsAndConditionsWidgetHiddencallbackResults =>
                            {
                                callbackResults.SetResult(termsAndConditionsWidgetHiddencallbackResults);

                                if (callbackResults.Success())
                                {
                                    screen.ShowWidget(AppData.WidgetType.SignInWidget, signInWidgetShownCallbackResults =>
                                    {
                                        callbackResults.SetResult(signInWidgetShownCallbackResults);

                                        if (callbackResults.Success())
                                        {
                                            callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget));

                                            if(callbackResults.Success())
                                            {
                                                var confirmationWidget = screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget).GetData();

                                                confirmationWidget.UnRegisterActionButtonListeners(buttonEventsunsubscribedCallbackResults => 
                                                {
                                                    callbackResults.SetResult(buttonEventsunsubscribedCallbackResults);

                                                    if(callbackResults.UnSuccessful())
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                }, confirmationButtonEvent, cancelationButtonEvent);
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnCancelButtonPressedEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On Cancel Button Pressed Event Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if(callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if(callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget));

                    if (callbackResults.Success())
                    {
                        var confirmationPopUpWidget = screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget).GetData();

                        callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.TermsAndConditionsWidget).GetData().GetScreenBlurConfig());

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.TermsAndConditionsWidget).GetData().GetScreenBlurConfig().GetData().Initialized());

                            if (callbackResults.Success())
                            {
                                screen.HideWidget(confirmationPopUpWidget, confirmationPopUpWidgetHiddencallbackResults =>
                                {
                                    callbackResults.SetResult(confirmationPopUpWidgetHiddencallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        confirmationPopUpWidget.UnRegisterActionButtonListeners(buttonEventsUnregisteredCallbackResults =>
                                        {
                                            callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget));

                                            if (callbackResults.Success())
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                        }, confirmationButtonEvent, cancelationButtonEvent);
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                }, screen.GetWidget(AppData.WidgetType.TermsAndConditionsWidget).GetData().GetScreenBlurConfig().GetData());
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
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            // if(confirmationButtonEnabled)
            //     return;

            // if (scroller.value.verticalNormalizedPosition == 1.0f)
            // {
            //     confirmationButtonEnabled = true;

            //     var confirmationButtonState = (confirmationButtonEnabled) ? AppData.InputUIState.Enabled : AppData.InputUIState.Disabled;
            //     SetActionButtonState(AppData.InputActionButtonType.ConfirmationButton, confirmationButtonState);
            // }
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