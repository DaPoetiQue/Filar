using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class LoadingSpinnerWidget : AppData.SettingsWidget
    {
        #region Components

        [SerializeField]
        GameObject content;

        [Space(5)]
        [SerializeField]

        Coroutine spinnerRoutine;

        bool loadingCompleted;

        Queue<AppData.ScreenLoadingInitializationData> loadingQueue = new Queue<AppData.ScreenLoadingInitializationData>();

        #endregion

        #region Components

        #endregion

        #region Main

        public void Show(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

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
                    callbackResults = loadingCallbackResults;
                }));
            }
            else
            {
                callbackResults.result = "Show Spinner Failed : Duration Is Less Than 0.";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        void Hide()
        {
            StopAllCoroutines();
            content.SetActive(false);
        }

        IEnumerator OnLoadingSpinner(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            content.SetActive(true);

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

        public void AddLoadingData(AppData.ScreenLoadingInitializationData initializationData)
        {
            if (!loadingQueue.Contains(initializationData))
                loadingQueue.Enqueue(initializationData);
        }

        public void OnLoadingCompleted() => loadingCompleted = true;

        public void SetScreenTextContent(string content, AppData.ScreenTextType textType)
        {
            OnScreenUITextInitialized((textInitializedCallbackResults) =>
            {
                if (AppData.Helpers.IsSuccessCode(textInitializedCallbackResults.resultCode))
                    SetScreenUITextValue(content, textType);
                else
                    Debug.LogWarning($"--> DisplayScreenTextContent's OnScreenUITextInitialized Failed With Results : {textInitializedCallbackResults.result}");

            });
        }

        #endregion

        #region Overrides

        protected override void Init()
        {
            if (content)
                Hide();
            else
                Debug.LogWarning("LoadingSpinnerHandler Initialize Failed : Content Missing / Not Assigned In The Editor Inspector Panel.");
        }

        protected override void RegisterEventListensers(bool register)
        {

        }

        protected override void OnWidgetClosed()
        {
            SetActionButtonState(AppData.InputActionButtonType.Cancel, AppData.InputUIState.Enabled);
        }

        protected override void OnActionButtonClickedEvent(AppData.ButtonDataPackets dataPackets)
        {
            if (dataPackets.action == AppData.InputActionButtonType.Cancel)
            {
                loadingCompleted = true;
                TriggerOnAbortRefreshEvent();
                StopAllCoroutines();
                Hide();
            }
        }

        protected override void OnActionInputFieldValueChangedEvent(string value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionSliderValueChangedEvent(float value, AppData.SliderDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionDropdownValueChangedEvent(string value, AppData.DropdownDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionDropdownValueChangedEvent(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputSliderValueChangedEvent(float value, AppData.InputSliderDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputSliderValueChangedEvent(string value, AppData.InputSliderDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionCheckboxValueChangedEvent(bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Events

        public event EventsListeners _OnRefreshInProgress_AddEventListener;
        public event EventsListeners _OnRefreshCompleted_AddEventListener;
        public event EventsListeners _OnAbortRefresh_AddEventListener;
        public event EventsListeners _OnRefreshFailed_AddEventListener;


        protected void TriggerOnRefreshInProgressEvent() => _OnRefreshInProgress_AddEventListener?.Invoke();
        protected void TriggerOnRefreshCompletedEvent() => _OnRefreshCompleted_AddEventListener?.Invoke();
        protected void TriggerOnAbortRefreshEvent() => _OnAbortRefresh_AddEventListener?.Invoke();
        protected void TriggerOnRefreshFailedEvent() => _OnRefreshFailed_AddEventListener?.Invoke();

        protected override void OnResetWidgetData(AppData.SettingsWidgetType widgetType)
        {
            throw new NotImplementedException();
        }

        protected override void OnWidgetOpened()
        {
        }

        protected override void OnActionDropdownValueChangedEvent(int value, List<string> contentList, AppData.DropdownDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
