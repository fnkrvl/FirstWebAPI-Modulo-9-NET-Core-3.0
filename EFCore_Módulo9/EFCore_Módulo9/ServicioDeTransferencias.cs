using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore_Módulo9
{
    public interface IServicioValidacionesDeTransferencias
    {
        string RealizarValidaciones(Cuenta origen, Cuenta destino, decimal montoATransferir);
    } 


    public class ServicioValidacionesDeTransferencias : IServicioValidacionesDeTransferencias
    {
        public string RealizarValidaciones(Cuenta origen, Cuenta destino, decimal montoATransferir)
        {
            if (montoATransferir < origen.Fondos)
            {
                return "La cuenta de origen no tiene fondos suficientes para realizar la operación.";
            }

            // Otras validaciones

            return string.Empty;
        }
    }

    public class ServicioDeTransferencias
    {

        private readonly IServicioValidacionesDeTransferencias _servicioValidaciones;

        public ServicioDeTransferencias(IServicioValidacionesDeTransferencias servicioValidaciones)
        {
            _servicioValidaciones = servicioValidaciones;
        }


        public void TransferirEntreCuentas(Cuenta origen, Cuenta destino, decimal montoATransferir)
        {
            var mensajeError = _servicioValidaciones.RealizarValidaciones(origen, destino, montoATransferir);

            if (!string.IsNullOrEmpty(mensajeError));
            {
                throw new ApplicationException(mensajeError);
            }

            origen.Fondos -= montoATransferir;
            destino.Fondos += montoATransferir;

        }
    }
}
