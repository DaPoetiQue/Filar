using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class AppTimeEventsHandler : AppMonoBaseClass
    {
        #region Main

        private void Awake() => AppData.ActionEvents.Awake();

        void Start() => AppData.ActionEvents.Start();

        void Update() => AppData.ActionEvents.Update();

        void LateUpdate() => AppData.ActionEvents.LateUpdate();

        void FixedUpdate() => AppData.ActionEvents.FixedUpdate();

        #endregion
    }
}
