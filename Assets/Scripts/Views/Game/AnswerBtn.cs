using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Game
{
    [RequireComponent(typeof(Button))]
    public class AnswerBtn : MonoBehaviour
    {
        [SerializeField]
        private TextUpdater _text;
        [SerializeField] 
        private List<Sprite> _sprites;

        private Button _btn;
        
        public event Action<AnswerBtn> OnPressBtn;

        private void OnEnable()
        {
            _btn = GetComponent<Button>();
            
            _btn.onClick.AddListener(Notification);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveAllListeners();
        }

        public void UpdateText(string text)
        {
            _text.UpdateText(text);
        }

        public void Activate()
        {
            _btn.interactable = true;
        }

        public void Disable()
        {
            _btn.interactable = false;
        }

        public void SetDefaultState()
        {
            SetSprite(0);
            SetTextColor(true);
        }

        public void SetPressState()
        {
            SetSprite(1);
            SetTextColor(false);
        }

        public void SetCorrectState()
        {
            SetSprite(2);
            SetTextColor(false);
        }

        public void SetIncorrectState()
        {
            SetSprite(3);
            SetTextColor(false);
        }

        private void SetSprite(int index)
        {
            _btn.image.sprite = _sprites[index];
        }

        private void SetTextColor(bool isDefault)
        {
            //_text.SetColor(isDefault ? Color.white : Color.black);
        }

        private void Notification()
        {
            OnPressBtn?.Invoke(this);
        }
    }
}