using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UILoadStateIndicatorWidget : AppData.Widget
    {
        #region Components

        Coroutine spinnerRoutine;

        bool loadingCompleted;

        Queue<AppData.ScreenLoadingInitializationData> loadingQueue = new Queue<AppData.ScreenLoadingInitializationData>();

        #endregion

        #region Main

        protected override void Initialize(Action<AppData.CallbackData<AppData.WidgetStatePacket>> callback)
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>();

            callbackResults.SetResult(GetType());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetType());

                if (callbackResults.Success())
                {
                    OnRegisterWidget(this, onRegisterWidgetCallbackResults =>
                    {
                        callbackResults.SetResult(onRegisterWidgetCallbackResults);

                        if (callbackResults.Success())
                        {
                            var widgetStatePacket = new AppData.WidgetStatePacket(name: GetName(), type: GetType().data, stateType: AppData.WidgetStateType.Initialized, value: this);

                            callbackResults.result = $"Widget : {GetName()} Of Type : {GetType().data}'s State Packet Has Been Initialized Successfully.";
                            callbackResults.data = widgetStatePacket;
                        }
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    });
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback.Invoke(callbackResults);
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

        protected override void OnScreenWidget()
        {

        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            if (loadingQueue.Count > 0)
            {
                if (spinnerRoutine != null)
                {
                    StopAllCoroutines();
                    spinnerRoutine = null;
                }

                loadingCompleted = false;

                spinnerRoutine = StartCoroutine(OnLoadingSpinner((loadingCallbackResults) =>
                {
                    
                }));
            }
        }

        IEnumerator OnLoadingSpinner(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetLayoutView(defaultLayoutType).layout.SetActive(true);

            yield return new WaitForEndOfFrame();

            while (loadingQueue.Count > 0)
            {
                AppData.ScreenLoadingInitializationData loadingData = loadingQueue.Dequeue();

                while (loadingData.Completed() == false)
                {

                    TriggerOnRefreshInProgressEvent();

                    yield return new WaitForSeconds(loadingData.duration);

                    if (loadingData.autoHide)
                    {
                        Hide();

                        TriggerOnRefreshCompletedEvent();

                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                        callback?.Invoke(callbackResults);
                    }
                    else
                    {
                        if (loadingData.isLargeFileSize)
                        {
                            loadingCompleted = true;

                            Hide();

                            TriggerOnRefreshFailedEvent();
                        }
                        else
                        {
                            yield return new WaitUntil(() => loadingCompleted == true);

                            loadingData.SetCompleted();

                            Hide();

                            TriggerOnRefreshCompletedEvent();

                            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                            callback?.Invoke(callbackResults);
                        }
                    }

                    yield return null;
                }

                yield return null;
            }
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            LogInfo($"===============> Subscribe : {subscribe}", this);
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        #region Events


        public event AppData.SettingsWidget.EventsListeners _OnRefreshInProgress_AddEventListener;
        public event AppData.SettingsWidget.EventsListeners _OnRefreshCompleted_AddEventListener;
        public event AppData.SettingsWidget.EventsListeners _OnAbortRefresh_AddEventListener;
        public event AppData.SettingsWidget.EventsListeners _OnRefreshFailed_AddEventListener;


        protected void TriggerOnRefreshInProgressEvent() => _OnRefreshInProgress_AddEventListener?.Invoke();
        protected void TriggerOnRefreshCompletedEvent() => _OnRefreshCompleted_AddEventListener?.Invoke();
        protected void TriggerOnAbortRefreshEvent() => _OnAbortRefresh_AddEventListener?.Invoke();
        protected void TriggerOnRefreshFailedEvent() => _OnRefreshFailed_AddEventListener?.Invoke();

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new NotImplementedException();
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

        #endregion

    }
}