using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Com.RedicalGames.Filar
{
    public class SelectableManager : AppData.SingletonBaseComponent<SelectableManager>
    {
        #region Components

        [SerializeField]
        string selectableAssetsLayerName = "Selectable";

        [Space(5)]
        [SerializeField]
        float pressHoldSelectionDuration = 0.5f;

        [Space(5)]
        [SerializeField]
        float widgetsTransitionDelayDuration = 1.0f;

        [Space(5)]
        [SerializeField]
        List<AppData.RuntimeValue<float>> assetPanDragSpeedRuntimeList = new List<AppData.RuntimeValue<float>>();

        [Space(5)]
        [SerializeField]
        AppData.SceneConfigDataPacket inspectorModeDataPackets = new AppData.SceneConfigDataPacket();

        [Space(5)]
        [SerializeField]
        AppData.SceneConfigDataPacket focusedModeDataPackets = new AppData.SceneConfigDataPacket();

        [Space(5)]
        [SerializeField]
        List<SelectableSceneAssetHandler> selectedAssetList = new List<SelectableSceneAssetHandler>();

        [Space(5)]
        [SerializeField]
        AppData.SceneAssetInteractableMode sceneAssetInteractableMode;

        SelectableSceneAssetHandler selectedSceneAsset;

        [SerializeField]
        List<GameObject> selectableGameObjectList = new List<GameObject>();

        [SerializeField]
        AppData.ProjectStructureSelectionSystem projectStructureSelectionSystem = new AppData.ProjectStructureSelectionSystem();

        [SerializeField]
        Queue<AppData.UIWidgetInfo> focusedWidgetInfo = new Queue<AppData.UIWidgetInfo>();

        Dictionary<string, SelectableSceneAssetHandler> selectableSceneAssetsHandlerList = new Dictionary<string, SelectableSceneAssetHandler>();

        Queue<string> focusedWidgets = new Queue<string>();

        Queue<string> newPageItemSelectedWidget = new Queue<string>();

        // Dictionary<AppData.InputUIState, List<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>> focusedSelectionsInfoList = new Dictionary<AppData.InputUIState, List<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>>();

        [Space(5)]
        [SerializeField]
        string previouslyFocusedWidget;

        bool hasAssetSelected;
        bool isFingerOverAsset;

        [Space(5)]
        public bool SmoothTransitionToSelection;

        Coroutine assetWidgetRoutine;

        // Selection
        Coroutine onSelectionRoutine;
        Coroutine onShowOptionsWidgetRoutine;
        Coroutine onHideOptionsWidgetRoutine;

        Coroutine onDeselectRoutine;
        Coroutine onDeselectAllRoutine;

        #endregion

        #region Unity Callbacks

        void OnEnable() => OnEventSSubscriptions(true);

        void OnDisable() => OnEventSSubscriptions(false);

        void Start() => Init();

        #endregion

        #region Main

        void OnEventSSubscriptions(bool subscribed)
        {
            if (subscribed)
            {
                AppData.ActionEvents._OnScreenChangedEvent += OnScreenChangedEvent;
                AppData.ActionEvents._OnWidgetSelectionEvent += ActionEvents__OnWidgetSelectionEvent;
                AppData.ActionEvents._OnWidgetSelectionAdded += ActionEvents__OnWidgetSelectionAdded;
                AppData.ActionEvents._OnWidgetSelectionRemoved += ActionEvents__OnWidgetSelectionRemoved;
            }
            else
            {
                AppData.ActionEvents._OnScreenChangedEvent -= OnScreenChangedEvent;
                AppData.ActionEvents._OnWidgetSelectionEvent -= ActionEvents__OnWidgetSelectionEvent;
                AppData.ActionEvents._OnWidgetSelectionAdded -= ActionEvents__OnWidgetSelectionAdded;
                AppData.ActionEvents._OnWidgetSelectionRemoved -= ActionEvents__OnWidgetSelectionRemoved;
            }
        }


        void Init()
        {
            projectStructureSelectionSystem.OnSelection = ShowWidgetOnSelection;
            projectStructureSelectionSystem.OnDeselection = HideWidgetOnDeselection;
        }

        #region Events

        private void ActionEvents__OnWidgetSelectionRemoved()
        {
            AppDatabaseManager.Instance.GetLayoutViewType(layoutViewCallbackResults =>
            {
                if (layoutViewCallbackResults.Success())
                {
                    switch (layoutViewCallbackResults.data)
                    {
                        case AppData.LayoutViewType.ItemView:

                            ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ItemViewSelectionIcon);

                            break;

                        case AppData.LayoutViewType.ListView:

                            ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ListViewSelectionIcon);

                            break;
                    }
                }
                else
                    Log(layoutViewCallbackResults.resultCode, layoutViewCallbackResults.result, this);
            });
        }

        private void ActionEvents__OnWidgetSelectionAdded()
        {
            AppDatabaseManager.Instance.GetLayoutViewType(layoutViewCallbackResults =>
            {
                if (layoutViewCallbackResults.Success())
                {
                    switch (layoutViewCallbackResults.data)
                    {
                        case AppData.LayoutViewType.ItemView:

                            AppDatabaseManager.Instance.GetRefreshData().screenContainer.HasAllWidgetsSelected(selectionCallback =>
                            {
                                if (selectionCallback.Success())
                                    ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ItemViewDeselectionIcon);
                                else
                                    ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ItemViewSelectionIcon);
                            });

                            break;

                        case AppData.LayoutViewType.ListView:

                            AppDatabaseManager.Instance.GetRefreshData().screenContainer.HasAllWidgetsSelected(selectionCallback =>
                            {
                                if (selectionCallback.Success())
                                    ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ListViewDeselectionIcon);
                                else
                                    ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ListViewSelectionIcon);
                            });


                            break;
                    }
                }
                else
                    Log(layoutViewCallbackResults.resultCode, layoutViewCallbackResults.result, this);
            });
        }

        private void ActionEvents__OnWidgetSelectionEvent()
        {
            AppDatabaseManager.Instance.GetLayoutViewType(layoutViewCallbackResults =>
            {
                if (layoutViewCallbackResults.Success())
                {
                    switch (layoutViewCallbackResults.GetData())
                    {
                        case AppData.LayoutViewType.ItemView:

                            ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ItemViewSelectionIcon);

                            break;

                        case AppData.LayoutViewType.ListView:

                            ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.ListViewSelectionIcon);

                            break;
                    }
                }
                else
                    Log(layoutViewCallbackResults.resultCode, layoutViewCallbackResults.result, this);
            });
        }

        #endregion

        public void GetProjectStructureSelectionSystem(Action<AppData.CallbackData<AppData.ProjectStructureSelectionSystem>> callback)
        {
            AppData.CallbackData<AppData.ProjectStructureSelectionSystem> callbackResults = new AppData.CallbackData<AppData.ProjectStructureSelectionSystem>();

            AppData.Helpers.ProjectDataComponentValid(projectStructureSelectionSystem, validComponentCallbackResults => 
            {
                callbackResults.result = validComponentCallbackResults.result;
                callbackResults.resultCode = validComponentCallbackResults.resultCode;

                if (callbackResults.Success())
                {
                    if (projectStructureSelectionSystem != null)
                    {
                        callbackResults.result = $"Project Structure Selection System : {projectStructureSelectionSystem.name} Found.";
                        callbackResults.data = projectStructureSelectionSystem;
                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.result = "ProjectStructureSelectionSystem Not Found / Not Yet Initialized.";
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }
            });

            callback.Invoke(callbackResults);
        }

        public void AddToSelectableList(GameObject asset, AppData.ContentContainerType containerType, AppData.ScreenType screenType)
        {
            selectableGameObjectList = new List<GameObject>();
            selectableSceneAssetsHandlerList = new Dictionary<string, SelectableSceneAssetHandler>();

            if (!selectableGameObjectList.Contains(asset))
            {
                if (asset.GetComponent<MeshRenderer>())
                {
                    selectableGameObjectList.Add(asset);

                    if (asset.transform.childCount > 0)
                    {
                        for (int i = 0; i < asset.transform.childCount; i++)
                        {
                            selectableGameObjectList.Add(asset.transform.GetChild(i).gameObject);
                        }
                    }
                }
                else
                {
                    if (asset.transform.childCount > 0)
                    {
                        for (int i = 0; i < asset.transform.childCount; i++)
                        {
                            selectableGameObjectList.Add(asset.transform.GetChild(i).gameObject);
                        }
                    }
                }

                if (selectableGameObjectList.Count > 0)
                {
                    foreach (var selectableItem in selectableGameObjectList)
                    {

                        if (!selectableItem.GetComponent<SelectableSceneAssetHandler>())
                        {
                            SelectableSceneAssetHandler selectable = selectableItem.AddComponent<SelectableSceneAssetHandler>();
                            SceneAssetInteractableHandler interactable = selectableItem.AddComponent<SceneAssetInteractableHandler>();

                            selectable.SetPressHoldSelectionDuration(pressHoldSelectionDuration);

                            if (AppDatabaseManager.Instance)
                                interactable.SetContentContainer(AppDatabaseManager.Instance.GetSceneAssetsContainer(containerType, screenType));
                            else
                                Debug.LogWarning($"--> Scene Assets Manager Not Yet Initialized For : {this.gameObject.name}.");

                            if (RenderingSettingsManager.Instance)
                            {
                                selectable.SetEventCamera(RenderingSettingsManager.Instance.GetRenderCamera());
                                interactable.SetEventCamera(RenderingSettingsManager.Instance.GetRenderCamera());
                            }
                            else
                                Debug.LogWarning($"--> Rendering Manager Not Yet Initialized For : {this.gameObject.name}.");

                            // MeshCollider collider = selectableItem.AddComponent<MeshCollider>();
                            //collider.convex = true;

                            int selectabletLayer = LayerMask.NameToLayer(selectableAssetsLayerName);
                            selectableItem.layer = selectabletLayer;


                            if (selectableItem.transform.childCount > 0)
                            {
                                for (int i = 0; i < selectableItem.transform.childCount; i++)
                                {
                                    selectableItem.transform.GetChild(i).gameObject.layer = selectabletLayer;
                                }
                            }

                            // if (!selectableSceneAssetsHandlerList.ContainsValue(selectable))
                            selectableSceneAssetsHandlerList.Add(selectableItem.name, selectable);
                        }
                    }
                }
                else
                    Debug.LogWarning($"--> Failed To Get Selectable Game Objects From : {asset.name}");
            }
            else
                Debug.LogWarning($"--> Failed To Add Selectable Asset To List. Asset : {asset.name} Already Exist.");
        }

        public void UpdateSelectableAssetContainer(GameObject asset, AppData.ContentContainerType containerType, AppData.ScreenType screenType, Action<bool> callback)
        {
            bool assetUpdatedSuccessfully = false;

            selectableGameObjectList = new List<GameObject>();
            selectableSceneAssetsHandlerList = new Dictionary<string, SelectableSceneAssetHandler>();

            if (!selectableGameObjectList.Contains(asset))
            {

                if (asset.GetComponent<MeshRenderer>())
                {
                    selectableGameObjectList.Add(asset);

                    if (asset.transform.childCount > 0)
                    {
                        for (int i = 0; i < asset.transform.childCount; i++)
                        {
                            selectableGameObjectList.Add(asset.transform.GetChild(i).gameObject);
                        }
                    }
                }
                else
                {
                    if (asset.transform.childCount > 0)
                    {
                        for (int i = 0; i < asset.transform.childCount; i++)
                        {
                            selectableGameObjectList.Add(asset.transform.GetChild(i).gameObject);
                        }
                    }
                }

                if (selectableGameObjectList.Count > 0)
                {
                    foreach (var selectableItem in selectableGameObjectList)
                    {

                        if (selectableItem.GetComponent<SelectableSceneAssetHandler>())
                        {
                            if (selectableItem.GetComponent<SceneAssetInteractableHandler>() && selectableItem.GetComponent<SelectableSceneAssetHandler>())
                            {
                                SceneAssetInteractableHandler interactable = selectableItem.GetComponent<SceneAssetInteractableHandler>();
                                SelectableSceneAssetHandler selectable = selectableItem.GetComponent<SelectableSceneAssetHandler>();

                                selectable.SetPressHoldSelectionDuration(pressHoldSelectionDuration);

                                if (AppDatabaseManager.Instance)
                                    interactable.SetContentContainer(AppDatabaseManager.Instance.GetSceneAssetsContainer(containerType, screenType));
                                else
                                    Debug.LogWarning($"--> Scene Assets Manager Not Yet Initialized For : {this.gameObject.name}.");

                                if (RenderingSettingsManager.Instance)
                                {
                                    selectable.SetEventCamera(RenderingSettingsManager.Instance.GetRenderCamera());
                                    interactable.SetEventCamera(RenderingSettingsManager.Instance.GetRenderCamera());
                                }
                                else
                                    Debug.LogWarning($"--> Rendering Manager Not Yet Initialized For : {this.gameObject.name}.");

                                // MeshCollider collider = selectableItem.AddComponent<MeshCollider>();
                                //collider.convex = true;

                                int selectabletLayer = LayerMask.NameToLayer(selectableAssetsLayerName);
                                selectableItem.layer = selectabletLayer;


                                if (selectableItem.transform.childCount > 0)
                                {
                                    for (int i = 0; i < selectableItem.transform.childCount; i++)
                                    {
                                        selectableItem.transform.GetChild(i).gameObject.layer = selectabletLayer;
                                    }
                                }

                                assetUpdatedSuccessfully = true;
                                // if (!selectableSceneAssetsHandlerList.ContainsValue(selectable))
                                selectableSceneAssetsHandlerList.Add(selectableItem.name, selectable);
                            }
                            else
                                Debug.LogWarning($"--> RG_Unity - Selectable Item : {asset.name}'s SelectableSceneAssetHandler / SelectableSceneAssetHandler Missing.");
                        }
                        else
                            Debug.LogWarning($"--> RG_Unity - Selectable Item : {asset.name}'s SelectableSceneAssetHandler Missing.");
                    }
                }
                else
                    Debug.LogWarning($"--> Failed To Get Selectable Game Objects From : {asset.name}");
            }
            else
                Debug.LogWarning($"--> Failed To Add Selectable Asset To List. Asset : {asset.name} Already Exist.");


            callback.Invoke(assetUpdatedSuccessfully);
        }

        public void UpdateSelectableAssetContainers(GameObject asset, AppData.ContentContainerType containerType, AppData.ScreenType screenType, Action<bool> callback)
        {

            bool assetUpdatedSuccessfully = false;

            if (selectableGameObjectList.Count > 0)
            {
                if (asset.transform.childCount > 0)
                {
                    for (int i = 0; i < asset.transform.childCount; i++)
                    {
                        if (selectableGameObjectList.Contains(asset.transform.GetChild(i).gameObject))
                        {
                            GameObject selectableAsset = asset.transform.GetChild(i).gameObject;

                            if (selectableAsset != null)
                            {
                                if (selectableAsset.GetComponent<SelectableSceneAssetHandler>())
                                {
                                    SelectableSceneAssetHandler selectable = selectableAsset.GetComponent<SelectableSceneAssetHandler>();
                                    SceneAssetInteractableHandler interactable = selectableAsset.GetComponent<SceneAssetInteractableHandler>();

                                    if (AppDatabaseManager.Instance)
                                    {
                                        if (interactable != null)
                                        {
                                            interactable.SetContentContainer(AppDatabaseManager.Instance.GetSceneAssetsContainer(containerType, screenType));
                                        }
                                        else
                                            Debug.LogWarning("--> RG_Unity - UpdateSelectableAssetContainer : Scene Asset Interactable Handler Is Null.");
                                    }
                                    else
                                        Debug.LogWarning($"--> Scene Assets Manager Not Yet Initialized For : {this.gameObject.name}.");

                                    if (RenderingSettingsManager.Instance)
                                    {
                                        selectable.SetEventCamera(RenderingSettingsManager.Instance.GetRenderCamera());
                                        interactable.SetEventCamera(RenderingSettingsManager.Instance.GetRenderCamera());
                                    }
                                    else
                                        Debug.LogWarning($"--> Rendering Manager Not Yet Initialized For : {this.gameObject.name}.");

                                    assetUpdatedSuccessfully = true;
                                }
                                else
                                    Debug.LogWarning($"--> Update Selectable Asset Container Failed - Scene Asset Model : {asset.name} Is Not A Selectable Item - Selectable Scene Asset Handler & Scene Asset Interactable Handler  Missing.");
                            }
                            else
                                Debug.LogWarning($"--> Update Selectable Asset Container - Selectable Asset : {asset.name} Not Found.");
                        }
                        else
                        {
                            assetUpdatedSuccessfully = false;
                            break;
                        }
                    }
                }
                else
                    Debug.LogWarning($"--> Update Selectable Asset Container Failed - Scene Asset Model : {asset.name} Child Count Is 0.");

            }
            else
                Debug.LogWarning($"--> Failed To Get Selectable Game Objects From : {asset.name}");

            callback.Invoke(assetUpdatedSuccessfully);
        }

        #region On Widgets Selections

        public void Select(string selectionName, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>> callback = null)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults => 
            {
                if (projectSelectionCallbackResults.Success())
                {
                    projectSelectionCallbackResults.data.Select(selectionName, selectionType, selectionCallback =>
                    {
                        callbackResults = selectionCallback;
                    });
                }
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void Select(AppData.UIScreenWidget selection, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>> callback = null)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.Select(selection.name, selectionType, selectionCallback => { callbackResults = selectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void Select(AppData.SceneAsset selection, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>> callback = null)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.Select(selection.name, selectionType, selectionCallback => { callbackResults = selectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void Select(List<string> selectionNames, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>> callback = null)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.Select(selectionNames, selectionType, selectionCallback => { callbackResults = selectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void Select(AppData.UIScreenWidget selectable, AppData.SceneConfigDataPacket dataPackets, bool isInitialSelection = false)
        {
            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.Select(selectable, dataPackets, isInitialSelection);
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });
        }

        public void Selected(string name, AppData.FocusedSelectionType selectionType)
        {
            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.Select(name, selectionType);
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });
        }

        public void CacheSelection(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.CacheSelection(cacheCallback => { callbackResults = cacheCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void ClearSelectionCache(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.ClearSelectionCache(cacheCallback => { callbackResults = cacheCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public bool HasCachedSelectionInfo()
        {
            bool hasCachedInfo = false;

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    hasCachedInfo = projectSelectionCallbackResults.data.HasCachedSelectionInfo();
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            return hasCachedInfo;
        }

        public void GetCachedSelectionInfo(Action<AppData.CallbackDataList<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>> callback)
        {
            AppData.CallbackDataList<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>> callbackResults = new AppData.CallbackDataList<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.GetCachedSelectionInfo(getCacheCallback => { callbackResults = getCacheCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback.Invoke(callbackResults);
        }

        public void GetCachedSelectionInfoNameList(Action<AppData.CallbackDataList<string>> callback)
        {
            AppData.CallbackDataList<string> callbackResults = new AppData.CallbackDataList<string>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.GetCachedSelectionInfoNameList(getCacheCallback => { callbackResults = getCacheCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback.Invoke(callbackResults);
        }

        public void OnAddSelection(string selectionName, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>> callback = null)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.OnAddSelection(selectionName, selectionType, selectionCallback => { callbackResults = selectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void OnRemoveSelection(string selectionName, AppData.FocusedSelectionType selectionType = AppData.FocusedSelectionType.SelectedItem, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.OnRemoveSelection(selectionName, selectionType, removeSelectionCallback => { callbackResults = removeSelectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void DeselectAll()
        {
            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.DeselectAll();
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });
        }

        public AppData.FocusedSelectionType GetCurrentSelectionType()
        {
            AppData.FocusedSelectionType selectionType = AppData.FocusedSelectionType.Default;

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    selectionType = projectSelectionCallbackResults.data.GetCurrentSelectionType();
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            return selectionType;
        }

        public void HasFocusedSelectionInfo(string selectionName, Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.HasFocusedSelectionInfo(selectionName, hasSelectionCallback => { callbackResults = hasSelectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback.Invoke(callbackResults);
        }

        public void OnSetFocusedWidgetSelectionInfo(AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket> newSelectionInfo, bool isActiveSelection = true, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.OnSetFocusedWidgetSelectionInfo(newSelectionInfo, isActiveSelection, onSetFocusedWidgetSelectionCallback => { callbackResults = onSetFocusedWidgetSelectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void OnSetFocusedWidgetSelectionInfo(List<AppData.FocusedSelectionInfo<AppData.SceneConfigDataPacket>> newSelectionInfoList, AppData.FocusedSelectionType selectionType, bool isActiveSelection = true, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.OnSetFocusedWidgetSelectionInfo(newSelectionInfoList, selectionType, isActiveSelection, onSetFocusedWidgetSelectionCallback => { callbackResults = onSetFocusedWidgetSelectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public bool HasActiveSelection()
        {
            bool hasActiveContent = false;

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    hasActiveContent = projectSelectionCallbackResults.data.HasActiveSelections();
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            return hasActiveContent;
        }

        public void OnClearFocusedSelectionsInfo(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.OnClearFocusedSelectionsInfo(onClearFocusedSelectionsInfoCallback => { callbackResults = onClearFocusedSelectionsInfoCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void OnClearFocusedSelectionsInfo(bool resetWidgets, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.OnClearFocusedSelectionsInfo(resetWidgets, onClearFocusedSelectionsInfoCallback => { callbackResults = onClearFocusedSelectionsInfoCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void GetFocusedSelectionData(Action<AppData.CallbackData<AppData.FocusedSelectionData>> callback)
        {
            AppData.CallbackData<AppData.FocusedSelectionData> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionData>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.GetFocusedSelectionData(getFocusedSelectionCallback => { callbackResults = getFocusedSelectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public int GetFocusedSelectionDataCount()
        {
            int dataCount = 0;

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    dataCount = projectSelectionCallbackResults.data.GetFocusedSelectionDataCount();
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            return dataCount;
        }

        public void SetSelectionInfoState(List<AppData.UIScreenWidget> selectionList, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.UIScreenWidget>> callback = null)
        {
            AppData.CallbackData<AppData.UIScreenWidget> callbackResults = new AppData.CallbackData<AppData.UIScreenWidget>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.SetSelectionInfoState(selectionList, selectionType, getFolderStructureSelectionCallback => { callbackResults = getFolderStructureSelectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public AppData.FocusedSelectionType GetFocusedSelectionTypeFromState(AppData.InputUIState state)
        {
            AppData.FocusedSelectionType selectionType = AppData.FocusedSelectionType.Default;

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    selectionType = projectSelectionCallbackResults.data.GetFocusedSelectionTypeFromState(state);
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            return selectionType;
        }

        public void GetFocusedSelectionState(AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionStateInfo>> callback)
        {
            AppData.CallbackData<AppData.FocusedSelectionStateInfo> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionStateInfo>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    projectSelectionCallbackResults.data.GetFocusedSelectionState(selectionType, getFocusedSelectionCallback => { callbackResults = getFocusedSelectionCallback; });
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            callback?.Invoke(callbackResults);
        }

        public List<AppData.FocusedSelectionStateInfo> GetSelectionStates()
        {
            List<AppData.FocusedSelectionStateInfo> selectionInfo = new List<AppData.FocusedSelectionStateInfo>();

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                    selectionInfo = projectSelectionCallbackResults.data.GetSelectionStates();
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            return selectionInfo;
        }

        #endregion

        public void GetNewPageItemSelectedWidget(Action<AppData.CallbackData<string>> callback)
        {
            AppData.CallbackData<string> callbackResults = new AppData.CallbackData<string>();

            if (HasNewPageItemSelectedWidget())
            {
                callbackResults.result = "Success - NewPageItemSelectedWidget Found.";
                callbackResults.data = newPageItemSelectedWidget.Dequeue();
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Failed - There Is No newPageItemSelectedWidget Found.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public bool HasNewPageItemSelectedWidget()
        {
            return newPageItemSelectedWidget.Count > 0;
        }

        void OnScreenChangedEvent(AppData.ScreenType screenType)
        {
            switch (screenType)
            {
                case AppData.ScreenType.ContentImportExportScreen:

                    sceneAssetInteractableMode = AppData.SceneAssetInteractableMode.Rotation;

                    break;

                case AppData.ScreenType.ProjectDashboardScreen:

                    // Reset Preview Pose.
                    AppData.ActionEvents.OnTransitionSceneEventCamera(focusedModeDataPackets);
                    ScreenUIManager.Instance.GetCurrentScreen().GetData().ShowWidget(focusedModeDataPackets);

                    sceneAssetInteractableMode = AppData.SceneAssetInteractableMode.Orbit;

                    break;

                case AppData.ScreenType.ARViewScreen:

                    sceneAssetInteractableMode = AppData.SceneAssetInteractableMode.All;

                    break;
            }
        }

        public void ClearSelection()
        {
            if (selectedSceneAsset != null)
            {
                if (RenderingSettingsManager.Instance)
                    RenderingSettingsManager.Instance.RevertSelectedAssetMaterial(selectedSceneAsset);
                else
                    Debug.LogWarning("--> Rendering Manager Not Yet Initialized.");

                hasAssetSelected = false;
            }
        }

        public bool HasSelection()
        {
            return selectedSceneAsset != null;
        }

        public AppData.ProjectStructureSelectionSystem GetFolderStructureSelection()
        {
            return projectStructureSelectionSystem;
        }

        public void SetSelectedSceneAsset(SelectableSceneAssetHandler selectableSceneAsset, bool eventCameraTransition = true)
        {

            if (selectableSceneAsset)
            {
                if (selectableSceneAsset != selectedSceneAsset)
                {
                    ClearSelection();
                }

                selectedSceneAsset = selectableSceneAsset;
                AddToSelectedList(selectableSceneAsset);

                hasAssetSelected = true;

                if (ScreenUIManager.Instance)
                {
                    ScreenUIManager.Instance.GetCurrentScreen().GetData().HideScreenWidget(AppData.WidgetType.AssetImportWidget, inspectorModeDataPackets);
                    inspectorModeDataPackets.sceneAssetMode = AppData.EventCameraState.InspectorMode;

                    if (assetWidgetRoutine != null)
                        assetWidgetRoutine = null;

                    assetWidgetRoutine = StartCoroutine(EnterInspectionMode());
                }
                else
                    Debug.LogWarning("--> Screen UI Manager Not Yet Initialized.");

                AppDatabaseManager.Instance.SetCurrentSceneMode(AppData.SceneMode.EditMode);
            }
            else
            {
                hasAssetSelected = false;

                if (ScreenUIManager.Instance)
                {
                    ScreenUIManager.Instance.GetCurrentScreen().GetData().HideScreenWidget(AppData.WidgetType.SceneAssetPropertiesWidget, inspectorModeDataPackets);
                    inspectorModeDataPackets.sceneAssetMode = AppData.EventCameraState.FocusedMode;

                    if (eventCameraTransition)
                    {
                        if (assetWidgetRoutine != null)
                            assetWidgetRoutine = null;

                        Debug.LogError("--> This Is Some how Called. Da Fuq");
                        assetWidgetRoutine = StartCoroutine(EnterFocusedMode());
                    }


                    //ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(selectionDataPackets);
                }
                else
                    Debug.LogWarning("--> Screen UI Manager Not Yet Initialized.");

                //if (ScreenUIManager.Instance)
                //{
                //    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.SceneAssetPropertiesWidget, selectionDataPackets);
                //}
                //else
                //    Debug.LogWarning("--> Screen UI Manager Not Yet Initialized.");


            }

        }

        IEnumerator EnterInspectionMode()
        {
            if (ScreenUIManager.Instance)
            {
                yield return new WaitUntil(() => ScreenUIManager.Instance.GetCurrentScreen().GetData().GeWidget(AppData.WidgetType.AssetImportWidget).IsTransitionState() == false);
                yield return new WaitForSecondsRealtime(widgetsTransitionDelayDuration);

                AppData.ActionEvents.OnTransitionSceneEventCamera(inspectorModeDataPackets);
                ScreenUIManager.Instance.GetCurrentScreen().GetData().ShowWidget(inspectorModeDataPackets);
            }
            else
                Debug.LogWarning("--> Screen UI Manager Not Yet Initialized.");

            yield return null;

            StopCoroutine(assetWidgetRoutine);
        }

        IEnumerator EnterFocusedMode()
        {
            if (ScreenUIManager.Instance)
            {
                yield return new WaitUntil(() => ScreenUIManager.Instance.GetCurrentScreen().GetData().GeWidget(AppData.WidgetType.SceneAssetPropertiesWidget).IsTransitionState() == false);
                AppData.ActionEvents.OnTransitionSceneEventCamera(focusedModeDataPackets);

                yield return new WaitForSecondsRealtime(widgetsTransitionDelayDuration);
                ScreenUIManager.Instance.GetCurrentScreen().GetData().ShowWidget(focusedModeDataPackets);
            }
            else
                Debug.LogWarning("--> Screen UI Manager Not Yet Initialized.");

            yield return null;

            StopCoroutine(assetWidgetRoutine);
        }

        public bool HasAssetSelected()
        {
            return hasAssetSelected;
        }

        public SelectableSceneAssetHandler GetSelectedSceneAsset()
        {
            return selectedSceneAsset;
        }

        public void AddToSelectedList(SelectableSceneAssetHandler selectableSceneAsset)
        {
            if (!selectedAssetList.Contains(selectableSceneAsset))
            {
                selectedAssetList.Add(selectableSceneAsset);

                foreach (var selectedAsset in selectedAssetList)
                {
                    if (selectedAsset == selectableSceneAsset)
                    {
                        if (RenderingSettingsManager.Instance)
                            RenderingSettingsManager.Instance.SetSelectedAssetMaterial(selectedAsset);
                        else
                            Debug.LogWarning("--> Rendering Manager Not Yet Initialized.");
                    }
                }
            }
        }

        public void ClearSelectionList()
        {
            if (selectedAssetList.Count > 0)
            {
                selectedAssetList.Clear();
                selectedAssetList = new List<SelectableSceneAssetHandler>();

                if (hasAssetSelected)
                    hasAssetSelected = false;

                if (selectedSceneAsset)
                    selectedSceneAsset = null;

                if (selectedAssetList.Count == 0)
                    AppData.ActionEvents.OnClearAllAssetSelectionEvent();
            }
            else
                Debug.LogWarning("--> Selected Asset List Is Null / Empty.");
        }

        public List<SelectableSceneAssetHandler> GetSelectionList()
        {
            return selectedAssetList;
        }

        public void OnPressAndHoldIn()
        {

        }

        public void OnDoubleTap(CallbackContext callback)
        {
            if (callback.performed)
            {
                AppData.ActionEvents.OnScreenDoubleTapInput();
            }
        }

        public AppData.RuntimeValue<float> GetAssetPanDragSpeedRuntimeValue(AppData.BuildType runtimeType, AppData.RuntimeExecution valueType)
        {
            if (assetPanDragSpeedRuntimeList.Count > 0)
            {
                AppData.RuntimeValue<float> runtimeValue = assetPanDragSpeedRuntimeList.Find((x) => x.runtimeType == runtimeType && x.valueType == valueType);
                return runtimeValue;
            }
            else
            {
                Debug.LogError("--> assetPanDragSpeedRuntimeList Is Not Yet Initialized. Returning New Empty value");
                return new AppData.RuntimeValue<float>();
            }
        }

        public AppData.SceneAssetInteractableMode GetSceneAssetInteractableMode()
        {
            return sceneAssetInteractableMode;
        }

        public void SetFocusedWidgetInfo(AppData.UIWidgetInfo widgetInfo)
        {
            focusedWidgetInfo.Clear();

            if (focusedWidgetInfo.Count == 0)
                focusedWidgetInfo.Enqueue(widgetInfo);
        }

        public AppData.UIWidgetInfo GetFocusedWidgetInfoData()
        {
            AppData.UIWidgetInfo infoData = focusedWidgetInfo.Dequeue();

            AppData.UIWidgetInfo newInfoData = new AppData.UIWidgetInfo
            {
                widgetName = infoData.widgetName,
                dimensions = infoData.dimensions,
                position = infoData.position,
                selectionState = infoData.selectionState
            };

            focusedWidgetInfo.Enqueue(newInfoData);

            return infoData;
        }

        public void GetFocusedWidgetInfo(Action<AppData.CallbackData<AppData.UIWidgetInfo>> callback)
        {
            AppData.CallbackData<AppData.UIWidgetInfo> callbackResults = new AppData.CallbackData<AppData.UIWidgetInfo>();

            if (focusedWidgetInfo.Count > 0)
            {
                callbackResults.data = focusedWidgetInfo.Dequeue();
                callbackResults.result = $"GetFocusedWidgetInfo Success : Focused To Widget : {callbackResults.data.GetWidgetName()}.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "GetFocusedWidgetInfo Failed : There Is No Focused Widget Assigned.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public bool HasFocusedWidgetInfo()
        {
            return focusedWidgetInfo.Count == 1;
        }

        public void SetFingerOverAsset(bool isFingerOverAsset) => this.isFingerOverAsset = isFingerOverAsset;

        public bool GetIsFingerOverAsset()
        {
            return isFingerOverAsset;
        }

        #region On UI Selection

        void ShowWidgetOnSelection(AppData.SceneConfigDataPacket dataPackets) => StartCoroutine(ShowWidgetOnSelectionAsync(dataPackets));

        public void SetScreenUIStateOnSelection(AppData.InputUIState state)
        {
            switch (ScreenUIManager.Instance.GetCurrentScreen().GetData().GetType().GetData())
            {
                case AppData.ScreenType.ProjectDashboardScreen:

                    //ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewAsset, state);
                    //ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewFolderButton, state);
                    //ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputUIState.Disabled);
                    //ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldState(AppData.InputFieldActionType.AssetSearchField, state);

                    break;
            }
        }

        IEnumerator ShowWidgetOnSelectionAsync(AppData.SceneConfigDataPacket dataPackets)
        {
            yield return new WaitUntil(() => HasActiveSelection());

            UpdateOptionsWidgetOnActionEvent();

            if (onShowOptionsWidgetRoutine != null)
            {
                StopCoroutine(onShowOptionsWidgetRoutine);
                onShowOptionsWidgetRoutine = null;
            }

            if (onShowOptionsWidgetRoutine == null)
                onShowOptionsWidgetRoutine = StartCoroutine(OnShowSelectionOptions(dataPackets));
        }

        void UpdateOptionsWidgetOnActionEvent()
        {
            #region Pin Button State

            GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
            {
                if (projectSelectionCallbackResults.Success())
                {
                    int selectionCount = projectSelectionCallbackResults.data.GetCurrentSelections().Count;
                    var pinData = HasPinnedSelection(projectSelectionCallbackResults.data.GetCurrentSelections());

                    if (pinData.disableButton)
                    {
                        ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.PinButton, AppData.InputUIState.Disabled);
                        return;
                    }
                    else
                    {
                        if (pinData.pinned)
                        {
                            ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.PinButton, AppData.InputUIState.Enabled);
                            ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.PinButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.PinDisabledIcon);
                        }
                        else
                        {
                            ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.PinButton, AppData.InputUIState.Enabled);
                            ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.PinButton, AppData.UIImageDisplayerType.InputIcon, AppData.UIImageType.PinEnabledIcon);
                        }
                    }

                    #endregion

                    #region Edit Button State

                    if (selectionCount == 1)
                    {
                        ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Edit, AppData.InputUIState.Enabled);
                        ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Delete, AppData.InputUIState.Enabled);
                    }
                    else if (selectionCount > 1)
                    {
                        ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Edit, AppData.InputUIState.Disabled);
                        ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Delete, AppData.InputUIState.Enabled);
                    }
                    else
                    {
                        ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Edit, AppData.InputUIState.Disabled);
                        ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.PinButton, AppData.InputUIState.Disabled);
                        ScreenUIManager.Instance.GetCurrentScreen().GetData().GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Delete, AppData.InputUIState.Disabled);
                        //ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Hide, AppData.InputUIState.Disabled);
                    }
                }
                else
                    Log(projectSelectionCallbackResults.resultCode, projectSelectionCallbackResults.result, this);
            });

            #endregion
        }

        void HideWidgetOnDeselection()
        {
            if (!HasActiveSelection())
            {
                OnClearFocusedSelectionsInfo();

                if (onHideOptionsWidgetRoutine != null)
                {
                    StopCoroutine(onHideOptionsWidgetRoutine);
                    onHideOptionsWidgetRoutine = null;
                }

                if (onHideOptionsWidgetRoutine == null)
                    onHideOptionsWidgetRoutine = StartCoroutine(OnDeselectionUISatetUpdateAsync());
            }
        }

        public (bool pinned, bool disableButton, int selectionCount, int pinnedCount) HasPinnedSelection(List<AppData.UIScreenWidget> selected)
        {
            bool hasPinned = false;
            bool disableButton = false;

            int pinnedCount = 0;
            int selectionCount = selected.Count;

            if (selectionCount > 0)
            {
                foreach (var selection in selected)
                    if (selection.GetDefaultUIWidgetActionState() == AppData.DefaultUIWidgetActionState.Pinned)
                        pinnedCount++;

                if (pinnedCount > 0 && pinnedCount == selectionCount)
                {
                    hasPinned = true;
                    disableButton = false;
                }
                else if (pinnedCount > 0 && pinnedCount != selectionCount)
                {
                    hasPinned = true;
                    disableButton = true;
                }
                else
                {
                    hasPinned = false;
                    disableButton = false;
                }
            }

            return (hasPinned, disableButton, selectionCount, pinnedCount);
        }

        public void AddSelectables(List<AppData.UIScreenWidget> selectables)
        {
            if (onSelectionRoutine != null)
            {
                StopCoroutine(onSelectionRoutine);
                onSelectionRoutine = null;
            }

            if (onSelectionRoutine == null)
                onSelectionRoutine = StartCoroutine(OnSelection(selectables));
        }

        public void OnDeselect(AppData.UIScreenWidget deselectedWidget)
        {
            if (onDeselectRoutine != null)
            {
                StopCoroutine(onDeselectRoutine);
                onDeselectRoutine = null;
            }

            if (onDeselectRoutine == null)
                onDeselectRoutine = StartCoroutine(OnDeselectAsync(deselectedWidget));
        }

        public void OnDeselectAll()
        {
            if (onDeselectAllRoutine != null)
            {
                StopCoroutine(onDeselectAllRoutine);
                onDeselectAllRoutine = null;
            }

            if (onDeselectAllRoutine == null)
                onDeselectAllRoutine = StartCoroutine(OnDeselectAllAsync());
        }

        IEnumerator OnSelection(List<AppData.UIScreenWidget> widgets)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => AppDatabaseManager.Instance.GetRefreshData().screenContainer.ContainerRefreshed() == true);

            projectStructureSelectionSystem.AddSelectables(widgets);
        }

        IEnumerator OnDeselectAsync(AppData.UIScreenWidget deselectedWidget)
        {
            yield return new WaitForEndOfFrame();
            projectStructureSelectionSystem.Deselect(deselectedWidget);

            OnTriggerFocusToWidgets(true);
        }

        IEnumerator OnDeselectAllAsync()
        {
            yield return new WaitForEndOfFrame();
            projectStructureSelectionSystem.DeselectAll();
        }

        IEnumerator OnShowSelectionOptions(AppData.SceneConfigDataPacket dataPackets)
        {
            yield return new WaitForSeconds(AppDatabaseManager.Instance.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetShowDelayValue).value);

            if (ScreenUIManager.Instance != null)
            {
                if (ScreenUIManager.Instance.GetCurrentScreen().GetData().GetType().GetData() == dataPackets.GetReferencedScreenType().GetData().GetValue().GetData())
                    ScreenUIManager.Instance.GetCurrentScreen().GetData().ShowWidget(dataPackets);

                SetScreenUIStateOnSelection(AppData.InputUIState.Disabled);
            }
            else
                Debug.LogWarning("--> Select Failed : ScreenUIManager.Instance Is Not Yet Initialized.");

            OnTriggerFocusToWidgets(true);
        }

        void OnTriggerFocusToWidgets(bool smoothTransitionToSelection)
        {
            UpdateOptionsWidgetOnActionEvent();

            Debug.LogError("==> CLear All Selections - Check, Has Something To Do With Ambushed Selection Data.");

            //if (folderStructureSelection.currentSelections.Count > 0)
            //    SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.ClearAllFocusedWidgetInfo();

            if (AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetPaginationViewType() == AppData.PaginationViewType.Pager)
            {
                GetNewPageItemSelectedWidget(onNewPageItemSelectedWidgetCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(onNewPageItemSelectedWidgetCallback.resultCode))
                        AppData.ActionEvents.OnNavigateAndFocusToSelectionEvent(onNewPageItemSelectedWidgetCallback.data);
                });
            }

            if (AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetPaginationViewType() == AppData.PaginationViewType.Scroller)
            {
                if (HasActiveSelection())
                {
                    if (GetFocusedSelectionDataCount() > 1)
                    {
                        Vector2 widgetsPosition = Vector2.zero;
                        List<float> positions = new List<float>();

                        switch (AppDatabaseManager.Instance.GetRefreshData().screenContainer.GetContainerOrientation())
                        {
                            case AppData.OrientationType.Vertical:

                                foreach (var selection in projectStructureSelectionSystem.GetCurrentSelections())
                                {
                                    if (!positions.Contains(selection.GetWidgetLocalPosition().y))
                                    {
                                        positions.Add(selection.GetWidgetLocalPosition().y);
                                        widgetsPosition.y += selection.GetWidgetLocalPosition().y;
                                    }
                                }

                                Vector2 targetPosY = new Vector2(0.0f, widgetsPosition.y / positions.Count);

                                AppData.ActionEvents.ScrollAndFocusToSelectionEvent(targetPosY, smoothTransitionToSelection);

                                break;

                            case AppData.OrientationType.Horizontal:

                                foreach (var selection in projectStructureSelectionSystem.GetCurrentSelections())
                                {
                                    if (!positions.Contains(selection.GetWidgetLocalPosition().x))
                                    {
                                        positions.Add(selection.GetWidgetLocalPosition().x);
                                        widgetsPosition.x = selection.GetWidgetLocalPosition().x;
                                    }
                                }

                                Vector2 targetPosX = new Vector2(0.0f, widgetsPosition.y / positions.Count);

                                AppData.ActionEvents.ScrollAndFocusToSelectionEvent(targetPosX, smoothTransitionToSelection);

                                break;
                        }
                    }
                    else
                    {
                        Vector2 widgetPosition = Vector2.zero;

                        foreach (var selection in projectStructureSelectionSystem.GetCurrentSelections())
                            widgetPosition += selection.GetWidgetLocalPosition();

                        AppData.ActionEvents.ScrollAndFocusToSelectionEvent(widgetPosition, SmoothTransitionToSelection);
                    }
                }
            }
        }

        IEnumerator OnDeselectionUISatetUpdateAsync()
        {
            yield return new WaitForSeconds(AppDatabaseManager.Instance.GetDefaultExecutionValue(AppData.RuntimeExecution.ScreenWidgetHideDelayValue).value);

            if (ScreenUIManager.Instance != null)
                ScreenUIManager.Instance.GetCurrentScreen().GetData().HideScreenWidget(AppData.WidgetType.FileSelectionOptionsWidget);
            else
                Debug.LogWarning("--> Select Failed : ScreenUIManager.Instance Is Not Yet Initialized.");

            SetScreenUIStateOnSelection(AppData.InputUIState.Enabled);
        }

        #endregion

        #endregion
    }
}
