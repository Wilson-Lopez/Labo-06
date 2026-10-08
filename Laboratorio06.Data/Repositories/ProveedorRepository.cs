using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Laboratorio06.Data.Connection;
using Laboratorio06.Data.Entities;

namespace Laboratorio06.Data.Repositories
{
    public class ProveedorRepository
    {
        // Listar Proveedores (Modo Desconectado)
        public DataTable ListarProveedores()
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ListarProveedores", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "Proveedores");
                return ds.Tables["Proveedores"];
            }
        }

        // Búsqueda de Proveedores por Nombre de Contacto y/o Ciudad
        public DataTable BuscarProveedores(string nombreContacto, string ciudad)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_BuscarProveedores", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NombreContacto", string.IsNullOrWhiteSpace(nombreContacto) ? (object)DBNull.Value : nombreContacto);
                cmd.Parameters.AddWithValue("@Ciudad", string.IsNullOrWhiteSpace(ciudad) ? (object)DBNull.Value : ciudad);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "Proveedores");
                return ds.Tables["Proveedores"];
            }
        }

        // Insertar Proveedor (Modo Conectado)
        public bool Insertar(Proveedor p)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CompaniaNombre", p.CompaniaNombre);
                cmd.Parameters.AddWithValue("@NombreContacto", p.NombreContacto ?? "");
                cmd.Parameters.AddWithValue("@CargoContacto", p.CargoContacto ?? "");
                cmd.Parameters.AddWithValue("@Direccion", p.Direccion ?? "");
                cmd.Parameters.AddWithValue("@Ciudad", p.Ciudad ?? "");
                cmd.Parameters.AddWithValue("@CodigoPostal", p.CodigoPostal ?? "");
                cmd.Parameters.AddWithValue("@Pais", p.Pais ?? "");
                cmd.Parameters.AddWithValue("@Telefono", p.Telefono ?? "");
                cmd.Parameters.AddWithValue("@Fax", p.Fax ?? "");

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Actualizar Proveedor (Modo Conectado)
        public bool Actualizar(Proveedor p)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProveedorID", p.ProveedorID);
                cmd.Parameters.AddWithValue("@CompaniaNombre", p.CompaniaNombre);
                cmd.Parameters.AddWithValue("@NombreContacto", p.NombreContacto ?? "");
                cmd.Parameters.AddWithValue("@CargoContacto", p.CargoContacto ?? "");
                cmd.Parameters.AddWithValue("@Direccion", p.Direccion ?? "");
                cmd.Parameters.AddWithValue("@Ciudad", p.Ciudad ?? "");
                cmd.Parameters.AddWithValue("@CodigoPostal", p.CodigoPostal ?? "");
                cmd.Parameters.AddWithValue("@Pais", p.Pais ?? "");
                cmd.Parameters.AddWithValue("@Telefono", p.Telefono ?? "");
                cmd.Parameters.AddWithValue("@Fax", p.Fax ?? "");

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Eliminar Lógicamente
        public bool EliminarLogico(int idProveedor)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProveedorID", idProveedor);

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}