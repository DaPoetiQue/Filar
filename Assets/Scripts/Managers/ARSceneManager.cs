using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Com.RedicalGames.Filar
{
    public class ARSceneManager : AppData.SingletonBaseComponent<ARSceneManager>
    {
        #region Components

        [SerializeField]
        AppData.ARSceneTrackingData sceneTrackingData = new AppData.ARSceneTrackingData();

        Coroutine initializationRoutine;

        #endregion

        #region Unity Callbacks

        void Awake() => Init();

        void OnEnable() => OnRegisterActionEvents(true);
        void OnDisable() => OnRegisterActionEvents(false);

        void Update() => OnSceneTrackingUpdate();

        #endregion

        #region Main

        void Init()
        {
            if (sceneTrackingData.session != null)
            {
                if (sceneTrackingData.initializeOnStart)
                    EnterARMode();
                else
                    ExitARMode();
            }
            else
                Debug.LogWarning("--> AR Session Handler Null - Not Initialized In The Inspector Panel.");

            if (AppManager.Instance.IsRuntime())
                sceneTrackingData.focusContent.Hide();

            #region Scene Tracking Data Initialization

            sceneTrackingData.screenCenter.x = UnityEngine.Screen.width / 2.0f;
            sceneTrackingData.screenCenter.y = UnityEngine.Screen.height / 2.0f;
            sceneTrackingData.screenCenter.z = sceneTrackingData.trackingDistance;

            sceneTrackingData.hitInfoList = new List<ARRaycastHit>();
            sceneTrackingData.trackedPose = new AppData.ARSceneTrackedPose();

            #endregion
        }

        void OnRegisterActionEvents(bool register)
        {
            if (register)
                AppData.ActionEvents._OnARSceneAssetStateEvent += ActionEvents__OnARSceneAssetStateEvent;
            else
                AppData.ActionEvents._OnARSceneAssetStateEvent -= ActionEvents__OnARSceneAssetStateEvent;
        }

        private void ActionEvents__OnARSceneAssetStateEvent(AppData.ARSceneContentState contentState)
        {
            switch (contentState)
            {
                case AppData.ARSceneContentState.Place:

                    sceneTrackingData.focusContent.sessionState = AppData.SceneARSessionState.AssetPlaced;
                    //sceneTrackingData.focusContent.Hide();

                    break;

                case AppData.ARSceneContentState.Remove:


                    sceneTrackingData.focusContent.sessionState = AppData.SceneARSessionState.TrackingLost;

                    break;
            }
        }

        public void EnterARMode()
        {
            if (sceneTrackingData.session != null)
            {
                sceneTrackingData.session.enabled = true;
                sceneTrackingData.focusContent.sessionState = AppData.SceneARSessionState.TrackingInProgress;
                ScreenUIManager.Instance.SetScreenSleepTime(true);
            }
            else
                Debug.LogWarning("--> AR Session Handler Null - Not Initialized In The Inspector Panel.");
        }

        public void ExitARMode()
        {
            if (sceneTrackingData.session != null)
            {
                sceneTrackingData.session.enabled = false;
                sceneTrackingData.focusContent.sessionState = AppData.SceneARSessionState.None;
                ScreenUIManager.Instance.SetScreenSleepTime(false);
            }
            else
                Debug.LogWarning("--> AR Session Handler Null - Not Initialized In The Inspector Panel.");
        }

        void OnSceneTrackingUpdate()
        {
            if (!AppManager.Instance.IsRuntime())
                return;

            if (sceneTrackingData.focusContent.sessionState == AppData.SceneARSessionState.AssetPlaced)
                return;

            switch (sceneTrackingData.focusContent.sessionState)
            {
                case AppData.SceneARSessionState.TrackingInProgress:

                    OnSurfaceDetected((status) =>
                    {
                        if (status)
                        {
                            sceneTrackingData.focusContent.sessionState = AppData.SceneARSessionState.TrackingFound;
                        }
                        else
                        {
                            if (sceneTrackingData.focusContent.sessionState == AppData.SceneARSessionState.TrackingFound)
                                sceneTrackingData.focusContent.sessionState = AppData.SceneARSessionState.TrackingLost;
                        }
                    });

                    break;

                case AppData.SceneARSessionState.TrackingFound:

                    OnGetDetectedSurfacePose((status, results) =>
                    {
                        if (status)
                            UpdateFocusContent(results);
                        else
                            sceneTrackingData.focusContent.sessionState = AppData.SceneARSessionState.TrackingLost;
                    });

                    break;

                case AppData.SceneARSessionState.TrackingLost:

                    if (initializationRoutine != null)
                    {
                        StopCoroutine(initializationRoutine);
                        initializationRoutine = null;
                    }

                    initializationRoutine = StartCoroutine(InitializeSceneTracking());

                    break;
            }
        }

        void OnSurfaceDetected(Action<bool> callback)
        {
            if (sceneTrackingData.rayCastManager.Raycast(sceneTrackingData.screenCenter, sceneTrackingData.hitInfoList, sceneTrackingData.trackableTypes))
            {
                callback.Invoke(true);
            }
            else
                callback.Invoke(false);
        }

        IEnumerator InitializeSceneTracking()
        {
            sceneTrackingData.focusContent.Hide();
            Debug.LogWarning("--> RG_Unity - Initialize Scene Tracking : Tracking Lost, Re-Initialization..............................");

            yield return new WaitForSeconds(5.0f);

            sceneTrackingData.focusContent.sessionState = AppData.SceneARSessionState.TrackingInProgress;

            yield return null;
        }

        void OnGetDetectedSurfacePose(Action<bool, AppData.ARSceneTrackedPose> callback)
        {
            if (sceneTrackingData.rayCastManager.Raycast(sceneTrackingData.screenCenter, sceneTrackingData.hitInfoList, sceneTrackingData.trackableTypes))
            {
                for (int i = 0; i < sceneTrackingData.hitInfoList.Count; i++)
                {
                    sceneTrackingData.trackedPose.position.x = sceneTrackingData.hitInfoList[i].pose.position.x;
                    sceneTrackingData.trackedPose.position.y = sceneTrackingData.hitInfoList[i].pose.position.y + sceneTrackingData.groundOffSet;
                    sceneTrackingData.trackedPose.position.z = sceneTrackingData.hitInfoList[i].pose.position.z;

                    sceneTrackingData.trackedPose.rotation.x = 0.0f;
                    sceneTrackingData.trackedPose.rotation.y = sceneTrackingData.hitInfoList[i].pose.rotation.y;
                    sceneTrackingData.trackedPose.rotation.z = 0.0f;
                }

                callback.Invoke(true, sceneTrackingData.trackedPose);
            }
            else
                callback.Invoke(false, new AppData.ARSceneTrackedPose());
        }

        void UpdateFocusContent(AppData.ARSceneTrackedPose pose)
        {
            sceneTrackingData.focusContent.SetPose(pose);
            sceneTrackingData.focusContent.Show();
        }


        public void SetARSceneSessionState(AppData.SceneARSessionState state)
        {
            sceneTrackingData.focusContent.sessionState = state;
        }

        public AppData.SceneARSessionState GetARSceneSessionState()
        {
            return sceneTrackingData.focusContent.sessionState;
        }

        #endregion
    }
}