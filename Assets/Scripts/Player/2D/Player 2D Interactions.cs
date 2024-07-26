using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player2DInteractions : MonoBehaviour
{

    public PlayerStats playerStats;

    private Image HealthBar;
    private TextMeshProUGUI HealsText;
    

    [SerializeField] private float healCooldown = 1f;
    private PlayerInput _playerInput;
    private InputAction _heal;
    private bool _canHeal = true;
    private Player2DMovement _player2DMovement;
    private Weapon _weapon;

    private ParticleSystem _healParticles;
    private void Awake() {
        _player2DMovement = GetComponent<Player2DMovement>();
        _playerInput = GetComponent<PlayerInput>();
        _heal = _playerInput.actions.FindAction("Heal");
        _healParticles = GetComponentInChildren<ParticleSystem>();
        _weapon = GetComponentInChildren<Weapon>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        HealthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        HealsText = GameObject.FindGameObjectWithTag("HealsCounter").GetComponent<TextMeshProUGUI>();        
        
        ResetStats();
    }

    void ResetStats() {
        playerStats.ResetHealth();
        playerStats.ResetHeals();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(_heal.WasPressedThisFrame()){
            if(playerStats.currentHeals > 0 && _canHeal){
                StartCoroutine(Heal());
            }
        }
    }

    private IEnumerator Heal(){
        _player2DMovement._canDash = false;
        _weapon.canAttack = false;
        _canHeal = false;
        playerStats.Heal();
        UpdateUI();
        _healParticles.Play();
        yield return new WaitForSeconds(healCooldown);
        _weapon.canAttack = true;
        _canHeal = true;
        _player2DMovement._canDash = true;
    }

    void UpdateHealthUI() {
        // Update UI
        HealthBar.fillAmount = (float)playerStats.currentHealth / playerStats.maxHealth;
    }
    void UpdateHealsUI() {
        // Update UI
        HealsText.text = playerStats.currentHeals.ToString();
    }

    void UpdateUI(){
        UpdateHealthUI();
        UpdateHealsUI();
    }

    // void OnTriggerEnter2D(Collider2D other) {
    //     if (other.CompareTag("Enemy")) {
    //         playerStats.Damage(10);
    //         UpdateHealthUI();
    //     }
    // }

    public void Damage(float damageAmount)
    {
        playerStats.Damage(damageAmount);
        UpdateHealthUI();
    }
}
