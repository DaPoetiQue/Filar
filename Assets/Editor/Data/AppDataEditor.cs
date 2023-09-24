using System.IO;
using UnityEditor;

namespace Com.RedicalGames.Filar
{
    public class AppDataEditor : Editor
    {
        #region Components

        static string appDataMetaStoragePath = "C:\\Users\\hlula\\Documents\\Development\\Designar Main App\\Filar\\Assets\\App.meta";
        static string appDataStorageDirectory = "C:\\Users\\hlula\\Documents\\Development\\Designar Main App\\Filar\\Assets\\App";

        #endregion

        #region Main

        [MenuItem("Filar/App/Clear Data", true)]
        private static bool CanClearAppData()
        {
            return File.Exists(appDataMetaStoragePath);
        }

        [MenuItem("Filar/App/Clear Data #R")]
        private static void CreateContentLoadManager()
        {
            Directory.Delete(appDataStorageDirectory, true);
            File.Delete(appDataMetaStoragePath);
        }

        #endregion
    }
}