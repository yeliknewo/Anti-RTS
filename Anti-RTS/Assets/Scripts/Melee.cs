using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    Enemy enemy
    {
        get {
            return this.gameObject.GetComponent<Enemy>();
        }
    }
    
    int damage;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Enemy>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
