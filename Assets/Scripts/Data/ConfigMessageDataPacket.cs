using UnityEngine;

namespace Com.RedicalGames.Filar
{
    [CreateAssetMenu(fileName = "New Config Data Packet", menuName = "Config Data Packets/ Message Config Data Packet")]
    public class ConfigMessageDataPacket : AppData.ScriptableConfigDataPacket<AppData.ConfigMessageType>
    {
        #region Components

        [Space(5)]
        [Header("Message Data")]

        [Space(5)]
        public string title;

        [Space(5)]
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

        public void SetMessageType(AppData.ConfigMessageType messageType) => this.type = messageType;

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

        public AppData.CallbackData<string> GetMessage()
        {
            var callbackResults = new AppData.CallbackData<string>();

            if (!string.IsNullOrEmpty(message))
            {
                callbackResults.result = $"Message For : {GetName()} - Has Been Successfully Set To : {message}";
                callbackResults.data = message;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Failed To Get Message For : {GetName()} - Message Value Is Not Assigned.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            return callbackResults;
        }

        #endregion

        #endregion
    }
}