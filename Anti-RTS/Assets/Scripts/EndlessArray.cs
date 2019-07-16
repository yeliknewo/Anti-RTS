using System;
using System.Collections.Generic;

[Serializable]
public class EndlessArray
{
	private Dictionary<int, double> data;
	private List<int> dimSizes;
	private double startingVal;
	private double minVal;

	public EndlessArray(List<int> dimSizes, double startingVal, double minVal)
	{
		this.dimSizes = dimSizes;
		this.startingVal = startingVal;
		this.minVal = minVal;
		this.data = new Dictionary<int, double>();
	}

	public double GetAt(List<int> indices)
	{
		int index = GetIndex(indices);
		if (index == -1)
		{
			return this.minVal;
		}
		if (this.data.ContainsKey(index))
		{
			return this.data[index];
		}
		else
		{
			return this.startingVal;
		}
	}

	public void SetAt(List<int> indices, double val)
	{
		int index = GetIndex(indices);
		if (index == -1)
		{
			return;
		}
		if (this.data.ContainsKey(index))
		{
			this.data[index] = val;
		}
		else
		{
			this.data.Add(index, val);
		}
	}

	private int GetIndex(List<int> indices)
	{
		int index = 0;
		int pow = 1;
		for (int i = indices.Count - 1; i >= 0; i--)
		{
			if (indices[i] >= this.dimSizes[i] || indices[i] < 0)
			{
				return -1;
			}
			index += indices[i] * pow;
			pow *= this.dimSizes[i];
		}
		return index;
	}
}
