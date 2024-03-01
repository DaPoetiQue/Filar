using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    [CreateAssetMenu(fileName = "New Config Data Packet", menuName = "Config Data Packets/ Message Config Data Packet")]
    public class ConfigMessageDataPacket : AppData.ScriptableConfigDataPacket<AppData.ConfigDataType>, AppData.IScriptableConfigDataPacket<AppData.ConfigMessageType>
    {
        #region Components

        [Space(10)]
        [Header("Config Packet")]

        [Space(5)]
        public AppData.ConfigMessageType configType;

        [Header("Message Info")]

        [Space(5)]
        public string title;

        [Space(5)]
        [TextArea]
        public string message;

        #endregion

        #region Main

        public AppData.Callback Initialized()
        {
            var callbackResults = new AppData.Callback(GetType());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetTitle());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(GetMessage());
                }
            }

            return callbackResults;
        }

        #region Data Setters

        public void SetTitle(string title) => this.title = title;
        public void SetMessage(string message) => this.message = message;

        public void SetConfigType(AppData.ConfigMessageType configType, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            if(configType != AppData.ConfigMessageType.None)
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

        #region Data Getters

        public AppData.CallbackData<string> GetTitle()
        {
            var callbackResults = new AppData.CallbackData<string>();

            if (!string.IsNullOrEmpty(title))
            {
                callbackResults.result = $"Title For : {GetName()} - Has Been Successfully Set To : {title}";
                callbackResults.data = title;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Failed To Get Title For : {GetName()} - Title Value Is Not Assigned.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            return callbackResults;
        }

        public AppData.CallbackData<string> GetMessage(string replace = null)
        {
            var callbackResults = new AppData.CallbackData<string>();

            if (!string.IsNullOrEmpty(message))
            {
                if (!string.IsNullOrEmpty(replace))
                {
                    var newMessage = message.Replace("[0]", replace);

                    callbackResults.result = $"Message For : {GetName()} - Has Been Successfully Set To : {newMessage}";
                    callbackResults.data = newMessage;
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.result = $"Message For : {GetName()} - Has Been Successfully Set To : {message}";
                    callbackResults.data = message;
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
            }
            else
            {
                callbackResults.result = $"Failed To Get Message For : {GetName()} - Message Value Is Not Assigned.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            return callbackResults;
        }


        public AppData.CallbackData<AppData.ConfigMessageType> GetConfigType()
        {
            var callbackResults = new AppData.CallbackData<AppData.ConfigMessageType>();

            if (configType != AppData.ConfigMessageType.None)
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

        #endregion
    }
}