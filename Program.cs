
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace CrudGastos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GastoRepositorio repo = new GastoRepositorio();
            string opcion;

            do
            {
                Console.WriteLine();
                Console.WriteLine("===== MENÚ DE GASTOS =====");
                Console.WriteLine("1. Registrar gasto");
                Console.WriteLine("2. Listar gastos");
                Console.WriteLine("3. Buscar gasto por Id");
                Console.WriteLine("4. Actualizar monto");
                Console.WriteLine("5. Eliminar gasto");
                Console.WriteLine("6. Salir");
                Console.Write("Elige una opción: ");
                opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        RegistrarGasto(repo);
                        break;

                    case "2":
                        ListarGastos(repo);
                        break;

                    case "3":
                        BuscarGastoPorId(repo);
                        break;

                    case "4":
                        ActualizarMonto(repo);
                        break;

                    case "5":
                        EliminarGasto(repo);
                        break;

                    case "6":
                        Console.WriteLine("Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

            } while (opcion != "6");
        }

        private static void RegistrarGasto(GastoRepositorio repo)
        {
            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                Console.WriteLine("La descripción es obligatoria.");
                return;
            }

            Console.Write("Monto: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal monto) || monto <= 0)
            {
                Console.WriteLine("Monto inválido.");
                return;
            }

            Console.Write("Categoría: ");
            string categoria = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("La categoría es obligatoria.");
                return;
            }

            Gasto gasto = new Gasto
            {
                Descripcion = descripcion,
                Monto = monto,
                Categoria = categoria
            };

            try
            {
                repo.Insertar(gasto);
                Console.WriteLine("Gasto registrado.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al registrar: {ex.Message}");
            }
        }

        private static void ListarGastos(GastoRepositorio repo)
        {
            try
            {
                List<Gasto> gastos = repo.ListarTodos();

                if (gastos.Count == 0)
                {
                    Console.WriteLine("No hay gastos registrados.");
                    return;
                }

                decimal total = 0;

                Console.WriteLine();
                Console.WriteLine(
                    "{0,-5}{1,-26}{2,-14}{3}",
                    "ID", "DESCRIPCIÓN", "MONTO", "CATEGORÍA");

                Console.WriteLine(
                    "--------------------------------------------------------------");

                foreach (Gasto gasto in gastos)
                {
                    Console.WriteLine(
                        "{0,-5}{1,-26}Q {2,-11:0.00}{3}",
                        gasto.Id,
                        gasto.Descripcion,
                        gasto.Monto,
                        gasto.Categoria);

                    total += gasto.Monto;
                }

                Console.WriteLine(
                    "--------------------------------------------------------------");

                Console.WriteLine($"TOTAL GASTADO: Q {total:0.00}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al listar: {ex.Message}");
            }
        }

        private static void BuscarGastoPorId(GastoRepositorio repo)
        {
            Console.Write("Id del gasto a buscar: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                Gasto? gasto = repo.BuscarPorId(id);

                if (gasto == null)
                {
                    Console.WriteLine("No existe ese Id.");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("===== GASTO ENCONTRADO =====");
                    Console.WriteLine(
                        "{0,-5}{1,-26}{2,-14}{3}",
                        "ID", "DESCRIPCIÓN", "MONTO", "CATEGORÍA");

                    Console.WriteLine(
                        "--------------------------------------------------------------");

                    Console.WriteLine(
                        "{0,-5}{1,-26}Q {2,-11:0.00}{3}",
                        gasto.Id,
                        gasto.Descripcion,
                        gasto.Monto,
                        gasto.Categoria);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al buscar: {ex.Message}");
            }
        }

        private static void ActualizarMonto(GastoRepositorio repo)
        {
            Console.Write("Id del gasto a actualizar: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                Gasto? gasto = repo.BuscarPorId(id);

                if (gasto == null)
                {
                    Console.WriteLine("No existe ese Id.");
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("===== GASTO ACTUAL =====");
                Console.WriteLine(
                    "{0,-5}{1,-26}{2,-14}{3}",
                    "ID", "DESCRIPCIÓN", "MONTO", "CATEGORÍA");

                Console.WriteLine(
                    "--------------------------------------------------------------");

                Console.WriteLine(
                    "{0,-5}{1,-26}Q {2,-11:0.00}{3}",
                    gasto.Id,
                    gasto.Descripcion,
                    gasto.Monto,
                    gasto.Categoria);

                Console.Write("Nuevo monto: ");

                if (!decimal.TryParse(Console.ReadLine(), out decimal monto) || monto <= 0)
                {
                    Console.WriteLine("Monto inválido.");
                    return;
                }

                gasto.Monto = monto;

                Console.WriteLine(
                    repo.Actualizar(gasto)
                        ? "Gasto actualizado."
                        : "No se actualizó.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al actualizar: {ex.Message}");
            }
        }

        private static void EliminarGasto(GastoRepositorio repo)
        {
            Console.Write("Id del gasto a eliminar: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                bool ok = repo.Eliminar(id);

                Console.WriteLine(
                    ok ? "Gasto eliminado." : "No existe ese Id.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
            }
        }
    }
}
