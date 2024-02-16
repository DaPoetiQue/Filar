using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ProjectCreationWidget : AppData.Widget
    {
        #region Components


        [Space(5)]
        [SerializeField]
        AppData.ProjectStructureData folderStructureDataTemplate = new AppData.ProjectStructureData();

        AppData.ProjectStructureData newProjectStructureData = new AppData.ProjectStructureData();

        private AppData.ActionButtonListener onConfirmationButtonEvent = new AppData.ActionButtonListener();
        private AppData.ActionButtonListener onCancelButtonEvent = new AppData.ActionButtonListener();

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>();

            Init(initializationCallbackResults =>
            {
                callbackResults.SetResultsData(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>> OnGetState()
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.WidgetType, AppData.TabViewType, AppData.Widget>>(AppData.Helpers.GetAppComponentValid(GetStatePacket(), $"{GetName()} - State Object", "Widget State Object Is Null / Not Yet Initialized In The Base Class."));

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

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    callbackResults.SetResult(screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget));

                    if (callbackResults.Success())
                    {
                        var confirmationWidget = screen.GetWidget(AppData.WidgetType.ConfirmationPopUpWidget).GetData() as ConfirmationPopUpWidget;

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(confirmationWidget, "Confirmation Widget", "Failed To Get Confirmation Widget - Invalid Operation."));

                        if (callbackResults.Success())
                        {
                            confirmationWidget.UnRegisterActionButtonListeners(eventsUnregisteredCallbackResults => 
                            {
                                callbackResults.SetResult(eventsUnregisteredCallbackResults);

                                if (callbackResults.Success())
                                {
                                    var confirmationWidgetConfig = new AppData.SceneConfigDataPacket();

                                    confirmationWidgetConfig.SetReferencedWidgetType(AppData.WidgetType.ConfirmationPopUpWidget);
                                    confirmationWidgetConfig.blurScreen = true;
                                    confirmationWidgetConfig.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.ForeGround);

                                    switch (actionType)
                                    {
                                        case AppData.InputActionButtonType.CloseButton:

                                            confirmationWidget.UnRegisterActionButtonListeners(actionEventsUnRegisteredCallbackResults =>
                                            {
                                                callbackResults.SetResult(actionEventsUnRegisteredCallbackResults);

                                                if (callbackResults.Success())
                                                {
                                                    onConfirmationButtonEvent.SetMethod(OnConfirmedEvent, methodSetCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(methodSetCallbackResults);

                                                        if (callbackResults.Success())
                                                        {
                                                            onConfirmationButtonEvent.SetAction(AppData.InputActionButtonType.ConfirmationButton, actionSetCallbackResults =>
                                                            {
                                                                callbackResults.SetResult(actionSetCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    onCancelButtonEvent.SetMethod(OnCancelledEvent, methodSetCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(methodSetCallbackResults);

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            onCancelButtonEvent.SetAction(AppData.InputActionButtonType.Cancel, actionSetCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(actionSetCallbackResults);

                                                                                if (callbackResults.Success())
                                                                                {
                                                                                    confirmationWidget.RegisterActionButtonListeners(onConfirmRegisteredCallbackResults =>
                                                                                    {
                                                                                        callbackResults.SetResult(onConfirmRegisteredCallbackResults);

                                                                                        if (callbackResults.Success())
                                                                                            screen.ShowWidget(confirmationWidgetConfig);
                                                                                        else
                                                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                                                    }, onConfirmationButtonEvent, onCancelButtonEvent);
                                                                                }
                                                                                else
                                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                            });
                                                                        }
                                                                        else
                                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                    });
                                                                }
                                                                else
                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                            });
                                                        }
                                                        else
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                                else
                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            });

                                            break;

                                        case AppData.InputActionButtonType.ConfirmationButton:

                                            OnDataValidation(newProjectStructureData, dataValidCallbackResults =>
                                            {
                                                if (dataValidCallbackResults.Success())
                                                {
                                                    OnInputFieldValidation(GetType().GetData(), AppData.ValidationResultsType.Success, dataValidCallbackResults.data);

                                                    if (AppDatabaseManager.Instance != null)
                                                    {
                                                        AppDatabaseManager.Instance.CreateNewProjectStructureData(newProjectStructureData, createNewProjectCallbackResults =>
                                                        {
                                                            if (createNewProjectCallbackResults.Success())
                                                            {
                                                                if (AppDatabaseManager.Instance.GetProjectRootStructureData().Success())
                                                                {
                                                                    if (ScreenUIManager.Instance.GetCurrentScreen().Success())
                                                                        ScreenUIManager.Instance.GetCurrentScreen().GetData().HideScreenWidget(dataPackets.widgetType, dataPackets);

                                                                    StartCoroutine(OnCreatedAsync(createNewProjectCallbackResults.data.GetProjectInfo(), async createdCallbackResults =>
                                                                    {
                                                                        if (createdCallbackResults.Success())
                                                                        {
                                                                            dataPackets.notification.message = createNewProjectCallbackResults.result;

                                                                            if (dataPackets.notification.showNotifications)
                                                                                NotificationSystemManager.Instance.ScheduleNotification(dataPackets.notification);

                                                                            await ScreenUIManager.Instance.RefreshAsync();
                                                                        }
                                                                        else
                                                                            Log(createdCallbackResults.resultCode, createdCallbackResults.result, this);
                                                                    }));
                                                                }
                                                                else
                                                                    Log(AppDatabaseManager.Instance.GetProjectRootStructureData().resultCode, AppDatabaseManager.Instance.GetProjectRootStructureData().result, this);
                                                            }
                                                            else
                                                                Log(createNewProjectCallbackResults.resultCode, createNewProjectCallbackResults.result, this);
                                                        });
                                                    }
                                                    else
                                                        LogError("Scene Assets Manager Instance Is Not Yet Initialized.", this);
                                                }
                                                else
                                                {
                                                    OnInputFieldValidation(GetType().GetData(), AppData.ValidationResultsType.Error, dataValidCallbackResults.data);

                                                    Log(dataValidCallbackResults.resultCode, dataValidCallbackResults.result, this);
                                                }
                                            });

                                            break;

                                        case AppData.InputActionButtonType.Cancel:

                                            confirmationWidget.UnRegisterActionButtonListeners(actionEventsUnRegisteredCallbackResults => 
                                            {
                                                callbackResults.SetResult(actionEventsUnRegisteredCallbackResults);

                                                if(callbackResults.Success())
                                                {
                                                    onConfirmationButtonEvent.SetMethod(OnConfirmedEvent, methodSetCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(methodSetCallbackResults);

                                                        if (callbackResults.Success())
                                                        {
                                                            onConfirmationButtonEvent.SetAction(AppData.InputActionButtonType.ConfirmationButton, actionSetCallbackResults =>
                                                            {
                                                                callbackResults.SetResult(actionSetCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    onCancelButtonEvent.SetMethod(OnCancelledEvent, methodSetCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(methodSetCallbackResults);

                                                                        if (callbackResults.Success())
                                                                        {
                                                                            onCancelButtonEvent.SetAction(AppData.InputActionButtonType.Cancel, actionSetCallbackResults =>
                                                                            {
                                                                                callbackResults.SetResult(actionSetCallbackResults);

                                                                                if (callbackResults.Success())
                                                                                {
                                                                                    confirmationWidget.RegisterActionButtonListeners(onConfirmRegisteredCallbackResults =>
                                                                                    {
                                                                                        callbackResults.SetResult(onConfirmRegisteredCallbackResults);

                                                                                        if (callbackResults.Success())
                                                                                            screen.ShowWidget(confirmationWidgetConfig);
                                                                                        else
                                                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                                                    }, onConfirmationButtonEvent, onCancelButtonEvent);
                                                                                }
                                                                                else
                                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                            });
                                                                        }
                                                                        else
                                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                    });
                                                                }
                                                                else
                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                            });
                                                        }
                                                        else
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                                else
                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            });

                                            break;
                                    }
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            });
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnConfirmedEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    screen.HideScreenWidget(this, widgetHiddenCallbackResults => 
                    {
                        callbackResults.SetResult(widgetHiddenCallbackResults);
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnCancelledEvent()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                if (callbackResults.Success())
                {
                    var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                    var createProjectWidgetConfig = new AppData.SceneConfigDataPacket();

                    createProjectWidgetConfig.SetReferencedWidgetType(AppData.WidgetType.ProjectCreationWidget);
                    createProjectWidgetConfig.blurScreen = true;
                    createProjectWidgetConfig.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.Default);

                    screen.ShowWidget(createProjectWidgetConfig, showWidgetCallbackResults =>
                    {
                        callbackResults.SetResult(showWidgetCallbackResults);
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        IEnumerator OnCreatedAsync(AppData.ProjectInfo projectInfo, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            yield return new WaitForEndOfFrame();

            var rootData = AppDatabaseManager.Instance.GetProjectRootStructureData().data;
            rootData.GetProjectCreationTemplateData().SetProjectInfo(projectInfo);

            AppDatabaseManager.Instance.SaveModifiedData(rootData, dataSavedCallbackResults =>
            {
                callbackResults.result = dataSavedCallbackResults.result;
                callbackResults.resultCode = dataSavedCallbackResults.resultCode;
            });

            yield return new WaitForEndOfFrame();

            callback?.Invoke(callbackResults);
        }

        void CreateNewFolderStructureData(Action<AppData.CallbackData<AppData.ProjectStructureData>> callback)
        {
            AppData.CallbackData<AppData.ProjectStructureData> callbackResults = new AppData.CallbackData<AppData.ProjectStructureData>();

            if (AppDatabaseManager.Instance.GetProjectRootStructureData().Success())
            {
                var project = new AppData.ProjectStructureData
                {
                    projectInfo = new AppData.ProjectInfo(string.Empty, AppDatabaseManager.Instance.GetProjectRootStructureData().data.projectCreationTemplateData.GetProjectInfo().GetCategoryType(), AppDatabaseManager.Instance.GetProjectRootStructureData().data.projectCreationTemplateData.GetProjectInfo().GetTamplateType()),
                    rootFolder = folderStructureDataTemplate.rootFolder,
                    excludedSystemFiles = folderStructureDataTemplate.excludedSystemFiles,
                    excludedSystemFolders = folderStructureDataTemplate.excludedSystemFolders,
                    layoutViewType = folderStructureDataTemplate.layoutViewType,
                    paginationViewType = folderStructureDataTemplate.paginationViewType,
                    layouts = folderStructureDataTemplate.layouts,
                };

                callbackResults.result = "New Project Data Created.";
                callbackResults.data = project;
                callbackResults.resultCode = AppDatabaseManager.Instance.GetProjectRootStructureData().resultCode;
            }
            else
            {
                callbackResults.result = AppDatabaseManager.Instance.GetProjectRootStructureData().result;
                callbackResults.data = default;
                callbackResults.resultCode = AppDatabaseManager.Instance.GetProjectRootStructureData().resultCode;
            }

            callback.Invoke(callbackResults);
        }

        protected override void OnHideScreenWidget(Action<AppData.Callback> callback = null)
        {
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            switch(dataPackets.GetAction().GetData())
            {
                case AppData.InputFieldActionType.AssetNameField:

                    if (newProjectStructureData != null)
                    {
                        newProjectStructureData.name = value;

                        OnDataValidation(newProjectStructureData, dataValidCallbackResults => 
                        {
                            if (dataValidCallbackResults.Success())
                            {
                                GetInputField(AppData.InputFieldActionType.AssetNameField, inputFieldCallbackResults =>
                                {
                                    if (inputFieldCallbackResults.Success())
                                    {
                                        if (inputFieldCallbackResults.GetData().GetValidationStateInfo().GetData().GetValidationResult().GetData() == AppData.ValidationResultsType.Warning || inputFieldCallbackResults.data.GetValidationStateInfo().GetData().GetValidationResult().GetData() == AppData.ValidationResultsType.Error)
                                            OnInputFieldValidation(GetType().GetData(), AppData.ValidationResultsType.Success, dataValidCallbackResults.GetData());
                                    }
                                    else
                                        Log(inputFieldCallbackResults.resultCode, inputFieldCallbackResults.result, this);
                                });
                            }
                            else
                            {
                                GetInputField(AppData.InputFieldActionType.AssetNameField, inputFieldCallbackResults =>
                                {
                                    if (inputFieldCallbackResults.Success())
                                    {
                                        if (inputFieldCallbackResults.GetData().GetValidationStateInfo().GetData().GetValidationResult().GetData() == AppData.ValidationResultsType.Success)
                                            OnInputFieldValidation(GetType().GetData(), AppData.ValidationResultsType.Warning, dataValidCallbackResults.GetData());
                                    }
                                    else
                                        Log(inputFieldCallbackResults.resultCode, inputFieldCallbackResults.result, this);
                                });
                            }
                        });
                    }
                    else
                        LogError("New Folder Structure Data Is Null.", this);

                    break;
            }
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenWidget(Action<AppData.Callback> callback = null)
        {

        }
        protected override void OnScreenWidget(AppData.SceneConfigDataPacket configDataPacket, Action<AppData.Callback> callback = null)
        {
            CreateNewFolderStructureData(newProjectCallbackResults => 
            {
                if (newProjectCallbackResults.Success())
                {
                    AppManager.Instance.GetAppRestriction(AppData.AppRestrictionType.ProjectSupport, restrictionCallbackResults => 
                    {
                        if (restrictionCallbackResults.Success())
                        {
                            newProjectStructureData = newProjectCallbackResults.GetData();
         
                            SetInputFieldPlaceHolder(AppData.InputFieldActionType.AssetNameField, "Project Name");

                            OnClearInputFieldValue(AppData.InputFieldActionType.AssetNameField);
                            OnClearInputFieldValidation(AppData.InputFieldActionType.AssetNameField);

                            var projectTypeContentParam = AppDatabaseManager.Instance.GetUIScreenGroupContentTemplate("Project Type Content", AppData.InputType.DropDown, state: AppData.InputUIState.Disabled,  dropdownActionType: AppData.InputDropDownActionType.ProjectType, placeHolder: "Project Type");
                            var projectTemplateContentParam = AppDatabaseManager.Instance.GetUIScreenGroupContentTemplate("Project Template Content", AppData.InputType.DropDown, state: AppData.InputUIState.Enabled, dropdownActionType: AppData.InputDropDownActionType.ProjectTamplate, placeHolder: "Templates", contents: AppDatabaseManager.Instance.GetDropdownContent<AppData.ProjectTamplateType>().data);

                            switch (restrictionCallbackResults.GetData().GetProjectSupportType())
                            {
                                case AppData.Compatibility.Supports_3D:

                                    projectTypeContentParam.contents = new List<string> { "3D" };
                                    projectTypeContentParam.SetUIInputState(AppData.InputUIState.Disabled);

                                    break;

                                case AppData.Compatibility.Supports_AR:

                                    projectTypeContentParam.contents = new List<string> { "3D", "AR" };
                                    projectTypeContentParam.SetUIInputState(AppData.InputUIState.Enabled);

                                    break;

                                case AppData.Compatibility.Supports_VR:

                                    projectTypeContentParam.contents = new List<string> { "3D", "AR", "VR" };
                                    projectTypeContentParam.SetUIInputState(AppData.InputUIState.Enabled);

                                    break;
                            }

                            SetActionDropdownContent(projectTypeContentParam, projectTemplateContentParam);

                            var projectType = (int)newProjectStructureData.GetProjectInfo().GetCategoryType() - 1;

                            SetActionDropdownSelection(AppData.InputDropDownActionType.ProjectType, projectType);
                            SetActionDropdownSelection(AppData.InputDropDownActionType.ProjectTamplate, (int)newProjectStructureData.GetProjectInfo().GetTamplateType());
                        }
                        else
                            Log(restrictionCallbackResults.GetResultCode, restrictionCallbackResults.GetResult, this);
                    });
                }
                else
                    Log(newProjectCallbackResults.GetResultCode, newProjectCallbackResults.GetResult, this);
            });
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new System.NotImplementedException();
        }

        void OnDataValidation(AppData.ProjectStructureData info, Action<AppData.CallbackData<AppData.InputFieldActionType>> callback)
        {
            bool isValidName = !string.IsNullOrEmpty(info.name);

            AppData.CallbackData<AppData.InputFieldActionType> callbackResults = new AppData.CallbackData<AppData.InputFieldActionType>();

            if (isValidName)
            {
                callbackResults.result = "Data Is Valid";
                callbackResults.data = AppData.InputFieldActionType.AssetNameField;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Name Field Is required - Invalid";
                callbackResults.data = AppData.InputFieldActionType.AssetNameField;
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            callback.Invoke(callbackResults);
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
           switch(dataPackets.GetAction().GetData())
            {
                case AppData.InputDropDownActionType.ProjectType:

                    int contentIndexA = AppDatabaseManager.Instance.GetDropdownContentCount<AppData.ProjectCategoryType>();
                    int contentIndexB = AppDatabaseManager.Instance.GetDropdownContentCount<AppData.ProjectCategoryType>("Project_", "All");

                    int index = value + AppDatabaseManager.Instance.GetDropdownContentIndex(contentIndexA, contentIndexB);
                    var categoryType = (AppData.ProjectCategoryType)index;

                    int projectIndex = AppDatabaseManager.Instance.GetDropdownContentTypeIndex(categoryType);

                    if (index.Equals(projectIndex))
                    {
                        if (newProjectStructureData != null)
                            newProjectStructureData.GetProjectInfo().SetCategoryType(categoryType);
                        else
                            LogError("New Folder Structure Data Is Null.", this);
                    }
                    else
                        LogWarning($"Project Type Index Is Invalid - Index Value {index} Is Not Equals To Project Type Index Value : {projectIndex}", this);

                    break;

                case AppData.InputDropDownActionType.ProjectTamplate:

                    var templateType = (AppData.ProjectTamplateType)value;

                    int templateIndex = AppDatabaseManager.Instance.GetDropdownContentTypeIndex(templateType);

                    if (value.Equals(templateIndex))
                    {
                        if (newProjectStructureData != null)
                            newProjectStructureData.projectInfo.templateType = templateType;
                        else
                            LogError("New Folder Structure Data Is Null.", this);
                    }
                    else
                        LogWarning($"Project Tamplate Type Index Is Invalid - Index Value {value} Is Not Equals To Project template Type Index Value : {templateIndex}", this);

                    break;
            }
        }

        protected override void ScrollerPosition(Vector2 position)
        {
           
        }

        protected override void OnScreenWidget<T>(AppData.ScriptableConfigDataPacket<T> scriptableConfigData, Action<AppData.Callback> callback = null)
        {
           
        }

        protected override void OnScreenWidgetShownEvent()
        {
         
        }

        protected override void OnScreenWidgetHiddenEvent()
        {
           
        }

        protected override void OnScreenWidgetTransitionInProgressEvent()
        {
          
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
          
        }

        #endregion
    }
}