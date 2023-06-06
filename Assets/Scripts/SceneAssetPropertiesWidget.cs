using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SceneAssetPropertiesWidget : AppData.Widget
    {
        #region Components

        [SerializeField]
        Vector2 showWidgetidgetScreenPoint;

        [Space(5)]
        [SerializeField]
        Vector2 hideWidgetidgetScreenPoint;

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main
        new void Init()
        {
            assetPropertiesWidget = this;
            base.Init();
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

        protected override void OnInputFieldValueChanged(string value, AppData.SceneDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.SceneDataPackets dataPackets)
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

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.SceneDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
