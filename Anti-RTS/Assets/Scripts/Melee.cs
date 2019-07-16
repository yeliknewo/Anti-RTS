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

	private const float MACRO_CHUNK_RANGE = 10.0f;

	public void Setup()
	{
		TargetPlayer();
	}

	private void Attack()
	{
		this.currentReloadTime = Time.time + this.reloadTime;
		this.player.GetComponent<Health>().TakeDamage(this.damage);
	}

	private void TargetPlayer()
	{
		this.enemy.SetTargetChunk(this.player.GetCurrentMicroChunk());
	}

	private void Update()
	{
		if (Vector2.Distance(this.player.transform.position, this.transform.position) > MACRO_CHUNK_RANGE)
		{
			if (this.player.GetCurrentMacroChunk() != this.enemy.GetTargetChunk().GetMacroNeighbors()[0])
			{
				TargetPlayer();
			}
		}
		else
		{
			if(this.player.GetCurrentMicroChunk() != this.enemy.GetTargetChunk())
			{
				TargetPlayer();
			}
		}

		if (this.reloadTime < Time.time && Vector2.Distance(this.transform.position, this.player.transform.position) < this.range)
		{
			Attack();
		}
	}
}
