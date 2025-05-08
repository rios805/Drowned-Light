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
    // public GameObject laserBeam;
    public AudioSource clickSound;
    // public AudioSource laserSound;
    public bool failSafe = false;
    // private bool laserOnCooldown = false;
    // private bool laserIsOn = false;
    


    public float lifetime = 100;
    public float batteries = 0;



    // Update is called once per frame
    void Update()
    {
        if (isOn && lifetime > 0)
        {
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
            }
        }

//    if (Input.GetKeyDown(KeyCode.V))
// {
//     if (laserBeam == null || laserOnCooldown)
//         return;

//     if (!laserIsOn && lifetime > 0)
//     {
//         float drainAmount = lifetime / 3f;
//         lifetime -= drainAmount;
//         if (lifetime < 0) lifetime = 0;

//         laserBeam.SetActive(true);
//         laserIsOn = true;

//         if (laserSound != null && !laserSound.isPlaying)
//             laserSound.Play();
//     }
//     else if (laserIsOn)
//     {
//         laserBeam.SetActive(false);
//         laserIsOn = false;

//         if (laserSound != null && laserSound.isPlaying)
//             laserSound.Stop();

//         StartCoroutine(LaserCooldown());
//     }
// }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            batteries += 1;
            Destroy(other.gameObject);
        }
    }

    IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(0.25f);
        failSafe = false;
    }

//    IEnumerator LaserCooldown()
// {
//     laserOnCooldown = true;

   
//     yield return new WaitForSeconds(5f);


//     laserOnCooldown = false;
// }

}
