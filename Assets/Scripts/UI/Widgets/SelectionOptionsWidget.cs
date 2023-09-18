using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SelectionOptionsWidget : AppData.Widget
    {
        #region Components

        #endregion

        #region Main

        protected override void Initialize(Action<AppData.CallbackData<AppData.WidgetStatePacket>> callback)
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>();

            callbackResults.SetResult(GetType());

            if (callbackResults.Success())
            {
                OnRegisterWidget(this, onRegisterWidgetCallbackResults =>
                {
                    callbackResults.SetResult(GetType());

                    if (callbackResults.Success())
                    {
                        var widgetStatePacket = new AppData.WidgetStatePacket(name: GetName(), type: GetType().data, stateType: AppData.WidgetStateType.Initialized, value: this);

                        callbackResults.result = $"Widget : {GetName()} Of Type : {GetType().data}'s State Packet Has Been Initialized Successfully.";
                        callbackResults.data = widgetStatePacket;
                    }
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback.Invoke(callbackResults);
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
            if (AppDatabaseManager.Instance != null)
            {
                if (AppDatabaseManager.Instance.GetProjectStructureData().Success())
                    SetCheckboxValue(AppDatabaseManager.Instance.GetProjectStructureData().data.InverseSelect(), AppData.CheckboxInputActionType.InverseSelection);
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

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
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

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket> OnGetState()
        {
            AppData.CallbackData<AppData.WidgetStatePacket> callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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

        #endregion
    }
}