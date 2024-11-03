using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rayDistance = 0.5f;

    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private bool invertMouse = false;

    private Animator _animator;
    private Rigidbody _rb;
    private Vector3 _moveDirection;
    private float _mouseX;
    private readonly int _horizontalHash = Animator.StringToHash("Horizontal");
    private readonly int _verticalHash = Animator.StringToHash("Vertical");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        InitializeRigidbody();
    }

    private void InitializeRigidbody()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * (invertMouse ? -1 : 1);
        _moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void UpdateAnimator()
    {
        _animator.SetFloat(_horizontalHash, _moveDirection.x, 0.1f, Time.deltaTime);
        _animator.SetFloat(_verticalHash, _moveDirection.z, 0.1f, Time.deltaTime);
    }

    private void Move()
    {
        if (_moveDirection == Vector3.zero) return;

        Vector3 moveDirection = transform.TransformDirection(_moveDirection);
        if (!IsBlockedByObstacle(moveDirection))
        {
            Vector3 movePosition = _rb.position + moveDirection * (moveSpeed * Time.fixedDeltaTime);
            _rb.MovePosition(movePosition);
        }
    }

    private bool IsBlockedByObstacle(Vector3 direction)
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, rayDistance);
    }

    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.Euler(0, _mouseX, 0);
        _rb.rotation = Quaternion.Lerp(_rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnValidate()
    {
        moveSpeed = Mathf.Max(0, moveSpeed);
        rotationSpeed = Mathf.Max(0, rotationSpeed);
        mouseSensitivity = Mathf.Max(0, mouseSensitivity);
        rayDistance = Mathf.Max(0.1f, rayDistance);
    }
}
