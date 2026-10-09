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
                // 1. Generar el hash de la contraseña
                string hash = BCrypt.Net.BCrypt.HashPassword(Password);

                // 2. Guardar en la BD (DAL)
                ObjetoCD.RegistrarUsuario(Usuario, hash);

                // 3. Si llegó aquí, todo salió bien
                return true;
            }
            catch
            {
                // Si falla (ej: usuario duplicado, error de BD), retornar false
                return false;
            }
        }
    }
}
