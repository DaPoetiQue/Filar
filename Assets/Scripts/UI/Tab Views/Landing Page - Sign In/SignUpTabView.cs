using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

                    if (callbackResults.Success())
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

                                        callbackResults.SetResult(GetInputField(AppData.InputFieldActionType.UserNameField));

                                        if (callbackResults.Success())
                                        {
                                            callbackResults.SetResult(GetInputField(AppData.InputFieldActionType.UserEmailField));

                                            if (callbackResults.Success())
                                            {
                                                callbackResults.SetResult(GetInputField(AppData.InputFieldActionType.UserPasswordField));

                                                if (callbackResults.Success())
                                                {
                                                    callbackResults.SetResult(GetInputField(AppData.InputFieldActionType.UserPasswordVarificationField));

                                                    if (callbackResults.Success())
                                                    {
                                                        var userName = GetInputField(AppData.InputFieldActionType.UserNameField).GetData().GetValue().GetData().text;
                                                        var userEmail = GetInputField(AppData.InputFieldActionType.UserEmailField).GetData().GetValue().GetData().text;
                                                        var userPassword = GetInputField(AppData.InputFieldActionType.UserPasswordField).GetData().GetValue().GetData().text;
                                                        var userPasswordVarification = GetInputField(AppData.InputFieldActionType.UserPasswordVarificationField).GetData().GetValue().GetData().text;

                                                        userProfile.SetUserName(userName);
                                                        userProfile.SetUserEmail(userEmail);
                                                        userProfile.SetUserPassword(userPassword);
                                                        userProfile.SetUserPasswordVerification(userPasswordVarification);

                                                        callbackResults.SetResult(userProfile.Initialized());

                                                        if (callbackResults.Success())
                                                        {
                                                            callbackResults.SetResult(AppData.Helpers.GetAppStringValueEqual(userProfile.GetUserPassword().GetData(), userProfile.GetUserPasswordVerification().GetData(), "Varifying Password"));

                                                            if (callbackResults.Success())
                                                            {
                                                                callbackResults.SetResult(userProfile.GetTermsAndConditionsAccepted());

                                                                if(callbackResults.Success())
                                                                {

                                                                    LogSuccess($"***_Log_cat: Sign Up Success - Name : {userName} - Email : {userEmail} - Password : {userPassword} - Password Varification : {userPasswordVarification} ", this);
                                                                }
                                                                else
                                                                {
                                                                    OnButtonValidation(GetType().GetData(), AppData.ValidationResultsType.Warning, AppData.InputActionButtonType.ReadButton, callback: termsAndConditionButtonHighlightCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(termsAndConditionButtonHighlightCallbackResults);
                                                                    });
                                                                }
                                                            }
                                                            else
                                                            {
                                                                OnInputFieldValidation(GetType().GetData(), AppData.ValidationResultsType.Error, AppData.InputFieldActionType.UserPasswordVarificationField, onInputValidationCallbackResults =>
                                                                {
                                                                    callbackResults.SetResult(onInputValidationCallbackResults);

                                                                    if (callbackResults.Success())
                                                                    {

                                                                    }
                                                                    else
                                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                });
                                                            }
                                                        }
                                                        else
                                                        {
                                                            callbackResults.SetResult(GetValidatableInputFields());

                                                            if (callbackResults.Success())
                                                            {
                                                                var invalidFieldType = userProfile.Initialized().GetData();

                                                                for (int i = 0; i < GetValidatableInputFields().GetData().Count; i++)
                                                                {
                                                                    var inputField = GetValidatableInputFields().GetData()[i];

                                                                    callbackResults.SetResult(inputField.GetDataPackets());

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        callbackResults.SetResult(inputField.GetDataPackets().GetData().GetAction());

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            if (inputField.GetDataPackets().GetData().GetAction().GetData() == invalidFieldType)
                                                                            {
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
                                                                            else
                                                                            {
                                                                                callbackResults.SetResult(inputField.GetValidationResults());

                                                                                if (callbackResults.Success())
                                                                                {
                                                                                    if (inputField.GetValidationResults().GetData() != AppData.ValidationResultsType.Default)
                                                                                    {
                                                                                        callbackResults.SetResult(OnProfileFieldIsValidated(userProfile, inputField));

                                                                                        if(callbackResults.Success())
                                                                                        {
                                                                                            OnInputFieldValidation(GetType().GetData(), AppData.ValidationResultsType.Success, inputField.GetDataPackets().GetData().GetAction().GetData(), onInputValidationCallbackResults =>
                                                                                            {
                                                                                                callbackResults.SetResult(onInputValidationCallbackResults);
                                                                                            });
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            callbackResults.result = $"Field : {inputField.GetName()} - Is Still Invalid - Continuing Exectution.";
                                                                                            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                        continue;
                                                                                }
                                                                                else
                                                                                {
                                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                            break;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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

        private AppData.Callback OnProfileFieldIsValidated(AppData.Profile userProfile, AppData.UIInputField<AppData.InputFieldConfigDataPacket> inputField)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(userProfile, "User Profile", $"On Profile Field Is Validated Failed - User Profile Parameter Value For : {GetName()} Is Missing / Null - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(inputField, "Input Field", $"On Profile Field Is Validated Failed - Input Field Parameter Value For : {GetName()} Is Missing / Null - Invalid Operation."));

                if(callbackResults.Success())
                {
                    callbackResults.SetResult(inputField.Initialized());

                    if (callbackResults.Success())
                    {
                        switch (inputField.GetDataPackets().GetData().GetAction().GetData())
                        {
                            case AppData.InputFieldActionType.UserNameField:

                                userProfile.SetUserName(inputField.GetValue().GetData().text);
                                callbackResults.SetResult(userProfile.GetUserName());

                                break;

                            case AppData.InputFieldActionType.UserEmailField:

                                userProfile.SetUserEmail(inputField.GetValue().GetData().text);
                                callbackResults.SetResult(userProfile.GetUserEmail());

                                LogInfo($" --Logs_Cats/: Email Check : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult}", this);

                                break;

                            case AppData.InputFieldActionType.UserPasswordField:

                                userProfile.SetUserPassword(inputField.GetValue().GetData().text);
                                callbackResults.SetResult(userProfile.GetUserPassword());

                                break;

                            case AppData.InputFieldActionType.UserPasswordVarificationField:

                                userProfile.SetUserPasswordVerification(inputField.GetValue().GetData().text);
                                callbackResults.SetResult(userProfile.GetUserPasswordVerification());

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

            return callbackResults;
        }


        public void AcceptTermsAndConditions(Action<AppData.Callback> callback = null)
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

                        callbackResults.SetResult(GetActionCheckbox(AppData.CheckboxInputActionType.AcceptTermsAndConditionsOption));

                        if (callbackResults.Success())
                        {
                            var checkbox = GetActionCheckbox(AppData.CheckboxInputActionType.AcceptTermsAndConditionsOption).GetData();

                            checkbox.SetToggleState(true, toggleStateSetCallbackResults => 
                            {
                                callbackResults.SetResult(toggleStateSetCallbackResults);

                                if(callbackResults.Success())
                                {
                                    callbackResults.SetResult(userProfile.AcceptTermsAndConditions());
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

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            
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