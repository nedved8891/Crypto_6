using UnityEngine;
using UnityEngine.SceneManagement;
using Types;

namespace Controllers.Scenes
{
    public class InitSceneController : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadScene(SceneType.MenuScene.ToString());
        }
    }
}