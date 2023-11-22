using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SplashDisplayerWidget : AppData.Widget
    {
        #region Components

        [SerializeField]
        private Texture2D testImage;

        AppData.TransitionableUIComponent transitionableUIScaleComponent, transitionableUITranslateComponent;
        AppData.TimedEventComponent changeSplashImageTimedEventComponent;

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>();

            //var initializationProgressCompletionEvent = new AppData.EventActionData(name, AppData.EventType.OnInitializationCompletedEvent, OnInitializationCompletedEvent);

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);

            });

            callback.Invoke(callbackResults);
        }

        void Delete()
        {
            //AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, databaseManagerCallbackResults =>
            //{
            //    callbackResults.SetResult(databaseManagerCallbackResults);

            //    if (callbackResults.Success())
            //    {
            //        var databaseManager = databaseManagerCallbackResults.data;

            //        OnRegisterWidget(this, onRegisteredWidgetCallbackResults =>
            //        {
            //            callbackResults.SetResult(onRegisteredWidgetCallbackResults);

            //            if (callbackResults.Success())
            //            {
            //                InitializeInputs(inputInitializationCallbackResults =>
            //                {
            //                    callbackResults.SetResult(inputInitializationCallbackResults);

            //                    if (callbackResults.Success())
            //                    {
            //                        var initializationProgressCompletionEvent = new AppData.EventActionData(name, AppData.EventType.OnInitializationCompletedEvent, OnInitializationCompletedEvent);

            //                        RegisterEventAction(actionEventRegisteredCallbackResults =>
            //                        {
            //                            callbackResults.SetResult(actionEventRegisteredCallbackResults);

            //                            if (callbackResults.Success())
            //                            {
            //                                GetUIImageDisplayerValue(AppData.ScreenImageType.Splash, imageDisplayerCallbackResults =>
            //                                {
            //                                    callbackResults.SetResult(imageDisplayerCallbackResults);

            //                                    if (callbackResults.Success())
            //                                    {
            //                                        var imageDisplayer = imageDisplayerCallbackResults.data;

            //                                        var randomPointIndex = GetRandomIndex();

            //                                        #region Transitionable UI

            //                                        #region Translation Component

            //                                        transitionableUITranslateComponent = new AppData.TransitionableUIComponent(imageDisplayer.GetWidgetRect(), AppData.UITransitionType.Translate, AppData.UITransitionStateType.Repeat);
            //                                        transitionableUITranslateComponent.SetTransitionableUIName(name + "_Translate");
            //                                        transitionableUITranslateComponent.SetTransitionSpeed(databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetTranslateTransitionalSpeed).value);

            //                                        #endregion

            //                                        #region Scaling Component

            //                                        transitionableUIScaleComponent = new AppData.TransitionableUIComponent(imageDisplayer.GetWidgetRect(), AppData.UITransitionType.Scale, AppData.UITransitionStateType.Repeat);
            //                                        transitionableUIScaleComponent.SetTransitionableUIName(name + "_Scale");
            //                                        transitionableUIScaleComponent.SetTransitionSpeed(databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetScaleTransitionalSpeed).value);

            //                                        #endregion

            //                                        var registerTransitionableUICallbackResults = OnRegisterTransitionableUIComponents(transitionableUITranslateComponent, transitionableUIScaleComponent);

            //                                        #endregion

            //                                        #region Timed Events

            //                                        changeSplashImageTimedEventComponent = new AppData.TimedEventComponent(name, databaseManager.GetDefaultExecutionValue(AppData.RuntimeExecution.SplashImageChangeEventInterval).value, OnRandomizeDisplayedSplashImage);

            //                                        var registerTimedEventsCallbackResults = OnRegisterTimedEventComponents(changeSplashImageTimedEventComponent);

            //                                        #endregion

            //                                        callbackResults.SetResult(registerTransitionableUICallbackResults);

            //                                        if (callbackResults.Success())
            //                                        {
            //                                            callbackResults.SetResult(registerTimedEventsCallbackResults);

            //                                            if (callbackResults.Success())
            //                                            {
            //                                                InitializeDisplayer(displayerInitializedCallbackResults =>
            //                                                {
            //                                                    callbackResults.SetResult(displayerInitializedCallbackResults);

            //                                                    if (callbackResults.Success())
            //                                                    {
            //                                                        callbackResults.SetResult(GetType());

            //                                                        if (callbackResults.Success())
            //                                                        {
            //                                                            var widgetStatePacket = new AppData.WidgetStatePacket(name: GetName(), type: GetType().data, stateType: AppData.WidgetStateType.Initialized, value: this);

            //                                                            callbackResults.result = $"Widget : {GetName()} Of Type : {GetType().data}'s State Packet Has Been Initialized Successfully.";
            //                                                            callbackResults.data = widgetStatePacket;
            //                                                        }
            //                                                        else
            //                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //                                                    }
            //                                                    else
            //                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //                                                });
            //                                            }
            //                                            else
            //                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //                                        }
            //                                        else
            //                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //                                    }
            //                                    else
            //                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //                                });
            //                            }

            //                        }, initializationProgressCompletionEvent);
            //                    }
            //                });
            //            }
            //            else
            //                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //        });
            //    }
            //    else
            //        Log(databaseManagerCallbackResults.resultCode, databaseManagerCallbackResults.result, this);

            //}, "App Database Manager Instance Is Not Yet Initialized.");
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

        void InitializeDisplayer(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "App Database Manager Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appDatabaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                callbackResults.SetResult(appDatabaseManager.GetRandomSplashImage());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(GetUIImageDisplayer(AppData.ScreenImageType.Splash));

                    if (callbackResults.Success())
                    {
                        var imageDisplayer = GetUIImageDisplayer(AppData.ScreenImageType.Splash).GetData();
                        var image = appDatabaseManager.GetRandomSplashImage().GetData();
                        imageDisplayer.SetImageData(image, true);

                        var randomPointIndex = GetRandomIndex();

                        if (randomPointIndex >= 1)
                        {
                            SetTransitionableUITarget(widgetContainer.hiddenScreenPoint.GetWidgetPoseAngle(), targetSetCallbackResults =>
                            {
                                callbackResults.SetResult(targetSetCallbackResults);

                                if (callbackResults.Success())
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

                        //InvokeTimedEvents(timeEventInvokedCallbackResults =>
                        //{
                        //    callbackResults.SetResult(timeEventInvokedCallbackResults);

                        //    if (callbackResults.Success())
                        //    {

                        //    }
                        //});
                    }
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

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket)
        {
            //if (callbackResults.Success())
            //{
            //    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "App Database Manager Is Not Yet Initialized."));

            //    if (callbackResults.Success())
            //    {
            //        var appDatabaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

            //        callbackResults.SetResult(appDatabaseManager.GetRandomSplashImage());

            //        if (callbackResults.Success())
            //        {

            //            var image = appDatabaseManager.GetRandomSplashImage().GetData();
            //            SetUIImageDisplayer(AppData.ScreenImageType.Splash, image, true, imageSetCallbackResults => { });
            //        }
            //    }
            //}
            //else
            //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        protected override void OnShowScreenWidget(Action<AppData.Callback> callback = null)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        void OnInitializationCompletedEvent()
        {
            LogInfo(" _________________________++++++++++++ Initialization Completed Event Called.", this);
        }

        void OnRandomizeDisplayedSplashImage()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "App Database Manager Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appDatabaseManager = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).data;

                callbackResults.SetResult(appDatabaseManager.GetRandomSplashImage());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(GetUIImageDisplayer(AppData.ScreenImageType.Splash));

                    if (callbackResults.Success())
                    {
                        var imageDisplayer = GetUIImageDisplayer(AppData.ScreenImageType.Splash).GetData();
                        var image = appDatabaseManager.GetRandomSplashImage().GetData();
                        imageDisplayer.SetImageData(image, true);
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            LogInfo(" ____________________________++++++++++++++++ OnChange Splash Image Event Called.", this);
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

        #endregion
    }
}