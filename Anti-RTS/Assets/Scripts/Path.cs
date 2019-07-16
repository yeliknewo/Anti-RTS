using System.Collections.Generic;

public class Path
{
	private readonly List<Chunk> chunks;

	public Path(List<Chunk> chunks)
	{
		this.chunks = chunks;
	}

	public Chunk TakeNextChunk()
	{
		Chunk chunk = this.chunks[0];
		this.chunks.RemoveAt(0);
		return chunk;
	}

}
