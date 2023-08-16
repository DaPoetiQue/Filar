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
            if (DatabaseManager.Instance != null)
            {
                if (DatabaseManager.Instance.GetProjectStructureData().Success())
                    SetCheckboxValue(DatabaseManager.Instance.GetProjectStructureData().data.InverseSelect(), AppData.CheckboxInputActionType.InverseSelection);
                else
                    Log(DatabaseManager.Instance.GetProjectStructureData().resultCode, DatabaseManager.Instance.GetProjectStructureData().result, this);

                var widgetsContainer = DatabaseManager.Instance.GetWidgetsRefreshData().widgetsContainer;

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
                        if (AppData.Helpers.IsSuccessCode(widgetsSelectionCallback.resultCode))
                        {
                            GetActionButtonOfType(AppData.InputActionButtonType.SelectionButton, getButtonCallback =>
                            {
                                if (AppData.Helpers.IsSuccessCode(getButtonCallback.resultCode))
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
                                    LogError(getButtonCallback.result, this);
                            });
                        }
                        else
                        {
                            GetActionButtonOfType(AppData.InputActionButtonType.SelectionButton, getButtonCallback =>
                            {
                                if (AppData.Helpers.IsSuccessCode(getButtonCallback.resultCode))
                                {
                                    foreach (var button in getButtonCallback.data)
                                    {
                                        if (button.dataPackets.selectionOption == AppData.SelectionOption.SelectPage)
                                        {
                                            button.SetTitle(title);

                                            if (enableSelectionButton)
                                            {
                                                LogWarning($"Not All Widgets Selected - Enabled : {enableSelectionButton}.");

                                                button.SetUIInputState(AppData.InputUIState.Enabled);
                                            }
                                            else
                                                button.SetUIInputState(AppData.InputUIState.Disabled);
                                        }
                                    }
                                }
                                else
                                    LogError(getButtonCallback.result, this);
                            });
                        }
                    });
                }
                else
                    LogError("Widget Container Missing / Null.", this);
            }
            else
                LogError("Scene Assets Manager Instance Is Not Yet Initialized.", this);
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
                if (DatabaseManager.Instance != null)
                {
                    if (DatabaseManager.Instance.GetProjectStructureData().Success())
                        DatabaseManager.Instance.GetProjectStructureData().data.SetInverseSelect(value);
                    else
                        Log(DatabaseManager.Instance.GetProjectStructureData().resultCode, DatabaseManager.Instance.GetProjectStructureData().result, this);
                }
                else
                    LogError("Scene Assets Manager Instance Is Not Yet Initialized.", this);
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

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}