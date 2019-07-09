using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int workerKills;
    public int meleeKills;
    public int rangedKills;
    public int baseKills;
    public double movementSpeed;
    public float bulletSpeed;
    public int bulletDamage;
    public int currentHealth;
    public int maxHealth;
    public double reloadTime;
    public double currentReloadTime;
    public Rigidbody2D rb2d;
    public PolygonCollider2D col;
    //Chunk currentChunk
    public GameObject bullet;
    public Transform playerShooter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate((float)movementSpeed*Input.GetAxis("Horizontal") * Time.deltaTime, (float)movementSpeed * Input.GetAxis("Vertical") * Time.deltaTime, 0f);

        Vector3 movement = new Vector3(Input.GetAxis("HorizontalLeft") * (float)movementSpeed * Time.deltaTime, Input.GetAxis("VerticalLeft") * (float)movementSpeed * Time.deltaTime);
        rb2d.MovePosition(transform.position + movement);

        float angle = (Mathf.Atan2(Input.GetAxis("VerticalRight"), Input.GetAxis("HorizontalRight")) * Mathf.Rad2Deg) - 90;
        Debug.Log(angle);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
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
            GameObject bulletShot = Instantiate(bullet,transform.position,transform.rotation);
            bulletShot.transform.position = playerShooter.position;
            //bulletShot.transform.rotation = playerShooter.transform.localRotation;
            bulletShot.GetComponent<Rigidbody2D>().AddForce(playerShooter.up * bulletSpeed * Time.deltaTime);
            //rb2d.velocity = playerShooter.up * bulletSpeed;
        }
    }
}
