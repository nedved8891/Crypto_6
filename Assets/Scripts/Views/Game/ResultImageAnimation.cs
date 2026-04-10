using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Views.Game
{
    public class ResultImageAnimation : MonoBehaviour
    {
        [SerializeField] 
        private List<Sprite> _sprites;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Show(bool value)
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }

            int spriteIndex = value ? 0 : 1;

            _image.sprite = _sprites[spriteIndex];
            
            StartAnim();
        }

        private void StartAnim()
        {
            _image.color = new Color(1, 1, 1, 0);
            _image.transform.localScale = Vector3.zero;

            _image.enabled = true;
            
            _image.transform.DOScale(1, 0.6f).SetEase(Ease.OutBack);
            _image.DOFade(1, 0.6f);
        }
    }
}