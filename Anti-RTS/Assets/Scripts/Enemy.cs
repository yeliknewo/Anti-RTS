using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private UnitType unitType;
	[SerializeField] private Chunk targetChunk;
	[SerializeField] private float stallTime;
	[SerializeField] private Chunk nextChunk;
	private Path path;
	private bool dirtyPath;

	public void SetTargetChunk(Chunk targetChunk)
	{
		this.targetChunk = targetChunk;
		this.dirtyPath = true;
	}

	public UnitType GetUnitType()
	{
		return unitType;
	}

	public Chunk GetTargetChunk()
	{
		return this.targetChunk;
	}

	public bool IsPathDone()
	{
		return this.nextChunk == null && !this.dirtyPath;
	}

	public bool IsStalled()
	{
		return this.stallTime > Time.time;
	}

	private void Start()
	{
		this.dirtyPath = true;
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		if (this.nextChunk == null)
		{
			if(dirtyPath)
			{
				UpdatePath(targetChunk);
			}
			return;
		}
		if(this.stallTime > Time.time)
		{
			return;
		}

		float angle = Vector2.Angle(this.transform.position, this.nextChunk.transform.position);
		float movementDistance = Mathf.Min(this.movementSpeed, Vector2.Distance(this.transform.position, this.nextChunk.transform.position));
		this.transform.position = this.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * movementDistance;
		if (movementDistance < this.movementSpeed)
		{
			if (this.dirtyPath)
			{
				UpdatePath(this.targetChunk);
			}
			else
			{
				this.nextChunk = this.path.TakeNextChunk();
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
