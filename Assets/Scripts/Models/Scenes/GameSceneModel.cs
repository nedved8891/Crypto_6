using System;
using Utilities;
using SO;
using Types;
using UnityEngine;

namespace Models.Scenes
{
    public class GameSceneModel
    {
        private bool _isCorrectAnswer;
        private int _currentContinent;
        private int _difficulty;
        private int _level;
        private int _question;
        private int _questionsCount;
        private ContinentSO _continent;

        public int CorrectAnswerIndex => GetCorrectAnswerIndex();
        public bool IsWin => _isCorrectAnswer;

        public GameSceneModel(ContinentSO[] continents)
        {
            _currentContinent = GameUtility.GetCurrentContinent();
            _difficulty = GameUtility.GetCurrentContinentDifficulty();
            _level = GameUtility.GetSelectedLevel();
            _question = 0;

            _continent = continents[_currentContinent];

            _questionsCount = continents[_currentContinent].difficulties[_difficulty].levels[_level].questions.Length;
        }

        public void TrySetLevelCompleted()
        {
            if (!IsWin)
            {
                return;
            }

            LevelStatusUtility.SetLevelStatus(_continent.continentName, (DifficultyType)_difficulty, _level, LevelStatusType.Completed, _continent.difficulties[_difficulty].levels.Length);
            LevelStatusUtility.SetUnlockedLevelIndex(_continent.continentName, (DifficultyType)_difficulty, _level+1);
        }

        public void SetNextPage()
        {
            SelectLevelUtility.SetPageLevel();
        }

        public bool TryUpdateQuestion()
        {
            _question++;

            return _question < _questionsCount;
        }
        
        public bool IsTrueAnswer(int index)
        {
            int trueAnswerIndex = GetCorrectAnswerIndex();

            _isCorrectAnswer = index == trueAnswerIndex;
            
            return _isCorrectAnswer;
        }

        public string GetAnswer(int index)
        {
            string text = $@"{index+1}. {_continent.difficulties[_difficulty].levels[_level]
                .questions[_question].answers[index].text}";

            return text;
        }

        public string GetQuestion()
        {
            string text = _continent.difficulties[_difficulty].levels[_level]
                .questions[_question].text;

            return text;
        }

        public string GetQuestionCounterText()
        {
            string text = $"Question {_question+1}/{_questionsCount}";

            return text;
        }

        public string GetQuestionResultCounterText()
        {
            int questions = IsWin ? _question : _question - 1 >= 0 ? _question - 1 : 0;
            
            string text = $"Questions: {questions}/{_questionsCount}";

            return text;
        }

        public string GetTimerText(int sec)
        {
            string text = $"00:0{sec}";

            return text;
        }

        private int GetCorrectAnswerIndex()
        {
            return Array.FindIndex(_continent.difficulties[_difficulty].levels[_level].questions[_question].answers,
                x => x.isTrue);
        }
    }
}