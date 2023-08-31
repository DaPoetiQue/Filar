
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

        public async void Publish()
        {
            AppData.PostHandler postHandler = new AppData.PostHandler();

            AppData.Post post = new AppData.Post();
            AppData.Profile profile = new AppData.Profile();
            AppData.ModelMeshData content = new AppData.ModelMeshData();

            #region Post Handler

            string identifier = AppData.Helpers.GenerateUniqueIdentifier(5);
            postHandler.SetIdentifier(identifier);

            #endregion

            #region Post

            post.SetIdentifier(postHandler.GetIdentifier());
            post.SetTitle("Excellent Diora");
            post.SetCaption("This Is No Christaino Maninja, Even Christian Knows Ukuthi His Big Nose Nauthy Nino In Beast Mode - I see Black Nah Nigros- Kings II");

            postHandler.SetPost(post);

            #endregion

            #region Profile

            profile.SetIdentifier(postHandler.GetIdentifier());

            profile.SetUserName("Shudz");
            profile.SetUserEmail("ShudzinShu@gmail.com");
            profile.SetUserPassword("@#ShuShu");

            postHandler.SetProfile(profile);

            #endregion

            #region Content

            var results = await AppData.Helpers.GetMeshDataAsync(obj);

            content.SetIdentifier(postHandler.GetIdentifier());
            content.mesh = content.MeshDataToString(results.data, "m|");

            postHandler.SetContent(content);

            #endregion

            #region Publish

            PublishingManager.Instance.OnPublish(postHandler, publishCallbackResults => 
            {
                Log(publishCallbackResults.ResultCode, publishCallbackResults.Result, this);
            });

            #endregion
        }

        #endregion
    }
}