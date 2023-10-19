using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        // public List<DialogueEvent> dialogueEvents;
        public DialogueEventSet dialogueEventSet;
        public bool canTalk;
        
        private Stack<DialoguePiece> _dialogueStack;
        private bool _isTalking;
        [SerializeField] private string currentDialogueEventID;

        private void Awake()
        {
            currentDialogueEventID = "001";
            LoadJson("Test");
            FillDialogueStack();
        }

        private void OnEnable()
        {
            EventHandler.OnOpenDialoguePanel += () =>
            {
                if (!_isTalking && _dialogueStack!=null) StartCoroutine(DialogueRoutine());
            };
        }

        private void Update()
        {
            if (_dialogueStack != null) canTalk = true;
        }

        private void LoadJson(string fileName)
        {
            TextAsset jsonTextAsset = Resources.Load<TextAsset>("JSON_Files/Data/" + fileName);
            if (jsonTextAsset != null)
            {
                dialogueEventSet = JsonUtility.FromJson<DialogueEventSet>(jsonTextAsset.text);
                Debug.Log("loaded");
            }
            else
            {
                Debug.Log("failed loading");
            }
        }

        private void FillDialogueStack()
        {
            DialoguePiece[] currentDialogueEvent = null;
            foreach (var dialogueEvent in dialogueEventSet.dialogueEvents.Where(dialogueEvent => dialogueEvent.dialogueEventID.Equals(currentDialogueEventID)))
            {
                currentDialogueEvent = dialogueEvent.dialogues;
            }
            if (currentDialogueEvent == null) return;
            _dialogueStack = new Stack<DialoguePiece>();
            for (int i = currentDialogueEvent.Length - 1; i >= 0; i--)
            {
                currentDialogueEvent[i].isRead = false;
                _dialogueStack.Push(currentDialogueEvent[i]);
            }
        }

        private IEnumerator DialogueRoutine()
        {
            _isTalking = true;
            if (_dialogueStack.TryPop(out var piece))
            {
                EventHandler.ShowDialoguePiece(piece);
                yield return new WaitUntil(() => piece.isRead);
                _isTalking = false;
            }
            else
            {
                FillDialogueStack();
                _isTalking = false;
                EventHandler.CloseDialoguePanel();
            }
        }
    }
}
