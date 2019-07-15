using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{ 
    Enemy enemy
    {
        get {
            return this.gameObject.GetComponent<Enemy>();
        }
    } 
    
    int damage;
    double reloadTime;
    double currentReloadTime;
    int bulletDamage;
    float bulletSpeed;
    GameObject bullet;
    GameObject ranged;
    Transform enemyShooter;
    
    void Attack()
    {
        GameObject bulletShot = Instantiate(bullet, transform.position, transform.rotation);
        bulletShot.transform.position = enemyShooter.position;
        bulletShot.GetComponent<Rigidbody2D>().AddForce(enemyShooter.up * bulletSpeed * Time.deltaTime);
        bulletShot.GetComponent<Bullet>().SetTeam(Enemy.team);
        bulletShot.GetComponent<Bullet>().SetDamage(bulletDamage);
        currentReloadTime = Time.time + reloadTime;
    }
    

    void Update()
    {
        if (currentReloadTime > 0)
        {
            enemy.Stall();
            currentReloadTime = 0;
        }
        else
        {
            enemy.Move();
        }
        if(enemy.currentHealth = 0)
        {
            Destroy(ranged);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Attack();
        }
    }
}
