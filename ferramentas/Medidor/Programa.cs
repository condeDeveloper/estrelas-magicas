using System.Diagnostics;
using Conde.EstrelasMagicas;

if (args.Length == 0)
{
    Console.WriteLine("medidor <tabela|classes|pentagrama|primeira|exemplo|juiz8|tudo>");
    return 1;
}

switch (args[0])
{
    case "tabela": Tabela(); break;
    case "classes": Classes(); break;
    case "pentagrama": Pentagrama(); break;
    case "primeira": Primeira(); break;
    case "exemplo": Exemplo(); break;
    case "juiz8": Juiz8(); break;
    case "tudo":
        Tabela(); Classes(); Pentagrama(); Primeira(); Exemplo();
        break;
    default:
        Console.WriteLine($"nao conheco '{args[0]}'");
        return 1;
}
return 0;

void Tabela()
{
    Console.WriteLine("pontas   numeros   constante   estrelas (busca)   juiz (ate 7 pontas)   tempo do juiz");
    foreach (var l in Medidas.Tabela())
    {
        string juiz = "-", tempo = "-";
        if (l.Pontas <= 7)
        {
            var sw = Stopwatch.StartNew();
            long j = Juiz.Contar(l.Pontas);
            tempo = $"{sw.Elapsed.TotalSeconds:F1} s";
            juiz = j == l.Estrelas ? j.ToString() : $"{j} DIVERGE";
        }
        Console.WriteLine($"{l.Pontas,-8} {$"1 a {2 * l.Pontas}",-9} {l.Constante,-11} {l.Estrelas,-18} {juiz,-21} {tempo}");
    }
    Console.WriteLine();
}

void Classes()
{
    Console.WriteLine("pontas   estrelas   classes por giro e espelho   juntando o complemento   que sao o proprio complemento");
    foreach (var l in Medidas.Tabela())
        Console.WriteLine($"{l.Pontas,-8} {l.Estrelas,-10} {l.Classes,-28} {l.ClassesComComplemento,-24} {l.Autocomplementares}");
    Console.WriteLine();
}

void Pentagrama()
{
    Console.WriteLine("estrela de 5 pontas com dez numeros distintos tirados de 1 ate o maior");
    Console.WriteLine("maior   conjuntos   constante inteira   com estrela   estrelas   estrelas por conjunto (quantas x quantos)   juiz");
    foreach (var p in Medidas.PentagramasAte16())
    {
        string juiz = "-";
        if (p.Maior <= 13)
        {
            long j = Medidas.Combinacoes(p.Maior, 10).Sum(s => Juiz.Contar(5, s));
            juiz = j == p.Estrelas ? j.ToString() : $"{j} DIVERGE";
        }
        string por = string.Join("  ", p.EstrelasPorConjunto.Select(x => $"{x.Key} x {x.Value}"));
        Console.WriteLine($"{p.Maior,-7} {p.Conjuntos,-11} {p.ComConstanteInteira,-19} {p.ComEstrela,-13} {p.Estrelas,-10} {por,-42} {juiz}");
    }
    Console.WriteLine("de 1 a 12, os que ficam de fora: " +
        string.Join("  ou  ", Medidas.PentagramasAte(12).Fora.Select(f => string.Join(" e ", f.Fora))));
    Console.WriteLine("de 1 a 16, os de 240 estrelas deixam de fora: " +
        string.Join("  ou  ", Medidas.PentagramasAte(16).Fora.Where(f => f.Estrelas != 120).Select(f => string.Join(" ", f.Fora))));
    Console.WriteLine();
}

void Primeira()
{
    Console.WriteLine("pontas   nos para contar todas   nos ate a primeira   fracao");
    foreach (var l in Medidas.Tabela())
        Console.WriteLine($"{l.Pontas,-8} {l.Nos,-23} {l.NosPrimeira,-20} {100.0 * l.NosPrimeira / l.Nos:F1}%");
    Console.WriteLine();
}

void Exemplo()
{
    var e = new Estrela(6);
    var v = Medidas.Primeira(6);
    Console.WriteLine("a primeira estrela de 6 pontas: as retas, cada uma ponta, miolo, miolo, ponta");
    for (int r = 0; r < e.Pontas; r++)
        Console.WriteLine($"   reta {r}: {string.Join(" + ", e.Retas[r].Select(c => v[c].ToString().PadLeft(2)))} = {e.Retas[r].Sum(c => v[c])}");
    Console.WriteLine();
}

void Juiz8()
{
    var sw = Stopwatch.StartNew();
    long j = Juiz.Contar(8);
    Console.WriteLine($"juiz, 8 pontas: {j} (busca: {Medidas.Da(8).Estrelas}), {sw.Elapsed.TotalSeconds:F0} s");
}
