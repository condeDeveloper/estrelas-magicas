namespace Conde.EstrelasMagicas.Testes;

public class PesadosTestes
{
    [Fact]
    [Trait("Category", "Pesado")]
    public void OJuizConfirmaAs1792EstrelasDeOitoPontas()
    {
        Assert.Equal(1792, Juiz.Contar(8));
    }
}
