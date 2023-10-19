using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private SoundObject soundObject;
    void Start(){
        soundObject = GetComponent<SoundObject>();
    }   
    public void OnCollisionEnter2D(Collision2D col){
        soundObject.MakeSound();
    }
}
