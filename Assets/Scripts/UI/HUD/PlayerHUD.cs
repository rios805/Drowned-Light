using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PlayerHUD : MonoBehaviour
{
    [Header("HUD Text and Player")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CanvasGroup healthGroup;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private CanvasGroup staminaGroup;
    [SerializeField] private Slider sanitySlider;
    [SerializeField] private CanvasGroup sanityGroup;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Player player;
    
    
    [Header("Bar Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float visibleTime = 2f;
    
    private Coroutine _fadeCoroutine;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        player.OnPlayerHealthChanged += Player_OnPlayerHealthChanged;
        player.OnPlayerSanityChanged += Player_OnPlayerSanityChanged;
        player.OnPlayerStaminaChanged += Player_OnPlayerStaminaChanged;
        player.OnPlayerKilled += Player_OnPlayerKilled;
        
        
        healthGroup.alpha = 0;
        sanityGroup.alpha = 0;
        staminaGroup.alpha = 0f;
    }

    private void Player_OnPlayerHealthChanged(object sender, System.EventArgs e ) {
        healthSlider.value = player.GetHealth();
        showStats(healthGroup);
    }

    private void Player_OnPlayerSanityChanged(object sender, System.EventArgs e) {
        sanitySlider.value = player.GetSanity();
        showStats(sanityGroup);
    }

    private void Player_OnPlayerStaminaChanged(object sender, System.EventArgs e) {
        staminaSlider.value = player.GetStamina();
        showStats(staminaGroup);
    }

    private void Player_OnPlayerKilled(object sender, System.EventArgs e) { 
        gameOverScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    private void showStats(CanvasGroup canvasGroup)
    {
        //Stops any current fade routine and then restarts it
        if(_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(FadeSequence(canvasGroup));
    }

    private IEnumerator FadeSequence(CanvasGroup canvasGroup = null)
    {
        //Fade In
        yield return StartCoroutine(FadeTo(1f, fadeDuration, canvasGroup));

        //Time it remains on screen
        yield return new WaitForSeconds(visibleTime);

        //Fade Out
        yield return StartCoroutine(FadeTo(0f, fadeDuration, canvasGroup));
    }

    private IEnumerator FadeTo(float targetAlpha, float duration, CanvasGroup canvasGroup)
    {
        float start = canvasGroup.alpha;
        float t = 0f; 

        while(t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, targetAlpha, t/duration);
            yield return null; 
        }
        canvasGroup.alpha = targetAlpha;
    }
}
