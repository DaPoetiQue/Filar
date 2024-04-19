using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    [CreateAssetMenu(fileName = "Progress Report Info Config Data Packet", menuName = "Config Data Packets/Progress Report Info Config")]
    public class ProgressReportInfoConfigDataPacket : AppData.ScriptableConfigDataPacket<AppData.ConfigDataType>
    {
        #region Components

        [Space(5)]
        [SerializeField]
        private AppData.ProgressReportType configType = AppData.ProgressReportType.None;

        [Space(5)]
        [SerializeField]
        private List<AppData.ProgressReportInfoComponent> progressReportInfoComponents = new List<AppData.ProgressReportInfoComponent>();

        #endregion

        #region Main

        public AppData.Callback Initialized()
        {
            var callbackResults = new AppData.Callback(GetConfigType());

            if(callbackResults.Success())
                callbackResults.SetResult(GetProgressReportInfoComponents());

            return callbackResults;
        }

        public AppData.CallbackDataList<AppData.ProgressReportInfoComponent> GetProgressReportInfoComponents()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.ProgressReportInfoComponent>(AppData.Helpers.GetAppComponentsValid(progressReportInfoComponents, "Progress Report Info Components", "Get Progress Report Info Components Failed - There Are No Progress Report Info Components Found - Invalid operation."));

            if (callbackResults.Success())
            {
                var initializedProgressReportInfoComponents = progressReportInfoComponents.FindAll(x => x.Initialized().Success());

                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(initializedProgressReportInfoComponents, "Initialized Progress Report Info Components", "Get Progress Report Info Components Failed - There Are No Initialized Progress Report Info Components Found - Invalid Operation."));

                if (callbackResults.Success())
                {
                    callbackResults.result = $"Get Progress Report Info Components Success - There Are : {progressReportInfoComponents.Count} Initialized Progress Report Info Components Found.";
                    callbackResults.data = progressReportInfoComponents;
                }
            }

            return callbackResults;
        }

        public AppData.CallbackData<AppData.ProgressReportInfoComponent> GetProgressReportInfoComponent(AppData.ProgressReportInfoState reportInfoState)
        {
            var callbackResults = new AppData.CallbackData<AppData.ProgressReportInfoComponent>(AppData.Helpers.GetAppEnumValueValid(reportInfoState, 
                "Report Info State", $"Get Progress Report Info Component Failed - Report Info State Parameter Value Is set To Default : {reportInfoState} - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.SetResult(GetProgressReportInfoComponents());

                if(callbackResults.Success())
                {
                    var progressReportInfoComponent = GetProgressReportInfoComponents().GetData().Find(progressReport => progressReport.GetState().GetData() == reportInfoState);

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(progressReportInfoComponent, "Progress Report Info Component", 
                        $"Get Progress Report Info Component Failed - Couldn't Find Progress Report Info Component For State : {reportInfoState} - " +
                        $"Component Missing / Not Assigned - Invalid Operation"));

                    if(callbackResults.Success())
                    {
                        callbackResults.result = $"Get Progress Report Info Component Success - A Progress Report Info Component For State : " +
                            $"{reportInfoState} - Has Been Successfully Found.";

                        callbackResults.data = progressReportInfoComponent;
                    }
                }
            }

            return callbackResults;
        }

        public AppData.CallbackData<AppData.ProgressReportType> GetConfigType()
        {
            var callbackResults = new AppData.CallbackData<AppData.ProgressReportType>();

            if (configType != AppData.ProgressReportType.None)
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

        #endregion
    }
}
