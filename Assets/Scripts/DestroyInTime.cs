using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    [SerializeField] private float time;
    

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0){
            Destroy(gameObject);
        }
    }
}
