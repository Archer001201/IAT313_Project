using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private InputControls _inputControls;
        private Vector2 _moveDir;

        public float moveSpeed;
        public bool canMove;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _inputControls = new InputControls();

            _inputControls.GamePlay.PlayerMove.performed += context => _moveDir = context.ReadValue<Vector2>();
            _inputControls.GamePlay.PlayerMove.canceled += _ => _moveDir = Vector2.zero;
        }

        private void OnEnable()
        {
            _inputControls.Enable();
            EventHandler.OnOpenDialoguePanel += () => canMove = false;
            EventHandler.OnCloseDialoguePanel += () => canMove = true;
        }

        private void OnDisable()
        {
            _inputControls.Disable();
        }

        private void FixedUpdate()
        {
            if (canMove) Move();
        }

        private void Move()
        {
            _rb.velocity = new Vector2(_moveDir.x * moveSpeed, _moveDir.y * moveSpeed);
        }
    }
}

