using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	[SerializeField] private List<Chunk> neighbors;

	private const float UNIT_DISTANCE = 1.41421356237f; // Sqrt of 2 because units are 1 by 1

	public List<Chunk> GetNeighbors()
	{
		if (this.neighbors == null)
		{
			this.neighbors = new List<Chunk>();
		}
		this.neighbors.RemoveAll((obj => obj == null));
		return this.neighbors;
	}

	public Chunk GetClosestChunk(Vector2 position)
	{
		return GetClosestChunk(position, GetNeighbors());
	}

	private static Chunk GetClosestChunk(Vector2 position, List<Chunk> neighbors)
	{
		Chunk ret = neighbors[0];
		float distance = Vector2.Distance(position, ret.transform.position);
		foreach (Chunk chunk in neighbors)
		{
			float tempDistance = Vector2.Distance(position, chunk.transform.position);
			if (tempDistance < distance)
			{
				distance = tempDistance;
				ret = chunk;
			}
		}
		return ret;
	}

	public static void SetupChunks(float chunkDistance)
	{
		foreach (Chunk chunk in FindObjectsOfType<Chunk>())
		{
			foreach (Wall wall in FindObjectsOfType<Wall>())
			{
				if(wall.GetComponent<BoxCollider2D>().OverlapPoint(chunk.transform.position))
				{
					Destroy(chunk.gameObject);
				}
			}
		}
		foreach (Chunk chunk in FindObjectsOfType<Chunk>())
		{
			foreach (Chunk possibleNeighbor in FindObjectsOfType<Chunk>())
			{
				float distance = Vector2.Distance(chunk.transform.position, possibleNeighbor.transform.position);
				if (distance < chunkDistance * 2)
				{
					chunk.GetNeighbors().Add(possibleNeighbor);
				}
			}
		}
	}
}