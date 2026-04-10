using Types;
using UnityEngine;
using Utilities;

namespace Models.Scenes
{
    public class MenuSceneModel
    {
        private ContinentType _selctedType;

        private const string OnboardingStatusKey = "MenuSceneModel.OnboardingStatus";

        public bool CanShowOnBoarding => OnBoardingStatus == 0;

        private int OnBoardingStatus
        {
            get => PlayerPrefs.GetInt(OnboardingStatusKey, 0);
            set => PlayerPrefs.SetInt(OnboardingStatusKey, value);
        }

        public void SetContinentType(ContinentType type)
        {
            _selctedType = type;
        }

        public void SetGameContinent()
        {
            GameUtility.SetCurrentContinent(_selctedType);
        }

        public void SetSelectLevelSceneSettings()
        {
            SelectLevelUtility.SetPageDifficulty();
        }

        public void SetOnBoardingStatusDisable()
        {
            OnBoardingStatus = 1;
        }

        public ContinentStatusType GetContinentStatus(int continent)
        {
            return ContinentsStatusUtility.GetContinentStatus((ContinentType)continent);
        }
    }
}