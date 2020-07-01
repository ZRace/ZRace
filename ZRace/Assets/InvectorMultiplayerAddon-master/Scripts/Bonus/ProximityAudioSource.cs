using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityAudioSource : MonoBehaviour
{
    public AudioSource source;
    public bool canPlay = false;
    public float fadeSpeed = 0.25f;
    [HideInInspector] public float setVolume = 0;
    [HideInInspector] public bool fadeIn = false;

    private void Start()
    {
        source.loop = true;
    }
    private void Update()
    {
        if (canPlay == false) return;
        if (fadeIn == true && source.volume < setVolume)
        {
            source.volume += Time.deltaTime * fadeSpeed;
            if (source.volume > setVolume)
            {
                source.volume = setVolume;
            }
        }
        else if (source.volume > setVolume)
        {
            source.volume -= Time.deltaTime * fadeSpeed;
            if (source.volume < setVolume)
            {
                source.volume = setVolume;
            }
        }
        if (setVolume > 0 && source.isPlaying == false)
        {
            source.Play();
        }
        if (source.volume == 0)
        {
            source.clip = null;
            source.Stop();
        }
    }
}
