using Page_Navigation_App;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Page_Navigation_App.View;

namespace ProyectoGym.View
{
    /// <summary>
    /// Lógica de interacción para FormCustom.xaml
    /// </summary>
    public partial class FormCustom : Window
    {
        public event Action ClienteGuardado;
        public FormCustom()
        {
            InitializeComponent();
        }

        private void btnSave(object sender, RoutedEventArgs e)
        {
            // Validar que los campos obligatorios no estén vacíos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text) ||
                !dpFechaNacimiento.SelectedDate.HasValue)
            {
                MessageBox.Show("Por favor, completa todos los campos obligatorios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Crear una nueva conexión
            ConexionDB conexionDB = new ConexionDB();

            try
            {
                // Abrir conexión
                conexionDB.AbrirConexion();

                // Consulta SQL para insertar datos
                string query = "INSERT INTO Clientes (nombre_completo, email, telefono, direccion, fecha_nacimiento, estado_cliente, fecha_registro) " +
                               "VALUES (@NombreCompleto, @Correo, @Telefono, @Direccion, @FechaNacimiento, @Estado, @FechaRegistro)";

                using (SqlCommand cmd = new SqlCommand(query, conexionDB.GetConexion()))
                {
                    // Asignar parámetros
                    cmd.Parameters.AddWithValue("@NombreCompleto", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text.Trim());
                    cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text.Trim());
                    cmd.Parameters.AddWithValue("@FechaNacimiento", dpFechaNacimiento.SelectedDate.Value);
                    cmd.Parameters.AddWithValue("@Estado", chkEstado.IsChecked == true);
                    cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                    // Ejecutar comando
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    // Verificar resultado
                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Cliente guardado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Invocar evento opcional o refrescar lista
                        ClienteGuardado?.Invoke();
                        Window.GetWindow(this)?.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar el cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el cliente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Cerrar conexión
                conexionDB.CerrarConexion();
            }
        }


        private void chkEstado_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
