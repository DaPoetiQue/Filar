using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ScreenSpaceTargetHandler : AppMonoBaseClass
    {
        #region Main

        public Vector2 GetPosition() => transform.GetComponent<RectTransform>().anchoredPosition;
        public Vector2 GetScale() => transform.GetComponent<RectTransform>().sizeDelta;
        public Vector3 GetRotation() => transform.localEulerAngles;

        public RectTransform GetTargetRect() => transform.GetComponent<RectTransform>();

        #endregion
    }
}