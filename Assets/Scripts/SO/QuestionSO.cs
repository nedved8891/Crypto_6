using UnityEngine;
using Data;

namespace SO
{
    [CreateAssetMenu(fileName = "NewQuestion", menuName = "Question")]
    public class QuestionSO : ScriptableObject
    {
        public int number;
    
        [TextArea(3, 10)]
        public string text;

        public Answer[] answers;
    }
}