using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicPlayer : MonoBehaviour
{
    public AudioSource introSource, loopSource;

    // Start is called before the first frame update
    void Start()
    {
        introSource.Play();
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
        //if (PauseMenu.isPause) loopSource.pitch *= .5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
