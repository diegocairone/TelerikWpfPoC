using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPoC_2
{
    class Usuario
    {
        private static Usuario usuario = null;

        private Usuario() {}

        public static Usuario GetInstance()
        {
            if(usuario == null)
            {
                usuario = new Usuario();
            }

            return usuario;
        }

        public String NombreUsuario { get; set; }
        public String Password { get; set; }

        public String GetEncodedAuthToken()
        {
            if (NombreUsuario != null && Password != null)
            {
                String token = NombreUsuario + ":" + Password;

                String encoded = System
                    .Convert
                    .ToBase64String(System.Text.Encoding.GetEncoding(name: "ISO-8859-1")
                    .GetBytes(token));

                return encoded;
            }

            return null;
        }
    }
}
