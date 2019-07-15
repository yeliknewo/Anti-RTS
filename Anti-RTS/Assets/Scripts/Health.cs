using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;

	private void Start()
	{
		this.currentHealth = this.maxHealth;
	}

	public void TakeDamage(int damage)
	{
		this.currentHealth = this.currentHealth - damage;
		if (this.currentHealth <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
