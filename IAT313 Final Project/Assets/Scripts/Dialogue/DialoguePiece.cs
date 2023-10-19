using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    [Serializable]
    public class DialoguePiece
    {
        public string name;
        public Sprite figure;
        [TextArea] public string dialogue;

        public bool hasToPause;
        public bool isDone;
    }
}
