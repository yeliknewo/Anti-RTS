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
	[SerializeField] private Chunk currentChunk;
	[SerializeField] private Vector2 overrideTargetValue;
	private Path path;
	private bool dirtyPath;

	public void SetTargetChunk(Chunk targetChunk)
	{
		if (targetChunk == null)
		{
			Debug.LogError("Target Chunk is Null");
		}
		this.targetChunk = targetChunk;
		this.dirtyPath = true;
	}

	public UnitType GetUnitType()
	{
		return this.unitType;
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

	private void Update()
	{
		if (FindObjectOfType<Planner>().IsPaused())
		{
			return;
		}
		Move();
	}

	private void Move()
	{
		if (this.currentChunk == null)
		{
			this.currentChunk = FindObjectOfType<Planner>().GetClosestChunk(this.transform.position);
		}
		else
		{
			this.currentChunk = currentChunk.GetClosestChunk(this.transform.position);
		}
		if (targetChunk == null)
		{
			SetTargetChunk(currentChunk);
		}
		Vector2 target;
		if (GetComponent<Worker>() == null)
		{
			overrideTargetValue = FindObjectOfType<Player>().transform.position;
			target = overrideTargetValue;
		}
		else
		{
			target = targetChunk.transform.position;
		}
		bool wallInWay = false;
		foreach (RaycastHit2D hit in Physics2D.RaycastAll(this.transform.position, target))
		{
			if (hit.collider.gameObject.GetComponent<Wall>() != null)
			{
				wallInWay = true;
				break;
			}
		}
		if (!wallInWay)
		{
			Vector2 diff = target - (Vector2)this.transform.position;
			float angle = Mathf.Atan2(diff.y, diff.x);
			bool snap = false;
			float moveMove = this.movementSpeed * Time.deltaTime;
			float distance = diff.magnitude;
			if (distance < moveMove)
			{
				snap = true;
			}
			if (snap)
			{
				this.transform.position = target;
			}
			else
			{
				this.transform.position = this.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * moveMove;
			}
		}
		else
		{
			if (this.nextChunk == null)
			{
				if (this.dirtyPath)
				{
					UpdatePath();
				}
				else
				{
					return;
				}
			}
			if (this.stallTime > Time.time)
			{
				return;
			}
			target = this.nextChunk.transform.position;
			Vector2 diff = target - (Vector2)this.transform.position;
			float angle = Mathf.Atan2(diff.y, diff.x);
			bool snap = false;
			float moveMove = this.movementSpeed * Time.deltaTime;
			float distance = diff.magnitude;
			if (distance < moveMove)
			{
				snap = true;
			}
			if (snap)
			{
				this.transform.position = target;
				if (this.dirtyPath)
				{
					UpdatePath();
				}
				else
				{
					this.nextChunk = this.path.TakeNextChunk();
				}
			}
			else
			{
				this.transform.position = this.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * moveMove;
			}
		}
	}

	private void UpdatePath()
	{
		this.path = FindObjectOfType<AStar>().FindPath(this.currentChunk, this.targetChunk);
		this.nextChunk = this.path.TakeNextChunk();
		this.dirtyPath = false;
	}

	public void Stall(float stallTime)
	{
		this.stallTime = Time.time + stallTime;
	}

}
