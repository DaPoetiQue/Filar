
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class PublishTest : AppMonoBaseClass
    {
        #region Components

        [SerializeField]
        GameObject obj;

        #endregion

        #region Main

        public void Publish()
        {
            PublishingManager.Instance.OnPublish(obj, publishCallbackResults => 
            {
                Log(publishCallbackResults.ResultCode, publishCallbackResults.Result, this);
            });
        }

        #endregion
    }
}