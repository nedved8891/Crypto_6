using System.Collections.Generic;
using UnityEngine;

using SO;
using Types;
using Utilities;

namespace Models.Scenes
{
    public class SelectLevelSceneModel
    {
        private int _difficulty;
        private int _currentContinent;
        private int _pageIndex;
        private ContinentSO _continent;
        
        public string DescriptionName => ConvertContinentTypeToName(_continent.continentName);
        public int PageIndex => _pageIndex;
        public Sprite DescriptionSprite => _continent.continentSprite;
        
        public SelectLevelSceneModel(ContinentSO[] continents)
        {
            _pageIndex = SelectLevelUtility.FirstPageIndex;
            _currentContinent = GameUtility.GetCurrentContinent();
            _continent = continents[_currentContinent];
        }

        public void SetDifficulty(int index)
        {
            _difficulty = index;
            GameUtility.SetCurrentContinentDifficulty(index);
        }

        public void SetLevelIndex(int index)
        {
            GameUtility.SetSelectedLevel(index);
        }

        public void UpdatePageIndex(int direction)
        {
            _pageIndex += direction;
        }

        public List<DifficultyStatusType> GetDifficultyStatusTypes()
        {
            List<DifficultyStatusType> types = new List<DifficultyStatusType>();

            for (int i = 0; i < 3; i++)
            {
                DifficultyStatusType type = DifficultyStatusUtility.GetDifficultyStatus(_continent.continentName,
                    (DifficultyType)i, _continent.difficulties[i].levels.Length);
                
                types.Add(type);
            }

            return types;
        }

        public List<LevelStatusType> GetLevelTypes()
        {
            List<LevelStatusType> types = new List<LevelStatusType>();
            
            _difficulty = GameUtility.GetCurrentContinentDifficulty();

            for (int i = 0; i < _continent.difficulties[_difficulty].levels.Length; i++)
            {
                LevelStatusType type =
                    LevelStatusUtility.GetLevelStatusType(_continent.continentName, (DifficultyType)_difficulty, i);
                
                types.Add(type);
            }

            int unlockedLevelIndex =
                LevelStatusUtility.GetUnlockedLevelIndex(_continent.continentName, (DifficultyType)_difficulty);

            if (unlockedLevelIndex < _continent.difficulties[_difficulty].levels.Length)
            {
                types[unlockedLevelIndex] =
                    LevelStatusType.Unlocked;
            }

            return types;
        }

        private string ConvertContinentTypeToName(ContinentType type)
        {
            string name = "";
            
            switch (type)
            {
                case ContinentType.EUR:
                    name = "EUROPE";
                    break;
                case ContinentType.AS:
                    name = "ASIA";
                    break;
                case ContinentType.NAm:
                    name = "NORTH AMERICA";
                    break;
            }

            return name;
        }
    }
}