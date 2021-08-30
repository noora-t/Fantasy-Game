using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 7f, _jumpSpeed = 0.5f, _turnSpeed = 4f;
    [SerializeField] float _gravity = 2f;
    [SerializeField] float _verticalRotMin = -80, _verticalRotMax = 80;
    [SerializeField] Transform _invisibleCameraOrigin;
   
    CharacterController _characterController;
    Animator _animator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical);
        Vector3 transformDirection = transform.TransformDirection(inputDirection);
        Vector3 flatMovement = _moveSpeed * Time.deltaTime * transformDirection;
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (PlayerJumped)
        {
            moveDirection.y = _jumpSpeed;
            _animator.SetBool("IsJumping", true);
        }          
        else if (_characterController.isGrounded)
        {
            moveDirection.y = 0f;
            _animator.SetBool("IsJumping", false);
        }
            
        else
            moveDirection.y -= _gravity * Time.deltaTime;

        moveDirection = new Vector3(flatMovement.x, moveDirection.y, flatMovement.z);

        if (vertical < 0)
              vertical *= -1;

        _animator.SetFloat("Speed", vertical);

        Vector2 rotInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 rot = transform.eulerAngles;
        rot.y += rotInput.x * _turnSpeed;
        transform.rotation = Quaternion.Euler(rot);

        if (vertical != 0)
            _characterController.Move(moveDirection);

        if (_invisibleCameraOrigin != null)
        {
            rot = _invisibleCameraOrigin.localRotation.eulerAngles;
            rot.x -= rotInput.y * _turnSpeed;
            if (rot.x > 180)
                rot.x -= 360;
            rot.x = Mathf.Clamp(rot.x, _verticalRotMin, _verticalRotMax);
            _invisibleCameraOrigin.localRotation = Quaternion.Euler(rot);
        }
    }

    private bool PlayerJumped => _characterController.isGrounded && Input.GetKey(KeyCode.Space);
}
