using UnityEngine;

public class Mineral : MonoBehaviour
{
	[SerializeField] private Chunk chunk;

	public Chunk GetChunk()
	{
		if(chunk == null)
		{
			chunk = FindObjectOfType<Planner>().GetClosestChunk(transform.position);
		}
		return chunk;
	}
}
