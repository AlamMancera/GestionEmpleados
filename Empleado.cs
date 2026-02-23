using System.Text.RegularExpressions;

namespace GestionEmpleados
{
    public enum EstadoEmpleado
    {
        Activo,
        Inactivo
    }

    internal class Empleado
    {
        // Campos privados
        private string _nombre;
        private string _email;
        private string _rfc;
        private string _nss;
        private decimal _salario;

        // Constantes privadas
        private const decimal SalarioMinimoLegal = 536.62m; // En USD

        // Campo readonly - solo se asigna en constructor
        private readonly DateTime _fechaContratacion;

        // ID único e inmutable
        public readonly int Id;

        // Estado del empleado
        private EstadoEmpleado _estado;

        // Constructor
        public Empleado(string nombre, string email, string rfc, string nss, decimal salario)
        {
            Id = GestorEmpleados.GenerarNuevoId();

            // Asignar usando las propiedades (para que se ejecuten las validaciones)
            Nombre = nombre;
            Email = email;
            Rfc = rfc;
            Nss = nss;
            Salario = salario;

            // Asignar fecha de contratación (inmutable)
            _fechaContratacion = DateTime.Today;

            // Todo empleado nuevo inicia activo
            _estado = EstadoEmpleado.Activo;
        }

        // MÉTODOS PÚBLICOS ESTÁTICOS DE VALIDACIÓN
        // Estos métodos permiten validar los datos ANTES de crear un empleado

        public static bool ValidarNombre(string nombre, out string nombreSanitizado, out string mensajeError)
        {
            nombreSanitizado = "";
            mensajeError = "";

            // Primera validación: no vacío 
            if (string.IsNullOrWhiteSpace(nombre))
            {
                mensajeError = "El nombre no puede estar vacío";
                return false;
            }

            // Segunda validación: Sanitización  
            nombreSanitizado = nombre.Trim();

            // Tercera validación: longitud mínima
            if (nombreSanitizado.Length < 3)
            {
                mensajeError = "El nombre debe tener al menos 3 caracteres";
                return false;
            }

            // Si pasa todas las validaciones devolvemos true
            return true;
        }

        public static bool ValidarEmail(string email, out string emailSanitizado, out string mensajeError)
        {
            mensajeError = "";
            emailSanitizado = "";

            // Primera validación: no vacío
            if (string.IsNullOrWhiteSpace(email))
            {
                mensajeError = "El email no puede estar vacío";
                return false;
            }

            // Segunda validación: sanitización
            emailSanitizado = email.Trim().ToLower();

            // Validación con regex: formato email básico 
            const string patronEmail = @"^[A-Za-z0-9._-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

            if (!Regex.IsMatch(emailSanitizado, patronEmail))
            {
                mensajeError = "El formato del email no es válido";
                return false;
            }

            // Si pasa todas las validaciones devolvemos true
            return true;
        }

        // RFC del empleado (formato México: 13 caracteres)
        public static bool ValidarRfc(string rfc, out string rfcSanitizado, out string mensajeError)
        {
            mensajeError = "";
            rfcSanitizado = "";

            // Primera validación: no vacío
            if (string.IsNullOrWhiteSpace(rfc))
            {
                mensajeError = "El RFC no puede estar vacío";
                return false;
            }

            // Segunda validación: sanitización
            rfcSanitizado = rfc.Trim().ToUpper();

            // Tercera validación: longitud exacta
            if (rfcSanitizado.Length != 13)
            {
                mensajeError = "El RFC debe tener exactamente 13 caracteres";
                return false;
            }

            // Validación con regex: 4 letras + 6 dígitos + 3 caracteres alfanuméricos 
            const string patronRfc = @"^[A-ZÑ&]{4}\d{6}[A-Z0-9]{3}$";

            if (!Regex.IsMatch(rfcSanitizado, patronRfc))
            {
                mensajeError = "El RFC debe tener el formato: AAAA######XXX (4 letras, 6 dígitos, 3 alfanuméricos)";
                return false;
            }

            // Si pasa todas las validaciones devolvemos true
            return true;
        }

        // Número de Seguro Social (11 dígitos)
        public static bool ValidarNss(string nss, out string nssSanitizado, out string mensajeError)
        {
            mensajeError = "";
            nssSanitizado = "";

            // Primera validación: no vacío
            if (string.IsNullOrWhiteSpace(nss))
            {
                mensajeError = "El NSS no puede estar vacío";
                return false;
            }

            // Segunda validación: sanitización
            nssSanitizado = nss.Trim();

            // Validación con regex: 11 dígitos 
            const string patronNss = @"^\d{11}$";

            if (!Regex.IsMatch(nssSanitizado, patronNss))
            {
                mensajeError = $"El NSS debe tener exactamente 11 dígitos. Se recibió: '{nssSanitizado}' ({nssSanitizado.Length} dígitos)";
                return false;
            }

            // Si pasa todas las validaciones devolvemos true
            return true;
        }

        public static bool ValidarSalario(decimal salario, out string mensajeError)
        {
            mensajeError = "";

            // Primera validación: menor a cero
            if (salario <= 0)
            {
                mensajeError = "El salario debe ser mayor a cero";
                return false;
            }

            // Segunda validación: no supere los $1000
            if (salario > 1000)
            {
                mensajeError = "El salario no puede exceder $1,000.00 USD";
                return false;
            }

            // Tercera validación: superior al mínimo
            if (salario < SalarioMinimoLegal)
            {
                mensajeError = $"El salario {salario:N2} es menor al mínimo legal para 2026: {SalarioMinimoLegal:N2}";
                return false;
            }

            // Si pasa todas las validaciones devolvemos true
            return true;
        }

        // PROPIEDADES PÚBLICAS
        public string Nombre
        {
            get => _nombre;
            set
            {
                // Si el nombre no es válido, lanzamos excepción
                if (!ValidarNombre(value, out string nombreSanitizado, out string mensajeError))
                {
                    throw new ArgumentException(mensajeError);
                }
                else
                {
                    // Asignar valor si el método de validación devuelve true
                    _nombre = nombreSanitizado;
                }


            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (!ValidarEmail(value, out string emailSanitizado, out string mensajeError))
                {
                    throw new ArgumentException(mensajeError);
                }
                else
                {
                    _email = emailSanitizado;
                }
            }
        }

        public string Rfc
        {
            get => _rfc;
            set
            {
                if (!ValidarRfc(value, out string rfcSanitizado, out string mensajeError))
                {
                    throw new ArgumentException(mensajeError);
                }
                else
                {
                    _rfc = rfcSanitizado;
                }
            }
        }

        public string Nss
        {
            get => _nss;
            set
            {
                if (!ValidarNss(value, out string nssSanitizado, out string mensajeError))
                {
                    throw new ArgumentException(mensajeError);
                }
                else
                {
                    _nss = nssSanitizado;
                }
            }
        }

        public decimal Salario
        {
            get => _salario;
            set
            {
                if (!ValidarSalario(value, out string mensajeError))
                {
                    throw new ArgumentException(mensajeError);
                }
                else
                {
                    _salario = value;
                }
            }
        }

        // Fecha en que el empleado fue contratado (solo lectura)
        public DateTime FechaContratacion => _fechaContratacion;

        // Estado actual del empleado (solo lectura)    
        public EstadoEmpleado Estado => _estado;

        // MÉTODOS
        // Calcula el tiempo que los empleados llevan en la empresa
        public string CalcularAntiguedad()
        {
            DateTime hoy = DateTime.Today;

            // Calcular años, meses y días
            int años = hoy.Year - _fechaContratacion.Year; // años valdría 1 (2026-2025)
            int meses = hoy.Month - _fechaContratacion.Month; // meses valdría -7 (3-10)
            int dias = hoy.Day - _fechaContratacion.Day; // días valdría -5 (20-25)

            // Si los días son negativos, "pedimos prestado" 1 mes
            if (dias < 0)
            {
                meses--; // meses ya valdría -8 (-7-1)

                DateTime mesAnterior = hoy.AddMonths(-1); // Si hoy es 20 de marzo, mesAnterior sería 20 de febrero
                dias += DateTime.DaysInMonth(mesAnterior.Year, mesAnterior.Month); // 23 días
            }

            // Si los meses son negativos, "pedimos prestado" 1 año
            if (meses < 0)
            {
                años--; // años ya valdría 0 (1-1)
                meses += 12; // meses ya valdría 4 (-8 + 12)
            }

            // Por si alguien consulta el mismo día que se contrató
            if (años < 0)
            {
                años = 0;
            }

            // Devolvemos la cadena formateada
            return $"{años} años, {meses} meses y {dias} días";
        }

        // Aplica un aumento porcentual al salario del empleado
        public void AplicarAumento(decimal porcentaje)
        {
            if (_estado == EstadoEmpleado.Inactivo)
            {
                throw new InvalidOperationException("No se puede aplicar un aumento a un empleado inactivo");
            }

            if (porcentaje <= 0)
            {
                throw new ArgumentException("El porcentaje de aumento debe ser mayor a cero");
            }

            if (porcentaje > 100)
            {
                throw new ArgumentException("El porcentaje de aumento no puede ser mayor a 100%");
            }

            // Calculamos el aumento y lo aplicamos
            decimal aumento = _salario * (porcentaje / 100);
            Salario = _salario + aumento; // Usa la propiedad para re-validar
        }

        // Método para suspender al empleado
        public void Suspender()
        {
            if (_estado == EstadoEmpleado.Inactivo)
            {
                throw new InvalidOperationException("El empleado ya está inactivo");
            }
            _estado = EstadoEmpleado.Inactivo;
        }

        // Método para reactivar al empleado
        public void Reactivar()
        {
            if (_estado == EstadoEmpleado.Activo)
            {
                throw new InvalidOperationException("El empleado ya está activo");
            }
            _estado = EstadoEmpleado.Activo;
        }

        public ReporteEmpleado GenerarReporte()
        {
            //Instanciamos a ReporteEmpleado y asignamos las propiedades
            ReporteEmpleado reporte = new ReporteEmpleado(Id, Nombre, Email, Rfc, Nss, Salario, FechaContratacion, CalcularAntiguedad(), Estado);

            return reporte;
        }

        // Representación en texto de los datos del empleado
        public override string ToString()
        {
            return $"[{Id}] {Nombre} - {Email} - {Estado} - {Salario:C}";
        }
    }
}
