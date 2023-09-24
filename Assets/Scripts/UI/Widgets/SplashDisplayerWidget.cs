using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SplashDisplayerWidget : AppData.Widget
    {
        #region Components

        AppData.TransitionableUIComponent transitionableUIScaleComponent, transitionableUITranslateComponent;

        bool canShowScreen = false;

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

                                    //if (randomPointIndex >= 1)
                                    //    imageDisplayer.SetUIPose(widgetContainer.hiddenScreenPoint.GetWidgetPoseAngle());

                                    //if (randomPointIndex <= 0)
                                    //    imageDisplayer.SetUIPose(widgetContainer.visibleScreenPoint.GetWidgetPoseAngle());

                                    #region Transitionable UI

                                    #region Translation Component

                                    transitionableUITranslateComponent = new AppData.TransitionableUIComponent(imageDisplayer.GetWidgetRect(), AppData.UITransitionType.Translate, AppData.UITransitionStateType.Repeat);
                                    transitionableUITranslateComponent.SetTransitionableUIName(name + "_Translate");
                                    transitionableUITranslateComponent.SetTransitionSpeed(databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetScaleTransitionalSpeed).value);

                                    #endregion

                                    #region Scaling Component

                                    transitionableUIScaleComponent = new AppData.TransitionableUIComponent(imageDisplayer.GetWidgetRect(), AppData.UITransitionType.Scale, AppData.UITransitionStateType.Repeat);
                                    transitionableUIScaleComponent.SetTransitionableUIName(name + "_Scale");
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
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name,"App Database Manager Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appDatabaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                callbackResults.SetResult(appDatabaseManager.GetRandomSplashImage());

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
                            {
                                SetTransitionableUITarget(AppData.UITransitionType.Translate, widgetContainer.hiddenScreenPoint.GetWidgetPosition(), targetSetCallbackResults => 
                                {
                                    callbackResults.SetResult(targetSetCallbackResults);

                                    if(callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });

                                imageDisplayer.SetUIPose(widgetContainer.visibleScreenPoint.GetWidgetPoseAngle());
                            }

                            if (randomPointIndex <= 0)
                            {
                                SetTransitionableUITarget(AppData.UITransitionType.Translate, widgetContainer.visibleScreenPoint.GetWidgetPosition(), targetSetCallbackResults =>
                                {
                                    callbackResults.SetResult(targetSetCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });

                                imageDisplayer.SetUIPose(widgetContainer.hiddenScreenPoint.GetWidgetPoseAngle());
                            }
                        }
                        else
                            Log(imageDisplayerCallbackResults.GetResultCode, imageDisplayerCallbackResults.GetResult, this);
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            InvokeTransitionableUI(AppData.UITransitionType.Translate);
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnHideScreenWidget()
        {
            CancelInvokedTransitionableUI(callback: transitionCancelledCallbackResults => 
            {
                if (transitionCancelledCallbackResults.Success())
                    HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
                else
                    Log(transitionCancelledCallbackResults.GetResultCode, transitionCancelledCallbackResults.GetResult, this);
            });
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