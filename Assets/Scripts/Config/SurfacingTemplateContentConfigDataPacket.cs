using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    [CreateAssetMenu(fileName = "New Config Data Packet", menuName = "Config Data Packets/ Message Config Data Packet")]
    public class SurfacingTemplateContentConfigDataPacket : AppData.ScriptableConfigDataPacket<AppData.ConfigDataType>, AppData.IScriptableConfigDataPacket<AppData.SurfacingContentType>
    {
        #region Components

        [Space(10)]
        [Header("Config Packet")]

        [Space(5)]
        public AppData.SurfacingContentType configType;

        [Header("Content Info")]

        [Space(5)]
        public string title;

        [Space(5)]
        [TextArea]
        public string message;

        [Space(5)]
        public AppData.ImageComponent icon;

        [Space(5)]
        public AppData.ImageComponent backgroundImage;

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

        public void SetConfigType(AppData.SurfacingContentType configType, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            if(configType != AppData.SurfacingContentType.None)
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

        public AppData.CallbackData<string> GetMessage(params string[] messageOverrides)
        {
            var callbackResults = new AppData.CallbackData<string>(Initialized());

            if(callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(messageOverrides, "Message Overrides", "Get Message With Overrides Unsuccessful - Message Overrides Params Are Not Assigned - Continuing Execution."));

                if (callbackResults.Success())
                {
                    var newMessage = string.Empty;

                    for (int i = 0; i < messageOverrides.Length; i++)
                        newMessage = message.Replace($"[{i}]", messageOverrides[i]);

                    callbackResults.SetResult(AppData.Helpers.GetAppStringValueNotNullOrEmpty(newMessage, "New Message", "Get Message With Overrides Unsuccessful - New Message String Is Null - Invalid Operation."));

                    if (callbackResults.Success())
                    {
                        callbackResults.result = $"Message For : {GetName()} - Has Been Successfully Set To : {newMessage}";
                        callbackResults.data = newMessage;
                    }
                    else
                    {
                        callbackResults.result = $"Message For : {GetName()} - Couldn't Be Overridden - Returning Base Message : {message}";
                        callbackResults.data = message;
                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                    }
                }
                else
                {
                    callbackResults.result = $"Message For : {GetName()} - Has Been Successfully Set To : {message}";
                    callbackResults.data = message;
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
            }

            return callbackResults;
        }

        public AppData.CallbackData<AppData.SurfacingContentType> GetConfigType()
        {
            var callbackResults = new AppData.CallbackData<AppData.SurfacingContentType>();

            if (configType != AppData.SurfacingContentType.None)
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

        public AppData.CallbackData<AppData.ImageComponent> GetIcon()
        {
            var callbackResults = new AppData.CallbackData<AppData.ImageComponent>(AppData.Helpers.GetAppComponentValid(icon, "Icon", "Get Icon Failed - Icon Is Not Assigned."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(icon.GetImageInfo());

                if(callbackResults.Success())
                {
                    callbackResults.result = $"Icon For : {GetName()} - Has Been Successfully Assigned.";
                    callbackResults.data = icon;
                }
            }

            return callbackResults;
        }

        public AppData.CallbackData<AppData.ImageComponent> GetBackgroundImage()
        {
            var callbackResults = new AppData.CallbackData<AppData.ImageComponent>(AppData.Helpers.GetAppComponentValid(backgroundImage, "Background Image", "Get Background Imag Failed - Background Imag Is Not Assigned."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(backgroundImage.GetImageInfo());

                if (callbackResults.Success())
                {
                    callbackResults.result = $"Background Image For : {GetName()} - Has Been Successfully Assigned.";
                    callbackResults.data = backgroundImage;
                }
            }

            return callbackResults;
        }

        #endregion

        #endregion
    }
}