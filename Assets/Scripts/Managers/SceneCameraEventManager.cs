using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{

    public class SceneCameraEventManager : AppData.SingletonBaseComponent<SceneCameraEventManager>
    {
        #region Components

        [SerializeField]
        private List<AppData.SceneEventCamera> sceneEventCameras = new List<AppData.SceneEventCamera>();

        [Space(5)]
        [SerializeField]
        private Transform eventCameraScene = null;

        public Transform target; // Target object to orbit around
        
        public float minYAngle = -30.0f; // Minimum pitch angle
        public float maxYAngle = 60.0f; // Maximum pitch angle
       
        private float rotationSpeed; // Mouse sensitivity
        private float damping;

        private float distance = 5.0f; // Distance from the target

        private float currentX = 0.0f;
        private float currentY = 0.0f;

        private Vector3 theSpeed;
        private Vector3 avgSpeed;
        private bool isDragging = false;

        private Vector3 targetPosition;

        private AppData.SceneAssetPose defaultEventCameraScenePose = new AppData.SceneAssetPose();

        private float lerpSpeed = 1.0F;

        private Screen screen = null;

        private bool resetEventCameraScenePose = false;

        #endregion

        #region Unity Callbacks

        private void OnMouseDrag() => isDragging = true;

        #endregion

        #region Main

        protected override void Init()
        {
            var callbackResults = new AppData.Callback(GetEventCameraScene());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetSceneEventCameras());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "App Database Manager Instance Is Not Initialized Yet."));

                    if (callbackResults.Success())
                    {
                        var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                        rotationSpeed = appDatabaseManagerInstance.GetDefaultExecutionValue(AppData.RuntimeExecution.PreviewModeOrbitSpeed).value;
                        damping = appDatabaseManagerInstance.GetDefaultExecutionValue(AppData.RuntimeExecution.PreviewModeOrbitDampingSpeed).value;
                        distance = appDatabaseManagerInstance.GetDefaultExecutionValue(AppData.RuntimeExecution.PreviewModeOrbitDistance).value;

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "App Events Manager Instance Is Not Initialized Yet."));

                        if (callbackResults.Success())
                        {
                            var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                            appEventsManagerInstance.OnEventSubscription<Screen>(OnScreenShownEvent, AppData.EventType.OnScreenShownEvent, true);
                            appEventsManagerInstance.OnEventSubscription<Screen>(OnScreenHiddenEvent, AppData.EventType.OnScreenHiddenEvent, true);

                            appEventsManagerInstance.OnEventSubscription(OnUpdateEvent, AppData.EventType.OnUpdate, true);
                            appEventsManagerInstance.OnEventSubscription(OnResetEventCameraScenePose, AppData.EventType.OnUpdate, true);

                            appEventsManagerInstance.OnEventSubscription<AppData.Post>(OnPostSelected, AppData.EventType.OnPostSelectedEvent, true);

                            SetDefaultEventCameraScenePose(GetEventCameraScene().GetData(), poseSetCallbackResults => 
                            {
                                callbackResults.SetResult(poseSetCallbackResults);
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

        #region Screen Events

        private void OnScreenShownEvent(Screen screen)
        {
            var callbackResults = new AppData.Callback(GetSceneEventCameras());

            if (callbackResults.Success())
            {
                SetFocusedScreen(screen, screenFocusedCallbackResults => 
                {
                    callbackResults.SetResult(screenFocusedCallbackResults);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnScreenHiddenEvent(Screen screen)
        {
            var callbackResults = new AppData.Callback(GetSceneEventCameras());

            if (callbackResults.Success())
            {
                RemoveFocusedScreen(screen, focusedScreenRemovedCallbackResults => 
                {
                    callbackResults.SetResult(focusedScreenRemovedCallbackResults);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        #endregion

        private void OnPostSelected(AppData.Post post)
        {
            var callbackResults = new AppData.Callback(CameraPoseChanged());

            if(callbackResults.Success())
            {
                ResetEventCameraScenePose(cameraPoseResetCallbackResults => 
                {
                    callbackResults.SetResult(cameraPoseResetCallbackResults);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void ResetEventCameraScenePose(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            LogInfo($" ___Losg_Cat:: Reset Camera Pose", this);

            resetEventCameraScenePose = true;

            callback?.Invoke(callbackResults);
        }

        private void OnResetEventCameraScenePose()
        {
            //if (!resetEventCameraScenePose)
            //    return;

            //eventCameraScene.rotation = Quaternion.Slerp(eventCameraScene.rotation, GetDefaultEventCameraScenePose().GetData().rotation, rotationSpeed * Time.smoothDeltaTime);

            //if (CameraPoseChanged().UnSuccessful())
            //    resetEventCameraScenePose = false;
        }

        private void OnUpdateEvent()
        {
            var callbackResults = new AppData.Callback(GetFocusedScreen());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetFocusedScreen().GetData().Focused());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(CanOrbit());

                    if (callbackResults.Success())
                    {
                        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
                        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;

                        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle); // Clamp pitch angle

                        theSpeed = new Vector3(currentX, -currentY, 0.0F);
                        avgSpeed = Vector3.Lerp(avgSpeed, theSpeed, Time.deltaTime * 5);


                        Vector3 direction = new Vector3(0, 0, -distance);
                        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
                        targetPosition = target.position + rotation * direction;
                    }
                    else
                    {
                        float i = Time.deltaTime * lerpSpeed;
                        theSpeed = Vector3.Lerp(theSpeed, Vector3.zero, i);
                    }

                    eventCameraScene.position = Vector3.Lerp(eventCameraScene.position, targetPosition, damping * Time.deltaTime);

                    eventCameraScene.Rotate(Camera.main.transform.up * theSpeed.x * rotationSpeed, Space.World);
                    eventCameraScene.Rotate(Camera.main.transform.right * theSpeed.y * rotationSpeed, Space.World);

                    eventCameraScene.LookAt(target.position);
                }
                else
                    return;
            }
            else
                return;
        }

        private AppData.Callback CanOrbit()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(SelectableManager.Instance, "Selectable Manager Instance", "Selectable Manager Instance Is Not Initialized Yet."));

            if(callbackResults.Success())
            {
                var selectableManagerInstance = AppData.Helpers.GetAppComponentValid(SelectableManager.Instance, "Selectable Manager Instance").GetData();

                callbackResults.SetResult(selectableManagerInstance.IsFingerOverSelectableAsset());

                if (callbackResults.Success())
                {
                    if (Input.GetMouseButton(0))
                        callbackResults.result = "Can Orbit - Finger Is Successfully Placed Over A Selectable Asset.";
                    else
                    {
                        callbackResults.result = "Can Not Orbit - Finger Is Not Placed Over A Selectable Asset.";
                        callbackResults.resultCode = AppData.Helpers.WarningCode;
                    }
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #region Data Setters

        private void SetDefaultEventCameraScenePose(Transform eventCameraSceneTransform, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            defaultEventCameraScenePose.Initialize(eventCameraSceneTransform, poseInitializationCallbackResults =>
            {
                callbackResults.SetResult(poseInitializationCallbackResults);
            });

            callback?.Invoke(callbackResults);
        }

        private void SetFocusedScreen(Screen screen, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(screen, "Screen", "Set Focused Screen Failed - Screen Parameter Value Is Missing / Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                this.screen = screen;
                callbackResults.result = $"Set Focused Screen Success -Focused Screen : {screen.GetName()} - Of Type : {screen.GetType().GetData()} Has Been Successfully Assigned.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        private void RemoveFocusedScreen(Screen screen, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(screen, "Screen", "Remove Focused Screen Failed - Screen Parameter Value Is Missing / Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                if (this.screen == screen)
                {
                    this.screen = null;
                    callbackResults.result = $"Remove Focused Screen Success -Focused Screen : {screen.GetName()} - Of Type : {screen.GetType().GetData()} Has Been Successfully Assigned.";
                }
                else
                {
                    callbackResults.result = $"Remove Focused Screen Failed -Focused Screen : {screen.GetName()} - Of Type : {screen.GetType().GetData()} Is Not Equal To Current Screen : {this.screen?.GetName()} - Of Type : {this.screen?.GetType()?.GetData()} - Invalid Operation..";
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Data Getters

        private AppData.Callback CameraPoseChanged()
        {
            var callbackResults = new AppData.Callback(GetDefaultEventCameraScenePose());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetDefaultEventCameraScenePose().GetData().IsEqualToPose(GetEventCameraScene().GetData()));

                if (callbackResults.UnSuccessful())
                {
                    callbackResults.result = "Camera Pose Has Changed.";
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.result = "Camera Pose Hasn't Changed.";
                    callbackResults.resultCode = AppData.Helpers.WarningCode;
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        private AppData.CallbackData<AppData.SceneAssetPose> GetDefaultEventCameraScenePose()
        {
            var callbackResults = new AppData.CallbackData<AppData.SceneAssetPose>();

            callbackResults.SetResult(defaultEventCameraScenePose.Initialized());

            if(callbackResults.Success())
            {
                callbackResults.result = "Get Default Event Camera Scene Pose Success - Pose Data Successfully Initialized.";
                callbackResults.data = defaultEventCameraScenePose;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        private AppData.CallbackData<Screen> GetFocusedScreen()
        {
            var callbackResults = new AppData.CallbackData<Screen>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(screen, "Screen", "Get Focused Screen Failed - Screen Value Is Missing / Null - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = $"Get Focused Screen Success -Focused Screen : {screen.GetName()} - Of Typ : {screen.GetType().GetData()} Has Been Successfully Found.";
                callbackResults.data = screen;
            }

            return callbackResults;
        }

        public AppData.CallbackData<AppData.SceneEventCamera> GetSceneEventCamera(AppData.ScreenType screenType)
        {
            var callbackResults = new AppData.CallbackData<AppData.SceneEventCamera>();

            callbackResults.SetResult(GetSceneEventCameras());

            if (callbackResults.Success())
            {
                var eventCamera = GetSceneEventCameras().GetData().Find(eventCam => eventCam.GetScreenType().GetData() == screenType);

                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(eventCamera, "Event Camera", $"Get Event Camera Failed - Couldn't Find Event Camera For Screen Type : {screenType} In Scene Event Cameras - Invalid Operation."));

                if(callbackResults.Success())
                {
                    callbackResults.result = $"Get Event Camera Success - Event Camera For Screen Type : {screenType} Has Been Successfully Found In Scene Event Cameras.";
                    callbackResults.data = eventCamera;
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackDataList<AppData.SceneEventCamera> GetSceneEventCameras()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.SceneEventCamera>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(sceneEventCameras, "Scene Event Cameras", "Get Scene Event Camera Failed - Scene Event Cameras Are Missing / Null - Invalid Opration."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Scene Event Cameras Success - {sceneEventCameras.Count} : Scene Event Camera(s) Value Has Been Assigned Successfully.";
                callbackResults.data = sceneEventCameras;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<Transform> GetEventCameraScene()
        {
            var callbackResults = new AppData.CallbackData<Transform>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(eventCameraScene, "Event Camera Scene", "Get Event Camera Scene Failed - Event Camera Scene Value Is Missing / Null - Invalid Opration."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Event Camera Scene Success - Event Camera Scene : {eventCameraScene.name} Value Has Been Assigned Successfully.";
                callbackResults.data = eventCameraScene;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

        #region Camera Functions

        #endregion

        #endregion

    }
}