using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ScreenSpaceTargetHandler : AppMonoBaseClass
    {
        #region Main

        public Vector2 GetPosition() => transform.GetComponent<RectTransform>().anchoredPosition;

        public RectTransform GetTargetRect() => transform.GetComponent<RectTransform>();

        #endregion
    }
}