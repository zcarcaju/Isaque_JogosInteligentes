using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	public GeneticAlgorithm m_geneticAlgorithm;

	public List<int> m_fittestDirections;
	public List<GameObject> m_pathTiles;

	public int[,] mapTile;

	public GameObject p_wallPrefab;
	public GameObject p_exitPrefab;
	public GameObject p_startPrefab;
	public GameObject p_pathPrefab;

	public Vector2 m_startPosition;
	public Vector2 m_endPosition;


	public GameObject PrefabByTile(int tile)
	{
		if (tile == 1) return p_wallPrefab;
		if (tile == 5) return p_startPrefab;
		if (tile == 8) return p_exitPrefab;
		return null;
	}

	void Start()
	{
		mapTile = new int[,] {
	  {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
	  {1,0,1,0,0,0,0,0,1,1,1,0,0,0,1},
	  {8,0,0,0,0,0,0,0,1,1,1,0,0,0,1},
	  {1,0,0,0,1,1,1,0,0,1,0,0,0,0,1},
	  {1,0,0,0,1,1,1,0,0,0,0,0,1,0,1},
	  {1,1,0,0,1,1,1,0,0,0,0,0,1,0,1},
	  {1,0,0,0,0,1,0,0,0,0,1,1,1,0,1},
	  {1,0,1,1,0,0,0,1,0,0,0,0,0,0,5},
	  {1,0,1,1,0,0,0,1,0,0,0,0,0,0,1},
	  {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
	  };
		Populate();

		m_fittestDirections = new List<int>();
		m_pathTiles = new List<GameObject>();

		m_geneticAlgorithm = new GeneticAlgorithm()
		{
			mazeController = this
		};
		
		m_geneticAlgorithm.Run();
	}

	void Update()
	{
		if (m_geneticAlgorithm.m_IsRunning)
		{
			m_geneticAlgorithm.Epoch();
		}
			
		
		RenderFittestChromosomePath();
	}

	public Vector2 Move(Vector2 position, int direction)
	{
		switch (direction)
		{
			case 0: 
				if (position.y - 1 < 0 || mapTile[(int)(position.y - 1), (int)position.x] == 1)
				{
					break;
				}
				else
				{
					position.y -= 1;
				}
				break;
			case 1: 
				if (position.y + 1 >= mapTile.GetLength(0) || mapTile[(int)(position.y + 1), (int)position.x] == 1)
				{
					break;
				}
				else
				{
					position.y += 1;
				}
				break;
			case 2: 
				if (position.x + 1 >= mapTile.GetLength(1) || mapTile[(int)position.y, (int)(position.x + 1)] == 1)
				{
					break;
				}
				else
				{
					position.x += 1;
				}
				break;
			case 3: 
				if (position.x - 1 < 0 || mapTile[(int)position.y, (int)(position.x - 1)] == 1)
				{
					break;
				}
				else
				{
					position.x -= 1;
				}
				break;
		}
		return position;
	}


	public void Populate()
	{
		Debug.Log("Length (0) = " + mapTile.GetLength(0));
		
		Debug.Log("Length (1) = " + mapTile.GetLength(1));

		for (int y = 0; y < mapTile.GetLength(0); y++)
		{
			for (int x = 0; x < mapTile.GetLength(1); x++)
			{
				GameObject prefab = PrefabByTile(mapTile[y, x]);
				if (prefab != null)
				{
					GameObject m_wall = Instantiate(prefab);
					m_wall.transform.position = new Vector3(x, 0, -y);
				}
			}
		}
	}

	public void ClearPathTiles()
	{
		foreach (GameObject pathTile in m_pathTiles)
		{
			Destroy(pathTile);
		}

		m_pathTiles.Clear();
	}

	public void RenderFittestChromosomePath()
	{
		ClearPathTiles();
		
		Genome fittestGenome = m_geneticAlgorithm.genomes[m_geneticAlgorithm.FittestGenome];
		
		List<int> fittestDirections = m_geneticAlgorithm.Decode(fittestGenome.m_Bits);
		
		Vector2 position = m_startPosition;

		foreach (int direction in fittestDirections)
		{
			GameObject pathTile = Instantiate(p_pathPrefab);
			
			position = Move(position, direction);

			pathTile.transform.position = new Vector3(position.x, 0, -position.y);

			m_pathTiles.Add(pathTile);
		}
	}


	public double TestRoute(List<int> directions)
	{
		Vector2 position = m_startPosition;

		for (int directionIndex = 0; directionIndex < directions.Count; directionIndex++)
		{
			int nextDirection = directions[directionIndex];
			
			position = Move(position, nextDirection);
		}

		Vector2 deltaPosition = new Vector2(
			Math.Abs(position.x - m_endPosition.x),
			Math.Abs(position.y - m_endPosition.y));
		
		double result = 1 / (double)(deltaPosition.x + deltaPosition.y + 1);
		
		if (result == 1) 
		{ 
			Debug.Log("TestRoute = " + result + ",(" + position.x + "," + position.y + ")");
		}
		return result;
	}
}
