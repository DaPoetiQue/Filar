using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class NetworkNotificationWidget : AppData.Widget
    {
        #region Components

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetType());

                if (callbackResults.Success())
                {
                    var widgetType = GetType().data;

                    callbackResults.SetResult(GetStatePacket().Initialized(widgetType));

                    if (callbackResults.Success())
                    {
                        callbackResults.result = $"Widget : {GetStatePacket().GetName()} Of Type : {GetStatePacket().GetType()} State Is Set To : {GetStatePacket().GetStateType()}";
                        callbackResults.data = GetStatePacket();
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

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
            var networkWarningConfigMessage = scriptableConfigData as ConfigMessageDataPacket;

            SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, networkWarningConfigMessage.GetTitle().GetData(), titleSetCallbackResults => { });
            SetUITextDisplayerValue(AppData.ScreenTextType.InfoDisplayer, networkWarningConfigMessage.GetMessage().GetData(), messageSetCallbackResults => { });
        }

        void SetWidgetAssetData(AppData.SceneAsset asset)
        {
            if (titleDisplayer != null && !string.IsNullOrEmpty(asset.name))
                titleDisplayer.text = asset.name;
        }

        //protected override void OnShowScreenWidget(Action<AppData.Callback> callback = null)
        //{
        //    ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

        //    if (AppDatabaseManager.Instance)
        //        SetWidgetAssetData(AppDatabaseManager.Instance.GetCurrentSceneAsset());
        //    else
        //        Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
        //}

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            if (actionType != AppData.InputActionButtonType.RetryButton)
                return;

            AppData.Callback callbackResults = new AppData.Callback();

            AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, hasScreenUIManagerCallbackResults => 
            {
                callbackResults.SetResult(hasScreenUIManagerCallbackResults);

                if (callbackResults.Success())
                {
                    var screenUIManager = hasScreenUIManagerCallbackResults.data;

                    switch (screenUIManager.GetCurrentScreenType().GetData())
                    {
                        case AppData.ScreenType.LoadingScreen:

                            screenUIManager.GetCurrentScreen().GetData().HideScreenWidget(this);

                            AppData.SceneConfigDataPacket loadingStateWidgetDataPackets = new AppData.SceneConfigDataPacket();

                            dataPackets.SetReferencedScreenType(screenUIManager.GetCurrentScreenType().GetData());
                            dataPackets.SetReferencedWidgetType(AppData.WidgetType.LoadingWidget);
                            dataPackets.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.Default);

                            AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, LoadingManager.Instance.name, async hasLoadingManagerCallbackResults =>
                            {
                                callbackResults.SetResult(hasLoadingManagerCallbackResults);

                                if (callbackResults.Success())
                                {
                                    var loadingManager = hasLoadingManagerCallbackResults.data;

                                    if (loadingManager.OnInitialLoad)
                                    {
                                        screenUIManager.GetCurrentScreen().GetData().ShowWidget(loadingStateWidgetDataPackets);
                                        await loadingManager.GetLoadingSequence().ProcessQueueSequenceAsync();
                                    }
                                    else
                                        LogWarning("Please Do Network Retry Here.", this);
                                }

                            }, "Loading Manager Instance Is Not Yet Initialized.");

                            break;
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            }, "Screen UI Manager Instance Is Not Yet Initialized.");
        }
        
        async void Retry()
        {
         
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
