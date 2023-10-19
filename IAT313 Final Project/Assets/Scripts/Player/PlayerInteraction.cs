using Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private InputControls _inputControls;

        public bool canTalk;
        public GameObject interoperableSign;
        
        private void Awake()
        {
            _inputControls = new InputControls();

            _inputControls.GamePlay.PlayerInteract.performed += OpenDialogueCanvas;
        }

        private void OnEnable()
        {
            _inputControls.Enable();
        }

        private void OnDisable()
        {
            _inputControls.Disable();
        }

        private void Update()
        {
            interoperableSign.SetActive(canTalk);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteroperableObject"))
            {
                canTalk = other.GetComponentInParent<DialogueManager>().canTalk;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("InteroperableObject"))
            {
                canTalk = false;
            }
        }

        private void OpenDialogueCanvas(InputAction.CallbackContext context)
        {
            if (canTalk) EventHandler.OpenDialoguePanel();
        }
    }
}

