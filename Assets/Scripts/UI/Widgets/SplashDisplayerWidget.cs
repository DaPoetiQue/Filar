using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SplashDisplayerWidget : AppData.Widget
    {
        #region Components

        AppData.TransitionableUIComponent transitionableUIScaleComponent, transitionableUITranslateComponent;

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
                                        imageDisplayer.SetUIPose(widgetContainer.hiddenScreenPoint.GetWidgetPoseAngle());

                                    if (randomPointIndex <= 0)
                                        imageDisplayer.SetUIPose(widgetContainer.visibleScreenPoint.GetWidgetPoseAngle());

                                    #region Transitionable UI

                                    #region Translation Component

                                    transitionableUITranslateComponent = new AppData.TransitionableUIComponent(imageDisplayer.GetWidgetRect(), AppData.UITransitionType.Translate, AppData.UITransitionStateType.Repeat);
                                    transitionableUITranslateComponent.SetTransitionableUIName(name);
                                    transitionableUITranslateComponent.SetTransitionSpeed(databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetScaleTransitionalSpeed).value);

                                    #endregion

                                    #region Scaling Component

                                    transitionableUIScaleComponent = new AppData.TransitionableUIComponent(imageDisplayer.GetWidgetRect(), AppData.UITransitionType.Scale, AppData.UITransitionStateType.Repeat);
                                    transitionableUIScaleComponent.SetTransitionableUIName(name);
                                    transitionableUIScaleComponent.SetTransitionSpeed(databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetScaleTransitionalSpeed).value);

                                    #endregion

                                    var registerTransitionableUICallbackResults = OnRegisterTransitionableUIComponents(transitionableUITranslateComponent, transitionableUIScaleComponent);

                                    #endregion

                                    callbackResults.SetResult(registerTransitionableUICallbackResults);

                                    if (callbackResults.Success())
                                    {
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
                                var getTransitionableUITranslateTaskResults = GetTransitionableUIComponent(AppData.UITransitionType.Translate);
                                var getTransitionableUIScaleTaskResults = GetTransitionableUIComponent(AppData.UITransitionType.Scale);

                                if (getTransitionableUITranslateTaskResults.Success() && getTransitionableUIScaleTaskResults.Success())
                                {
                                    var translationComponent = getTransitionableUITranslateTaskResults.data;
                                    var scalingComponent = getTransitionableUIScaleTaskResults.data;

                                    //if (splashImageURLCallbackResults.Success())
                                    //    SetUIImageDisplayerValue(AppData.ScreenImageType.Splash, splashImageURLCallbackResults.data);
                                    //else
                                    //    Log(splashImageURLCallbackResults.ResultCode, splashImageURLCallbackResults.Result, this);

                                    var imageDisplayer = imageDisplayerCallbackResults.data;
                                    var randomPointIndex = GetRandomIndex();

                                    Debug.LogError(" _____________ Shit Not Showing Up.");

                                    if (randomPointIndex >= 1)
                                    {
                                        imageDisplayer.SetUIPose(widgetContainer.hiddenScreenPoint.GetWidgetPoseAngle());

                                        translationComponent.SetTarget(widgetContainer.hiddenScreenPoint.GetWidgetPosition());
                                        scalingComponent.SetTarget(widgetContainer.hiddenScreenPoint.GetWidgetScale());
                                    }

                                    if (randomPointIndex <= 0)
                                    {
                                        imageDisplayer.SetUIPose(widgetContainer.visibleScreenPoint.GetWidgetPoseAngle());

                                        translationComponent.SetTarget(widgetContainer.visibleScreenPoint.GetWidgetPosition());
                                        scalingComponent.SetTarget(widgetContainer.visibleScreenPoint.GetWidgetScale());
                                    }

                                    //LogInfo($" _____________++++++++++ On Transition Called - Index : {randomPointIndex} - Source : {componentData.GetSource()} Source Origin : {componentData.GetSourceOrigin()} - Target : {componentData.GetTarget()} - Target Origin : {componentData.GetTargetOrigin()}.", this);

                                }
                                else
                                {
                                    Log(getTransitionableUITranslateTaskResults.GetResultCode, getTransitionableUITranslateTaskResults.GetResult, this);
                                    Log(getTransitionableUIScaleTaskResults.GetResultCode, getTransitionableUIScaleTaskResults.GetResult, this);
                                }
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

            var getTransitionableUITranslateTaskResults = GetTransitionableUIComponent(AppData.UITransitionType.Translate);
            var getTransitionableUIScaleTaskResults = GetTransitionableUIComponent(AppData.UITransitionType.Scale);

            //LogInfo($" _____________++++++++++ Show Widget - Code : {getTransitionableUITaskResults.GetResultCode} - Results : {getTransitionableUITaskResults.GetResult}", this);

            if (getTransitionableUITranslateTaskResults.Success() && getTransitionableUIScaleTaskResults.Success())
            {
                var translationComponent = getTransitionableUITranslateTaskResults.data;
                var scalingComponent = getTransitionableUIScaleTaskResults.data;

                var translationTaskResults = await translationComponent.InvokeTransitionAsync();
                var scalingTaskResults = await translationComponent.InvokeTransitionAsync();

                if (translationTaskResults.Success() && scalingTaskResults.Success())
                    ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
                else
                {
                    Log(translationTaskResults.GetResultCode, translationTaskResults.GetResult, this);
                    Log(scalingTaskResults.GetResultCode, scalingTaskResults.GetResult, this);
                }
            }
            else
            {
                Log(getTransitionableUITranslateTaskResults.GetResultCode, getTransitionableUITranslateTaskResults.GetResult, this);
                Log(getTransitionableUIScaleTaskResults.GetResultCode, getTransitionableUIScaleTaskResults.GetResult, this);
            }
        }

        protected override void OnHideScreenWidget()
        {
            var getTransitionableUITranslateTaskResults = GetTransitionableUIComponent(AppData.UITransitionType.Translate);
            var getTransitionableUIScaleTaskResults = GetTransitionableUIComponent(AppData.UITransitionType.Scale);

            //LogInfo($" _____________++++++++++ Show Widget - Code : {getTransitionableUITaskResults.GetResultCode} - Results : {getTransitionableUITaskResults.GetResult}", this);

            if (getTransitionableUITranslateTaskResults.Success() && getTransitionableUIScaleTaskResults.Success())
            {
                var translationComponent = getTransitionableUITranslateTaskResults.data;
                var scalingComponent = getTransitionableUIScaleTaskResults.data;

                translationComponent.CancelTransition(translationCancelledCallbackResults =>
                {
                    if (translationCancelledCallbackResults.Success())
                    {
                        scalingComponent.CancelTransition(scalingCancelledCallbackResults =>
                        {
                            if (scalingCancelledCallbackResults.Success())
                                HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
                        });
                    }
                }); 
            }
            else
            {
                Log(getTransitionableUITranslateTaskResults.GetResultCode, getTransitionableUITranslateTaskResults.GetResult, this);
                Log(getTransitionableUIScaleTaskResults.GetResultCode, getTransitionableUIScaleTaskResults.GetResult, this);
            }
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