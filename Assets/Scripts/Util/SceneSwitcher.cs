using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class SceneSwitcher : MonoBehaviour
    {
        [Tooltip("Name of the scene which is to be loaded next")]
        public string nextSceneName;

        [Tooltip("Screen fade used to fade out the view on load")]
        public OVRScreenFade fade;

        private Scene nextScene;

        /// <summary>
        /// Load scene with given name.
        /// </summary>
        public void LoadNextScene()
        {
            StartCoroutine(FadeOutLoad());
        }

        private IEnumerator FadeOutLoad()
        {
            yield return fade.CreateFadeOut();
            SceneManager.LoadScene(nextSceneName);
        }

        private void Awake()
        {
            nextScene = SceneManager.GetSceneByName(nextSceneName);
            if (nextScene.IsValid())
            {
                return;
            }

            nextScene = SceneManager.GetActiveScene();
            nextSceneName = nextScene.name;
        }
    }
}
