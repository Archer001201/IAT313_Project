using Dialogue;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private GameObject _player;
    private GameObject _mainCanvas;
    private GameObject _npc;
    private EventData_SO _eventData;

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

        _eventData = Resources.Load<EventData_SO>("Data_SO/EventData_SO");
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
        foreach (var myScene in _eventData.scenes)
        {
            if (myScene.sceneID.Equals(_eventData.currentSceneID))
            {
                SceneInfo currentScene = myScene;
                foreach (var myEvent in currentScene.events)
                {   
                    InstantiateEvent(myEvent.position,myEvent.fileName);
                }
            }
            else Debug.LogError("Can not find this sceneID: " + _eventData.currentSceneID);
        }
    }
}
