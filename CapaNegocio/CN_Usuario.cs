using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        public bool ValidarUsuario(string Usuario, string Password)
        {
            CD_Usuario ObjetoCD = new CD_Usuario();

            DataTable tabla = new DataTable();

            tabla = ObjetoCD.ValidarUsuario(Usuario);

            if (tabla.Rows.Count == 0)
            {
                return false;
            }

            string HashBD = Convert.ToString(tabla.Rows[0]["PasswordHash"]);

            bool EsValido = BCrypt.Net.BCrypt.Verify(Password, HashBD);

            return EsValido;

        }

        public bool RegistrarUsuario(string Usuario, string Password)
        {
            CD_Usuario ObjetoCD = new CD_Usuario();
            try
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(Password);
                ObjetoCD.RegistrarUsuario(Usuario, hash);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
