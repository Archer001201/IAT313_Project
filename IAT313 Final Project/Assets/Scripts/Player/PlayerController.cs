using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private InputControls _inputControls;
        private Vector2 _moveDir;
        private Animator _animator;

        public float moveSpeed;
        public bool canMove;
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int SpeedY = Animator.StringToHash("speedY");
        private static readonly int SpeedX = Animator.StringToHash("speedX");

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _inputControls = new InputControls();
        
            _inputControls.GamePlay.PlayerMove.performed += context => _moveDir = context.ReadValue<Vector2>();
            _inputControls.GamePlay.PlayerMove.canceled += _ => _moveDir = Vector2.zero;
        }

        private void OnEnable()
        {
            _inputControls.Enable();
            EventHandler.OnOpenDialoguePanel += (x,y) => canMove = false;
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
            var velocity = new Vector2(_moveDir.x * moveSpeed, _moveDir.y * moveSpeed);
            _rb.velocity = velocity;

            bool isMoving = velocity.x != 0 || velocity.y != 0;
            _animator.SetBool(IsMoving, isMoving);

            if (isMoving)
            {
                _animator.SetFloat(SpeedY, velocity.y);
                _animator.SetFloat(SpeedX, velocity.x);
            }
        }
    }
}

