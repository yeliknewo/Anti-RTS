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
    float stallTimeLeft;
    float Timer;
    float movementSpeed;
    double movementLeft;
    UnitType unitType;
    Chunk targetChunk;
    Path path;
    static Vector2 pathTo;
    Team team = Team.ENEMY;

    void Move()
    {
        Timer += Time.deltaTime * movementSpeed;

        if (Player.transform.position != targetChunk)
        {
            path.TakeNextNode();
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, pathTo, Timer);
        }
    }

    void Stall()
    {
        StartCoroutine(sleep());
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
    }

    IEnumerator sleep()
    {
        yield return new WaitForSeconds(stallTimeLeft);
    }
}
