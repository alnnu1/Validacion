using System.Net.Mail;

namespace Validacion
{
    public static class Email
    {
        /// <summary>
        /// Valida si un email sigue con el formato correcto, no verifica 
        /// <br/>si el dominio existe o si el email es valido
        /// </summary>
        /// <param name="email">El email a validar</param>
        /// <returns><see langword="true"/> si el email es valido, de otra forma <see langword="false"/></returns>
        public static bool ValidarEmail(string email)
        {
            email = email.Trim();

            if (email.EndsWith(".") || string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return new MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
