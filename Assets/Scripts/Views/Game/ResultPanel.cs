using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views.General;

namespace Views.Game
{
    public class ResultPanel : PanelView
    {
        [SerializeField] 
        private Image _descriptionImage;
        [SerializeField] 
        private Image _infoImage;
        [SerializeField] 
        private List<Sprite> _descriptionSprites;
        [SerializeField] 
        private List<Sprite> _infoSprites;
        [SerializeField] 
        private List<Sprite> _btnSprites;
        [SerializeField] 
        private TextUpdater _questionCountText;

        public void SetState(bool value)
        {
            int index = value ? 0 : 1;
            
            SetBtnSprite(index);
            SetImageSprite(index);
        }

        public void SetText(string value)
        {
            _questionCountText.UpdateText(value);
        }

        private void SetImageSprite(int index)
        {
            _descriptionImage.sprite = _descriptionSprites[index];
            _infoImage.sprite = _infoSprites[index];
            
            //SetImageSize(_descriptionImage);
            //SetImageSize(_infoImage);
        }

        private void SetBtnSprite(int index)
        {
            base.Btns[0].image.sprite = _btnSprites[index];
        }

        private void SetImageSize(Image image)
        {
            image.SetNativeSize();

            RectTransform rect = image.rectTransform;
            
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.rect.width /4);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.rect.height /4);
        }
    }
}