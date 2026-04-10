using UnityEngine;
using UnityEngine.UI;

namespace Views.Menu
{
    public class HintView : MonoBehaviour
    {
        [SerializeField] private string txt;
        [SerializeField] private Text hintText;
        [SerializeField] private float typingSpeed = 0.05f;

        private Coroutine typingCoroutine;
    
        private string targetText = "";
        private float timeElapsed = 0f;
        private int currentIndex = 0;

        public bool IsTypingCompleted { get; private set; }

        public void SetHintText()
        {
            if (hintText != null)
            {
                targetText = txt;
                hintText.text = "";
                currentIndex = 0;
                timeElapsed = 0f;
                IsTypingCompleted = false;
            }
        }

        private void Update()
        {
            if (!IsTypingCompleted && currentIndex < targetText.Length)
            {
                timeElapsed += Time.deltaTime;

                if (timeElapsed >= typingSpeed)
                {
                    hintText.text += targetText[currentIndex];
                    currentIndex++;
                    timeElapsed = 0f;
                }

                if (currentIndex >= targetText.Length)
                {
                    IsTypingCompleted = true;
                }
            }
        }
    }
}