using Dialogue;
using Unity.Mathematics;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private GameObject _player;
    private GameObject _mainCanvas;
    private GameObject _npc;

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
    }

    private void InstantiateObjects()
    {
        Instantiate(_player, new Vector3(0, 0, 0), quaternion.identity);
        Instantiate(_mainCanvas, new Vector3(0, 0, 0), quaternion.identity);
        
        InstantiateEvent(new Vector3(1,1,0),"test");
    }

    private void InstantiateEvent(Vector3 pos, string fileName)
    {
        GameObject eventInstance = Instantiate(_npc, pos, quaternion.identity);
        eventInstance.GetComponent<DialogueController>().jsonFile = fileName;
    }
}
