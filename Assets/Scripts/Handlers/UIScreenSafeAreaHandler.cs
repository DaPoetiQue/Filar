using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenSafeAreaHandler : AppMonoBaseClass
    {
        #region Components

        private RectTransform screenSafeArea = null;

        #endregion

        #region Main

        public void Configure(AppData.ScreenResolution screenResolution, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "Init Failed - App Events Manager Instance Is not Yet Initialized - Invalid Operation."));

            if (callbackResults.Success())
            {
                var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                appEventsManagerInstance.OnEventSubscription<AppData.ScreenResolution>(OnScreenResolutionChangedEvent, AppData.EventType.OnScreenResolutionChanged, true, resolutionChangedEventCallbackResults => 
                {
                    callbackResults.SetResult(resolutionChangedEventCallbackResults);

                    if(callbackResults.Success())
                    {
                        ChangeScreenResolution(screenResolution, resolutionSetCallbackResults =>
                        {
                            callbackResults.SetResult(resolutionSetCallbackResults);

                            if (callbackResults.UnSuccessful())
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        });
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        private void OnScreenResolutionChangedEvent(AppData.ScreenResolution screenResolution)
        {
            var callbackResults = new AppData.Callback();

            ChangeScreenResolution(screenResolution, resolutionChangedCallbackResults => 
            {
                callbackResults.SetResult(resolutionChangedCallbackResults);

                if(callbackResults.UnSuccessful())
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            });
        }

        public AppData.CallbackData<RectTransform> GetScreenSafeArea()
        {
            var callbackResults = new AppData.CallbackData<RectTransform>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(screenSafeArea, "Screen Safe Area", "Get Screen Safe Area - Findnging Screen Safe Area."));

            if(callbackResults.Success())
            {
                callbackResults.result = $"Get Screen Safe Area Success - Screen Safe Area Assigned.";
                callbackResults.data = screenSafeArea;
            }
            else
            {
                screenSafeArea = GetComponent<RectTransform>();

                callbackResults.result = $"Get Screen Safe Area Success - Screen Safe Area Found.";
                callbackResults.data = screenSafeArea;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }

            return callbackResults;
        }

        private void ChangeScreenResolution(AppData.ScreenResolution screenResolution, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetScreenSafeArea());

            if(callbackResults.Success())
            {
                callbackResults.SetResult(screenResolution.GetResolution());

                if (callbackResults.Success())
                {
                    GetScreenSafeArea().GetData().SetWidgetScale(screenResolution.GetResolution().GetData());
                    Canvas.ForceUpdateCanvases();
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion
    }
}
