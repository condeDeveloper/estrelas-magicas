namespace Conde.EstrelasMagicas.Testes;

/// <summary>Os numeros do README, presos.</summary>
public class MedidasTestes
{
    [Fact]
    public void QuantasEstrelasDeCincoADezPontas()
    {
        Assert.Equal(new long[] { 0, 960, 1008, 1792, 54252, 217640 }, Medidas.Tabela().Select(l => l.Estrelas));
    }

    [Fact]
    public void ClassesSaoOTotalDivididoPor2nSemSobra()
    {
        Assert.Equal(new[] { 0, 80, 72, 112, 3014, 10882 }, Medidas.Tabela().Select(l => l.Classes));
        Assert.All(Medidas.Tabela(), l => Assert.Equal(l.Estrelas, 2L * l.Pontas * l.Classes));
    }

    [Fact]
    public void OComplementoJuntaAsClassesDeDuasEmDuasENenhumaEOProprio()
    {
        Assert.Equal(new[] { 0, 40, 36, 56, 1507, 5441 }, Medidas.Tabela().Select(l => l.ClassesComComplemento));
        Assert.All(Medidas.Tabela(), l => Assert.Equal(0, l.Autocomplementares));
    }

    [Fact]
    public void OsNosDaBusca()
    {
        Assert.Equal(new long[] { 11979, 93217, 646583, 4734139, 36473249, 293754239 }, Medidas.Tabela().Select(l => l.Nos));
        Assert.Equal(new long[] { 11979, 17, 25, 3168, 213, 171 }, Medidas.Tabela().Select(l => l.NosPrimeira));
    }

    [Fact]
    public void ATabelaDosPentagramas()
    {
        var t = Medidas.PentagramasAte16();
        Assert.Equal(new[] { 1, 11, 66, 286, 1001, 3003, 8008 }, t.Select(p => p.Conjuntos));
        Assert.Equal(new[] { 1, 3, 14, 58, 201, 603, 1604 }, t.Select(p => p.ComConstanteInteira));
        Assert.Equal(new[] { 0, 0, 2, 12, 34, 82, 188 }, t.Select(p => p.ComEstrela));
        Assert.Equal(new long[] { 0, 0, 240, 1440, 4080, 9840, 22800 }, t.Select(p => p.Estrelas));
    }

    [Fact]
    public void DeUmADozeSoSaemO7EO11OuO2EO6()
    {
        var fora = Medidas.PentagramasAte(12).Fora.Select(f => string.Join(" ", f.Fora)).OrderBy(s => s);
        Assert.Equal(new[] { "2 6", "7 11" }, fora);
    }

    [Fact]
    public void Ate15TodoConjuntoDa120EO16TemDoisDe240()
    {
        for (int maior = 12; maior <= 15; maior++)
            Assert.Equal(new[] { 120L }, Medidas.PentagramasAte(maior).EstrelasPorConjunto.Keys);
        var d16 = Medidas.PentagramasAte(16).EstrelasPorConjunto;
        Assert.Equal(186, d16[120]);
        Assert.Equal(2, d16[240]);
        var de240 = Medidas.PentagramasAte(16).Fora.Where(f => f.Estrelas == 240).Select(f => string.Join(" ", f.Fora)).OrderBy(s => s);
        Assert.Equal(new[] { "2 3 5 10 12 14", "3 5 7 12 14 15" }, de240);
    }
}
