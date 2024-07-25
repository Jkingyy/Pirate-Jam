using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject,IDamageable
{

    public int maxHealth;
    public int currentHealth;

    public int maxHeals;
    public int currentHeals;

    public int healPercentage;
    public int healThreshold;
    public void ResetHealth() {
        currentHealth = maxHealth;
    }

    public void ResetHeals() {
        currentHeals = maxHeals;
    }

    public void Heal(){
        int missingHealth = maxHealth - currentHealth;
        float missingHeathPercentage = (float)missingHealth / maxHealth * 100;
        if(missingHeathPercentage < healThreshold){
            currentHealth = maxHealth;
        }else{
            currentHealth += (int)(missingHealth * (healPercentage / 100f));
        }
        
        currentHeals--;
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= (int)damageAmount;
    }
}
