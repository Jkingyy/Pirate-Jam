using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Callbacks;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player3DMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float StickThreshold = 0.1f;
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private float turnSmoothVelocity;
    private Rigidbody _rb;
    
    private Vector3 _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update() {
        GatherInput();
        Look();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if(_input.magnitude < StickThreshold) return;
        _rb.MovePosition(transform.position + _input.ToIso() * _input.normalized.magnitude * moveSpeed * Time.deltaTime);
    }

    void Look(){
        if(_input == Vector3.zero || _input.magnitude < StickThreshold) return;

        var rotation = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        

    }

    void GatherInput(){
        _input = new Vector3(_moveAction.ReadValue<Vector2>().x, 0, _moveAction.ReadValue<Vector2>().y);

    }
}
