using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private InputControls _inputControls;

        public bool canInteract;
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
            interoperableSign.SetActive(canInteract);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteroperableObject"))
            {
                canInteract = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("InteroperableObject"))
            {
                canInteract = false;
            }
        }

        private void OpenDialogueCanvas(InputAction.CallbackContext context)
        {
            EventHandler.OpenDialoguePanel("dialogue");
        }
    }
}

