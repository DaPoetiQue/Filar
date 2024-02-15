using System;
using System.Collections.Generic;

namespace Com.RedicalGames.Filar
{
    public class AppEventsManager : AppData.SingletonBaseComponent<AppEventsManager>
    {
        #region Components

        Dictionary<string, AppData.TimedEventComponent> timedEventComponents = new Dictionary<string, AppData.TimedEventComponent>();

        #endregion

        #region Unity Callbacks

        private void Awake() => AppData.ActionEvents.Awake();

        void Update() => AppData.ActionEvents.Update();

        void LateUpdate() => AppData.ActionEvents.LateUpdate();

        void FixedUpdate() => AppData.ActionEvents.FixedUpdate();

        #endregion

        #region Main

        protected override void Init() => AppData.ActionEvents.Start();

        #region Subscriptions

        public void OnEventSubscription(Action eventMethod, AppData.EventType eventType, bool subscribe = true, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(eventMethod, "Event Method", "On Event Subscription Failed - Event Menthod Parameter Value Is Not Assigned."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppEnumValueValid(eventType, "Timed Event Type", $"On Event Subscription Failed - Typed Event Parameter Value Is Set To Default : {eventType}"));

                if (callbackResults.Success())
                {
                    switch (eventType)
                    {
                        case AppData.EventType.OnAppAwake:

                            if (subscribe)
                                AppData.ActionEvents._OnAwake += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnAwake -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnAppStart:

                            if (subscribe)
                                AppData.ActionEvents._OnStart += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnStart -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnUpdate:

                            if (subscribe)
                                AppData.ActionEvents._OnUpdate += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnUpdate -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnLateUpdate:

                            if (subscribe)
                                AppData.ActionEvents._OnLateUpdate += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnLateUpdate -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnFixedUpdate:

                            if (subscribe)
                                AppData.ActionEvents._OnFixedUpdate += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnFixedUpdate -= eventMethod.Invoke;

                            break;
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void OnEventSubscription<T>(Action<T> eventMethod, AppData.EventType eventType, bool subscribe = true, Action<AppData.Callback> callback = null) where T : class
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(eventMethod, "Event Method", "On Event Subscription Failed - Event Menthod Parameter Value Is Not Assigned."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppEnumValueValid(eventType, "Timed Event Type", $"On Event Subscription Failed - Typed Event Parameter Value Is Set To Default : {eventType}"));

                if (callbackResults.Success())
                {
                    switch (eventType)
                    {
                        case AppData.EventType.OnScreenShownEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnScreenShownEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnScreenShownEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnScreenHiddenEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnScreenHiddenEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnScreenHiddenEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnScreenTransitionInProgressEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnScreenTransitionInProgressEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnScreenTransitionInProgressEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnWidgetShownEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnWidgetShownEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnWidgetShownEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnWidgetHiddenEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnWidgetHiddenEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnWidgetHiddenEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnWidgetTransitionInProgressEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnWidgetTransitionInProgressEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnWidgetTransitionInProgressEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnSelectableWidgetShownEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnSelectableWidgetShownEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnSelectableWidgetShownEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnSelectableWidgetHiddenEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnSelectableWidgetHiddenEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnSelectableWidgetHiddenEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnSelectableWidgetTransitionInProgressEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnSelectableWidgetTransitionInProgressEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnSelectableWidgetTransitionInProgressEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnPostSelectedEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnPostSelectedEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnPostSelectedEvent -= eventMethod.Invoke;

                            break;
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void OnEventSubscription<T>(AppData.EventActionComponent<T> eventActionsComponent, bool subscribe = true, Action<AppData.Callback> callback = null) where T : AppMonoBaseClass
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(eventActionsComponent, "Event Actions Component", "On Event Subscription Failed - Event Actions Component Parameter Value Is Invalid / Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(eventActionsComponent.GetInitializedGroupType());

                if(callbackResults.Success())
                {
                    switch(eventActionsComponent.GetInitializedGroupType().GetData())
                    {
                        case AppData.EventActionInitializedGroupType.All:

                            callbackResults.SetResult(eventActionsComponent.GetRegisteredEventActions());

                            if (callbackResults.Success())
                            {
                                var subscibedEventActions = eventActionsComponent.GetRegisteredEventActions().GetData();

                                for (int i = 0; i < subscibedEventActions.Count; i++)
                                {
                                    callbackResults.SetResult(subscibedEventActions[i].GetEventType());

                                    if (callbackResults.Success())
                                    {
                                        callbackResults.SetResult(subscibedEventActions[i].GetEventMethod());

                                        if (callbackResults.Success())
                                        {
                                            AppData.ActionEvents.OnEventActionSubscription(subscibedEventActions[i], subscribe, subscriptionCallbackResults =>
                                            {
                                                callbackResults.SetResult(subscriptionCallbackResults);
                                            });

                                            if (callbackResults.UnSuccessful())
                                            {
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        break;
                                    }
                                }
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                            if(callbackResults.Success())
                            {
                                callbackResults.SetResult(eventActionsComponent.GetRegisteredParameterEventActions());

                                if (callbackResults.Success())
                                {
                                    var subscibedParameterEventActions = eventActionsComponent.GetRegisteredParameterEventActions().GetData();

                                    for (int i = 0; i < subscibedParameterEventActions.Count; i++)
                                    {
                                        callbackResults.SetResult(subscibedParameterEventActions[i].GetEventType());

                                        if (callbackResults.Success())
                                        {
                                            callbackResults.SetResult(subscibedParameterEventActions[i].GetEventMethod());

                                            if (callbackResults.Success())
                                            {
                                                AppData.ActionEvents.OnEventActionSubscription(subscibedParameterEventActions[i], subscribe, subscriptionCallbackResults =>
                                                {
                                                    callbackResults.SetResult(subscriptionCallbackResults);
                                                });

                                                if (callbackResults.UnSuccessful())
                                                {
                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                            break;

                        case AppData.EventActionInitializedGroupType.EventActions:

                            callbackResults.SetResult(eventActionsComponent.GetRegisteredEventActions());

                            if (callbackResults.Success())
                            {
                                var subscibedEventActions = eventActionsComponent.GetRegisteredEventActions().GetData();

                                for (int i = 0; i < subscibedEventActions.Count; i++)
                                {
                                    callbackResults.SetResult(subscibedEventActions[i].GetEventType());

                                    if (callbackResults.Success())
                                    {
                                        callbackResults.SetResult(subscibedEventActions[i].GetEventMethod());

                                        if (callbackResults.Success())
                                        {
                                            AppData.ActionEvents.OnEventActionSubscription(subscibedEventActions[i], subscribe, subscriptionCallbackResults =>
                                            {
                                                callbackResults.SetResult(subscriptionCallbackResults);
                                            });

                                            if (callbackResults.UnSuccessful())
                                            {
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        break;
                                    }
                                }
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                            break;

                        case AppData.EventActionInitializedGroupType.ParameterEventActions:

                            callbackResults.SetResult(eventActionsComponent.GetRegisteredParameterEventActions());

                            if (callbackResults.Success())
                            {
                                var subscibedParameterEventActions = eventActionsComponent.GetRegisteredParameterEventActions().GetData();

                                for (int i = 0; i < subscibedParameterEventActions.Count; i++)
                                {
                                    callbackResults.SetResult(subscibedParameterEventActions[i].GetEventType());

                                    if (callbackResults.Success())
                                    {
                                        callbackResults.SetResult(subscibedParameterEventActions[i].GetEventMethod());

                                        if (callbackResults.Success())
                                        {
                                            AppData.ActionEvents.OnEventActionSubscription(subscibedParameterEventActions[i], subscribe, subscriptionCallbackResults =>
                                            {
                                                callbackResults.SetResult(subscriptionCallbackResults);
                                            });

                                            if (callbackResults.UnSuccessful())
                                            {
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        break;
                                    }
                                }
                            }

                            break;
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Timed Events

        public void RegisterTimedEvent(string eventName, Action eventAction, float intervals, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            if(!timedEventComponents.ContainsKey(eventName))
            {
                AppData.TimedEventComponent timedEvent = new AppData.TimedEventComponent(eventName, intervals, eventAction);
                timedEventComponents.Add(eventName, timedEvent);

                callbackResults.result = $"Timed Event : {eventAction.Method.Name} Has Been Successfully Added To Timed Event Components.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Timed Events Already Contains Event : {eventAction.Method.Name}";
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void InvokeEvent(string eventName, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(timedEventComponents, "Timed Event Components", "There Are No Timed Event Components Initialized."));

            if(callbackResults.Success())
            {
                if(timedEventComponents.TryGetValue(eventName, out AppData.TimedEventComponent timedEvent))
                {
                    timedEvent.Start();

                    callbackResults.result = $"Timed Event : {eventName} Has Been Invoked Successfully.";
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.result = $"Event Named  : {eventName} Not Found In Registered Timed Events. Make Sure This Event Is Registered Before Invoking.";
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }

            callback?.Invoke(callbackResults);
        }


        public void InvokeEvents(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(timedEventComponents, "Timed Event Components", "There Are No Timed Event Components Initialized."));

            if (callbackResults.Success())
                foreach (var activeEvent in timedEventComponents)
                    activeEvent.Value.Start();

            callback?.Invoke(callbackResults);
        }

        public void CancelEvents(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(timedEventComponents, "Timed Event Components", "There Are No Timed Event Components Initialized."));

            if (callbackResults.Success())
                foreach (var activeEvent in timedEventComponents)
                    activeEvent.Value.Stop();

            callback?.Invoke(callbackResults);
        }

        public void CancelEvent(string eventName, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(timedEventComponents, "Timed Event Components", "There Are No Timed Event Components Initialized."));

            if (callbackResults.Success())
            {
                if (timedEventComponents.TryGetValue(eventName, out AppData.TimedEventComponent timedEvent))
                {
                    timedEvent.Stop();

                    callbackResults.result = $"Timed Event : {eventName} Has Been Invoked Successfully.";
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.result = $"Event Named  : {eventName} Not Found In Registered Timed Events. Make Sure This Event Is Registered Before Invoking.";
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }

            callback?.Invoke(callbackResults);
        }

        #endregion

        #endregion
    }
}
