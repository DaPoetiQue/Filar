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

        [Space(5)]
        public AppData.StorageType messageStorageType = AppData.StorageType.App_Information;

        #endregion

        #region Main

        public void GetMessageGroup(AppData.UIScreenType screenIdentifier, Action<AppData.CallbackData<AppData.MessageGroup>> callback)
        {
            AppData.CallbackData<AppData.MessageGroup> callbackResults = new AppData.CallbackData<AppData.MessageGroup>();

            LoadMessageGroups(messageGroupsLoadedCallbackResults =>
            {
                callbackResults.result = messageGroupsLoadedCallbackResults.result;
                callbackResults.resultCode = messageGroupsLoadedCallbackResults.resultCode;

                if (callbackResults.Success())
                {
                   

                    foreach (var item in messageGroupsLoadedCallbackResults.data.messageGroups)
                    {
                        LogInfo($" <=================================> Loaded : {item.name} For Screen ID : {item.screenIdentifier}");
                    }

                    var messageGroup = messageGroupsLoadedCallbackResults.data.messageGroups.Find(group => group.GetScreenIdentifier() == screenIdentifier);

                    if (messageGroup != null)
                    {
                        callbackResults.result = $"Message Group : {messageGroup.name} - For Screen ID : {screenIdentifier} Found.";
                        callbackResults.data = messageGroup;
                    }
                    else
                    {
                        callbackResults.result = $"Couldn't Find Message Group For Screen ID : {screenIdentifier}.";
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }
            });

            callback.Invoke(callbackResults);
        }

        #region Load Messages

        void LoadMessageGroups(Action<AppData.CallbackData<AppData.MessageGroupData>> callback = null)
        {
            AppData.CallbackData<AppData.MessageGroupData> callbackResults = new AppData.CallbackData<AppData.MessageGroupData>();

            GetMessageCroups(messageGroupsCallbackResults =>
            {
                callbackResults.result = messageGroupsCallbackResults.result;
                callbackResults.resultCode = messageGroupsCallbackResults.resultCode;

                if (callbackResults.Success())
                {
                    AppData.Helpers.GetAppComponentValid(SceneAssetsManager.Instance, SceneAssetsManager.Instance.name, sceneAssetsManagerCallbackResults =>
                    {
                        callbackResults.result = sceneAssetsManagerCallbackResults.result;
                        callbackResults.resultCode = sceneAssetsManagerCallbackResults.resultCode;

                        if (callbackResults.Success())
                        {
                            callbackResults.result = sceneAssetsManagerCallbackResults.data.GetAppDirectoryData(messageStorageType).result;
                            callbackResults.resultCode = sceneAssetsManagerCallbackResults.data.GetAppDirectoryData(messageStorageType).resultCode;

                            if (callbackResults.Success())
                            {
                                sceneAssetsManagerCallbackResults.data.GetStorageData("Message Groups", messageStorageType, storageDataCallbackResults =>
                                {
                                    callbackResults.result = storageDataCallbackResults.result;
                                    callbackResults.resultCode = storageDataCallbackResults.resultCode;

                                    if (callbackResults.Success())
                                    {
                                        AppData.MessageGroupData messageGroupData = new AppData.MessageGroupData
                                        {
                                            name = "Message Groups",
                                            messageGroups = messageGroupsCallbackResults.data,
                                            storageData = storageDataCallbackResults.data
                                        };

                                        sceneAssetsManagerCallbackResults.data.LoadData<AppData.MessageGroupData>(storageDataCallbackResults.data, loadedDataCallbackResults => 
                                        {
                                            callbackResults.result = loadedDataCallbackResults.result;
                                            callbackResults.resultCode = loadedDataCallbackResults.resultCode;

                                            if (callbackResults.Success())
                                                callbackResults.data = loadedDataCallbackResults.data;
                                            else
                                            {
                                                sceneAssetsManagerCallbackResults.data.CreateData(messageGroupData, storageDataCallbackResults.data, dataCreatedCallbackResults =>
                                                {
                                                    callbackResults.result = sceneAssetsManagerCallbackResults.result;
                                                    callbackResults.data = messageGroupData;
                                                    callbackResults.resultCode = sceneAssetsManagerCallbackResults.resultCode;

                                                    Log(callbackResults.resultCode, callbackResults.result, this);
                                                });
                                            }
                                        });
                                    }
                                });
                            }
                        }

                    }, "Scene Assets Manager Is Not Yet Initialized");
                }
            });

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Message Accessor

        void GetMessageCroups(Action<AppData.CallbackDataList<AppData.MessageGroup>> callback)
        {
            AppData.CallbackDataList<AppData.MessageGroup> callbackResults = new AppData.CallbackDataList<AppData.MessageGroup>();

            AppData.Helpers.GetAppComponentsValid(messageGroups, "Message Group List", messageGroupCallbackResults =>
            {
                callbackResults.result = messageGroupCallbackResults.result;
                callbackResults.resultCode = messageGroupCallbackResults.resultCode;

                if (callbackResults.Success())
                    callbackResults.data = messageGroupCallbackResults.data;

            }, "There Were No Message Groups Found - Initialize Message Groups In Messaging System Manager Using The Editor Inspector Panel.", $"{messageGroups.Count} Message Group(s) Found.");

            callback.Invoke(callbackResults);
        }

        #endregion

        #endregion
    }
}