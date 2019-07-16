using UnityEngine;

[RequireComponent(typeof(Identifier))]
public class Bullet : MonoBehaviour
{
	[SerializeField] private int damage;
	private Team team;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.GetComponent<Identifier>().IsBase())
		{
			collision.gameObject.GetComponent<Health>().TakeDamage(this.damage);
			Destroy(this.gameObject);
		}
		else if (collision.gameObject.GetComponent<Identifier>().IsWall())
		{
			Destroy(this.gameObject);
		}
		else if (collision.gameObject.GetComponent<Identifier>().IsEnemy() && this.team == Team.PLAYER)
		{
			collision.gameObject.GetComponent<Health>().TakeDamage(this.damage);
			Destroy(this.gameObject);
		}
		else if (collision.gameObject.GetComponent<Identifier>().IsPlayer() && this.team == Team.ENEMY)
		{
			collision.gameObject.GetComponent<Health>().TakeDamage(this.damage);
			Destroy(this.gameObject);
		}
		else if (collision.gameObject.GetComponent<Identifier>().IsBullet() && this.team != collision.GetComponent<Bullet>().GetTeam())
		{
			Destroy(this.gameObject);
		}
	}

	public void SetDamage(int damageIn)
	{
		this.damage = damageIn;
	}

	public void SetTeam(Team teamIn)
	{
		this.team = teamIn;
	}

	private Team GetTeam()
	{
		return this.team;
	}
}
