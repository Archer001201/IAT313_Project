using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public bool canTalk;

        [SerializeField] private string jsonFile;
        [SerializeField] private DialogueEventSet dialogueEventSet;
        
        private Stack<DialoguePiece> _dialogueStack;
        private DialogueOption[] _dialogueOptions;
        private bool _isTalking;
        private bool _isSelecting;
        [SerializeField] private string currentDialogueEventID;
        private int _currentOptionIndex;

        private void Awake()
        {
            currentDialogueEventID = "001";
            LoadJson(jsonFile);
            FillDialogueStack();
        }

        private void OnEnable()
        {
            EventHandler.OnOpenDialoguePanel += () =>
            {
                if (_isSelecting && _dialogueOptions != null) SelectionConfirmed();
                if (!_isTalking && _dialogueStack!=null) StartCoroutine(DialogueRoutine());
            };

            EventHandler.OnNavigationUp += () =>
            {
                if (_isSelecting && _currentOptionIndex > 0) _currentOptionIndex--;
            };
            EventHandler.OnNavigationDown += () =>
            {
                if (_isSelecting && _currentOptionIndex < _dialogueOptions.Length-1) _currentOptionIndex++;
            };
        }

        private void Update()
        {
            if (_dialogueStack != null) canTalk = true;
            UpdateOptionHighlight();
        }

        private void LoadJson(string fileName)
        {
            TextAsset jsonTextAsset = Resources.Load<TextAsset>("JSON_Files/Data/" + fileName);
            if (jsonTextAsset != null)
            {
                dialogueEventSet = JsonUtility.FromJson<DialogueEventSet>(jsonTextAsset.text);
            }
            else
            {
                Debug.LogError(fileName + ".json can not be loaded");
            }
        }

        private void FillDialogueStack()
        {
            DialoguePiece[] currentDialogueEvent = null;
            foreach (var dialogueEvent in dialogueEventSet.dialogueEvents.Where(dialogueEvent => dialogueEvent.dialogueEventID.Equals(currentDialogueEventID)))
            {
                currentDialogueEvent = dialogueEvent.dialogues;
                _dialogueOptions = dialogueEvent.options.Length > 0 ? dialogueEvent.options : null;
                _currentOptionIndex = 0;
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
                if (_dialogueOptions != null)
                {
                    ShowDialogueOptions(_dialogueOptions);
                    yield return new WaitForSeconds(0.5f);
                    _isSelecting = true;
                }
                else
                {
                    _isTalking = false;
                    EventHandler.CloseDialoguePanel();
                }
            }
        }

        private void ShowDialogueOptions(DialogueOption[] options)
        {
            foreach (var option in options)
            {
                EventHandler.ShowDialogueOption(option);
            }
        }

        private void SelectionConfirmed()
        {
            currentDialogueEventID = _dialogueOptions[_currentOptionIndex].nextDialogueEventID;
            _isSelecting = false;
            _isTalking = false;
            _dialogueOptions = null;
            EventHandler.DestroyOptions();
            FillDialogueStack();
        }

        private void UpdateOptionHighlight()
        {
            if (_dialogueOptions == null) return;
            for (int i = 0; i < _dialogueOptions.Length; i++)
            {
                DialogueOption option = _dialogueOptions[i];
                option.isSelected = i == _currentOptionIndex;
                EventHandler.ShowSelectedOption(option);
            }
        }
    }
}
