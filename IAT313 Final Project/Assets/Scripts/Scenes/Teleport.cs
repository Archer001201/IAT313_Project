using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class Teleport : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene("TestScene2");
            }
        }
    }
}
