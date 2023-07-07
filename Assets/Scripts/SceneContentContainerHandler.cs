using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SceneContentContainerHandler : MonoBehaviour
    {
        #region Components

        [SerializeField]
        List<AppData.SceneContainerData> sceneAssetContainerList = new List<AppData.SceneContainerData>();

        [Space(5)]
        [SerializeField]
        float transitionSpeed;

        Transform targetContainer;

        bool canTransition = false;

        #endregion

        #region Unity Callbacks

        void OnEnable() => OnEventsSubscriptions(true);

        void OnDisable() => OnEventsSubscriptions(false);

        void Start() => Init();

        void Update() => OnContanierTransition();

        #endregion

        #region Main

        void OnEventsSubscriptions(bool subscribed)
        {
            if (subscribed)
            {
                AppData.ActionEvents._OnResetSceneAssetPreviewPoseEvent += OnResetSceneAssetPreviewPoseEvent;
                AppData.ActionEvents._OnScreenTogglableStateEvent += ActionEvents__OnScreenTogglableStateEvent;
            }
            else
            {
                AppData.ActionEvents._OnResetSceneAssetPreviewPoseEvent -= OnResetSceneAssetPreviewPoseEvent;
                AppData.ActionEvents._OnScreenTogglableStateEvent -= ActionEvents__OnScreenTogglableStateEvent;
            }
        }

        private void ActionEvents__OnScreenTogglableStateEvent(AppData.TogglableWidgetType tValue, bool uValue, bool vValue)
        {
            if (canTransition)
                canTransition = false;
        }

        void Init()
        {
            if (sceneAssetContainerList == null)
                Debug.LogWarning("--> Scene Asset Container List Is Null / Not Initialized In The Inspector Panel.");
        }

        void OnResetSceneAssetPreviewPoseEvent(AppData.AssetModeType assetModeType)
        {
            if (sceneAssetContainerList.Count > 0)
            {
                AppData.SceneContainerData sceneContainer = sceneAssetContainerList.Find((x) => x.assetModeType == assetModeType);

                if (sceneContainer.value != null)
                {
                    targetContainer = sceneContainer.value;
                    canTransition = true;

                    AppData.ActionEvents.OnResetContaineToDefaultPoseEvent();
                }
                else
                    Debug.LogWarning("--> OnResetSceneAssetPreviewPoseEvent : Scene Container Value Is Null.");
            }
            else
                Debug.LogWarning("--> Scene Asset Container List Is Empty / Null.");

        }

        void OnRotateSceneAsset()
        {
            canTransition = false;
        }

        void OnContanierTransition()
        {
            if (canTransition)
            {
                Quaternion containerQuaternion = targetContainer.rotation;
                containerQuaternion = Quaternion.Slerp(containerQuaternion, Quaternion.identity, transitionSpeed * Time.smoothDeltaTime);

                targetContainer.rotation = containerQuaternion;

                float distance = (targetContainer.eulerAngles - Vector3.zero).sqrMagnitude;

                if (distance <= 0.1f)
                {
                    AppData.ActionEvents.OnSceneModelPoseResetEvent();
                    targetContainer = null;
                    canTransition = false;
                }
            }
            else
                return;
        }

        #endregion

    }
}
