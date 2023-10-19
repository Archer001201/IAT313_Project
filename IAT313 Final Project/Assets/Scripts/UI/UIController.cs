using System;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject dialoguePanel;

        private void Awake()
        {
            dialoguePanel.SetActive(false);
        }

        private void OnEnable()
        {
            EventHandler.OnOpenDialoguePanel += OpenPanel;
        }

        private void OnDisable()
        {
            EventHandler.OnOpenDialoguePanel -= OpenPanel;
        }

        private void OpenPanel(string type)
        {
            switch (type)
            {
                case "dialogue":
                    dialoguePanel.SetActive(true);
                    break;
            }
        }
    }
}
