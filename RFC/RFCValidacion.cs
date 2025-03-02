using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Validación.RFC
{
    /// <summary>
    /// Validación del RFC
    /// </summary>
    public static class RFCValidacion
    {
        public const string RFCRegex = @"^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A\d])";

        /// <summary>
        /// Valida un RFC el cual verifica que cumpla con el formato de RFC <br/>
        /// y el digito verificador sea correcto
        /// </summary>
        /// <param name="rfc">El RFC a validar</param>
        /// <param name="aceptarGenerico">Indica si se deberia aceptar un RFC generico</param>
        /// <returns>Regresa <see cref="RFCSalida.Fisica"/> o <see cref="RFCSalida.Moral"/> en caso de que el RFC sea valido, <br/>
        /// de caso contrario regresara <see cref="RFCSalida.Error"/>
        /// </returns>
        public static RFCSalida ValidarRFC(string rfc, bool aceptarGenerico = true)
        {
            Match rfcMatch = Regex.Match(rfc, RFCRegex);

            //Si no cumple con el formato de RFC, regresa error
            if (!rfcMatch.Success)
                return RFCSalida.Error;

            //Separa el digito verificador del RFC  
            int digitoVerificador = int.Parse(rfcMatch.Groups[4].Value[0].ToString());
            string rfcSinDigito = rfcMatch.Groups[1].Value + rfcMatch.Groups[2].Value + rfcMatch.Groups[3].Value;

            const string Diccionario = "0123456789ABCDEFGHIJKLMN&OPQRSTUVWXYZ Ñ";
            int lenghtRFC = rfcSinDigito.Length;
            int suma = lenghtRFC == 12 ? 0 : 481;

            for (int i = 0; i < lenghtRFC; i++)
            {
                suma += Diccionario.IndexOf(rfcSinDigito[i]) * (lenghtRFC + 1 - i);
            }

            int digitoEsperado = 11 - suma % 11;
            digitoEsperado = digitoEsperado == 11 ? 0 : digitoEsperado == 10 ? 'A' : digitoEsperado;

            if ((digitoEsperado != digitoVerificador) && (!aceptarGenerico || (rfcSinDigito + digitoVerificador.ToString() != "XAXX010101000")))
                return RFCSalida.Error;
            else if (!aceptarGenerico || ((rfcSinDigito + digitoVerificador.ToString()) == "XEXX010101000"))
                return RFCSalida.Error;

            return lenghtRFC == 12 ? RFCSalida.Fisica : RFCSalida.Moral;
        }

        /// <summary>
        /// Salida de la validacion de un RFC
        /// </summary>
        public enum RFCSalida
        {
            /// <summary>
            /// Indica que el RFC no es valido
            /// </summary>
            Error,
            /// <summary>
            /// Indica que el RFC es de una persona fisica
            /// </summary>
            Fisica,
            /// <summary>
            /// Indica que el RFC es de una persona moral
            /// </summary>
            Moral
        }
    }
}
