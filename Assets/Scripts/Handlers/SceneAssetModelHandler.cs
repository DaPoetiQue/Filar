using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SceneAssetModelHandler : AppData.SceneAssetModel
    {
        #region Components

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main

        void Init()
        {
            value = this.gameObject;
        }

        #endregion
    }
}
