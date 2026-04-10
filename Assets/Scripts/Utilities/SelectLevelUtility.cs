using UnityEngine;

namespace Utilities
{
    public static class SelectLevelUtility
    {
        private const string PageIndexKey = "SelectLevel.PageIndex";

        public static int FirstPageIndex => PlayerPrefs.GetInt(PageIndexKey, 0);
        
        public static void SetPageDifficulty()
        {
            SetPageIndex(0);
        }

        public static void SetPageLevel()
        {
            SetPageIndex(1);
        }

        private static void SetPageIndex(int index)
        {
            PlayerPrefs.SetInt(PageIndexKey, index);
        }
    }
}