using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createSoundOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private SoundObject sound;
    void Start()
    {
        sound = GetComponent<SoundObject>();
        sound.MakeSound();
    }
}
