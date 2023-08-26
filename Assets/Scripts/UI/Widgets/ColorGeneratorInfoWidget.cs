using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ColorGeneratorInfoWidget : AppData.SettingsWidget
    {
        #region Components

        [SerializeField]
        ColorPalletWidgetUIHandler colorPalletWidget = null;

        #endregion

        #region Main

        #endregion
        protected override void Init()
        {
            if (colorPalletWidget == null)
            {
                if (GetComponentInParent<ColorPalletWidgetUIHandler>())
                    colorPalletWidget = GetComponentInParent<ColorPalletWidgetUIHandler>();
            }
        }

        protected override void OnActionButtonClickedEvent(AppData.ButtonDataPackets dataPackets)
        {
            switch (dataPackets.action)
            {
                case AppData.InputActionButtonType.RetryButton:


                    CloseWidget(this);

                    break;

                case AppData.InputActionButtonType.CloseButton:

                    parentWidget.HideChildWidget(AppData.SettingsWidgetType.ColorSettingsImportWidget);
                    CloseWidget(this);

                    break;

                case AppData.InputActionButtonType.ConfirmationButton:

                    if (colorPalletWidget != null)
                    {
                        colorPalletWidget.SelectGeneratedColorSwatch();
                        parentWidget.HideChildWidget(AppData.SettingsWidgetType.ColorSettingsImportWidget);
                        CloseWidget(this);
                    }
                    else
                        Debug.LogWarning("--> OnActionButtonClickedEvent Failed - colorPalletWidget  Is Not Assigned In The Editor Inspector Panel.");

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
    }
}
