using UnityEngine;

public class UnloadPrevious : MonoBehaviour
{
    [Header("RemoveFromPreviousFloor")]
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private GameObject blocker;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            foreach (GameObject gameObject in gameObjects) {
                gameObject.SetActive(false);
            }
            blocker.SetActive(true);
            Destroy(this);
        }
    }
}
