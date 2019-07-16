using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	[SerializeField] private ChunkSize chunkSize;
	[SerializeField] private List<Chunk> macroNeighbors;
	[SerializeField] private List<Chunk> microNeighbors;

	private const float UNIT_DISTANCE = 1.41421356237f; // Sqrt of 2 because units are 1 by 1

	public ChunkSize GetChunkSize()
	{
		return this.chunkSize;
	}

	public List<Chunk> GetMacroNeighbors()
	{
		if (this.macroNeighbors == null)
		{
			this.macroNeighbors = new List<Chunk>();
		}
		return this.macroNeighbors;
	}

	public List<Chunk> GetMicroNeighbors()
	{
		if (this.microNeighbors == null)
		{
			this.microNeighbors = new List<Chunk>();
		}
		return this.microNeighbors;
	}

	public void SetChunkSize(ChunkSize chunkSize)
	{
		this.chunkSize = chunkSize;
	}

	public Chunk GetClosestMacroChunk(Vector2 position)
	{
		if (this.chunkSize == ChunkSize.MACRO)
		{
			return GetClosestChunk(position, this.macroNeighbors);
		}
		else
		{
			return this.macroNeighbors[0].GetClosestMacroChunk(position);
		}

	}

	public Chunk GetClosestMicroChunk(Vector2 position)
	{
		return GetClosestChunk(position, this.microNeighbors);
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

	public static void SetupChunks(float microDistance, float macroDistance)
	{
		List<Chunk> smalls = new List<Chunk>();
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
				if (chunk.GetChunkSize() == ChunkSize.MACRO)
				{
					float distance = Vector2.Distance(chunk.transform.position, possibleNeighbor.transform.position);
					if (distance < macroDistance)
					{
						if (possibleNeighbor.GetChunkSize() == ChunkSize.MACRO)
						{
							chunk.GetMacroNeighbors().Add(possibleNeighbor);
						}
						else if(possibleNeighbor.GetMacroNeighbors().Count == 0)
						{
							possibleNeighbor.GetMacroNeighbors().Add(chunk);
						}
						else if(distance < Vector2.Distance(possibleNeighbor.transform.position, possibleNeighbor.GetMacroNeighbors()[0].transform.position))
						{
							possibleNeighbor.GetMacroNeighbors()[0] = chunk;
						}
					}
				}
				else
				{
					smalls.Add(chunk);
					float distance = Vector2.Distance(chunk.transform.position, possibleNeighbor.transform.position);
					if(distance < microDistance && possibleNeighbor.GetChunkSize() == ChunkSize.MICRO)
					{
						chunk.GetMicroNeighbors().Add(possibleNeighbor);
					}
				}
			}
		}
		foreach(Chunk chunk in smalls)
		{
			chunk.GetMacroNeighbors()[0].GetMicroNeighbors().Add(chunk);
		}
	}
}