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
            throw new NotImplementedException();
        }

        protected override Task<AppData.Callback> OnClearAsync(bool showSpinner = false)
        {
            throw new NotImplementedException();
        }

        protected override void OnContainerUpdate()
        {
            throw new NotImplementedException();
        }

        protected override Task<AppData.Callback> OnContainerUpdateAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnInitialization()
        {
            throw new NotImplementedException();
        }

        protected override void OnUpdatedContainerSize(Action<AppData.CallbackData<Vector2>> callback = null)
        {
            throw new NotImplementedException();
        }

        protected override Task<AppData.CallbackData<Vector2>> OnUpdatedContainerSizeAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}