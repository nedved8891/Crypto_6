using Types;
using UnityEngine;

namespace Utilities
{
    public static class ContinentsStatusUtility
    {
        private const string LastOpenedContinentIndexKey ="ContinentsStatusUtility.ContinentIndex";
        
        public static ContinentStatusType GetContinentStatus(ContinentType type)
        {
            int lastOpenedContinent = PlayerPrefs.GetInt(LastOpenedContinentIndexKey, 0);

            return lastOpenedContinent >= (int)type ? ContinentStatusType.Opened : ContinentStatusType.Closed;
        }

        public static void SetOpenedContinent(ContinentType type)
        {
            PlayerPrefs.SetInt(LastOpenedContinentIndexKey, (int)type+1);
        }
    }
}