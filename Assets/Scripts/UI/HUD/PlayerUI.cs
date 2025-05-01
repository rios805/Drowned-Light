using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
public class PlayerUI : MonoBehaviour
{
    [Header("Player UI Bars")]
    [SerializeField] private Player player;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image sanityBar;
    [SerializeField] private CanvasGroup statsGroup; 

    [Header("Bar Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float visibleTime = 2f; 

    private Coroutine _fadeCoroutine;

    void Start() {
        player.OnPlayerHealthChanged += Player_OnPlayerHealthChanged;
        player.OnPlayerSanityChanged += Player_OnPlayerSanityChanged;
        player.OnPlayerStaminaChanged += Player_OnPlayerStaminaChanged;
    }

    private void Player_OnPlayerHealthChanged(object sender, System.EventArgs e ) {
        float health = player.GetHealth();
        float healthFill = health / player.startHealth;
        healthBar.fillAmount = healthFill;
        showStats();
    }

    private void Player_OnPlayerSanityChanged(object sender, System.EventArgs e) {
        float sanity = player.GetSanity();
        float sanityFill  = sanity / player.startSanity;
        sanityBar.fillAmount = sanityFill;
        showStats();
    }

    private void Player_OnPlayerStaminaChanged(object sender, System.EventArgs e) {
        float stamina = player.GetStamina();
        float staminaFill = stamina / player.startStamina; 
        staminaBar.fillAmount = staminaFill; 
        showStats();
    }
     private void showStats()
    {
        //Stops any current fade routine and then restarts it
        if(_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        //Fade In
        yield return StartCoroutine(FadeTo(1f, fadeDuration));

        //Time it remains on screen
        yield return new WaitForSeconds(visibleTime);

        //Fade Out
        yield return StartCoroutine(FadeTo(0f, fadeDuration));
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float start = statsGroup.alpha;
        float t = 0f; 

        while(t < duration)
        {
            t += Time.deltaTime;
            statsGroup.alpha = Mathf.Lerp(start, targetAlpha, t/duration);
            yield return null; 
        }
        statsGroup.alpha = targetAlpha;
    }
}
