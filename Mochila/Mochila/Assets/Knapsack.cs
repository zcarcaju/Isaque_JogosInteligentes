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
    public int size;

    public InputField m_weights;
    public InputField m_values;

    int[] convertedWeights;
    int[] convertedValues;

    int idealFitness = 100;
    int fitDiscount = 10;

    int m_Generation = 0;

    double totalFit = 0.0;

    bool canRunGA;

    Item[] m_itens; //Item = [Posição] // Valor do item =[Peso] 
    //Item = [Posição] //Value = [Valor atribuído ao item]
    int capacity = 100;//Capacidade da mochila

    Genome<Item> m_Genome;
    Operations<Item> m_Operations;

    private void Start()
    {
        convertedWeights = new int[size];
        convertedValues = new int[size];

        m_itens = new Item[size];

        m_Genome = new Genome<Item>(size);
        m_Operations = new Operations<Item>();
    }


    void Update()
    {
        //Quanto maior o valor, mais chance de levar o item na mochila (maior fitness)
        //Usuário define os pesos dos itens
        //Usuario define os valores do Valor
        if (canRunGA)
        {
            if (m_Operations.GARunning)
            {
                NewEpoch();
            }
        }
    }

    public void ReadInputFields()
    {
        string ifw = m_weights.text;
        string ifv = m_values.text;

        string[] weights = ifw.Split(' ');
        string[] values = ifv.Split(' ');

        for (int i = 0; i < size; ++i)
        {
            convertedWeights[i] = int.Parse(weights[i]);
            convertedValues[i] = int.Parse(values[i]);
            //Debug.Log("Weight: " + convertedWeights[i] + " Value: " + convertedValues[i]);
        }

        for (int i = 0; i < size; ++i)
        {
            m_itens[i] = new Item(convertedWeights[i], convertedValues[i]);
        }

        StartGA();
    }

    void StartGA()
    {
        //Chamar depois que tiver todos os inputs do user
        for (int i = 0; i < 168; ++i)
        {
            m_itens.CopyTo(m_Operations.genomes[i].Bits, 0);
        }
        canRunGA = true;
    }

    void CalcFitness()
    {
        int fittestGenome = 0;
        double bestFitness = 0.0;

        for (int i = 0; i < size; ++i)
        {
            Debug.Log(m_Operations.genomes[i].Bits);
            m_Operations.genomes[i].Fitness = TryChromosome(m_Operations.genomes[i].Bits);

            totalFit += m_Operations.genomes[i].Fitness;

            if (m_Operations.genomes[i].Fitness > bestFitness)
            {
                bestFitness = m_Operations.genomes[i].Fitness;
                fittestGenome = i;

                if (m_Operations.genomes[i].Fitness >= idealFitness)
                {
                    m_Operations.GARunning = false;
                    Debug.Log("Solução encontrada na geração: " + m_Generation);
                    return;
                }
            }
        }
    }

    double TryChromosome(Item[] bits)
    {
        int totalWeight = 0;

        double totalValueScore = 0.0;

        double fitness = 0.0;
        //vamos aplicar um método semelhante à roleta de seleção para selecionar quem vai entrar na mochila
        for (int i = 0; i < bits.Length; ++i)
        {
            Debug.Log(bits[i]);
            //totalValueScore += bits[i].Value;
        }

        double valueSlice = (double)Random.value * totalValueScore;
        double valueTotal = 0.0;
        List<Item> selectedItensKnapsack = new List<Item>();

        while (totalWeight < capacity)
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
                    }
                }
            }
        }


        /*
          41,60,83,64,95,33,28,12,49,77          
         [20,10,2,4,19,45,28,66,80,7] 

         [2,10,19,7,20,28] = 86 <= 100 --> n ideal
         [2,4,10,19,7,20,28] = 90 <= 100 --> ideal
         
         -totalW = 90
         -percorrer o vetor de itens e verificar todos os que não entraram na lista
         -vamos verificar se existe algum peso que, fazendo a conta (capacity - peso) se dá >= totalW
         -se houver, desconta a fitness
         */

        if (totalWeight > capacity) //remover o ultimo elemento sempre que a capacidade estourar
        {
            //selectedItensKnapsack.Remove(selectedItensKnapsack[selectedItensKnapsack.Count - 1]);
            Debug.Log(selectedItensKnapsack.Count);
        }

        for (int i = 0; i < bits.Length; ++i)
        {
            if (!selectedItensKnapsack.Contains(bits[i]))
            {
                int restWeight = capacity - bits[i].Weight;

                if (restWeight >= totalWeight)
                {
                    fitness -= fitDiscount;
                }
            }
        }

        fitness += idealFitness;

        return fitness;

        //próximo passo: inserir os elementos da lista na mochila e fazer descontos no fitness para cada infração
    }

    public void NewEpoch()
    {
        CalcFitness();

        int populationCurrentSize = 0;

        Genome<Item>[] newGenomes = new Genome<Item>[size];

        while (populationCurrentSize < size)
        {
            Genome<Item> parent1 = m_Operations.Tournament();
            Genome<Item> parent2 = m_Operations.Tournament();

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