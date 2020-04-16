using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Operacoes
{
    public class Item
    {
        private int m_Weight;
        public int Weight { get => m_Weight; set => m_Weight = value; }
        private int m_Value;
        public int Value { get => m_Value; set => m_Value = value; }
        public Item(int weight, int value)
        {
            m_Weight = weight;
            m_Value = value;
        }
    }
}
