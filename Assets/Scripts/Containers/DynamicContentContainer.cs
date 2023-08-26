using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class DynamicContentContainer : AppData.DynamicContainer
    {
        #region Components

        #endregion

        #region Main

        #region Overrides

        protected override void OnInitialization()
        {

        }

        protected override void OnClear(bool showSpinner = false, Action<AppData.Callback> callback = null)
        {
          
        }

        protected override Task<AppData.Callback> OnClearAsync(bool showSpinner = false)
        {
            return null;
        }

        protected override void OnContainerUpdate()
        {
            
        }

        protected override Task<AppData.Callback> OnContainerUpdateAsync()
        {
            return null;
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

        #endregion
    }
}