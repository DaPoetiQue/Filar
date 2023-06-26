using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class DynamicContainerUIWidgetPlaceHolder : AppData.UIScreenWidget
    {
        #region Components

        #endregion


        #region Unity Callbacks

        void Start() => Initialization();

        #endregion

        #region Main

        void Initialization()
        {
            // Initialize Assets.
            Init((callback) =>
            {
                if (AppData.Helpers.IsSuccessCode(callback.resultsCode))
                    if (screenManager == null)
                        screenManager = ScreenUIManager.Instance;
                    else
                        LogWarning(callback.results, this, () => Initialization());
                else
                    LogWarning("Couldn't to Initialize Scene Asset UI.", this, () => Initialization());
            });
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonDataPackets> actionButton)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSetUIWidgetData(AppData.Folder folder)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelect(bool isInitialSelection)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDeselect() => Deselected();

        protected override void OnSetFileData(AppData.SceneAsset assetData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenUIRefreshed()
        {

        }

        #endregion
    }
}
