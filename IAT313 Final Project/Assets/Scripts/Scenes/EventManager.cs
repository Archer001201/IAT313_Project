using System.Collections.Generic;
using Dialogue;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Scenes
{
    public class EventManager : MonoBehaviour
    {
        private Scene _currentScene;
        private string _sceneName;
        private GameObject _player;
        private GameObject _mainCanvas;
        private GameObject _npc;
        private GameObject _dummy;
        private LevelData_SO _levelData;
        private string _currentFile;
        private int _currentLevelIndex;
        private int _currentSceneIndex;
        private AudioSource _audioSource;

        [SerializeField] private List<GameObject> teleports;

        private void Awake()
        {
            _currentScene = SceneManager.GetActiveScene();
            _sceneName = _currentScene.name;
            _audioSource = GetComponent<AudioSource>();
            LoadResources();
            InstantiateObjects();
        }

        private void OnEnable()
        {
            EventHandler.OnDeliverEventName += HandleDeliverEventName;
        }

        private void OnDisable()
        {
            EventHandler.OnDeliverEventName -= HandleDeliverEventName;
        }

        private void LoadResources()
        {
            _player = Resources.Load<GameObject>("Prefabs/Vincent");
            _mainCanvas = Resources.Load<GameObject>("Prefabs/Main Canvas");
            _npc = Resources.Load<GameObject>("Prefabs/NPC");
            _dummy = Resources.Load<GameObject>("Prefabs/Dummy");
            _levelData = Resources.Load<LevelData_SO>("Data_SO/EventData_SO");
        }

        private void InstantiateObjects()
        {
            Instantiate(_player, new Vector3(0, 0, 0), quaternion.identity);
            Instantiate(_mainCanvas, new Vector3(0, 0, 0), quaternion.identity);

            InstantiateEventsInScene();
        }

        private void InstantiateEvent(Vector3 pos, string fileName, bool isFinished)
        {
            DialogueController dialogueController = _npc.GetComponent<DialogueController>();
            dialogueController.InitializeDialogueData(fileName,isFinished);
            Instantiate(_npc, pos, quaternion.identity);
        }

        private void InstantiateEventsInScene()
        {
            LevelInfo currentLevel = null;

            for (int i = 0; i < _levelData.levels.Count; i++)
            {
                LevelInfo level = _levelData.levels[i];
                if (!level.levelID.Equals(_levelData.currentLevelID)) continue;
                currentLevel = level;
                _currentLevelIndex = i;
            }
        

            if (currentLevel == null) return;
            string audioSourceFile = currentLevel.audioFile;
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioSourceFile);
            if (clip != null)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
            for (var i = 0; i < currentLevel.scenes.Count; i++)
            {
                var myScene = currentLevel.scenes[i];
                if (myScene.sceneName.Equals(_sceneName))
                {
                    foreach (var myEvent in myScene.events)
                    {
                        InstantiateEvent(myEvent.position, myEvent.fileName, myEvent.isFinished);
                    }

                    foreach (var npcInfo in myScene.npcs)
                    {
                        var dummy = Instantiate<GameObject>(_dummy, npcInfo.position, quaternion.identity);
                        dummy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Characters/" + npcInfo.npcName);
                    }
                    
                    // teleport.GetComponent<DialogueController>().jsonFile = myScene.teleportFile;
                    for (var j=0; j<teleports.Count; j++)
                    {
                        teleports[j].GetComponent<DialogueController>().jsonFile = myScene.teleportFiles[j];
                    }
                    
                    _currentSceneIndex = i;
                    ShowEventInformation(currentLevel, myScene);
                    return;
                }
            }

            Debug.LogError("Can not find this sceneID: " + _sceneName);
        }

        private void ShowEventInformation(LevelInfo levelInfo, SceneInfo sceneInfo)
        {
            Debug.Log("LevelID: " + levelInfo.levelID);
            Debug.Log("SceneName: " + sceneInfo.sceneName);
        }

        private void HandleDeliverEventName(string fileName)
        {
            var currentLevel = _levelData.levels[_currentLevelIndex];
            var currentScene = currentLevel.scenes[_currentSceneIndex];
            foreach (var myEvent in currentScene.events)
            {
                if (myEvent.fileName.Equals(fileName)) myEvent.isFinished = true;
            }
        }
    }
}
