using Dialogue;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utilities;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private InputControls _inputControls;
        private PlayerData_SO _playerData;
        [SerializeField] private Animator animator;
        
        public bool canTalk;
        public GameObject interactableSign;
        
        private void Awake()
        {
            _playerData = Resources.Load<PlayerData_SO>("Data_SO/PlayerData_SO");
            _inputControls = new InputControls();
            _inputControls.GamePlay.ConfirmButton.performed += OpenDialogueCanvas;
            _inputControls.GamePlay.NavigationDown.performed += _ => EventHandler.NavigationDown();
            _inputControls.GamePlay.NavigationUp.performed += _ => EventHandler.NavigationUp();
            _inputControls.GamePlay.NextLevel.performed += _ =>
            {
                if (_playerData.actionPoint <= 0) SceneManager.LoadScene("TestScene3");
            };
        }

        private void OnEnable()
        {
            _inputControls.Enable();
            EventHandler.OnCloseInteractableSign += HandleCloseInteractableSign;
        }

        private void OnDisable()
        {
            _inputControls.Disable();
            EventHandler.OnCloseInteractableSign -= HandleCloseInteractableSign;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteroperableObject"))
            {
                other.GetComponentInParent<DialogueController>().enabled = true;
                canTalk = other.GetComponentInParent<DialogueController>().canTalk;
                interactableSign.SetActive(canTalk);
                // other.GetComponentInParent<DialogueController>().FacePlayer(-animator.GetFloat("speedX"), -animator.GetFloat("speedY"));
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
            if (canTalk) EventHandler.OpenDialoguePanel(animator.GetFloat("speedX"),animator.GetFloat("speedY"));
        }

        private void HandleCloseInteractableSign()
        {
            interactableSign.SetActive(false);
        }
    }
}

