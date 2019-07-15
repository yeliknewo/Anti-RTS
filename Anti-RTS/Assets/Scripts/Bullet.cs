using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] PolygonCollider2D col;
    [SerializeField] int damage;
    Team team;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 position = transform.position;

        //position = new Vector2(position.x, position.y + 5f * Time.deltaTime);

        //transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //idEnum = collision.gameObject.GetComponent<Identifier>().is

        if (collision.gameObject.GetComponent<Identifier>().IsBase())
        {
            collision.gameObject.GetComponent<Base>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.GetComponent<Identifier>().IsWall())
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.GetComponent<Identifier>().IsEnemy() && team == Team.PLAYER)
        {
            Debug.Log("Hit Enemy");
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.GetComponent<Identifier>().IsPlayer() && team == Team.ENEMY)
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
        
        if (collision.gameObject.GetComponent<Identifier>().IsBullet())
        {
            if ((team == Team.ENEMY && collision.gameObject.GetComponent<Bullet>().GetTeam() == Team.PLAYER) || (team == Team.PLAYER && collision.gameObject.GetComponent<Bullet>().GetTeam() == Team.ENEMY))
            {
                Destroy(gameObject);
            }
        }
    }


    void OnCollideWithEnemy(Enemy other)
    {
        
    }

    public void SetDamage(int damageIn)
    {
        damage = damageIn;
    }

    public void SetTeam(Team teamIn)
    {
        team = teamIn;
    }

    private Team GetTeam()
    {
        return team;
    }
}
