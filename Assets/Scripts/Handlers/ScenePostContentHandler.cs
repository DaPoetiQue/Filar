using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.RedicalGames.Filar
{
    public class ScenePostContentHandler : AppData.SelectableWidgetComponent
    {
        #region Components

        public GameObject model;

        public AppData.Post post;

        #endregion

        #region Main

        #region Data Setters

        public void SetContent(GameObject model) => this.model = model;
        public void SetPost(AppData.Post post) => this.post = post;

        public void ShowContent()
        {
            if(model.transform.childCount > 0)
                for (int i = 0; i < model.transform.childCount; i++)
                    model.transform.GetChild(i).gameObject.Show();

            model.Show();
        }


        public void HideContent()
        {
            if (model.transform.childCount > 0)
                for (int i = 0; i < model.transform.childCount; i++)
                    model.transform.GetChild(i).gameObject.Hide();

            model.Hide();
        }

        public void SetContentPose((Vector3 position, Vector3 scale, Quaternion rotation) pose)
        {
            model.transform.position = pose.position;
            model.transform.rotation = pose.rotation;
            model.transform.localScale = pose.scale;
        }

        public void SetContenLocaltPose((Vector3 position, Vector3 scale, Quaternion rotation) pose)
        {
            model.transform.localPosition = pose.position;
            model.transform.localRotation = pose.rotation;
            model.transform.localScale = pose.scale;
        }

        #endregion

        #region Data Getters

        public GameObject GetModel() => model;

        public AppData.CallbackData<AppData.Post> GetPost()
        {
            var callbackResults = new AppData.CallbackData<AppData.Post>(AppData.Helpers.GetAppComponentValid(post, "Post", $"Get Post Failed - There Is No Post Assigned For Scene Post Content : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = post;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

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

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType>> OnGetState()
        {
            return null;
        }

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType>>> callback)
        {
         
        }

        protected override void OnScreenWidgetShownEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetHiddenEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetTransitionInProgressEvent()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}