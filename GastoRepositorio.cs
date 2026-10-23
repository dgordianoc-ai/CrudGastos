
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace CrudGastos
{
    public class GastoRepositorio
    {
        private readonly string cadenaConexion =
            @"Server=(localdb)\MSSQLLocalDB;Database=GastosDB;Trusted_Connection=True;";

        public void Insertar(Gasto gasto)
        {
            string sql = "INSERT INTO Gastos (Descripcion, Monto, Categoria) " +
                         "VALUES (@descripcion, @monto, @categoria)";

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@descripcion", gasto.Descripcion);
                comando.Parameters.AddWithValue("@monto", gasto.Monto);
                comando.Parameters.AddWithValue("@categoria", gasto.Categoria);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        public List<Gasto> ListarTodos()
        {
            List<Gasto> gastos = new List<Gasto>();

            string sql = "SELECT Id, Descripcion, Monto, Categoria " +
                         "FROM Gastos ORDER BY Id";

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Gasto gasto = new Gasto
                        {
                            Id = lector.GetInt32(0),
                            Descripcion = lector.GetString(1),
                            Monto = lector.GetDecimal(2),
                            Categoria = lector.GetString(3)
                        };

                        gastos.Add(gasto);
                    }
                }
            }

            return gastos;
        }

        public Gasto? BuscarPorId(int id)
        {
            string sql = "SELECT Id, Descripcion, Monto, Categoria " +
                         "FROM Gastos WHERE Id = @id";

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return new Gasto
                        {
                            Id = lector.GetInt32(0),
                            Descripcion = lector.GetString(1),
                            Monto = lector.GetDecimal(2),
                            Categoria = lector.GetString(3)
                        };
                    }
                }
            }

            return null;
        }

        public bool Actualizar(Gasto gasto)
        {
            string sql = "UPDATE Gastos SET Monto = @monto WHERE Id = @id";

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@monto", gasto.Monto);
                comando.Parameters.AddWithValue("@id", gasto.Id);

                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }

        public bool Eliminar(int id)
        {
            string sql = "DELETE FROM Gastos WHERE Id = @id";

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);

                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }
    }
}
