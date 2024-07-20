using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _interactAction;
    private InputAction _actionAction;

    [SerializeField] private Transform _objAnchor;

    private bool _isActioning = false;


    public bool holdingItem = false;
    public GameObject heldItem;
    private PlayerDetector _playerDetector;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions.FindAction("Interact");
        _actionAction = _playerInput.actions.FindAction("Action");
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerDetector = GetComponentInChildren<PlayerDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInteractions();
        HandleActions();
    }


    void HandleInteractions(){
        if(_interactAction.WasPressedThisFrame()){
            if(_playerDetector.currentTargetCounter == null) return;
            GameObject parent = GetParentObj(_playerDetector.currentTargetCounter);
            if(parent.GetComponent<Worktop>() == null) return;       
            IInteractable interactable = parent.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this.gameObject, holdingItem , _objAnchor);
            }
        }
    }
    void HandleActions(){
        if(_actionAction.WasPressedThisFrame()){
            _isActioning = true;
        } else if(_actionAction.WasReleasedThisFrame()){
            _isActioning = false;
        }

        if(_isActioning){
            if(_playerDetector.currentTargetCounter == null) return;
            Debug.Log("Actioning");
        }
    }
    private GameObject GetParentObj(GameObject obj)
    {
        if (obj.transform.parent == null)
        {
            return obj;
        }
        return obj.transform.parent.gameObject;
    }

    private void OnEnable()
    {
        _interactAction.Enable();
        _actionAction.Enable();
    }
    private void OnDisable()
    {
        _interactAction.Disable();
        _actionAction.Disable();
    }
}
