using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private GameObject _vincent;
    private GameObject _mainCanvas;

    private void Awake()
    {
        _vincent = Resources.Load<GameObject>("Prefabs/Vincent");
        _mainCanvas = Resources.Load<GameObject>("Prefabs/Main Canvas");

        Instantiate(_vincent, new Vector3(0, 0, 0), quaternion.identity);
        Instantiate(_mainCanvas, new Vector3(0, 0, 0), quaternion.identity);
    }
}
