using UnityEngine;
using UnityEngine.UI;

using Views.SelectLevel;
using Models.Scenes;
using SO;
using Types;

namespace Controllers.Scenes
{
    public class SelectLevelSceneController : AbstractSceneController
    {
        [Space(5)] [Header("Views")] 
        [SerializeField] private DescriptionView _descriptionView;

        [Space(5)] [Header("Panels")] 
        [SerializeField] private DifficultyPanel _difficultyPanel;
        [SerializeField] private LevelsPanel _levelsPanel;
        
        [Space(5)] [Header("SO")] 
        [SerializeField] private ContinentSO[] _continents;

        [Space(5)] [Header("Buttons")]
        [SerializeField] private Button _backbtn;
        [SerializeField] private Button _settingsBtn;

        private SelectLevelSceneModel _model;
        
        protected override void OnSceneEnable()
        {
            SetDescription();
            CheckFirstPage();
        }

        protected override void OnSceneStart()
        {
            
        }

        protected override void OnSceneDisable()
        {
            
        }

        protected override void Initialize()
        {
            _model = new SelectLevelSceneModel(_continents);
        }

        protected override void Subscribe()
        {
            _backbtn.onClick.AddListener(OnPressBackBtn);
            _settingsBtn.onClick.AddListener(OnPressSettingsBtn);
            
            _difficultyPanel.PressDifficultyBtn += OnPressDifficultyBtn;
            _levelsPanel.PressLevelBtn += OnPressLevelBtn;
        }

        protected override void Unsubscribe()
        {
            _backbtn.onClick.RemoveAllListeners();
            _settingsBtn.onClick.RemoveAllListeners();
            
            _difficultyPanel.PressDifficultyBtn -= OnPressDifficultyBtn;
            _levelsPanel.PressLevelBtn -= OnPressLevelBtn;
        }

        private void SetDescription()
        {
            _descriptionView.SetDescription(_model.DescriptionSprite, _model.DescriptionName);
        }

        private void OpenDifficultyPanel()
        {
            _difficultyPanel.SetStateBtns(_model.GetDifficultyStatusTypes());
            _difficultyPanel.Show();
        }

        private void OpenLevelsPanel()
        {
            _levelsPanel.SetLevelStates(_model.GetLevelTypes());
            _levelsPanel.Show();
        }

        private void CloseDifficultyPanel()
        {
            _difficultyPanel.Hide();
        }

        private void CloseLevelsPanel()
        {
            _levelsPanel.Hide();
        }

        private void OnPressDifficultyBtn(int index)
        {
            base.SetClickClip();
            
            _model.SetDifficulty(index);
            _model.UpdatePageIndex(1);
            
            CloseDifficultyPanel();
            OpenLevelsPanel();
        }

        private void OnPressLevelBtn(int index)
        {
            _model.SetLevelIndex(index);
            
            LoadGameScene();
        }

        private void OnPressBackBtn()
        {
            switch (_model.PageIndex)
            {
                case 0:
                    LoadMenuScene();
                    break;
                case 1:
                    base.SetClickClip();
                    _model.UpdatePageIndex(-1);
                    CloseLevelsPanel();
                    OpenDifficultyPanel();
                    break;
            }
        }

        private void OnPressSettingsBtn()
        {
            base.LoadScene(SceneType.SettingsScene, false);
        }

        private void CheckFirstPage()
        {
            if (_model.PageIndex == 0)
            {
                OpenDifficultyPanel();
            }
            else
            {
                OpenLevelsPanel();
            }
        }

        private void LoadGameScene()
        {
            base.LoadScene(SceneType.GameScene);
        }

        private void LoadMenuScene()
        {
            base.LoadScene(SceneType.MenuScene);
        }
    }
}