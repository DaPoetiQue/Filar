using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class LoadingScreenWidget : AppData.Widget
    {
        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            loadingScreenWidget = this;
            base.Init();
        }

        protected override void OnScreenWidget()
        {

        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {

        }

        protected override void OnHideScreenWidget()
        {

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
