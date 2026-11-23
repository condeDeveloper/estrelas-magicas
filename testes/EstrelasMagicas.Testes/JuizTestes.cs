namespace Conde.EstrelasMagicas.Testes;

public class JuizTestes
{
    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void OJuizAchaAsMesmasEstrelasQueABusca(int n)
    {
        var e = new Estrela(n);
        var busca = new HashSet<string>();
        new Busca(e).Contar(v => busca.Add(string.Join(",", v)));
        var juiz = new HashSet<string>();
        Juiz.Contar(n, v => juiz.Add(string.Join(",", v)));
        Assert.Equal(busca, juiz);
    }

    [Fact]
    public void OJuizConcordaNosConjuntosDeDezAteTreze()
    {
        foreach (int maior in new[] { 12, 13 })
        {
            long juiz = Medidas.Combinacoes(maior, 10).Sum(s => Juiz.Contar(5, s));
            Assert.Equal(Medidas.PentagramasAte(maior).Estrelas, juiz);
        }
    }

    [Fact]
    public void AsCombinacoesSaoTodasEEmOrdem()
    {
        var c = Medidas.Combinacoes(6, 3).ToList();
        Assert.Equal(20, c.Count);
        Assert.Equal(new[] { 1, 2, 3 }, c[0]);
        Assert.Equal(new[] { 4, 5, 6 }, c[^1]);
    }
}
