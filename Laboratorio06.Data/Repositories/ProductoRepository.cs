using System.Data;
using Microsoft.Data.SqlClient;
using Laboratorio06.Data.Connection;
using Laboratorio06.Data.Entities;

namespace Laboratorio06.Data.Repositories
{
    public class ProductoRepository
    {
        public DataTable ListarProductos()
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ListarProductos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "Productos");
                return ds.Tables["Productos"];
            }
        }

        public bool Insertar(Producto p)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreProducto", p.NombreProducto);
                cmd.Parameters.AddWithValue("@ProveedorID", p.IdProveedor);
                cmd.Parameters.AddWithValue("@CategoriaID", p.IdCategoria);
                cmd.Parameters.AddWithValue("@CantidadPorUnidad", (object)p.CantidadPorUnidad ?? System.DBNull.Value);
                cmd.Parameters.AddWithValue("@PrecioUnidad", p.PrecioUnidad);
                cmd.Parameters.AddWithValue("@UnidadesEnExistencia", p.UnidadesEnExistencia);

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Actualizar(Producto p)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoID", p.IdProducto);
                cmd.Parameters.AddWithValue("@NombreProducto", p.NombreProducto);
                cmd.Parameters.AddWithValue("@ProveedorID", p.IdProveedor);
                cmd.Parameters.AddWithValue("@CategoriaID", p.IdCategoria);
                cmd.Parameters.AddWithValue("@CantidadPorUnidad", (object)p.CantidadPorUnidad ?? System.DBNull.Value);
                cmd.Parameters.AddWithValue("@PrecioUnidad", p.PrecioUnidad);
                cmd.Parameters.AddWithValue("@UnidadesEnExistencia", p.UnidadesEnExistencia);

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool EliminarLogico(int idProducto)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Debe coincidir con @ProductoID del SP
                cmd.Parameters.AddWithValue("@ProductoID", idProducto);

                cn.Open();
                int filasAfectadas = cmd.ExecuteNonQuery();

                // Retorna true solo si realmente modificó 1 o más filas en la BD
                return filasAfectadas > 0;
            }
        }
    }
}