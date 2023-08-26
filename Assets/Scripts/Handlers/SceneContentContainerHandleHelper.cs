using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SceneContentContainerHandleHelper : AppMonoBaseClass
    {
        #region Components

        [Space(5)]
        [SerializeField]
        float transitionSpeed;

        bool canTransition = false;

        #endregion

        #region Unity Callbacks

        void OnEnable() => OnSubscribeToEvents(true);

        void OnDisable() => OnSubscribeToEvents(false);

        void Update() => OnContanierTransition();

        #endregion

        #region Main

        void OnSubscribeToEvents(bool subscribe)
        {
            if (subscribe)
                AppData.ActionEvents._OnResetContaineToDefaultrPoseEvent += OnResetContaineToDefaultrPoseEvent;
            else
                AppData.ActionEvents._OnResetContaineToDefaultrPoseEvent -= OnResetContaineToDefaultrPoseEvent;
        }

        void OnResetContaineToDefaultrPoseEvent()
        {
            canTransition = true;
        }


        void OnContanierTransition()
        {
            if (canTransition)
            {
                Quaternion containerQuaternion = this.transform.localRotation;
                containerQuaternion = Quaternion.Slerp(containerQuaternion, Quaternion.identity, transitionSpeed * Time.deltaTime);

                this.transform.localRotation = containerQuaternion;

                Debug.LogError($"--> RG_Unity : Resetting Pose - {this.transform.name} - Pose : {this.transform.localRotation.ToString()} - From : {containerQuaternion.ToString()}");

                float distance = (this.transform.eulerAngles - Vector3.zero).sqrMagnitude;

                if (distance <= 0.1f)
                {
                    AppData.ActionEvents.OnSceneModelPoseResetEvent();
                    canTransition = false;

                    Debug.LogError($"--> RG_Unity : Scene Container Reset Completed - {this.transform.name}");
                }
            }
            else
                return;
        }

        #endregion
    }
}
