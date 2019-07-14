using System.Collections.Generic;
using UnityEngine;

public class Planner : MonoBehaviour
{
	private const int UNIT_COST = 100;

	private Dictionary<UnitType, float> ratio;
	private Dictionary<UnitType, int> count;
	private int resource = 0;

	private void Spawn()
	{
		UnitType nextUnitType = GetNextUnit();
		Base[] bases = FindObjectsOfType<Base>();
		Base theBase = bases[Random.Range(0, bases.Length)];
		GameObject unit;
		switch (nextUnitType)
		{
			case UnitType.MELEE:
				unit = Instantiate<GameObject>(prefabMelee);
				break;

			case UnitType.RANGED:
				unit = Instantiate<GameObject>(prefabRanged);
				break;

			case UnitType.WORKER:
				unit = Instantiate<GameObject>(prefabWorker);
				Worker worker = unit.GetComponent<Worker>();
				worker.SetDumpBase(theBase);
				break;

			default:
				Debug.LogError("Forgot to add unit type: " + nextUnitType);
				return;
		}

		unit.transform.position = theBase.transform.position;
		
	}

	private void Update()
	{
		if (resource > UNIT_COST)
		{
			Spawn();
		}
	}

	public void AddResource(int amount)
	{
		resource += amount;
	}

	private void Start()
	{
		count = new Dictionary<UnitType, int>();
		count.Add(UnitType.MELEE, 0);
		count.Add(UnitType.RANGED, 0);
		count.Add(UnitType.WORKER, 0);
	}

	public UnitType GetNextUnit()
	{
		UnitType nextType = UnitType.WORKER;
		float max = float.MinValue;
		float total = 0;
		foreach (int amount in count.Values)
		{
			total += amount;
		}
		foreach (UnitType type in count.Keys)
		{
			float idealRatio = ratio[type];
			float currentRatio = count[type] / total;
			float diff = idealRatio - currentRatio;
			if (max < diff)
			{
				max = diff;
				nextType = type;
			}
		}
		this.count[nextType] += 1;
		return nextType;
	}

	public void KillUnit(UnitType type)
	{
		count[type]--;
	}

	public void SetRatio(Dictionary<UnitType, float> ratio)
	{
		this.ratio = ratio;
	}
}