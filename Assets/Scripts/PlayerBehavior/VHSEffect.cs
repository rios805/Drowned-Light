using System;
using UnityEngine;
using UnityEngine.Rendering;
using VolFx;

public class VHSEffect : MonoBehaviour
{
    private Player player;
    private float playerSanity;
    private VhsVol vhsVol;
    private Volume volume;
    private void Start() {
        player = Player.Instance;
        player.OnPlayerSanityChanged += Player_OnPlayerSanityChanged;
    }

    private void Awake() {
        volume = GetComponent<Volume>();
        volume.profile.TryGet<VhsVol>(out vhsVol);
    }

    void Player_OnPlayerSanityChanged(object sender, System.EventArgs e) {
        playerSanity = player.GetSanity();
        
        vhsVol._weight.value = 1f - playerSanity * .01f;
        vhsVol._noise.value = 1f - playerSanity * .01f;
        vhsVol._flickering.value = 1f - playerSanity * .01f;
        
        vhsVol._noise.value = Mathf.Clamp(vhsVol._noise.value, 0f, .80f);
        
    }
}
