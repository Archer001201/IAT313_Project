using Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private InputControls _inputControls;

        public bool canTalk;
        public GameObject interactableSign;
        
        private void Awake()
        {
            _inputControls = new InputControls();
            _inputControls.GamePlay.ConfirmButton.performed += OpenDialogueCanvas;
            _inputControls.GamePlay.NavigationDown.performed += _ => EventHandler.NavigationDown();
            _inputControls.GamePlay.NavigationUp.performed += _ => EventHandler.NavigationUp();
        }

        private void OnEnable()
        {
            _inputControls.Enable();
            EventHandler.OnCloseInteractableSign += () =>
            {
                interactableSign.SetActive(false);
            };
        }

        private void OnDisable()
        {
            _inputControls.Disable();
        }

        // private void Update()
        // {
        //     interactableSign.SetActive(canTalk);
        // }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteroperableObject"))
            {
                other.GetComponentInParent<DialogueController>().enabled = true;
                canTalk = other.GetComponentInParent<DialogueController>().canTalk;
                interactableSign.SetActive(canTalk);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("InteroperableObject"))
            {
                other.GetComponentInParent<DialogueController>().enabled = false;
                canTalk = false;
                interactableSign.SetActive(canTalk);
            }
        }

        private void OpenDialogueCanvas(InputAction.CallbackContext context)
        {
            if (canTalk) EventHandler.OpenDialoguePanel();
        }
    }
}

