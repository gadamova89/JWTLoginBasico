using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Usuario
    {
        CD_Conexion Conexion = new CD_Conexion();

        public DataTable ValidarUsuario(string Usuario)
        {
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            comando.Connection = Conexion.AbrirConexion();
            comando.CommandText = "ValidarUsuario";
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@Usuario", Usuario);
            SqlDataReader leer = comando.ExecuteReader();
            tabla.Load(leer);
            Conexion.CerrarConexion();
            return tabla;//notar que estamos retornando una tabla en cn lo validamos
        }
        public void RegistrarUsuario(string Usuario, string PasswordHash)
        {
            SqlCommand comando = new SqlCommand();
            comando.Connection = Conexion.AbrirConexion();
            comando.CommandText = "RegistrarUsuario";
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@Usuario", Usuario);
            comando.Parameters.AddWithValue("@Passwordhash", PasswordHash);
            comando.ExecuteNonQuery();
            Conexion.CerrarConexion();

        }

    }
}
