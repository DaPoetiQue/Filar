using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Com.RedicalGames.Filar
{
    public class SelectableManager : AppMonoBaseClass
    {
        #region Static

        private static SelectableManager _instance;

        public static SelectableManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<SelectableManager>();

                return _instance;
            }
        }

        #endregion

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
        AppData.SceneDataPackets inspectorModeDataPackets = new AppData.SceneDataPackets();

        [Space(5)]
        [SerializeField]
        AppData.SceneDataPackets focusedModeDataPackets = new AppData.SceneDataPackets();

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
        AppData.FolderStructureSelectionSystem folderStructureSelection = new AppData.FolderStructureSelectionSystem();

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
            Debug.Log($"--> Selectable Manager Initialized From : {this.gameObject.name}");

            folderStructureSelection.OnSelection = ShowWidgetOnSelection;
            folderStructureSelection.OnDeselection = HideWidgetOnDeselection;
        }

        #region Events

        private void ActionEvents__OnWidgetSelectionRemoved()
        {
            switch (SceneAssetsManager.Instance.GetLayoutViewType())
            {
                case AppData.LayoutViewType.ItemView:

                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ItemViewSelectionIcon);

                    break;

                case AppData.LayoutViewType.ListView:

                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ListViewSelectionIcon);

                    break;
            }
        }

        private void ActionEvents__OnWidgetSelectionAdded()
        {
            switch (SceneAssetsManager.Instance.GetLayoutViewType())
            {
                case AppData.LayoutViewType.ItemView:

                    SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.HasAllWidgetsSelected(selectionCallback => 
                    {
                        if(selectionCallback.Success())
                            ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ItemViewDeselectionIcon);
                        else
                            ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ItemViewSelectionIcon);
                    });

                    break;

                case AppData.LayoutViewType.ListView:

                    SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.HasAllWidgetsSelected(selectionCallback =>
                    {
                        if (selectionCallback.Success())
                            ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ListViewDeselectionIcon);
                        else
                            ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ListViewSelectionIcon);
                    });


                    break;
            }
        }

        private void ActionEvents__OnWidgetSelectionEvent()
        {
            switch(SceneAssetsManager.Instance.GetLayoutViewType())
            {
                case AppData.LayoutViewType.ItemView:

                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ItemViewSelectionIcon);

                    break;

                case AppData.LayoutViewType.ListView:

                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.SelectionOptionsButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.ListViewSelectionIcon);

                    break;
            }
        }

        #endregion

        public AppData.FolderStructureSelectionSystem GetFolderStructureSelectionData()
        {
            return folderStructureSelection;
        }

        public void AddToSelectableList(GameObject asset, AppData.ContentContainerType containerType, AppData.UIScreenType screenType)
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

                            if (SceneAssetsManager.Instance)
                                interactable.SetContentContainer(SceneAssetsManager.Instance.GetSceneAssetsContainer(containerType, screenType));
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

        public void UpdateSelectableAssetContainer(GameObject asset, AppData.ContentContainerType containerType, AppData.UIScreenType screenType, Action<bool> callback)
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

                                if (SceneAssetsManager.Instance)
                                    interactable.SetContentContainer(SceneAssetsManager.Instance.GetSceneAssetsContainer(containerType, screenType));
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

        public void UpdateSelectableAssetContainers(GameObject asset, AppData.ContentContainerType containerType, AppData.UIScreenType screenType, Action<bool> callback)
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

                                    if (SceneAssetsManager.Instance)
                                    {
                                        if (interactable != null)
                                        {
                                            interactable.SetContentContainer(SceneAssetsManager.Instance.GetSceneAssetsContainer(containerType, screenType));
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

        public void Select(string selectionName, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>> callback = null)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>();

            GetFolderStructureSelectionData().Select(selectionName, selectionType, selectionCallback => 
            {
                callbackResults = selectionCallback; 
            });

            callback?.Invoke(callbackResults);
        }

        public void Select(AppData.UIScreenWidget<AppData.SceneDataPackets> selection, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>> callback = null)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>();

            GetFolderStructureSelectionData().Select(selection.name, selectionType, selectionCallback => { callbackResults = selectionCallback; });

            callback?.Invoke(callbackResults);
        }

        public void Select(AppData.SceneAsset selection, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>> callback = null)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>();

            GetFolderStructureSelectionData().Select(selection.name, selectionType, selectionCallback => { callbackResults = selectionCallback; });

            callback?.Invoke(callbackResults);
        }

        public void Select(List<string> selectionNames, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>> callback = null)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>();

            GetFolderStructureSelectionData().Select(selectionNames, selectionType, selectionCallback => { callbackResults = selectionCallback; });

            callback?.Invoke(callbackResults);
        }

        public void Select(AppData.UIScreenWidget<AppData.SceneDataPackets> selectable, AppData.SceneDataPackets dataPackets, bool isInitialSelection = false) => GetFolderStructureSelectionData().Select(selectable, dataPackets, isInitialSelection);

        public void Selected(string name, AppData.FocusedSelectionType selectionType) => GetFolderStructureSelectionData().Select(name, selectionType);

        public void CacheSelection(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetFolderStructureSelectionData().CacheSelection(cacheCallback => { callbackResults = cacheCallback; });

            callback?.Invoke(callbackResults);
        }

        public void ClearSelectionCache(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetFolderStructureSelectionData().ClearSelectionCache(cacheCallback => { callbackResults = cacheCallback; });

            callback?.Invoke(callbackResults);
        }

        public bool HasCachedSelectionInfo()
        {
            return GetFolderStructureSelectionData().HasCachedSelectionInfo();
        }

        public void GetCachedSelectionInfo(Action<AppData.CallbackDatas<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>> callback)
        {
            AppData.CallbackDatas<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> callbackResults = new AppData.CallbackDatas<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>();

            GetFolderStructureSelectionData().GetCachedSelectionInfo(getCacheCallback => { callbackResults = getCacheCallback; });

            callback.Invoke(callbackResults);
        }

        public void GetCachedSelectionInfoNameList(Action<AppData.CallbackDatas<string>> callback)
        {
            AppData.CallbackDatas<string> callbackResults = new AppData.CallbackDatas<string>();

            GetFolderStructureSelectionData().GetCachedSelectionInfoNameList(getCacheCallback => { callbackResults = getCacheCallback; });

            callback.Invoke(callbackResults);
        }

        public void OnAddSelection(string selectionName, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>> callback = null)
        {
            AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>>();

            GetFolderStructureSelectionData().OnAddSelection(selectionName, selectionType, selectionCallback => { callbackResults = selectionCallback; });

            callback?.Invoke(callbackResults);
        }

        public void OnRemoveSelection(string selectionName, AppData.FocusedSelectionType selectionType = AppData.FocusedSelectionType.SelectedItem, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetFolderStructureSelectionData().OnRemoveSelection(selectionName, selectionType, removeSelectionCallback => { callbackResults = removeSelectionCallback; });

            callback?.Invoke(callbackResults);
        }

        public void DeselectAll() => GetFolderStructureSelectionData().DeselectAll();

        public AppData.FocusedSelectionType GetCurrentSelectionType()
        {
            return GetFolderStructureSelectionData().GetCurrentSelectionType();
        }

        public void HasFocusedSelectionInfo(string selectionName, Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetFolderStructureSelectionData().HasFocusedSelectionInfo(selectionName, hasSelectionCallback => { callbackResults = hasSelectionCallback; });

            callback.Invoke(callbackResults);
        }

        public void OnSetFocusedWidgetSelectionInfo(AppData.FocusedSelectionInfo<AppData.SceneDataPackets> newSelectionInfo, bool isActiveSelection = true, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetFolderStructureSelectionData().OnSetFocusedWidgetSelectionInfo(newSelectionInfo, isActiveSelection, onSetFocusedWidgetSelectionCallback => { callbackResults = onSetFocusedWidgetSelectionCallback; });

            callback?.Invoke(callbackResults);
        }

        public void OnSetFocusedWidgetSelectionInfo(List<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> newSelectionInfoList, AppData.FocusedSelectionType selectionType, bool isActiveSelection = true, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetFolderStructureSelectionData().OnSetFocusedWidgetSelectionInfo(newSelectionInfoList, selectionType, isActiveSelection, onSetFocusedWidgetSelectionCallback => { callbackResults = onSetFocusedWidgetSelectionCallback; });

            callback?.Invoke(callbackResults);
        }

        public bool HasActiveSelection()
        {
            return GetFolderStructureSelectionData().HasActiveSelections();
        }

        public void OnClearFocusedSelectionsInfo(Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetFolderStructureSelectionData().OnClearFocusedSelectionsInfo(onClearFocusedSelectionsInfoCallback => { callbackResults = onClearFocusedSelectionsInfoCallback; });

            callback?.Invoke(callbackResults);
        }

        public void OnClearFocusedSelectionsInfo(bool resetWidgets, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetFolderStructureSelectionData().OnClearFocusedSelectionsInfo(resetWidgets, onClearFocusedSelectionsInfoCallback => { callbackResults = onClearFocusedSelectionsInfoCallback; });

            callback?.Invoke(callbackResults);
        }

        public void GetFocusedSelectionData(Action<AppData.CallbackData<AppData.FocusedSelectionData>> callback)
        {
            AppData.CallbackData<AppData.FocusedSelectionData> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionData>();

            GetFolderStructureSelectionData().GetFocusedSelectionData(getFocusedSelectionCallback => { callbackResults = getFocusedSelectionCallback; });

            callback?.Invoke(callbackResults);
        }

        public int GetFocusedSelectionDataCount()
        {
            return GetFolderStructureSelectionData().GetFocusedSelectionDataCount();
        }

        public void SetSelectionInfoState(List<AppData.UIScreenWidget<AppData.SceneDataPackets>> selectionList, AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.UIScreenWidget<AppData.SceneDataPackets>>> callback = null)
        {
            AppData.CallbackData<AppData.UIScreenWidget<AppData.SceneDataPackets>> callbackResults = new AppData.CallbackData<AppData.UIScreenWidget<AppData.SceneDataPackets>>();

            GetFolderStructureSelectionData().SetSelectionInfoState(selectionList, selectionType, getFolderStructureSelectionCallback => { callbackResults = getFolderStructureSelectionCallback; });

            callback?.Invoke(callbackResults);
        }

        public AppData.FocusedSelectionType GetFocusedSelectionTypeFromState(AppData.InputUIState state)
        {
            return GetFolderStructureSelectionData().GetFocusedSelectionTypeFromState(state);
        }

        public void GetFocusedSelectionState(AppData.FocusedSelectionType selectionType, Action<AppData.CallbackData<AppData.FocusedSelectionStateInfo>> callback)
        {
            AppData.CallbackData<AppData.FocusedSelectionStateInfo> callbackResults = new AppData.CallbackData<AppData.FocusedSelectionStateInfo>();

            GetFolderStructureSelectionData().GetFocusedSelectionState(selectionType, getFocusedSelectionCallback => { callbackResults = getFocusedSelectionCallback; });

            callback?.Invoke(callbackResults);
        }

        public List<AppData.FocusedSelectionStateInfo> GetSelectionStates()
        {
            return GetFolderStructureSelectionData().GetSelectionStates();
        }

        #endregion

        public void GetNewPageItemSelectedWidget(Action<AppData.CallbackData<string>> callback)
        {
            AppData.CallbackData<string> callbackResults = new AppData.CallbackData<string>();

            if (HasNewPageItemSelectedWidget())
            {
                callbackResults.results = "Success - NewPageItemSelectedWidget Found.";
                callbackResults.data = newPageItemSelectedWidget.Dequeue();
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Failed - There Is No newPageItemSelectedWidget Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public bool HasNewPageItemSelectedWidget()
        {
            return newPageItemSelectedWidget.Count > 0;
        }

        void OnScreenChangedEvent(AppData.UIScreenType screenType)
        {
            // Reset Preview Pose.
            AppData.ActionEvents.OnTransitionSceneEventCamera(focusedModeDataPackets);
            ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(focusedModeDataPackets);

            switch (screenType)
            {
                case AppData.UIScreenType.AssetCreationScreen:

                    sceneAssetInteractableMode = AppData.SceneAssetInteractableMode.Rotation;

                    break;

                case AppData.UIScreenType.ProjectViewScreen:

                    sceneAssetInteractableMode = AppData.SceneAssetInteractableMode.Orbit;

                    break;

                case AppData.UIScreenType.ARViewScreen:

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

        public AppData.FolderStructureSelectionSystem GetFolderStructureSelection()
        {
            return folderStructureSelection;
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
                    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.AssetImportWidget, inspectorModeDataPackets);
                    inspectorModeDataPackets.sceneAssetMode = AppData.EventCameraState.InspectorMode;

                    if (assetWidgetRoutine != null)
                        assetWidgetRoutine = null;

                    assetWidgetRoutine = StartCoroutine(EnterInspectionMode());
                }
                else
                    Debug.LogWarning("--> Screen UI Manager Not Yet Initialized.");

                SceneAssetsManager.Instance.SetCurrentSceneMode(AppData.SceneMode.EditMode);
            }
            else
            {
                hasAssetSelected = false;

                if (ScreenUIManager.Instance)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.SceneAssetPropertiesWidget, inspectorModeDataPackets);
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
                yield return new WaitUntil(() => ScreenUIManager.Instance.GetCurrentScreenData().value.GeWidget(AppData.WidgetType.AssetImportWidget).IsTransitionState() == false);
                yield return new WaitForSecondsRealtime(widgetsTransitionDelayDuration);

                AppData.ActionEvents.OnTransitionSceneEventCamera(inspectorModeDataPackets);
                ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(inspectorModeDataPackets);
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
                yield return new WaitUntil(() => ScreenUIManager.Instance.GetCurrentScreenData().value.GeWidget(AppData.WidgetType.SceneAssetPropertiesWidget).IsTransitionState() == false);
                AppData.ActionEvents.OnTransitionSceneEventCamera(focusedModeDataPackets);

                yield return new WaitForSecondsRealtime(widgetsTransitionDelayDuration);
                ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(focusedModeDataPackets);
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

        public AppData.RuntimeValue<float> GetAssetPanDragSpeedRuntimeValue(AppData.BuildType runtimeType, AppData.RuntimeValueType valueType)
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
                callbackResults.results = $"GetFocusedWidgetInfo Success : Focused To Widget : {callbackResults.data.GetWidgetName()}.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "GetFocusedWidgetInfo Failed : There Is No Focused Widget Assigned.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
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

        void ShowWidgetOnSelection(AppData.SceneDataPackets dataPackets) => StartCoroutine(ShowWidgetOnSelectionAsync(dataPackets));

        public void SetScreenUIStateOnSelection(AppData.InputUIState state)
        {
            switch (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType())
            {
                case AppData.UIScreenType.ProjectViewScreen:

                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewAsset, state);
                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.CreateNewFolderButton, state);
                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionDropdownState(AppData.InputUIState.Disabled);
                    ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionInputFieldState(AppData.InputFieldActionType.AssetSearchField, state);

                    break;
            }
        }

        IEnumerator ShowWidgetOnSelectionAsync(AppData.SceneDataPackets dataPackets)
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

            int selectionCount = GetFolderStructureSelectionData().GetCurrentSelections().Count;
            var pinData = HasPinnedSelection(GetFolderStructureSelectionData().GetCurrentSelections());

            if (pinData.disableButton)
            {
                ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.PinButton, AppData.InputUIState.Disabled);
                return;
            }
            else
            {
                if (pinData.pinned)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.PinButton, AppData.InputUIState.Enabled);
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.PinButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.PinDisabledIcon);
                }
                else
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.PinButton, AppData.InputUIState.Enabled);
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonUIImageValue(AppData.InputActionButtonType.PinButton, AppData.UIImageDisplayerType.ButtonIcon, AppData.UIImageType.PinEnabledIcon);
                }
            }

            #endregion

            #region Edit Button State

            if (selectionCount == 1)
            {
                ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Edit, AppData.InputUIState.Enabled);
                ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Delete, AppData.InputUIState.Enabled);
            }
            else if (selectionCount > 1)
            {
                ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Edit, AppData.InputUIState.Disabled);
                ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Delete, AppData.InputUIState.Enabled);
            }
            else
            {
                ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Edit, AppData.InputUIState.Disabled);
                ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.PinButton, AppData.InputUIState.Disabled);
                ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Delete, AppData.InputUIState.Disabled);
                //ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.FileSelectionOptionsWidget).SetActionButtonState(AppData.InputActionButtonType.Hide, AppData.InputUIState.Disabled);
            }

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

        public (bool pinned, bool disableButton, int selectionCount, int pinnedCount) HasPinnedSelection(List<AppData.UIScreenWidget<AppData.SceneDataPackets>> selected)
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

        public void AddSelectables(List<AppData.UIScreenWidget<AppData.SceneDataPackets>> selectables)
        {
            if (onSelectionRoutine != null)
            {
                StopCoroutine(onSelectionRoutine);
                onSelectionRoutine = null;
            }

            if (onSelectionRoutine == null)
                onSelectionRoutine = StartCoroutine(OnSelection(selectables));
        }

        public void OnDeselect(AppData.UIScreenWidget<AppData.SceneDataPackets> deselectedWidget)
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

        IEnumerator OnSelection(List<AppData.UIScreenWidget<AppData.SceneDataPackets>> widgets)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.ContainerRefreshed() == true);

            folderStructureSelection.AddSelectables(widgets);
        }

        IEnumerator OnDeselectAsync(AppData.UIScreenWidget<AppData.SceneDataPackets> deselectedWidget)
        {
            yield return new WaitForEndOfFrame();
            folderStructureSelection.Deselect(deselectedWidget);

            OnTriggerFocusToWidgets(true);
        }

        IEnumerator OnDeselectAllAsync()
        {
            yield return new WaitForEndOfFrame();
            folderStructureSelection.DeselectAll();
        }

        IEnumerator OnShowSelectionOptions(AppData.SceneDataPackets dataPackets)
        {
            yield return new WaitForSeconds(SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.ScreenWidgetShowDelayValue).value);

            if (ScreenUIManager.Instance != null)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() == dataPackets.screenType)
                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(dataPackets);

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

            if (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetPaginationViewType() == AppData.PaginationViewType.Pager)
            {
                GetNewPageItemSelectedWidget(onNewPageItemSelectedWidgetCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(onNewPageItemSelectedWidgetCallback.resultsCode))
                        AppData.ActionEvents.OnNavigateAndFocusToSelectionEvent(onNewPageItemSelectedWidgetCallback.data);
                });
            }

            if (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetPaginationViewType() == AppData.PaginationViewType.Scroller)
            {
                if (HasActiveSelection())
                {
                    if (GetFocusedSelectionDataCount() > 1)
                    {
                        Vector2 widgetsPosition = Vector2.zero;
                        List<float> positions = new List<float>();

                        switch (SceneAssetsManager.Instance.GetWidgetsRefreshData().widgetsContainer.GetContainerOrientation())
                        {
                            case AppData.OrientationType.Vertical:

                                foreach (var selection in folderStructureSelection.GetCurrentSelections())
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

                                foreach (var selection in folderStructureSelection.GetCurrentSelections())
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

                        foreach (var selection in folderStructureSelection.GetCurrentSelections())
                            widgetPosition += selection.GetWidgetLocalPosition();

                        AppData.ActionEvents.ScrollAndFocusToSelectionEvent(widgetPosition, SmoothTransitionToSelection);
                    }
                }
            }
        }

        IEnumerator OnDeselectionUISatetUpdateAsync()
        {
            yield return new WaitForSeconds(SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.ScreenWidgetHideDelayValue).value);

            if (ScreenUIManager.Instance != null)
                ScreenUIManager.Instance.GetCurrentScreenData().value.HideScreenWidget(AppData.WidgetType.FileSelectionOptionsWidget);
            else
                Debug.LogWarning("--> Select Failed : ScreenUIManager.Instance Is Not Yet Initialized.");

            SetScreenUIStateOnSelection(AppData.InputUIState.Enabled);
        }

        #endregion

        #endregion
    }
}
