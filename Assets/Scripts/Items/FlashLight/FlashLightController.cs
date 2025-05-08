using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;


public class FlashLightController : MonoBehaviour
{
    public EventHandler OnFlashlightBatteryPercentChanged;
    public EventHandler OnFlashlightBatteryCountChanged;
    
    public bool isOn = false;
    public GameObject lightSource;
    public AudioSource clickSound;
    public bool failSafe = false;
    public float lifetime = 100;
    public int batteries = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (isOn && lifetime > 0)
        {
            OnFlashlightBatteryPercentChanged?.Invoke(this, EventArgs.Empty);
            lifetime -= 1 * Time.deltaTime; 
            if (lifetime <= 0)
            {
                lifetime = 0;
                lightSource.SetActive(false);
                isOn = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && lifetime > 0)
        {
            if(isOn == false && failSafe == false)
            {
                failSafe = true;
                lightSource.SetActive(true);
                clickSound.Play();
                isOn = true;
                StartCoroutine(FailSafe());
            }
            if (isOn == true && failSafe == false)
            {
                failSafe = true;
                lightSource.SetActive(false);
                clickSound.Play();
                isOn = false;
                StartCoroutine(FailSafe());
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            if (batteries > 0 && lifetime < 100f)
            {
                batteries -= 1;
                lifetime += 50;
                if (lifetime > 100f) lifetime = 100f;
                OnFlashlightBatteryCountChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            OnFlashlightBatteryCountChanged?.Invoke(this, EventArgs.Empty);
            batteries += 1;
            Destroy(other.gameObject);
        }
    }

    IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(0.25f);
        failSafe = false;
    }

    public float GetPercentage() {
        return lifetime;
    }

    public int GetBatteryCount() {
        return batteries;
    }
}
