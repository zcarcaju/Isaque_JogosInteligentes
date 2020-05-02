using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Operacoes;

public class Knapsack : MonoBehaviour
{
    //Não pode repetir um item
    //Se quiser um item igual, deve conter um item igual no vetor
    /* Encher uma mochila com objetos de pesos e valores diferentes, maximizando o valor da mochila dentro do seu limite de peso.
    O projeto deve permitir que o usuário defina a descrição, peso e valor de cada objeto (mínimo 10, máximo 20) e a capacidade da mochila. */
    public int m_Size;
    public int m_PopulationSize;

    public InputField m_Weights;
    public InputField m_Values;
    public InputField m_Capacity;
    public Text m_result;

    int[] convertedWeights;
    int[] convertedValues;
    double[] fitnesses;

    int idealFitness = 100;
    int fitDiscount = 10;

    public int m_Generation = 0;

    double totalFit = 0.0;

    bool canRunGA;

    Item[] m_itens; 
    int knapsackCapacity;//Capacidade da mochila

    Genome<Item> m_Genome;
    Operations<Item> m_Operations;

    System.Random random;

    double bestOfBestsFitness;
    Genome<Item> bestOfBestsGenome;
    int bestOfBestsGenomeGeneration;
    int bestOfBestsWeight;

    double[] allBestFitnessGenerations;
    Genome<Item>[] allBestGenomesGenerations;
    int[] allBestWeightGeneration;
    int sumBestValue;
    int sumBestWeight;

    bool finished;

    private void Start()
    {
        random = new System.Random((int)DateTime.UtcNow.Ticks);
        allBestFitnessGenerations = new double[100];
        allBestGenomesGenerations = new Genome<Item>[100];
    }

    void Update()
    {
        if (canRunGA)
        {
            while (m_Generation < 100)
            {
                if (m_Operations.GARunning)
                {
                    NewEpoch();
                }
            }
            finished = true;
        }

        for (int i = 0; i < allBestFitnessGenerations.Length; ++i)
        {
            if (allBestFitnessGenerations[i] > bestOfBestsFitness)
            {
                bestOfBestsFitness = allBestFitnessGenerations[i];
                bestOfBestsGenome = allBestGenomesGenerations[i];
                bestOfBestsGenomeGeneration = i;
            }
        }

        if (finished)
        {
            m_result.text = "Melhor benefício (soma dos valores na mochila) dentro de 100 gerações foi: " + bestOfBestsFitness + " com " + bestOfBestsGenome.HowManyInKnapsack + " itens dentro dela, com peso total de " + bestOfBestsGenome.TotalWeight + " e foi encontrado na geração " + bestOfBestsGenomeGeneration;
        }
    }

    public void ReadInputFields()
    {
        string ifw = m_Weights.text;
        string ifv = m_Values.text;
        string ifc = m_Capacity.text;

        string[] weights = ifw.Split();
        string[] values = ifv.Split();

        bool canStartGA = false;

        knapsackCapacity = int.Parse(ifc);
        Debug.Log(knapsackCapacity);

        m_Size = values.Length;

        InitializeArrays(m_Size);

        if (weights.Length < 10 && values.Length < 10)
        {
            Debug.LogWarning("Por favor, insira no mínimo 10 valores de peso ou de valor");
        }
        else if (weights.Length > 20 && values.Length > 20)
        {
            Debug.LogWarning("Por favor, insira menos do que 20 valores de peso ou de valor");
        }
        else
        {
            canStartGA = true;
        }

        if (canStartGA)
        {
            for (int i = 0; i < m_Size; ++i)
            {
                convertedWeights[i] = int.Parse(weights[i]);
                convertedValues[i] = int.Parse(values[i]);
                m_itens[i] = new Item(convertedWeights[i], convertedValues[i]);
            }

            StartGA();
        }
    }

    void InitializeArrays(int size)
    {
        convertedWeights = new int[size];
        convertedValues = new int[size];
        fitnesses = new double[m_PopulationSize];

        m_itens = new Item[size];

        m_Genome = new Genome<Item>(size);
        m_Operations = new Operations<Item>(m_PopulationSize, size);
        canRunGA = true;
    }

    void StartGA()
    {
        for (int i = 0; i < m_Operations.m_PopulationSize; ++i)
        {
            for (int j = 0; j < m_Operations.genomes[i].Bits.Length; ++j)
            {
                m_Operations.genomes[i].Bits[j] = m_itens[j];
            }
        }
        canRunGA = true;
    }

    void CalcFitness()
    {
        int fittestGenome = 0;
        double bestFitness = 0.0;

        for (int i = 0; i < m_Size; ++i)
        {
            Debug.Log(m_Operations.genomes[i].Bits);
            m_Operations.genomes[i].Fitness = TryChromosome(m_Operations.genomes[i].Bits, m_Operations.genomes[i]);

            totalFit += m_Operations.genomes[i].Fitness;

            if (m_Operations.genomes[i].Fitness > bestFitness)
            {
                bestFitness = m_Operations.genomes[i].Fitness;
                fittestGenome = i;
            }
        }

        allBestFitnessGenerations[m_Generation] = bestFitness;
        allBestGenomesGenerations[m_Generation] = m_Operations.genomes[fittestGenome];
    }

    double TryChromosome(Item[] bits, Genome<Item> currentGenome)
    {
        int totalWeight = 0;

        double totalValueScore = 0.0;

        double fitness = 0.0;
        //vamos aplicar um método semelhante à roleta de seleção para selecionar quem vai entrar na mochila
        for (int i = 0; i < bits.Length; ++i)
        {
            totalValueScore += bits[i].Value;
        }

        double valueSlice = random.NextDouble() * totalValueScore;
        double valueTotal = 0.0;
        List<Item> selectedItensKnapsack = new List<Item>();

        while (totalWeight < knapsackCapacity)
        {

            for (int i = 0; i < bits.Length; ++i)
            {
                valueTotal += bits[i].Value;

                if (valueTotal > valueSlice)
                {
                    if (!selectedItensKnapsack.Contains(bits[i]))
                    {
                        selectedItensKnapsack.Add(bits[i]);
                        totalWeight += bits[i].Weight;
                        if (totalWeight > knapsackCapacity)
                        {
                            break;
                        }
                    }
                }
                if (selectedItensKnapsack.Count >= m_Size)
                {
                    break;
                }
            }
        }

        if (totalWeight > knapsackCapacity) //remover o ultimo elemento sempre que a capacidade estourar
        {
            totalWeight -= selectedItensKnapsack[selectedItensKnapsack.Count - 1].Weight;
            selectedItensKnapsack.Remove(selectedItensKnapsack[selectedItensKnapsack.Count - 1]);
        }

        for (int i = 0; i < selectedItensKnapsack.Count; ++i)
        {
            fitness += selectedItensKnapsack[i].Value;
        }

        currentGenome.TotalWeight = totalWeight;
        currentGenome.HowManyInKnapsack = selectedItensKnapsack.Count;

        return fitness;
    }

    public void NewEpoch()
    {
        CalcFitness();

        int populationCurrentSize = 0;

        Genome<Item>[] newGenomes = new Genome<Item>[m_PopulationSize];

        while (populationCurrentSize < m_PopulationSize)
        {
            Genome<Item> parent1 = m_Operations.RouletteWheelSelection(totalFit);
            Genome<Item> parent2 = m_Operations.RouletteWheelSelection(totalFit);

            Genome<Item> child1 = new Genome<Item>(parent1.Bits.Length);
            Genome<Item> child2 = new Genome<Item>(parent2.Bits.Length);

            m_Operations.SingleCrossover<Item>(parent1.Bits, parent2.Bits, child1.Bits, child2.Bits);

            m_Operations.SwapMutate(child1.Bits);
            m_Operations.SwapMutate(child2.Bits);

            newGenomes[populationCurrentSize] = child1;
            newGenomes[populationCurrentSize + 1] = child2;

            populationCurrentSize += 2;
        }

        for (int i = 0; i < m_Operations.genomes.Length; ++i)
        {
            m_Operations.genomes[i] = newGenomes[i];
        }

        ++m_Generation;
    }
}