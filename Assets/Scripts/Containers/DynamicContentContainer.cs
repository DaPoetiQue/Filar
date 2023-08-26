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

        #endregion
    }
}