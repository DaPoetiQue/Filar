using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                        callbackResults.result = $"Widget : {GetStatePacket().GetName()} Of Type : {GetStatePacket().GetType()} State Is Set To : {GetStatePacket().GetStateType().GetData()}";
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

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket)
        {

        }

        protected override async void OnShowScreenWidget(Action<AppData.Callback> callback = null)
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

                var onLoadingSpinnerTaskResuts = await OnLoadingSpinner();

                if (onLoadingSpinnerTaskResuts.UnSuccessful())
                    Log(onLoadingSpinnerTaskResuts.GetResultCode, onLoadingSpinnerTaskResuts.GetResult, this);
            }
        }

        private async Task<AppData.Callback> OnLoadingSpinner()
        {
            AppData.Callback callbackResults = new AppData.Callback(GetLayoutView());

            if (callbackResults.Success())
            {
                while (loadingQueue.Count > 0)
                {
                    AppData.ScreenLoadingInitializationData loadingData = loadingQueue.Dequeue();

                    while (loadingData.Completed() == false)
                    {

                        TriggerOnRefreshInProgressEvent();

                        int delay = (int)loadingData.duration * 1000;
                        await Task.Delay(delay);

                        if (loadingData.autoHide)
                        {
                            HideWidget();

                            TriggerOnRefreshCompletedEvent();

                            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                            break;
                        }
                        else
                        {
                            if (loadingData.isLargeFileSize)
                            {
                                loadingCompleted = true;

                                HideWidget();

                                TriggerOnRefreshFailedEvent();
                            }
                            else
                            {
                                while (!loadingCompleted)
                                    await Task.Yield();

                                loadingData.SetCompleted();

                                HideWidget();

                                TriggerOnRefreshCompletedEvent();

                                callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                break;
                            }

                            await Task.Yield();
                        }

                        await Task.Yield();
                    }
                }
            }

            return callbackResults;
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
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

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }
}