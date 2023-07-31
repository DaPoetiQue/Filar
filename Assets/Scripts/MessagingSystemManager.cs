using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class MessagingSystemManager : AppMonoBaseClass
    {
        #region Static

        private static MessagingSystemManager _instance;

        public static MessagingSystemManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<MessagingSystemManager>();

                return _instance;
            }
        }

        #endregion

        #region Components

        [Header("Message Group")]
        [Space(5)]
        public List<AppData.MessageGroup> messageGroups = new List<AppData.MessageGroup>();

        #endregion

        #region Main

        public void GetMessageCroups(Action<AppData.CallbackData<AppData.MessageGroup>> callback)
        {

        }

        public void GetMessageGroup(AppData.UIScreenType screenIdentifier, Action<AppData.CallbackData<AppData.MessageGroup>> callback)
        {
            AppData.CallbackData<AppData.MessageGroup> callbackResults = new AppData.CallbackData<AppData.MessageGroup>();

            AppData.Helpers.GetAppComponentsValid(messageGroups, "MessageGroup List", messageGroupCallbackResults => 
            {
                callbackResults.results = messageGroupCallbackResults.results;
                callbackResults.resultsCode = messageGroupCallbackResults.resultsCode;

                if(callbackResults.Success())
                {
                    var messageGroup = messageGroupCallbackResults.data.Find(group => group.GetScreenIdentifier() == screenIdentifier);

                    if(messageGroup != null)
                    {
                        callbackResults.results = $"Message Group : {messageGroup.name} - For Screen ID : {screenIdentifier} Found.";
                        callbackResults.data = messageGroup;
                    }
                    else
                    {
                        callbackResults.results = $"Couldn't Find Message Group For Screen ID : {screenIdentifier}.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }

            }, "There Were No Message Groups Found - Initialize Message Groups In Messaging System Manager Using The Editor Inspector Panel.");

            callback.Invoke(callbackResults);
        }

        #endregion
    }
}