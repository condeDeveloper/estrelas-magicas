# Estrelas magicas

A estrela de n pontas e desenhada ligando cada ponta a segunda ponta
seguinte. As n retas se cruzam por dentro em n miolos, e cada reta passa por
quatro casas: duas pontas e dois miolos. Sao 2n casas, e cada uma esta em
duas retas. A estrela e **magica** quando os numeros de 1 a 2n, um por casa,
fazem todas as retas somarem o mesmo; como cada numero conta duas vezes, essa
soma e `4n + 2`.

```
a primeira estrela de 6 pontas: cada reta e ponta, miolo, miolo, ponta
   reta 0:  1 +  2 + 11 + 12 = 26
   reta 1:  5 + 11 +  4 +  6 = 26
   reta 2: 12 +  4 +  3 +  7 = 26
   reta 3:  6 +  3 +  8 +  9 = 26
   reta 4:  7 +  8 + 10 +  1 = 26
   reta 5:  9 + 10 +  2 +  5 = 26
```

Este repositorio conta todas as estrelas de 5 a 10 pontas, por dois caminhos
que nao se falam ate 8 pontas, e pergunta quais dez numeros servem para a
estrela de 5 pontas, ja que os de 1 a 10 nao servem.

## Como rodar

```bash
dotnet test testes/EstrelasMagicas.Testes/EstrelasMagicas.Testes.csproj -c Release --filter "Category!=Pesado"
dotnet test testes/EstrelasMagicas.Testes/EstrelasMagicas.Testes.csproj -c Release --filter "Category=Pesado"
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- tabela
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- classes
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- pentagrama
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- primeira
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- exemplo
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- juiz8
```

O teste pesado (o juiz na estrela de 8 pontas) leva uns nove minutos e roda
num job proprio do CI.

## A busca e o juiz

- **A busca** poe um numero por vez. Se uma reta tem uma casa vazia so, o
  valor dela e forcado; senao a reta com menos casas vazias da a proxima
  casa, e cada numero so entra se as duas retas da casa ainda puderem fechar
  com os menores e com os maiores numeros que sobram.
- **O juiz** nao conhece as retas da estrela. Ele varre TODAS as maneiras de
  por numeros nos n miolos; com os miolos postos, cada reta diz quanto somam
  as suas duas pontas, `P(i) + P(i+2) = M - X(i-1) - X(i)`. Andando de 2 em 2
  pela roda das pontas, a primeira ponta de cada ciclo e livre e as outras
  saem dela; o ciclo tem que fechar na conta e so usar numeros livres. No 8,
  sao 518.918.400 maneiras de por os miolos, e por isso o juiz para ali.

```
pontas   numeros   constante   estrelas (busca)   juiz
5        1 a 10    22          0                  0
6        1 a 12    26          960                960
7        1 a 14    30          1008               1008
8        1 a 16    34          1792               1792 (teste pesado)
9        1 a 18    38          54252              -
10       1 a 20    42          217640             -
```

Ate 7 pontas o juiz e a busca acham as mesmas estrelas, uma por uma, e nao
so o mesmo numero.

## O que isto mediu

**Com os numeros de 1 a 10 nao existe estrela magica de 5 pontas.** A
constante seria 22, e a busca esgota a arvore em 11.979 nos sem fechar
nenhuma.

**Com dez numeros de 1 a 12, so dois conjuntos servem: sem o 7 e o 11, ou sem
o 2 e o 6.** Os dois sao o complemento um do outro (`13 - 7 = 6`,
`13 - 11 = 2`), e cada um da 120 estrelas. Tirando os dez numeros de 1 ate um
maximo maior, os conjuntos que servem aumentam, e todo conjunto que serve da
exatamente 120 estrelas ate o maximo 15. No 16 aparecem os dois primeiros com
240:

```
maior   conjuntos   constante inteira   com estrela   estrelas   estrelas por conjunto
10      1           1                   0             0
11      11          3                   0             0
12      66          14                  2             240        120 x 2
13      286         58                  12            1440       120 x 12
14      1001        201                 34            4080       120 x 34
15      3003        603                 82            9840       120 x 82
16      8008        1604                188           22800      120 x 186  240 x 2
```

Os dois de 240 deixam de fora 3 5 7 12 14 15 e 2 3 5 10 12 14, de novo um o
complemento do outro. O juiz confere a tabela inteira ate o maximo 13.

**Nenhuma estrela magica de 5 a 10 pontas e simetrica: as classes sao
exatamente o total dividido por 2n.** Girar e espelhar a estrela da 2n
posicoes, e todas as estrelas achadas caem em classes cheias:

```
pontas   estrelas   classes por giro e espelho   juntando o complemento   que sao o proprio complemento
6        960        80                           40                       0
7        1008       72                           36                       0
8        1792       112                          56                       0
9        54252      3014                         1507                     0
10       217640     10882                        5441                     0
```

**E nenhuma e o proprio complemento.** Trocar cada numero v por `2n + 1 - v`
leva estrela magica em estrela magica; em nenhum dos tamanhos isso devolveu a
mesma classe, e o complemento junta as classes exatamente de duas em duas.

**Achar uma estrela custa quase nada: 171 nos na de 10 pontas, contra
293.754.239 para contar todas.** No 8 a primeira demora mais (3.168 nos) que
no 9 e no 10:

```
pontas   nos para contar todas   nos ate a primeira
6        93217                   17
7        646583                  25
8        4734139                 3168
9        36473249                213
10       293754239               171
```

## O que as medidas me corrigiram

**Rodei a tabela inteira com a saida presa num `| tail`.** O juiz da estrela
de 8 pontas levou 535 segundos, e por dez minutos eu nao sabia se era a busca
do 10 ou o juiz do 8 que estava parado. Medindo cada tamanho com o tempo do
lado, o juiz do 8 saiu da tabela comum e foi para o teste pesado.

**Escrevi de cabeca um conjunto com constante quebrada para o teste:** os
numeros de 2 a 11. Eles somam 65, e `2 x 65 = 130` divide por 5: a constante e
inteira, 26, e o teste que esperava a recusa falhou. Troquei por 1 a 9 com o
11, que somam 56.

**A tabela que eu tinha planejado nao media nada.** Perguntei que numeros
nunca vao numa ponta; em todos os tamanhos, todo numero aparece em ponta e em
miolo. A tabela saiu e deu lugar a dos conjuntos de dez numeros.

## Os arquivos

```
src/EstrelasMagicas/Estrela.cs    as pontas, os miolos, as retas e as 2n simetrias
src/EstrelasMagicas/Busca.cs      valor forcado na reta com uma vazia, e a poda dos extremos
src/EstrelasMagicas/Juiz.cs       os miolos varridos e as pontas saindo da roda
src/EstrelasMagicas/Medidas.cs    as tabelas deste README, guardadas
ferramentas/Medidor/              o executavel que imprime as tabelas
testes/                           27 testes, 1 deles pesado
```

27 testes, 1 deles pesado.
