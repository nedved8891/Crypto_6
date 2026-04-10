using System;
using UnityEngine;
using Types;

namespace Utilities
{
    public static class LevelStatusUtility
    {
        public static void SetLevelStatus(ContinentType type, DifficultyType difficulty, int levelIndex, LevelStatusType status, int levelsCount)
        {
            string key = $"{type}_{difficulty}_{levelIndex}_Levels";

            if (PlayerPrefs.GetInt(key, (int)LevelStatusType.Locked) == (int)LevelStatusType.Completed)
            {
                return;
            }

            if (levelIndex == levelsCount - 1 && difficulty == DifficultyType.Hard)
            {
                ContinentsStatusUtility.SetOpenedContinent(type);
            }

            PlayerPrefs.SetInt(key, (int)status);
        }

        public static LevelStatusType GetLevelStatusType(ContinentType type, DifficultyType difficulty, int levelIndex)
        {
            string key = $"{type}_{difficulty}_{levelIndex}_Levels";

            int levelData = PlayerPrefs.GetInt(key, (int)LevelStatusType.Locked);

            return (LevelStatusType)levelData;
        }

        public static void SetUnlockedLevelIndex(ContinentType type, DifficultyType difficulty, int levelIndex)
        {
            string key = $"{type}_{difficulty}_Unlocked_Level";
            
            if (levelIndex <  PlayerPrefs.GetInt(key, 0))
            {
                return;
            }
            
            PlayerPrefs.SetInt(key, levelIndex);
        }

        public static int GetUnlockedLevelIndex(ContinentType type, DifficultyType difficulty)
        {
            string key = $"{type}_{difficulty}_Unlocked_Level";

            return PlayerPrefs.GetInt(key, 0);
        }
    }
}