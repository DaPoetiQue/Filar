
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SceneAssetInteractableHandler : AppData.Interactable
    {
        #region Components

        [SerializeField]
        Camera eventCamera;

        [Space(5)]
        [SerializeField]
        Transform containerTargetPoseReference;

        [SerializeField]
        LayerMask sceneAssetInteractableLayer;

        Transform contentContainer;


        Vector3 previousPosition = Vector3.zero;
        bool interactable = true;

        #endregion

        #region Unity Callbacks

        void OnEnable() => RegisterEventListeners(true);
        void OnDisable() => RegisterEventListeners(false);

        void Start() => Init();

        void Update() => OnContentOrbitUpdate();

        #endregion

        #region Main

        void Init()
        {
            if (eventCamera == null || contentContainer == null)
            {
                Debug.LogError("--> Event Cam or Content Container Missing");
                return;
            }

            if (contentContainer != null)
                containerTargetPoseReference = contentContainer.GetChild(0);
        }

        void RegisterEventListeners(bool register)
        {
            if (register)
                AppData.ActionEvents._OnScreenViewStateChangedEvent += ActionEvents__OnScreenViewStateChangedEvent;
            else
                AppData.ActionEvents._OnScreenViewStateChangedEvent -= ActionEvents__OnScreenViewStateChangedEvent;
        }

        private void ActionEvents__OnScreenViewStateChangedEvent(AppData.ScreenViewState state)
        {
            if (ScreenUIManager.Instance != null)
            {
                if (ScreenUIManager.Instance.GetCurrentScreenData().value.GetUIScreenType() != AppData.ScreenType.ContentImportExportScreen)
                    return;

                if (state == AppData.ScreenViewState.None)
                    return;

                if (state == AppData.ScreenViewState.Overlayed)
                    interactable = false;
                else
                    interactable = true;
            }
            else
                Debug.LogWarning("--> RG_Unity - ActionEvents__OnScreenViewStateChangedEvent Failed : Screen UI Manager Instance Is Not Yet Initialized.");
        }

        public void SetEventCamera(Camera camera)
        {
            eventCamera = camera;
        }

        public void SetContentContainer(Transform container)
        {
            contentContainer = container;
        }


        void OnContentOrbitUpdate()
        {
            if (interactable)
            {
                if (SelectableManager.Instance.GetSceneAssetInteractableMode() == AppData.SceneAssetInteractableMode.Rotation)
                {
                    if (!SelectableManager.Instance.GetIsFingerOverAsset())
                        return;

                    if (Input.GetMouseButtonDown(0))
                    {
                        previousPosition = eventCamera.ScreenToViewportPoint(Input.mousePosition);
                    }

                    Quaternion previousAngle = contentContainer.rotation;

                    if (Input.GetMouseButton(0))
                    {
                        if (gameObject.layer == sceneAssetInteractableLayer)
                        {
                            if (AppManager.Instance.IsRuntime())
                            {
                                float xRot = Input.GetAxis("Mouse X") * SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.InspectorModePanSpeed).value;
                                float yRot = Input.GetAxis("Mouse Y") * SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.InspectorModePanSpeed).value;

                                Vector3 rightVect = Vector3.Cross(eventCamera.transform.up, containerTargetPoseReference.position - eventCamera.transform.position);
                                Vector3 upVect = Vector3.Cross(containerTargetPoseReference.position - eventCamera.transform.position, rightVect);

                                contentContainer.rotation = Quaternion.Slerp(contentContainer.rotation, Quaternion.AngleAxis(-xRot, upVect) * contentContainer.rotation, SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.InspectorModePanSpeed).value * Time.smoothDeltaTime);
                                contentContainer.rotation = Quaternion.Slerp(contentContainer.rotation, Quaternion.AngleAxis(yRot, rightVect) * contentContainer.rotation, SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.InspectorModePanSpeed).value * Time.smoothDeltaTime);

                                previousPosition = eventCamera.ScreenToViewportPoint(Input.mousePosition);

                                if (previousAngle != contentContainer.rotation)
                                {
                                    AppData.ActionEvents.OnScreenTogglableStateEvent(AppData.TogglableWidgetType.ResetAssetModelRotationButton, true);
                                }
                            }
                            else
                            {
                                float xRot = Input.GetAxis("Mouse X") * SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Editor, AppData.RuntimeExecution.InspectorModePanSpeed).value;
                                float yRot = Input.GetAxis("Mouse Y") * SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Editor, AppData.RuntimeExecution.InspectorModePanSpeed).value;

                                Vector3 rightVect = Vector3.Cross(eventCamera.transform.up, containerTargetPoseReference.position - eventCamera.transform.position);
                                Vector3 upVect = Vector3.Cross(containerTargetPoseReference.position - eventCamera.transform.position, rightVect);

                                contentContainer.rotation = Quaternion.Slerp(contentContainer.rotation, Quaternion.AngleAxis(-xRot, upVect) * contentContainer.rotation, SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Editor, AppData.RuntimeExecution.InspectorModePanSpeed).value * Time.smoothDeltaTime);
                                contentContainer.rotation = Quaternion.Slerp(contentContainer.rotation, Quaternion.AngleAxis(yRot, rightVect) * contentContainer.rotation, SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Editor, AppData.RuntimeExecution.InspectorModePanSpeed).value * Time.smoothDeltaTime);

                                previousPosition = eventCamera.ScreenToViewportPoint(Input.mousePosition);

                                if (previousAngle != contentContainer.rotation)
                                {
                                    AppData.ActionEvents.OnScreenTogglableStateEvent(AppData.TogglableWidgetType.ResetAssetModelRotationButton, true);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
