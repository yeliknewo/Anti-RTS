using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
	[SerializeField] float smallChunkDistance;

	public Path FindPath(Chunk start, Chunk end)
	{
		Dictionary<Chunk, Node_> openSet = new Dictionary<Chunk, Node_>();
		Dictionary<Chunk, Node_> closedSet = new Dictionary<Chunk, Node_>();
		openSet.Add(start, new Node_(start, null, openSet, end));
		while (openSet.Count != 0)
		{
			Node_ current = null;
			float bestFScore = float.MaxValue;
			foreach (Node_ temp in openSet.Values)
			{
				float fScore = temp.GetFScore();
				if (fScore < bestFScore)
				{
					bestFScore = fScore;
					current = temp;
				}
			}
			Chunk currentChunk = current.GetChunk();

			if (current.GetChunk().Equals(end))
			{
				List<Chunk> chunks = new List<Chunk>();
				Node_ temp = current;
				while (temp.GetFrom() != null)
				{
					chunks.Add(temp.GetChunk());
					temp = closedSet[temp.GetFrom()];
				}
				chunks.Reverse();
				return new Path(chunks);
			}

			openSet.Remove(currentChunk);
			closedSet.Add(current.GetChunk(), current);

			List<Chunk> neighborChunks;

			if (current.GetHScore() < this.smallChunkDistance)
			{
				neighborChunks = current.GetChunk().GetMicroNeighbors();
			}
			else
			{
				neighborChunks = current.GetChunk().GetMacroNeighbors();
			}

			foreach (Chunk neighborChunk in neighborChunks)
			{
				if (!closedSet.ContainsKey(neighborChunk))
				{
					Node_ temp = new Node_(neighborChunk, currentChunk, openSet, end);
					if (!openSet.ContainsKey(neighborChunk))
					{
						openSet.Add(neighborChunk, temp);
					}
					else
					{
						if (temp.GetGScore() < openSet[neighborChunk].GetGScore())
						{
							openSet[neighborChunk] = temp;
						}
					}
				}
			}
		}
		return null;
	}

	private class Node_
	{
		private readonly Chunk chunk;
		private float gScore;
		private float fScore;
		private float hScore;
		private readonly Chunk from;
		private readonly float distanceToFrom;

		public Node_(Chunk chunk, Chunk from, Dictionary<Chunk, Node_> openSet, Chunk end)
		{
			this.chunk = chunk;
			this.from = from;
			if(from == null)
			{
				this.distanceToFrom = 0f;
			}
			else
			{
				this.distanceToFrom = (chunk.transform.position - from.transform.position).magnitude;
			}
			CalculateScores(openSet, end);
		}

		private void CalculateScores(Dictionary<Chunk, Node_> openSet, Chunk end)
		{
			this.gScore = this.distanceToFrom;
			Chunk temp = this.from;
			while (temp != null)
			{
				this.gScore += openSet[temp].GetGScore();
				temp = openSet[temp].GetFrom();
			}
			this.hScore = (this.chunk.transform.position - end.transform.position).magnitude;
			this.fScore = this.gScore + this.hScore;
		}

		public float GetDistanceToFrom()
		{
			return this.distanceToFrom;
		}

		public float GetHScore()
		{
			return this.hScore;
		}

		public float GetGScore()
		{
			return this.gScore;
		}

		public float GetFScore()
		{
			return this.fScore;
		}

		public Chunk GetFrom()
		{
			return this.from;
		}

		public Chunk GetChunk()
		{
			return this.chunk;
		}

	}
}
