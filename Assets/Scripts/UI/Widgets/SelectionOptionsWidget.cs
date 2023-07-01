using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SelectionOptionsWidget : AppData.Widget
    {
        #region Components

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            selectionOptionsWidget = this;
            base.Init();
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

        protected override void OnScreenWidget()
        {
            if (SceneAssetsManager.Instance != null)
            {
                SetCheckboxValue(SceneAssetsManager.Instance.GetFolderStructureData().InverseSelect(), AppData.CheckboxInputActionType.InverseSelection);

                var widgetsContainer = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer;

                if (widgetsContainer != null)
                {
                    string title = (widgetsContainer.GetPaginationViewType() == AppData.PaginationViewType.Pager) ? "Select Page" : "Select View";
                    bool enableSelectionButton = false;

                    if (widgetsContainer.GetPaginationViewType() == AppData.PaginationViewType.Pager)
                        enableSelectionButton = widgetsContainer.Pagination_GetPageCount() > 1;

                    if (widgetsContainer.GetPaginationViewType() == AppData.PaginationViewType.Scroller)
                    {
                        int itemsPerView = (widgetsContainer.GetLayout().viewType == AppData.LayoutViewType.ItemView) ? widgetsContainer.GetPaginationComponent().itemView_ItemsPerPage : widgetsContainer.GetPaginationComponent().listView_ItemsPerPage;
                        enableSelectionButton = widgetsContainer.GetContentCount() > itemsPerView;
                    }

                    widgetsContainer.WidgetsInCurrentPageSelected(widgetsSelectionCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(widgetsSelectionCallback.resultsCode))
                        {
                            GetActionButtonOfType(AppData.InputActionButtonType.SelectionButton, getButtonCallback =>
                            {
                                if (AppData.Helpers.IsSuccessCode(getButtonCallback.resultsCode))
                                {
                                    foreach (var button in getButtonCallback.data)
                                    {
                                        if (button.dataPackets.selectionOption == AppData.SelectionOption.SelectPage)
                                        {
                                            button.SetTitle(title);

                                            button.SetUIInputState(AppData.InputUIState.Disabled);
                                        }
                                    }
                                }
                                else
                                    LogError(getButtonCallback.results, this);
                            });
                        }
                        else
                        {
                            GetActionButtonOfType(AppData.InputActionButtonType.SelectionButton, getButtonCallback =>
                            {
                                if (AppData.Helpers.IsSuccessCode(getButtonCallback.resultsCode))
                                {
                                    foreach (var button in getButtonCallback.data)
                                    {
                                        if (button.dataPackets.selectionOption == AppData.SelectionOption.SelectPage)
                                        {
                                            button.SetTitle(title);

                                            if (enableSelectionButton)
                                            {
                                                LogWarning($"Not All Widgets Selected - Enabled : {enableSelectionButton}.", this, () => OnScreenWidget());

                                                button.SetUIInputState(AppData.InputUIState.Enabled);
                                            }
                                            else
                                                button.SetUIInputState(AppData.InputUIState.Disabled);
                                        }
                                    }
                                }
                                else
                                    LogError(getButtonCallback.results, this);
                            });
                        }
                    });
                }
                else
                    LogError("Widget Container Missing / Null.", this, () => OnScreenWidget());
            }
            else
                LogError("Scene Assets Manager Instance Is Not Yet Initialized.", this, () => OnScreenWidget());
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
           if(actionType == AppData.CheckboxInputActionType.InverseSelection)
            {
                if (SceneAssetsManager.Instance != null)
                {
                    SceneAssetsManager.Instance.GetFolderStructureData().SetInverseSelect(value);

                }
                else
                    LogError("Scene Assets Manager Instance Is Not Yet Initialized.", this, () => OnCheckboxValueChanged(actionType, value, dataPackets));
            }
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}