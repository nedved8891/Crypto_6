using UnityEngine;

namespace Views.Game
{
    public class QuestionView : MonoBehaviour
    {
        [SerializeField] 
        private TextUpdater _questionTextUpdater;
        [SerializeField] 
        private ResultImageAnimation _resultImageAnimation;

        public void UpdateText(string text)
        {
            _questionTextUpdater.UpdateText(text);
        }

        public void SetState(bool state)
        {
            _questionTextUpdater.gameObject.SetActive(state);
            _resultImageAnimation.gameObject.SetActive(!state);
        }

        public void ShowResult(bool value)
        {
            _resultImageAnimation.Show(value);
        }
    }
}