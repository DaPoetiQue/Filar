using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace Com.RedicalGames.Filar
{
    public class ConfirmationPopUpWidget : AppData.Widget
    {
        #region Components

        [Space(5)]
        [SerializeField]
        Image thumbnailDisplayer;

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
            confirmationWidget = this;
            base.Init();
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
        }

        protected override void OnScreenWidget()
        {
            if (snapToSelection)
            {
                AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, screenUIManagerCallbackResults =>
                {
                    if (screenUIManagerCallbackResults.Success())
                    {
                        var screenUIManager = screenUIManagerCallbackResults.data;

                        DatabaseManager.Instance.GetDynamicContainer<DynamicWidgetsContainer>(screenUIManager.GetCurrentUIScreenType(), containerCallbackResults =>
                        {
                            if (containerCallbackResults.Success())
                            {
                                var container = containerCallbackResults.data;

                                var paginationViewType = containerCallbackResults.data.GetPaginationViewType();

                                if (paginationViewType == AppData.PaginationViewType.Pager)
                                {
                                    List<AppData.UIScreenWidget> currentPage = container.Pagination_GetCurrentPage();

                                    if (currentPage != null && currentPage.Count > 0)
                                    {
                                        List<AppData.UIScreenWidget> selectedWidgets = new List<AppData.UIScreenWidget>();

                                        SelectableManager.Instance.GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
                                        {
                                            if (projectSelectionCallbackResults.Success())
                                            {
                                                var currentSelections = projectSelectionCallbackResults.data.GetCurrentSelections();

                                                foreach (var item in currentPage)
                                                {
                                                    foreach (var selection in currentSelections)
                                                    {
                                                        if (selection == item)
                                                            selectedWidgets.Add(item);
                                                    }
                                                }

                                                AppData.Helpers.ValueAssigned(selectedWidgets.Count, valueAssignedCallbackResults =>
                                                {
                                                    if (valueAssignedCallbackResults.Success())
                                                    {
                                                        DatabaseManager.Instance.GetSortedWidgetsFromList(selectedWidgets, GetSelectableAssetType(), getFolderStructureSelectionData =>
                                                        {
                                                            if (AppData.Helpers.IsSuccessCode(getFolderStructureSelectionData.resultCode))
                                                            {
                                                                int contentCount = getFolderStructureSelectionData.data.Count;

                                                                if (getFolderStructureSelectionData.data.Count == 1)
                                                                    UpdateWidgetSelection(getFolderStructureSelectionData.data[contentCount - 1]);
                                                                else
                                                                    UpdateWidgetsSelection(getFolderStructureSelectionData.data);
                                                            }
                                                            else
                                                                Debug.LogWarning($"--> OnScreenWidget's GetFilteredWidgetsFromList Failed With Results : {getFolderStructureSelectionData.result}");
                                                        });
                                                    }
                                                    else
                                                        Log(valueAssignedCallbackResults.resultCode, valueAssignedCallbackResults.result, this);
                                                });
                                            }
                                            else
                                                Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
                                        });
                                    }
                                    else
                                        Debug.LogWarning("--> Current Page Not Found / Null.");
                                }

                                if (paginationViewType == AppData.PaginationViewType.Scroller)
                                {
                                    SelectableManager.Instance.GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
                                    {
                                        if (projectSelectionCallbackResults.Success())
                                        {
                                            var currentSelections = projectSelectionCallbackResults.data.GetCurrentSelections();

                                            if (currentSelections != null && currentSelections.Count > 0)
                                            {
                                                DatabaseManager.Instance.GetSortedWidgetsFromList(currentSelections, GetSelectableAssetType(), getFolderStructureSelectionData =>
                                                {
                                                    if (AppData.Helpers.IsSuccessCode(getFolderStructureSelectionData.resultCode))
                                                    {
                                                        int contentCount = getFolderStructureSelectionData.data.Count;

                                                        if (contentCount == 1)
                                                        {
                                                            AppData.UIScreenWidget widget = getFolderStructureSelectionData.data[contentCount - 1];

                                                            if (widget != null)
                                                            {
                                                                DatabaseManager.Instance.GetRefreshData().screenContainer.OnFocusToWidget(widget);
                                                                UpdateWidgetSelection(widget);
                                                            }
                                                            else
                                                                Debug.LogWarning($"--> OnScreenWidget's GetFilteredWidgetsFromList Failed - Widget Is Null.");
                                                        }

                                                        AppData.Helpers.ValueIsGraterThanReference(contentCount, 1, valueAssignedCallbackResults =>
                                                        {
                                                            if (valueAssignedCallbackResults.Success())
                                                            {
                                                                int focusedIndex = Mathf.RoundToInt(contentCount / 2);
                                                                AppData.UIScreenWidget widget = getFolderStructureSelectionData.data[focusedIndex];

                                                                if (widget != null)
                                                                {
                                                                    containerCallbackResults.data.OnFocusToWidget(widget);
                                                                    UpdateWidgetsSelection(getFolderStructureSelectionData.data);
                                                                }
                                                                else
                                                                    Debug.LogWarning($"--> OnScreenWidget's GetFilteredWidgetsFromList Failed - Widget Is Null.");
                                                            }
                                                            else
                                                                Log(valueAssignedCallbackResults.resultCode, valueAssignedCallbackResults.result, this);
                                                        });
                                                    }
                                                    else
                                                        Debug.LogWarning($"--> OnScreenWidget's GetFilteredWidgetsFromList Failed With Results : {getFolderStructureSelectionData.result}");
                                                });
                                            }
                                            else
                                                Debug.LogWarning("--> OnScreenWidget's GetCurrentSelections Failed - No Selections Found.");
                                        }
                                        else
                                            Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
                                    });
                                }
                            }
                            else
                                Log(containerCallbackResults.resultCode, containerCallbackResults.result, this);
                        });
                    }
                });
            }
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            if (!string.IsNullOrEmpty(base.dataPackets.popUpMessage))
                SetUITextDisplayerValue(AppData.ScreenTextType.MessageDisplayer, base.dataPackets.popUpMessage);

            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        void UpdateWidgetSelection(AppData.UIScreenWidget selectedWidget)
        {
            if (DatabaseManager.Instance.GetProjectStructureData().Success())
            {
                if (DatabaseManager.Instance.GetProjectStructureData().data.GetPaginationViewType() == AppData.PaginationViewType.Pager)
                {
                    if (DatabaseManager.Instance.GetRefreshData().screenContainer.Pagination_ItemExistInCurrentPage(selectedWidget))
                    {
                        Vector3 widgetPosition = new Vector3(selectedWidget.GetWidgetPosition().x, selectedWidget.GetWidgetPosition().y, widgetRect.position.z);
                        SetWidgetPosition(widgetPosition);
                        SetWidgetSizeDelta(selectedWidget.GetWidgetSizeDelta());
                    }
                }

                if (DatabaseManager.Instance.GetProjectStructureData().data.GetPaginationViewType() == AppData.PaginationViewType.Scroller)
                {
                    Vector3 widgetPosition = new Vector3(selectedWidget.GetWidgetPosition().x, selectedWidget.GetWidgetPosition().y, widgetRect.position.z);
                    SetWidgetPosition(widgetPosition);
                    SetWidgetSizeDelta(selectedWidget.GetWidgetSizeDelta());
                }

                DatabaseManager.Instance.GetRefreshData().screenContainer.OnUpdateSelectedWidgets(true, AppData.InputUIState.Selected, true);
            }
            else
                Log(DatabaseManager.Instance.GetProjectStructureData().resultCode, DatabaseManager.Instance.GetProjectStructureData().result, this);
        }

        void UpdateWidgetsSelection(List<AppData.UIScreenWidget> selectedWidgets)
        {
            int focusedIndex = Mathf.RoundToInt(selectedWidgets.Count / 2);
            var selectedWidget = selectedWidgets[focusedIndex];

            if (DatabaseManager.Instance.GetRefreshData().screenContainer.GetLayout().viewType == AppData.LayoutViewType.ItemView)
            {
                float positionX = selectedWidget.GetWidgetPosition().x - selectedWidget.GetWidgetPosition().x;
                float positionY = selectedWidget.GetWidgetPosition().y;
                float positionZ = widgetRect.position.z;

                Vector3 widgetPosition = new Vector3(positionX, positionY, positionZ);

                float width = (selectedWidget.GetWidgetSizeDelta().x * 2) + (DatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().x * 2);
                float height = selectedWidget.GetWidgetSizeDelta().y + (DatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().y * 2);
                Vector2 sizeDelta = new Vector2(width, height);

                SetWidgetSizeDelta(sizeDelta);
                SetWidgetPosition(widgetPosition);
            }

            if (DatabaseManager.Instance.GetRefreshData().screenContainer.GetLayout().viewType == AppData.LayoutViewType.ListView)
            {
                float widgetHeight = selectedWidget.GetWidgetSizeDelta().y + DatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().y;

                float positionX = selectedWidget.GetWidgetPosition().x;
                float positionY = selectedWidget.GetWidgetPosition().y + ((widgetHeight + DatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().y) * 2);
                float positionZ = widgetRect.position.z;

                Vector3 widgetPosition = new Vector3(positionX, positionY, positionZ);

                float width = selectedWidget.GetWidgetSizeDelta().x + (DatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().x * 2);
                float height = (selectedWidget.GetWidgetSizeDelta().y * 2) + (DatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().y * 2);

                Vector2 sizeDelta = new Vector2(width, height);

                SetWidgetSizeDelta(sizeDelta);
                SetWidgetPosition(widgetPosition);
            }

            DatabaseManager.Instance.GetRefreshData().screenContainer.OnUpdateSelectedWidgets(true, AppData.InputUIState.Selected, true);
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
