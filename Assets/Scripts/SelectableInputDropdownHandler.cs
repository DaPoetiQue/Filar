using TMPro;

namespace Com.RedicalGames.Filar
{
    public class SelectableInputDropdownHandler : AppData.SelectableUIInputComponent<TMP_Dropdown, AppData.DropdownDataPackets, AppData.UIDropDown<AppData.DropdownDataPackets>>
    {

        #region Components

        #endregion

        #region Main

        public void Init<T>(AppData.UIDropDown<T> input) where T : AppData.DataPackets
        {
            this.input = input as AppData.UIDropDown<AppData.DropdownDataPackets>;
            base.Init(this.input);
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
