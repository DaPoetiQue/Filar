using System;
using TMPro;

namespace Com.RedicalGames.Filar
{
    public class SelectableInputComponentHandler : AppData.SelectableUIInputComponent<TMP_Dropdown, AppData.DropdownConfigDataPacket, AppData.UIDropDown<AppData.DropdownConfigDataPacket>>
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

                callbackResults.result = "Input Initialized";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Input Missing.";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        protected override void OnInputSelected() => selectable.OnInputPointerDownEvent();

        protected override void OnInputSelected(AppData.UIInputComponent<TMP_Dropdown, AppData.DropdownConfigDataPacket, AppData.UIDropDown<AppData.DropdownConfigDataPacket>> input)
        {

        }

        #endregion

    }
}
