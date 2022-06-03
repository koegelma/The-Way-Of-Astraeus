using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource ammoPickupSFX;
    public AudioSource coinPickupSFX;
    private bool canPlayCoin = true;
    private Coroutine coinCoroutine;
    public AudioSource stageClearedSFX;
    public AudioSource empSFX;
    public AudioSource gameOverSFX;

    public void PlayAmmoPickup()
    {
        ammoPickupSFX.Play();
    }

    public void PlayStageCleared()
    {
        stageClearedSFX.Play();
    }

    public void PlayGameOver()
    {
        gameOverSFX.Play();
    }

    public void PlayEMP()
    {
        empSFX.Play();
    }

    public IEnumerator DelayPlayEMP(float _delaySeconds)
    {
        yield return new WaitForSeconds(_delaySeconds);
        empSFX.Play();
    }

    public void PlayCoinPickup()
    {
        if (canPlayCoin)
        {
            coinPickupSFX.Play();
            if (coinCoroutine == null) coinCoroutine = StartCoroutine(HndCoinTimer());
        }
    }

    private IEnumerator HndCoinTimer()
    {
        canPlayCoin = false;
        yield return new WaitForSeconds(0.1f);
        canPlayCoin = true;
        coinCoroutine = null;
    }

    public IEnumerator QueuePlayCoinPickup()
    {
        yield return new WaitUntil(() => !coinPickupSFX.isPlaying);
        coinPickupSFX.Play();
    }
}
