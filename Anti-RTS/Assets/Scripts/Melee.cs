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
	private Player player; //Caching

	private Player GetPlayer()
	{
		if(player == null)
		{
			player = FindObjectOfType<Player>();
		}
		return player;
	}

	private void Attack()
	{
		currentReloadTime = Time.time + reloadTime;
		player.GetComponent<Health>().TakeDamage(damage);
	}

	private void Update()
	{
		if (reloadTime < Time.time && Vector2.Distance(transform.position, GetPlayer().transform.position) < range)
		{
			Attack();
		}
	}
}
