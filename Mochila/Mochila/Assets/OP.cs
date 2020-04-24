using System;
using System.Collections.Generic;

namespace Operacoes
{
    class OP
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    class Genome<T>
    {

        private T[] m_Bits; //genes
        public T[] Bits { get => m_Bits; set => m_Bits = value; }
        private double m_Fitness; // aptidâo
        public double Fitness { get => m_Fitness; set => m_Fitness = value; }
        public int m_Size;

        public int HowManyInKnapsack { get; set; }
        public int TotalWeight { get; set; }

        public Genome(int size)
        {
            m_Size = size;
            Initialize();
        }

        private void Initialize()
        {
            m_Fitness = 0;
            m_Bits = new T[m_Size];
        }
    }

    class Operations<T>
    {
        public Genome<T>[] genomes;

        public double m_CrossoverRate = 0.3;
        public double m_MutationRate = 0.01;
        public int m_PopulationSize;

        public int m_ChromosomeLength;
        public int m_GeneLength = 1;

        Random m_Random;

        private bool m_GARunning;
        public bool GARunning { get => m_GARunning; set => m_GARunning = value; }

        public Operations(int populationSize, int chromosomeLength)
        {
            m_PopulationSize = populationSize;
            m_ChromosomeLength = chromosomeLength;
            genomes = new Genome<T>[m_PopulationSize];
            m_Random = new Random((int)DateTime.UtcNow.Ticks);
            m_GARunning = true;

            for (int i = 0; i < m_PopulationSize; ++i)
            {
                genomes[i] = new Genome<T>(m_ChromosomeLength);
            }
        }

                                //CROSSOVER

                    //CROSSOVER SINGLE POINT


        public void SingleCrossover<T>(T[] parent1, T[] parent2, T[] child1, T[] child2)
        {
            if (m_Random.NextDouble() > m_CrossoverRate || parent1 == parent2)
            {
                for (int i = 0; i < parent1.Length; ++i)
                {
                    child1[i] = parent1[i];
                    child2[i] = parent2[i];
                }
                return;
            }

            int crossoverPoint = m_Random.Next(0, m_ChromosomeLength - 1);

            for (int i = 0; i < crossoverPoint; ++i)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];

            }

            for (int i = crossoverPoint; i < parent1.Length; ++i)
            {
                child1[i] = parent2[i];
                child2[i] = parent1[i];

            }
        }

        //CROSSOVER DOUBLE POINT

        //CROSSOVER MULTI POINT

        //CROSSOVER SINGLE ARITHMETIC
        //A: 𝛼𝑦𝑘 + 1 − 𝛼 𝑥𝑘
        //B: 𝛼𝑥𝑘 + 1 − 𝛼 𝑦k

        public void SingleArithmetic (double[] parent1, double[] parent2, double[] child1, double[] child2)
        {
            double alphaSort = m_Random.NextDouble(); //Sortear entre 0 e 1
            int sortGenome = m_Random.Next(m_ChromosomeLength - 1); // sortear genoma
            for (int i = 0; i < parent1.Length; ++i)
            {
                if(sortGenome == i)
                {
                    child1[i] = alphaSort * parent2[i] + (1 - alphaSort) * parent1[i];
                    child2[i] = alphaSort * parent1[i] + (1 - alphaSort) * parent2[i];
                }
                else
                {
                    child1[i] = parent1[i];
                    child2[i] = parent2[i];
                }
            }
        }



        //CROSSOVER SIMPLE ARITHMETIC

        public void SimpleArithmetic(double[] parent1, double[] parent2, double[] child1, double[] child2)
        {
            double alphaSort = m_Random.NextDouble(); //Sortear entre 0 e 1
            int sortGenome = m_Random.Next(m_ChromosomeLength - 1); // sortear genoma
            for (int i = sortGenome; i < parent1.Length; ++i)
            {
                child1[i] = alphaSort * parent2[i] + (1 - alphaSort) * parent1[i];
                child2[i] = alphaSort * parent1[i] + (1 - alphaSort) * parent2[i];

            }
        }

        //CROSSOVER WHOLE ARITHMETIC

        public void WholeArithmeticCrossover(double[] parent1, double[] parent2, double[] child1, double[] child2)
        {
            double alphaSort = m_Random.NextDouble(); //Sortear entre 0 e 1
            
            for (int i = 0; i < parent1.Length; ++i)
            {
                child1[i] = alphaSort * parent2[i] + (1 - alphaSort) * parent1[i];
                child2[i] = alphaSort * parent1[i] + (1 - alphaSort) * parent2[i];

            }
        }

        //CROSSOVER PARTIALLY MAPPED

        //CROSSOVER ORDER BASED

        //CROSSOVER POSITION BASED

        public void PositionBasedCrossover<T>(Genome<T>[] parent1, Genome<T>[] parent2, Genome<T>[] child1, Genome<T>[] child2)
        {
            List<Genome<T>> selectedParent1 = new List<Genome<T>>();

            selectedParent1.Add(parent1[6]);
            selectedParent1.Add(parent1[84]);
            selectedParent1.Add(parent1[41]);

            List<Genome<T>> selectedParent2 = new List<Genome<T>>();

            selectedParent2.Add(parent1[19]);
            selectedParent2.Add(parent1[64]);
            selectedParent2.Add(parent1[22]);

            for(int i = 0; i < parent1.Length; ++i)
            {
                if (selectedParent1.Contains(parent1[i]))
                {
                    child1[i] = parent1[i];
                }
                else
                {
                    child1[i] = parent2[i];
                }
            }

            for (int i = 0; i < parent2.Length; ++i)
            {
                if (selectedParent2.Contains(parent2[i]))
                {
                    child2[i] = parent2[i];
                }
                else
                {
                    child2[i] = parent1[i];
                }
            }
        }
        //MUTAÇÂO

        //INVERSÃO DE BITS

        public void InverseBitsMutate<T>(bool[] bits)
        {
            for(int currentBit = 0; currentBit < bits.Length; ++currentBit)
            {
                if(m_Random.NextDouble() < m_MutationRate)
                {
                    bits[currentBit] = !bits[currentBit];
                }
            }
        }


        //INVERSÃO

        public void InverseMutate<T>(bool [] bits)
        {
            int[] selectedPositions = new int [2];

            selectedPositions[0] = m_Random.Next(m_ChromosomeLength - 1);
            selectedPositions[1] = m_Random.Next(m_ChromosomeLength - 1);

            Array.Reverse(bits, selectedPositions[0], selectedPositions[1]);
        }

        //EMBARALHAMENTO

        //DESLOCAMENTO

        //INVERSÃO DESLOCADA

        //SWAP

        public void SwapMutate<T>(T [] bits)
        {
            int[] selectedPositions = new int[2];

            selectedPositions[0] = m_Random.Next(bits.Length);
            selectedPositions[1] = m_Random.Next(bits.Length);

            T temp;

            temp = bits[selectedPositions[0]];
            bits[selectedPositions[0]] = bits[selectedPositions[1]];
            bits[selectedPositions[1]] = temp;
        }

        //INSERÇÃO

        //SELEÇÃO
        //ROLETA

        public Genome<T> RouletteWheelSelection(double totalFitness)
        {
            double fitnessSlice = m_Random.NextDouble() * totalFitness;
            double fitnessTotal = 0.0f;
            int selectedGenome = 0;

            for(int i = 0; i < m_PopulationSize; ++i)
            {
                fitnessTotal += genomes[i].Fitness;
                
                if(fitnessTotal > fitnessSlice)
                {
                    selectedGenome = i;
                    break;
                }
            }
            return genomes[selectedGenome];
        }

        //TORNEIO
        public Genome<T> Tournament()
        {
            double [] tour = new double[5];
            tour[0] = genomes[6].Fitness;
            tour[1] = genomes[23].Fitness;
            tour[2] = genomes[67].Fitness;
            tour[3] = genomes[39].Fitness;
            tour[4] = genomes[87].Fitness;

            Array.Sort(tour);

            for(int i = 0; i < genomes.Length; ++i)
            {
                if (genomes[i].Fitness == tour[4])
                {
                    return genomes[i];
                }
            }

            return null;
        }

        //ELITISMO
        public List <Genome<T>> Elitism()
        {

            double[] genomesFitness = new double[genomes.Length];

            for(int i = 0; i < genomes.Length; ++i)
            {
                genomesFitness[i] = genomes[i].Fitness;

            }
            

            Array.Sort(genomesFitness);

            List<Genome<T>> eliteGenome = new List<Genome<T>>();

            for(int i = 0; i < genomes.Length; ++i)
            {
                if (genomes[i].Fitness == genomesFitness.Length -1)
                {
                    eliteGenome.Add(genomes[i]);
                }

                if (genomes[i].Fitness == genomesFitness.Length - 2)
                {
                    eliteGenome.Add(genomes[i]);
                }

                if (genomes[i].Fitness == genomesFitness.Length - 3)
                {
                    eliteGenome.Add(genomes[i]);
                }

                if (genomes[i].Fitness == genomesFitness.Length - 4)
                {
                    eliteGenome.Add(genomes[i]);
                }
            }

            return eliteGenome;
        }
    }
}
    

    //ROLETA

    //ESTADO ESTACIONÁRIO

    //SELEÇÃO UNIVERSAL ESTOCÁSTICA

    


                

    
    
                





