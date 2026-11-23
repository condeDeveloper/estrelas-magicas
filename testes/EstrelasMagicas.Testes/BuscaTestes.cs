namespace Conde.EstrelasMagicas.Testes;

public class BuscaTestes
{
    [Fact]
    public void AsEstrelasDeSeisSaoMagicasPelaDefinicaoETodasDiferentes()
    {
        var e = new Estrela(6);
        var vistas = new HashSet<string>();
        new Busca(e).Contar(v =>
        {
            Assert.True(e.EMagica(v));
            Assert.True(vistas.Add(string.Join(",", v)));
        });
        Assert.Equal(960, vistas.Count);
    }

    [Fact]
    public void SemO7EO11AEstrelaDeCincoFechaEm24()
    {
        var e = new Estrela(5);
        var numeros = Enumerable.Range(1, 12).Except(new[] { 7, 11 }).ToArray();
        var b = new Busca(e, numeros);
        Assert.Equal(24, b.Constante);
        long c = b.Contar(v =>
        {
            Assert.Equal(numeros, v.OrderBy(x => x));
            Assert.All(e.Retas, r => Assert.Equal(24, r.Sum(x => v[x])));
        });
        Assert.Equal(120, c);
    }

    [Fact]
    public void ConstanteQuebradaNaoMontaBusca()
    {
        Assert.Throws<ArgumentException>(() => new Busca(new Estrela(5), Enumerable.Range(1, 9).Append(11).ToArray()));
    }

    [Fact]
    public void APrimeiraEUmaDasQueSeContam()
    {
        var e = new Estrela(7);
        var p = Medidas.Primeira(7);
        bool achou = false;
        new Busca(e).Contar(v => achou |= v.SequenceEqual(p));
        Assert.True(achou);
    }
}
