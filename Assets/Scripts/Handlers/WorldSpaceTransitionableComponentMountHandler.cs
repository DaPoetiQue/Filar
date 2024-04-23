using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class WorldSpaceTransitionableComponentMountHandler : AppMonoBaseClass
    {
        #region Components

        private Transform target;

        #endregion

        #region Main

        public AppData.CallbackData<Vector3> GetPosition()
        {
            var callbackResults = new AppData.CallbackData<Vector3>(GetTarget());

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Position Sucess - Position Is Set At : {GetTarget().GetData().GetObjectPosition()}.";
                callbackResults.data = GetTarget().GetData().GetObjectPosition();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<Vector3> GetScale()
        {
            var callbackResults = new AppData.CallbackData<Vector3>(GetTarget());

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Scale Sucess - Scale Is Set At : {GetTarget().GetData().GetObjectScale()}.";
                callbackResults.data = GetTarget().GetData().GetObjectScale();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<Quaternion> GetRotation()
        {
            var callbackResults = new AppData.CallbackData<Quaternion>(GetTarget());

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Rotation Sucess - Rotation Is Set At : {GetTarget().GetData().GetObjectRotation()}.";
                callbackResults.data = GetTarget().GetData().GetObjectRotation();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<Quaternion> GetLocalRotation()
        {
            var callbackResults = new AppData.CallbackData<Quaternion>(GetTarget());

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Rotation Sucess - Rotation Is Set At : {GetTarget().GetData().GetObjectRotationLocal()}.";
                callbackResults.data = GetTarget().GetData().GetObjectRotationLocal();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<Vector3> GetRotationAngle()
        {
            var callbackResults = new AppData.CallbackData<Vector3>(GetTarget());

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Rotation Sucess - Rotation Is Set At : {GetTarget().GetData().GetObjectRotationAngle()}.";
                callbackResults.data = GetTarget().GetData().GetObjectRotationAngle();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<(Vector3 position, Vector3 scale, Quaternion rotation)> GetPoseAngle()
        {
            var callbackResults = new AppData.CallbackData<(Vector3 position, Vector3 scale, Quaternion rotation)>(GetTarget());

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Pose Angle Success - Pose Angle Is Set To : {GetTarget().GetData().GetObjectPoseAngle()}.";
                callbackResults.data = GetTarget().GetData().GetObjectPose();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<Transform> GetTarget()
        {
            var callbackResults = new AppData.CallbackData<Transform>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(target, "Target", "Get Target Failed - Target Value Is Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Target Success - Target Has Been Assigned.";
                callbackResults.data = target;
            }
            else
            {
                target = transform;

                callbackResults.result = $"Get Target Success - Target Has Been Found From Component.";
                callbackResults.data = target;
            }

            return callbackResults;
        }

        #endregion
    }
}
