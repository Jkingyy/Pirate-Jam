using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{

    [SerializeField] private GameObject progressBar;
    [SerializeField] private float progressFillRate = 0.5f;
    private PlayerInput _playerInput;
    private InputAction _interactAction;
    private InputAction _actionAction;
    [SerializeField] private Image progressBarImage;
    
    [SerializeField] private Transform _objAnchor;


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
        if(!_interactAction.WasPressedThisFrame()) return;
        if(!IsInRangeOfCounter(out Worktop parent)) return;
            
        IInteractable interactable = parent.GetComponent<IInteractable>();
        if (interactable == null) return;
        
        interactable.Interact(this.gameObject, holdingItem , _objAnchor);
        
    }
    void HandleActions(){
        if(!IsInRangeOfCounter(out Worktop counter)) return;
        if(counter.heldItem == null) return;
        if(_actionAction.WasPressedThisFrame()){
            EnableProgressBar();
        } else if(_actionAction.WasReleasedThisFrame()){
            DisableProgressBar();
        }

        if(_actionAction.IsPressed() && progressBarImage.fillAmount < 1){
            ChargeActionBar();
        } 
        if(progressBarImage.fillAmount >= 1){
            DisableProgressBar();
            counter.Action();
            
        }
    }

    void ChargeActionBar(){
        progressBarImage.fillAmount += progressFillRate * Time.deltaTime;
    }

    void DisableProgressBar(){
        progressBar.SetActive(false);
        progressBarImage.fillAmount = 0;
    }
    void EnableProgressBar(){
        progressBar.SetActive(true);
    }

    bool IsInRangeOfCounter(out Worktop counter){
        counter = null;
        if(_playerDetector.currentTargetCounter == null) return false;
        counter = _playerDetector.currentTargetCounter.GetComponent<Worktop>();
        if(counter == null) return false;      
        return true; 
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
