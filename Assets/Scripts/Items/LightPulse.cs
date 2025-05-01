using UnityEngine;

public class LightPulse : MonoBehaviour
{
    public float minIntensity = 0.2f;
    public float maxIntensity = 1f;
    public float pulseSpeed = 2f;

    private Light pointLight;

    void Start()
    {
        pointLight = GetComponent<Light>();
    }

    void Update()
    {
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        pointLight.intensity = intensity;
    }
}
