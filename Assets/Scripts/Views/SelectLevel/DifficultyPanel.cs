using System;
using System.Collections.Generic;

using UnityEngine;
using Types;

namespace Views.SelectLevel
{
    public class DifficultyPanel : MonoBehaviour
    {
        [SerializeField] 
        private List<DifficultyBtn> _difficultyBtns;

        public event Action<int> PressDifficultyBtn;

        private void OnEnable()
        {
            _difficultyBtns.ForEach(btn => btn.OnPressBtn += OnPressDifficultyBtn);
        }

        private void OnDisable()
        {
            _difficultyBtns.ForEach(btn => btn.OnPressBtn -= OnPressDifficultyBtn);
        }

        public void SetStateBtns(List<DifficultyStatusType> types)
        {
            for (int i = 0; i < types.Count; i++)
            {
                _difficultyBtns[i].SetState(types[i]);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnPressDifficultyBtn(DifficultyBtn btn)
        {
            int index = _difficultyBtns.IndexOf(btn);
            
            Notification(index);
        }

        private void Notification(int index)
        {
            PressDifficultyBtn?.Invoke(index);
        }
    }
}