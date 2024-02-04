using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Com.RedicalGames.Filar
{
    public class SelectableSceneAssetHandler : AppData.Selectable
    {
        #region Components

        [SerializeField]
        Camera eventCamera;

        [SerializeField]
        List<AppData.RendererMaterial> selectableAssetRenderMaterialList = new List<AppData.RendererMaterial>();

        Material[] defaultMaterial;

        float pressHoldDuration = 0.0f;

        float currentPressHoldDuration = 0.0f;

        bool isFingerDown = false;

        bool tappingAsset = false;

        bool isFingerMoved = false;

        bool isSelected = false;

        //SelectableManager selectableManager;
        AppInputsManager inputsManager;

        SelectableSceneAssetHandler selectableHandler;

        [SerializeField]
        MeshRenderer meshRenderer;

        Vector2 fingerDownInitPos;

        #endregion

        #region Main

        protected override void OnInitialization()
        {
            meshRenderer = this.GetComponent<MeshRenderer>();

            if (meshRenderer)
            {
                defaultMaterial = meshRenderer.sharedMaterials;

                if (defaultMaterial.Length == 0)
                    Debug.LogWarning($"--> Failed To Get Default Asset Materials For Mesh : {meshRenderer.name}.");
            }
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnScreenDoubleTapInput += OnScreenDoubleTapActionEvent;
                AppData.ActionEvents._OnScreenPressAndHoldInput += OnScreenPressAndHoldActionEvent;
                AppData.ActionEvents._OnClearAllAssetSelectionEvent += OnClearAllAssetSelectionEvent;
            }
            else
            {
                AppData.ActionEvents._OnScreenDoubleTapInput -= OnScreenDoubleTapActionEvent;
                AppData.ActionEvents._OnScreenPressAndHoldInput -= OnScreenPressAndHoldActionEvent;
                AppData.ActionEvents._OnClearAllAssetSelectionEvent -= OnClearAllAssetSelectionEvent;
            }
        }

        protected override void OnFingerDown(Finger finger)
        {
            if (AppDatabaseManager.Instance)
            {
                isFingerDown = true;
                isFingerMoved = false;

                fingerDownInitPos = finger.screenPosition;

                Ray ray = eventCamera.ScreenPointToRay(finger.screenPosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
                {

                    selectableHandler = hitInfo.transform.GetComponent<SelectableSceneAssetHandler>();

                    if (selectableHandler != null)
                    {
                        tappingAsset = true;
                        SelectableManager.Instance.SetFingerOverAsset(tappingAsset);
                    }
                }
                else
                {
                    tappingAsset = false;
                    SelectableManager.Instance.SetFingerOverAsset(tappingAsset);
                }
            }
            else
                Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
        }

        void OnScreenDoubleTapActionEvent()
        {
            if (AppDatabaseManager.Instance)
            {
                if (AppDatabaseManager.Instance.GetCurrentSceneMode() == AppData.SceneMode.EditMode)
                {
                    if (selectableHandler && !tappingAsset)
                    {
                        if (SelectableManager.Instance)
                        {
                            if (SelectableManager.Instance.HasSelection())
                            {

                                SelectableManager.Instance.SetSelectedSceneAsset(null);
                                SelectableManager.Instance.ClearSelection();
                                SelectableManager.Instance.ClearSelectionList();

                                if (AppDatabaseManager.Instance.GetCurrentSceneMode() != AppData.SceneMode.EditMode)
                                    AppDatabaseManager.Instance.SetCurrentSceneMode(AppData.SceneMode.EditMode);
                            }
                            else
                                Debug.Log("--> No Selection");
                        }
                        else
                            Debug.LogWarning($"--> Selectable Manager Not Yet Initialized For : {this.gameObject}");
                    }

                    if (SelectableManager.Instance)
                    {
                        if (selectableHandler && tappingAsset && SelectableManager.Instance.HasAssetSelected())
                        {
                            SelectableManager.Instance.AddToSelectedList(selectableHandler);

                            if (AppDatabaseManager.Instance.GetCurrentSceneMode() != AppData.SceneMode.EditMode)
                                AppDatabaseManager.Instance.SetCurrentSceneMode(AppData.SceneMode.EditMode);
                        }
                    }
                }
                else
                    return;
            }
            else
                Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
        }

        void OnScreenPressAndHoldActionEvent()
        {
            if (!isFingerMoved)
                Debug.LogError("--> Pressed $ Hold..........");
        }

        protected override void UpdateSelectableStateAction()
        {
            if (AppDatabaseManager.Instance)
            {
                if (AppDatabaseManager.Instance.GetCurrentSceneMode() == AppData.SceneMode.EditMode)
                {
                    if (isFingerDown)
                    {
                        if (currentPressHoldDuration < pressHoldDuration)
                            currentPressHoldDuration += Time.deltaTime;
                        else
                            OnAssetSelected();
                    }
                }
                else
                    return;
            }
            else
                Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
        }

        void OnAssetSelected()
        {
            if (selectableHandler)
            {
                if (selectableHandler)
                {
                    if (SelectableManager.Instance)
                    {
                        SelectableManager.Instance.ClearSelection();
                        SelectableManager.Instance.ClearSelectionList();
                        SelectableManager.Instance.SetSelectedSceneAsset(selectableHandler);
                    }
                    else
                        Debug.LogWarning($"--> Selectable Manager Not Yet Initialized For : {this.gameObject.name}");
                }
                else
                    Debug.LogWarning($"--> Not A Selectable Object.");

                isFingerDown = false;
            }
        }


        protected override void OnFingerMoved(Finger finger)
        {
            if (isFingerDown)
            {

                float distance = (fingerDownInitPos - finger.screenPosition).magnitude;

                if (distance > 100.0f)
                {
                    isFingerDown = false;
                    currentPressHoldDuration = 0.0f;

                    isFingerMoved = true;
                }
            }
        }

        protected override void OnFingerUp(Finger finger)
        {
            isFingerDown = false;
            isFingerMoved = false;
            currentPressHoldDuration = 0.0f;
            fingerDownInitPos = Vector3.zero;

            tappingAsset = false;
            SelectableManager.Instance.SetFingerOverAsset(tappingAsset);
        }

        void OnClearAllAssetSelectionEvent()
        {
            if (isSelected)
                meshRenderer.sharedMaterials = defaultMaterial;
        }

        public void SetEventCamera(Camera camera, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(camera, "Camera", "Set Event Camera Failed - Camera Parameter Value Is missing / Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                eventCamera = camera;
                callbackResults.result = $"Set Event Camera Success - Scene Event Camera Has Been Set To : {camera.name}.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        public MeshRenderer GetSelectableAssetRenderer()
        {
            if (!meshRenderer)
                meshRenderer = this.GetComponent<MeshRenderer>();

            return meshRenderer;
        }

        public void SetPressHoldSelectionDuration(float duration)
        {
            pressHoldDuration = duration;
        }

        public void SetIsSelectedState(bool selectionState)
        {
            isSelected = selectionState;
        }

        public bool GetIsSelectedState()
        {
            return isSelected;
        }

        public void Select()
        {

        }

        public void DeSelect()
        {

        }

        #endregion
    }
}
