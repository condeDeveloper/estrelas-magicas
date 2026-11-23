namespace Conde.EstrelasMagicas;

/// <summary>
/// O juiz nao usa as retas da Estrela nem a busca. Ele varre TODAS as
/// maneiras de por numeros distintos nos n miolos; com os miolos postos, a
/// reta i diz quanto as pontas i e i+2 somam: P_i + P_(i+2) = M - X_(i-1) - X_i.
/// Andando de 2 em 2 pela roda das pontas, cada ciclo tem uma ponta livre (a
/// primeira) e as outras saem dela; o ciclo tem que fechar na conta e usar
/// numeros ainda livres.
/// </summary>
public static class Juiz
{
    public static long Contar(int n, Action<int[]>? visita = null) =>
        Contar(n, Enumerable.Range(1, 2 * n).ToArray(), visita);

    /// <summary>Com 2n numeros quaisquer: a constante e 2 * soma / n.</summary>
    public static long Contar(int n, int[] numeros, Action<int[]>? visita = null)
    {
        int casas = 2 * n;
        int soma = numeros.Sum();
        if (2 * soma % n != 0)
            return 0;
        int m = 2 * soma / n;
        int maior = numeros.Max();
        var miolo = new int[n];
        var ponta = new int[n];
        var usado = new bool[maior + 2];
        var noJogo = new bool[maior + 2];
        foreach (int v in numeros)
            noJogo[v] = true;
        long achadas = 0;

        // os ciclos de i -> i + 2 nas pontas
        var ciclos = new List<int[]>();
        var visto = new bool[n];
        for (int i = 0; i < n; i++)
        {
            if (visto[i])
                continue;
            var ciclo = new List<int>();
            for (int j = i; !visto[j]; j = (j + 2) % n)
            {
                visto[j] = true;
                ciclo.Add(j);
            }
            ciclos.Add(ciclo.ToArray());
        }

        int SomaDoPar(int i) => m - miolo[(i - 1 + n) % n] - miolo[i];

        void Pontas(int k)
        {
            if (k == ciclos.Count)
            {
                achadas++;
                if (visita != null)
                {
                    var v = new int[casas];
                    for (int i = 0; i < n; i++)
                    {
                        v[i] = ponta[i];
                        v[n + i] = miolo[i];
                    }
                    visita(v);
                }
                return;
            }
            var ciclo = ciclos[k];
            foreach (int a in numeros)
            {
                if (usado[a])
                    continue;
                var postos = new List<int>();
                bool ok = true;
                int atual = a;
                for (int t = 0; t < ciclo.Length; t++)
                {
                    if (atual < 1 || atual > maior || !noJogo[atual] || usado[atual])
                    {
                        ok = false;
                        break;
                    }
                    usado[atual] = true;
                    postos.Add(atual);
                    ponta[ciclo[t]] = atual;
                    atual = SomaDoPar(ciclo[t]) - atual; // a proxima ponta do ciclo
                }
                if (ok && atual == a) // o ciclo fecha na primeira ponta
                    Pontas(k + 1);
                foreach (int p in postos)
                    usado[p] = false;
            }
        }

        void Miolos(int i)
        {
            if (i == n)
            {
                Pontas(0);
                return;
            }
            foreach (int v in numeros)
            {
                if (usado[v])
                    continue;
                usado[v] = true;
                miolo[i] = v;
                Miolos(i + 1);
                usado[v] = false;
            }
        }

        Miolos(0);
        return achadas;
    }
}
