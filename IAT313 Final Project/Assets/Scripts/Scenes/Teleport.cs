using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class Teleport : MonoBehaviour
    {
        private void OnEnable()
        {
            EventHandler.OnLoadNextScene += HandleTeleport;
        }

        private void OnDisable()
        {
            EventHandler.OnLoadNextScene -= HandleTeleport;
        }

        private void HandleTeleport(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
