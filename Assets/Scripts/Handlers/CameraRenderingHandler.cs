using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class CameraRenderingHandler : MonoBehaviour
    {
        #region Components

        [SerializeField]
        Color wirefarmeColor = Color.white;

        DatabaseManager assetsManager;

        #endregion

        #region Unity Callback

        void Start() => Init();

        #endregion

        #region Main

        void Init()
        {
            assetsManager = DatabaseManager.Instance;
        }

        //void OnPreRender()
        //{

        //    GL.Color(wirefarmeColor);
        //    GL.wireframe = RenderWireframe();
        //}

        //void OnPostRender()
        //{
        //    GL.Color(wirefarmeColor);
        //    GL.wireframe = RenderWireframe();
        //}

        bool RenderWireframe()
        {
            bool renderWireframes = false;

            if (assetsManager != null)
            {
                AppData.SceneAssetRenderMode renderMode = assetsManager.GetSceneAssetRenderMode();

                if (renderMode == AppData.SceneAssetRenderMode.Wireframe)
                    renderWireframes = true;
            }

            return renderWireframes;
        }

        #endregion
    }
}
