using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

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

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.WidgetType>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetType());

                if (callbackResults.Success())
                {
                    var widgetType = GetType().data;

                    callbackResults.SetResult(GetStatePacket().Initialized(widgetType));

                    if (callbackResults.Success())
                    {
                        callbackResults.result = $"Widget : {GetStatePacket().GetName()} Of Type : {GetStatePacket().GetType()} State Is Set To : {GetStatePacket().GetStateType()}";
                        callbackResults.data = GetStatePacket();
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            if (snapToSelection)
            {
                AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, screenUIManagerCallbackResults =>
                {
                    if (screenUIManagerCallbackResults.Success())
                    {
                        var screenUIManager = screenUIManagerCallbackResults.data;

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "App Database Manager Instance Is Not Yet Initialized."));

                        if (callbackResults.Success())
                        {
                            var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).GetData();

                            callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                            if (callbackResults.Success())
                            {
                                var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                                assetBundlesLibrary.GetDynamicContainer<DynamicWidgetsContainer>(screenUIManager.GetCurrentScreenType().GetData(), AppData.ContentContainerType.FolderStuctureContent, AppData.ContainerViewSpaceType.Screen, dynamicContainerCallbackResults =>
                                {
                                    callbackResults.SetResult(dynamicContainerCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        var container = dynamicContainerCallbackResults.GetData();

                                        var paginationViewType = container.GetPaginationViewType();

                                        if (paginationViewType == AppData.PaginationViewType.Pager)
                                        {
                                            List<AppData.SelectableWidget> currentPage = container.Pagination_GetCurrentPage();

                                            if (currentPage != null && currentPage.Count > 0)
                                            {
                                                List<AppData.SelectableWidget> selectedWidgets = new List<AppData.SelectableWidget>();

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
                                                                AppDatabaseManager.Instance.GetSortedWidgetsFromList(selectedWidgets, GetSelectableAssetType(), getFolderStructureSelectionData =>
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
                                                        AppDatabaseManager.Instance.GetSortedWidgetsFromList(currentSelections, GetSelectableAssetType(), getFolderStructureSelectionData =>
                                                        {
                                                            if (AppData.Helpers.IsSuccessCode(getFolderStructureSelectionData.resultCode))
                                                            {
                                                                int contentCount = getFolderStructureSelectionData.data.Count;

                                                                if (contentCount == 1)
                                                                {
                                                                    AppData.SelectableWidget widget = getFolderStructureSelectionData.data[contentCount - 1];

                                                                    if (widget != null)
                                                                    {
                                                                        AppDatabaseManager.Instance.GetRefreshData().screenContainer.OnFocusToWidget(widget);
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
                                                                        AppData.SelectableWidget widget = getFolderStructureSelectionData.data[focusedIndex];

                                                                        if (widget != null)
                                                                        {
                                                                            container.OnFocusToWidget(widget);
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
                                });
                            }
                        }
                    }
                });
            }
        }

        void UpdateWidgetSelection(AppData.SelectableWidget selectedWidget)
        {
            if (AppDatabaseManager.Instance.GetProjectStructureData().Success())
            {
                if (AppDatabaseManager.Instance.GetProjectStructureData().data.GetPaginationViewType() == AppData.PaginationViewType.Pager)
                {
                    if (AppDatabaseManager.Instance.GetRefreshData().screenContainer.Pagination_ItemExistInCurrentPage(selectedWidget))
                    {
                        Vector3 widgetPosition = new Vector3(selectedWidget.GetWidgetPosition().x, selectedWidget.GetWidgetPosition().y, widgetRect.position.z);
                        SetWidgetPosition(widgetPosition);
                        SetWidgetSizeDelta(selectedWidget.GetWidgetSizeDelta());
                    }
                }

                if (AppDatabaseManager.Instance.GetProjectStructureData().data.GetPaginationViewType() == AppData.PaginationViewType.Scroller)
                {
                    Vector3 widgetPosition = new Vector3(selectedWidget.GetWidgetPosition().x, selectedWidget.GetWidgetPosition().y, widgetRect.position.z);
                    SetWidgetPosition(widgetPosition);
                    SetWidgetSizeDelta(selectedWidget.GetWidgetSizeDelta());
                }

                AppDatabaseManager.Instance.GetRefreshData().screenContainer.OnUpdateSelectedWidgets(true, AppData.InputUIState.Selected, true);
            }
            else
                Log(AppDatabaseManager.Instance.GetProjectStructureData().resultCode, AppDatabaseManager.Instance.GetProjectStructureData().result, this);
        }

        void UpdateWidgetsSelection(List<AppData.SelectableWidget> selectedWidgets)
        {
            int focusedIndex = Mathf.RoundToInt(selectedWidgets.Count / 2);
            var selectedWidget = selectedWidgets[focusedIndex];

            if (AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetLayout().viewType == AppData.LayoutViewType.ItemView)
            {
                float positionX = selectedWidget.GetWidgetPosition().x - selectedWidget.GetWidgetPosition().x;
                float positionY = selectedWidget.GetWidgetPosition().y;
                float positionZ = widgetRect.position.z;

                Vector3 widgetPosition = new Vector3(positionX, positionY, positionZ);

                float width = (selectedWidget.GetWidgetSizeDelta().x * 2) + (AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().x * 2);
                float height = selectedWidget.GetWidgetSizeDelta().y + (AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().y * 2);
                Vector2 sizeDelta = new Vector2(width, height);

                SetWidgetSizeDelta(sizeDelta);
                SetWidgetPosition(widgetPosition);
            }

            if (AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetLayout().viewType == AppData.LayoutViewType.ListView)
            {
                float widgetHeight = selectedWidget.GetWidgetSizeDelta().y + AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().y;

                float positionX = selectedWidget.GetWidgetPosition().x;
                float positionY = selectedWidget.GetWidgetPosition().y + ((widgetHeight + AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().y) * 2);
                float positionZ = widgetRect.position.z;

                Vector3 widgetPosition = new Vector3(positionX, positionY, positionZ);

                float width = selectedWidget.GetWidgetSizeDelta().x + (AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().x * 2);
                float height = (selectedWidget.GetWidgetSizeDelta().y * 2) + (AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetLayoutSpacing().y * 2);

                Vector2 sizeDelta = new Vector2(width, height);

                SetWidgetSizeDelta(sizeDelta);
                SetWidgetPosition(widgetPosition);
            }

            AppDatabaseManager.Instance.GetRefreshData().screenContainer.OnUpdateSelectedWidgets(true, AppData.InputUIState.Selected, true);
        }

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
