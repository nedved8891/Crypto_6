using Types;

namespace Data
{
    [System.Serializable]
    public class DifficultySettings
    {
        public DifficultyType difficulty;

        public Level[] levels;
    }
}