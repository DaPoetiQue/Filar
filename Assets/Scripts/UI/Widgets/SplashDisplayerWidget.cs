using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SplashDisplayerWidget : AppData.Widget
    {

        #region Components

        [SerializeField]
        private AppData.TransitionableUIComponent transitionableSplashImageComponent;

        private List<int> randomGeneratedIndexList = new List<int>();

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>();

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

                                    //  databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.SplashImageChangeEventInterval).value
                                    timeManager.RegisterTimedEvent("Randomize Displayed Image", OnRandomizeDisplayedSplashImage, 5.0f);

                                    callbackResults.SetResult(GetImageInputHandler(AppData.ScreenImageType.Splash));

                                    if (callbackResults.Success())
                                    {
                                        var splashImageHandler = GetImageInputHandler(AppData.ScreenImageType.Splash).GetData();

                                        callbackResults.SetResult(splashImageHandler.GetImageComponent());

                                        if (callbackResults.Success())
                                            transitionableSplashImageComponent = new AppData.TransitionableUIComponent(splashImageHandler.GetImageComponent().GetData().GetWidgetRect(), AppData.UITransitionType.Default, AppData.UITransitionStateType.Once, databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.SplashImageTransitionSpeed).value);
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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

        private void OnTriggerTransitions(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetImageInputHandler(AppData.ScreenImageType.Splash));

            if (callbackResults.Success())
            {
                var splashImageHandler = GetImageInputHandler(AppData.ScreenImageType.Splash).GetData();

                callbackResults.SetResult(splashImageHandler.GetTransitionableUIMounts());

                if(callbackResults.Success())
                {
                    var mounts = splashImageHandler.GetTransitionableUIMounts().GetData();

                    callbackResults.SetResult(GetTransitionableSplashImageComponent());

                    if (callbackResults.Success())
                    {
                        var transitionableUI = GetTransitionableSplashImageComponent().GetData();

                        callbackResults.SetResult(transitionableUI.Initialized());

                        if (callbackResults.Success())
                        {
                            var mount = mounts[1];

                            transitionableUI.InvokeTransition(mount, AppData.UITransitionType.Default, transitionInvokedCallbackResults =>
                            {
                                callbackResults.SetResult(transitionInvokedCallbackResults);
                            });
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
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
         
        }

        void OnRandomizeDisplayedSplashImage()
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
                        callbackResults.SetResult(GetTransitionableSplashImageComponent());

                        if (callbackResults.Success())
                        {
                            var transitionableUI = GetTransitionableSplashImageComponent().GetData();

                            callbackResults.SetResult(transitionableUI.Initialized());

                            if (callbackResults.Success())
                            {
                                callbackResults.SetResult(GetImageInputHandler(AppData.ScreenImageType.Splash));

                                if (callbackResults.Success())
                                {
                                    var splashImageHandler = GetImageInputHandler(AppData.ScreenImageType.Splash).GetData();

                                    callbackResults.SetResult(splashImageHandler.GetTransitionableUIMounts());

                                    if (callbackResults.Success())
                                    {
                                        var mounts = splashImageHandler.GetTransitionableUIMounts().GetData();

                                        transitionableUI.SetTransitionDestination(mounts[GetRandomIndexValue(mounts.Count)]);

                                        var imageDisplayer = GetUIImageDisplayer(AppData.ScreenImageType.Splash).GetData();
                                        imageDisplayer.SetImageData(randomImageCallbackResults.GetData(), true);
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            LogInfo(" ____________________________++++++++++++++++ OnChange Splash Image Event Called.", this);
        }

        #region Events

        public void OnNetworkErrorEvent()
        {
            OnLoadCompletedEvent();
        }

        public void OnLoadInProgressEvent()
        {
            var callbackResults = new AppData.Callback(GetScreenType());

            if (callbackResults.Success())
            {
                switch (GetScreenType().GetData())
                {
                    case AppData.ScreenType.LoadingScreen:

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "App Time Events Manager Instance Is Not Yet Initialized."));

                        if (callbackResults.Success())
                        {
                            var timeManager = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                            timeManager.InvokeEvent("Randomize Displayed Image", invokeRandomizeDisplayedImageCallbackResults =>
                            {
                                callbackResults.SetResult(invokeRandomizeDisplayedImageCallbackResults);

                                if (callbackResults.Success())
                                {
                                    OnTriggerTransitions(transitionTriggeredCallbackResults =>
                                    {
                                        callbackResults.SetResult(transitionTriggeredCallbackResults);
                                    });
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            });
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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

        #endregion

        private AppData.CallbackData<AppData.TransitionableUIComponent> GetTransitionableSplashImageComponent()
        {
            var callbackResults = new AppData.CallbackData<AppData.TransitionableUIComponent>(AppData.Helpers.GetAppComponentValid(transitionableSplashImageComponent, "Transitionable Splash Image Component", $"Get Transitionable Splash Image Component Failed - Transitionable Splash Image Component Is Not Initialized For : {GetName()} - Of Type {GetType().GetData()}."));

            if(callbackResults.Success())
            {
                callbackResults.SetResult(transitionableSplashImageComponent.Initialized());

                if(callbackResults.Success())
                {
                    callbackResults.result = $"Get Transitionable Splash Image Component Success - Transitionable Splash Image Component For : {GetName()} - Of Type : {GetType().GetData()} - Have Been Initialized.";
                    callbackResults.data = transitionableSplashImageComponent;
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        private int GetRandomIndexValue(int count)
        {
            if (randomGeneratedIndexList.Count >= count - 1)
                randomGeneratedIndexList.Clear();

            int randomIndex = AppData.Helpers.GetRandomValue(count);

            while (randomGeneratedIndexList.Contains(randomIndex))
                randomIndex = AppData.Helpers.GetRandomValue(count);

            if (!randomGeneratedIndexList.Contains(randomIndex))
            {
                randomGeneratedIndexList.Add(randomIndex);
                return randomIndex;
            }

            return randomIndex;
        }

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {

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
            throw new System.NotImplementedException();
        }

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

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}