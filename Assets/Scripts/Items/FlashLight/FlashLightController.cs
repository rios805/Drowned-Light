using UnityEngine;
using System.Collections;
using TMPro;


public class FlashLightController : MonoBehaviour
{

    public bool isOn = false;
    public GameObject lightSource;
    public GameObject laserBeam;
    public AudioSource clickSound;
    public AudioSource laserSound;
    public bool failSafe = false;
    private bool laserOnCooldown = false;
    private bool laserIsOn = false;
    

    public TMP_Text text;
    public TMP_Text batteryText;


    public float lifetime = 100;
    public float batteries = 0;



    // Update is called once per frame
    void Update()
    {

        text.text = "BatteryLife: " + lifetime.ToString("0") + "%";
        batteryText.text = "Batteries: " + batteries.ToString();


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

        if (Input.GetKeyDown(KeyCode.F))
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

   if (Input.GetKeyDown(KeyCode.V))
{
    if (laserBeam == null || laserOnCooldown)
        return;

    if (!laserIsOn && lifetime > 0)
    {
        float drainAmount = lifetime / 3f;
        lifetime -= drainAmount;
        if (lifetime < 0) lifetime = 0;

        laserBeam.SetActive(true);
        laserIsOn = true;

        if (laserSound != null && !laserSound.isPlaying)
            laserSound.Play();
    }
    else if (laserIsOn)
    {
        laserBeam.SetActive(false);
        laserIsOn = false;

        if (laserSound != null && laserSound.isPlaying)
            laserSound.Stop();

        StartCoroutine(LaserCooldown());
    }
}


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

   IEnumerator LaserCooldown()
{
    laserOnCooldown = true;

   
    yield return new WaitForSeconds(5f);


    laserOnCooldown = false;
}

}
