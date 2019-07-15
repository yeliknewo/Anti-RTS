using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private UnitType unitType;

	private Chunk targetChunk;
	private float stallTime;
	private Node nextNode;
	private Path path;
	private bool dirtyPath;

	public void SetTargetChunk(Chunk targetChunk)
	{
		this.targetChunk = targetChunk;
		this.dirtyPath = true;
	}

	public bool IsPathDone()
	{
		return nextNode == null && !dirtyPath;
	}

	public bool IsStalled()
	{
		return stallTime > Time.time;
	}

	private void Start()
	{
		dirtyPath = true;
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		if (this.nextNode == null || this.stallTime > Time.time)
		{
			return;
		}

		float angle = Vector2.Angle(this.transform.position, this.nextNode.GetChunk().transform.position);
		float movementDistance = Mathf.Min(this.movementSpeed, Vector2.Distance(this.transform.position, this.nextNode.GetChunk().transform.position));
		this.transform.position = this.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * movementDistance;
		if (movementDistance < this.movementSpeed)
		{
			if (this.dirtyPath)
			{
				UpdatePath(this.nextNode.GetChunk());
			}
			else
			{
				this.nextNode = this.path.TakeNextNode();
			}
		}
	}

	private void UpdatePath(Chunk currentChunk)
	{
		this.path = FindObjectOfType<AStar>().FindPath(currentChunk, this.targetChunk);
		this.dirtyPath = false;
	}

	public void Stall(float stallTime)
	{
		this.stallTime = Time.time + stallTime;
	}

}
