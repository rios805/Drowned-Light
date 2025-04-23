using TMPro;
using UnityEngine;

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
        
        healthText.text = "Health: " + player.GetHealth();
        sanityText.text = "Sanity: " + Mathf.CeilToInt(player.GetSanity());
        staminaText.text = "Stamina: " +  Mathf.CeilToInt(player.GetStamina());
    }

    private void Player_OnPlayerHealthChanged(object sender, System.EventArgs e ) {
        healthText.text = "Health: " + player.GetHealth();
    }

    private void Player_OnPlayerSanityChanged(object sender, System.EventArgs e) {
        sanityText.text = "Sanity: " +  Mathf.CeilToInt(player.GetSanity());
    }

    private void Player_OnPlayerStaminaChanged(object sender, System.EventArgs e) {
        staminaText.text = "Stamina: " + Mathf.CeilToInt(player.GetStamina());
    }

    private void Player_OnPlayerKilled(object sender, System.EventArgs e) { 
        gameOverScreen.SetActive(true);
        gameObject.SetActive(false);
    }
    
}
