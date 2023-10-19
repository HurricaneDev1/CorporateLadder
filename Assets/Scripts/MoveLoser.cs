using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveLoser : MonoBehaviour
{
    [SerializeField] private Vector2 vel = new Vector2(0,0);
    private Rigidbody2D rb;
    public int speed = 5;

    public int jumpSpeed = 15;
    public float gravity = .5f;
    [SerializeField] private float terminalVelocity = 20;
    private bool moving = false;

    [SerializeField] private bool wallJump = true;
    private bool onWall = false;

    [SerializeField]private bool jumpingTriggered = false;

    [SerializeField] private float jumpPressTime = .2f;
    [SerializeField] private Transform point;
    [SerializeField] private Transform right;
    [SerializeField] private Transform left;
    [SerializeField] private bool grounded = false;
    [SerializeField] private float footCircleSize = .3f;
    [SerializeField]private LayerMask groundLayer;
    private float tempVelX = 0f;
    [SerializeField] private float drag = 1.05f;
    [SerializeField] private float accleration = 10f;
    [SerializeField] private float cyoTime = .2f;
    private bool gravityBool = false;
    [SerializeField] private ParticleSystem RightWallParticleSystem;
    [SerializeField] private ParticleSystem LeftWallParticleSystem;

    private IEnumerator holdJumpThing;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        holdJumpThing = HoldJump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Mathf.Abs(tempVelX) < 1 || Mathf.Abs(tempVelX)/tempVelX != Mathf.Abs(vel.x)/vel.x){
            tempVelX += vel.x/accleration;
        }

        if(vel.x == 0){
            tempVelX /= drag;
        }
        if(rb.velocity.y == 0 && vel.y < jumpSpeed -1 && vel.y > 0){
            vel = new Vector2(vel.x, 0);
            Debug.Log("hit roof");
        }
        if(rb.velocity.x == 0 && vel.x == 0){
            tempVelX = 0;
        }
        if(!onWall){
            if(vel.y < terminalVelocity){
                vel = new Vector2(vel.x, terminalVelocity); 
            }
            rb.velocity = new Vector2(tempVelX*speed,vel.y);
            if(!RightWallParticleSystem.isStopped){
                RightWallParticleSystem.Stop();
                //RightWallParticleSystem.Clear();
            }
            if(!LeftWallParticleSystem.isStopped){
                LeftWallParticleSystem.Stop();
                //LeftWallParticleSystem.Clear();
            }
            
        } else {
            if(vel.y < terminalVelocity/10){
                vel = new Vector2(vel.x, terminalVelocity/10); 
            }
            rb.velocity = new Vector2(tempVelX*speed,vel.y);
            if(Physics2D.Raycast(right.position, new Vector2(1,0),footCircleSize, groundLayer) && vel.x == -1){
                Debug.Log("dkjlfhasjkdhfk");
                if(!LeftWallParticleSystem.isPlaying){
                    LeftWallParticleSystem.Play();
                }
            }
            if(Physics2D.Raycast(left.position, new Vector2(-1,0),footCircleSize, groundLayer) && vel.x == 1){
                if(!RightWallParticleSystem.isPlaying){
                    RightWallParticleSystem.Play();
                }
                
            }
        }
        if(gravityBool == true){
                vel = new Vector2(vel.x, vel.y - gravity);
            } else {
                vel = new Vector2(vel.x,0);
            }

        if(!moving){
            vel = new Vector2(0,vel.y);
        }
        

        if(vel.y <= 0){
            if(Physics2D.CircleCast(point.position, footCircleSize, new Vector2(0,0),footCircleSize, groundLayer)){
                grounded = true;
                gravityBool = false;
            } else {
                StartCoroutine(CoyoteTime(cyoTime));
                gravityBool = true;
            }
        }
        if(wallJump){
            //Check if wall is next to me then fall slower next to wall
            if(Physics2D.Raycast(right.position, new Vector2(1,0),footCircleSize, groundLayer) && vel.x == -1 || Physics2D.Raycast(left.position, new Vector2(-1,0),footCircleSize, groundLayer) && vel.x == 1){
                onWall = true;
            } else {
                onWall = false;
            }
        }
        
        if(jumpingTriggered && grounded){
         
            vel = new Vector2(vel.x, jumpSpeed);
            grounded = false;
            gravityBool = true;
            jumpingTriggered = false;
            
        } else if(jumpingTriggered && onWall){
             if(vel.x == 1){
                vel = new Vector2(vel.x, jumpSpeed);
                tempVelX = -speed/3;
            } else if(vel.x == -1){
                vel = new Vector2(vel.x, jumpSpeed);
                tempVelX = speed/3;
            }
        }
    }
    
    public void Walk(InputAction.CallbackContext context){
        if(context.action.triggered){
            vel = new Vector2(context.ReadValue<Vector2>().x,vel.y);
            moving = true;
        } else if(context.ReadValue<Vector2>() == new Vector2(0,0)){
            //stop move
            
            moving = false;
        }
        
    }
    public void Jump(InputAction.CallbackContext context){
        
        if(context.action.triggered && grounded){
            vel = new Vector2(vel.x, jumpSpeed);
            grounded = false;
            gravityBool = true;
            
            
        } else if(context.action.triggered && onWall){
            if(vel.x == 1){
                vel = new Vector2(vel.x, jumpSpeed);
                tempVelX = -speed/3;
            } else if(vel.x == -1){
                vel = new Vector2(vel.x, jumpSpeed);
                tempVelX = speed/3;
            }
        } else if(context.action.triggered) {
            jumpingTriggered = true;
            StartCoroutine(HoldJump());
        }
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
        
    }
    
    private IEnumerator CoyoteTime(float time){
        yield return new WaitForSeconds(time);
        grounded = false;
    }
    private IEnumerator HoldJump(){
        yield return new WaitForSeconds(jumpPressTime);
        jumpingTriggered = false;
        Debug.Log("Jump time stoped");
    }
    
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(point.position,footCircleSize);
        Gizmos.DrawRay(right.position, new Vector2(1,0));
    }
}
