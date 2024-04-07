using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    [CreateAssetMenu(fileName = "New Dynamic UI Text Content Config Data Packet", menuName = "Config Data Packets/Dynamic UI Text Content Config Data Packet")]

    public class DynamicUITextContentConfigDataPacket : AppData.ScriptableConfigDataPacket<AppData.ConfigDataType>
    {
        #region Components

        [Space(5)]
        [SerializeField]
        private List<AppData.DynamicUITextComponent> dynamicUITextComponents = new List<AppData.DynamicUITextComponent>();

        #endregion

        #region Main

        public AppData.Callback Initialized()
        {
            var callbackResults = new AppData.Callback(GetDynamicUITextComponents());
            return callbackResults;
        }

        public AppData.CallbackDataList<AppData.DynamicUITextComponent> GetDynamicUITextComponents()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.DynamicUITextComponent>(AppData.Helpers.GetAppComponentsValid(dynamicUITextComponents, "Dynamic UI Text Components", "Get Dynamic UI Text Components Failed - There Are No Dynamic UI Text Components Found - Invalid operation."));

            if(callbackResults.Success())
            {
                var initializedDynamicUITextComponents = dynamicUITextComponents.FindAll(x => x.Initialized().Success());

                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(initializedDynamicUITextComponents, "Initialized Dynamic UI Text Components", "Get Dynamic UI Text Components Failed - There Are No Initialized Dynamic UI Text Components Found - Invalid Operation."));

                if (callbackResults.Success())
                {
                    callbackResults.result = $"Get Dynamic UI Text Components Success - There Are : {initializedDynamicUITextComponents.Count} Initialized Dynamic UI Text Components Found.";
                    callbackResults.data = initializedDynamicUITextComponents;
                }
            }

            return callbackResults;
        }

        #endregion
    }
}