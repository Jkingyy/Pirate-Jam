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

    private bool _isInteracting = false;
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
        if(_interactAction.WasPressedThisFrame()){
            if(_playerDetector.currentTargetCounter == null) return;

            float dot = Vector3.Dot(transform.forward, (_playerDetector.currentTargetCounter.transform.position - transform.position).normalized);
            if(dot < 0.7f) return;
            GameObject parent = GetParentObj(_playerDetector.currentTargetCounter);
            if(parent.GetComponent<Worktop>() == null) return;       
            IInteractable interactable = parent.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this.gameObject, holdingItem , _objAnchor);
            }
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
    }
    private void OnDisable()
    {
        _interactAction.Disable();
    }
}
