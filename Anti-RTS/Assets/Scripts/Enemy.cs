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
    Team team = Team.ENEMY;

    void Move()
    {

    }

    void Stall()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
