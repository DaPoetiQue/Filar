using UnityEngine.EventSystems;

namespace Com.RedicalGames.Filar
{
    /// <summary>
    /// Handles logging screen raycast object info.
    /// </summary>
    public class UIScreenRayCastDebugLogger : AppMonoBaseClass, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData ped) => LogInfo($"UI Graphic Raycaster has hit a game object named: {ped.pointerCurrentRaycast.gameObject.name}", this);
    }
}
