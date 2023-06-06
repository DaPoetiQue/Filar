using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class PagerNavigationWidget : AppData.Widget
    {
        #region Components

        #endregion


        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            pagerNavigationWidget = this;
            base.Init();
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.SceneDataPackets dataPackets)
        {
            if (string.IsNullOrEmpty(value))
                return;

            int goToPage = 0;

            int.TryParse(value, out goToPage);

            if (goToPage == 0)
                goToPage = 1;

            int pageNumber = goToPage - 1;

            SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.Pagination_GoToPage(pageNumber);
        }

        protected override void OnInputFieldValueChanged(int value, AppData.SceneDataPackets dataPackets)
        {

        }

        protected override void OnScreenWidget()
        {

        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
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