using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2DMovement : MonoBehaviour
{
    /// <summary>
    /// ////////////////MOVEMENT SERIALIZED////////////////////////
    /// </summary>

    [SerializeField] private float StickThreshold = 0.1f;

    [SerializeField] private GameObject cursorPivot;

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
    private InputAction _cursorAction;

    private string keyboardMouse = "Keyboard&Mouse";
    private string controller = "Controller";
    private string currentControlScheme;
    
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
    private Animator _animator;
    private TrailRenderer trailRenderer;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        trailRenderer = GetComponent<TrailRenderer>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move 2D");
        _dashAction = _playerInput.actions.FindAction("Dash");
        _cursorAction = _playerInput.actions.FindAction("Cursor");
    
    }
    // Start is called before the first frame update
    void Start()
    {
        currentControlScheme = _playerInput.currentControlScheme;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        Animate();
        if(_canDash && _dashAction.WasPressedThisFrame()){
            StartCoroutine(Dash());
        }
        CheckControlScheme();

        if(currentControlScheme == controller){
            ControllerCursor();
        }
    }

    void ControllerCursor(){
        Vector2 JoyStickInput = _cursorAction.ReadValue<Vector2>();

        if(JoyStickInput.magnitude == 0) return;

        float angle = Mathf.Atan2(JoyStickInput.y, JoyStickInput.x) * Mathf.Rad2Deg;
        cursorPivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void CheckControlScheme(){
        if(_playerInput.currentControlScheme == currentControlScheme) return;

        currentControlScheme = _playerInput.currentControlScheme;
        
        if(currentControlScheme == keyboardMouse){
            Cursor.visible = true;
            cursorPivot.SetActive(false);

        } else if(currentControlScheme == controller){
            Cursor.visible = false;
            cursorPivot.SetActive(true);
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
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashDistance);
        _isDashing = false;
        trailRenderer.emitting = false;
        _rb.velocity = Vector2.zero;
        col.isTrigger = false;
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    void Animate(){
        _animator.SetFloat("Speed", _movementInput.sqrMagnitude);
        if(_movementInput.magnitude == 0) return;
        _animator.SetFloat("Horizontal", _movementInput.x);
        _animator.SetFloat("Vertical", _movementInput.y);
    }
}
