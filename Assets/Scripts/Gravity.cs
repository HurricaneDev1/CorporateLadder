using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]private float gravity;
    [SerializeField]private float terminalVelocity;
    [SerializeField]private int mass = 1;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(rb.velocity.y < terminalVelocity){
            rb.velocity = new Vector2(rb.velocity.x, terminalVelocity);
        }


        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - (gravity * mass));
    }
}
