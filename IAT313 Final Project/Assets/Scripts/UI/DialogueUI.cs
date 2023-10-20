using System;
using System.Collections;
using DG.Tweening;
using Dialogue;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private GameObject identity;
        [SerializeField] private TextMeshProUGUI charName;
        [SerializeField] private Image figure;
        [SerializeField] private TextMeshProUGUI dialogue;
        [SerializeField] private GameObject continueBox;
        [SerializeField] private GameObject optionBox;
        [SerializeField] private int currentOptionIndex;
        
        private void Awake()
        {
            dialoguePanel.SetActive(false);
        }

        private void OnEnable()
        {
            currentOptionIndex = 0;
            EventHandler.OnShowDialoguePiece += UpdateDialoguePanel;
            EventHandler.OnCloseDialoguePanel += () => dialoguePanel.SetActive(false);
            EventHandler.OnShowDialogueOption += UpdateDialogueOption;
            EventHandler.OnShowSelectedOption += UpdateSelectedOption;
            EventHandler.OnDestroyOptions += DestroyAllOptions;
        }

        private void OnDisable()
        {
            EventHandler.OnShowDialoguePiece -= UpdateDialoguePanel;
            EventHandler.OnShowDialogueOption -= UpdateDialogueOption;
            EventHandler.OnShowSelectedOption -= UpdateSelectedOption;
            EventHandler.OnDestroyOptions -= DestroyAllOptions;
        }

        private void Update()
        {
            if (optionBox.transform.childCount > 0)
            {
                Transform currentOptionTrans = optionBox.transform.GetChild(currentOptionIndex);
                GameObject currentOptionHighlight = currentOptionTrans.GetChild(0).gameObject;
                currentOptionHighlight.SetActive(true);
            }
        }

        private void UpdateDialoguePanel(DialoguePiece piece)
        {
            dialoguePanel.SetActive(true);
            StartCoroutine(UpdateDialoguePanelRoutine(piece));
        }

        private IEnumerator UpdateDialoguePanelRoutine(DialoguePiece piece)
        {
            if (piece == null) yield break;
            dialoguePanel.SetActive(true);
            piece.isRead = false;
            dialogue.text = string.Empty;
            continueBox.SetActive(false);

            if (piece.name != string.Empty)
            {
                dialoguePanel.SetActive(true);
                identity.SetActive(true);
                charName.text = piece.name;
                figure.sprite = Resources.Load<Sprite>("Characters/" + piece.name);
            }
            else
            {
                identity.SetActive(false);
            }

            yield return dialogue.DOText(piece.content, 1f).WaitForCompletion();
            piece.isRead = true;
            if (piece.isRead)
            {
                continueBox.SetActive(true);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void UpdateDialogueOption(DialogueOption option)
        {
            optionBox.SetActive(true);
            GameObject optionPrefab = Resources.Load<GameObject>("Prefabs/Choice");
            GameObject optionObj = Instantiate(optionPrefab, optionBox.transform.position, Quaternion.identity, optionBox.transform);
            optionObj.GetComponentInChildren<TextMeshProUGUI>().text = option.optionContent;
            optionObj.transform.GetChild(0).gameObject.SetActive(false);
        }

        private void UpdateSelectedOption(DialogueOption option)
        {
            Transform optionBoxTrans = optionBox.transform;
            for (int i = 0; i < optionBoxTrans.childCount; i++)
            {
                Transform iOption = optionBoxTrans.GetChild(i);
                if (iOption.GetComponentInChildren<TextMeshProUGUI>().text == option.optionContent)
                {
                    iOption.GetChild(0).gameObject.SetActive(option.isSelected);
                }
            }
        }

        private void DestroyAllOptions()
        {
            Transform optionBoxTrans = optionBox.transform;
            foreach (Transform child in optionBoxTrans)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
