using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Types;

namespace Views.SelectLevel
{
    public class LevelBtn : MonoBehaviour
    {
        [SerializeField] 
        private List<GameObject> _stateObjects;

        private Button _btn;

        public event Action<LevelBtn> OnPressBtn;

        private void OnEnable()
        {
            _btn.onClick.AddListener(Notification);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveAllListeners();
        }

        public void SetLevel(LevelStatusType type)
        {
            switch (type)
            {
                case LevelStatusType.Locked:
                    SetLockedState();
                    break;
                case LevelStatusType.Unlocked:
                    SetUnlockedState();
                    break;
                case LevelStatusType.Completed:
                    SetCompletedState();
                    break;
            }
        }

        private void SetLockedState()
        {
            SetStateObjectActive(1);
            SetBtnActive(false);
        }

        private void SetUnlockedState()
        {
            SetStateObjectActive(0);
            SetBtnActive(true);
        }

        private void SetCompletedState()
        {
            SetStateObjectActive(2);
            SetBtnActive(true);
        }

        private void SetStateObjectActive(int index)
        {
            _stateObjects.ForEach(o => o.SetActive(false));
            
            _stateObjects[index].SetActive(true);
        }

        private void SetBtnActive(bool value)
        {
            _btn ??= GetComponent<Button>();
            
            _btn.interactable = value;
        }

        private void Notification()
        {
            OnPressBtn?.Invoke(this);
        }
    }
}