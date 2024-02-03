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


        private float lerpSpeed = 1.0F;

        #endregion

        #region Unity Callbacks

        private void OnMouseDrag() => isDragging = true;

        #endregion

        #region Main

        protected override void Init()
        {
            var callbackResults = new AppData.Callback(GetSceneEventCameraContainerTransform());

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

                        appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnWidgetShownEvent, AppData.EventType.OnWidgetShownEvent, true);
                        appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnWidgetHiddenEvent, AppData.EventType.OnWidgetHiddenEvent, true);

                        appEventsManagerInstance.OnEventSubscription(OnUpdateEvent, AppData.EventType.OnUpdate, true);
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
            var callbackResults = new AppData.Callback(GetSceneEventCameraContainerTransform());

            if (callbackResults.Success())
            {

            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnScreenHiddenEvent(Screen screen)
        {
            var callbackResults = new AppData.Callback(GetSceneEventCameraContainerTransform());

            if (callbackResults.Success())
            {

            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        #endregion

        #region Widgets Events


        private void OnWidgetShownEvent(AppData.Widget widget)
        {
            var callbackResults = new AppData.Callback(GetSceneEventCameraContainerTransform());

            if (callbackResults.Success())
            {

            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnWidgetHiddenEvent(AppData.Widget widget)
        {
            var callbackResults = new AppData.Callback(GetSceneEventCameraContainerTransform());

            if (callbackResults.Success())
            {

            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        #endregion

        private void OnUpdateEvent()
        {

            if (Input.GetMouseButton(0))
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
                if (isDragging)
                {
                    theSpeed = avgSpeed;
                    isDragging = false;
                }
                float i = Time.deltaTime * lerpSpeed;
                theSpeed = Vector3.Lerp(theSpeed, Vector3.zero, i);
            }

            eventCameraScene.position = Vector3.Lerp(eventCameraScene.position, targetPosition, damping * Time.deltaTime);

            eventCameraScene.Rotate(Camera.main.transform.up * theSpeed.x * rotationSpeed, Space.World);
            eventCameraScene.Rotate(Camera.main.transform.right * theSpeed.y * rotationSpeed, Space.World);

            eventCameraScene.LookAt(target.position);
        }

        public AppData.CallbackData<Transform> GetSceneEventCameraContainerTransform()
        {
            var callbackResults = new AppData.CallbackData<Transform>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(eventCameraScene, "Scene Event Camera Container Transform", "Get Scene Event Camera Container Transform Failed - Scene Event Camera Container Transform Value Is Missing / Null - Invalid Opration."));

            if(callbackResults.Success())
            {
                callbackResults.result = "Get Scene Event CameraContainer Transform Success - Scene Event CameraContainer Transform Value Has Been Assigned Successfully.";
                callbackResults.data = eventCameraScene;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #region Camera Functions

        #endregion

        #endregion

    }
}