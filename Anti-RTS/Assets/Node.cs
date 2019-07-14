using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	private Chunk chunk;
	private float distance;

	public Chunk GetChunk()
	{
		return chunk;
	}

	public float GetDistance()
	{
		return distance;
	}

	public Node(Chunk chunk, float distance)
	{
		this.chunk = chunk;
		this.distance = distance;
	}
}
