using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SignUpTabView : AppData.TabView<AppData.WidgetType>
    {
        #region Components

        #region Network Events

        private AppData.ActionButtonListener onNetworkRetryButtonEvent = new AppData.ActionButtonListener();
        private AppData.ActionButtonListener onNetworkCancelButtonEvent = new AppData.ActionButtonListener();

        #endregion

        #region Sign Up Verification Events

        private AppData.ActionButtonListener onResendEmailButtonEvent = new AppData.ActionButtonListener();
        private AppData.ActionButtonListener onIncorrectEmailButtonEvent = new AppData.ActionButtonListener();

        #endregion


        #region Sign Up Complete Events

        private AppData.ActionButtonListener onCompletedSignUpConfirmButtonEvent = new AppData.ActionButtonListener();

        #endregion

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.TabViewType, AppData.TabViewType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.TabViewType, AppData.TabViewType, AppData.Widget>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);

                if(callbackResults.Success())
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "App Events Manager Instance Is Not Yet Initialized."));

                    if (callbackResults.Success())
                    {
                        var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                        appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnWidgetShown, AppData.EventType.OnWidgetShownEvent, true);
                        appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnWidgetHidden, AppData.EventType.OnWidgetHiddenEvent, true);
                        appEventsManagerInstance.OnEventSubscription<AppData.TabView<AppData.WidgetType>>(OnTabViewShownEvent, AppData.EventType.OnTabViewShownEvent, true);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            });

            callback.Invoke(callbackResults);
        }

        #region On Widget Events

        private void OnWidgetShown(AppData.Widget widget)
        {
            var callbackResults = new AppData.Callback(widget.GetType());

            if (callbackResults.Success())
            {
                if (widget.GetType().GetData() == AppData.WidgetType.SignInWidget)
                {
                    HighlightInputField(AppData.InputFieldActionType.UserNameField, callback: fieldHighlightedCallbackResults =>
                    {
                        callbackResults.SetResult(fieldHighlightedCallbackResults);
                    });
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnWidgetHidden(AppData.Widget widget)
        {
            var callbackResults = new AppData.Callback(widget.GetType());

            if (callbackResults.Success())
            {
                if (widget.GetType().GetData() == AppData.WidgetType.SignInWidget)
                {
                    HighlightInputField(AppData.InputFieldActionType.UserNameField, false, fieldHighlightedCallbackResults =>
                    {
                        callbackResults.SetResult(fieldHighlightedCallbackResults);

                        if(callbackResults.Success())
                        {
                            callbackResults.SetResult(GetValidatableInputFields());

                            if(callbackResults.Success())
                            {
                                var validatedFields = GetValidatableInputFields().GetData().FindAll(field => field.GetValidationResults().GetData() != AppData.ValidationResultsType.Default);

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(validatedFields, "Validated Fields", $"Clear Validated Fields On Widget Hide Failed - Couldn't Find Validated Fields For : {GetName()} - Continuing Execution."));

                                if (callbackResults.Success())
                                {
                                    for (int i = 0; i < validatedFields.Count; i++)
                                    {
                                        OnClearInputFieldValidation(validatedFields[i].GetDataPackets().GetData().GetAction().GetData(), clearValidationCallbackResults =>
                                        {
                                            callbackResults.SetResult(clearValidationCallbackResults);
                                        });

                                        if (callbackResults.UnSuccessful())
                                        {
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            break;
                                        }
                                    }
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    });
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        #endregion

        #region On tab View Events

        private void OnTabViewShownEvent(AppData.TabView<AppData.WidgetType> tabView)
        {
            var callbackResults = new AppData.Callback(tabView.GetType());

            if (callbackResults.Success())
            {
                if (tabView.GetType().GetData() == AppData.TabViewType.SignInView)
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "On Tab View Shown Event Failed - App Database Manager Instance Is Not Yet Initialized - Invalid Operation."));

                    if (callbackResults.Success())
                    {
                        var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                        callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                        if (callbackResults.Success())
                        {
                            var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                            callbackResults.SetResult(assetBundlesLibrary.GetLoadedConfigMessageDataPacket(AppData.ConfigMessageType.SignUpCompletedMessage));

                            if (callbackResults.Success())
                            {
                                var successMessageDataObject = assetBundlesLibrary.GetLoadedConfigMessageDataPacket(AppData.ConfigMessageType.SignUpCompletedMessage).GetData();

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "On Tab View Shown Event Failed - Profile Manager Instance Is Not Yet Initialized - Invalid Operation."));

                                if (callbackResults.Success())
                                {
                                    var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                                    profileManagerInstance.GetUserProfile(async userProfileCallbackResults =>
                                    {
                                        callbackResults.SetResult(userProfileCallbackResults);

                                        if (callbackResults.Success())
                                        {
                                            var userProfile = userProfileCallbackResults.GetData();

                                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On Tab View Shown Event Failed - Screen UI Manager Instance Is Not Yet Initialized - Invalid Operation."));

                                            if (callbackResults.Success())
                                            {
                                                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                                                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                                                if (callbackResults.Success())
                                                {
                                                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                                                    await Task.Delay(1000);

                                                    screen.HideScreenWidget(AppData.WidgetType.LoadingWidget, false, widgtHiddenCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(widgtHiddenCallbackResults);

                                                        if (callbackResults.Success())
                                                        {
                                                            #region Feature Blocker Widget Config

                                                            var successNotificationConfigDatapacket = new AppData.SceneConfigDataPacket();

                                                            successNotificationConfigDatapacket.SetReferencedWidgetType(AppData.WidgetType.SuccessNotificationPopUpWidget);
                                                            successNotificationConfigDatapacket.SetScreenBlurState(true);
                                                            successNotificationConfigDatapacket.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.ForeGround);

                                                            #endregion

                                                            screen.ShowWidget(successNotificationConfigDatapacket, showConfirmationWidgetCallbackResults =>
                                                            {
                                                                callbackResults.SetResult(showConfirmationWidgetCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.SuccessNotificationPopUpWidget));

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        var successNotificationWidget = screen.GetWidget(AppData.WidgetType.SuccessNotificationPopUpWidget).GetData();

                                                                        callbackResults.SetResult(successMessageDataObject.GetTitle());

                                                                        if(callbackResults.Success())
                                                                        {
                                                                            successNotificationWidget.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, successMessageDataObject.GetTitle().GetData(), widgetTitleSetCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(widgetTitleSetCallbackResults);

                                                                                if (callbackResults.Success())
                                                                                {
                                                                                    callbackResults.SetResult(successMessageDataObject.GetMessage());

                                                                                    if (callbackResults.Success())
                                                                                    {
                                                                                        var successMessage = successMessageDataObject.GetMessage($"{userProfile.GetUserName().GetData()}").GetData();

                                                                                        successNotificationWidget.SetUITextDisplayerValue(AppData.ScreenTextType.MessageDisplayer, successMessage, widgetTitleSetCallbackResults =>
                                                                                        {
                                                                                            callbackResults.SetResult(widgetTitleSetCallbackResults);

                                                                                            if (callbackResults.Success())
                                                                                            {
                                                                                                successNotificationWidget.SetActionButtonTitle(AppData.InputActionButtonType.ConfirmationButton, "Continue", buttonTitleUpdatedCallbackResults =>
                                                                                                {
                                                                                                    callbackResults.SetResult(buttonTitleUpdatedCallbackResults);

                                                                                                    if (callbackResults.Success())
                                                                                                    {
                                                                                                        onCompletedSignUpConfirmButtonEvent.SetAction(AppData.InputActionButtonType.ConfirmationButton, onCompletedSignUpConfirmButtonEventActionCallbackResults => 
                                                                                                        {
                                                                                                            callbackResults.SetResult(onCompletedSignUpConfirmButtonEventActionCallbackResults);

                                                                                                            if (callbackResults.Success())
                                                                                                            {
                                                                                                                onCompletedSignUpConfirmButtonEvent.SetMethod(OnUserInitialSignInEvent, onCompletedSignUpConfirmButtonEventMethodCallbackResults => 
                                                                                                                {
                                                                                                                    callbackResults.SetResult(onCompletedSignUpConfirmButtonEventMethodCallbackResults);

                                                                                                                    if (callbackResults.Success())
                                                                                                                    {
                                                                                                                        successNotificationWidget.RegisterActionButtonListeners(registerActionEventsCallbackResults =>
                                                                                                                        {
                                                                                                                            callbackResults.SetResult(registerActionEventsCallbackResults);

                                                                                                                            if(callbackResults.Success())
                                                                                                                            {
                                                                                                                                // Addd User Name Field Value And Highlight Password Field. 
                                                                                                                            }
                                                                                                                            else
                                                                                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                                                                                        }, onCompletedSignUpConfirmButtonEvent);
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
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        #endregion

        #region Tab View Overrides

        protected override void OnTabViewShown(Action<AppData.Callback> callback = null)
        {
           var callbackResults = new AppData.Callback();

            callback?.Invoke(callbackResults);
        }

        protected override void OnTabViewHidden(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

           

            callback?.Invoke(callbackResults);
        }

        #endregion

        protected override void OnActionButtonEvent(AppData.TabViewType screenWidgetType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
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
                                    case AppData.InputActionButtonType.ReadButton:

                                        screen.HideScreenWidget(AppData.WidgetType.SignInWidget);

                                        var readTermsAndConditionsWidgetConfig = new AppData.SceneConfigDataPacket();

                                        readTermsAndConditionsWidgetConfig.SetReferencedWidgetType(AppData.WidgetType.TermsAndConditionsWidget);
                                        readTermsAndConditionsWidgetConfig.blurScreen = true;

                                        screen.ShowWidget(readTermsAndConditionsWidgetConfig);

                                        break;

                                    case AppData.InputActionButtonType.SignUpButton:

                                        callbackResults.SetResult(GetValidatableInputFields());

                                        if (callbackResults.Success())
                                        {
                                            userProfile.SetUserName(GetInputField(AppData.InputFieldActionType.UserNameField).GetData().GetValue().GetData().text);
                                            userProfile.SetUserEmail(GetInputField(AppData.InputFieldActionType.UserEmailField).GetData().GetValue().GetData().text);
                                            userProfile.SetUserPassword(GetInputField(AppData.InputFieldActionType.UserPasswordField).GetData().GetValue().GetData().text);
                                            userProfile.SetUserPasswordValidation(GetInputField(AppData.InputFieldActionType.UserPasswordValidationField).GetData().GetValue().GetData().text);

                                            #region Fields Validations

                                            callbackResults.SetResult(userProfile.Initialized());

                                            if (callbackResults.Success())
                                            {
                                                profileManagerInstance.UpdateUserProfile(userProfile, updateUserProfileCallbackResults =>
                                                {
                                                    callbackResults.SetResult(updateUserProfileCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        callbackResults.SetResult(AppData.Helpers.GetAppStringValueEqual(userProfile.GetUserPassword().GetData(), userProfile.GetUserPasswordValidation().GetData(), "Varifying Password"));

                                                        if (callbackResults.Success())
                                                        {
                                                            callbackResults.SetResult(userProfile.GetTermsAndConditionsAccepted());

                                                            if (callbackResults.Success())
                                                            {
                                                                OnClearValidations(validationsClearedCallbackResults => 
                                                                {
                                                                    callbackResults.SetResult(validationsClearedCallbackResults);

                                                                    if (callbackResults.Success())
                                                                        OnUserSignUpEvent();
                                                                    else
                                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                });
                                                            }
                                                            else
                                                            {
                                                                callbackResults.SetResult(userProfile.GetTermsAndConditionsRead());

                                                                if (callbackResults.Success())
                                                                {
                                                                    OnInputCheckboxValidation(GetType().GetData(), AppData.ValidationResultsType.Warning, AppData.CheckboxInputActionType.AcceptTermsAndConditionsOption, callback: termsAndConditionCheckboxHighlightCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(termsAndConditionCheckboxHighlightCallbackResults);
                                                                    });
                                                                }
                                                                else
                                                                {
                                                                    OnButtonValidation(GetType().GetData(), AppData.ValidationResultsType.Warning, AppData.InputActionButtonType.ReadButton, callback: termsAndConditionButtonHighlightCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(termsAndConditionButtonHighlightCallbackResults);
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            OnInputFieldValidation(GetType().GetData(), AppData.ValidationResultsType.Error, AppData.InputFieldActionType.UserPasswordValidationField, onInputValidationCallbackResults =>
                                                            {
                                                                callbackResults.SetResult(onInputValidationCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    // Set Field Validation Info.
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
                                            {
                                                var invalidFieldType = userProfile.Initialized().GetData();

                                                OnInputFieldValidation(GetType().GetData(), AppData.ValidationResultsType.Warning, invalidFieldType, onInputValidationCallbackResults =>
                                                {
                                                    callbackResults.SetResult(onInputValidationCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        // Set Field Validation Info.
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });
                                            }

                                            #endregion
                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                        break;
                                }
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

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

        private void OnUserSignUpEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "Profile Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                profileManagerInstance.GetUserProfile(userProfileCallbackResults => 
                {
                    var userProfile = userProfileCallbackResults.GetData();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

                    if (callbackResults.Success())
                    {
                        var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                        callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.GetName(), "App Database Manager Instance Is Not Yet Initialized."));

                            if (callbackResults.Success())
                            {
                                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.GetName()).GetData();

                                callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                                if (callbackResults.Success())
                                {
                                    var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();
                                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();
                                    var loadingConfigDatapacket = new AppData.SceneConfigDataPacket();

                                    loadingConfigDatapacket.SetReferencedWidgetType(AppData.WidgetType.LoadingWidget);
                                    loadingConfigDatapacket.SetScreenBlurState(true);
                                    loadingConfigDatapacket.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.ForeGround);

                                    screen.ShowWidget(loadingConfigDatapacket, async showingLoadingCallbackResults =>
                                    {
                                        callbackResults.SetResult(showingLoadingCallbackResults);

                                        if (callbackResults.Success())
                                        {
                                            var checkCredentialsCallbackResults = await profileManagerInstance.CredentialsAvailable(userProfile.GetUserName().GetData(), userProfile.GetUserEmail().GetData());

                                            callbackResults.SetResult(checkCredentialsCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                var signUpUserProfileAsyncTask = await profileManagerInstance.SignUpAsync(userProfile);

                                                callbackResults.SetResult(signUpUserProfileAsyncTask);

                                                if (callbackResults.Success())
                                                {
                                                    var emailVerificationCheckCallbackResultsTask = await profileManagerInstance.UserEmailVerified();

                                                    callbackResults.SetResult(emailVerificationCheckCallbackResultsTask);

                                                    if (callbackResults.UnSuccessful())
                                                    {
                                                        screen.HideScreenWidget(AppData.WidgetType.LoadingWidget, false, widgtHiddenCallbackResults =>
                                                        {
                                                            callbackResults.SetResult(widgtHiddenCallbackResults);

                                                            if (callbackResults.Success())
                                                            {
                                                                #region Confirmation Widget Config

                                                                var emailVerificationSentWidgetConfigDatapacket = new AppData.SceneConfigDataPacket();

                                                                emailVerificationSentWidgetConfigDatapacket.SetReferencedWidgetType(AppData.WidgetType.ScreenNotificationPopUpWidget);
                                                                emailVerificationSentWidgetConfigDatapacket.SetScreenBlurState(true);
                                                                emailVerificationSentWidgetConfigDatapacket.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.ForeGround);

                                                                #endregion

                                                                screen.ShowWidget(emailVerificationSentWidgetConfigDatapacket, showVarificationEmailSentWidgetCallbackResults =>
                                                                {
                                                                    callbackResults.SetResult(showVarificationEmailSentWidgetCallbackResults);

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.ScreenNotificationPopUpWidget));

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            callbackResults.SetResult(assetBundlesLibrary.GetLoadedConfigMessageDataPacket(AppData.ConfigMessageType.EmailVerificationSentMessage));

                                                                            if (callbackResults.Success())
                                                                            {
                                                                                var emailVerificationMessageDataObject = assetBundlesLibrary.GetLoadedConfigMessageDataPacket(AppData.ConfigMessageType.EmailVerificationSentMessage).GetData();

                                                                                var emailVerificationSentNotificationWidget = screen.GetWidget(AppData.WidgetType.ScreenNotificationPopUpWidget).GetData();

                                                                                callbackResults.SetResult(emailVerificationMessageDataObject.GetTitle());

                                                                                if(callbackResults.Success())
                                                                                {
                                                                                    emailVerificationSentNotificationWidget.SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, emailVerificationMessageDataObject.GetTitle().GetData(), verificationTitleSetCallbackResults =>
                                                                                    {
                                                                                        callbackResults.SetResult(verificationTitleSetCallbackResults);

                                                                                        if (callbackResults.Success())
                                                                                        {
                                                                                            callbackResults.SetResult(emailVerificationMessageDataObject.GetMessage());

                                                                                            if (callbackResults.Success())
                                                                                            {
                                                                                                string verificationMessage = emailVerificationMessageDataObject.GetMessage($"{userProfile.GetUserEmail().GetData()}").GetData();

                                                                                                emailVerificationSentNotificationWidget.SetUITextDisplayerValue(AppData.ScreenTextType.MessageDisplayer, verificationMessage, verificationMessageSetCallbackResults =>
                                                                                                {
                                                                                                    callbackResults.SetResult(verificationMessageSetCallbackResults);

                                                                                                    if (callbackResults.Success())
                                                                                                    {
                                                                                                        emailVerificationSentNotificationWidget.SetActionButtonTitle(AppData.InputActionButtonType.ConfirmationButton, "Resend Email", resendEmailButtonTitleUpdatedCallbackResults => 
                                                                                                        {
                                                                                                            callbackResults.SetResult(resendEmailButtonTitleUpdatedCallbackResults);

                                                                                                            if (callbackResults.Success())
                                                                                                            {
                                                                                                                emailVerificationSentNotificationWidget.SetActionButtonTitle(AppData.InputActionButtonType.Cancel, "Incorrect Email", resendEmailButtonTitleUpdatedCallbackResults =>
                                                                                                                {
                                                                                                                    callbackResults.SetResult(resendEmailButtonTitleUpdatedCallbackResults);

                                                                                                                    if (callbackResults.Success())
                                                                                                                    {
                                                                                                                        onResendEmailButtonEvent.SetAction(AppData.InputActionButtonType.ConfirmationButton, resendEmailRequestButtonActionCallbackResults => 
                                                                                                                        {
                                                                                                                            callbackResults.SetResult(resendEmailRequestButtonActionCallbackResults);

                                                                                                                            if(callbackResults.Success())
                                                                                                                            {
                                                                                                                                onResendEmailButtonEvent.SetMethod(OnUserEmailResendRequestButtonPressedEvent, resendEmailRequestButtonMethodCallbackResults => 
                                                                                                                                {
                                                                                                                                    callbackResults.SetResult(resendEmailRequestButtonMethodCallbackResults);

                                                                                                                                    if (callbackResults.Success())
                                                                                                                                    {
                                                                                                                                        onIncorrectEmailButtonEvent.SetAction(AppData.InputActionButtonType.Cancel, incorrectEmailuttonActionCallbackResults =>
                                                                                                                                        {
                                                                                                                                            callbackResults.SetResult(incorrectEmailuttonActionCallbackResults);

                                                                                                                                            if (callbackResults.Success())
                                                                                                                                            {
                                                                                                                                                onIncorrectEmailButtonEvent.SetMethod(OnIncorrectUserEmailButtonPressedEvent, incorrectEmailButtonMethodCallbackResults =>
                                                                                                                                                {
                                                                                                                                                    callbackResults.SetResult(incorrectEmailButtonMethodCallbackResults);

                                                                                                                                                    if (callbackResults.Success())
                                                                                                                                                    {
                                                                                                                                                        emailVerificationSentNotificationWidget.RegisterActionButtonListeners(emailVerificationButtonsEventCallbackResults => 
                                                                                                                                                        {
                                                                                                                                                            callbackResults.SetResult(emailVerificationButtonsEventCallbackResults);

                                                                                                                                                            if(callbackResults.Success())
                                                                                                                                                            {
                                                                                                                                                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name, "App Time Events Manager Instance Is Not Yet Initialized."));

                                                                                                                                                                if (callbackResults.Success())
                                                                                                                                                                {
                                                                                                                                                                    var timeManager = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name).GetData();

                                                                                                                                                                    timeManager.RegisterTimedEvent("On User Email Verification Check Event", OnUserEmailVerificationCheckEvent, 5.0f);

                                                                                                                                                                    timeManager.InvokeEvent("On User Email Verification Check Event", invokeUserEmailVerificationCheckEventCallbackResults =>
                                                                                                                                                                    {
                                                                                                                                                                        callbackResults.SetResult(invokeUserEmailVerificationCheckEventCallbackResults);

                                                                                                                                                                    });
                                                                                                                                                                }
                                                                                                                                                                else
                                                                                                                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                                                                                                            }
                                                                                                                                                            else
                                                                                                                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                                                                                                                        }, onResendEmailButtonEvent, onIncorrectEmailButtonEvent);
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
                                                                });
                                                            }
                                                            else
                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                        });
                                                    }
                                                    else
                                                    {
                                                        LogSuccess($" __Log_Cat/: Sign Up : {userProfile.GetUserName().GetData()} With Profile ID : {userProfile.GetUniqueIdentifier().GetData()} - Sending Email varification To : {userProfile.GetUserEmail().GetData()}", this);
                                                    }
                                                }
                                                else
                                                {
                                                    switch (signUpUserProfileAsyncTask.GetData())
                                                    {
                                                        case Firebase.Auth.AuthError.AccountExistsWithDifferentCredentials:

                                                            break;

                                                        case Firebase.Auth.AuthError.AdminRestrictedOperation:

                                                            break;

                                                        case Firebase.Auth.AuthError.EmailAlreadyInUse:

                                                            screen.HideScreenWidget(AppData.WidgetType.LoadingWidget, false, widgtHiddenCallbackResults =>
                                                            {
                                                                callbackResults.SetResult(widgtHiddenCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    #region Feature Blocker Widget Config

                                                                    var confirmationConfigDatapacket = new AppData.SceneConfigDataPacket();

                                                                    confirmationConfigDatapacket.SetReferencedWidgetType(AppData.WidgetType.FeatureBlockerPopUpWidget);
                                                                    confirmationConfigDatapacket.SetScreenBlurState(true);
                                                                    confirmationConfigDatapacket.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.ForeGround);

                                                                    #endregion

                                                                    screen.ShowWidget(confirmationConfigDatapacket, async showConfirmationWidgetCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(showConfirmationWidgetCallbackResults);

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            callbackResults.SetResult(screen.GetWidgetOfType(AppData.WidgetType.FeatureBlockerPopUpWidget));

                                                                            if (callbackResults.Success())
                                                                            {
                                                                                var emailVerificationCheckCallbackResultsTask = await profileManagerInstance.UserEmailVerified();

                                                                                callbackResults.SetResult(emailVerificationCheckCallbackResultsTask); ;

                                                                                if (callbackResults.Success())
                                                                                {

                                                                                }
                                                                                else
                                                                                {

                                                                                }
                                                                            }

                                                                            //OnInputFieldValidation(GetType().GetData(), AppData.ValidationResultsType.Error, AppData.InputFieldActionType.UserEmailField, onInputValidationCallbackResults =>
                                                                            //{
                                                                            //    callbackResults.SetResult(onInputValidationCallbackResults);

                                                                            //    if (callbackResults.Success())
                                                                            //    {
                                                                            //        //OnClearInputFieldValue(AppData.InputFieldActionType.UserEmailField, fieldClearedCallbackResults => 
                                                                            //        //{
                                                                            //        //    callbackResults.SetResult(fieldClearedCallbackResults);

                                                                            //        //    if (callbackResults.Success())
                                                                            //        //    {

                                                                            //        //    }
                                                                            //        //    else
                                                                            //        //        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                            //        //});
                                                                            //    }
                                                                            //    else
                                                                            //        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                            //});
                                                                        }
                                                                        else
                                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                    });
                                                                }
                                                                else
                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                            });

                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                callbackResults.SetResult(AppData.Helpers.GetAppEnumValueValid(checkCredentialsCallbackResults.GetData()));

                                                if (callbackResults.Success())
                                                {
                                                    switch (checkCredentialsCallbackResults.GetData())
                                                    {
                                                        case AppData.CredentialStatusInfo.DeviceNetworkError:

                                                            callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.NetworkNotificationWidget));

                                                            if (callbackResults.Success())
                                                            {
                                                                var networkNotificationWidget = screen.GetWidget(AppData.WidgetType.NetworkNotificationWidget).GetData();

                                                                networkNotificationWidget.SetActionButtonTitle(AppData.InputActionButtonType.Cancel, "Cancel", buttonTitleSetCallbackResults =>
                                                                {
                                                                    callbackResults.SetResult(buttonTitleSetCallbackResults);

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        networkNotificationWidget.UnRegisterActionButtonListeners(actionEventsUnregisteredCallbackResults =>
                                                                        {
                                                                            callbackResults.SetResult(actionEventsUnregisteredCallbackResults);

                                                                            if (callbackResults.Success())
                                                                            {
                                                                                onNetworkRetryButtonEvent.SetMethod(OnUserSignUpEvent, retryMethodSetCallbackResults =>
                                                                                {
                                                                                    callbackResults.SetResult(retryMethodSetCallbackResults);

                                                                                    if (callbackResults.Success())
                                                                                    {
                                                                                        onNetworkRetryButtonEvent.SetAction(AppData.InputActionButtonType.RetryButton, retryActionSetCallbackResults =>
                                                                                        {
                                                                                            callbackResults.SetResult(retryActionSetCallbackResults);

                                                                                            if (callbackResults.Success())
                                                                                            {
                                                                                                onNetworkCancelButtonEvent.SetMethod(OnNetworkFailedCancelEvent, cancelMethodSetCallbackResults =>
                                                                                                {
                                                                                                    callbackResults.SetResult(cancelMethodSetCallbackResults);

                                                                                                    if (callbackResults.Success())
                                                                                                    {
                                                                                                        onNetworkCancelButtonEvent.SetAction(AppData.InputActionButtonType.Cancel, cancelActionSetCallbackResults =>
                                                                                                        {
                                                                                                            callbackResults.SetResult(cancelActionSetCallbackResults);

                                                                                                            if (callbackResults.Success())
                                                                                                            {
                                                                                                                networkNotificationWidget.RegisterActionButtonListeners(registerActionEventsCallbackResults =>
                                                                                                                {
                                                                                                                    callbackResults.SetResult(registerActionEventsCallbackResults);

                                                                                                                }, onNetworkRetryButtonEvent, onNetworkCancelButtonEvent);
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

                                                            break;

                                                        case AppData.CredentialStatusInfo.UserNameError:

                                                            break;

                                                        case AppData.CredentialStatusInfo.UserEmailError:

                                                            break;
                                                    }
                                                }
                                                else
                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnUserInitialSignInEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On User Sign In Failed - Screen UI Manager Instance Is Not Yet Initialized - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "On User Sign In Failed - Profile Manager Instance Is Not Yet Initialized - Invalid Operation."));

                    if (callbackResults.Success())
                    {
                        var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();
                        var screen = screenUIManagerInstance.GetCurrentScreen().GetData();
                        var loadingConfigDatapacket = new AppData.SceneConfigDataPacket();

                        loadingConfigDatapacket.SetReferencedWidgetType(AppData.WidgetType.LoadingWidget);
                        loadingConfigDatapacket.SetScreenBlurState(true);
                        loadingConfigDatapacket.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.ForeGround);

                        callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.SuccessNotificationPopUpWidget));

                        if (callbackResults.Success())
                        {
                            var successNotificationPopUpWidget = screen.GetWidget(AppData.WidgetType.SuccessNotificationPopUpWidget).GetData();

                            successNotificationPopUpWidget.UnRegisterActionButtonListeners(actionsUnregisteredcallbackResults =>
                            {
                                callbackResults.SetResult(actionsUnregisteredcallbackResults);

                                if (callbackResults.Success())
                                {
                                    screen.HideScreenWidget(successNotificationPopUpWidget, widgetHiddenCallbackResults =>
                                    {
                                        callbackResults.SetResult(widgetHiddenCallbackResults);

                                        if (callbackResults.Success())
                                        {
                                            screen.ShowWidget(loadingConfigDatapacket, async showingLoadingCallbackResults =>
                                            {
                                                callbackResults.SetResult(showingLoadingCallbackResults);

                                                if (callbackResults.Success())
                                                {
                                                    var registerUserProfileCallbackResultsTask = await profileManagerInstance.RegisterUserProfileAsync();

                                                    callbackResults.SetResult(registerUserProfileCallbackResultsTask);

                                                    if (callbackResults.Success())
                                                    {
                                                        var userProfile = registerUserProfileCallbackResultsTask.GetData();

                                                        screen.HideScreenWidget(AppData.WidgetType.LoadingWidget, false, loadingWidgetHiddenCallbackResults =>
                                                        {
                                                            callbackResults.SetResult(loadingWidgetHiddenCallbackResults);

                                                            if (callbackResults.Success())
                                                            {
                                                                screen.Blur(AppData.ScreenUIPlacementType.Default, true, screenBluredCallbackResults =>
                                                                {
                                                                    callbackResults.SetResult(screenBluredCallbackResults);

                                                                    if(callbackResults.Success())
                                                                    {
                                                                        callbackResults.SetResult(GetParentWidget());

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            var signInWidget = GetParentWidget().GetData();

                                                                            callbackResults.SetResult(signInWidget.GetTabViewComponent());

                                                                            if (callbackResults.Success())
                                                                            {
                                                                                callbackResults.SetResult(signInWidget.GetTabViewComponent().GetData().GetTabView(AppData.TabViewType.SignInView));

                                                                                if (callbackResults.Success())
                                                                                {
                                                                                    var signInTabView = signInWidget.GetTabViewComponent().GetData().GetTabView(AppData.TabViewType.SignInView).GetData();

                                                                                    signInTabView.SetActionInputFieldValueText(AppData.InputFieldActionType.UserEmailField, userProfile.GetUserEmail().GetData(), userPasswordValueSetCallbackResults => 
                                                                                    {
                                                                                        callbackResults.SetResult(userPasswordValueSetCallbackResults);

                                                                                        if(callbackResults.Success())
                                                                                        {
                                                                                            signInTabView.HighlightInputField(AppData.InputFieldActionType.UserPasswordField, callback: fieldHighlightedCallbackResults =>
                                                                                            {
                                                                                                callbackResults.SetResult(fieldHighlightedCallbackResults);
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
                                                                });
                                                            }
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
        }

        private void OnNetworkFailedCancelEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    screen.Blur(AppData.ScreenUIPlacementType.Default, true, screenBluredCallbackResults => 
                    {
                        callbackResults.SetResult(screenBluredCallbackResults);
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private async void OnUserEmailVerificationCheckEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "Profile Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                callbackResults.SetResult(profileManagerInstance.GetCurrentUser());

                if (callbackResults.Success())
                {
                    var emailVerificationCheckCallbackResultsTask = await profileManagerInstance.UserEmailVerified();

                    callbackResults.SetResult(emailVerificationCheckCallbackResultsTask);

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

                                screen.HideScreenWidget(AppData.WidgetType.ScreenNotificationPopUpWidget, false, screenNotificationHiddenCallbackResults => 
                                {
                                    callbackResults.SetResult(screenNotificationHiddenCallbackResults);

                                    if(callbackResults.Success())
                                    {
                                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name, "App Time Events Manager Instance Is Not Yet Initialized."));

                                        if (callbackResults.Success())
                                        {
                                            var timeManager = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name).GetData();

                                            timeManager.RegisterTimedEvent("On User Email Verification Check Event", OnUserEmailVerificationCheckEvent, 5.0f);

                                            timeManager.CancelEvent("On User Email Verification Check Event", cancelUserEmailVerificationCheckEventCallbackResults =>
                                            {
                                                callbackResults.SetResult(cancelUserEmailVerificationCheckEventCallbackResults);

                                                if(callbackResults.Success())
                                                {
                                                    var loadingConfigDatapacket = new AppData.SceneConfigDataPacket();

                                                    loadingConfigDatapacket.SetReferencedWidgetType(AppData.WidgetType.LoadingWidget);
                                                    loadingConfigDatapacket.SetScreenBlurState(true);
                                                    loadingConfigDatapacket.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.ForeGround);

                                                    screen.ShowWidget(loadingConfigDatapacket, async loadingWidgetShownCallbackResults => 
                                                    {
                                                        callbackResults.SetResult(loadingWidgetShownCallbackResults);

                                                        if (callbackResults.Success())
                                                        {
                                                            await Task.Delay(2000);

                                                            callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.SignInWidget));

                                                            if(callbackResults.Success())
                                                            {
                                                                var signInWidget = screen.GetWidget(AppData.WidgetType.SignInWidget).GetData() as SignInWidget;

                                                                signInWidget.SwitchPageAsync(AppData.TabViewType.SignInView, switchingToSignUpPageCallbackresults => 
                                                                {
                                                                    callbackResults.SetResult(switchingToSignUpPageCallbackresults);

                                                                    if(callbackResults.Success())
                                                                    {
                                                                        LogSuccess("Page Switched", this);
                                                                    }
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
        }

        #region User Email Verification Button events

        private void OnUserEmailResendRequestButtonPressedEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On User Email Resend Request Button Pressed Event Failed - Screen UI Manager Instance Is Not Yet Initialized - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "On User Email Resend Request Button Pressed Event Failed - Profile Manager Instance Is Not Yet Initialized - Invalid Operation."));

                    if (callbackResults.Success())
                    {
                        var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name, "App Time Events Manager Instance Is Not Yet Initialized."));

                        if (callbackResults.Success())
                        {
                            var timeManager = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name).GetData();

                            timeManager.RegisterTimedEvent("On User Email Verification Check Event", OnUserEmailVerificationCheckEvent, 5.0f);

                            timeManager.CancelEvent("On User Email Verification Check Event", cancelUserEmailVerificationCheckEventCallbackResults =>
                            {
                                var loadingConfigDatapacket = new AppData.SceneConfigDataPacket();

                                loadingConfigDatapacket.SetReferencedWidgetType(AppData.WidgetType.LoadingWidget);
                                loadingConfigDatapacket.SetScreenBlurState(true);
                                loadingConfigDatapacket.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.ForeGround);

                                screen.HideScreenWidget(AppData.WidgetType.ScreenNotificationPopUpWidget, callback: screenNotificationPopUpWidgetHiddenCallbackResults =>
                                {
                                    callbackResults.SetResult(screenNotificationPopUpWidgetHiddenCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        screen.ShowWidget(loadingConfigDatapacket, async loadingWidgetShownCallbackResults =>
                                        {
                                            callbackResults.SetResult(loadingWidgetShownCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                var emailVerificationCheckCallbackResultsTask = await profileManagerInstance.UserEmailVerified();

                                                callbackResults.SetResult(emailVerificationCheckCallbackResultsTask);

                                                if (callbackResults.UnSuccessful())
                                                {
                                                    var deleteAccountCallbackResultsTask = await profileManagerInstance.OnAccountDeleteRequestAsync();

                                                    callbackResults.SetResult(deleteAccountCallbackResultsTask);

                                                    if (callbackResults.Success())
                                                        OnUserSignUpEvent();
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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

        private void OnIncorrectUserEmailButtonPressedEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On Incorrect User Email Button Pressed Event Failed - Screen UI Manager Instance Is Not Yet Initialized - Invalid Operation."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "On Incorrect User Email Button Pressed Event Failed - Profile Manager Instance Is Not Yet Initialized - Invalid Operation."));

                    if (callbackResults.Success())
                    {
                        var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name, "App Time Events Manager Instance Is Not Yet Initialized."));

                        if (callbackResults.Success())
                        {
                            var timeManager = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name).GetData();

                            timeManager.RegisterTimedEvent("On User Email Verification Check Event", OnUserEmailVerificationCheckEvent, 5.0f);

                            timeManager.CancelEvent("On User Email Verification Check Event", cancelUserEmailVerificationCheckEventCallbackResults =>
                            {
                                callbackResults.SetResult(cancelUserEmailVerificationCheckEventCallbackResults);

                                var loadingConfigDatapacket = new AppData.SceneConfigDataPacket();

                                loadingConfigDatapacket.SetReferencedWidgetType(AppData.WidgetType.LoadingWidget);
                                loadingConfigDatapacket.SetScreenBlurState(true);
                                loadingConfigDatapacket.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.ForeGround);

                                screen.HideScreenWidget(AppData.WidgetType.ScreenNotificationPopUpWidget, callback: screenNotificationPopUpWidgetHiddenCallbackResults =>
                                {
                                    callbackResults.SetResult(screenNotificationPopUpWidgetHiddenCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        screen.ShowWidget(loadingConfigDatapacket, async loadingWidgetShownCallbackResults =>
                                        {
                                            callbackResults.SetResult(loadingWidgetShownCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                var deleteAccountCallbackResultsTask = await profileManagerInstance.OnAccountDeleteRequestAsync();

                                                callbackResults.SetResult(deleteAccountCallbackResultsTask);

                                                if (callbackResults.Success())
                                                {
                                                    screen.HideScreenWidget(AppData.WidgetType.LoadingWidget, callback: screenLoadingWidgetHiddenCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(screenLoadingWidgetHiddenCallbackResults);

                                                        if (callbackResults.Success())
                                                        {
                                                            screen.Blur(AppData.ScreenUIPlacementType.Default, true, screenBluredCallbackResults =>
                                                            {
                                                                callbackResults.SetResult(screenBluredCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    OnInputFieldValidation(GetType().GetData(), AppData.ValidationResultsType.Warning, AppData.InputFieldActionType.UserEmailField, fieldValidatedCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(fieldValidatedCallbackResults);
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
                                });
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

        #endregion

        public void ReadAndAcceptTermsAndConditions(Action<AppData.Callback> callback = null)
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

                        userProfile.ReadTermsAndConditions(termsAndConditionsReadCallbackResults => 
                        {
                            callbackResults.SetResult(termsAndConditionsReadCallbackResults);

                                if(callbackResults.Success())
                                {
                                    bool acceptTermsAndConditions = !userProfile.GetTermsAndConditionsAccepted().Success();

                                    userProfile.AcceptTermsAndConditions(acceptTermsAndConditions, acceptTermsAndConditionsCallbackResults => 
                                    {
                                        callbackResults.SetResult(acceptTermsAndConditionsCallbackResults);

                                        if(callbackResults.Success())
                                        {
                                            callbackResults.SetResult(GetActionCheckbox(AppData.CheckboxInputActionType.AcceptTermsAndConditionsOption));

                                            if (callbackResults.Success())
                                            {
                                                var checkbox = GetActionCheckbox(AppData.CheckboxInputActionType.AcceptTermsAndConditionsOption).GetData();

                                                checkbox.SetToggleState(acceptTermsAndConditions, toggleStateSetCallbackResults => 
                                                {
                                                    callbackResults.SetResult(toggleStateSetCallbackResults);

                                                    if(callbackResults.Success())
                                                    {
                                                        profileManagerInstance.UpdateUserProfile(userProfile, profileUpdatedCallbackResults => 
                                                        {
                                                            callbackResults.SetResult(profileUpdatedCallbackResults);
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
                            });
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
           
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
           
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
          
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool acceptTermsAndConditions, AppData.CheckboxConfigDataPacket dataPackets)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "Profile Manager Instance Is Not Yet Initialized."));

            if(callbackResults.Success())
            {
                var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                profileManagerInstance.GetUserProfile(userProfileCallbackResults =>
                {
                    callbackResults.SetResult(userProfileCallbackResults);

                    if(callbackResults.Success())
                    {
                        if(actionType == AppData.CheckboxInputActionType.AcceptTermsAndConditionsOption)
                        {
                            var userProfile = userProfileCallbackResults.GetData();

                            userProfile.AcceptTermsAndConditions(acceptTermsAndConditions, termsAndConditionsAcceptedCallbackResults => 
                            {
                                callbackResults.SetResult(termsAndConditionsAcceptedCallbackResults);

                                if(callbackResults.Success())
                                {
                                    LogInfo($" __Logs_Cat/: Terms And Conditions Accepted : {userProfile.GetTermsAndConditionsAccepted().Success()}", this);

                                    profileManagerInstance.UpdateUserProfile(userProfile, userProfileUpdatedcallbackResults => 
                                    {
                                        callbackResults.SetResult(userProfileUpdatedcallbackResults);
                                    });
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            });
                        }
                        else
                        {
                            callbackResults.result = $"On Check box Value Changed Failed - Checkbox Is not Set To Accept Terms And Conditions Option - The Checkbox Is Action Type For : {GetName()} Is Set To : {actionType} - Invalid operation.";
                            callbackResults.resultCode = AppData.Helpers.WarningCode;
                        }
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this); 
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.TabViewType, AppData.TabViewType, AppData.Widget>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.TabViewType, AppData.TabViewType, AppData.Widget>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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