using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoGym.Services
{
    class LoginServices
    {
        private ConexionDB conexion;

        public LoginServices()
        {
            conexion = new ConexionDB();
        }

        public bool VerificarCredenciales(string nombreUsuario, string contrasena)
        {
            try
            {
                conexion.AbrirConexion();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM usuarios WHERE nombre_usuario = @nombreUsuario AND contrasena = @contrasena", conexion.GetConexion());
                cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar las credenciales: " + ex.Message);
            }
            finally
            {
                conexion.CerrarConexion();
            }
        }
    }
}
