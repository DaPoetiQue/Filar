using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Firebase.Database;

namespace Com.RedicalGames.Filar
{
    public class LocalizationManager : AppData.SingletonBaseComponent<LocalizationManager>
    {
        #region Components


        private AppData.LanguageRestriction  languageRestriction;
        private bool generateLanguageRestriction;

        DatabaseReference databaseReference;

        #endregion

        #region Main

        protected override async void Init()
        {
            if (generateLanguageRestriction)
            {
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
             
                string languageFile = JsonUtility.ToJson(languageRestriction);

                Dictionary<string, object> languageFileObject = new Dictionary<string, object>();
                languageFileObject.Add("Localization-Filar", languageFile);

                await databaseReference.Child("Filar Localization").Child("Language Restrictions").UpdateChildrenAsync(languageFileObject);
            }
        }

        public void SyncLocalizationData(AppData.LanguageRestriction languageRestriction, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(languageRestriction, "Language Restrictions", "Get Language Black List Failed - Language Restriction Parameter Value Is Missing / Null - Invalid Operation."));

            if(callbackResults.Success())
            {
                this.languageRestriction = languageRestriction;
                callbackResults.result = "Get Language Black List Success - Language Restriction Has been Successfully Synced.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackData<AppData.LanguageRestriction > GetLanguageRestriction()
        {
            var callbackResults = new AppData.CallbackData<AppData.LanguageRestriction >();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(languageRestriction, "Language Restriction", "Get Language Restriction Failed - Language Restriction Is Not Initialized - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(languageRestriction.Initialized());

                if(callbackResults.Success())
                    callbackResults.data = languageRestriction;
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<string> UnRestricted(string validatedValue)
        {
            var callbackResults = new AppData.CallbackData<string>(AppData.Helpers.GetAppStringValueNotNullOrEmpty(validatedValue, "Validated Value", "Unrestricted Failed - Validated Name Parameter Value Is Null / Empty - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetLanguageRestriction());

                if(callbackResults.Success())
                {
                    var restricted = GetLanguageRestriction().GetData().GetRetrictedList().GetData();

                    for (int i = 0; i < restricted.Count; i++)
                    {
                        if (validatedValue.Contains(restricted[i]))
                        {
                            callbackResults.result = $"Value : {validatedValue} - Usage Restricted.";
                            callbackResults.data = default;
                            callbackResults.resultCode = AppData.Helpers.WarningCode;

                            break;
                        }
                        else
                        {
                            callbackResults.result = $"Value : {validatedValue} - Usage Not Restricted.";
                            callbackResults.data = validatedValue;
                            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                        }
                    }
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion
    }
}