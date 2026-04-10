using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Types;

namespace Views.SelectLevel
{
    public class DifficultyBtn : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _btnSprites;
        
        private Button _btn;

        public event Action<DifficultyBtn> OnPressBtn; 

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _btn ??= GetComponent<Button>();
            
            _btn.onClick.AddListener(Notification);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveAllListeners();
        }

        public void SetState(DifficultyStatusType type)
        {
            switch (type)
            {
                case DifficultyStatusType.Locked:
                    SetLockedState();
                    break;
                case DifficultyStatusType.Unlocked:
                    SetUnlockedState();
                    break;
            }
        }

        private void SetLockedState()
        {
            SetSprite(1);
            SetBtnActive(false);
        }
        
        private void SetUnlockedState()
        {
            SetSprite(0);
            SetBtnActive(true);
        }

        private void SetSprite(int index)
        {
            _btn ??= GetComponent<Button>();

            _btn.image.sprite = _btnSprites[index];
        }

        private void SetBtnActive(bool value)
        {
            _btn ??= GetComponent<Button>();

            if (_btn != null) _btn.interactable = value;
        }

        private void Notification()
        {
            OnPressBtn?.Invoke(this);
        }
    }
}