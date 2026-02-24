using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEmpleados
{
    internal class MenuPrincipal
    {
        // Tipos de campo de empleado que se pueden validar
        private enum CampoEmpleado
        {
            Nombre,
            Email,
            Rfc,
            Nss
        }

        // Campo que controla el ciclo del menú
        private bool _continuar = true;

        private void MostrarOpciones()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║     GESTOR DE EMPLEADOS - MENÚ    ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine("");
            Console.WriteLine("  1) Agregar empleado");
            Console.WriteLine("  2) Listar todos los empleados");
            Console.WriteLine("  3) Buscar empleado por ID");
            Console.WriteLine("  4) Buscar empleado por nombre");
            Console.WriteLine("  5) Ver empleados activos");
            Console.WriteLine("  6) Ver empleados inactivos");
            Console.WriteLine("  7) Aplicar aumento salarial");
            Console.WriteLine("  8) Cambiar estado (Activar/Suspender)");
            Console.WriteLine("  9) Ver estadísticas generales de nómina ");
            Console.WriteLine("  10) Generar Reporte");
            Console.WriteLine("  0) Salir");
            Console.Write("\nSelecciona una opción: ");
        }

        private int LeerOpcionMenu()
        {
            string entrada = Console.ReadLine() ?? "";

            if (int.TryParse(entrada, out int opcion))
            {
                return opcion;
            }
            else
            {
                return -1; // Devuelve -1 si el TryParse falla
            }
        }

        private void EjecutarOpcion(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    AgregarEmpleado();
                    break;
                case 2:
                    ListarEmpleados();
                    break;
                case 3:
                    BuscarPorId();
                    break;
                case 4:
                    BuscarPorNombre();
                    break;
                case 5:
                    VerActivos();
                    break;
                case 6:
                    VerInactivos();
                    break;
                case 7:
                    AplicarAumento();
                    break;
                case 8:
                    GestionarEstado();
                    break;
                case 9:
                    MostrarEstadisticasNomina();
                    break;
                case 10:
                    GenerarReporte();
                    break;
                case 0:
                    Salir();
                    break;
                default:
                    Console.WriteLine("Opción inválida. Intenta de nuevo.");
                    EsperarTecla();
                    break;
            }
        }

        // Método principal para mostrar el menú y procesar opciones
        public void Ejecutar()
        {
            while (_continuar)
            {
                MostrarOpciones();
                int opcion = LeerOpcionMenu();
                EjecutarOpcion(opcion);
            }
        }

        private void AgregarEmpleado()
        {
            MostrarTitulo("AGREGAR NUEVO EMPLEADO");

            // Leemos y validamos cada campo necesario
            string nombre = LeerYValidarCampo("Nombre: ", CampoEmpleado.Nombre);
            string email = LeerYValidarCampo("Email: ", CampoEmpleado.Email);
            string rfc = LeerYValidarCampo("RFC: ", CampoEmpleado.Rfc);
            // Para el NSS y RFC, además de validar el formato, también validamos que no existan ya en el sistema (ya que deben ser únicos)
            while (GestorEmpleados.ExisteEmpleadoConRfc(rfc))
            {
                Console.WriteLine($"\nError: Ya existe un empleado registrado con el RFC: '{rfc}'. Intente de nuevo!");
                rfc = LeerYValidarCampo("RFC: ", CampoEmpleado.Rfc);
            }
            string nss = LeerYValidarCampo("NSS: ", CampoEmpleado.Nss);
            while (GestorEmpleados.ExisteEmpleadoConNss(nss))
            {
                Console.WriteLine($"\nError: Ya existe un empleado registrado con el NSS: '{nss}'. Intente de nuevo!");
                nss = LeerYValidarCampo("NSS: ", CampoEmpleado.Nss);
            }
            decimal salario = LeerYValidarSalario();

            // Ya que tenemos todos los datos del empleado, lo creamos usando el constructor
            Empleado empleado = new Empleado(nombre, email, rfc, nss, salario);

            // Y lo agregamos al gestor (lista)
            GestorEmpleados.AgregarEmpleado(empleado);

            // Confirmamos que el empleado se agregó correctamente
            Console.WriteLine($"\nEmpleado '{empleado.Nombre}' agregado exitosamente (ID: {empleado.Id})");

            EsperarTecla();
        }

        private string LeerYValidarCampo(string textoMostrar, CampoEmpleado tipoCampo) //Nombre:
        {
            while (true)
            {
                Console.Write(textoMostrar);
                string entrada = Console.ReadLine() ?? "";

                bool esValido = false;
                string valorSanitizado = "";
                string mensajeError = "";

                // Switch que se ayuda del Enum para seguir una ruta
                switch (tipoCampo)
                {
                    case CampoEmpleado.Nombre:
                        esValido = Empleado.ValidarNombre(entrada, out valorSanitizado, out mensajeError);
                        break;
                    case CampoEmpleado.Email:
                        esValido = Empleado.ValidarEmail(entrada, out valorSanitizado, out mensajeError);
                        break;
                    case CampoEmpleado.Rfc:
                        esValido = Empleado.ValidarRfc(entrada, out valorSanitizado, out mensajeError);
                        break;
                    case CampoEmpleado.Nss:
                        esValido = Empleado.ValidarNss(entrada, out valorSanitizado, out mensajeError);
                        break;
                }

                // Si la entrada pasó las validaciones entonces la devolvemos ya sanitizada
                if (esValido)
                {
                    return valorSanitizado;
                }
                else
                {
                    // Pero si no, entonces mostramos el mensaje de error
                    Console.WriteLine($"{mensajeError}\n");
                }
            }
        }

        private decimal LeerYValidarSalario()
        {
            while (true)
            {
                Console.Write("Salario (USD): ");
                string entrada = Console.ReadLine() ?? "";

                if (!decimal.TryParse(entrada, out decimal salario))
                {
                    Console.WriteLine("El salario debe ser un número válido.\n");
                    continue;
                }
                if (Empleado.ValidarSalario(salario, out string mensajeError))
                {
                    return salario;
                }
                else
                {
                    Console.WriteLine($"{mensajeError}\n");
                }
            }
        }

        private void MostrarLista(List<Empleado> empleados)
        {
            if (empleados.Count == 0)
            {
                Console.WriteLine("No hay empleados para mostrar.");
            }
            else
            {
                foreach (Empleado empleado in empleados)
                {
                    Console.WriteLine(empleado);
                }
            }
        }

        private void ListarEmpleados()
        {
            MostrarTitulo("LISTA DE EMPLEADOS");

            // Lista local para almacenar los empleados obtenidos del gestor
            List<Empleado> empleados = GestorEmpleados.ObtenerTodos();

            // Delegamos la impresión al método especializado
            MostrarLista(empleados);

            EsperarTecla();
        }

        private void BuscarPorId()
        {
            MostrarTitulo("BUSCAR EMPLEADO POR ID");

            Empleado? empleado = SolicitarEmpleadoPorId();

            if (empleado != null)
            {
                Console.WriteLine("\n" + empleado);
            }

            EsperarTecla();
        }

        private void BuscarPorNombre()
        {
            MostrarTitulo("BUSCAR EMPLEADO POR NOMBRE");

            Console.Write("Ingresa el nombre (o parte de este): ");
            string nombre = Console.ReadLine() ?? "";

            List<Empleado> resultadosNombres = GestorEmpleados.BuscarPorNombre(nombre);

            // Nos ayudamos de nuestro método especializado para mostrar la lista de resultados (aunque sea de 1 solo elemento o incluso vacía)
            MostrarLista(resultadosNombres);

            EsperarTecla();
        }

        private void VerActivos()
        {
            MostrarTitulo("EMPLEADOS ACTIVOS");

            List<Empleado> activos = GestorEmpleados.ObtenerActivos();

            MostrarLista(activos);

            EsperarTecla();
        }

        private void VerInactivos()
        {
            MostrarTitulo("EMPLEADOS INACTIVOS");

            List<Empleado> inactivos = GestorEmpleados.ObtenerInactivos();
            MostrarLista(inactivos);

            EsperarTecla();
        }

        private void AplicarAumento()
        {
            MostrarTitulo("APLICAR AUMENTO SALARIAL");

            Empleado? empleado = SolicitarEmpleadoPorId();

            if (empleado != null)
            {
                Console.Write($"Salario actual de {empleado.Nombre}: {empleado.Salario:C}. \nIngresa el % de aumento: ");
                string porcentajeTexto = Console.ReadLine() ?? "";

                if (decimal.TryParse(porcentajeTexto, out decimal porcentaje))
                {
                    try
                    {
                        empleado.AplicarAumento(porcentaje);
                        Console.WriteLine($"¡Éxito! Nuevo salario: {empleado.Salario:C}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("\nError: El porcentaje debe ser un número válido.");
                }
            }
            EsperarTecla();
        }

        private void GestionarEstado()
        {
            MostrarTitulo("GESTIÓN DE ESTADO (REACTIVAR/SUSPENDER)\n");

            Empleado? empleado = SolicitarEmpleadoPorId();

            if (empleado != null)
            {
                Console.WriteLine($"Empleado: {empleado.Nombre} | Estado actual: {empleado.Estado}\n");
                Console.WriteLine("1. Suspender | 2. Reactivar");

                Console.Write("Selecciona una opción: ");
                string opcion = Console.ReadLine() ?? "";

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            empleado.Suspender();
                            break;
                        case "2":
                            empleado.Reactivar();
                            break;
                        default:
                            Console.WriteLine("\nOpción inválida. No se realizaron cambios.");
                            EsperarTecla();
                            return;
                    }
                    Console.WriteLine($"Estado actualizado a: {empleado.Estado}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
            EsperarTecla();
        }

        private void MostrarEstadisticasNomina()
        {
            MostrarTitulo("ESTADÍSTICAS GENERALES DE NÓMINA");

            decimal totalNomina = GestorEmpleados.CalcularNominaActivos();

            GestorEmpleados.ObtenerEstadisticasSalariales(out decimal promedio, out decimal max, out decimal min);

            Console.WriteLine($"Total de Empleados:    {GestorEmpleados.TotalEmpleados}");
            Console.WriteLine($"Nómina Total (Activos): {totalNomina:C}");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"Salario Promedio:      {promedio:C}");
            Console.WriteLine($"Salario Máximo:        {max:C}");
            Console.WriteLine($"Salario Mínimo:        {min:C}");

            EsperarTecla();
        }

        private void GenerarReporte()
        {
            MostrarTitulo("GENERAR REPORTE DE EMPLEADO");

            Empleado? empleado = SolicitarEmpleadoPorId();

            if (empleado != null)
            {
                ReporteEmpleado reporte = empleado.GenerarReporte();
                Console.WriteLine();
                Console.WriteLine(reporte.GenerarReporte());

            }
            EsperarTecla();
        }

        private Empleado? SolicitarEmpleadoPorId()
        {
            Console.Write("Ingresa el ID del empleado: ");
            string idTexto = Console.ReadLine() ?? "";

            if (int.TryParse(idTexto, out int id))
            {
                Empleado? empleado = GestorEmpleados.BuscarPorId(id);

                if (empleado != null)
                {
                    return empleado;
                }
                else
                {
                    Console.WriteLine($"\nNo se encontró empleado con ID {id}");
                }
            }
            else
            {
                Console.WriteLine("\nError: El ID debe ser un número válido.");
            }
            return null; // Si algo falla, devolvemos null
        }

        private void Salir()
        {
            _continuar = false;
            Console.Clear();
            Console.WriteLine("\n\nGracias por usar el Gestor de Empleados. ¡Hasta luego!\n\n");
        }

        private void EsperarTecla()
        {
            Console.Write("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void MostrarTitulo(string titulo)
        {
            Console.Clear();
            Console.WriteLine($"=== {titulo} ===\n");
        }
    }

}