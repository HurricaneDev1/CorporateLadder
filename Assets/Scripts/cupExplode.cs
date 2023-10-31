using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cupExplode : MonoBehaviour
{
    [SerializeField] private GameObject Smash;
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag != "Player"){
            Instantiate(Smash, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
