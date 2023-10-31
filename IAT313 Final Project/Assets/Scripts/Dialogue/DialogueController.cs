using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        public string jsonFile;
        public bool canTalk;
        public bool isTeleport;
        
        private SpriteRenderer _character;
        private PlayerData_SO _playerData;
        // private LevelData_SO _levelData;
        
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private DialogueEventSet dialogueEventSet;
        
        private Stack<DialoguePiece> _dialogueStack;
        private DialogueOption[] _dialogueOptions;
        private bool _isTalking;
        private bool _isSelecting;
        [SerializeField] private string currentDialogueEventID;
        private int _currentOptionIndex;

        private string _levelId;
        private string _sceneName;
        private EventInfo _eventInfoInData;

        private void Awake()
        {
            _playerData = Resources.Load<PlayerData_SO>("Data_SO/PlayerData_SO");
            
            currentDialogueEventID = "001";
            LoadJson(jsonFile);
            FillDialogueStack();
            
            if (!isTeleport)
            {
                if (canTalk) nameText.color = dialogueEventSet.mainEvent ? new Color(1, 1, 0) : new Color(1, 1, 1);
                else nameText.color = new Color(0.5f,0.5f,0.5f);
                nameText.text = dialogueEventSet.character;
                _character = GetComponent<SpriteRenderer>();
                _character.sprite = Resources.Load<Sprite>("Characters/" + dialogueEventSet.character);
            }
            this.enabled = false;
        }

        private void OnEnable()
        {
            _isTalking = false;
            _isSelecting = false; 
            if (dialogueEventSet.mainEvent && _playerData.actionPoint < 1) canTalk = false;
            
            EventHandler.OnOpenDialoguePanel += HandleOpenDialoguePanel;

            EventHandler.OnNavigationUp += HandleNavigationUp;
            EventHandler.OnNavigationDown += HandleNavigationDown;
        }

        private void OnDisable()
        {
            EventHandler.OnOpenDialoguePanel -= HandleOpenDialoguePanel;

            EventHandler.OnNavigationUp -= HandleNavigationUp;
            EventHandler.OnNavigationDown -= HandleNavigationDown;
        }

        private void Update()
        {
            UpdateOptionHighlight();
        }

        public void InitializeDialogueData(string fileName, bool isFinished)
        {
            jsonFile = fileName;
            canTalk = !isFinished;
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
            EventHandler.CloseInteractableSign();
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
                    canTalk = false;
                    EventHandler.CloseDialoguePanel();
                    if (!isTeleport)
                    {
                        nameText.color = new Color(0.5f,0.5f,0.5f);
                        EventHandler.DeliverEventName(jsonFile);
                    }
                    if (dialogueEventSet.mainEvent) EventHandler.CostActionPoint();
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
            if (_dialogueOptions[_currentOptionIndex].effect != null)
                EventHandler.AfterEventEffect(_dialogueOptions[_currentOptionIndex].effect);
            if (isTeleport)
                EventHandler.LoadNextScene(_dialogueOptions[_currentOptionIndex].optionContent);
            currentDialogueEventID = _dialogueOptions[_currentOptionIndex].nextDialogueEventID;
            _isSelecting = false;
            _isTalking = false;
            _dialogueOptions = null;
            EventHandler.DestroyOptions();
            FillDialogueStack();
        }

        // ReSharper disable Unity.PerformanceAnalysis
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
        
        private void HandleOpenDialoguePanel()
        {
            if (_isSelecting && _dialogueOptions != null) SelectionConfirmed();
            if (!_isTalking && _dialogueStack != null) StartCoroutine(DialogueRoutine());
        }

        private void HandleNavigationUp()
        {
            if (_isSelecting && _currentOptionIndex > 0) _currentOptionIndex--;
        }

        private void HandleNavigationDown()
        {
            if (_isSelecting && _currentOptionIndex < _dialogueOptions.Length-1) _currentOptionIndex++;
        }
    }
}
