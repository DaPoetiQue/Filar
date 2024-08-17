using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SignInTabView : AppData.TabView<AppData.WidgetType>
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

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "App Events Manager Instance Is Not Yet Initialized."));

                    if (callbackResults.Success())
                    {
                        var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                        appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnWidgetShown, AppData.EventType.OnWidgetShown, true);
                        appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnWidgetHidden, AppData.EventType.OnWidgetHidden, true);
                        appEventsManagerInstance.OnEventSubscription<AppData.TabView<AppData.WidgetType>>(OnTabViewShownEvent, AppData.EventType.OnTabViewShown, true);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            });

            callback.Invoke(callbackResults);
        }

        protected override void Configure(Action<AppData.Callback> callback = null)
        {
           
        }

        #region On Widget Events

        private void OnWidgetShown(AppData.Widget widget)
        {
            var callbackResults = new AppData.Callback(widget.GetType());

            if (callbackResults.Success())
            {
                if (widget.GetType().GetData() == AppData.WidgetType.SignInWidget)
                {
                    callbackResults.SetResult(GetScreenTitleLocalizationKey());

                    if (callbackResults.Success())
                    {
                        SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, GetScreenTitleLocalizationKey().GetData(), titleSetCallbackResults =>
                        {
                            callbackResults.SetResult(titleSetCallbackResults);

                            if (callbackResults.Success())
                            {
                                HighlightInputField(AppData.InputFieldActionType.UserNameField, callback: fieldHighlightedCallbackResults =>
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

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(GetValidatableInputFields());

                            if (callbackResults.Success())
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
                    #region Set UI Text

                    SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, GetScreenTitleLocalizationKey().GetData(), titleSetCallbackResults =>
                    {
                        callbackResults.SetResult(titleSetCallbackResults);
                    });

                    SetUITextDisplayerValue(AppData.ScreenTextType.Toaster, AppData.LocalizationKey.info_SignInToaster, signInToasterSetCallbackResults =>
                    {
                        callbackResults.SetResult(signInToasterSetCallbackResults);
                    });

                    SetUITextDisplayerValue(AppData.ScreenTextType.InfoDisplayer, AppData.LocalizationKey.info_AppSigningOptions, signInInfoSetCallbackResults =>
                    {
                        callbackResults.SetResult(signInInfoSetCallbackResults);
                    });

                    #endregion

                    #region Set Button Title Text

                    SetActionButtonTitle(AppData.InputActionButtonType.SignInButton, AppData.LocalizationKey.title_SignIn, buttonTitleSetCallbackResults =>
                    {
                        callbackResults.SetResult(buttonTitleSetCallbackResults);
                    });

                    SetActionButtonTitle(AppData.InputActionButtonType.PasswordResetButton, AppData.LocalizationKey.title_ForgotPassword, buttonTitleSetCallbackResults =>
                    {
                        callbackResults.SetResult(buttonTitleSetCallbackResults);
                    });

                    #endregion

                    #region Highlight Fields

                    HighlightInputField(AppData.InputFieldActionType.UserEmailField, callback: fieldHighlightedCallbackResults =>
                    {
                        callbackResults.SetResult(fieldHighlightedCallbackResults);
                    });

                    #endregion
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        #endregion

        #region Tab View Overrides

        protected override void OnTabViewShown(Action<AppData.Callback> callback = null)
        {
        
        }

        protected override void OnTabViewHidden(Action<AppData.Callback> callback = null)
        {

        }

        #endregion

        protected override void OnActionButtonEvent(AppData.TabViewType screenWidgetType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            
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