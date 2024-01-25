using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class NetworkNotificationWidget : AppData.Widget
    {
        #region Components

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

        protected override void OnActionButtonEvent(AppData.WidgetType screenWidgetType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            AppData.Callback callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var screenUIManager = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                switch (actionType)
                {
                    case AppData.InputActionButtonType.RetryButton:

                        screenUIManager.GetCurrentScreen(async currentScreenCallbackResults =>
                        {
                            callbackResults.SetResult(currentScreenCallbackResults);

                            var screen = currentScreenCallbackResults.GetData();

                            screen.HideScreenWidget(GetType().GetData(), callback: async widgetHiddenCallbackResults => 
                            {
                                callbackResults.SetResult(widgetHiddenCallbackResults);

                                if(callbackResults.Success())
                                {
                                    await Task.Delay(1000);

                                    screen.ShowWidget(AppData.WidgetType.LoadingWidget, async widgetShownCallbackResults =>
                                    {
                                        callbackResults.SetResult(widgetShownCallbackResults);

                                        if (callbackResults.Success())
                                        {
                                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance", "Loading Manager Instance Is Not Yet Initialized."));

                                            if (callbackResults.Success())
                                            {
                                                var loadingManager = AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance").GetData();

                                                if (loadingManager.OnInitialLoad)
                                                {
                                                    await loadingManager.GetLoadingSequence().ProcessQueueSequenceAsync();
                                                }
                                                else
                                                    LogWarning("Please Do Network Retry Here.", this);
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    });
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            });
                        });

                        break;
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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
