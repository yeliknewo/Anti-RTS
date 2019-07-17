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
		return neighbors;
	}

	public Chunk GetClosestChunk(Vector2 position)
	{
		return GetClosestChunk(position, this.neighbors);
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
			foreach(Wall wall in FindObjectsOfType<Wall>())
			{
				if (Vector2.Distance(chunk.transform.position, wall.transform.position) < UNIT_DISTANCE)
				{
					Debug.LogError("Invalid Map", wall);
				}
			}
			foreach (Chunk possibleNeighbor in FindObjectsOfType<Chunk>())
			{
				float distance = Vector2.Distance(chunk.transform.position, possibleNeighbor.transform.position);
				if (distance < chunkDistance)
				{
					chunk.GetNeighbors().Add(possibleNeighbor);
				}
			}
		}
	}
}