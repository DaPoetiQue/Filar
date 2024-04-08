using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro;

namespace Com.RedicalGames.Filar
{
    [RequireComponent(typeof(LocalizeStringEvent))]
    [RequireComponent(typeof(TMP_Text))]

    public class TMPLocalizationHandler : AppMonoBaseClass
    {
        private TMP_Text textComponent;
        private LocalizeStringEvent stringEvent;

        #region Localization

        public void SetLocalizedString(AppData.LocalizationKey localizationKey, Action<AppData.Callback> callback)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValueValid(localizationKey, "Localization Key", $"The Localization Key Parameter Value Is Set To Default : {localizationKey} - For {GetName()} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(LocalizationManager.Instance, "Localization Manager Instance", "Set Localized String Failed - Localization Manager Instance Is Not Initialized - Invalid Operation."));

                if(callbackResults.Success())
                {
                    var localizationManagerInstance = AppData.Helpers.GetAppComponentValid(LocalizationManager.Instance, "Localization Manager Instance").GetData();

                    callbackResults.SetResult(localizationManagerInstance.GetLocaleFromKey(localizationKey));

                    if(callbackResults.Success())
                    {
                        SetLocalizedString(localizationManagerInstance.GetLocaleFromKey(localizationKey).GetData(), localizationSetCallbackResults => 
                        {
                            callbackResults.SetResult(localizationSetCallbackResults);
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

            callback?.Invoke(callbackResults);
        }

        public void SetLocalizedString(LocalizedString localizedString, Action<AppData.Callback> callback)
        {
            if(stringEvent == null)
                stringEvent = GetComponent<LocalizeStringEvent>();

            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(stringEvent, "String Event", $"There Is No String Event Found For TMP Localization Handler : {GetName()} - Invalid Operation."));

            if (callbackResults.Success())
            {
                stringEvent.StringReference.SetReference(localizedString.TableReference, localizedString.TableEntryReference);

                stringEvent.RefreshString();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public AppData.Callback SetLocalizedString(LocalizedString localizedString)
        {
            if (stringEvent == null)
                stringEvent = GetComponent<LocalizeStringEvent>();

            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(stringEvent, "String Event", $"There Is No String Event Found For TMP Localization Handler : {GetName()} - Invalid Operation."));

            if (callbackResults.Success())
            {
                stringEvent.StringReference.SetReference(localizedString.TableReference, localizedString.TableEntryReference);

                stringEvent.RefreshString();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public void SetUITextComponentConfig(AppData.UITextComponentConfig uiTextComponentConfig, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(uiTextComponentConfig, "UI Text Component Config", $"Set UI Text Component Config Failed - UI Text Component Config Parameter Value For : {GetName()} Is Null - Invalid Operation"));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(uiTextComponentConfig.Initialized());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(GetTextComponent());

                    if(callbackResults.Success())
                    {
                        GetTextComponent().GetData().font = uiTextComponentConfig.GetFont().GetData();
                        GetTextComponent().GetData().fontWeight = uiTextComponentConfig.GetFontWeight().GetData();
                        GetTextComponent().GetData().fontStyle = uiTextComponentConfig.GetFontStyles().GetData();
                        GetTextComponent().GetData().alignment = uiTextComponentConfig.GetTextAlignmentOptions().GetData();
                        GetTextComponent().GetData().fontSizeMin = uiTextComponentConfig.GetFontSize().GetData().fontMinSize;
                        GetTextComponent().GetData().fontSizeMax = uiTextComponentConfig.GetFontSize().GetData().fontMaxSize;
                        //GetTextComponent().GetData().lineSpacing = uiTextComponentConfig.GetTextLineInfo().GetData().lineSpacing;
                        //GetTextComponent().GetData().lineSpacingAdjustment = uiTextComponentConfig.GetTextLineInfo().GetData().lineSpacingAdjustment;
                        //GetTextComponent().GetData().maxVisibleLines = uiTextComponentConfig.GetTextLineInfo().GetData().maxVisibleLines;
                        GetTextComponent().GetData().color = uiTextComponentConfig.GetTextColor().GetData();
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Unlocalized

        public AppData.CallbackData<TMP_Text> GetTextComponent()
        {
            if (textComponent == null)
                textComponent = GetComponent<TMP_Text>();

            var callbackResults = new AppData.CallbackData<TMP_Text>(AppData.Helpers.GetAppComponentValid(textComponent, "Text", "Get Text Component Failed - Text Component Value Is Not Assigned - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = "Get Text Component Success - Text Component Has Been Successfully Found.";
                callbackResults.data = textComponent;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

    }
}