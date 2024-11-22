using System;
using System.Data.SqlClient;

public class ConexionDB
{
    private SqlConnection connection;

    public ConexionDB()
    {
        string connectionString = "Server=DESKTOP-UM8BAE3\\SQLEXPRESS;Database=broGym;Integrated Security=True;TrustServerCertificate=False;";
        connection = new SqlConnection(connectionString);
    }

    public void AbrirConexion()
    {
        try
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al abrir la conexión: " + ex.Message);
        }
    }

    public void CerrarConexion()
    {
        try
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al cerrar la conexión: " + ex.Message);
        }
    }

    public SqlConnection GetConexion()
    {
        return connection;
    }
}