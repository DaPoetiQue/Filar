using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class DynamicScreenContainer : AppData.DynamicContainerBase
    {
        #region Components

        [Space(5)]
        [SerializeField]
        AppData.SelectableWidgetType selectableWidgetType;

        [Space(5)]
        [SerializeField]
        AppData.OrientationType orientation;

        protected override void OnClear(bool showSpinner = false, Action<AppData.Callback> callback = null)
        {
            LogInfo($" _______________+++++++ Not Yet Implemented Function - On Clear Container Called - For Dynamic Container : {GetName()} - Of Type : {GetContainerType().GetData()}", this);
        }

        protected override Task<AppData.Callback> OnClearAsync(bool showSpinner = false)
        {
            LogInfo($" _______________+++++++ Not Yet Implemented Function - On Clear Container Async Called - For Dynamic Container : {GetName()} - Of Type : {GetContainerType().GetData()}", this);
            return null;
        }

        protected override void OnContainerUpdate()
        {
            LogInfo($" _______________+++++++ Not Yet Implemented Function - On Container Update Called - For Dynamic Container : {GetName()} - Of Type : {GetContainerType().GetData()}", this);
        }

        protected override Task<AppData.Callback> OnContainerUpdateAsync()
        {
            LogInfo($" _______________+++++++ Not Yet Implemented Function - On Container Update Async Called - For Dynamic Container : {GetName()} - Of Type : {GetContainerType().GetData()}", this);
            return null;
        }

        protected override void OnInitialization()
        {
            LogInfo($" _______________+++++++ Not Yet Implemented Function - On Container Initialization Called - For Dynamic Container : {GetName()} - Of Type : {GetContainerType().GetData()}", this);
        }

        protected override void OnUpdatedContainerSize(Action<AppData.CallbackData<Vector2>> callback = null)
        {
            LogInfo($" _______________+++++++ Not Yet Implemented Function - Updating Container Size For Dynamic Container : {GetName()} - Of Type : {GetContainerType().GetData()}", this);
        }

        protected override Task<AppData.CallbackData<Vector2>> OnUpdatedContainerSizeAsync()
        {
            LogInfo($" _______________+++++++ Not Yet Implemented Function -Updating Container Size Async For Dynamic Container : {GetName()} - Of Type : {GetContainerType().GetData()}", this);
            return null;
        }

        protected override void UpdateContentOrderInLayer(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetContainerType());

            if (callbackResults.Success())
            {
                switch (GetContainerType().GetData())
                {
                    case AppData.ContentContainerType.AppScreenContainer:

                        var screens = AppData.Helpers.GetList(GetComponentsInChildren<Screen>());

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(screens, "Screens", $"There Were No Screen Components Found In Container : {GetName()} - Of Type : {GetContainerType().GetData()} - Invalid Operation, Please Check Here."));

                        if(callbackResults.Success())
                        {
                            callbackResults.SetResult(GetOrderInLayerType());

                            if(callbackResults.Success())
                            {
                                screens.Sort((screenA, screenB) => (GetOrderInLayerType().GetData() == AppData.OrderInLayerType.Ascending)? screenA.GetOrderInLayer().CompareTo(screenB.GetOrderInLayer()) : screenB.GetOrderInLayer().CompareTo(screenA.GetOrderInLayer()));

                                for (int i = 0; i < screens.Count; i++)
                                    screens[i].SetOrderInLayer(i);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;

                    case AppData.ContentContainerType.ScreenWidgetContainer:

                        var widgets = AppData.Helpers.GetList(GetComponentsInChildren<AppData.Widget>());

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(widgets, "Widgets", $"There Were No Widget Components Found In Container : {GetName()} - Of Type : {GetContainerType().GetData()} - Invalid Operation, Please Check Here."));

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(GetOrderInLayerType());

                            if (callbackResults.Success())
                            {
                                widgets.Sort((widgetA, widgetB) => (GetOrderInLayerType().GetData() == AppData.OrderInLayerType.Ascending) ? widgetA.GetOrderInLayer().CompareTo(widgetB.GetOrderInLayer()) : widgetB.GetOrderInLayer().CompareTo(widgetA.GetOrderInLayer()));

                                for (int i = 0; i < widgets.Count; i++)
                                    widgets[i].SetOrderInLayer(i);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        #endregion
    }
}