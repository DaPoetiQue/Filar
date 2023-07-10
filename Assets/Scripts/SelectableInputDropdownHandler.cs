using System;
using TMPro;

namespace Com.RedicalGames.Filar
{
    public class SelectableInputDropdownHandler : AppData.SelectableUIInputComponent<TMP_Dropdown, AppData.DropdownDataPackets, AppData.UIDropDown<AppData.DropdownDataPackets>>
    {

        #region Components

        #endregion

        #region Main

        public void Init<T>(AppData.UIDropDown<T> input, Action<AppData.Callback> callback = null) where T : AppData.SceneDataPackets
        {
            AppData.Callback callbackResults = new AppData.Callback();
            
            this.input = input as AppData.UIDropDown<AppData.DropdownDataPackets>;

            if(this.input != null)
            {
                Init(this.input);

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

        protected override void OnInputSelected()
        {
            if (input.value)
                input.OnInputPointerDownEvent();
        }

        protected override void OnInputSelected(AppData.UIInputComponent<TMP_Dropdown, AppData.DropdownDataPackets, AppData.UIDropDown<AppData.DropdownDataPackets>> input)
        {

        }

        #endregion

    }
}
