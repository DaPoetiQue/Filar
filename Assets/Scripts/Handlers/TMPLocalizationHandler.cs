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