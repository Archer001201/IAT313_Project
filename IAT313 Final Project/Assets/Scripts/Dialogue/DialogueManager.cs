using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();

        private Stack<DialoguePiece> _dialogueStack;
        private bool _isTalking;

        private void Awake()
        {
            FillDialogueStack();
        }

        private void OnEnable()
        {
            EventHandler.OnOpenDialoguePanel += _ =>
            {
                if (!_isTalking) StartCoroutine(DialogueRoutine());
            };
        }

        private void FillDialogueStack()
        {
            _dialogueStack = new Stack<DialoguePiece>();
            for (int i = dialoguePieces.Count - 1; i >= 0; i--)
            {
                dialoguePieces[i].isDone = false;
                _dialogueStack.Push(dialoguePieces[i]);
            }
        }

        private IEnumerator DialogueRoutine()
        {
            _isTalking = true;
            if (_dialogueStack.TryPop(out var piece))
            {
                EventHandler.ShowDialoguePiece(piece);
                yield return new WaitUntil(() => piece.isDone);
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
