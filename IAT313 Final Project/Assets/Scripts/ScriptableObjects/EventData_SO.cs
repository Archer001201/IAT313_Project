using System.Collections.Generic;
using Dialogue;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EventData_SO", menuName = "Scriptable Object/Event Data")]
    // ReSharper disable once InconsistentNaming
    public class EventData_SO : ScriptableObject
    {
        public string currentSceneID;
        public List<SceneInfo> scenes;
    }
}
