using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome {
	public List<int> m_Bits; //genes
	public double m_Fitness; // aptidâo

	public Genome() {
		Initialize ();
	}


	public Genome(int size) {
		Initialize ();

		for (int i = 0; i < size; i++) {
            		System.Random randomNumberGen = new System.Random(DateTime.Now.GetHashCode() *  SystemInfo.processorFrequency.GetHashCode());

					m_Bits.Add(randomNumberGen.Next(0, 2));
		}
	}
		private void Initialize() {
		m_Fitness = 0;
		m_Bits = new List<int> ();
	}
}
