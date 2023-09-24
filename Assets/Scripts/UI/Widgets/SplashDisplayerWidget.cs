using System;
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

                                    #region Transitionable UI

                                    #region Translation Component

                                    transitionableUITranslateComponent = new AppData.TransitionableUIComponent(imageDisplayer.GetWidgetRect(), AppData.UITransitionType.Translate, AppData.UITransitionStateType.Repeat);
                                    transitionableUITranslateComponent.SetTransitionableUIName(name + "_Translate");
                                    transitionableUITranslateComponent.SetTransitionSpeed(databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetTranslateTransitionalSpeed).value);

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
                            var image = appDatabaseManager.GetRandomSplashImage().GetData();
                            imageDisplayer.SetImageData(image, true);

                            var randomPointIndex = GetRandomIndex();

                            if (randomPointIndex >= 1)
                            {
                                SetTransitionableUITarget(widgetContainer.hiddenScreenPoint.GetWidgetPoseAngle(), targetSetCallbackResults => 
                                {
                                    callbackResults.SetResult(targetSetCallbackResults);

                                    if(callbackResults.Success())
                                        imageDisplayer.SetUIPose(widgetContainer.visibleScreenPoint.GetWidgetPoseAngle());
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }

                            if (randomPointIndex <= 0)
                            {
                                SetTransitionableUITarget(widgetContainer.visibleScreenPoint.GetWidgetPoseAngle(), targetSetCallbackResults =>
                                {
                                    callbackResults.SetResult(targetSetCallbackResults);

                                    if (callbackResults.Success())
                                        imageDisplayer.SetUIPose(widgetContainer.hiddenScreenPoint.GetWidgetPoseAngle());
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
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
            InvokeTransitionableUI();
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnHideScreenWidget() => HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

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