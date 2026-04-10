using System.Collections.Generic;
using UnityEngine;

using Views.Menu;
using Models.Scenes;
using Types;
using UnityEngine.UI;
using Views.General;

namespace Controllers.Scenes
{
    public class MenuSceneController : AbstractSceneController
    {
        [Header("Views")]
        [SerializeField] private List<SpriteButton> _spriteBtns;
        [SerializeField] private PanelView _infoPanel;
        [SerializeField] private OnboardingPanel _onboardingPanel;

        [Space(5)] [Header("Buttons")] 
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _infoBtn;
        [SerializeField] private Button _settingsBtn;

        [Space(5)] [Header("MainPanelObjects")]
        [SerializeField] private List<GameObject> _mainPanelObjects;

        private MenuSceneModel _model;
        
        protected override void OnSceneEnable()
        {
            CheckActiveSpriteBtns();
        }

        protected override void OnSceneStart()
        {
            ResetContinentsStates();
        }

        protected override void OnSceneDisable()
        {
            
        }

        protected override void Initialize()
        {
            _model = new MenuSceneModel();
            
            TryOpenOnBoarding();
        }

        protected override void Subscribe()
        {
            _playBtn.onClick.AddListener(OnPressPlayBtn);
            _infoBtn.onClick.AddListener(OnPressInfoBtn);
            _settingsBtn.onClick.AddListener(OnPressSettingsBtn);
            _spriteBtns.ForEach(btn => btn.OnButtonClicked += OnPressContinentBtn);
        }

        protected override void Unsubscribe()
        {
            _playBtn.onClick.RemoveAllListeners();
            _infoBtn.onClick.RemoveAllListeners();
            _settingsBtn.onClick.RemoveAllListeners();
            _spriteBtns.ForEach(btn => btn.OnButtonClicked -= OnPressContinentBtn);
        }

        private void OnPressContinentBtn(SpriteButton btn)
        {
            base.SetClickClip();
            
            _model.SetContinentType(btn.Type);
            
            ResetContinentsStates();
            
            btn.SetState(ContinentStateType.Selected);

            if (_playBtn.interactable)
            {
                return;
            }

            _playBtn.interactable = true;
        }

        private void OnPressPlayBtn()
        {
            _model.SetSelectLevelSceneSettings();
            _model.SetGameContinent();
            
            LoadSelectLevelScene();
        }

        private void OnPressInfoBtn()
        {
            base.SetClickClip();
            
            SetActiveMainPanelObjects(false);
            OpenInfoPanel();
        }

        private void OnPressSettingsBtn()
        {
            base.LoadScene(SceneType.SettingsScene, false);
        }

        private void OnReceiveAnswerInfoPanel(int answer)
        {
            base.SetClickClip();
            
            _infoPanel.PressBtnAction -= OnReceiveAnswerInfoPanel;
            SetActiveInfoPanel(false);
            SetActiveMainPanelObjects(true);
        }

        private void LoadSelectLevelScene()
        {
            base.LoadScene(SceneType.SelectLevelScene);
        }

        private void ResetContinentsStates()
        {
            _spriteBtns.ForEach(btn => btn.SetState(ContinentStateType.Default));
        }

        private void CheckActiveSpriteBtns()
        {
            for (int i = 0; i < _spriteBtns.Count; i++)
            {
                ContinentStatusType type = _model.GetContinentStatus(i);
                
                bool status = type == ContinentStatusType.Opened;
            
                _spriteBtns[i].gameObject.SetActive(status);
            }
        }

        private void OpenInfoPanel()
        {
            _infoPanel.PressBtnAction += OnReceiveAnswerInfoPanel;
            SetActiveInfoPanel(true);
        }

        private void SetActiveMainPanelObjects(bool value)
        {
            _mainPanelObjects.ForEach(go => go.SetActive(value));
        }

        private void SetActiveInfoPanel(bool value)
        {
            _infoPanel.gameObject.SetActive(value);
        }

        private void TryOpenOnBoarding()
        {
            if (!_model.CanShowOnBoarding)
            {
                return;
            }
            
            OpenOnBoarding();
        }

        private void OpenOnBoarding()
        {
            _onboardingPanel.PressBtnAction += OnReceiveAnswerOnboardingPanel;
            SetActiveOnboardingPanel(true);
        }

        private void OnReceiveAnswerOnboardingPanel(int answer)
        {
            _onboardingPanel.PressBtnAction -= OnReceiveAnswerOnboardingPanel;
            SetActiveOnboardingPanel(false);
            
            _model.SetOnBoardingStatusDisable();
        }

        private void SetActiveOnboardingPanel(bool value)
        {
            _onboardingPanel.gameObject.SetActive(value);
        }
    }
}