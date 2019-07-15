using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
public class Base : MonoBehaviour
{
	[SerializeField] private Chunk chunk;
	[SerializeField] private Mineral miningTarget;

	public Chunk GetChunk()
	{
		return chunk;
	}

	public Mineral GetMiningTarget()
	{
		return miningTarget;
	}
}
