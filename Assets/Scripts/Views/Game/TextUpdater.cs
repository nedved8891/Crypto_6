using UnityEngine;
using UnityEngine.UI;

namespace Views.Game
{
    public class TextUpdater : MonoBehaviour
    {
        [SerializeField] private Text _text;

        public void UpdateText(string text)
        {
            if (_text == null)
            {
                Debug.LogError("Text is null");
                return;
            }

            _text.text = text;
        }

        public void SetColor(Color color)
        {
            _text.color = color;
        }
    }
}