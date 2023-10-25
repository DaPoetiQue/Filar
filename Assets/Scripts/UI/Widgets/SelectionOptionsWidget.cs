using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SelectionOptionsWidget : AppData.Widget
    {
        #region Components

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

        protected override void OnHideScreenWidget()
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

        protected override void OnScreenWidget()
        {
            if (AppDatabaseManager.Instance != null)
            {
                if (AppDatabaseManager.Instance.GetProjectStructureData().Success())
                    SetActionCheckboxValue(AppData.CheckboxInputActionType.InverseSelection, AppDatabaseManager.Instance.GetProjectStructureData().data.InverseSelect());
                else
                    Log(AppDatabaseManager.Instance.GetProjectStructureData().resultCode, AppDatabaseManager.Instance.GetProjectStructureData().result, this);

                var widgetsContainer = AppDatabaseManager.Instance.GetRefreshData().screenContainer;

                if (widgetsContainer != null)
                {
                    string title = (widgetsContainer.GetPaginationViewType() == AppData.PaginationViewType.Pager) ? "Select Page" : "Select View";
                    bool enableSelectionButton = false;

                    if (widgetsContainer.GetPaginationViewType() == AppData.PaginationViewType.Pager)
                        enableSelectionButton = widgetsContainer.Pagination_GetPageCount() > 1;

                    if (widgetsContainer.GetPaginationViewType() == AppData.PaginationViewType.Scroller)
                    {
                        int itemsPerView = (widgetsContainer.GetLayout().viewType == AppData.LayoutViewType.ItemView) ? widgetsContainer.GetPaginationComponent().itemView_ItemsPerPage : widgetsContainer.GetPaginationComponent().listView_ItemsPerPage;
                        enableSelectionButton = widgetsContainer.GetContentCount().data > itemsPerView;
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

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
           if(actionType == AppData.CheckboxInputActionType.InverseSelection)
            {
                if (AppDatabaseManager.Instance != null)
                {
                    if (AppDatabaseManager.Instance.GetProjectStructureData().Success())
                        AppDatabaseManager.Instance.GetProjectStructureData().data.SetInverseSelect(value);
                    else
                        Log(AppDatabaseManager.Instance.GetProjectStructureData().resultCode, AppDatabaseManager.Instance.GetProjectStructureData().result, this);
                }
                else
                    LogError("Scene Assets Manager Instance Is Not Yet Initialized.", this);
            }
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
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

        #endregion
    }
}