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
    private Transform player;
    [SerializeField]private float attentionSpan;
    private float attentionTracker;
    protected new void Start(){
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MoveLoser>().transform;
        attentionTracker = Mathf.Infinity;
    }
    void Update(){
        attentionTracker -= Time.deltaTime;
        if(attentionTracker <= 0 && state == EnemyState.Curious){
            state = EnemyState.Idle;
            questionMark.SetActive(false);
            attentionTracker = Mathf.Infinity;
        }
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
            case EnemyState.Alert:
                Vector2 PlayerDirection = player.transform.position - transform.position;
                if(moveDirection != PlayerDirection.x/Mathf.Abs(PlayerDirection.x))ChangeDirection();
                rb.velocity = new Vector2(moveDirection * moveSpeed * 5, rb.velocity.y);
                if(Vector3.Distance(transform.position, player.position) < 0.5f){
                    player.GetComponent<MoveLoser>().Die();
                }
                break;
        }
    }
    //If the enemy hears a sound it will change their state
    public override void HeardSound(Vector2 posOfSound)
    {
        switch(state){
            case EnemyState.Idle:
                state = EnemyState.Curious;
                attentionTracker = attentionSpan;
                questionMark.SetActive(true);
                break;
            case EnemyState.Curious:
                state = EnemyState.Alert;
                questionMark.SetActive(false);
                exclamationMark.SetActive(true);
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