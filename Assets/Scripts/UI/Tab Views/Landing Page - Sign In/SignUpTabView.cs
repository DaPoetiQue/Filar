using System;
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
                    //callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "App Events Manager Instance Is Not Yet Initialized."));

                    //if (callbackResults.Success())
                    //{
                    //    var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                    //    appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnWidgetShown, AppData.EventType.OnWidgetShownEvent, true);
                    //    appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnWidgetHidden, AppData.EventType.OnWidgetHiddenEvent, true);
                    //}
                    //else
                    //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            });

            callback.Invoke(callbackResults);
        }

        #region On Widget Events

        private void OnWidgetShown(AppData.Widget widget)
        {
            //var callbackResults = new AppData.Callback(widget.GetType());

            //if (callbackResults.Success())
            //{
            //    if (widget.GetType().GetData() == AppData.WidgetType.SignInWidget)
            //    {
            //        HighlightInputFieldValue(AppData.InputFieldActionType.UserNameField, callback: fieldHighlightedCallbackResults =>
            //        {
            //            callbackResults.SetResult(fieldHighlightedCallbackResults);
            //        });
            //    }
            //}
            //else
            //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnWidgetHidden(AppData.Widget widget)
        {
            //var callbackResults = new AppData.Callback(widget.GetType());

            //if (callbackResults.Success())
            //{
            //    if (widget.GetType().GetData() == AppData.WidgetType.SignInWidget)
            //    {
            //        HighlightInputFieldValue(AppData.InputFieldActionType.UserNameField, false, fieldHighlightedCallbackResults =>
            //        {
            //            callbackResults.SetResult(fieldHighlightedCallbackResults);
            //        });
            //    }
            //}
            //else
            //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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

                                                        callbackResults.SetResult(AppData.Helpers.GetAppStringValueEqual(userPassword, userPasswordVarification, "Varifying Password"));

                                                        if(callbackResults.Success())
                                                        {
                                                            userProfile.SetUserName(userName);
                                                            userProfile.SetUserEmail(userEmail);
                                                            userProfile.SetUserPassword(userPassword);

                                                            callbackResults.SetResult(userProfile.Initialized());

                                                            if (callbackResults.Success())
                                                            {
                                                                LogSuccess($"***_Log_cat: Sign Up Success - Name : {userName} - Email : {userEmail} - Password : {userPassword} - Password Varification : {userPasswordVarification} ", this);
                                                            }
                                                            else
                                                            {
                                                                var invalidFieldType = userProfile.Initialized().GetData();

                                                                LogWarning($"***_Log_cat: Sign Up Failed - {invalidFieldType} Field Is Invalid", this);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            LogWarning($"***_Log_cat: Sign Up Failed - Passwords Doesn't Match - Password : {userPassword} -Varification Pasword : {userPasswordVarification}", this);
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