using System.Numerics;

namespace Conde.EstrelasMagicas;

/// <summary>
/// Poe os numeros 1..2n nas casas, um por vez. Se uma reta tem uma casa vazia
/// so, o valor dela e forcado; senao a reta com menos casas vazias da a
/// proxima casa, e cada numero livre so entra se as duas retas da casa ainda
/// puderem fechar com os menores e com os maiores numeros que sobram.
/// </summary>
public sealed class Busca
{
    private readonly Estrela _e;
    private readonly int _n;
    private readonly int _m;
    private readonly int[][] _retasDaCasa;
    private readonly int[] _soma;
    private readonly int[] _vazias;
    private readonly int[] _valor;
    private readonly uint _todos; // bit v-1 = numero v faz parte do jogo
    private uint _livres; // bit v-1 = numero v livre
    private bool _parar;
    private Action<int[]>? _visita;
    private long _achadas;

    public long Nos { get; private set; }

    /// <summary>Com os numeros 1..2n, ou com 2n numeros quaisquer entre 1 e 32.</summary>
    public Busca(Estrela e, IReadOnlyCollection<int>? numeros = null)
    {
        _e = e;
        _n = e.Casas;
        numeros ??= Enumerable.Range(1, e.Casas).ToArray();
        if (numeros.Count != _n || numeros.Distinct().Count() != _n || numeros.Any(v => v < 1 || v > 32))
            throw new ArgumentException("2n numeros distintos entre 1 e 32");
        foreach (int v in numeros)
            _todos |= 1u << (v - 1);
        int soma = numeros.Sum();
        if (2 * soma % e.Pontas != 0)
            throw new ArgumentException("a constante nao e inteira");
        _m = 2 * soma / e.Pontas;
        _retasDaCasa = new int[_n][];
        for (int c = 0; c < _n; c++)
            _retasDaCasa[c] = Enumerable.Range(0, e.Pontas).Where(r => e.Retas[r].Contains(c)).ToArray();
        _soma = new int[e.Pontas];
        _vazias = new int[e.Pontas];
        _valor = new int[_n];
    }

    public int Constante => _m;

    public long Contar(Action<int[]>? visita = null)
    {
        Preparar(visita);
        Descer(0);
        return _achadas;
    }

    public (int[]? Valores, long Nos) Primeira()
    {
        int[]? achada = null;
        Preparar(v => { achada = v; _parar = true; });
        Descer(0);
        return (achada, Nos);
    }

    private void Preparar(Action<int[]>? visita)
    {
        _visita = visita;
        _parar = false;
        _achadas = 0;
        Nos = 0;
        Array.Clear(_valor);
        Array.Clear(_soma);
        Array.Fill(_vazias, 4);
        _livres = _todos;
    }

    private void Descer(int postas)
    {
        Nos++;
        if (postas == _n)
        {
            _achadas++;
            _visita?.Invoke((int[])_valor.Clone());
            return;
        }
        int escolhida = -1, menos = int.MaxValue;
        for (int r = 0; r < _e.Pontas; r++)
        {
            int vz = _vazias[r];
            if (vz == 1)
            {
                int v = _m - _soma[r];
                if (v < 1 || v > 32 || (_livres >> (v - 1) & 1) == 0)
                    return;
                Tentar(Vazia(r), v, postas);
                return;
            }
            if (vz > 1 && vz < menos)
            {
                menos = vz;
                escolhida = r;
            }
        }
        int c = Vazia(escolhida);
        uint livres = _livres;
        while (livres != 0 && !_parar)
        {
            int v = BitOperations.TrailingZeroCount(livres) + 1;
            livres &= livres - 1;
            Tentar(c, v, postas);
        }
    }

    private void Tentar(int c, int v, int postas)
    {
        _valor[c] = v;
        _livres &= ~(1u << (v - 1));
        foreach (int r in _retasDaCasa[c])
        {
            _soma[r] += v;
            _vazias[r]--;
        }
        bool ok = true;
        foreach (int r in _retasDaCasa[c])
            ok &= Fecha(r);
        if (ok)
            Descer(postas + 1);
        foreach (int r in _retasDaCasa[c])
        {
            _soma[r] -= v;
            _vazias[r]++;
        }
        _livres |= 1u << (v - 1);
        _valor[c] = 0;
    }

    private bool Fecha(int r)
    {
        int vz = _vazias[r];
        int falta = _m - _soma[r];
        if (vz == 0)
            return falta == 0;
        if (BitOperations.PopCount(_livres) < vz)
            return false;
        int menor = 0, maior = 0;
        uint x = _livres;
        for (int k = 0; k < vz; k++)
        {
            menor += BitOperations.TrailingZeroCount(x) + 1;
            x &= x - 1;
        }
        x = _livres;
        for (int k = 0; k < vz; k++)
        {
            int b = 31 - BitOperations.LeadingZeroCount(x);
            maior += b + 1;
            x &= ~(1u << b);
        }
        return menor <= falta && falta <= maior;
    }

    private int Vazia(int r)
    {
        foreach (int c in _e.Retas[r])
            if (_valor[c] == 0)
                return c;
        throw new InvalidOperationException();
    }
}
