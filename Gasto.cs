
namespace CrudGastos
{
    public class Gasto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Cuota { get; set; }
        public string Instructor { get; set; } = "";

        public override string ToString()
        {
            return Id + " - " + Nombre + " - Q " +
                   Cuota.ToString("0.00") + " - " + Instructor;
        }
    }
}
