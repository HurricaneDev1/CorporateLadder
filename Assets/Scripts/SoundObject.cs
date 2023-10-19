using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField]private float soundRadius;
    [SerializeField]private Transform soundCircleObject;
    [SerializeField]private float timeForAnimation;
    private bool animGoing;
    void Start(){
        soundCircleObject = Instantiate(Resources.Load<GameObject>("SoundCircle"), transform.position, Quaternion.identity).transform;
        soundCircleObject.SetParent(transform);
    }
    void Update(){
        SoundCircleAnim();
    }
    //The MakeSound function you're going to be using most of the time. Tells every soundobject in the radius that this thing made a sound
    public void MakeSound(){
        animGoing = true;
        foreach(Collider2D col in Physics2D.OverlapCircleAll(transform.position, soundRadius)){
            if(col.GetComponent<SoundObject>()){
                col.GetComponent<SoundObject>().HeardSound();
            }
            //Also make an actual sound
        }
    }
    //Same as other MakeSound but has the option to make more sound
    public void MakeSound(int amplitude){
        animGoing = true;
        foreach(Collider2D col in Physics2D.OverlapCircleAll(transform.position, soundRadius * amplitude)){
            if(col.GetComponent<SoundObject>()){
                col.GetComponent<SoundObject>().HeardSound();
            }
        }
    }

    void SoundCircleAnim(){
        Vector2 soundPos = soundCircleObject.localScale;
        if(animGoing){
            if(soundPos.x < soundRadius * 2){
                soundCircleObject.localScale = new Vector2(soundPos.x + (((soundRadius*2))/ timeForAnimation) * Time.deltaTime, soundPos.y + (((soundRadius*2))/ timeForAnimation) * Time.deltaTime);
            }else{
                animGoing = false;
                StartCoroutine(CircleGoAway());
            }
        }
    }

    IEnumerator CircleGoAway(){
        yield return new WaitForSeconds(0.15f);
        soundCircleObject.localScale = new Vector2(0,0);
        // if(soundPos.x > 0){
            //     soundCircleObject.localScale = new Vector2(soundPos.x - (((soundRadius*2))/ timeForAnimation) * Time.deltaTime, soundPos.y - (((soundRadius*2))/ timeForAnimation) * Time.deltaTime);
            // }
    }
    public void HeardSound(){

    }
    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position, soundRadius);
    }
}
