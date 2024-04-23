using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ScreenSpaceTransitionableComponentMountHandler : AppMonoBaseClass
    {
        #region Components

        private RectTransform target;

        #endregion

        #region Main

        public AppData.CallbackData<Vector2> GetPosition()
        {
            var callbackResults = new AppData.CallbackData<Vector2>(GetTarget());

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Position Sucess - Position Is Set At : {GetTarget().GetData().GetWidgetPosition()}.";
                callbackResults.data = GetTarget().GetData().GetWidgetPosition();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<Vector2> GetScale()
        {
            var callbackResults = new AppData.CallbackData<Vector2>(GetTarget());

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Scale Sucess - Scale Is Set At : {GetTarget().GetData().GetWidgetScale()}.";
                callbackResults.data = GetTarget().GetData().GetWidgetScale();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<Vector3> GetRotation()
        {
            var callbackResults = new AppData.CallbackData<Vector3>(GetTarget());

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Rotation Sucess - Rotation Is Set At : {GetTarget().GetData().GetWidgetRotationAngle()}.";
                callbackResults.data = GetTarget().GetData().GetWidgetRotationAngle();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<(Vector2 position, Vector2 scale, Vector3 rotationalAngle)> GetPoseAngle()
        {
            var callbackResults = new AppData.CallbackData<(Vector2 position, Vector2 scale, Vector3 rotationalAngle)>(GetTarget());

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Pose Angle Success - Pose Angle Is Set To : {GetTarget().GetData().GetWidgetPoseAngle()}.";
                callbackResults.data = GetTarget().GetData().GetWidgetPoseAngle();
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<RectTransform> GetTarget()
        {
            var callbackResults = new AppData.CallbackData<RectTransform>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(target, "Target", "Get Target Failed - Target Value Is Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Target Success - Target Has Been Assigned.";
                callbackResults.data = target;
            }
            else
            {
                target = GetComponent<RectTransform>();

                callbackResults.result = $"Get Target Success - Target Has Been Found From Component.";
                callbackResults.data = target;
            }

            return callbackResults;
        }

        #endregion
    }
}