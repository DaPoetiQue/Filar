using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class NetworkNotificationWidget : AppData.Widget
    {
        #region Components

        #endregion

        #region Main

        protected override void Initialize(Action<AppData.CallbackData<AppData.WidgetStatePacket>> callback)
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>();

            callbackResults.SetResult(GetType());

            if (callbackResults.Success())
            {
                OnRegisterWidget(this, onRegisterWidgetCallbackResults =>
                {
                    callbackResults.SetResult(GetType());

                    if (callbackResults.Success())
                    {
                        var widgetStatePacket = new AppData.WidgetStatePacket(name: GetName(), type: GetType().data, stateType: AppData.WidgetStateType.Initialized, value: this);

                        callbackResults.result = $"Widget : {GetName()} Of Type : {GetType().data}'s State Packet Has Been Initialized Successfully.";
                        callbackResults.data = widgetStatePacket;
                    }
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback.Invoke(callbackResults);
        }

        protected override void OnScreenWidget()
        {

        }

        void SetWidgetAssetData(AppData.SceneAsset asset)
        {
            if (titleDisplayer != null && !string.IsNullOrEmpty(asset.name))
                titleDisplayer.text = asset.name;
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            if (AppDatabaseManager.Instance)
                SetWidgetAssetData(AppDatabaseManager.Instance.GetCurrentSceneAsset());
            else
                Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
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

                    switch (screenUIManager.GetCurrentUIScreenType())
                    {
                        case AppData.UIScreenType.LoadingScreen:

                            screenUIManager.GetCurrentScreenData().value.HideScreenWidget(this);

                            AppData.SceneDataPackets loadingStateWidgetDataPackets = new AppData.SceneDataPackets
                            {
                                screenType = screenUIManager.GetCurrentUIScreenType(),
                                widgetType = AppData.WidgetType.LoadingWidget
                            };

                            AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, LoadingManager.Instance.name, async hasLoadingManagerCallbackResults =>
                            {
                                callbackResults.SetResult(hasLoadingManagerCallbackResults);

                                if (callbackResults.Success())
                                {
                                    var loadingManager = hasLoadingManagerCallbackResults.data;

                                    if (loadingManager.OnInitialLoad)
                                    {
                                        screenUIManager.GetCurrentScreenData().value.ShowWidget(loadingStateWidgetDataPackets);
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

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket> OnGetState()
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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

        #endregion
    }
}
