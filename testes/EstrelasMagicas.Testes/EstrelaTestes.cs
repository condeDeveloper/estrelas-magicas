namespace Conde.EstrelasMagicas.Testes;

public class EstrelaTestes
{
    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(9)]
    public void CadaRetaTemQuatroCasasECadaCasaEstaEmDuasRetas(int n)
    {
        var e = new Estrela(n);
        Assert.All(e.Retas, r => Assert.Equal(4, r.Distinct().Count()));
        for (int c = 0; c < e.Casas; c++)
            Assert.Equal(2, e.Retas.Count(r => r.Contains(c)));
    }

    [Fact]
    public void DuasRetasDividemNoMaximoUmaCasa()
    {
        var e = new Estrela(7);
        for (int a = 0; a < 7; a++)
            for (int b = a + 1; b < 7; b++)
                Assert.True(e.Retas[a].Intersect(e.Retas[b]).Count() <= 1);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(8)]
    public void AsSimetriasSaoDiferentesELevamRetaEmReta(int n)
    {
        var e = new Estrela(n);
        var retas = e.Retas.Select(r => string.Join(",", r.OrderBy(x => x))).ToHashSet();
        var imagens = new HashSet<string>();
        for (int t = 0; t < 2 * n; t++)
        {
            imagens.Add(string.Join(",", Enumerable.Range(0, e.Casas).Select(c => e.Imagem(t, c))));
            foreach (var r in e.Retas)
                Assert.Contains(string.Join(",", r.Select(c => e.Imagem(t, c)).OrderBy(x => x)), retas);
        }
        Assert.Equal(2 * n, imagens.Count);
    }

    [Fact]
    public void AConstanteE4nMais2()
    {
        Assert.Equal(22, new Estrela(5).Constante);
        Assert.Equal(26, new Estrela(6).Constante);
    }

    [Fact]
    public void OComplementoDeUmaEstrelaTambemEMagico()
    {
        var e = new Estrela(7);
        var v = Medidas.Primeira(7);
        Assert.True(e.EMagica(v));
        Assert.True(e.EMagica(Medidas.Complemento(e, v)));
    }

    [Fact]
    public void QuatroPontasNaoEEstrela()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Estrela(4));
    }
}
