using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
	private List<Node> nodes;

	public Path(List<Node> nodes)
	{
		this.nodes = nodes;
	}

	public Node TakeNextNode()
	{
		Node node = nodes[0];
		nodes.RemoveAt(0);
		return node;
	}

}
