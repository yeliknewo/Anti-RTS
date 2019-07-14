using System.Collections.Generic;
using UnityEngine;

namespace Tabular1
{
	public class QLearner
	{
		private readonly EndlessArray<double> qValues;
		private readonly List<Action> actions;

		public QLearner(List<Action> actions, List<int> tableDimensions, double startingQ)
		{
			this.actions = actions;
			this.qValues = new EndlessArray<double>(tableDimensions, startingQ, -startingQ);
		}

		public State RunStep(World world, State state, int eValue, double alpha, double gamma, bool wonLastGame)
		{
			int actionIndex;
			if (Random.Range(0, eValue) == 0)
			{
				actionIndex = Random.Range(0, this.actions.Count);
			}
			else
			{
				actionIndex = GetNextActionIndex(state, eValue);
			}
			StateReward stateReward = world.GetNextStateReward(state, this.actions[actionIndex], wonLastGame);
			State nextState = stateReward.GetState();
			int reward = stateReward.GetReward();
			double qValue = GetQValue(state, actionIndex);
			double maxQValue = GetMaxQValue(nextState);
			double newQValue = GetNewQValue(maxQValue, qValue, alpha, gamma, reward);
			UpdateQValues(state, actionIndex, newQValue);
			return nextState;
		}

		public void UpdateQValues(State state, int actionIndex, double newQ)
		{
			List<int> inputs = state.GetInputs();
			inputs.Add(actionIndex);
			this.qValues.SetAt(inputs, newQ);
		}

		public double GetNewQValue(double maxQ, double q, double alpha, double gamma, int reward)
		{
			return q + alpha * (reward + gamma * maxQ - q);
		}

		public double GetMaxQValue(State state)
		{
			List<int> inputs = state.GetInputs();
			double maxQ = double.MinValue;
			for (int actionIndex = 0; actionIndex < this.actions.Count; actionIndex++)
			{
				inputs.Add(actionIndex);
				double q = GetQValue(inputs);
				if (maxQ < q)
				{
					maxQ = q;
				}
				inputs.RemoveAt(inputs.Count - 1);
			}
			return maxQ;
		}

		public double GetMinQValue(State state)
		{
			List<int> inputs = state.GetInputs();
			double minQ = double.MaxValue;
			for (int actionIndex = 0; actionIndex < this.actions.Count; actionIndex++)
			{
				inputs.Add(actionIndex);
				double q = GetQValue(inputs);
				if (minQ > q)
				{
					minQ = q;
				}
				inputs.RemoveAt(inputs.Count - 1);
			}
			return minQ;
		}

		public int GetNextActionIndex(State state, int eValue)
		{
			if (Random.Range(0, eValue) == 0)
			{
				return Random.Range(0, this.actions.Count);
			}
			double maxQ = double.MinValue;
			int maxIndex = 0;
			for (int i = 0; i < this.actions.Count; i++)
			{
				double q = GetQValue(state, i);
				if (maxQ < q)
				{
					maxQ = q;
					maxIndex = i;
				}
			}
			return maxIndex;
		}

		public Action GetNextAction(int actionIndex)
		{
			return this.actions[actionIndex];
		}

		public Action GetNextAction(State state, int eValue)
		{
			return this.actions[GetNextActionIndex(state, eValue)];
		}

		public double GetQValue(State state, int actionIndex)
		{
			List<int> inputs = state.GetInputs();
			inputs.Add(actionIndex);
			return GetQValue(inputs);
		}

		private double GetQValue(List<int> inputs)
		{
			return this.qValues.GetAt(inputs);
		}
	}

	public class Action
	{
		private readonly UnitType unitType;
		private readonly float percentIncrease;

		// The Action is make more of this type of unit
		public Action(UnitType unitType, float percentIncrease)
		{
			this.unitType = unitType;
			this.percentIncrease = percentIncrease;
		}

		public UnitType GetUnitType()
		{
			return this.unitType;
		}

		public float GetPercentIncrease()
		{
			return this.percentIncrease;
		}
	}

	public class State
	{
		private const int PRECISION = 100;

		private readonly Dictionary<UnitType, float> ratio;

		public State(Dictionary<UnitType, float> ratio)
		{
			this.ratio = ratio;
		}

		public Dictionary<UnitType, float> GetRatio()
		{
			return this.ratio;
		}

		public List<int> GetInputs()
		{
			List<int> inputs = new List<int>
			{
				Mathf.RoundToInt(this.ratio[UnitType.WORKER] * PRECISION),
				Mathf.RoundToInt(this.ratio[UnitType.MELEE] * PRECISION),
				Mathf.RoundToInt(this.ratio[UnitType.RANGED] * PRECISION)
			};
			return inputs;
		}
	}

	public class StateReward
	{
		private readonly State state;
		private readonly int reward;

		public StateReward(State state, int reward)
		{
			this.state = state;
			this.reward = reward;
		}

		public State GetState()
		{
			return this.state;
		}

		public int GetReward()
		{
			return this.reward;
		}
	}

	public class World
	{
		private int winReward;
		private int loseReward;

		public World()
		{

		}

		public void UpdateWorld(int winReward, int loseReward)
		{
			this.winReward = winReward;
			this.loseReward = loseReward;
		}

		public StateReward GetNextStateReward(State state, Action action, bool wonLastGame)
		{
			State nextState = GetNextState(state, action);
			int reward = GetReward(wonLastGame);
			return new StateReward(nextState, reward);
		}

		public State GetNextState(State state, Action action)
		{
			Dictionary<UnitType, float> lastRatio = state.GetRatio();
			Dictionary<UnitType, float> nextRatio = new Dictionary<UnitType, float>();
			foreach (UnitType type in lastRatio.Keys)
			{
				nextRatio.Add(type, lastRatio[type]);
			}
			float increase = lastRatio[action.GetUnitType()] * action.GetPercentIncrease();
			foreach (UnitType type in nextRatio.Keys)
			{
				if (type == action.GetUnitType())
				{
					nextRatio[type] = lastRatio[type] + increase;
				}
				else
				{
					nextRatio[type] = lastRatio[type] - increase / 2f;
				}
			}
			return new State(nextRatio);
		}

		private int GetReward(bool wonLastGame)
		{
			if (wonLastGame)
			{
				return this.winReward;
			}
			else
			{
				return this.loseReward;
			}
		}
	}
}

