using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveLoser : MonoBehaviour
{
    private Vector2 vel = new Vector2(0,0);
    private Rigidbody2D rb;
    public int speed = 5;

    public int jumpSpeed = 15;
    public float gravity = .5f;
    private bool moving = false;
    [SerializeField] private Transform point;
    [SerializeField] private bool grounded = false;
    [SerializeField] private float footCircleSize = .3f;
    [SerializeField]private LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(vel.x*speed,vel.y);
        if(grounded == false){
            vel = new Vector2(vel.x, vel.y - gravity);
        } else {
            vel = new Vector2(vel.x,0);
        }
        if(!moving){
            vel = new Vector2(vel.x/1.05f,vel.y);
        }
        if(Physics2D.CircleCast(point.position, footCircleSize, new Vector2(0,0),footCircleSize, groundLayer)){
            grounded = true;
        }
    }
    public void walk(InputAction.CallbackContext context){
        if(context.action.triggered){
            vel = new Vector2(context.ReadValue<Vector2>().x,vel.y);
            moving = true;
        } else if(context.ReadValue<Vector2>() == new Vector2(0,0)){
            //stop move
            
            moving = false;
        }
        
    }
    public void jump(InputAction.CallbackContext context){
        
        if(context.action.triggered && grounded){
            vel = new Vector2(vel.x, jumpSpeed);
            grounded = false;
        } 
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
        
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(point.position,footCircleSize);
    }
}
