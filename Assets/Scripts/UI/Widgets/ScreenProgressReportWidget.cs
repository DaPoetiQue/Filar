using UnityEngine;
using System;

namespace Com.RedicalGames.Filar
{
    public class ScreenProgressReportWidget : AppData.Widget
    {
        #region Components

        private int progressIntValue = 0;

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override void Configure(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", $"Configure {GetName()} Failed - App Events Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if(callbackResults.Success())
            {
                var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                appEventsManagerInstance.OnEventSubscription(ProgressUpdateEvent, AppData.EventType.OnLateUpdate, true, progressUpdateEventCallbackResults => { callbackResults.SetResult(progressUpdateEventCallbackResults); });
                appEventsManagerInstance.OnEventSubscription(PercentageIntValueProgressReportEvent, AppData.EventType.OnProgressPercentageIntValue, true, eventSubscriptionCallbackResults => { callbackResults.SetResult(eventSubscriptionCallbackResults); });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        private void PercentageIntValueProgressReportEvent(int percentageIntValue)
        {
            var callbackResults = new AppData.Callback();

            SetProgressIntValue(percentageIntValue, progressIntValueSetCallbackResults => 
            {
                callbackResults.SetResult(progressIntValueSetCallbackResults);

                if(callbackResults.UnSuccessful())
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            });
        }

        private void ProgressUpdateEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "Progress Update Event Failed - App Database Manager Instance Is Not Initialized Yet - Invalid Operation."));

            if (callbackResults.Success())
            {
                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                callbackResults.SetResult(GetUIImageDisplayer(AppData.ScreenImageType.ProgressBar));

                if (callbackResults.Success())
                {
                    #region Progress Bar

                    callbackResults.SetResult(GetUIImageDisplayer(AppData.ScreenImageType.ProgressBar).GetData().GetValue());

                    if (callbackResults.Success())
                    {
                        var progressBar = GetUIImageDisplayer(AppData.ScreenImageType.ProgressBar).GetData().GetValue().GetData();

                        callbackResults.SetResult(GetUITextDisplayer(AppData.ScreenTextType.ProgressPercentageDisplayer));

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(GetUITextDisplayer(AppData.ScreenTextType.ProgressPercentageDisplayer).GetData().GetValue());

                            if (callbackResults.Success())
                            {
                                var percentageDisplayer = GetUITextDisplayer(AppData.ScreenTextType.ProgressPercentageDisplayer).GetData().GetValue().GetData();

                                if (GetProgressBarValue().GetData() > 0)
                                {
                                    var fillAmountValue = Mathf.Lerp(progressBar.fillAmount, GetProgressBarValue().GetData(), appDatabaseManagerInstance.GetDefaultExecutionValue(AppData.RuntimeExecution.ProgressReportTransitionalSpeed).value * Time.smoothDeltaTime);
                                    progressBar.fillAmount = fillAmountValue;

                                    #region Progress Text

                                    var progressText = $"{(int)Math.Round(fillAmountValue * 100, 0)}%";

                                    percentageDisplayer.GetTextComponent().GetData().SetText(progressText);

                                    #endregion

                                    callbackResults.result = "Progress Has Been Successfully Reset.";
                                }
                                else
                                {
                                    var progressText = $"{GetProgressBarValue().GetData()}%";

                                    progressBar.fillAmount = GetProgressBarValue().GetData();
                                    percentageDisplayer.GetTextComponent().GetData().SetText(progressText);
                                    callbackResults.result = "Progress Has Been Successfully Reset.";
                                }
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                    #endregion
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void SetProgressIntValue(int progressIntValue, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            this.progressIntValue = progressIntValue;
            callbackResults.result = $"Set Progress Int Value Success - Progress Int Value Is Set To : {progressIntValue}.";
            callbackResults.resultCode = AppData.Helpers.SuccessCode;

            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackData<float> GetProgressBarValue()
        {
            var callbackResults = new AppData.CallbackData<float>();

            callbackResults.result = $"Get Progress Int Value Success - Progress Int Value Is Set To : {progressIntValue}.";
            callbackResults.data = (float)progressIntValue/100;
            callbackResults.resultCode = AppData.Helpers.SuccessCode;

            return callbackResults;
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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

        protected override void OnScreenWidget(Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {

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

        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {

        }

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {

        }

        protected override void OnScreenWidgetShownEvent()
        {

        }

        protected override void OnScreenWidgetHiddenEvent()
        {

        }

        protected override void OnScreenWidgetTransitionInProgressEvent()
        {

        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {

        }

        #endregion
    }
}
