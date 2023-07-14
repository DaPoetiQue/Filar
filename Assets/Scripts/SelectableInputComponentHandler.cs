using System;
using TMPro;

namespace Com.RedicalGames.Filar
{
    public class SelectableInputComponentHandler : AppData.SelectableUIInputComponent<TMP_Dropdown, AppData.DropdownDataPackets, AppData.UIDropDown<AppData.DropdownDataPackets>>
    {

        #region Components

        #endregion

        #region Main

        public void Init(AppData.UISelectable selectable, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();
            
            this.selectable = selectable;

            if(this.selectable != null)
            {
                base.Init(this.selectable);

                callbackResults.results = "Input Initialized";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Input Missing.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        protected override void OnInputSelected() => selectable.OnInputPointerDownEvent();

        protected override void OnInputSelected(AppData.UIInputComponent<TMP_Dropdown, AppData.DropdownDataPackets, AppData.UIDropDown<AppData.DropdownDataPackets>> input)
        {

        }

        #endregion

    }
}
