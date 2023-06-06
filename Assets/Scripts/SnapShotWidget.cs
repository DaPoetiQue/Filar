using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SnapShotWidget : AppData.Widget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        AppData.UIImageDisplayer<AppData.ImageDataPackets> imageDisplayer = new AppData.UIImageDisplayer<AppData.ImageDataPackets>();

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            snapShotWidget = this;
            base.Init();
        }

        protected override void OnScreenWidget()
        {

        }

        void SetWidgetAssetData(AppData.SceneAsset asset)
        {
            if (titleDisplayer != null && !string.IsNullOrEmpty(asset.name))
                titleDisplayer.text = asset.name;
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            AppData.ImageData captureData = ScreenCaptureManager.Instance.GetScreenCaptureData();
            imageDisplayer.SetImageData(captureData, imageDisplayer.dataPackets);

            if (SceneAssetsManager.Instance)
                SetWidgetAssetData(SceneAssetsManager.Instance.GetCurrentSceneAsset());
            else
                Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
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
