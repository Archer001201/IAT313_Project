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

        private void Awake()
        {
            dialoguePanel.SetActive(false);
        }

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
    }
}
