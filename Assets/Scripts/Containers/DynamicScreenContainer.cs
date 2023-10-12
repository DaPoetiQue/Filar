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

        #endregion
    }
}