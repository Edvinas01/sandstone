using System.Collections;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class SceneSwitcher : MonoBehaviour
    {
        [Tooltip("Name of the scene which is to be loaded next")]
        public string nextSceneName;

        [Tooltip("Audio mixer manager used for audio fading")]
        public AudioMixerManager mixerManager;

        [Tooltip("Screen fade used to fade out the view on load")]
        public OVRScreenFade fade;

        private Scene activeScene;
        private Scene nextScene;

        private bool triggered;

        /// <summary>
        /// Reload active scene.
        /// </summary>
        public void ReloadActiveScene()
        {
            StartCoroutine(FadeOutLoad(activeScene.name));
        }

        /// <summary>
        /// Load scene with name <see cref="nextSceneName"/>.
        /// </summary>
        public void LoadNextScene()
        {
            StartCoroutine(FadeOutLoad(nextSceneName));
        }

        private IEnumerator FadeOutLoad(string sceneName)
        {
            if (triggered)
            {
                yield break;
            }

            triggered = true;

            mixerManager.TransitionFadeOut();
            yield return fade.CreateFadeOut();

            SceneManager.LoadScene(sceneName);
        }

        private void Awake()
        {
            activeScene = SceneManager.GetActiveScene();
            nextScene = SceneManager.GetSceneByName(nextSceneName);

            if (nextScene.IsValid())
            {
                return;
            }

            // Use current scene as fallback.
            nextSceneName = activeScene.name;
            nextScene = activeScene;
        }

        private void Start()
        {
            mixerManager.TransitionFadeIn();
        }
    }
}
