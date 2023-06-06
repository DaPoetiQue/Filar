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

        protected override void OnActionButtonClickedEvent(AppData.ButtonDataPackets dataPackets)
        {
            switch (dataPackets.actionType)
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

        protected override void OnActionCheckboxValueChangedEvent(bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChangedEvent(string value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChangedEvent(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChangedEvent(int value, List<string> contentList, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionInputFieldValueChangedEvent(string value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionSliderValueChangedEvent(float value, AppData.SliderDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputSliderValueChangedEvent(float value, AppData.InputSliderDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputSliderValueChangedEvent(string value, AppData.InputSliderDataPackets dataPackets)
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
