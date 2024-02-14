using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SignUpTabView : AppData.TabView<AppData.WidgetType>
    {
        #region Components

        private AppData.Profile userProfileReference = null;

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

                                            if(callbackResults.Success())
                                            {
                                                userProfile.SetUserName(GetInputField(AppData.InputFieldActionType.UserNameField).GetData().GetValue().GetData().text);
                                                userProfile.SetUserEmail(GetInputField(AppData.InputFieldActionType.UserEmailField).GetData().GetValue().GetData().text);
                                                userProfile.SetUserPassword(GetInputField(AppData.InputFieldActionType.UserPasswordField).GetData().GetValue().GetData().text);
                                                userProfile.SetUserPasswordValidation(GetInputField(AppData.InputFieldActionType.UserPasswordValidationField).GetData().GetValue().GetData().text); 

                                                #region Fields Validations

                                                callbackResults.SetResult(userProfile.Initialized());

                                                if(callbackResults.Success())
                                                {
                                                    profileManagerInstance.UpdateUserProfile(userProfile, updateUserProfileCallbackResults => 
                                                    {
                                                        callbackResults.SetResult(updateUserProfileCallbackResults);

                                                        if(callbackResults.Success())
                                                        {
                                                            callbackResults.SetResult(AppData.Helpers.GetAppStringValueEqual(userProfile.GetUserPassword().GetData(), userProfile.GetUserPasswordValidation().GetData(), "Varifying Password"));

                                                            if(callbackResults.Success())
                                                            {
                                                                callbackResults.SetResult(userProfile.GetTermsAndConditionsAccepted());

                                                                if(callbackResults.Success())
                                                                {
                                                                    LogSuccess($" __Log_Cat/: Sign Up : {userProfile.GetUserName().GetData()} With Profile ID : {userProfile.GetUniqueIdentifier().GetData()} - Sending Email varification To : {userProfile.GetUserEmail().GetData()}", this);
                                                                }
                                                                else
                                                                {
                                                                    callbackResults.SetResult(userProfile.GetTermsAndConditionsRead());

                                                                    if(callbackResults.Success())
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

                                                                        if(callbackResults.Success())
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