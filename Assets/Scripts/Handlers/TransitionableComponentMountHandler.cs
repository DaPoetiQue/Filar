using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class TransitionableComponentMountHandler : AppMonoBaseClass
    {
        #region Components

        [Space(5)]
        [SerializeField]
        private AppData.ViewSpaceType viewSpace = AppData.ViewSpaceType.None;

        #endregion

        #region Main

        public AppData.CallbackData<AppData.ViewSpaceType> GetViewSpace()
        {
            var callbackResults = new AppData.CallbackData<AppData.ViewSpaceType>(AppData.Helpers.GetAppEnumValueValid(viewSpace, "View Space", $"Get View Space Failed - View Space Value Is Set To Default : {viewSpace} - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = $"Get View Space Success - View Space Value Is Set To : {viewSpace}.";
                callbackResults.data = viewSpace;
            }

            return callbackResults;
        }

        public AppData.CallbackData<object> GetPosition()
        {
            var callbackResults = new AppData.CallbackData<object>(GetViewSpace());

            if (callbackResults.Success())
            {
                switch(GetViewSpace().GetData())
                {
                    case AppData.ViewSpaceType.ScreenSpace:

                        callbackResults.data = transform.GetComponent<RectTransform>().anchoredPosition;

                        break;

                    case AppData.ViewSpaceType.WorldSpace:

                        callbackResults.data = transform.position;

                        break;
                }

                callbackResults.result = $"Getting Position Of : {GetViewSpace().GetData()} View.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }
        public AppData.CallbackData<object> GetScale()
        {
            var callbackResults = new AppData.CallbackData<object>(GetViewSpace());

            if (callbackResults.Success())
            {
                switch (GetViewSpace().GetData())
                {
                    case AppData.ViewSpaceType.ScreenSpace:

                        callbackResults.data = transform.GetComponent<RectTransform>().sizeDelta;

                        break;

                    case AppData.ViewSpaceType.WorldSpace:

                        callbackResults.data = transform.localScale;

                        break;
                }

                callbackResults.result = $"Getting Scale Of : {GetViewSpace().GetData()} View.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }
        public AppData.CallbackData<object> GetRotation()
        {
            var callbackResults = new AppData.CallbackData<object>(GetViewSpace());

            if (callbackResults.Success())
            {
                switch (GetViewSpace().GetData())
                {
                    case AppData.ViewSpaceType.ScreenSpace:

                        callbackResults.data = transform.localEulerAngles;

                        break;

                    case AppData.ViewSpaceType.WorldSpace:

                        callbackResults.data = transform.rotation;

                        break;
                }

                callbackResults.result = $"Getting Rotation Of : {GetViewSpace().GetData()} View.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public RectTransform GetWidgetRect() => transform.GetComponent<RectTransform>();

        #endregion
    }
}