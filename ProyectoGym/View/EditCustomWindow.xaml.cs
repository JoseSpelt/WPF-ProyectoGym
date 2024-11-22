using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Data.SqlClient;
using System.Windows;

namespace ProyectoGym.View
{
    public partial class EditCustomWindow : Window
    {
        private int clienteId;

        public EditCustomWindow(int id)
        {
            InitializeComponent();
            clienteId = id;
            CargarDatosCliente();
        }

        private void CargarDatosCliente()
        {
            ConexionDB conexionDB = new ConexionDB();

            try
            {
                conexionDB.AbrirConexion();
                string query = "SELECT nombre_completo, email, telefono, direccion, fecha_nacimiento, estado_cliente FROM Clientes WHERE id_cliente = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conexionDB.GetConexion()))
                {
                    cmd.Parameters.AddWithValue("@Id", clienteId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            txtNombre.Text = reader.GetString(0);
                            txtCorreo.Text = reader.GetString(1);
                            txtTelefono.Text = reader.GetString(2);
                            txtDireccion.Text = reader.GetString(3);
                            dpFechaNacimiento.SelectedDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);

                            string estado = reader.GetString(5);

                            
                            if (estado == "1" || estado.ToLower() == "activo")
                            {
                                chkEstado.IsChecked = true; 
                            }
                            else
                            {
                                chkEstado.IsChecked = false; 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos del cliente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conexionDB.CerrarConexion();
            }
        }




        private void btnSave(object sender, RoutedEventArgs e)
        {
            ConexionDB conexionDB = new ConexionDB();

            try
            {
                conexionDB.AbrirConexion();

                string updateQuery = "UPDATE Clientes SET nombre_completo = @Nombre, email = @Correo, telefono = @Telefono, direccion = @Direccion, fecha_nacimiento = @FechaNacimiento, estado_cliente = @Estado WHERE id_cliente = @Id";


                using (SqlCommand cmd = new SqlCommand(updateQuery, conexionDB.GetConexion()))
                {
                    cmd.Parameters.AddWithValue("@Id", clienteId); 
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text.Trim());
                    cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text.Trim());
                    cmd.Parameters.AddWithValue("@FechaNacimiento", dpFechaNacimiento.SelectedDate ?? (object)DBNull.Value); 
                    cmd.Parameters.AddWithValue("@Estado", chkEstado.IsChecked ?? false);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el cliente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conexionDB.CerrarConexion();
            }
        }



        private void btnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
