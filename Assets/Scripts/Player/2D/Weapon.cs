using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{

    [SerializeField] private GameObject weaponPivot;
    [SerializeField] private float lightSwingCooldown = 0.5f;

    PlayerInput playerInput;
    InputAction _lightAttackAction;


    bool canAttack = true;
    Player2DMovement movementScript;

    private void Awake() {
        playerInput = GetComponentInParent<PlayerInput>();
        _lightAttackAction = playerInput.actions["Light Attack"];
    }

    // Start is called before the first frame update
    void Start()
    {
        movementScript = GetComponentInParent<Player2DMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_lightAttackAction.WasPressedThisFrame() && canAttack){
            float angle = movementScript.GetAttackDirection();
            weaponPivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Animator animator = weaponPivot.GetComponentInChildren<Animator>();
            animator.SetTrigger("Attack");
            canAttack = false;
        }
    }

    public void EndOfLightAttack(){
        StartCoroutine(ResetAttack(lightSwingCooldown));
    }

    private IEnumerator ResetAttack(float cooldown){
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
