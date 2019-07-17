using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
public class Base : MonoBehaviour
{
	[SerializeField] private Chunk chunk;
	[SerializeField] private Mineral miningTarget;

	public Chunk GetChunk()
	{
		if(chunk == null)
		{
			chunk = FindObjectOfType<Planner>().GetClosestChunk(this.transform.position);
		}
		return chunk;
	}

	public Mineral GetMiningTarget()
	{
		if(miningTarget == null)
		{
			float minDistance = float.MaxValue;
			foreach(Mineral mineral in FindObjectsOfType<Mineral>())
			{
				float temp = Vector2.Distance(mineral.transform.position, this.transform.position);
				if (temp < minDistance)
				{
					miningTarget = mineral;
					minDistance = temp;
				}
			}
		}
		return miningTarget;
	}
}
