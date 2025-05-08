using TMPro;
using UnityEngine;
using System.Collections;

public class FlickerText : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public float flickerSpeed = 0.05f;
    private Coroutine flickerCoroutine;

    void OnEnable()
    {
        flickerCoroutine = StartCoroutine(Flicker());
    }

    void OnDisable()
    {
        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            tmpText.alpha = Random.Range(0.3f, 1f);
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
