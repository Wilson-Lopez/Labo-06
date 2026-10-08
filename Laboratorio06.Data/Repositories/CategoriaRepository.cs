using System.Data;
using Microsoft.Data.SqlClient;
using Laboratorio06.Data.Connection;
using Laboratorio06.Data.Entities;

namespace Laboratorio06.Data.Repositories
{
    public class CategoriaRepository
    {
        // MODO DESCONECTADO (DataSet) para consultas
        public DataTable ListarCategorias()
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ListarCategorias", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "Categorias");

                return ds.Tables["Categorias"];
            }
        }

        // MODO CONECTADO (ExecuteNonQuery) para Insertar
        public bool Insertar(Categoria c)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombrecategoria", c.NombreCategoria);
                cmd.Parameters.AddWithValue("@descripcion", (object)c.Descripcion ?? System.DBNull.Value);

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // MODO CONECTADO (ExecuteNonQuery) para Actualizar
        public bool Actualizar(Categoria c)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idcategoria", c.IdCategoria);
                cmd.Parameters.AddWithValue("@nombrecategoria", c.NombreCategoria);
                cmd.Parameters.AddWithValue("@descripcion", (object)c.Descripcion ?? System.DBNull.Value);

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // MODO CNECTADO (ExecuteNonQuery) para Baja Lógica
        public bool EliminarLogico(int idCategoria)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idcategoria", idCategoria);

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}