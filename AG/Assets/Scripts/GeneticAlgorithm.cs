using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm {
	public List<Genome> genomes;

	public double m_CrossoverRate = 0.3f;
	public double m_MutationRate = 0.004f;
	public int m_PopulationSize = 168;
	
	public int m_ChromosomeLength = 92;
	public int m_GeneLength = 2;

	public double m_BestFitnessScore;
	public double m_TotalFitnessScore;

	public int FittestGenome;
	public int Generation;

	public Controller mazeController;
	public Controller mazeControllerDisplay;

	public bool m_IsRunning;

	public GeneticAlgorithm() {
		m_IsRunning = false;
		genomes = new List<Genome> ();	
	}

	public void UpdateFitnessScores()
	{
		FittestGenome = 0;
		m_BestFitnessScore = 0;
		m_TotalFitnessScore = 0;

		for (int i = 0; i < m_PopulationSize; i++)
		{
			List<int> directions = Decode(genomes[i].m_Bits);

			genomes[i].m_Fitness = mazeController.TestRoute(directions);

			m_TotalFitnessScore += genomes[i].m_Fitness;

			if (genomes[i].m_Fitness > m_BestFitnessScore)
			{
				m_BestFitnessScore = genomes[i].m_Fitness;
				FittestGenome = i;


				if (genomes[i].m_Fitness == 1)
				{
					m_IsRunning = false;
					return;
				}
			}
		}
	}

	public List<int> Decode(List<int> bits)
	{
		List<int> directions = new List<int>();

		for (int geneIndex = 0; geneIndex < bits.Count; geneIndex += m_GeneLength)
		{
			List<int> gene = new List<int>();

			for (int bitIndex = 0; bitIndex < m_GeneLength; bitIndex++)
			{
				gene.Add(bits[geneIndex + bitIndex]);
			}

			directions.Add(GeneToInt(gene));
		}
		return directions;
	}

	public int GeneToInt(List<int> gene)
	{
		int value = 0;
		int multiplier = 1;

		for (int i = gene.Count; i > 0; i--)
		{
			value += gene[i - 1] * multiplier;
			multiplier *= 2;
		}
		return value;
	}

	public Genome RouletteWheelSelection()
	{
		double slice = UnityEngine.Random.value * m_TotalFitnessScore;
		double total = 0;
		int selectedGenome = 0;

		for (int i = 0; i < m_PopulationSize; i++)
		{
			total += genomes[i].m_Fitness;

			if (total > slice)
			{
				selectedGenome = i;
				break;
			}
		}
		return genomes[selectedGenome];
	}

	public void Crossover(List<int> mom, List<int> dad, List<int> baby1, List<int> baby2)
	{
		if (UnityEngine.Random.value > m_CrossoverRate || mom == dad)
		{
			baby1.AddRange(mom);
			baby2.AddRange(dad);

			return;
		}

		System.Random rnd = new System.Random();

		int crossoverPoint = rnd.Next(0, m_ChromosomeLength - 1);

		for (int i = 0; i < crossoverPoint; i++)
		{
			baby1.Add(mom[i]);
			baby2.Add(dad[i]);
		}

		for (int i = crossoverPoint; i < mom.Count; i++)
		{
			baby1.Add(dad[i]);
			baby2.Add(mom[i]);
		}
	}


	public void Mutate(List<int> bits) {
		for (int i = 0; i < bits.Count; i++) {
			
			if (UnityEngine.Random.value < m_MutationRate) {
				
				bits [i] = bits [i] == 0 ? 1 : 0;
			}
		}
	}

	public void CreateStartPopulation() {
		genomes.Clear ();

		for (int i = 0; i < m_PopulationSize; i++) {
			Genome baby = new Genome (m_ChromosomeLength);
			genomes.Add (baby);
		}
	}

	public void Run() {
		CreateStartPopulation ();
		m_IsRunning = true;
	}

	public void Epoch() {
		UpdateFitnessScores ();
		
		int numberOfNewBabies = 0;

		List<Genome> newGenomes = new List<Genome> ();
		
		while (numberOfNewBabies < m_PopulationSize) 
		{

			Genome parent1 = RouletteWheelSelection ();
			Genome parent2 = RouletteWheelSelection ();
			
			Genome child1 = new Genome();
			Genome child2 = new Genome();
			Crossover (parent1.m_Bits, parent2.m_Bits, child1.m_Bits, child2.m_Bits);
			
			Mutate (child1.m_Bits);
			Mutate (child2.m_Bits);
			
			newGenomes.Add (child1);
			newGenomes.Add (child2);

			numberOfNewBabies += 2;
		}

		genomes = newGenomes;
		Generation++;
	}
}
