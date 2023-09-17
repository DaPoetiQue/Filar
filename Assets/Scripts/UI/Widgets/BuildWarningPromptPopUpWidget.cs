using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class BuildWarningPromptPopUpWidget : AppData.Widget
    {
        #region Components

        RectTransform screenRect;

        #endregion

        #region Main

        protected override void Initialize()
        {
            buildWarningPromptWidget = this;

            AppData.WidgetLayoutView layoutView = widgetLayouts.Find(layout => layout.layoutViewType == defaultLayoutType);

            if (layoutView.layout)
            {
                if (layoutView.layout.GetComponent<RectTransform>())
                    screenRect = layoutView.layout.GetComponent<RectTransform>();
                else
                    Debug.LogWarning("Init : Value Doesn't Have A Rect Transform Component.");
            }
            else
                Debug.LogWarning("--> Failed - Layout Value Missing / Not Assigned In The Inspector.");
        }

        protected override void OnScreenWidget()
        {

        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}