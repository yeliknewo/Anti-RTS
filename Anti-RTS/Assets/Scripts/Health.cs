using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;

	private void Start()
	{
		this.currentHealth = this.maxHealth;
	}

	public void SetMaxHealth(int maxHealth)
	{
		this.maxHealth = maxHealth;
	}

	public int GetMaxHealth()
	{
		return this.maxHealth;
	}

	public int GetCurrentHealth()
	{
		return this.currentHealth;
	}

	public void TakeDamage(int damage)
	{
		this.currentHealth -= damage;
		if (this.currentHealth <= 0)
		{
			Enemy enemy = GetComponent<Enemy>();
			if (enemy != null)
			{
				UnitType unitType = enemy.GetUnitType();
				EnemyType enemyType;
				switch (unitType)
				{
					case UnitType.MELEE:
						enemyType = EnemyType.MELEE;
						break;

					case UnitType.RANGED:
						enemyType = EnemyType.RANGED;
						break;

					case UnitType.WORKER:
						enemyType = EnemyType.WORKER;
						break;

					default:
						Debug.LogError("INvalid Enemy Type in TakeDamage");
						return;
				}
				FindObjectOfType<Player>().DoKill(enemyType);
			}
			else
			{
				Base theBase = GetComponent<Base>();
				if(theBase != null)
				{
					FindObjectOfType<Player>().DoKill(EnemyType.BASE);
				}
			}
			Destroy(this.gameObject);
		}
	}
}