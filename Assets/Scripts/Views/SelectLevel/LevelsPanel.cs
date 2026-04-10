using System;
using System.Collections.Generic;

using UnityEngine;
using Types;

namespace Views.SelectLevel
{
    public class LevelsPanel : MonoBehaviour
    {
        [SerializeField] private List<LevelBtn> _btns;

        public event Action<int> PressLevelBtn;

        private void OnEnable()
        {
            _btns.ForEach(btn => btn.OnPressBtn += OnPressLevelBtn);
        }

        private void OnDisable()
        {
            _btns.ForEach(btn => btn.OnPressBtn -= OnPressLevelBtn);
        }

        public void SetLevelStates(List<LevelStatusType> types)
        {
            for (int i = 0; i < _btns.Count; i++)
            {
                _btns[i].SetLevel(types[i]);
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

        private void OnPressLevelBtn(LevelBtn btn)
        {
            int index = _btns.IndexOf(btn);
            
            Notification(index);
        }

        private void Notification(int index)
        {
            PressLevelBtn?.Invoke(index);
        }
    }
}