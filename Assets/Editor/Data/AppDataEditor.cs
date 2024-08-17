using System.IO;
using UnityEngine; 
using UnityEditor;

namespace Com.RedicalGames.Filar
{
    public class AppDataEditor : Editor
    {
        #region Main

        [MenuItem("Filar/App/Clear Data #R", true)]
        private static bool CanClearAppData()
        {
            return File.Exists(GetAppDataMetaStoragePath()) || Directory.Exists(GetAppDataStorageDirectory());
        }

        [MenuItem("Filar/App/Clear Data #R")]
        private static void CreateContentLoadManager()
        {
            Directory.Delete(GetAppDataStorageDirectory(), true);
            File.Delete(GetAppDataMetaStoragePath());
        }

        private static string GetAppDataStorageDirectory() => Path.Combine(Application.dataPath, "App");
        private static string GetAppDataMetaStoragePath() => Path.Combine(Application.dataPath, "App.meta");

        #endregion
    }
}