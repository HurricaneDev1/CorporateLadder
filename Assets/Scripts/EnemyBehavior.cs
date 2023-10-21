using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : SoundObject
{
    private EnemyState state;
    [SerializeField]private GameObject questionMark, exclamationMark;
    private int moveDirection = 1;
    [SerializeField]private int moveSpeed;
    private Rigidbody2D rb;
    private Vector2 directionOfSound;
    protected new void Start(){
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update(){
        switch(state){
            //Randomly moves the enemy, and changes its direction randomly. Should change later
            case EnemyState.Idle:
                rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
                if(Random.Range(0,1000) == 0){
                    ChangeDirection();
                }
                break;
            case EnemyState.Curious:
                //Makes the enemy face towards the direction of the last sound they heard and move toward it
                if(moveDirection != directionOfSound.x/Mathf.Abs(directionOfSound.x))ChangeDirection();
                rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
                break;
        }
    }
    //If the enemy hears a sound it will change their state
    public override void HeardSound(Vector2 posOfSound)
    {
        switch(state){
            case EnemyState.Idle:
                state = EnemyState.Curious;
                questionMark.SetActive(true);
                break;
            case EnemyState.Curious:
                // state = EnemyState.Alert;
                // questionMark.SetActive(false);
                // exclamationMark.SetActive(true);
                break;
        }
        directionOfSound = posOfSound - (Vector2)transform.position;
        directionOfSound.Normalize();
    }
    //Flips the direction the enemy is facing and moving
    void ChangeDirection(){
        moveDirection *= -1;
        transform.rotation = Quaternion.Euler(0, moveDirection == 1? 0 : 180 , 0);
    }
}

public enum EnemyState{
    Idle,
    Curious,
    Alert
}