
namespace CrudGastos
{
    public class Gasto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = "";
        public decimal Monto { get; set; }
        public string Categoria { get; set; } = "";

        public override string ToString()
        {
            return Id + " - " + Descripcion + " - Q " +
                   Monto.ToString("0.00") + " - " + Categoria;
        }
    }
}
