using System;
using ScriptableObjects;
using UnityEngine;

namespace Scenes
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        private LevelData_SO _levelData;

        private void Awake()
        {
            _levelData = Resources.Load<LevelData_SO>("Data_SO/EventData_SO");
            
        }
    }
}
