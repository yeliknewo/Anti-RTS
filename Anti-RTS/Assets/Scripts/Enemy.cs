using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    int maxHealth;
    public int _maxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
        }
    }
    [SerializeField] int currentHealth;
    double stallTimeLeft;
    double movementSpeed;
    double movementLeft;
    Unit unitType;
    //Chunk targetChunk;
    //Path path;

    void Move()
    {

    }

    void Stall()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
