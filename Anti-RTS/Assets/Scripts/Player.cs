using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int workerKills;
    [SerializeField] int meleeKills;
    [SerializeField] int rangedKills;
    [SerializeField] int baseKills;
    [SerializeField] double movementSpeed;
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] double reloadTime;
    [SerializeField] double currentReloadTime;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] PolygonCollider2D col;
    //Chunk currentChunk
    [SerializeField] GameObject bullet;
    [SerializeField] Transform playerShooter;
    Team team = Team.PLAYER;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate((float)movementSpeed*Input.GetAxis("Horizontal") * Time.deltaTime, (float)movementSpeed * Input.GetAxis("Vertical") * Time.deltaTime, 0f);

        Vector3 movement = new Vector3(Input.GetAxis("HorizontalLeft") * (float)movementSpeed * Time.deltaTime, Input.GetAxis("VerticalLeft") * (float)movementSpeed * Time.deltaTime);
        rb2d.MovePosition(transform.position + movement);


        if (Input.GetAxis("VerticalRight") != 0 && Input.GetAxis("HorizontalRight") != 0)
        {
            float angle = (Mathf.Atan2(Input.GetAxis("VerticalRight"), Input.GetAxis("HorizontalRight")) * Mathf.Rad2Deg) - 90;
            Debug.Log(angle);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        
        //Mathf.atan2 to get the angle of an x,y cos give x, sin gives y.
        //rotation = quaternian.euler of atan2 to handle player rotation.
        //Get camera to follow player
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0f, 0f, 1f);
            //playerShooter.transform.localRotation = transform.rotation;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0f, 0f, -1f);
            //playerShooter.transform.localRotation = transform.rotation;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time > currentReloadTime)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject bulletShot = Instantiate(bullet,transform.position,transform.rotation);
        bulletShot.transform.position = playerShooter.position;
        //bulletShot.transform.rotation = playerShooter.transform.localRotation;
        bulletShot.GetComponent<Rigidbody2D>().AddForce(playerShooter.up * bulletSpeed * Time.deltaTime);
        bulletShot.GetComponent<Bullet>().SetTeam(team);
        bulletShot.GetComponent<Bullet>().SetDamage(bulletDamage);
        currentReloadTime = Time.time + reloadTime;
        //rb2d.velocity = playerShooter.up * bulletSpeed;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        if (currentHealth <= 0)
        {
            Debug.Log("PLAYER DEAD!! YOU LOSE!!");
        }
    }

}
