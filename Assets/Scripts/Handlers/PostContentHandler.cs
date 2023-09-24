using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.RedicalGames.Filar
{
    public class PostContentHandler : AppData.SelectableDynamicContent
    {
        #region Components

        public GameObject model;

        #endregion

        #region Main

        #region Data Setters

        public void SetModel(GameObject model) => this.model = model;

        #endregion

        #region Data Getters

        public GameObject GetModel() => model;

        protected override void OnBeginDragExecuted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDragExecuted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnEndDragExecuted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnPointerDownExecuted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnPointerUpExecuted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #endregion
    }
}