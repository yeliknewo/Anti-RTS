using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Planner : MonoBehaviour
{
	private const int UNIT_COST = 2;

	private Dictionary<UnitType, float> ratio;
	private Dictionary<UnitType, int> count;
	private int resources = 0;
	private QLearner qLearner;
	private State state;
	private World world;
	private bool gameEnded;

	[SerializeField] private GameObject prefabChunk;
	[SerializeField] private GameObject prefabMelee;
	[SerializeField] private GameObject prefabRanged;
	[SerializeField] private GameObject prefabWorker;
	[SerializeField] private GameObject prefabWall;

	[SerializeField] private Transform topRightMapBorder;
	[SerializeField] private Transform bottomLeftMapBorder;
	[SerializeField] private int startingResources;
	[SerializeField] private string qValuePath;
	[SerializeField] private string statePath;
	[SerializeField] private int eValue;
	[SerializeField] private double alpha;
	[SerializeField] private double gamma;
	[SerializeField] private float chunkDistance;
	[SerializeField] private Chunk uberChunk;
	[SerializeField] private int baseIndex;

	public bool IsPaused()
	{
		bool paused = FindObjectOfType<Player>() == null || FindObjectOfType<Base>() == null;
		Time.timeScale = paused ? 0 : 1;
		return paused;
	}

	private void Spawn()
	{
		UnitType nextUnitType = GetNextUnit();
		Base[] bases = FindObjectsOfType<Base>();
		baseIndex = (baseIndex + 1) % bases.Length;
		Base theBase = bases[baseIndex];
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
		if (gameEnded)
		{
			return;
		}
		if (FindObjectOfType<Player>() == null)
		{
			EndGame(true);
		}
		if (FindObjectOfType<Base>() == null)
		{
			EndGame(false);
		}
		if (FindObjectOfType<Planner>().IsPaused())
		{
			return;
		}
		if(Time.deltaTime < 1f / 50f)
		{
			while (this.resources >= UNIT_COST)
			{
				Spawn();
			}
		}
	}

	public void AddResource(int amount)
	{
		this.resources += amount;
	}

	public Chunk GetClosestChunk(Vector2 position)
	{
		return uberChunk.GetClosestChunk(position);
	}

	private void SpawnWalls()
	{

	}

	private void SpawnChunks()
	{
		for (float y = this.bottomLeftMapBorder.transform.position.y; y < this.topRightMapBorder.transform.position.y; y += this.chunkDistance)
		{
			for (float x = this.bottomLeftMapBorder.transform.position.x; x < this.topRightMapBorder.transform.position.x; x += this.chunkDistance)
			{
				GameObject chunkObj = Instantiate(prefabChunk, transform);
				chunkObj.name = "Chunk(" + x + "," + y + ")";
				chunkObj.transform.position = new Vector2(x, y);

			}
		}
	}

	private void SpawnUberChunk()
	{
		GameObject uberChunkObj = new GameObject();
		uberChunkObj.name = "UberChunk";
		uberChunkObj.transform.position = new Vector2(1000, 1000);
		uberChunkObj.transform.parent = transform;
		uberChunk = uberChunkObj.AddComponent<Chunk>();
		foreach (Chunk chunk in FindObjectsOfType<Chunk>())
		{
			uberChunk.GetNeighbors().Add(chunk);
		}
	}

	private void Awake()
	{
		SpawnChunks();
		Chunk.SetupChunks(this.chunkDistance);
		SpawnUberChunk();
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
		world.UpdateWorld(1, 1);

		if (!File.Exists(this.qValuePath))
		{
			List<int> tableDimensions = new List<int>
			{
				State.PRECISION, State.PRECISION, State.PRECISION, actions.Count
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
				{ UnitType.MELEE, 0.33f },
				{ UnitType.RANGED, 0.33f },
				{ UnitType.WORKER, 0.33f },
			}));
		}

		this.state = Utils.loadFromDisk<State>(this.statePath);

		this.ratio = this.state.GetRatio();

		foreach (UnitType unitType in ratio.Keys)
		{
			Debug.Log(unitType + " ||| " + ratio[unitType]);
		}

		FindObjectOfType<Player>().Setup();
	}

	public void EndGame(bool playerLost)
	{
		gameEnded = true;
		this.state = this.qLearner.RunStep(this.world, this.state, this.eValue, this.alpha, this.gamma, playerLost);
		this.qLearner.saveToDisk(this.qValuePath);
		Utils.saveToDisk(this.statePath, this.state);
		SceneManager.LoadSceneAsync("DemoScene");
	}

	public UnitType GetNextUnit()
	{
		if (count[UnitType.WORKER] < FindObjectsOfType<Base>().Length)
		{
			this.count[UnitType.WORKER] += 1;
			return UnitType.WORKER;
		}
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