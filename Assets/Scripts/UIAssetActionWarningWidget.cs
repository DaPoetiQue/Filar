using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIAssetActionWarningWidget : AppData.Widget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        TMP_Text messageDisplayer;

        [Space(5)]
        [SerializeField]
        bool snapToSelection = false;

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            uiAssetWarningWidget = this;
            base.Init();
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {

        }

        protected override void OnScreenWidget()
        {
            if (snapToSelection)
            {
                if (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetPaginationViewType() == AppData.PaginationViewType.Pager)
                {
                    List<AppData.UIScreenWidget<AppData.SceneDataPackets>> currentPage = SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.Pagination_GetCurrentPage();

                    if (currentPage != null && currentPage.Count > 0)
                    {
                        List<AppData.UIScreenWidget<AppData.SceneDataPackets>> selectedWidgets = new List<AppData.UIScreenWidget<AppData.SceneDataPackets>>();
                        var currentSelections = SelectableManager.Instance.GetFolderStructureSelectionData().GetCurrentSelections();

                        foreach (var item in currentPage)
                        {
                            foreach (var selection in currentSelections)
                            {
                                if (selection == item)
                                    selectedWidgets.Add(item);
                            }
                        }

                        if (selectedWidgets.Count > 0)
                        {
                            SceneAssetsManager.Instance.GetSortedWidgetsFromList(selectedWidgets, getFolderStructureSelectionData =>
                            {
                                if (AppData.Helpers.IsSuccessCode(getFolderStructureSelectionData.resultsCode))
                                {
                                    int contentCount = getFolderStructureSelectionData.data.Count;

                                    if (getFolderStructureSelectionData.data.Count == 1)
                                        UpdateWidgetSelection(getFolderStructureSelectionData.data[contentCount - 1]);
                                    else
                                        UpdateWidgetsSelection(getFolderStructureSelectionData.data);
                                }
                                else
                                    Debug.LogWarning($"--> OnScreenWidget's GetFilteredWidgetsFromList Failed With Results : {getFolderStructureSelectionData.results}");
                            });
                        }
                        else
                            Debug.LogWarning("--> OnScreenWidget Failed : No Selected Items Found In Current Page.");
                    }
                    else
                        Debug.LogWarning("--> Current Page Not Found / Null.");
                }

                if (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetPaginationViewType() == AppData.PaginationViewType.Scroller)
                {
                    var currentSelections = SelectableManager.Instance.GetFolderStructureSelectionData().GetCurrentSelections();

                    if (currentSelections != null && currentSelections.Count > 0)
                    {
                        SceneAssetsManager.Instance.GetSortedWidgetsFromList(currentSelections, getFolderStructureSelectionData =>
                        {
                            if (AppData.Helpers.IsSuccessCode(getFolderStructureSelectionData.resultsCode))
                            {
                                int contentCount = getFolderStructureSelectionData.data.Count;

                                if (contentCount == 1)
                                {
                                    AppData.UIScreenWidget<AppData.SceneDataPackets> widget = getFolderStructureSelectionData.data[contentCount - 1];

                                    if (widget != null)
                                    {
                                        SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.OnFocusToWidget(widget);
                                        UpdateWidgetSelection(widget);
                                    }
                                    else
                                        Debug.LogWarning($"--> OnScreenWidget's GetFilteredWidgetsFromList Failed - Widget Is Null.");
                                }

                                if (contentCount > 1)
                                {
                                    int focusedIndex = Mathf.RoundToInt(contentCount / 2);
                                    AppData.UIScreenWidget<AppData.SceneDataPackets> widget = getFolderStructureSelectionData.data[focusedIndex];

                                    if (widget != null)
                                    {
                                        SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.OnFocusToWidget(widget);
                                        UpdateWidgetsSelection(getFolderStructureSelectionData.data);
                                    }
                                    else
                                        Debug.LogWarning($"--> OnScreenWidget's GetFilteredWidgetsFromList Failed - Widget Is Null.");
                                }
                            }
                            else
                                Debug.LogWarning($"--> OnScreenWidget's GetFilteredWidgetsFromList Failed With Results : {getFolderStructureSelectionData.results}");
                        });
                    }
                    else
                        Debug.LogWarning("--> OnScreenWidget's GetCurrentSelections Failed - No Selections Found.");
                }
            }
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            if (!string.IsNullOrEmpty(currentDataPackets.popUpMessage))
                SetUITextDisplayerValue(AppData.ScreenTextType.MessageDisplayer, currentDataPackets.popUpMessage);
        }

        void UpdateWidgetSelection(AppData.UIScreenWidget<AppData.SceneDataPackets> selectedWidget)
        {
            if (SceneAssetsManager.Instance.GetFolderStructureData().GetCurrentPaginationViewType() == AppData.PaginationViewType.Pager)
            {
                if (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.Pagination_ItemExistInCurrentPage(selectedWidget))
                {
                    Vector3 widgetPosition = new Vector3(selectedWidget.GetWidgetPosition().x, selectedWidget.GetWidgetPosition().y, widgetRect.position.z);
                    SetWidgetPosition(widgetPosition);
                    SetWidgetSizeDelta(selectedWidget.GetWidgetSizeDelta());
                }
            }

            if (SceneAssetsManager.Instance.GetFolderStructureData().GetCurrentPaginationViewType() == AppData.PaginationViewType.Scroller)
            {
                Vector3 widgetPosition = new Vector3(selectedWidget.GetWidgetPosition().x, selectedWidget.GetWidgetPosition().y, widgetRect.position.z);
                SetWidgetPosition(widgetPosition);
                SetWidgetSizeDelta(selectedWidget.GetWidgetSizeDelta());
            }

            SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.OnUpdateSelectedWidgets(true, AppData.InputUIState.Selected, true);
        }

        void UpdateWidgetsSelection(List<AppData.UIScreenWidget<AppData.SceneDataPackets>> selectedWidgets)
        {
            int focusedIndex = Mathf.RoundToInt(selectedWidgets.Count / 2);
            var selectedWidget = selectedWidgets[focusedIndex];

            if (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetLayout().viewType == AppData.LayoutViewType.ItemView)
            {
                float positionX = selectedWidget.GetWidgetPosition().x - selectedWidget.GetWidgetPosition().x;
                float positionY = selectedWidget.GetWidgetPosition().y;
                float positionZ = widgetRect.position.z;

                Vector3 widgetPosition = new Vector3(positionX, positionY, positionZ);

                float width = (selectedWidget.GetWidgetSizeDelta().x * 2) + (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetLayoutSpacing().x * 2);
                float height = selectedWidget.GetWidgetSizeDelta().y + (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetLayoutSpacing().y * 2);
                Vector2 sizeDelta = new Vector2(width, height);

                SetWidgetSizeDelta(sizeDelta);
                SetWidgetPosition(widgetPosition);
            }

            if (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetLayout().viewType == AppData.LayoutViewType.ListView)
            {
                float widgetHeight = selectedWidget.GetWidgetSizeDelta().y + SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetLayoutSpacing().y;

                float positionX = selectedWidget.GetWidgetPosition().x;
                float positionY = selectedWidget.GetWidgetPosition().y + ((widgetHeight + SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetLayoutSpacing().y) * 2);
                float positionZ = widgetRect.position.z;

                Vector3 widgetPosition = new Vector3(positionX, positionY, positionZ);

                float width = selectedWidget.GetWidgetSizeDelta().x + (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetLayoutSpacing().x * 2);
                float height = (selectedWidget.GetWidgetSizeDelta().y * 2) + (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetLayoutSpacing().y * 2);

                Vector2 sizeDelta = new Vector2(width, height);

                SetWidgetSizeDelta(sizeDelta);
                SetWidgetPosition(widgetPosition);
            }

            SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.OnUpdateSelectedWidgets(true, AppData.InputUIState.Selected, true);
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
