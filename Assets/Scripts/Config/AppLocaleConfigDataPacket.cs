using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Com.RedicalGames.Filar
{
    [CreateAssetMenu(fileName = "New App Locale Config Data Packet", menuName = "App Locale/Config Data Packet")]
    public class AppLocaleConfigDataPacket : AppData.ScriptableConfigDataPacket<AppData.ConfigDataType>
    {
        #region Components

        [Space(10)]
        [Header("Localization Keys")]

        [Space(5)]
        [SerializeField]
        private List<AppData.LocalizationKeyValuePair> localizationKeyValuePairs = new List<AppData.LocalizationKeyValuePair>();

        #endregion

        #region Main

        #region Localization Keys

        public AppData.Callback Initialized()
        {
            var callbackResults = new AppData.Callback(GetLocalizationKeyValuePairs());
            return callbackResults;
        }

        public AppData.CallbackData<LocalizedString> GetLocalizationValueFromKey(AppData.LocalizationKey localizationKey)
        {
            var callbackResults = new AppData.CallbackData<LocalizedString>(GetLocalizationKeyValuePairs());

            if (callbackResults.Success())
            {
                var localizationKeyValuePair = GetLocalizationKeyValuePairs().GetData().Find(key => key.GetKey().GetData() == localizationKey);

                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(localizationKeyValuePair, "Localization Key Value Pair", $"Get Localization Key Value Pair Failed - Couldn't Find Localization Key Value Pair For Key : {localizationKey} - Invalid Operation."));

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(localizationKeyValuePair.GetValue());

                    if (callbackResults.Success())
                    {
                        callbackResults.result = $"Get Localization Key Value Pair Success - Localization Key Value Pair For Key : {localizationKey} Has Been Found.";
                        callbackResults.data = localizationKeyValuePair.GetValue().GetData();
                    }
                }
            }

            return callbackResults;
        }

        public AppData.CallbackData<AppData.LocalizationKeyValuePair> GetLocalizationKeyValuePair(AppData.LocalizationKey localizationKey)
        {
            var callbackResults = new AppData.CallbackData<AppData.LocalizationKeyValuePair>(GetLocalizationKeyValuePairs());

            if (callbackResults.Success())
            {
                var localizationKeyValuePair = GetLocalizationKeyValuePairs().GetData().Find(key => key.GetKey().GetData() == localizationKey);

                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(localizationKeyValuePair, "Localization Key Value Pair", $"Get Localization Key Value Pair Failed - Couldn't Find Localization Key Value Pair For Key : {localizationKey} - Invalid Operation."));

                if (callbackResults.Success())
                {
                    callbackResults.result = $"Get Localization Key Value Pair Success - Localization Key Value Pair For Key : {localizationKey} Has Been Found.";
                    callbackResults.data = localizationKeyValuePair;
                }
            }

            return callbackResults;
        }

        public AppData.CallbackDataList<AppData.LocalizationKeyValuePair> GetLocalizationKeyValuePairs()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.LocalizationKeyValuePair>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(localizationKeyValuePairs, "Localization Key Value Pairs", "Get Localization Key Value Pairs Failed - There Are No Localization Key Value Pairs Found - Invalid Operation."));

            if (callbackResults.Success())
            {
                var initializedLocalizationKeyValuePairs = localizationKeyValuePairs.FindAll(key => key.Initialized().Success());

                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(initializedLocalizationKeyValuePairs, "Initialized Localization Key Value Pairs", "Get Localization Key Value Pairs Failed - There Are No Initialized Localization Key Value Pairs Found - Invalid Operation."));

                if (callbackResults.Success())
                {
                    callbackResults.result = $"Get Localization Key Value Pairs Success - There Are {initializedLocalizationKeyValuePairs.Count} Initialized Localization Key Value Pairs Found.";
                    callbackResults.data = initializedLocalizationKeyValuePairs;
                }
            }

            return callbackResults;
        }

        #endregion

        #endregion
    }
}