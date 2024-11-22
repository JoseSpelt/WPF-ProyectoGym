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
using ProyectoGym.Model;
using ProyectoGym.View;

namespace Page_Navigation_App.View
{

    public partial class Customers : UserControl
    {
        public Customers()
        {
            InitializeComponent();
            Refresh();
        }

        private async void Refresh()
        {
            List<CustomerModel> lst = new List<CustomerModel>();
            ConexionDB conexionDB = new ConexionDB();

            try
            {
                conexionDB.AbrirConexion();
                using (SqlCommand cmd = new SqlCommand("SELECT id_cliente, nombre_completo, telefono, email, fecha_nacimiento, sexo, estado_cliente, plan_membresia FROM Clientes", conexionDB.GetConexion()))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            CustomerModel customer = new CustomerModel
                            {
                                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Telefono = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                FechaNacimiento = reader.IsDBNull(4) ? default(DateTime) : reader.GetDateTime(4),
                                Sexo = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                Estado = reader.IsDBNull(6) ? "Inactivo" : (reader.GetString(6).ToLower() == "1" || reader.GetString(6).ToLower() == "activo" ? "Activo" : "Inactivo"),

                                Membresia = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
                            };

                            lst.Add(customer);
                        }
                    }
                }

                Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = lst;
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Error al cargar los datos: " + ex.Message);
                });
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNew(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new ProyectoGym.View.FormCustom();
            newUserWindow.ClienteGuardado += Refresh;
            newUserWindow.ShowDialog();
        }

        private void btnDelete(object sender, RoutedEventArgs e)
        {
            int Id = (int)((Button)sender).CommandParameter;

            var result = MessageBox.Show("¿Estás seguro de que deseas eliminar este cliente?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ConexionDB conexionDB = new ConexionDB();

                try
                {
                    conexionDB.AbrirConexion();

                    string query = "DELETE FROM Clientes WHERE id_cliente = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, conexionDB.GetConexion()))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);

                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Cliente eliminado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                            Refresh(); 
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el cliente a eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el cliente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    conexionDB.CerrarConexion();
                }
            }
        }

        private void btnUpdate(object sender, RoutedEventArgs e)
        {
            int Id = (int)((Button)sender).CommandParameter;

            var editWindow = new EditCustomWindow(Id); 
            editWindow.Title = "Actualizar Cliente";

            ConexionDB conexionDB = new ConexionDB();

            try
            {
                conexionDB.AbrirConexion();

                
                string query = "SELECT nombre_completo, email, telefono, direccion, fecha_nacimiento, estado_cliente FROM Clientes WHERE id_cliente = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conexionDB.GetConexion()))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            editWindow.txtNombre.Text = reader.GetString(0);
                            editWindow.txtCorreo.Text = reader.GetString(1);
                            editWindow.txtTelefono.Text = reader.GetString(2);
                            editWindow.txtDireccion.Text = reader.GetString(3);
                            editWindow.dpFechaNacimiento.SelectedDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);

                            
                            string estado = reader.GetString(5); 

                            
                            if (estado == "1" || estado.ToLower() == "activo")
                            {
                                editWindow.chkEstado.IsChecked = true;  
                            }
                            else
                            {
                                editWindow.chkEstado.IsChecked = false; 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos del cliente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            finally
            {
                conexionDB.CerrarConexion();
            }

            if (editWindow.ShowDialog() == true)
            {
                
                if (string.IsNullOrWhiteSpace(editWindow.txtNombre.Text) || string.IsNullOrWhiteSpace(editWindow.txtCorreo.Text) ||
                    string.IsNullOrWhiteSpace(editWindow.txtTelefono.Text) || string.IsNullOrWhiteSpace(editWindow.txtDireccion.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    conexionDB.AbrirConexion();

                    
                    string estado = (editWindow.chkEstado.IsChecked ?? false) ? "1" : "0";

                    
                    string updateQuery = "UPDATE Clientes SET nombre_completo = @Nombre, email = @Correo, telefono = @Telefono, direccion = @Direccion, fecha_nacimiento = @FechaNacimiento, estado_cliente = @Estado WHERE id_cliente = @Id";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conexionDB.GetConexion()))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.Parameters.AddWithValue("@Nombre", editWindow.txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@Correo", editWindow.txtCorreo.Text.Trim());
                        cmd.Parameters.AddWithValue("@Telefono", editWindow.txtTelefono.Text.Trim());
                        cmd.Parameters.AddWithValue("@Direccion", editWindow.txtDireccion.Text.Trim());
                        cmd.Parameters.AddWithValue("@FechaNacimiento", editWindow.dpFechaNacimiento.SelectedDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Estado", estado); 

                        
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Cliente actualizado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                            Refresh(); 
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
        }

    }
}