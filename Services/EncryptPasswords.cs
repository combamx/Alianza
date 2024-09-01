using System.Security.Cryptography;
using System.Text;

namespace Alianza.Services
{
    public static class EncryptPasswords
    {
        public static string EncryptPassword ( string password )
        {
            if (string.IsNullOrEmpty ( password ))
            {
                throw new ArgumentException ( "La contraseña no puede estar vacía." );
            }

            // Crear una instancia de SHA256
            using (SHA256 sha256 = SHA256.Create ( ))
            {
                // Convertir la contraseña en un array de bytes
                byte [] bytes = Encoding.UTF8.GetBytes ( password );

                // Calcular el hash de la contraseña
                byte [] hash = sha256.ComputeHash ( bytes );

                // Convertir el array de bytes en una cadena hexadecimal
                StringBuilder builder = new StringBuilder ( );
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append ( hash [ i ].ToString ( "x2" ) );
                }

                return builder.ToString ( ); // Devolver la contraseña encriptada
            }
        }
    }
}
