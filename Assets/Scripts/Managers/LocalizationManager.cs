using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Firebase.Database;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Com.RedicalGames.Filar
{
    public class LocalizationManager : AppData.SingletonBaseComponent<LocalizationManager>
    {
        #region Components

        [SerializeField]
        private AppData.LocaleType currentLanguage = AppData.LocaleType.English_en;

        private AppData.LanguageRestriction  languageRestriction;
        private bool generateLanguageRestriction;
        private DatabaseReference databaseReference;
        private AppLocaleConfigDataPacket localeConfigDataPacket;

        #endregion

        #region Main

        protected override async void Init()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.GetName(), "App Database Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.GetName()).GetData();

                callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                if(callbackResults.Success())
                {
                    var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                    var waitForAddressablesCallbackResults = await assetBundlesLibrary.OnAwaitAssetsInitialization(AppData.AssetBundleResourceLocatorType.Config);

                    callbackResults.SetResult(waitForAddressablesCallbackResults);

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(assetBundlesLibrary.GetLoadedConfigData(AppData.ConfigDataType.LocaleConfigData));

                        if (callbackResults.Success())
                        {
                            var configDataPackets = assetBundlesLibrary.GetLoadedConfigData(AppData.ConfigDataType.LocaleConfigData).GetData();

                            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(configDataPackets, "Config Data Packets", "Init Failed - There Are No Config Data Packets Found - Invalid Operation."));

                            if (callbackResults.Success())
                            {
                                var localeConfigDataPacket = configDataPackets[0] as AppLocaleConfigDataPacket;

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(localeConfigDataPacket, "Locale Config Data Packet", "Init Failed - Couldn't Find A Local Config Data Packet - Invalid Operation."));

                                if (callbackResults.Success())
                                {
                                    ChangeLanguage(GetCurrentLanguage().GetData(), languageChangedCallbackResults => 
                                    {
                                        callbackResults.SetResult(languageChangedCallbackResults);

                                        if(callbackResults.Success())
                                        {

                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    });

                                    callbackResults.result = "Init Success - Local Config Data Packet Found.";

                                    this.localeConfigDataPacket = localeConfigDataPacket;

                                    if (generateLanguageRestriction)
                                    {
                                        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                                        string languageFile = JsonUtility.ToJson(languageRestriction);

                                        Dictionary<string, object> languageFileObject = new Dictionary<string, object>();
                                        languageFileObject.Add("Localization-Filar", languageFile);

                                        await databaseReference.Child("Filar Localization").Child("Language Restrictions").UpdateChildrenAsync(languageFileObject);
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

        public AppData.CallbackData<LocalizedString> GetLocaleFromKey(AppData.LocalizationKey localizationKey)
        {
            var callbackResults = new AppData.CallbackData<LocalizedString>(AppData.Helpers.GetAppEnumValueValid(localizationKey, "Localization Key", $"Get Locale From Key Failed - Localization Key Parameter Value Is Set To Default : {localizationKey} - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(localeConfigDataPacket, "Locale Config Data Packet", "GetLocaleFromKey Failed - Trying To Access Locale Config Data Packet While It Is Not Yet Loaded From Addressabless - Invalid Operation."));

                if(callbackResults.Success())
                {
                    callbackResults.SetResult(localeConfigDataPacket.Initialized());

                    if(callbackResults.Success())
                    {
                        callbackResults.SetResult(localeConfigDataPacket.GetLocalizationValueFromKey(localizationKey));

                        if(callbackResults.Success())
                        {
                            callbackResults.result = $"Get Locale From Key Success - Locale Has Been successfully Found Using Localization Key Parameter Value : {localizationKey}";
                            callbackResults.data = localeConfigDataPacket.GetLocalizationValueFromKey(localizationKey).GetData();
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

            return callbackResults;
        }

        public async void ChangeLanguage(AppData.LocaleType LocaleType, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            await LocalizationSettings.InitializationOperation.Task;

            if(LocalizationSettings.InitializationOperation.Task.IsCompleted && LocalizationSettings.InitializationOperation.Task.Exception == null)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)LocaleType];

                // Save Language To Data
            }
            else
            {
                callbackResults.result = $"Change Language Failed With Exception : {LocalizationSettings.InitializationOperation.Task.Exception.Message}";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackData<AppData.LocaleType> GetCurrentLanguage()
        {
            var callbackResults = new AppData.CallbackData<AppData.LocaleType>();

            callbackResults.data = currentLanguage;
            callbackResults.resultCode = AppData.Helpers.SuccessCode;

            return callbackResults;
        }

        #endregion
    }
}