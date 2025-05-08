using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject lowSanityMonster;
    [SerializeField] private Player player;
    [SerializeField] private float lowSanitySpawn;
    
    private float checkPlayerLocationTimer;
    private Transform lastPlayerTransform;
    private GameObject spawnedMonster;
    private float myPlayTime;
    private TimeSpan timeSpan;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.Instance;
        player.OnPlayerSanityChanged += Player_OnPlayerSanityChanged;
        
    }

    private void Player_OnPlayerSanityChanged(object sender, System.EventArgs e) {
        if (player.GetSanity() <= lowSanitySpawn && spawnedMonster == null) {
            spawnedMonster = GameObject.Instantiate(lowSanityMonster, lastPlayerTransform.position, lastPlayerTransform.rotation);
            lowSanityMonster.transform.position = lastPlayerTransform.position;
            Debug.Log("Player is going insane, make them see things!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayerLocationTimer -= Time.deltaTime;

        if (checkPlayerLocationTimer <= 0) {
            Debug.Log("Player Location saved");
            lastPlayerTransform = player.transform;
            checkPlayerLocationTimer = 10f;
        }
        
        myPlayTime += Time.deltaTime;
        
        timeSpan = TimeSpan.FromSeconds(myPlayTime);
        
    }

    public TimeSpan GetTimePlayed() {
        return timeSpan;
    }
    
}
