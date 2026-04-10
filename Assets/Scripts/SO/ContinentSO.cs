using UnityEngine;
using Data;
using Types;

namespace SO
{
    [CreateAssetMenu(fileName = "NewContinent", menuName = "Continent")]
    public class ContinentSO : ScriptableObject
    {
        public ContinentType continentName;

        public Sprite continentSprite;

        public DifficultySettings[] difficulties;
    }
}