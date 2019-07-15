using UnityEngine;

[RequireComponent(typeof(Identifier))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Enemy))]
public class Worker : MonoBehaviour
{
	private Enemy enemy => this.gameObject.GetComponent<Enemy>();
	private Mineral miningTarget;
	private WorkerJobStatus jobStatus = WorkerJobStatus.MOVINGTOMINERAL;
	private Base dumpBase;
	private const float MINING_TIME = 1;
	private const int MINE_AMOUNT = 1;
	private Planner planner => FindObjectOfType<Planner>();

	public void SetBase(Base dumpBase)
	{
		this.dumpBase = dumpBase;
		this.miningTarget = dumpBase.GetMiningTarget();
	}

	private void Update()
	{
		switch (this.jobStatus)
		{
			case WorkerJobStatus.MINING:
				if (!this.enemy.IsStalled())
				{
					Dump();
				}
				break;

			case WorkerJobStatus.MOVINGTOMINERAL:
				if (this.enemy.IsPathDone())
				{
					Mine();
				}
				break;

			case WorkerJobStatus.DUMPING:
				if(this.enemy.IsPathDone())
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
		this.enemy.SetTargetChunk(this.miningTarget.GetComponent<Chunk>());
	}

	private void Mine()
	{
		this.jobStatus = WorkerJobStatus.MINING;
		this.enemy.Stall(MINING_TIME);
	}

	private void Dump()
	{
		this.jobStatus = WorkerJobStatus.DUMPING;
		this.enemy.SetTargetChunk(this.dumpBase.GetChunk());
	}
}
