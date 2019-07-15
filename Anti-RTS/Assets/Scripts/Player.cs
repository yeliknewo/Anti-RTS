using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
	[SerializeField] private int workerKills;
	[SerializeField] private int meleeKills;
	[SerializeField] private int rangedKills;
	[SerializeField] private int baseKills;
	[SerializeField] private double movementSpeed;
	[SerializeField] private float bulletSpeed;
	[SerializeField] private int bulletDamage;
	[SerializeField] private double reloadTime;
	[SerializeField] private double currentReloadTime;
	[SerializeField] private Rigidbody2D rb2d;
	[SerializeField] private PolygonCollider2D col;
	[SerializeField] private Chunk currentChunk;
	[SerializeField] private GameObject prefabBullet;
	[SerializeField] private Transform playerShooter;

	public Chunk GetChunk()
	{
		return this.currentChunk;
	}

	// Update is called once per frame
	private void Update()
	{
		//transform.Translate((float)movementSpeed*Input.GetAxis("Horizontal") * Time.deltaTime, (float)movementSpeed * Input.GetAxis("Vertical") * Time.deltaTime, 0f);

		Vector3 movement = new Vector3(Input.GetAxis("HorizontalLeft") * (float)this.movementSpeed * Time.deltaTime, Input.GetAxis("VerticalLeft") * (float)this.movementSpeed * Time.deltaTime);
		this.rb2d.MovePosition(this.transform.position + movement);


		if (Input.GetAxis("VerticalRight") != 0 && Input.GetAxis("HorizontalRight") != 0)
		{
			float angle = (Mathf.Atan2(Input.GetAxis("VerticalRight"), Input.GetAxis("HorizontalRight")) * Mathf.Rad2Deg) - 90;
			Debug.Log(angle);
			this.transform.rotation = Quaternion.Euler(0, 0, angle);
		}

		//Mathf.atan2 to get the angle of an x,y cos give x, sin gives y.
		//rotation = quaternian.euler of atan2 to handle player rotation.
		//Get camera to follow player
		if (Input.GetKey(KeyCode.Q))
		{
			this.transform.Rotate(0f, 0f, 1f);
		}
		if (Input.GetKey(KeyCode.E))
		{
			this.transform.Rotate(0f, 0f, -1f);
		}
		if (Input.GetButtonDown("Fire1"))
		{
			if (Time.time > this.currentReloadTime)
			{
				Shoot();
			}
		}
	}

	private void Shoot()
	{
		GameObject bulletShot = Instantiate(this.prefabBullet, this.transform.position, this.transform.rotation);
		bulletShot.transform.position = this.playerShooter.position;
		bulletShot.GetComponent<Rigidbody2D>().AddForce(this.playerShooter.up * this.bulletSpeed * Time.deltaTime);
		bulletShot.GetComponent<Bullet>().SetTeam(Team.PLAYER);
		bulletShot.GetComponent<Bullet>().SetDamage(this.bulletDamage);
		this.currentReloadTime = Time.time + this.reloadTime;
	}
}