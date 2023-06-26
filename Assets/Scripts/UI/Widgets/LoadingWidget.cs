using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class LoadingWidget : AppData.Widget
    {
        #region Components

        Coroutine spinnerRoutine;

        bool loadingCompleted;

        Queue<AppData.ScreenLoadingInitializationData> loadingQueue = new Queue<AppData.ScreenLoadingInitializationData>();

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            loadingWidget = this;
            base.Init();
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

                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
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

                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
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

        #endregion

        #endregion

    }
}