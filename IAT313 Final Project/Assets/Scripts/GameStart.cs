using Dialogue;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public string sceneName;
    private GameObject _player;
    private GameObject _mainCanvas;
    private GameObject _npc;
    private LevelData_SO _levelData;

    private void Awake()
    {
        LoadResources();
        InstantiateObjects();
    }

    private void LoadResources()
    {
        _player = Resources.Load<GameObject>("Prefabs/Vincent");
        _mainCanvas = Resources.Load<GameObject>("Prefabs/Main Canvas");
        _npc = Resources.Load<GameObject>("Prefabs/NPC");

        _levelData = Resources.Load<LevelData_SO>("Data_SO/EventData_SO");
    }

    private void InstantiateObjects()
    {
        Instantiate(_player, new Vector3(0, 0, 0), quaternion.identity);
        Instantiate(_mainCanvas, new Vector3(0, 0, 0), quaternion.identity);

        InstantiateEventsInScene();
    }

    private void InstantiateEvent(Vector3 pos, string fileName)
    {
        DialogueController dialogueController = _npc.GetComponent<DialogueController>();
        dialogueController.InitializeDialogueData(fileName);
        Instantiate(_npc, pos, quaternion.identity);
    }

    private void InstantiateEventsInScene()
    {
        LevelID currentDayEvent = null;
        foreach (var dayEventSet in _levelData.dayEventSets)
        {
            if (dayEventSet.levelID.Equals(_levelData.currentLevelID))
                currentDayEvent = dayEventSet;
        }

        if (currentDayEvent == null) return;
        foreach (var myScene in currentDayEvent.scenes)
        {
            if (myScene.sceneName.Equals(sceneName))
            {
                // SceneInfo currentScene = myScene;
                foreach (var myEvent in myScene.events)
                {
                    InstantiateEvent(myEvent.position, myEvent.fileName);
                }
                ShowEventInformation(currentDayEvent, myScene);
                return;
            }
        }
        Debug.LogError("Can not find this sceneID: " + sceneName);
    }

    private void ShowEventInformation(LevelID levelID, SceneInfo sceneInfo)
    {
        Debug.Log("LevelID: " + levelID.levelID);
        Debug.Log("SceneName: " + sceneInfo.sceneName);
    }
}
