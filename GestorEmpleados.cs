using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEmpleados
{
    internal static class GestorEmpleados
    {
        // Lista para almacenar empleados
        private static readonly List<Empleado> _empleados = new();

        // Contador para asignar IDs únicos
        private static int _contadorIds = 0;

        // Propiedad para obtener el total de empleados
        public static int TotalEmpleados => _empleados.Count;

        // MÉTODOS
        // Agregar empleado
        public static void AgregarEmpleado(Empleado empleado)
        {
            _empleados.Add(empleado);
        }

        // Genera un nuevo ID único para un empleado
        public static int GenerarNuevoId()
        {
            _contadorIds++;
            return _contadorIds;
        }

        // Obtener todos los empleados
        public static List<Empleado> ObtenerTodos()
        {
            // Crear una nueva lista (copia de la original) para evitar modificaciones externas
            List<Empleado> copiaEmpleados = new(_empleados);
            return copiaEmpleados;
        }

        public static Empleado? BuscarPorId(int id)
        {
            foreach (Empleado empleado in _empleados)
            {
                if (empleado.Id == id)
                {
                    return empleado;
                }
            }
            return null; // No encontrado
        }

        // Buscar por nombre
        public static List<Empleado> BuscarPorNombre(string nombreBuscado)
        {
            // Creamos una lista para los resultados (temporal y que solo vive en este método)
            List<Empleado> resultadosPorNombre = new();

            foreach (Empleado empleado in _empleados)
            {
                // Contains sí lo conocen (es de string)
                if (empleado.Nombre.ToLower().Contains(nombreBuscado.ToLower()))
                {
                    resultadosPorNombre.Add(empleado);
                }
            }
            return resultadosPorNombre;
        }

        // Obtener empleados activos
        public static List<Empleado> ObtenerActivos()
        {
            // Lista para empleados activos (temporal) que solo vive en este método
            List<Empleado> empleadosActivos = new();

            foreach (Empleado empleado in _empleados)
            {
                if (empleado.Estado == EstadoEmpleado.Activo)
                {
                    empleadosActivos.Add(empleado);
                }
            }
            return empleadosActivos;
        }

        // Obtener empleados inactivos
        public static List<Empleado> ObtenerInactivos()
        {
            // Lista para empleados inactivos (temporal) que solo vive en este método
            List<Empleado> empleadosInactivos = new();

            foreach (Empleado empleado in _empleados)
            {
                if (empleado.Estado == EstadoEmpleado.Inactivo)
                {
                    empleadosInactivos.Add(empleado);
                }
            }
            return empleadosInactivos;
        }

        // Calcular nómina solo activos
        public static decimal CalcularNominaActivos()
        {
            decimal totalNominaActivos = 0;

            foreach (Empleado empleado in _empleados)
            {
                if (empleado.Estado == EstadoEmpleado.Activo)
                {
                    totalNominaActivos += empleado.Salario;
                }
            }
            return totalNominaActivos;
        }

        // Obtener estadísticas
        public static void ObtenerEstadisticasSalariales(out decimal promedio, out decimal maximo, out decimal minimo)
        {
            if (_empleados.Count == 0)
            {
                // Las tres variables se les asigna 0
                promedio = maximo = minimo = 0;
                return;
            }
            // Inicializar con sueldo del primer empleado de la lista
            maximo = _empleados[0].Salario;
            minimo = _empleados[0].Salario;

            decimal nomina = 0;

            // Recorrer todos los empleados
            foreach (Empleado empleado in _empleados)
            {
                // Acumular salarios
                nomina += empleado.Salario;

                // Actualizar máximo
                if (empleado.Salario > maximo)
                {
                    maximo = empleado.Salario;
                }
                // Actualizar mínimo
                if (empleado.Salario < minimo)
                {
                    minimo = empleado.Salario;
                }
            }
            promedio = nomina / _empleados.Count;
        }
    }
}

