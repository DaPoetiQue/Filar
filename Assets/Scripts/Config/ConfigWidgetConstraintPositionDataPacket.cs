using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    [CreateAssetMenu(fileName = "New Config Data Packet", menuName = "Config Data Packets/ Screen Widget Constraint Config Data Packet")]
    public class ConfigWidgetConstraintPositionDataPacket : AppData.ScriptableConfigDataPacket<AppData.ConfigDataType>, AppData.IScriptableConfigDataPacket<AppData.ConstraintType>
    {
        #region Components

        [Space(10)]
        [Header("Config Packet")]

        [Space(5)]
        [SerializeField]
        private AppData.ConstraintType configType;

        [Header("Constraint Data")]

        [Space(5)]
        [SerializeField]
        private List<AppData.ScreenConstraintObject> constraints = new List<AppData.ScreenConstraintObject>();

        #endregion

        #region Main

        #region Data Setters

        #endregion

        #region Data Getters

        public AppData.CallbackData<AppData.ConstraintType> GetConfigType()
        {
            var callbackResults = new AppData.CallbackData<AppData.ConstraintType>();

            if (configType != AppData.ConstraintType.None)
            {
                callbackResults.result = $"Get Config Data Packet For : {GetName()} Successful - Config Type Is Set To Type : {configType}";
                callbackResults.data = configType;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Get Config Data Packet For : {GetName()} Failed  - Config Type Is Set To Default : {configType}";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            return callbackResults;
        }

        public AppData.CallbackDataList<AppData.ScreenConstraintObject> GetConstraints()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.ScreenConstraintObject>(AppData.Helpers.GetAppComponentsValid(constraints, "Constraints", $"Get Constraints Failed - There Are No Constraints Found For Config Data : {GetName()} - Of Type : {GetConfigType().GetData()}."));

            if (callbackResults.Success())
            {
                var initializedConstraints = constraints.FindAll(constraint => constraint.Initialized().Success());

                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(initializedConstraints, "Initialized Constraints", $"Get Constraints Failed - Cound'nt Find All Initialized Constraints - There Are No Initialized Constraints Found For Config Data : {GetName()} Of Type : {GetType().GetData()}"));

                if(callbackResults.Success())
                {
                    callbackResults.result = $"Found : {initializedConstraints.Count} Initialized Constraints For Config Data : {GetName()} - Of Type : {GetType().GetData()}";
                    callbackResults.data = initializedConstraints;
                }
            }

            return callbackResults;
        }

        #endregion

        public AppData.Callback Initialized()
        {
            var callbackResults = new AppData.Callback(GetType());

            if (callbackResults.Success())
                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(constraints, "Constraints", $"Constraints Are Not Initialized For : {GetName()} Of Type : {GetType().GetData()}"));

            return callbackResults;
        }

        public void SetConfigType(AppData.ConstraintType configType, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            if (configType != AppData.ConstraintType.None)
            {
                callbackResults.result = $"Config Type For : {GetName()} - Has Been Successfully Assigned To Type : {configType}";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Failed To Set Config Type For : {GetName()} - Config Type Parameter Value Is Set To Default : {configType}";
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            callback?.Invoke(callbackResults);
        }

        #endregion
    }
}