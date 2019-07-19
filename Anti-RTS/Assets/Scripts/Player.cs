using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
	[SerializeField] private float currentReloadTime;
	[SerializeField] private GameObject prefabBullet;
	[SerializeField] private Transform playerShooter;
	[SerializeField] private Chunk currentChunk;

	private Dictionary<StatType, Stat> stats;
	private Dictionary<EnemyType, int> kills;

	public Chunk GetCurrentChunk()
	{
		return this.currentChunk;
	}

	public void Setup()
	{
		this.currentChunk = FindObjectOfType<Planner>().GetClosestChunk(this.transform.position);
		this.kills = new Dictionary<EnemyType, int>
			{
				{ EnemyType.BASE, 0 },
				{ EnemyType.MELEE, 0 },
				{ EnemyType.RANGED, 0 },
				{ EnemyType.WORKER, 0 },
			};
		this.stats = new Dictionary<StatType, Stat>
		{
			{
				StatType.MOVEMENT_SPEED,
				new Stat(
					this.kills,
					5.0f,
					new Dictionary<EnemyType, float>
					{
						{ EnemyType.BASE, 1.0f },
						{ EnemyType.MELEE, 1.0f },
						{ EnemyType.RANGED, 1.0f },
						{ EnemyType.WORKER, 1.0f },
					}
				)
			},
			{
				StatType.BULLET_DAMAGE,
				new Stat(
					this.kills,
					1.0f,
					new Dictionary<EnemyType, float>
					{
						{ EnemyType.BASE, 1.0f },
						{ EnemyType.MELEE, 1.0f },
						{ EnemyType.RANGED, 1.0f },
						{ EnemyType.WORKER, 1.0f },
					}
				)
			},
			{
				StatType.BULLET_SPEED,
				new Stat(
					this.kills,
					5.0f,
					new Dictionary<EnemyType, float>
					{
						{ EnemyType.BASE, 1.0f },
						{ EnemyType.MELEE, 1.0f },
						{ EnemyType.RANGED, 1.0f },
						{ EnemyType.WORKER, 1.0f },
					}
				)
			},
			{
				StatType.RELOAD_TIME,
				new Stat(
					this.kills,
					1.0f,
					new Dictionary<EnemyType, float>
					{
						{ EnemyType.BASE, 1.0f },
						{ EnemyType.MELEE, 1.0f },
						{ EnemyType.RANGED, 1.0f },
						{ EnemyType.WORKER, 1.0f },
					}
				)
			},
			{
				StatType.MAX_HEALTH,
				new Stat(
					this.kills,
					1.0f,
					new Dictionary<EnemyType, float>
					{
						{ EnemyType.BASE, 1.0f },
						{ EnemyType.MELEE, 1.0f },
						{ EnemyType.RANGED, 1.0f },
						{ EnemyType.WORKER, 1.0f },
					}
				)
			}
		};
	}

	private Rigidbody2D GetRigidbody2D()
	{
		return GetComponent<Rigidbody2D>();
	}

	private void UpdateStats()
	{
		foreach (Stat stat in this.stats.Values)
		{
			stat.SetDirty();
		}
		GetComponent<Health>().SetMaxHealth(Mathf.RoundToInt(GetStatVal(StatType.MAX_HEALTH)));
	}

	public void DoKill(EnemyType enemyType)
	{
		this.kills[enemyType]++;
		UpdateStats();
	}

	private float GetStatVal(StatType statType)
	{
		return this.stats[statType].GetStat();
	}

	private void Update()
	{
		if (FindObjectOfType<Planner>().IsPaused())
		{
			return;
		}
		this.transform.position = this.transform.position + new Vector3(Input.GetAxis("HorizontalLeft"), Input.GetAxis("VerticalLeft")).normalized * GetStatVal(StatType.MOVEMENT_SPEED) * Time.deltaTime;

		this.currentChunk = this.currentChunk.GetClosestChunk(this.transform.position);

		if (Input.GetAxis("VerticalRight") != 0 && Input.GetAxis("HorizontalRight") != 0)
		{
			this.transform.rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(Input.GetAxis("VerticalRight"), Input.GetAxis("HorizontalRight")) * Mathf.Rad2Deg) - 90);
		}

		if (Input.GetButton("Fire1"))
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
		bulletShot.GetComponent<Rigidbody2D>().AddForce(this.playerShooter.up * GetStatVal(StatType.BULLET_SPEED), ForceMode2D.Impulse);
		bulletShot.GetComponent<Bullet>().SetTeam(Team.PLAYER);
		bulletShot.GetComponent<Bullet>().SetDamage(Mathf.RoundToInt(GetStatVal(StatType.BULLET_DAMAGE)));
		this.currentReloadTime = Time.time + GetStatVal(StatType.RELOAD_TIME);
	}

	private enum StatType
	{
		MOVEMENT_SPEED, BULLET_DAMAGE, RELOAD_TIME, MAX_HEALTH, BULLET_SPEED
	}

	private class Stat
	{
		private readonly float baseVal;
		private readonly Dictionary<EnemyType, float> mults;
		private readonly Dictionary<EnemyType, int> kills;
		private bool dirty;
		private float val;

		public Stat(Dictionary<EnemyType, int> kills, float baseVal, Dictionary<EnemyType, float> mults)
		{
			this.kills = kills;
			this.baseVal = baseVal;
			this.mults = mults;
			SetDirty();
		}

		public void SetDirty()
		{
			this.dirty = true;
		}

		public float GetStat()
		{
			if (this.dirty)
			{
				this.val = this.baseVal;
				foreach (EnemyType enemyType in this.mults.Keys)
				{
					this.val += this.kills[enemyType] * this.mults[enemyType];
				}
			}
			return this.val;
		}
	}
}