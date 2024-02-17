using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.RedicalGames.Filar
{
    public class SceneManager : AppData.SingletonBaseComponent<SceneManager>
    {
        #region Components

        [SerializeField]
        private AppData.SceneEnvironmentLibrary environmentLibrary = new AppData.SceneEnvironmentLibrary();

        #endregion

        #region Main

        protected override void Init()
        {

        }

        public async Task<AppData.Callback> LoadEnvironment(AppData.SceneEnvironmentType environmentType)
        {
            var callbackResults = new AppData.Callback(GetEnvironmentLibrary());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetEnvironmentLibrary().GetData().GetSceneEnvironment(environmentType));

                if (callbackResults.Success())
                {
                    await Task.Delay(1000);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public async Task<AppData.Callback> LoadEnvironments()
        {
            var callbackResults = new AppData.Callback(GetEnvironmentLibrary());

            if (callbackResults.Success())
            {
                var environments = GetEnvironmentLibrary().GetData().GetIncludedSceneEnvironments().GetData();

                await Task.Delay(1000);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #region Data Getters

        public AppData.CallbackData<AppData.SceneEnvironmentLibrary> GetEnvironmentLibrary()
        {
            var callbackResults = new AppData.CallbackData<AppData.SceneEnvironmentLibrary>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(environmentLibrary, "Environment Library", "Get Environment Library Failed - Environment Library is Not Initialized In The Editor Inspector Panel - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(environmentLibrary.Initialized());

                if(callbackResults.Success())
                {
                    callbackResults.result = $"Get Environment Library Success - Environment Library Has Been Initialized With : {environmentLibrary.GetIncludedSceneEnvironments().GetData().Count}";
                    callbackResults.data = environmentLibrary;
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

        #endregion
    }
}