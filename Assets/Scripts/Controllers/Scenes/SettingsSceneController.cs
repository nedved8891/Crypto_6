using UnityEngine;
using UnityEngine.UI;

using Views.Settings;
using Sounds;
using Types;
using Views.General;

namespace Controllers.Scenes
{
    public class SettingsSceneController : AbstractSceneController
    {
        [Space(5)] [Header("Panels")] 
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private PanelView _privacyPanel;
        
        [Space(5)][Header("Buttons")]
        [SerializeField] 
        private SwitcherBtn _soundSwitcherView;
        [SerializeField] 
        private SwitcherBtn _musicSwitcherView;
        [SerializeField] 
        private Button _ppBtn;
        [SerializeField] 
        private Button _backBtn;
        
        protected override void OnSceneEnable()
        {
            
        }

        protected override void OnSceneStart()
        {
            UpdateSoundBtnSprite();
            UpdateMusicBtnSprite();
        }

        protected override void OnSceneDisable()
        {
            
        }

        protected override void Subscribe()
        {
            _ppBtn.onClick.AddListener(OnPressPrivacyBtn);
            _backBtn.onClick.AddListener(OnPressBackBtn);

            _soundSwitcherView.PressBtnAction += ChangeSoundState;
            _musicSwitcherView.PressBtnAction += ChangeMusicState;
        }

        protected override void Initialize()
        {
            
        }

        protected override void Unsubscribe()
        {
            _ppBtn.onClick.RemoveAllListeners();
            _backBtn.onClick.RemoveAllListeners();
            
            _soundSwitcherView.PressBtnAction += ChangeSoundState;
            _musicSwitcherView.PressBtnAction += ChangeMusicState;
        }

        private void UpdateSoundBtnSprite()
        {
            _soundSwitcherView.SetSprite(SoundsStates.CanPlaySound ? 0 : 1);
        }

        private void UpdateMusicBtnSprite()
        {
            _musicSwitcherView.SetSprite(SoundsStates.CanPlayMusic ? 0: 1);
        }

        private void ChangeSoundState()
        {
            SoundsStates.ChangeSoundsState();
            
            base.SetClickClip();
            
            UpdateSoundBtnSprite();
        }

        private void ChangeMusicState()
        {
            base.SetClickClip();

            SoundsStates.ChangeMusicState();
            
            base.PlayMusic();
            
            UpdateMusicBtnSprite();
        }

        private void OnPressBackBtn()
        {
            base.UnloadScene(SceneType.SettingsScene);
        }

        private void OnPressPrivacyBtn()
        {
            base.SetClickClip();
            
            SetActivePanel(_mainPanel, false);
            SetActivePanel(_privacyPanel.gameObject, true);

            _privacyPanel.PressBtnAction += OnReceiveAnswerPrivacyPanel;
        }

        private void OnReceiveAnswerPrivacyPanel(int answer)
        {
            base.SetClickClip();
            
            _privacyPanel.PressBtnAction -= OnReceiveAnswerPrivacyPanel;
            
            SetActivePanel(_privacyPanel.gameObject, false);
            SetActivePanel(_mainPanel, true);
        }

        private void SetActivePanel(GameObject go, bool active)
        {
            go.SetActive(active);
        }
    }
}