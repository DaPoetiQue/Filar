using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class EditorRuntimePublisher : AppMonoBaseClass
    {
        #region Components

        [SerializeField]
        AppData.Post post = new AppData.Post();

        [SerializeField]
        GameObject obj;

        [SerializeField]
        Sprite thumbnail;

        [SerializeField]
        AppData.Helpers.ImageEncoderType imageEncoderType;

        #endregion

        #region Main

        public async void Publish()
        {
            #region Post Handler

            string profileIdentifier = AppData.Helpers.GenerateAppKey(5); // This Will Come Publishing User Profile Identifier
            string postIdentifier = AppData.Helpers.GenerateUniqueIdentifier(5);
            string contentIdentifier = AppData.Helpers.GenerateUniqueIdentifier(5);

            #endregion

            #region Post

            post.SetUniqueIdentifier(postIdentifier);
            post.SetRootdentifier(profileIdentifier);
            post.InitializeCreationDateTime();
            post.SetPostThumbnail(thumbnail);

            #endregion

            #region Content

            AppData.ContentGenerator contentGenerator = new AppData.ContentGenerator(obj);

            var getObjectStringTaskResults = await contentGenerator.SetGameObject();

            if (getObjectStringTaskResults.Success())
            {
                AppData.SerializableGameObject content = new AppData.SerializableGameObject(getObjectStringTaskResults.GetData());

                AppData.SerializableImage imageSerializer = new AppData.SerializableImage(thumbnail, imageEncoderType);

                #endregion

                #region Publish

                PublishingManager.Instance.OnPublish(post, imageSerializer, content, contentIdentifier, publishCallbackResults =>
                {
                    Log(publishCallbackResults.GetResultCode, publishCallbackResults.GetResult, this);
                });
            }

            #endregion
        }

        #endregion
    }
}