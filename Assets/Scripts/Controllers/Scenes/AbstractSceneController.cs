using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

using SO;
using Sounds;
using Types;

namespace Controllers.Scenes
{
    public abstract class AbstractSceneController : MonoBehaviour
    {
        [SerializeField] 
        private SoundsController _soundsController;
        [SerializeField]
        private SceneSounds _sceneSounds;

        private MusicController _musicController;

        private void OnEnable()
        {
            _musicController = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicController>();
            
            _sceneSounds.SetAudioClip();
            
            Initialize();
            Subscribe();
            OnSceneEnable();
        }

        private void Start()
        {
            PlayMusic();
            OnSceneStart();
        }

        private void OnDisable()
        {
            Unsubscribe();
            OnSceneDisable();
        }

        protected abstract void OnSceneEnable();
        protected abstract void OnSceneStart();
        protected abstract void OnSceneDisable();
        protected abstract void Initialize();
        protected abstract void Subscribe();
        protected abstract void Unsubscribe();

        protected void LoadScene(SceneType type, bool isSingle =true)
        {
            SetClickClip();
            
            StartCoroutine(DelayLoadScene(type.ToString(), isSingle));
        }

        protected void UnloadScene(SceneType type)
        {
            SetClickClip();

            StartCoroutine(DelayCloseScene(type.ToString()));
        }

        protected void SetClickClip()
        {
            PlaySound(AudioNames.ClickClip);
        }

        protected void PlaySound(AudioNames name)
        {
           _soundsController.TryPlaySound(GetAudioClip(name.ToString()));
        }
        
        protected void PlayMusic()
        {
            string clipName = AudioNames.MenuClip.ToString();

            _musicController.TryPlayMusic(GetAudioClip(clipName));
        }
        
        private AudioClip GetAudioClip(string clipName)
        {
            return _sceneSounds.GetAudioClipByName(clipName);
        }

        private IEnumerator DelayLoadScene(string sceneName, bool isSingle)
        {
            yield return new WaitForSecondsRealtime(0.3f);

            if (Time.timeScale == 0 && isSingle)
            {
                Time.timeScale = 1;
            }
            else if(!isSingle)
            {
                Time.timeScale = 0;
            }

            SceneManager.LoadScene(sceneName, isSingle ? LoadSceneMode.Single : LoadSceneMode.Additive);
        }

        private IEnumerator DelayCloseScene(string sceneName)
        {
            yield return new WaitForSecondsRealtime(0.3f);

            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }

            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}