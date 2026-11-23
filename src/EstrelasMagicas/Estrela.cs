namespace Conde.EstrelasMagicas;

/// <summary>
/// A estrela de n pontas {n/2}: as pontas P0..P(n-1) num circulo e as n retas
/// L_i ligando P_i a P_(i+2). Duas retas so se cruzam por dentro quando sao
/// vizinhas (L_i e L_(i+1)); esse cruzamento e o miolo X_i. Cada reta passa
/// por quatro casas, P_i, X_(i-1), X_i e P_(i+2), e cada casa esta em duas retas.
///
/// Casas 0..n-1 sao as pontas; casas n..2n-1 sao os miolos.
/// </summary>
public sealed class Estrela
{
    public int Pontas { get; }
    public int Casas => 2 * Pontas;
    public int[][] Retas { get; }

    public Estrela(int pontas)
    {
        if (pontas < 5)
            throw new ArgumentOutOfRangeException(nameof(pontas), "a estrela {n/2} pede n >= 5");
        Pontas = pontas;
        Retas = new int[pontas][];
        for (int i = 0; i < pontas; i++)
            Retas[i] = new[] { P(i), X(i - 1), X(i), P(i + 2) };
    }

    public int P(int i) => Mod(i);
    public int X(int i) => Pontas + Mod(i);

    /// <summary>A constante: cada numero de 1 a 2n esta em duas retas, entao n retas somam 2 * (2n)(2n+1)/2.</summary>
    public int Constante => 4 * Pontas + 2;

    private int Mod(int i) => ((i % Pontas) + Pontas) % Pontas;

    /// <summary>
    /// As 2n simetrias da estrela: girar i -> i + t, e espelhar i -> -i
    /// (que leva X_i, o cruzamento de L_i com L_(i+1), em X_(-i-3)).
    /// </summary>
    public int Imagem(int t, int casa)
    {
        int n = Pontas;
        bool espelho = t >= n;
        int g = t % n;
        if (casa < n)
            return P((espelho ? -casa : casa) + g);
        int i = casa - n;
        return X((espelho ? -i - 3 : i) + g);
    }

    public bool EMagica(IReadOnlyList<int> v)
    {
        if (v.Count != Casas || v.OrderBy(x => x).Where((x, i) => x != i + 1).Any())
            return false;
        return Retas.All(r => r.Sum(c => v[c]) == Constante);
    }
}
