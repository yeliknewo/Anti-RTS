using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	private ChunkSize chunkSize;
	private List<Chunk> largeNeighbors;
	private List<Chunk> smallNeighbors;

	public ChunkSize GetChunkSize()
	{
		return this.chunkSize;
	}

	public List<Chunk> GetLargeNeighbors()
	{
		return this.largeNeighbors;
	}

	public List<Chunk> GetSmallNeighbors()
	{
		return this.smallNeighbors;
	}

}
