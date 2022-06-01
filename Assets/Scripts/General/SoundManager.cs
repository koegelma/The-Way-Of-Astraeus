using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource ammoPickupSFX;
    public AudioSource coinPickupSFX;

    public void PlayAmmoPickup()
    {
        ammoPickupSFX.Play();
    }

    public IEnumerator QueuePlayCoinPickup()
    {
        yield return new WaitUntil(() => !coinPickupSFX.isPlaying);
        coinPickupSFX.Play();
    }
}
