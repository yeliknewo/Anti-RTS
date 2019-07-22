using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Enemy))]
public class Worker : MonoBehaviour
{
	private Planner planner => FindObjectOfType<Planner>();
	private Enemy enemy => this.gameObject.GetComponent<Enemy>();

	[SerializeField] private Mineral miningTarget;
	[SerializeField] private WorkerJobStatus jobStatus = WorkerJobStatus.MOVINGTOMINERAL;
	[SerializeField] private Base dumpBase;

	private const float MINING_TIME = 1;
	private const int MINE_AMOUNT = 1;
	private const float MINING_DISTANCE = 0.1f;

	public void SetBase(Base dumpBase)
	{
		this.dumpBase = dumpBase;
		this.miningTarget = dumpBase.GetMiningTarget();
	}

	private void Start()
	{
		MoveToMineral();
	}

	private void Update()
	{
		if (FindObjectOfType<Planner>().IsPaused())
		{
			return;
		}
		switch (this.jobStatus)
		{
			case WorkerJobStatus.MINING:
				if (!this.enemy.IsStalled())
				{
					Dump();
				}
				break;

			case WorkerJobStatus.MOVINGTOMINERAL:
				if (Vector2.Distance(transform.position, miningTarget.transform.position) < MINING_DISTANCE)
				{
					Mine();
				}
				break;

			case WorkerJobStatus.DUMPING:
				if(Vector2.Distance(transform.position, dumpBase.transform.position) < MINING_DISTANCE)
				{
					MoveToMineral();
					planner.AddResource(MINE_AMOUNT);
				}
				break;
		}
	}

	private void MoveToMineral()
	{
		this.jobStatus = WorkerJobStatus.MOVINGTOMINERAL;
		this.enemy.SetTargetChunk(this.miningTarget.GetChunk());
	}

	private void Mine()
	{
		this.jobStatus = WorkerJobStatus.MINING;
		this.enemy.Stall(Random.Range(MINING_TIME, MINING_TIME * 2));
	}

	private void Dump()
	{
		this.jobStatus = WorkerJobStatus.DUMPING;
		this.enemy.SetTargetChunk(this.dumpBase.GetChunk());
	}
}
