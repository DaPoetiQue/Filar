using System;
using System.Collections.Generic;

namespace Com.RedicalGames.Filar
{
    public class AppTimeEventsManager : AppData.SingletonBaseComponent<AppTimeEventsManager>
    {
        #region Components


        Dictionary<string, AppData.TimedEventComponent> timedEventComponents = new Dictionary<string, AppData.TimedEventComponent>();

        #endregion

        #region Main

        #region Unity Callbacks

        private void Awake() => AppData.ActionEvents.Awake();

        void Start() => AppData.ActionEvents.Start();

        void Update() => AppData.ActionEvents.Update();

        void LateUpdate() => AppData.ActionEvents.LateUpdate();

        void FixedUpdate() => AppData.ActionEvents.FixedUpdate();

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

        public void Cancel(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(timedEventComponents, "Timed Event Components", "There Are No Timed Event Components Initialized."));

            if (callbackResults.Success())
                foreach (var activeEvent in timedEventComponents)
                    activeEvent.Value.Stop();

            callback?.Invoke(callbackResults);
        }

        public void Cancel(string eventName, Action<AppData.Callback> callback = null)
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
