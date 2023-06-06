using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class AssetPreviewHandler : AppData.Scene3DPreviewer
    {
        #region Unity Callbacks

        void Start() => Init();

        void OnEnable() => ActionEventsSubscription(true);

        void OnDisable() => ActionEventsSubscription(false);

        #endregion

        #region Main

        void Init()
        {
            assetsManager = SceneAssetsManager.Instance;

            if (initialVisibilityState == false)
                Hide();
        }

        void ActionEventsSubscription(bool subscribe)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnClearPreviewedSceneAssetObjectEvent += OnClearPreviewedSceneAssetObjectEvent;
                AppData.ActionEvents._OnCreatedAssetDataEditEvent += OnSceneAssetScreenPreviewEvent;
                AppData.ActionEvents._OnScreenChangeEvent += OnScreenChangeEvent;
                AppData.ActionEvents._OnScreenExitEvent += OnScreenExitEvent;
            }
            else
            {
                AppData.ActionEvents._OnClearPreviewedSceneAssetObjectEvent -= OnClearPreviewedSceneAssetObjectEvent;
                AppData.ActionEvents._OnCreatedAssetDataEditEvent -= OnSceneAssetScreenPreviewEvent;
                AppData.ActionEvents._OnScreenChangeEvent -= OnScreenChangeEvent;
                AppData.ActionEvents._OnScreenExitEvent -= OnScreenExitEvent;
            }
        }

        #endregion
    }
}
