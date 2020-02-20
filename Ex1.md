# Isaque_JogosInteligentes

**▪ Como representar um caminho a ser percorrido por um NPC?**


  - É possível definir um cromossomo com diversos genes(vetores) com as posições sorteadas, por exemplo: Gene[Up,Down,Left,Right] 
  
  Gene1 [5, 0, 2, 1]
  
  Gene2 [5, 0, 2, 1]
  
  Gene3 [5, 0, 2, 1]
  
  Gene4 [5, 0, 2, 1]
  
  
  
  A junção dos genes permite o indivíduo de percorrer um caminho.


**▪ Como definir a população inicial (caminhos/cromossomos)?**


  - Criar vários indivíduos com genes (vetor) setados de forma aleatória(valores), exemplo: [Up,Down,Left,Right,Steps].


**▪ Como avaliar a aptidão de cada cromossomo?**


  - Por serem valores aleatórios, definir que num determinado espaço, o gene que alcançar tal posição estará ápto para prosseguir. 


**▪ Como selecionar os pais para gerar novos seres (filhos)?**


  - Os primeiros que alcançarem tal posição (comentada na questão anterior), serão considerados áptos para trocarem seu conteúdo com outro indivíduo.


**▪ Como fazer o cruzamento dos pais?**


  - Definir certa porcentagem para criar um novo indivíduo com uma característica/alelo (valores da posição do vetor) que receberam de um de seus pais.


**▪ Como um filho pode sofrer mutação?**


  - Definir que há uma porcentagem de ocorrência de um crossover entre os valores dos alelos, permitindo assim, a possibilidade de ocorrência num crossover.
  
  
**▪ Como criar uma nova população/geração?**


  - Após cruzar diversos pares de indivíduos, o resultado é uma população com diferentes indivíduos com características/alelos com valores distintos(por conta de eventuais mutações), porém, recebendo algumas valores semelhantes de seus pais.
  
  
  
//Alelo > Posição do vetor


//Gene > Vetor de X posições (estrutura definida por mim)

