using UnityEngine;
using Types;

namespace Utilities
{
    public static class GameUtility
    {
        private const string CurrentContinentKey = "GameUtility.CurrentContinent";
        private const string DifficultyKey = "GameUtility.Difficulty";
        private const string SelectedLevelKey = "GameUtility.SelectedLevel";
        
        public static int GetCurrentContinent()
        {
            int currentContinent = PlayerPrefs.GetInt(CurrentContinentKey, 0);

            return currentContinent;
        }

        public static void SetCurrentContinent(ContinentType type)
        {
            PlayerPrefs.SetInt(CurrentContinentKey, (int)type);
        }

        public static int GetCurrentContinentDifficulty()
        {
            int difficulty = PlayerPrefs.GetInt($"{(ContinentType)GetCurrentContinent()+DifficultyKey}");

            return difficulty;
        }
        
        public static void SetCurrentContinentDifficulty(int value)
        {
            PlayerPrefs.SetInt($"{(ContinentType)GetCurrentContinent()+DifficultyKey}", value);
        }

        public static int GetSelectedLevel()
        {
            int index = PlayerPrefs.GetInt($"{(ContinentType)GetCurrentContinent() + SelectedLevelKey}", 0);

            return index;
        }

        public static void SetSelectedLevel(int value)
        {
            PlayerPrefs.SetInt($"{(ContinentType)GetCurrentContinent() + SelectedLevelKey}", value);
        }
    }
}