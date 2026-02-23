using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEmpleados
{
    internal record ReporteEmpleado(int Id, string Nombre, string Email, string Rfc, string Nss, decimal Salario, DateTime FechaContratacion, string AntiguedadAnios, EstadoEmpleado Estado)
    {
        public string GenerarReporte()
        {
            return $"""
                ****************
                REPORTE EMPLEADO
                ****************

                ID:                 {Id}
                Nombre:             {Nombre}
                Email:              {Email}
                RFC:                {Rfc}
                NSS:                {Nss}
                Salario:            {Salario:C}
                Fecha Contratación: {FechaContratacion:dd/MM/yyyy}
                Antigüedad:         {AntiguedadAnios}
                Estado:             {Estado}  
                """;
        }
    }
}
