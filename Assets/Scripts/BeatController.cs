using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatController : MonoBehaviour{
    
    public AudioSource audioSource;
    public float bpm;

    public float t;
    public float beatLength;

    public int beats = 1;

    public UnityEvent BeatEvent;

    void Awake(){
        beatLength = 60f / (bpm *4);
    }

    void Update(){
        t = (audioSource.timeSamples / (audioSource.clip.frequency * beatLength));
        updateBeats();
    }

    void updateBeats(){
        int nextBeat = Mathf.FloorToInt(t) + 1;
        if (nextBeat != beats){
            beats = nextBeat;
            BeatEvent.Invoke();
        }
    }
}
