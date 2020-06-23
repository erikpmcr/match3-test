using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

    
    public AudioSource sounds;

    public static SFXManager voice1 = null;
    public static SFXManager voice2 = null;

    void Awake()
    {
        if (voice1 == null)
        {
            voice1 = this;
        }

        else if (voice1 != this)
        {
            if (voice2 == null)
            {
                voice2 = this;
            }

            else if (voice2 != this)
            {
                Destroy(gameObject);
            }
            
        }

        DontDestroyOnLoad(this.gameObject);
        sounds = this.gameObject.GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clipAudio)
    {
        sounds.clip = clipAudio;
        print(sounds.clip);
        sounds.Play();
    }
}