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
    [SerializeField] private Slider flashlightSlider;
    [SerializeField] private TextMeshProUGUI flashlightBatteryText;
    [SerializeField] private CanvasGroup flashlightGroup;
    public CanvasGroup damageFlash;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Player player;
    [SerializeField] private FlashLightController flashLight;
    
    
    [Header("Bar Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float visibleTime = 2f;
    
    private Coroutine _fadeCoroutine;
    private Coroutine healthFade;
    private Coroutine staminaFade;
    private Coroutine sanityFade;
    private Coroutine flashlightFade;
    private Coroutine damageFlashFade;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        player.OnPlayerHealthChanged += Player_OnPlayerHealthChanged;
        player.OnPlayerSanityChanged += Player_OnPlayerSanityChanged;
        player.OnPlayerStaminaChanged += Player_OnPlayerStaminaChanged;
        player.OnPlayerKilled += Player_OnPlayerKilled;
        
        flashLight.OnFlashlightBatteryPercentChanged += Flashlight_OnFlashlightBatteryPercentChanged;
        flashLight.OnFlashlightBatteryCountChanged += Flashlight_OnFlashlightBatteryCountChanged;
        
        
        healthGroup.alpha = 0;
        sanityGroup.alpha = 0;
        staminaGroup.alpha = 0f;
        flashlightGroup.alpha = 0f;
        damageFlash.alpha = 0f;
        flashlightBatteryText.text = "X" + flashLight.GetBatteryCount();
    }

    private void Flashlight_OnFlashlightBatteryPercentChanged(object sender, System.EventArgs e) {
        flashlightSlider.value = flashLight.GetPercentage();
        showStats(flashlightGroup);
    }
    private void Flashlight_OnFlashlightBatteryCountChanged(object sender, System.EventArgs e) {
        flashlightBatteryText.text = "X" + flashLight.GetBatteryCount();
        showStats(flashlightGroup);
    }

    private void Player_OnPlayerHealthChanged(object sender, System.EventArgs e ) {
        healthSlider.value = player.GetHealth();
        showStats(healthGroup);
    }

    private void Player_OnPlayerSanityChanged(object sender, System.EventArgs e) {
        sanitySlider.value = player.GetSanity();
        if (player.GetSanity() <= 99f) {
            showStats(sanityGroup);
        }
    }

    private void Player_OnPlayerStaminaChanged(object sender, System.EventArgs e) {
        staminaSlider.value = player.GetStamina();
        showStats(staminaGroup);
    }

    private void Player_OnPlayerKilled(object sender, System.EventArgs e) { 
        gameOverScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void showStats(CanvasGroup canvasGroup) {
        Coroutine fade = null;
        
        if (canvasGroup == healthGroup) fade = healthFade;
        else if (canvasGroup == staminaGroup) fade = staminaFade;
        else if (canvasGroup == sanityGroup) fade = sanityFade;
        else if (canvasGroup == flashlightGroup) fade = flashlightFade;
        else if (canvasGroup == damageFlash) fade = damageFlashFade;

        if (fade != null) StopCoroutine(fade);

        Coroutine newFade = StartCoroutine(FadeSequence(canvasGroup));

        if (canvasGroup == healthGroup) healthFade = newFade;
        else if (canvasGroup == staminaGroup) staminaFade = newFade;
        else if (canvasGroup == sanityGroup) sanityFade = newFade;
        else if (canvasGroup == flashlightGroup) flashlightFade = newFade;
        else if (canvasGroup == damageFlash) damageFlashFade = newFade;
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
