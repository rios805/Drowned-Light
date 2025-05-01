using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PlayerHUD : MonoBehaviour
{
    [Header("HUD Text and Player")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI sanityText;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Player player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        player.OnPlayerHealthChanged += Player_OnPlayerHealthChanged;
        player.OnPlayerSanityChanged += Player_OnPlayerSanityChanged;
        player.OnPlayerStaminaChanged += Player_OnPlayerStaminaChanged;
        player.OnPlayerKilled += Player_OnPlayerKilled;
        
        healthText.text = "100/100";
        sanityText.text = "100/100";
        staminaText.text = "100/100";
    }

    private void Player_OnPlayerHealthChanged(object sender, System.EventArgs e ) {
        healthText.text = player.GetHealth() + "/100";
    }

    private void Player_OnPlayerSanityChanged(object sender, System.EventArgs e) {
        sanityText.text = Mathf.CeilToInt(player.GetSanity()) + "/100";
    }

    private void Player_OnPlayerStaminaChanged(object sender, System.EventArgs e) {
        staminaText.text = Mathf.CeilToInt(player.GetStamina()) + "/100";
    }

    private void Player_OnPlayerKilled(object sender, System.EventArgs e) { 
        gameOverScreen.SetActive(true);
        gameObject.SetActive(false);
    }
    
}
