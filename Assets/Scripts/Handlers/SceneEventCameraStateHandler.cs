using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

namespace Com.RedicalGames.Filar
{
    public class SceneEventCameraStateHandler : AppMonoBaseClass
    {
        #region Components

        [SerializeField]
        List<AppData.SceneEventCamera> sceneEventCameraList = new List<AppData.SceneEventCamera>();

        [Space(5)]
        [SerializeField]
        AppData.SceneEventCameraType defaultActiveEventCameraType;

        [Space(5)]
        [SerializeField]
        Vector3 targetTransitionPosition;

        [Space(5)]
        [SerializeField]
        float transitionalSpeed = 5.0f;

        [Space(3)]
        [SerializeField]
        float previewCamHeight = 1.0f;

        [Space(3)]
        [SerializeField]
        float previewCamDistance = -2.0f;

        [Space(3)]
        [SerializeField]
        bool inverseOrbitX = false;

        [Space(3)]
        [SerializeField]
        Transform camPoseTarget;

        [Space(3)]
        [SerializeField]
        Transform previewFocusPoint = null;

        [SerializeField]
        AppData.SceneEventCamera currentEventCamera;

        Vector3 defaultCamPositionReference;
        Quaternion defaultCamRotationReference;

        Vector3 defaultTransitionPosition;
        Vector3 previousPosition;

        bool canTransition = false;
        bool transitionToTarget = false;
        bool isDragging = false;
        bool interactable = true;


        #endregion

        #region Unity Callbacks

        void OnEnable() => RegisterEventListeners(true);

        void OnDisable() => RegisterEventListeners(false);

        void Start() => Init();

        void Update()
        {
            if (!interactable)
                return;

            OnEventCameraTransition();

            if (SelectableManager.Instance.GetSceneAssetInteractableMode() == AppData.SceneAssetInteractableMode.Orbit)
                OnContentOrbitUpdate();

            OnCameraResetPoseTransition();
        }

        #endregion

        #region Main

        void Init()
        {
            OnSetCurrentEventCamera(defaultActiveEventCameraType);

            if (camPoseTarget != null)
            {
                defaultCamPositionReference = camPoseTarget.position;
                defaultCamRotationReference = camPoseTarget.rotation;
            }
            else
                Debug.LogWarning("--> Cam Pose Target Missing / Null.");

            if (currentEventCamera.value)
            {
                defaultTransitionPosition = currentEventCamera.GetCameraTransform().position;
            }
            else
                Debug.LogWarning("--> Current Event Camera Not Set.");

            InitializeCameraList();
        }

        void InitializeCameraList()
        {
            if (sceneEventCameraList.Count > 0)
            {
                foreach (AppData.SceneEventCamera eventCam in sceneEventCameraList)
                {
                    if (eventCam.value != null)
                    {
                        eventCam.Init();
                    }
                    else
                        Debug.LogWarning($"-->RG_Unity - Init Failed : Scene Event Camera Value Missing For : {eventCam.name}");
                }
            }
        }

        void RegisterEventListeners(bool subscribed)
        {
            if (subscribed)
            {
                AppData.ActionEvents._OnTransitionSceneEventCamera += OnTransitionSceneEventCamera;
                AppData.ActionEvents._OnSetCurrentActiveSceneCameraEvent += OnSetCurrentEventCamera;
                AppData.ActionEvents._OnResetSceneAssetPreviewPoseEvent += OnResetSceneAssetPreviewPoseEvent;
                AppData.ActionEvents._OnResetCameraToDefaultrPoseEvent += OnResetCameraToDefaultrPoseEvent;
                AppData.ActionEvents._OnScreenChangedEvent += ActionEvents__OnScreenChangedEvent;
                AppData.ActionEvents._OnScreenViewStateChangedEvent += ActionEvents__OnScreenViewStateChangedEvent;
            }
            else
            {
                AppData.ActionEvents._OnTransitionSceneEventCamera -= OnTransitionSceneEventCamera;
                AppData.ActionEvents._OnSetCurrentActiveSceneCameraEvent -= OnSetCurrentEventCamera;
                AppData.ActionEvents._OnResetSceneAssetPreviewPoseEvent -= OnResetSceneAssetPreviewPoseEvent;
                AppData.ActionEvents._OnResetCameraToDefaultrPoseEvent -= OnResetCameraToDefaultrPoseEvent;
                AppData.ActionEvents._OnScreenChangedEvent -= ActionEvents__OnScreenChangedEvent;
                AppData.ActionEvents._OnScreenViewStateChangedEvent -= ActionEvents__OnScreenViewStateChangedEvent;
            }
        }


        private void ActionEvents__OnScreenViewStateChangedEvent(AppData.ScreenViewState state)
        {
            if (state == AppData.ScreenViewState.None)
                return;

            if (state == AppData.ScreenViewState.Overlayed)
                interactable = false;
            else
                interactable = true;
        }

        private void ActionEvents__OnScreenChangedEvent(AppData.ScreenType tValue)
        {
            OnResetCameraPose();
        }

        void OnResetCameraToDefaultrPoseEvent()
        {
            // Reset Preview Pose.
            OnResetCameraPose();
        }

        void OnSetCurrentEventCamera(AppData.SceneEventCameraType eventCameraType)
        {
            if (sceneEventCameraList.Count > 0)
            {
                foreach (var eventCamera in sceneEventCameraList)
                {
                    if (eventCamera.value)
                    {
                        if (eventCamera.eventCameraType == eventCameraType)
                        {
                            if (eventCameraType == AppData.SceneEventCameraType.ARViewCamera)
                            {
                                if (AppManager.Instance.IsRuntime())
                                {

                                    if (AppManager.Instance)
                                        AppManager.Instance.CameraUsagePermissionRequest();

                                    if (AppManager.Instance.PermissionGranted(Permission.Camera))
                                    {
                                        eventCamera.EnableCamera();
                                        currentEventCamera = eventCamera;

                                        AppData.SceneAssetPose assetPose = new AppData.SceneAssetPose
                                        {
                                            position = currentEventCamera.value.transform.position,
                                            rotation = currentEventCamera.value.transform.rotation,
                                            scale = currentEventCamera.value.transform.localScale
                                        };

                                        eventCamera.SetDefaultCameraPose(assetPose);
                                    }
                                    else
                                        Debug.LogWarning("--> No Permissions Granted For Camera.");

                                    ARSceneManager.Instance.EnterARMode();
                                }
                                else
                                {
                                    eventCamera.EnableCamera();
                                    currentEventCamera = eventCamera;

                                    AppData.SceneAssetPose assetPose = new AppData.SceneAssetPose
                                    {
                                        position = currentEventCamera.value.transform.position,
                                        rotation = currentEventCamera.value.transform.rotation,
                                        scale = currentEventCamera.value.transform.localScale
                                    };

                                    eventCamera.SetDefaultCameraPose(assetPose);
                                }
                            }
                            else
                            {
                                eventCamera.EnableCamera();
                                currentEventCamera = eventCamera;

                                AppData.SceneAssetPose assetPose = new AppData.SceneAssetPose
                                {
                                    position = currentEventCamera.value.transform.position,
                                    rotation = currentEventCamera.value.transform.rotation,
                                    scale = currentEventCamera.value.transform.localScale
                                };

                                eventCamera.SetDefaultCameraPose(assetPose);

                                ARSceneManager.Instance.ExitARMode();
                            }
                        }
                        else
                            eventCamera.DisableCamera();

                    }
                    else
                        Debug.LogWarning($"--> Scene Event Camera : {eventCamera.name}'s Value Required.");
                }
            }
            else
                Debug.LogWarning("--> There Are Not Scene Event Cameras Assigned.");
        }

        void OnTransitionSceneEventCamera(AppData.SceneConfigDataPacket dataPackets)
        {
            transitionToTarget = true;

            if (dataPackets.overrideSceneAssetTargetPosition)
                targetTransitionPosition = dataPackets.sceneAssetTransitionalTargetPosition;

            if (dataPackets.overrideSceneAssetTransitionSpeed)
                transitionalSpeed = dataPackets.sceneAssetTransitionSpeed;

            canTransition = dataPackets.canTransitionSceneAsset;
        }

        void OnEventCameraTransition()
        {
            if (!canTransition || !currentEventCamera.value)
                return;

            if (transitionToTarget)
            {
                Vector3 screenPoint = currentEventCamera.GetCameraTransform().localPosition;
                screenPoint = Vector3.Lerp(screenPoint, targetTransitionPosition, transitionalSpeed * Time.deltaTime);

                currentEventCamera.GetCameraTransform().localPosition = screenPoint;

                float distance = (currentEventCamera.GetCameraTransform().localPosition - targetTransitionPosition).sqrMagnitude;

                if (distance <= 0.1f)
                    canTransition = false;
            }
            else
            {
                Vector3 screenPoint = currentEventCamera.GetCameraTransform().localPosition;
                screenPoint = Vector3.Lerp(screenPoint, defaultTransitionPosition, transitionalSpeed * Time.deltaTime);

                currentEventCamera.GetCameraTransform().localPosition = screenPoint;

                float distance = (currentEventCamera.GetCameraTransform().localPosition - defaultTransitionPosition).sqrMagnitude;

                if (distance <= 0.1f)
                    canTransition = false;
            }
        }

        void OnResetSceneAssetPreviewPoseEvent(AppData.AssetModeType assetModeType)
        {
            OnResetCameraPose();
            AppData.ActionEvents.OnResetContaineToDefaultPoseEvent();
        }

        void OnContentOrbitUpdate()
        {
            if (!SelectableManager.Instance.GetIsFingerOverAsset())
                return;

            if (Input.GetMouseButtonDown(0))
            {
                previousPosition = currentEventCamera.value.ScreenToViewportPoint(Input.mousePosition);
            }

            Quaternion previousAngle = currentEventCamera.value.transform.rotation;

            if (Input.GetMouseButton(0))
            {
                Vector3 direction = previousPosition - currentEventCamera.value.ScreenToViewportPoint(Input.mousePosition);

                camPoseTarget.position = previewFocusPoint.position;

                camPoseTarget.Rotate(new Vector3(1.0f, 0.0f, 0.0f), direction.y * 180.0f);
                camPoseTarget.Rotate(new Vector3(0.0f, 1.0f, 0.0f), (inverseOrbitX == true) ? direction.x : -direction.x * 180.0f, Space.World);
                camPoseTarget.Translate(new Vector3(0.0f, previewCamHeight, previewCamDistance));

                previousPosition = currentEventCamera.value.ScreenToViewportPoint(Input.mousePosition);

                isDragging = true;
            }

            if (Input.GetMouseButtonUp(0) && isDragging == true)
            {
                isDragging = false;
            }

            if (currentEventCamera.value.transform.position != camPoseTarget.position)
            {
                currentEventCamera.value.transform.position = Vector3.Lerp(currentEventCamera.value.transform.position, camPoseTarget.position, SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.PreviewModeOrbitSpeed).value * Time.smoothDeltaTime);
            }

            if (currentEventCamera.value.transform.rotation != camPoseTarget.rotation)
            {
                currentEventCamera.value.transform.rotation = Quaternion.Slerp(currentEventCamera.value.transform.rotation, camPoseTarget.rotation, SelectableManager.Instance.GetAssetPanDragSpeedRuntimeValue(AppData.BuildType.Runtime, AppData.RuntimeExecution.PreviewModeOrbitSpeed).value * Time.smoothDeltaTime);
            }

            if (previousAngle != currentEventCamera.value.transform.rotation)
            {
                AppData.ActionEvents.OnScreenTogglableStateEvent(AppData.TogglableWidgetType.ResetAssetModelRotationButton, true);
            }
        }

        void OnCameraResetPoseTransition()
        {
            //if (transitionCameraOnPoseReset)
            //{

            //    if (currentEventCamera.value.transform.position != defaultCurrentEventCameraPosition)
            //    {
            //        currentEventCamera.value.transform.position = Vector3.Lerp(-currentEventCamera.value.transform.position, defaultCurrentEventCameraPosition, inertia * Time.smoothDeltaTime);
            //    }

            //    if (currentEventCamera.value.transform.rotation != defaultCurrentEventCameraRotation)
            //    {
            //        currentEventCamera.value.transform.rotation = Quaternion.Slerp(currentEventCamera.value.transform.rotation, defaultCurrentEventCameraRotation, inertia * Time.smoothDeltaTime);
            //    }

            //    float distance = (currentEventCamera.value.transform.position - defaultCurrentEventCameraPosition).sqrMagnitude;

            //    if (distance <= 0.1f)
            //    {
            //        OnResetCameraPose();

            //        AppData.ActionEvents.OnSceneModelPoseResetEvent();
            //        transitionCameraOnPoseReset = false;
            //    }
            //}
            //else
            //    return;
        }

        void OnResetCameraPose()
        {
            //currentEventCamera.value.transform.position = defaultCurrentEventCameraPosition;
            //currentEventCamera.value.transform.rotation = defaultCurrentEventCameraRotation;

            AppData.SceneEventCamera eventCam = sceneEventCameraList.Find((x) => x.eventCameraType == AppData.SceneEventCameraType.AssetPreviewCamera);

            eventCam.value.transform.position = eventCam.GetDefaultCameraPose().position;
            eventCam.value.transform.rotation = eventCam.GetDefaultCameraPose().rotation;
            eventCam.value.transform.localScale = eventCam.GetDefaultCameraPose().scale;

            camPoseTarget.position = defaultCamPositionReference;
            camPoseTarget.rotation = defaultCamRotationReference;
        }

        void OnReset()
        {


            OnResetCameraPose();
        }

        #endregion
    }
}
