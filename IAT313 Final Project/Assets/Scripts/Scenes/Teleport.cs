using System;
using System.Collections.Generic;
using System.Reflection;
using Dialogue;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class Teleport : MonoBehaviour
    {
        // public DialogueEvent sceneSelection;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene("TestScene2");
            }
        }
    }
}
