using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth;
    public float maxPlayerHealth = 100;
    public TextMeshProUGUI playerHealthText;
    public Slider playerHealthBar;

    public void Start()
    {
        playerHealth = maxPlayerHealth;
        UpdateUI();
    }
    public void RemovePlayerHealth(float _removedHealth)
    {
        playerHealth = playerHealth - _removedHealth;
        UpdateUI();
        if(playerHealth <= 0)
            PlayerIsDead();
    }
    public void PlayerIsDead()
    {
        print("Player is dead");
        // Other Actions
    }
    public void UpdateUI()
    {
        playerHealthBar.value = playerHealth;
        playerHealthText.text = playerHealth.ToString(); 
    }
}
