using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class CustomColorSwatchWidget : AppData.SettingsWidget
    {
        #region Components

        #endregion

        #region Main
        protected override void Init()
        {
        }

        protected override void OnActionButtonClickedEvent(AppData.ButtonDataPackets dataPackets)
        {
            switch (dataPackets.action)
            {
                case AppData.InputActionButtonType.CloseButton:

                    CloseWidget(this);

                    break;

                case AppData.InputActionButtonType.ClearButton:

                    Debug.LogError("--> Clear Colors.");

                    break;

                case AppData.InputActionButtonType.ClearAllButton:

                    Debug.LogError("--> Clear All Colors.");

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
            throw new System.NotImplementedException();
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

        #endregion
    }
}
