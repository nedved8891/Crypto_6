using UnityEngine;
using UnityEngine.UI;

namespace Views.SelectLevel
{
    public class DescriptionView : MonoBehaviour
    {
        [SerializeField]
        private Image _image;
        [SerializeField] 
        private Text _text;

        public void SetDescription(Sprite sprite, string text)
        {
            _image.sprite = sprite;
            _text.text = text;
            
            SetImageSize();
        }

        private void SetImageSize()
        {
            _image.SetNativeSize();
            
            RectTransform rect = _image.rectTransform;
            
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.rect.width/4);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.rect.height/4);
        }
    }
}