using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Enemy))]
public class Ranged : MonoBehaviour
{
	private Enemy enemy => this.gameObject.GetComponent<Enemy>();
	[SerializeField] private double reloadTime;
	[SerializeField] private double currentReloadTime;
	[SerializeField] private int bulletDamage;
	[SerializeField] private float bulletSpeed;
	[SerializeField] private float range;
	[SerializeField] private GameObject prefabBullet;
	[SerializeField] private Transform enemyShooter;

	private Player _player; //Caching
	private Player player
	{
		get
		{
			if (this._player == null)
			{
				this._player = FindObjectOfType<Player>();
			}
			return this._player;
		}
	}

	public void TargetPlayer()
	{
		this.enemy.SetTargetChunk(this.player.GetCurrentChunk());
	}

	private void Attack()
	{
		GameObject bulletShot = Instantiate(this.prefabBullet, this.transform.position, this.transform.rotation);
		bulletShot.transform.position = this.enemyShooter.position;
		bulletShot.GetComponent<Rigidbody2D>().AddForce(this.enemyShooter.up * this.bulletSpeed, ForceMode2D.Impulse);
		bulletShot.GetComponent<Bullet>().SetTeam(Team.ENEMY);
		bulletShot.GetComponent<Bullet>().SetDamage(this.bulletDamage);
		this.currentReloadTime = Time.time + this.reloadTime;
	}

	private void Update()
	{
		if (FindObjectOfType<Planner>().IsPaused())
		{
			return;
		}
		if (this.player.GetCurrentChunk() != this.enemy.GetTargetChunk() && Time.deltaTime < 1f / 50f)
		{
			TargetPlayer();
		}
		Vector2 diff = transform.position - player.transform.position;
		float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 90;
		transform.rotation = Quaternion.Euler(0, 0, angle);
		if (this.currentReloadTime < Time.time && Vector2.Distance(this.transform.position, this.player.transform.position) < this.range)
		{
			Attack();
		}
	}
}