using System;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class DialoguePiece
    {
        public string name;
        [TextArea] public string content;
        
        public bool isRead;
    }
    
    [Serializable]
    public class Option
    {
        public string optionContent;
        public string nextDialogueEventID;
    }
    
    [Serializable]
    public class DialogueEvent
    {
        public string dialogueEventID;
        public DialoguePiece[] dialogues;
        public Option[] options;
    }
    
    [Serializable]
    public class DialogueEventSet
    {
        public string dialogueEventSetId;
        public DialogueEvent[] dialogueEvents;
    }
}
