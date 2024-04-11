using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class AppEventsManager : AppData.SingletonBaseComponent<AppEventsManager>
    {
        #region Components

        [Tooltip("Alert! - Do No Assign This Value, This Is Assigned In Runtime - Note : Any Settings Applied Will Be Overriden In Runtime.")]
        [Space(5)]
        [SerializeField]
        private AppData.EventType currentEvent = AppData.EventType.None;

        Dictionary<string, AppData.TimedEventComponent> timedEventComponents = new Dictionary<string, AppData.TimedEventComponent>();

        #endregion

        #region Unity Callbacks

        private void Awake() => AppData.ActionEvents.Awake();

        void Update() => AppData.ActionEvents.Update();

        void LateUpdate() => AppData.ActionEvents.LateUpdate();

        void FixedUpdate() => AppData.ActionEvents.FixedUpdate();

        #endregion

        #region Main

        #region Initializations

        protected override void Init()
        {
            var callbackResults = new AppData.Callback();

            OnEventSubscription(OnGlobalEventSubscriptions, true, subscribedToEventCallbackResults => 
            {
                callbackResults.SetResult(subscribedToEventCallbackResults); 

                if(callbackResults.UnSuccessful())
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            });
        }

        private void OnGlobalEventSubscriptions(AppData.EventType currentEvent)
        {
            var callbackResults = new AppData.Callback();

            SetCurrentEventType(currentEvent, globalEventSubscriptionsCallbackResults => 
            {
                callbackResults.SetResult(globalEventSubscriptionsCallbackResults);

                if (callbackResults.UnSuccessful())
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            });
        }

        private void SetCurrentEventType(AppData.EventType currentEvent, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValueValid(currentEvent, "Current Event", $"Set Current Event Failed - Current Event Parameter Value Is Set To Default : {currentEvent} - Invalid Operation."));

            if (callbackResults.Success())
            {
                this.currentEvent = currentEvent;
                callbackResults.result = $"Set Current Event Success - Current Event Parameter Value Is Set To : {currentEvent}.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackData<AppData.EventType> GetCurrentEvent()
        {
            var callbackResults = new AppData.CallbackData<AppData.EventType>(AppData.Helpers.GetAppEnumValueValid(currentEvent, "Current Event", $"Get Current Event Failed - Current Event Value Is Set To Default : {currentEvent} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Current Event Success - Current Event Value Is Set To : {currentEvent}.";
                callbackResults.data = currentEvent;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

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
                        case AppData.EventType.OnAwake:

                            if (subscribe)
                                AppData.ActionEvents._OnAwake += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnAwake -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnStart:

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

                        case AppData.EventType.OnInitializationStarted:

                            if (subscribe)
                                AppData.ActionEvents._OnInitializationStartedEvent += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnInitializationStartedEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnInitializationCompleted:

                            if (subscribe)
                                AppData.ActionEvents._OnInitializationCompletedEvent += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnInitializationCompletedEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnDownloadStarted:

                            if (subscribe)
                                AppData.ActionEvents._OnDownloadStartedEvent += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnDownloadStartedEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnDownloadCompleted:

                            if (subscribe)
                                AppData.ActionEvents._OnDownloadCompletedEvent += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnDownloadCompletedEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnAppLanguageChanged:

                            if (subscribe)
                                AppData.ActionEvents._OnAppLanguageChanged += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnAppLanguageChanged -= eventMethod.Invoke;

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

        public void OnEventSubscription(Action<AppData.EventType> eventMethod, bool subscribe = true, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(eventMethod, "Event Method", "On Event Subscription Failed - Event Menthod Parameter Value Is Not Assigned."));

            if (callbackResults.Success())
            {
                if (subscribe)
                {
                    AppData.ActionEvents._OnStart += () => eventMethod?.Invoke(AppData.EventType.OnStart);
                    AppData.ActionEvents._OnInitializationStartedEvent += () => eventMethod?.Invoke(AppData.EventType.OnInitializationStarted);
                    AppData.ActionEvents._OnInitializationCompletedEvent += () => eventMethod?.Invoke(AppData.EventType.OnInitializationCompleted);
                    AppData.ActionEvents._OnDownloadStartedEvent += () => eventMethod?.Invoke(AppData.EventType.OnDownloadStarted);
                    AppData.ActionEvents._OnDownloadCompletedEvent += () => eventMethod?.Invoke(AppData.EventType.OnDownloadCompleted);
                    AppData.ActionEvents._OnScreenChangedEvent += (value) => eventMethod?.Invoke(AppData.EventType.OnScreenChangedEvent);
                    AppData.ActionEvents._OnScreenRefreshed += (value) => eventMethod?.Invoke(AppData.EventType.OnScreenRefreshed);
                    AppData.ActionEvents._OnActionButtonClickedEvent += (value) => eventMethod?.Invoke(AppData.EventType.OnActionButtonClicked);
                    AppData.ActionEvents._OnNetworkConnectedEvent += () => eventMethod?.Invoke(AppData.EventType.OnNetworkConnectedEvent);
                    AppData.ActionEvents._OnNetworkFailedEvent += () => eventMethod?.Invoke(AppData.EventType.OnNetworkFailedEvent);
                    AppData.ActionEvents._OnAppLanguageChanged += () => eventMethod?.Invoke(AppData.EventType.OnAppLanguageChanged);
                }
                else
                {
                    AppData.ActionEvents._OnStart -= () => eventMethod?.Invoke(AppData.EventType.OnStart);
                    AppData.ActionEvents._OnInitializationStartedEvent -= () => eventMethod?.Invoke(AppData.EventType.OnInitializationStarted);
                    AppData.ActionEvents._OnInitializationCompletedEvent -= () => eventMethod?.Invoke(AppData.EventType.OnInitializationCompleted);
                    AppData.ActionEvents._OnDownloadStartedEvent -= () => eventMethod?.Invoke(AppData.EventType.OnDownloadStarted);
                    AppData.ActionEvents._OnDownloadCompletedEvent -= () => eventMethod?.Invoke(AppData.EventType.OnDownloadCompleted);
                    AppData.ActionEvents._OnScreenChangedEvent -= (value) => eventMethod?.Invoke(AppData.EventType.OnScreenChangedEvent);
                    AppData.ActionEvents._OnScreenRefreshed -= (value) => eventMethod?.Invoke(AppData.EventType.OnScreenRefreshed);
                    AppData.ActionEvents._OnActionButtonClickedEvent -= (value) => eventMethod?.Invoke(AppData.EventType.OnActionButtonClicked);
                    AppData.ActionEvents._OnNetworkConnectedEvent -= () => eventMethod?.Invoke(AppData.EventType.OnNetworkConnectedEvent);
                    AppData.ActionEvents._OnNetworkFailedEvent -= () => eventMethod?.Invoke(AppData.EventType.OnNetworkFailedEvent);
                    AppData.ActionEvents._OnAppLanguageChanged -= () => eventMethod?.Invoke(AppData.EventType.OnAppLanguageChanged);
                }
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

                        case AppData.EventType.OnTabViewShownEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnTabViewShownEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnTabViewShownEvent -= eventMethod.Invoke;

                            break;

                        case AppData.EventType.OnTabViewHiddenEvent:

                            if (subscribe)
                                AppData.GenericActionEvents<T>._OnTabViewHiddenEvent += eventMethod.Invoke;
                            else
                                AppData.GenericActionEvents<T>._OnTabViewHiddenEvent -= eventMethod.Invoke;

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

        public void OnEventSubscription(Action<AppData.TabViewType> eventMethod, AppData.EventType eventType, bool subscribe = true, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(eventMethod, "Event Method", "On Event Subscription Failed - Event Menthod Parameter Value Is Not Assigned."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppEnumValueValid(eventType, "Timed Event Type", $"On Event Subscription Failed - Typed Event Parameter Value Is Set To Default : {eventType}"));

                if (callbackResults.Success())
                {
                    switch (eventType)
                    {
                        case AppData.EventType.OnShowTabViewEvent:

                            if (subscribe)
                                AppData.ActionEvents._OnShowTabViewEvent += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnShowTabViewEvent -= eventMethod.Invoke;

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

        public void OnEventSubscription(Action<AppData.TabViewType, Action<AppData.Callback>> eventMethod, AppData.EventType eventType, bool subscribe = true, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(eventMethod, "Event Method", "On Event Subscription Failed - Event Menthod Parameter Value Is Not Assigned."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppEnumValueValid(eventType, "Timed Event Type", $"On Event Subscription Failed - Typed Event Parameter Value Is Set To Default : {eventType}"));

                if (callbackResults.Success())
                {
                    switch (eventType)
                    {
                        case AppData.EventType.OnShowTabViewAsyncEvent:

                            if (subscribe)
                                AppData.ActionEvents._OnShowTabViewAsyncEvent += eventMethod.Invoke;
                            else
                                AppData.ActionEvents._OnShowTabViewAsyncEvent -= eventMethod.Invoke;

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

        public void InvokeEvent(AppData.EventType eventType, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValueValid(eventType, "Event type", $"Invoke Event Failed - Event Type Parameter Value Is set To Default : {eventType} - Invalid Operation."));

            if (callbackResults.Success())
            {
                switch(eventType)
                {
                    case AppData.EventType.OnStart:

                        AppData.ActionEvents.Start();

                        break;

                    case AppData.EventType.OnInitializationStarted:

                        AppData.ActionEvents.OnInitializationStartedEvent();

                        break;

                    case AppData.EventType.OnInitializationCompleted:

                        AppData.ActionEvents.OnInitializationCompletedEvent();

                        break;

                    case AppData.EventType.OnDownloadStarted:

                        AppData.ActionEvents.OnDownloadStartedEvent();

                        break;

                    case AppData.EventType.OnDownloadCompleted:

                        AppData.ActionEvents.OnDownloadCompletedEvent();

                        break;

                    case AppData.EventType.OnAppLanguageChanged:

                        AppData.ActionEvents.OnAppLanguageChangedEvent();

                        break;
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
