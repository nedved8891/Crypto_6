using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

using Views.Game;
using Models.Scenes;
using SO;
using Types;

namespace Controllers.Scenes
{
    public class GameSceneController : AbstractSceneController
    {
        [Space(5)] [Header("Views")] 
        [SerializeField] private QuestionView _questionView;
        [SerializeField] private List<AnswerBtn> _answerBtns;
        [SerializeField] private TextUpdater _questionCounter;
        [SerializeField] private TextUpdater _timerUpdater;

        [Space(5)] [Header("Panels")] 
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private ResultPanel _resultPanel;

        [Space(5)] [Header("SO")] 
        [SerializeField] private ContinentSO[] _continents;

        [Space(5)] [Header("Buttons")]
        [SerializeField] private Button _backBtn;
        [SerializeField] private Button _settingsBtn;
        
        private GameSceneModel _model;
        private Coroutine _timerCoroutine;
        
        protected override void OnSceneEnable()
        {
            SetQuestion();
        }

        protected override void OnSceneStart()
        {
            _timerCoroutine = StartCoroutine(StartTimer());
        }

        protected override void OnSceneDisable()
        {
            
        }

        protected override void Initialize()
        {
            _model = new GameSceneModel(_continents);
        }

        protected override void Subscribe()
        {
            _backBtn.onClick.AddListener(OnPressBackBtn);
            _settingsBtn.onClick.AddListener(OnPressSettingsBtn);
            
            _answerBtns.ForEach(btn => btn.OnPressBtn += OnPressAnswerBtn);
        }

        protected override void Unsubscribe()
        {
            _backBtn.onClick.RemoveAllListeners();
            _settingsBtn.onClick.RemoveAllListeners();
            
            _answerBtns.ForEach(btn => btn.OnPressBtn -= OnPressAnswerBtn);
        }

        private void SetQuestionState(bool value)
        {
            _questionView.SetState(value);
        }

        private void SetQuestion()
        {
            SetQuestionState(true);
            UpdateQuestion();
            UpdateAnswers();
            UpdateQuestionCounter();
        }

        private void UpdateQuestion()
        {
            string text = _model.GetQuestion();

            _questionView.UpdateText(text);
        }

        private void UpdateAnswers()
        {
            for (int i = 0; i < _answerBtns.Count; i++)
            {
                string text = _model.GetAnswer(i);
                
                _answerBtns[i].UpdateText(text);
            }
        }

        private void UpdateQuestionCounter()
        {
            _questionCounter.UpdateText(_model.GetQuestionCounterText());
        }

        private void OnPressAnswerBtn(AnswerBtn btn)
        {
            base.SetClickClip();
            
            DisableAnswerBtns();
            
            StopTimer();
            
            int index = _answerBtns.IndexOf(btn);
            
            _answerBtns[index].SetPressState();

            StartCoroutine(DelayShowResult(index));
        }

        private void OnPressSettingsBtn()
        {
            base.LoadScene(SceneType.SettingsScene, false);
        }

        private void OnPressBackBtn()
        {
            _model.SetNextPage();
            LoadSelectLevelScene();
        }

        private void CheckAnswer(int index)
        {
            bool isCorrect = _model.IsTrueAnswer(index);
            
            if (!isCorrect)
            {
                base.PlaySound(AudioNames.LoseClip);
                _answerBtns[index].SetIncorrectState();
            }
            else
            {
                base.PlaySound(AudioNames.WinClip);
            }
            
            _answerBtns[_model.CorrectAnswerIndex].SetCorrectState();
            _questionView.SetState(false);
            _questionView.ShowResult(isCorrect);

            StartCoroutine(DelayOpenResultPanel());
        }

        private void CheckEndGame()
        {
            if (_model.TryUpdateQuestion())
            {
                SetQuestion();
            }
            else
            {
                _mainPanel.gameObject.SetActive(false);

                _model.TrySetLevelCompleted();

                OpenResultPanel(_model.IsWin);
            }
        }

        private void OpenResultPanel(bool isWin)
        {
            _resultPanel.SetState(isWin);
            _resultPanel.SetText(_model.GetQuestionResultCounterText());
            _resultPanel.PressBtnAction += OnReceiveAnswerResultPanel;
            _resultPanel.Show();
        }

        private void DisableAnswerBtns()
        {
            _answerBtns.ForEach(btn => btn.Disable());
        }

        private void OnReceiveAnswerResultPanel(int answer)
        {
            _resultPanel.PressBtnAction -= OnReceiveAnswerResultPanel;

            switch (answer)
            {
                case 0:
                    if (_model.IsWin)
                    {
                        _model.SetNextPage();
                        LoadSelectLevelScene();
                    }
                    else
                    {
                        RestartGame();
                    }
                    break;
                case 1:
                    LoadMenuScene();
                    break;
            }
        }

        private void StopTimer()
        {
            StopCoroutine(_timerCoroutine);
        }

        private void LoadSelectLevelScene()
        {
            base.LoadScene(SceneType.SelectLevelScene);
        }

        private void LoadMenuScene()
        {
            base.LoadScene(SceneType.MenuScene);
        }

        private void RestartGame()
        {
            base.LoadScene(SceneType.GameScene);
        }

        private IEnumerator DelayShowResult(int btnIndex)
        {
            yield return new WaitForSeconds(1);
            
            CheckAnswer(btnIndex);
        }

        private IEnumerator DelayOpenResultPanel()
        {
            yield return new WaitForSeconds(1);
            
            CheckEndGame();
        }

        private IEnumerator StartTimer()
        {
            int sec = 5;

            while (sec > 0)
            {
                yield return new WaitForSeconds(1);
                
                sec--;
                
                _timerUpdater.UpdateText(_model.GetTimerText(sec));
            }
            
            _mainPanel.gameObject.SetActive(false);
            OpenResultPanel(false);
        }
    }
}