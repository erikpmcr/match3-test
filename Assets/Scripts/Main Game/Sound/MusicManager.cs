using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    
    public AudioSource sounds;

    public static MusicManager inst = null;

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }

        else if (inst != this)
        {
            Destroy(gameObject);
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