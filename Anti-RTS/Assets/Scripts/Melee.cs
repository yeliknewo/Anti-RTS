using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Enemy))]
public class Melee : MonoBehaviour
{
	private Enemy enemy => this.gameObject.GetComponent<Enemy>();

	[SerializeField] private int damage;
	[SerializeField] private float range;
	[SerializeField] private double reloadTime;
	[SerializeField] private double currentReloadTime;
	private Player _player; //Caching
	private Player player
	{
		get
		{
			if(_player == null)
			{
				_player = FindObjectOfType<Player>();
			}
			return _player;
		}
	}

	private void Attack()
	{
		this.currentReloadTime = Time.time + this.reloadTime;
		this.player.GetComponent<Health>().TakeDamage(this.damage);
	}

	public void TargetPlayer()
	{
		this.enemy.SetTargetChunk(this.player.GetCurrentChunk());
	}

	private void Update()
	{
		if (FindObjectOfType<Planner>().IsPaused())
		{
			return;
		}
		if (this.player.GetCurrentChunk() != this.enemy.GetTargetChunk()  && Time.deltaTime < 1f / 50f)
		{
			TargetPlayer();
		}

		if (this.reloadTime < Time.time && Vector2.Distance(this.transform.position, this.player.transform.position) < this.range)
		{
			Attack();
		}
	}
}
