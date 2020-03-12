using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knapsack : MonoBehaviour
{
    //Não pode repetir um item
    //Se quiser um item igual, deve conter um item igual no vetor
    /* Encher uma mochila com objetos de pesos e valores diferentes, maximizando o valor da mochila dentro do seu limite de peso.
    O projeto deve permitir que o usuário defina a descrição, peso e valor de cada objeto (mínimo 10, máximo 20) e a capacidade da mochila. */

        
    int[] m_item; //Item = [Posição] // Valor do item =[Peso]
    int [] m_value; //Item = [Posição] //Value = [Valor atribuído ao item]
    int capacity = 100;//Capacidade da mochila


    
    private void Start()
    {
        m_value = new int[10]; //inicializa o vetor com 10 posições (itens)

        //Item e peso
        m_item[0] = 15;
        m_item[1] = 18;
        m_item[2] = 13;
        m_item[3] = 23;
        m_item[4] = 9;
        m_item[5] = 10;
        m_item[6] = 11;
        m_item[7] = 5;
        m_item[8] = 14;
        m_item[9] = 5;

        

        //Item e valor
        m_value[0] = 5;
        m_value[1] = 7;
        m_value[2] = 6;
        m_value[3] = 10;
        m_value[4] = 8;
        m_value[5] = 3;
        m_value[6] = 4;
        m_value[7] = 1;
        m_value[8] = 7;
        m_value[9] = 3;

    }


    void Update()
    {
        //Quanto maior o valor, mais chance de levar o item na mochila (maior fitness)
        //Usuário define os pesos dos itens
        //Usuario define os valores do Valor
        for (int i = m_value[0]; i < capacity; ++i )
            if (m_value[i] < capacity)
            {

            }
    }
}
