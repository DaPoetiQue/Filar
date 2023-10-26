using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ScreenNavigationManager : AppData.SingletonBaseComponent<ScreenNavigationManager>
    {
        #region Components

        [SerializeField]
        Stack<AppData.FolderNavigationCommand> folderNavigationCommands = new Stack<AppData.FolderNavigationCommand>();

        [Space(5)]
        [SerializeField]
        AppData.SceneConfigDataPacket folderNavigationDataPackets = new AppData.SceneConfigDataPacket();

        [Space(5)]
        [SerializeField]
        AppData.SceneConfigDataPacket emptyFolderDataPackets = new AppData.SceneConfigDataPacket();

        [Space(5)]
        [SerializeField]
        AppData.SceneConfigDataPacket pagerNavigationWidgetDataPackets = new AppData.SceneConfigDataPacket();

        [Space(5)]
        [SerializeField]
        AppData.SceneConfigDataPacket scrollerNavigationWidgetDataPackets = new AppData.SceneConfigDataPacket();

        AppData.UIWidgetInfo currentUIFolderWidgetInfo = new AppData.UIWidgetInfo();
        List<string> folderNavigationNameList = new List<string>();

        #endregion

        #region Main

        public void NavigateToFolder(AppData.Folder folder, AppData.UIWidgetInfo folderWidgetInfo, AppData.FolderStructureType structureType)
        {
            // Store Previous Folder
            AppData.FolderNavigationCommand previousFolderCommand = new AppData.FolderNavigationCommand(AppDatabaseManager.Instance.GetCurrentFolder(), folderWidgetInfo, structureType);

            if (!folderNavigationCommands.Contains(previousFolderCommand))
            {
                folderNavigationCommands.Push(previousFolderCommand);
                folderNavigationNameList.Add(folderWidgetInfo.GetWidgetName().Replace("_FolderData", ""));
            }

            currentUIFolderWidgetInfo = folderWidgetInfo;

            // Open Selected Folder
            AppData.FolderNavigationCommand navigationCommand = new AppData.FolderNavigationCommand(folder, folderWidgetInfo, structureType);
            navigationCommand.Execute();
        }

        public void ReturnFromFolder(Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name, "Screen UI Manager Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var screenUIManager = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, ScreenUIManager.Instance.name).data;

                if (folderNavigationCommands.Count > 0)
                {
                    // Get Previous Folder
                    AppData.FolderNavigationCommand folderNavigation = folderNavigationCommands.Pop();

                    if (folderNavigationNameList.Count > 0)
                    {
                        string folderName = folderNavigation.folderWidgetInfo.GetWidgetName().Replace("_FolderData", "");

                        if (!string.IsNullOrEmpty(folderName) && folderNavigationNameList.Contains(folderName))
                            folderNavigationNameList.Remove(folderName);

                        SelectableManager.Instance.Select(folderNavigation.folderWidgetInfo.widgetName, AppData.FocusedSelectionType.InteractedItem, selectionCallback => { });
                    }

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "App Database Manager Instance Is Not Yet Initialized."));

                    if(callbackResults.Success())
                    {
                        var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name).GetData();

                        callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                        if (callbackResults.Success())
                        {
                            var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                            assetBundlesLibrary.GetDynamicContainer<DynamicWidgetsContainer>(screenUIManager.GetCurrentScreenType().GetData(), AppData.ContentContainerType.FolderStuctureContent, AppData.ContainerViewSpaceType.Screen, dynamicContainerCallbackResults => 
                            {
                                callbackResults.SetResult(dynamicContainerCallbackResults);

                                if(callbackResults.Success())
                                {
                                    if (SelectableManager.Instance != null)
                                        SelectableManager.Instance.Select(folderNavigation.folderWidgetInfo.widgetName, AppData.FocusedSelectionType.InteractedItem, selectionCallback => { });
                                    else
                                    {
                                        callbackResults.result = "Selectable Manager Instance Not Yet Initialized.";
                                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                        callbackResults.data = default;
                                    }
                                }
                            });
                        }

                    }

                    folderNavigation.Execute();

                    callbackResults.result = "Success : Returning From Folder";
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;

                    if (ScreenUIManager.Instance != null)
                    {
                        callbackResults.SetResult(ScreenUIManager.Instance.GetCurrentScreen());

                        if (callbackResults.Success())
                        {
                            if (folderNavigationCommands.Count == 0)
                            {
                                //ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.FolderNavigationWidget);
                                UpdateNavigationRootTitleDisplayer();

                                if (AppDatabaseManager.Instance.GetProjectStructureData().Success())
                                    folderNavigationDataPackets.widgetTitle = AppDatabaseManager.Instance.GetProjectStructureData().data.GetRootFolder().name;
                                else
                                    Log(AppDatabaseManager.Instance.GetProjectStructureData().resultCode, AppDatabaseManager.Instance.GetProjectStructureData().result, this);
                            }
                        }
                        else
                            Debug.LogWarning("--> ReturnFromFolder's hide Screen Widget Failed : ScreenUIManager.Instance Current Screen Value Is Missing / Not Found.");
                    }
                    else
                        Debug.LogWarning("--> ReturnFromFolder's hide Screen Widget Failed : ScreenUIManager.Instance Is Not Yet Initialized.");

                    //if(folderNavigationCommands.Count == 0)
                    //    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonUIImageValue(AppData.InputActionButtonType.Return, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.HomeIcon);
                }
                else
                {
                    callbackResults.result = "Failed : There Are No Commands In folderNavigationCommands";
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }

            callback?.Invoke(callbackResults);
        }

        public void HighligtEnteredFolder()
        {

        }

        public void UpdateNavigationRootTitleDisplayer()
        {
            if (AppDatabaseManager.Instance.GetProjectStructureData().Success())
            {
                string rootFolderName = AppDatabaseManager.Instance.GetProjectStructureData().data.GetRootFolder().name + "/";
                string formattedTitle = string.Empty;

                if (folderNavigationNameList.Count == 1)
                    formattedTitle = rootFolderName + folderNavigationNameList[0];
                else if (folderNavigationNameList.Count > 1)
                {
                    string folderDirectoryIndexedWidgetName = rootFolderName + folderNavigationNameList[0];

                    for (int i = 0; i < folderNavigationNameList.Count; i++)
                        if (i != 0)
                            folderDirectoryIndexedWidgetName += "/" + folderNavigationNameList[i];

                    formattedTitle = folderDirectoryIndexedWidgetName;
                }
                else
                    formattedTitle = AppDatabaseManager.Instance.GetProjectStructureData().data.GetRootFolder().name;

                if (formattedTitle.Length > folderNavigationDataPackets.widgetTitleCharacterLimit)
                {
                    string trimmedWidgetTitle = formattedTitle.Substring(formattedTitle.Length - folderNavigationDataPackets.widgetTitleCharacterLimit, folderNavigationDataPackets.widgetTitleCharacterLimit);
                    string output = rootFolderName + ".../" + trimmedWidgetTitle.Substring(trimmedWidgetTitle.IndexOf("/") + 1);
                    formattedTitle = output;
                }

                //if (ScreenUIManager.Instance.GetCurrentScreenData().value != null)
                //{
                //    ScreenUIManager.Instance.GetCurrentScreenData().value.SetUITextDisplayerValue(AppData.ScreenTextType.NavigationRootTitleDisplayer, formattedTitle);

                //    if (folderNavigationNameList.Count == 0)
                //        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.Return, AppData.InputUIState.Hidden);
                //    else
                //        ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.Return, AppData.InputUIState.Shown);
                //}
                //else
                //    LogWarning("Update Navigation Root Title Displayer Failed : Screen UI Manager Instance - Get Current Screen Data's Value Is Missing / Not Found.", this);
            }
            else
                Log(AppDatabaseManager.Instance.GetProjectStructureData().resultCode, AppDatabaseManager.Instance.GetProjectStructureData().result, this);
        }

        public AppData.SceneConfigDataPacket GetEmptyFolderDataPackets()
        {
            return emptyFolderDataPackets;
        }


        public AppData.SceneConfigDataPacket GetPagerNavigationWidgetDataPackets()
        {
            return pagerNavigationWidgetDataPackets;
        }

        public AppData.SceneConfigDataPacket GetScrollerNavigationWidgetDataPackets()
        {
            return scrollerNavigationWidgetDataPackets;
        }

        public void GetEmptyContentDataPacketsForScreen(AppData.ScreenType screenType, AppData.Folder contentFolder, Action<AppData.CallbackData<AppData.SceneConfigDataPacket>> callback)
        {
            AppData.CallbackData<AppData.SceneConfigDataPacket> callbackResults = new AppData.CallbackData<AppData.SceneConfigDataPacket>();

            AppData.Helpers.GetComponent(ScreenUIManager.Instance, validComponentCallbackResults => 
            {
                callbackResults.SetResult(validComponentCallbackResults);
            
                if(callbackResults.Success())
                {
                    callbackResults.SetResult(ScreenUIManager.Instance.HasCurrentScreen());

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(ScreenUIManager.Instance.HasCurrentScreen());

                        if (callbackResults.Success())
                        {
                            var currentScreen = ScreenUIManager.Instance.GetCurrentScreen().GetData();

                            callbackResults.SetResult(currentScreen.GetType());

                            if (callbackResults.Success())
                            {
                                if (screenType == currentScreen.GetType().GetData())
                                {
                                    AppData.SceneConfigDataPacket dataPackets = GetEmptyFolderDataPackets();

                                    dataPackets.GetReferencedScreenType().GetData().SetValue(screenType);
                                    dataPackets.isRootFolder = contentFolder.IsRootFolder();


                                    switch (screenType)
                                    {
                                        case AppData.ScreenType.ProjectCreationScreen:

                                            dataPackets.popUpMessage = (dataPackets.isRootFolder) ? "There Are No Projects Found" : "Project Is Empty";

                                            dataPackets.referencedActionButtonDataList = new List<AppData.ReferencedActionButtonData>
                                    {
                                            new AppData.ReferencedActionButtonData
                                            {
                                                title = (dataPackets.isRootFolder)? "Create New" : "Delete",
                                                type = AppData.InputActionButtonType.FolderActionButton,
                                                state = AppData.InputUIState.Enabled
                                            }
                                    };

                                            break;

                                        case AppData.ScreenType.ProjectDashboardScreen:

                                            dataPackets.popUpMessage = (dataPackets.isRootFolder) ? "There's No Content Found. Create New" : "Folder Is Empty";

                                            dataPackets.referencedActionButtonDataList = new List<AppData.ReferencedActionButtonData>
                                    {
                                            new AppData.ReferencedActionButtonData
                                            {
                                                title = (dataPackets.isRootFolder)? "Create New" : "Delete",
                                                type = AppData.InputActionButtonType.FolderActionButton,
                                                state = AppData.InputUIState.Enabled
                                            }
                                    };

                                            break;
                                    }

                                    callbackResults.result = $"Empty Content Data Packets For Screen Type : {screenType} Found.";
                                    callbackResults.data = dataPackets;
                                }
                                else
                                {
                                    callbackResults.result = $"Requested Data Packets For Screen Type : {screenType} Not Found - Scrren Type Mismatched - Current Found Screen Is Of Type : {ScreenUIManager.Instance.GetCurrentScreen().GetData().GetType().GetData()}";
                                    callbackResults.data = default;
                                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                }
                            }
                        }
                    }
                }
            });

            callback.Invoke(callbackResults);
        }

        #endregion
    }
}
