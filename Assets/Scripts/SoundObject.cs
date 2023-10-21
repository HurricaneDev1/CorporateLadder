using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField]private float soundRadius = 2;
    private Transform soundCircleObject;
    [SerializeField]private float timeForAnimation = 0.1f;
    private bool animGoing;
    [SerializeField]private bool soundEmitter;
    protected void Start(){
        soundCircleObject = Instantiate(Resources.Load<GameObject>("SoundCircle"), transform.position, Quaternion.identity).transform;
        soundCircleObject.SetParent(transform);
    }
    void Update(){
        SoundCircleAnim();
    }
    //The MakeSound function you're going to be using most of the time. Tells every soundobject in the radius that this thing made a sound
    public void MakeSound(){
        if(soundEmitter){
            animGoing = true;
            foreach(Collider2D col in Physics2D.OverlapCircleAll(transform.position, soundRadius)){
                if(col.GetComponent<SoundObject>()){
                    col.GetComponent<SoundObject>().HeardSound(transform.position);
                }
                //Also make an actual sound
            }
        }
    }
    //Same as other MakeSound but has the option to make more sound
    public void MakeSound(int amplitude){
        if(soundEmitter){
            animGoing = true;
            foreach(Collider2D col in Physics2D.OverlapCircleAll(transform.position, soundRadius * amplitude)){
                if(col.GetComponent<SoundObject>()){
                    col.GetComponent<SoundObject>().HeardSound(transform.position);
                }
            }
        }
    }
    //Does a little animation to show how far your sound reachs
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
    //Gets rid of the sound circle
    IEnumerator CircleGoAway(){
        yield return new WaitForSeconds(0.15f);
        soundCircleObject.localScale = new Vector2(0,0);
        // if(soundPos.x > 0){
            //     soundCircleObject.localScale = new Vector2(soundPos.x - (((soundRadius*2))/ timeForAnimation) * Time.deltaTime, soundPos.y - (((soundRadius*2))/ timeForAnimation) * Time.deltaTime);
            // }
    }
    //Tells an object that it heard a sound. Meant to be changed in child scripts
    public virtual void HeardSound(Vector2 posOfSound){

    }
    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position, soundRadius);
    }
}
