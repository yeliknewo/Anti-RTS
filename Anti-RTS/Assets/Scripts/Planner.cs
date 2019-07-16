using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Planner : MonoBehaviour
{
	private const int UNIT_COST = 100;

	private Dictionary<UnitType, float> ratio;
	private Dictionary<UnitType, int> count;
	private int resources = 0;
	private QLearner qLearner;
	private State state;
	private World world;

	[SerializeField] private GameObject prefabMacroChunk;
	[SerializeField] private GameObject prefabMicroChunk;
	[SerializeField] private Transform topRightMapBorder;
	[SerializeField] private Transform bottomLeftMapBorder;
	[SerializeField] private int startingResources;
	[SerializeField] private GameObject prefabMelee;
	[SerializeField] private GameObject prefabRanged;
	[SerializeField] private GameObject prefabWorker;
	[SerializeField] private string qValuePath;
	[SerializeField] private string statePath;
	[SerializeField] private int eValue;
	[SerializeField] private double alpha;
	[SerializeField] private double gamma;
	[SerializeField] private float macroDistance;
	[SerializeField] private float microDistance;
	[SerializeField] private Chunk uberChunk;

	private void Spawn()
	{
		UnitType nextUnitType = GetNextUnit();
		Base[] bases = FindObjectsOfType<Base>();
		Base theBase = bases[Random.Range(0, bases.Length)];
		GameObject unit;
		switch (nextUnitType)
		{
			case UnitType.MELEE:
				unit = Instantiate(this.prefabMelee);
				break;

			case UnitType.RANGED:
				unit = Instantiate(this.prefabRanged);
				break;

			case UnitType.WORKER:
				unit = Instantiate(this.prefabWorker);
				Worker worker = unit.GetComponent<Worker>();
				worker.SetBase(theBase);
				break;

			default:
				Debug.LogError("Forgot to add unit type: " + nextUnitType);
				return;
		}
		unit.transform.position = theBase.transform.position;
		this.resources -= UNIT_COST;
	}

	private void Update()
	{
		if (this.resources > UNIT_COST)
		{
			Spawn();
		}
	}

	public void AddResource(int amount)
	{
		this.resources += amount;
	}

	public Chunk GetClosestMacroChunk(Vector2 position)
	{
		return uberChunk.GetClosestMacroChunk(position);
	}

	public Chunk GetClosestMicroChunk(Vector2 positon)
	{
		return uberChunk.GetClosestMicroChunk(positon);
	}

	private void SpawnChunks()
	{
		GameObject uberChunkObj = new GameObject();
		uberChunkObj.name = "UberChunk";
		uberChunkObj.transform.position = new Vector2(1000, 1000);
		uberChunkObj.transform.parent = transform;
		uberChunk = uberChunkObj.AddComponent<Chunk>();
		for (float y = this.bottomLeftMapBorder.transform.position.y; y < this.topRightMapBorder.transform.position.y; y += this.microDistance * 0.5f)
		{
			for (float x = this.bottomLeftMapBorder.transform.position.x; x < this.topRightMapBorder.transform.position.x; x += this.microDistance * 0.5f)
			{
				GameObject chunkObj = Instantiate(prefabMicroChunk, transform);
				chunkObj.transform.position = new Vector2(x, y);
				uberChunk.GetMicroNeighbors().Add(chunkObj.GetComponent<Chunk>());
			}
		}
		for (float y = this.bottomLeftMapBorder.transform.position.y; y < this.topRightMapBorder.transform.position.y; y += this.macroDistance * 0.5f)
		{
			for (float x = this.bottomLeftMapBorder.transform.position.x; x < this.topRightMapBorder.transform.position.x; x += this.macroDistance * 0.5f)
			{
				GameObject chunkObj = Instantiate(prefabMacroChunk, transform);
				chunkObj.transform.position = new Vector2(x, y);
				uberChunk.GetMacroNeighbors().Add(chunkObj.GetComponent<Chunk>());
			}
		}
	}

	private void Awake()
	{
		SpawnChunks();
		Chunk.SetupChunks(this.microDistance, this.macroDistance);
		AddResource(this.startingResources);
		this.count = new Dictionary<UnitType, int>
		{
			{ UnitType.MELEE, 0 },
			{ UnitType.RANGED, 0 },
			{ UnitType.WORKER, 0 },
		};

		List<Action> actions = new List<Action>
		{
			new Action(UnitType.MELEE, 0.2f),
			new Action(UnitType.RANGED, 0.2f),
			new Action(UnitType.WORKER, 0.2f)
		};

		this.world = new World();

		if (!File.Exists(this.qValuePath))
		{
			List<int> tableDimensions = new List<int>
			{
				State.PRECISION, State.PRECISION, State.PRECISION
			};

			double startingQ = 1.0;

			this.qLearner = new QLearner(actions, tableDimensions, startingQ);
		}
		else
		{
			this.qLearner = new QLearner(actions, this.qValuePath);
		}

		if (!File.Exists(this.statePath))
		{
			Utils.saveToDisk(this.statePath, new State(new Dictionary<UnitType, float>()
			{
				{ UnitType.MELEE, 1.0f },
				{ UnitType.RANGED, 1.0f },
				{ UnitType.WORKER, 1.0f },
			}));
		}

		this.state = Utils.loadFromDisk<State>(this.statePath);

		this.ratio = this.state.GetRatio();

		FindObjectOfType<Player>().Setup();
	}

	public void EndGame(bool playerLost)
	{
		this.state = this.qLearner.RunStep(this.world, this.state, this.eValue, this.alpha, this.gamma, playerLost);
		this.qLearner.saveToDisk(this.qValuePath);
		Utils.saveToDisk(this.statePath, this.state);
	}

	public UnitType GetNextUnit()
	{
		UnitType nextType = UnitType.WORKER;
		float max = float.MinValue;
		float total = 0;
		foreach (int amount in this.count.Values)
		{
			total += amount;
		}
		foreach (UnitType type in this.count.Keys)
		{
			float idealRatio = this.ratio[type];
			float currentRatio = this.count[type] / total;
			float diff = idealRatio - currentRatio;
			if (max < diff)
			{
				max = diff;
				nextType = type;
			}
		}
		this.count[nextType] += 1;
		return nextType;
	}

	public void KillUnit(UnitType type)
	{
		this.count[type]--;
	}
}