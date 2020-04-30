using System;
using System.Collections.Generic;

namespace Isaque_Perceptron_T
{
    class Program
    {

        static void Main(string[] args)
        {
            double[] value1 = new double[] { 0.3, 0.7 };
            double[] value2 = new double[] { -0.6, 0.3 };
            double[] value3 = new double[] { -0.1, -0.8 };
            double[] value4 = new double[] { 0.1, -0.45 };

            List<double[]> list = new List<double[]>();
            list.Add(value1);
            list.Add(value2);
            list.Add(value3);
            list.Add(value4);

            double[] vClass = new double[] { 1, 0, 0, 1 };
            double[] vWeights = new double[] { 0.8, -0.5 };

            double bias = 0;


            for (int i = 0; i < list.Count; ++i)
            {
                Training(list[i], vWeights, 0.5, bias, vClass[i]);
            }

            foreach (double weight in vWeights)
            {
                Console.WriteLine(weight);
            }
            Console.ReadKey();
        }

        static void Training(double[] inputs, double[] weights, double learningRate, double bias, double Tclass)
        {
            //Output do neurônio
            double sum = 0;
            double output = 0;
            for (int i = 0; i < inputs.Length; ++i)
            {
                sum += inputs[i] * weights[i];
            }

            sum += bias;

            output = sum >= 0 ? 1 : 0;

            //Verificar se precisa treinar o neurônio
            bool needToTrain = false;

            needToTrain = output == Tclass ? false : true;

            if (needToTrain)
            {
                double error = Tclass - output;
                for (int i = 0; i < weights.Length; ++i)
                {
                    weights[i] = weights[i] + (error * learningRate * inputs[i]);
                }
            }
        }
    }
}
