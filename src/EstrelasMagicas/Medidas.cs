namespace Conde.EstrelasMagicas;

public sealed record Linha(
    int Pontas, int Constante, long Estrelas, long Nos, long NosPrimeira,
    int Classes, int ClassesComComplemento, int Autocomplementares);

/// <summary>Os conjuntos de dez numeros tirados de 1..Maior para a estrela de 5 pontas.</summary>
public sealed record Pentagramas(
    int Maior, int Conjuntos, int ComConstanteInteira, int ComEstrela, long Estrelas,
    SortedDictionary<long, int> EstrelasPorConjunto, List<(int[] Fora, long Estrelas)> Fora);

/// <summary>As tabelas do README, calculadas uma vez e guardadas.</summary>
public static class Medidas
{
    public const int MenorPontas = 5;
    public const int MaiorPontas = 10;

    private static readonly Lazy<IReadOnlyList<Linha>> _tabela = new(() =>
        Enumerable.Range(MenorPontas, MaiorPontas - MenorPontas + 1).Select(Calcular).ToList());

    public static IReadOnlyList<Linha> Tabela() => _tabela.Value;

    public static Linha Da(int pontas) => Tabela()[pontas - MenorPontas];

    private static Linha Calcular(int n)
    {
        var e = new Estrela(n);
        var b = new Busca(e);
        var classes = new HashSet<string>();
        var classesC = new HashSet<string>();
        int auto = 0;
        long total = b.Contar(v =>
        {
            string canon = Canonica(e, v);
            if (!classes.Add(canon))
                return;
            // primeira vez que a classe aparece: olha o complemento
            string canonC = Canonica(e, Complemento(e, v));
            if (canonC == canon)
                auto++;
            classesC.Add(string.CompareOrdinal(canon, canonC) < 0 ? canon : canonC);
        });
        long nos = b.Nos;
        long nosPrimeira = b.Primeira().Nos;
        return new Linha(n, e.Constante, total, nos, nosPrimeira, classes.Count, classesC.Count, auto);
    }

    /// <summary>Trocar cada numero v por 2n + 1 - v: a soma de cada reta vira 4(2n+1) - M = M.</summary>
    public static int[] Complemento(Estrela e, IReadOnlyList<int> v) =>
        v.Select(x => e.Casas + 1 - x).ToArray();

    /// <summary>A menor das 2n imagens por giro e espelho, como texto.</summary>
    public static string Canonica(Estrela e, IReadOnlyList<int> v)
    {
        string? melhor = null;
        for (int t = 0; t < 2 * e.Pontas; t++)
        {
            var img = new int[e.Casas];
            for (int c = 0; c < e.Casas; c++)
                img[e.Imagem(t, c)] = v[c];
            string s = string.Join(",", img.Select(x => x.ToString("D2")));
            if (melhor == null || string.CompareOrdinal(s, melhor) < 0)
                melhor = s;
        }
        return melhor!;
    }

    private static readonly Lazy<IReadOnlyList<Pentagramas>> _pentagramas = new(() =>
        Enumerable.Range(10, 7).Select(CalcularPentagramas).ToList());

    /// <summary>De 1..10 ate 1..16: quais conjuntos de dez numeros fazem estrela de 5 pontas.</summary>
    public static IReadOnlyList<Pentagramas> PentagramasAte16() => _pentagramas.Value;

    public static Pentagramas PentagramasAte(int maior) => PentagramasAte16()[maior - 10];

    private static Pentagramas CalcularPentagramas(int maior)
    {
        var e = new Estrela(5);
        int conjuntos = 0, inteira = 0, com = 0;
        long estrelas = 0;
        var porConjunto = new SortedDictionary<long, int>();
        var fora = new List<(int[], long)>();
        foreach (var s in Combinacoes(maior, 10))
        {
            conjuntos++;
            if (2 * s.Sum() % 5 != 0)
                continue;
            inteira++;
            long c = new Busca(e, s).Contar();
            if (c == 0)
                continue;
            com++;
            estrelas += c;
            porConjunto[c] = porConjunto.GetValueOrDefault(c) + 1;
            fora.Add((Enumerable.Range(1, maior).Except(s).ToArray(), c));
        }
        return new Pentagramas(maior, conjuntos, inteira, com, estrelas, porConjunto, fora);
    }

    /// <summary>Os subconjuntos de k elementos de 1..n, em ordem.</summary>
    public static IEnumerable<int[]> Combinacoes(int n, int k)
    {
        var idx = Enumerable.Range(1, k).ToArray();
        while (true)
        {
            yield return (int[])idx.Clone();
            int t = k - 1;
            while (t >= 0 && idx[t] == n - k + t + 1)
                t--;
            if (t < 0)
                yield break;
            idx[t]++;
            for (int u = t + 1; u < k; u++)
                idx[u] = idx[u - 1] + 1;
        }
    }

    public static int[] Primeira(int pontas) => new Busca(new Estrela(pontas)).Primeira().Valores!;
}
