using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Enemy))]
public class Ranged : MonoBehaviour
{
	private Enemy enemy => gameObject.GetComponent<Enemy>();
	private Player player;
	[SerializeField] private int damage;
	[SerializeField] private double reloadTime;
	[SerializeField] private double currentReloadTime;
	[SerializeField] private int bulletDamage;
	[SerializeField] private float bulletSpeed;
	[SerializeField] private float range;
	[SerializeField] private GameObject prefabBullet;
	[SerializeField] private Transform enemyShooter;

	private Player GetPlayer()
	{
		if (player == null)
		{
			player = FindObjectOfType<Player>();
		}
		return player;
	}

	private void Attack()
	{
		GameObject bulletShot = Instantiate(this.prefabBullet, this.transform.position, this.transform.rotation);
		bulletShot.transform.position = this.enemyShooter.position;
		bulletShot.GetComponent<Rigidbody2D>().AddForce(this.enemyShooter.up * this.bulletSpeed * Time.deltaTime);
		bulletShot.GetComponent<Bullet>().SetTeam(Team.ENEMY);
		bulletShot.GetComponent<Bullet>().SetDamage(this.bulletDamage);
		this.currentReloadTime = Time.time + this.reloadTime;
	}

	private void Update()
	{
		if (reloadTime < Time.time && Vector2.Distance(transform.position, GetPlayer().transform.position) < range)
		{
			Attack();
		}
	}
}
