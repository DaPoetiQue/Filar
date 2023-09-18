using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SplashDisplayerWidget : AppData.Widget
    {
        #region Components

        AppData.TransitionableUIComponent transitionableUIComponent;

        #endregion

        #region Main

        protected override void Initialize(Action<AppData.CallbackData<AppData.WidgetStatePacket>> callback)
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>();

            AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, databaseManagerCallbackResults =>
            {
                callbackResults.SetResult(databaseManagerCallbackResults);

                if (callbackResults.Success())
                {
                    var databaseManager = databaseManagerCallbackResults.data;

                    OnRegisterWidget(this, onRegisteredWidgetCallbackResults =>
                    {
                        callbackResults.SetResult(onRegisteredWidgetCallbackResults);

                        if (callbackResults.Success())
                        {
                            GetUIImageDisplayerValue(AppData.ScreenImageType.Splash, imageDisplayerCallbackResults =>
                            {
                                callbackResults.SetResult(imageDisplayerCallbackResults);

                                if (callbackResults.Success())
                                {
                                    var imageDisplayer = imageDisplayerCallbackResults.data;

                                    var randomPointIndex = GetRandomIndex();

                                    if (randomPointIndex >= 1)
                                        imageDisplayer.SetUIScale(widgetContainer.visibleScreenPoint.GetWidgetScale());

                                    if (randomPointIndex <= 0)
                                        imageDisplayer.SetUIScale(widgetContainer.hiddenScreenPoint.GetWidgetScale());

                                    transitionableUIComponent = new AppData.TransitionableUIComponent(imageDisplayer.GetWidgetRect(), AppData.UITransitionType.Scale, AppData.UITransitionStateType.Repeat);
                                    transitionableUIComponent.SetTransitionableUIName(name);
                                    transitionableUIComponent.SetTransitionSpeed(databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetScaleTransitionalSpeed).value);

                                    callbackResults.SetResult(GetType());

                                    if (callbackResults.Success())
                                    {
                                        var widgetStatePacket = new AppData.WidgetStatePacket(name: GetName(), type: GetType().data, stateType: AppData.WidgetStateType.Initialized, value: this);

                                        callbackResults.result = $"Widget : {GetName()} Of Type : {GetType().data}'s State Packet Has Been Initialized Successfully.";
                                        callbackResults.data = widgetStatePacket;
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
                }
                else
                    Log(databaseManagerCallbackResults.resultCode, databaseManagerCallbackResults.result, this);

            }, "App Database Manager Instance Is Not Yet Initialized.");

            callback.Invoke(callbackResults);
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
            LogInfo($" _____________++++++++++ On Screen Widget - Starts Here : .", this);

            if (widgetType == AppData.WidgetType.ImageDisplayerWidget)
            {
                LogInfo($" _____________++++++++++ On Screen Widget - Starts Here.", this);

                AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, async appDatabaseManagerCallbackResults =>
                {
                    if (appDatabaseManagerCallbackResults.Success())
                    {
                        var appDatabaseManager = appDatabaseManagerCallbackResults.data;

                        var splashImageURLCallbackResults = await appDatabaseManager.GetRandomSplashImage();

                        GetUIImageDisplayerValue(AppData.ScreenImageType.Splash, async imageDisplayerCallbackResults =>
                        {
                            if (imageDisplayerCallbackResults.Success())
                            {
                                var getTransitionableUITaskResults = await GetTransitionableUIComponent();

                                if (getTransitionableUITaskResults.Success())
                                {
                                    var componentData = getTransitionableUITaskResults.data;

                                    //if (splashImageURLCallbackResults.Success())
                                    //    SetUIImageDisplayerValue(AppData.ScreenImageType.Splash, splashImageURLCallbackResults.data);
                                    //else
                                    //    Log(splashImageURLCallbackResults.ResultCode, splashImageURLCallbackResults.Result, this);

                                    var imageDisplayer = imageDisplayerCallbackResults.data;
                                    var randomPointIndex = GetRandomIndex();

                                    Debug.LogError(" _____________ Shit Not Showing Up.");

                                    if (randomPointIndex >= 1)
                                    {
                                        imageDisplayer.SetUIScale(widgetContainer.visibleScreenPoint.GetWidgetScale());
                                        componentData.SetTarget(widgetContainer.hiddenScreenPoint.GetWidgetScale());
                                    }

                                    if (randomPointIndex <= 0)
                                    {
                                        imageDisplayer.SetUIScale(widgetContainer.hiddenScreenPoint.GetWidgetScale());
                                        componentData.SetTarget(widgetContainer.visibleScreenPoint.GetWidgetScale());
                                    }

                                    LogInfo($" _____________++++++++++ On Transition Called - Index : {randomPointIndex} - Source : {componentData.GetSource()} Source Origin : {componentData.GetSourceOrigin()} - Target : {componentData.GetTarget()} - Target Origin : {componentData.GetTargetOrigin()}.", this);

                                }
                                else
                                    Log(getTransitionableUITaskResults.GetResultCode, getTransitionableUITaskResults.GetResult, this);
                            }
                            else
                                Log(imageDisplayerCallbackResults.GetResultCode, imageDisplayerCallbackResults.GetResult, this);
                        });
                    }
                    else
                        Log(appDatabaseManagerCallbackResults.GetResultCode, appDatabaseManagerCallbackResults.GetResult, this);
                });
            }
        }

        protected override async void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            Debug.LogError(" _____________ Shit Not Showing Up Too.");

            LogInfo($" _____________++++++++++ Show Widget - Starts Here.", this);

            var getTransitionableUITaskResults = await GetTransitionableUIComponent();

            LogInfo($" _____________++++++++++ Show Widget - Code : {getTransitionableUITaskResults.GetResultCode} - Results : {getTransitionableUITaskResults.GetResult}", this);

            if (getTransitionableUITaskResults.Success())
            {
                var componentData = getTransitionableUITaskResults.data;
                var transitionTaskResults = await componentData.InvokeTransitionAsync();

                if (transitionTaskResults.Success())
                    ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
                else
                    Log(transitionTaskResults.GetResultCode, transitionTaskResults.GetResult, this);
            }
            else
                Log(getTransitionableUITaskResults.GetResultCode, getTransitionableUITaskResults.GetResult, this);
        }

        protected override async void OnHideScreenWidget()
        {
            var getTransitionableUITaskResults = await GetTransitionableUIComponent();

            if (getTransitionableUITaskResults.Success())
            {
                var componentData = getTransitionableUITaskResults.data;

                HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
                componentData.CancelTransition();
            }
            else
                Log(getTransitionableUITaskResults.GetResultCode, getTransitionableUITaskResults.GetResult, this);
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

        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        int GetRandomIndex(int maxIndex = 2) => UnityEngine.Random.Range(0, maxIndex);

        private async Task<AppData.CallbackData<AppData.TransitionableUIComponent>> GetTransitionableUIComponent()
        {
            AppData.CallbackData<AppData.TransitionableUIComponent> callbackResults = new AppData.CallbackData<AppData.TransitionableUIComponent>();

            var initializationTaskResults = await transitionableUIComponent.Initialized();

            callbackResults.SetResult(initializationTaskResults);

            if (callbackResults.Success())
                callbackResults.SetData(transitionableUIComponent);
            else
                callbackResults.SetResults($"TransitionableUIComponent Is Not Yet Initialized.", AppData.Helpers.ErrorCode);

            return callbackResults;
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