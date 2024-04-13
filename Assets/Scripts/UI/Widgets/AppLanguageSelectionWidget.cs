using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class AppLanguageSelectionWidget : AppData.Widget
    {

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

        protected override void Configure(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetScreenTitleLocalizationKey());

            if (callbackResults.Success())
            {
                SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, GetScreenTitleLocalizationKey().GetData(), titleSetCallbackResults =>
                {
                    callbackResults.SetResult(titleSetCallbackResults);

                    if (callbackResults.UnSuccessful())
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });

                SetActionButtonTitle(AppData.InputActionButtonType.ConfirmationButton, AppData.LocalizationKey.btn_Confirm, buttonTitleSetCallbackResults => 
                {
                    callbackResults.SetResult(buttonTitleSetCallbackResults);

                    if (callbackResults.UnSuccessful())
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });

                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "App Language Selection Widget Configure Failed - App Database Manager Instance Is Not Initialized Yet - Invalid Operation."));

                if(callbackResults.Success())
                {
                    var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                    var languageFormatReplacement = ("_SouthAfrica", " [South Africa]");

                    callbackResults.SetResult(AppData.Helpers.GetFormatedTextStrings(appDatabaseManagerInstance.GetDropdownContent<AppData.LocaleType>().data, false, languageFormatReplacement));

                    if (callbackResults.Success())
                    {
                        var languages = AppData.Helpers.GetFormatedTextStrings(appDatabaseManagerInstance.GetDropdownContent<AppData.LocaleType>().data, false, languageFormatReplacement).GetData();

                        var appLanguages = appDatabaseManagerInstance.GetUIScreenGroupContentTemplate("Languages", AppData.InputType.DropDown, placeHolder: "English", contents: languages, dropdownActionType: AppData.InputDropDownActionType.LanguageSelection);

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(LocalizationManager.Instance, "Localization Manager Instance", "App Language Selection Widget Configure Failed - Localization Manager Instance Is Not Initialized Yet - Invalid Operation."));

                        if (callbackResults.Success())
                        {
                            var localizationManagerInstance = AppData.Helpers.GetAppComponentValid(LocalizationManager.Instance, "Localization Manager Instance").GetData();

                            callbackResults.SetResult(localizationManagerInstance.GetCurrentLanguage());

                            if (callbackResults.Success())
                            {
                                SetActionDropdownContent((int)localizationManagerInstance.GetCurrentLanguage().GetData(), languagesSetCallbackResults =>
                                {
                                    callbackResults.SetResult(languagesSetCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                }, appLanguages);
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

            callback?.Invoke(callbackResults);
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

        protected override void OnScreenWidget(Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {

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

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValueValid(actionType, "Action Type", $"On Action Button Event Failed - Action Type Parameter Value Is set To Default : {actionType} - Invalid Operation."));

            if(callbackResults.Success())
            {
                if(actionType == AppData.InputActionButtonType.ConfirmationButton)
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppManager.Instance, "App Manager Instance", "On Action Button Event Failed - App Manager Instance Is Not Initialized Yet - Invalid Operation."));

                    if(callbackResults.Success())
                    {
                        var appManagerInstance = AppData.Helpers.GetAppComponentValid(AppManager.Instance, "App Manager Instance").GetData();

                        appManagerInstance.CacheAppSettingsDataFile(appLanguageInfoSetCallbackResults => 
                        {
                            callbackResults.SetResult(appLanguageInfoSetCallbackResults);
                        
                            if(callbackResults.UnSuccessful())
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

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(LocalizationManager.Instance, "Localization Manager Instance", "On Action Dropdown Value Changed Failed - Localization Manager Instance Is No Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var localizationManagerInstance = AppData.Helpers.GetAppComponentValid(LocalizationManager.Instance, "Localization Manager Instance").GetData();

                var selectedLanguage = (AppData.LocaleType)value;

                localizationManagerInstance.ChangeLanguage(selectedLanguage, languageChangesCallbackResults => 
                {
                    callbackResults.SetResult(languageChangesCallbackResults);

                    if(callbackResults.Success())
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        protected override void ScrollerPosition(Vector2 position)
        {

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
            throw new NotImplementedException();
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {

        }

        #endregion
    }
}
