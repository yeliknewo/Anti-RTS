using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb2d;
	[SerializeField] private PolygonCollider2D col;
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
		else if (collision.gameObject.GetComponent<Identifier>().IsBullet())
		{
			if ((this.team == Team.ENEMY && collision.gameObject.GetComponent<Bullet>().GetTeam() == Team.PLAYER) || (this.team == Team.PLAYER && collision.gameObject.GetComponent<Bullet>().GetTeam() == Team.ENEMY))
			{
				Destroy(this.gameObject);
			}
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
