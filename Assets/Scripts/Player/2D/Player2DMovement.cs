using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2DMovement : MonoBehaviour
{
    /// <summary>
    /// ////////////////MOVEMENT SERIALIZED////////////////////////
    /// </summary>

    [SerializeField] private float StickThreshold = 0.1f;

    [Header("Movement Speeds")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Dash Variables")]
    [SerializeField] private float dashPower = 10f;
    [SerializeField] private float dashDistance = 0.1f, dashCooldown = 1f;

    /// <summary>
    /// ////////////////MOVEMENT PRIVATE////////////////////////
    /// </summary>

    //movement input
    private Vector2 _movementInput;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _dashAction;
    
    /// <summary>
    /// ////////////////BOOL CHECKS////////////////////////
    /// </summary>
    private bool _isDashing;
    private bool _canDash = true;

    
    /// <summary>
    /// ////////////////COMPONENT REFERENCES////////////////////////
    /// </summary>
    private Rigidbody2D _rb;
    private Collider2D col;
    //private Animator _animator;


    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        //_animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move 2D");
        _dashAction = _playerInput.actions.FindAction("Dash");
    
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        if(_canDash && _dashAction.WasPressedThisFrame()){
            StartCoroutine(Dash());
        }
    }
    private void FixedUpdate() {
        if(_isDashing) return;
        Move();
    }

    private void Move() {
        if(_movementInput.magnitude < StickThreshold) return;

        _rb.MovePosition(transform.position + new Vector3(_movementInput.x, _movementInput.y, 0) * moveSpeed * Time.deltaTime);
    }

    private void GetInputs() {
        _movementInput = _moveAction.ReadValue<Vector2>();
    }

    private IEnumerator Dash(){
        _canDash = false;
        _isDashing = true;
        _rb.velocity = _movementInput.normalized * dashPower;
        col.isTrigger = true;
        yield return new WaitForSeconds(dashDistance);
        _isDashing = false;
        _rb.velocity = Vector2.zero;
        col.isTrigger = false;
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }
}
