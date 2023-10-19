using System.Collections;
using DG.Tweening;
using Dialogue;
using TMPro;
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
        
        private void OnEnable()
        {
            EventHandler.OnShowDialoguePiece += UpdateDialoguePanel;
            EventHandler.OnCloseDialoguePanel += () => dialoguePanel.SetActive(false);
        }

        private void OnDisable()
        {
            EventHandler.OnShowDialoguePiece -= UpdateDialoguePanel;
        }

        private void UpdateDialoguePanel(DialoguePiece piece)
        {
            StartCoroutine(UpdateDialoguePanelRoutine(piece));
        }

        private IEnumerator UpdateDialoguePanelRoutine(DialoguePiece piece)
        {
            if (piece == null) yield break;
            piece.isDone = false;
            dialogue.text = string.Empty;
            continueBox.SetActive(false);

            if (piece.name != string.Empty)
            {
                dialoguePanel.SetActive(true);
                identity.SetActive(true);
                charName.text = piece.name;
                figure.sprite = piece.figure;
            }
            else
            {
                identity.SetActive(false);
            }

            yield return dialogue.DOText(piece.dialogue, 1f).WaitForCompletion();
            piece.isDone = true;
            if (piece.hasToPause && piece.isDone)
            {
                continueBox.SetActive(true);
            }
        }
    }
}
