using System.Collections.Generic;

namespace Com.RedicalGames.Filar
{
    public class SettingsScreenWarningInfoWidget : AppData.SettingsWidget
    {
        #region Components

        #endregion

        #region Main

        #endregion
        protected override void Init()
        {

        }

        protected override void OnActionButtonClickedEvent(AppData.ButtonConfigDataPacket dataPackets)
        {
            switch (dataPackets.GetAction().GetData())
            {
                case AppData.InputActionButtonType.RetryButton:

                    parentWidget.ResetChildWidget(AppData.SettingsWidgetType.ColorSettingsImportWidget);

                    break;

                case AppData.InputActionButtonType.Cancel:

                    parentWidget.HideChildWidget(AppData.SettingsWidgetType.ColorSettingsImportWidget);
                    CloseWidget(this);

                    break;
            }
        }

        protected override void OnActionCheckboxValueChangedEvent(bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChangedEvent(string value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChangedEvent(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChangedEvent(int value, List<string> contentList, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionInputFieldValueChangedEvent(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionSliderValueChangedEvent(float value, AppData.SliderConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputSliderValueChangedEvent(float value, AppData.InputSliderConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputSliderValueChangedEvent(string value, AppData.InputSliderConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnResetWidgetData(AppData.SettingsWidgetType widgetType)
        {

        }

        protected override void OnWidgetClosed()
        {

        }

        protected override void OnWidgetOpened()
        {

        }

        protected override void RegisterEventListensers(bool register)
        {

        }
    }
}
