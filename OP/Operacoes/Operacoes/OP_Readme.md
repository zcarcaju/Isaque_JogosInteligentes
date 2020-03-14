1. Clonar repo;
2. Importar arquivo OP.cs;
3. Declarar variável do tipo OP<T> e instanciar um objeto;
4. A classe OP possui duas classes com a implementação do Genoma Generico e das operações. Essas operações incluem métodos de seleção, crossover e mutação;
5. Na //CROSSOVER SINGLE POINT, ele sorteia uma posição aleatória nos vetores dos parents e copia a posição nos childs. O que não for a posição sorteada é copiada inversamente;
6. Na //CROSSOVER SINGLE ARITHMETIC, ele sorteia uma posição aleatória nos vetores dos parents e aplica a fórmula aritmética na mesma posição dos vetores dos childs;
7. No //CROSSOVER SIMPLE ARITHMETIC , ele sorteia uma posição aleatória nos vetores dos parents e aplica a fórmula aritmética para todas as posições nas childs a partir daquele ponto;
8. No //CROSSOVER WHOLE ARITHMETIC, ele aplica a fórmula aritmética para todas as posições dos vetores dos childs;
9. No //CROSSOVER POSITION BASED, ele sorteia 3 posições aleatórias do parent1 e parent2, copiando para os filhos respectivamente, mantendo as mesmas posições;
10. Na mutação //INVERSÃO DE BITS, ele inverte os bits de todo o vetor;
11. Na mutação //INVERSÃO, ele seleciona dois pontos aleatórios no vetor e inverte todas as posições entre esses dois pontos;
12. Na mutação //SWAP, ele seleciona dois pontos e troca seus valores;
13. Na seleção //ROLETA, ele seleciona um indivíduo qualquer, porém, aquele que possuir maior fitness, terá maior chance de ser selecionado
14. Na seleção //TORNEIO, ele seleciona 5 genomas aleatórios e retorna o que obtiver a melhor fitness;
15. Na seleção //ELITISMO, ele retorna um vetor com os 4 indivíduos com melhor fitness;
