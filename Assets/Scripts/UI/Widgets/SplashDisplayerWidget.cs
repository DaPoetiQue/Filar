using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SplashDisplayerWidget : AppData.Widget
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

                if(callbackResults.Success())
                {
                    switch (GetScreenType().GetData())
                    {
                        case AppData.ScreenType.LoadingScreen:

                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "App Database Manager Instance Is Not Yet Initialized."));

                            if (callbackResults.Success())
                            {
                                var databaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "App Database Manager Instance Is Not Yet Initialized.").GetData();

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name, "App Time Events Manager Instance Is Not Yet Initialized."));

                                if (callbackResults.Success())
                                {
                                    var timeManager = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name, "App Time Events Manager Instance Is Not Yet Initialized.").GetData();
                                    timeManager.RegisterTimedEvent("Randomize Displayed Image", OnRandomizeDisplayedSplashImage, databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.SplashImageChangeEventInterval).value);
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }

                            break;
                    }
                }
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

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetScreenType());

            if (callbackResults.Success())
            {
                switch (GetScreenType().GetData())
                {
                    case AppData.ScreenType.LoadingScreen:

                        callbackResults.SetResult(GetImageInputHandler(AppData.ScreenImageType.Splash));

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name, "App Time Events Manager Instance Is Not Yet Initialized."));

                            if (callbackResults.Success())
                            {
                                var timeManager = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name, "App Time Events Manager Instance Is Not Yet Initialized.").GetData();

                                timeManager.InvokeEvent("Randomize Displayed Image", invokeRandomizeDisplayedImageCallbackResults =>
                                {
                                    callbackResults.SetResult(invokeRandomizeDisplayedImageCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        //var splashImageHandler = GetImageInputHandler(AppData.ScreenImageType.Splash).GetData();
                                        //splashImageHandler.InitializeImageTransitions();

                                        //splashImageHandler.InvokeTransitionableUI(transitionsInvokedCallbackResults =>
                                        //{
                                        //    callbackResults.SetResult(transitionsInvokedCallbackResults);
                                        //});
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        void OnInitializationCompletedEvent()
        {
            LogInfo(" _________________________++++++++++++ Initialization Completed Event Called.", this);
        }

        async void OnRandomizeDisplayedSplashImage()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "App Database Manager Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appDatabaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                var randomImageCallbackResults = appDatabaseManager.GetRandomSplashImage();

                callbackResults.SetResult(randomImageCallbackResults);

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(GetUIImageDisplayer(AppData.ScreenImageType.Splash));

                    if (callbackResults.Success())
                    {
                        var imageDisplayer = GetUIImageDisplayer(AppData.ScreenImageType.Splash).GetData();
                        imageDisplayer.SetImageData(randomImageCallbackResults.GetData(), true);

                        await Task.Yield();
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            LogInfo(" ____________________________++++++++++++++++ OnChange Splash Image Event Called.", this);
        }

        public void OnLoadInProgressEvent()
        {
            LogInfo("____________________________++++++++++++++++_______________________ On Load In Progress... Event", this);
        }

        public void OnLoadCompletedEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name, "App Time Events Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var timeManager = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, AppEventsManager.Instance.name, "App Time Events Manager Instance Is Not Yet Initialized.").GetData();

                timeManager.CancelEvent("Randomize Displayed Image", invokeRandomizeDisplayedImageCallbackResults =>
                {
                    callbackResults.SetResult(invokeRandomizeDisplayedImageCallbackResults);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null) => HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

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
            throw new System.NotImplementedException();
        }

        int GetRandomIndex(int maxIndex = 2) => UnityEngine.Random.Range(0, maxIndex);

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
            throw new NotImplementedException();
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

        #endregion
    }
}