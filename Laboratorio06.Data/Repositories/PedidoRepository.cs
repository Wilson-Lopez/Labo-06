using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Laboratorio06.Data.Connection;
using Laboratorio06.Data.Entities;

namespace Laboratorio06.Data.Repositories
{
    public class PedidoRepository
    {
        // Listar Pedidos
        public DataTable ListarPedidos()
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ListarPedidos", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "Pedidos");
                return ds.Tables["Pedidos"];
            }
        }

        // Insertar Pedido
        public bool Insertar(Pedido p)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarPedido", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClienteID", p.ClienteID);
                cmd.Parameters.AddWithValue("@EmpleadoID", p.EmpleadoID);
                cmd.Parameters.AddWithValue("@FechaPedido", p.FechaPedido);
                cmd.Parameters.AddWithValue("@FechaRequerida", p.FechaRequerida);
                cmd.Parameters.AddWithValue("@FechaEnvio", p.FechaEnvio);
                cmd.Parameters.AddWithValue("@TransportistaID", p.TransportistaID);
                cmd.Parameters.AddWithValue("@Destinatario", p.Destinatario ?? "");
                cmd.Parameters.AddWithValue("@CiudadDestino", p.CiudadDestino ?? "");
                cmd.Parameters.AddWithValue("@PaisDestino", p.PaisDestino ?? "");

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Actualizar Pedido
        public bool Actualizar(Pedido p)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarPedido", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PedidoID", p.PedidoID);
                cmd.Parameters.AddWithValue("@ClienteID", p.ClienteID);
                cmd.Parameters.AddWithValue("@EmpleadoID", p.EmpleadoID);
                cmd.Parameters.AddWithValue("@FechaPedido", p.FechaPedido);
                cmd.Parameters.AddWithValue("@FechaRequerida", p.FechaRequerida);
                cmd.Parameters.AddWithValue("@FechaEnvio", p.FechaEnvio);
                cmd.Parameters.AddWithValue("@TransportistaID", p.TransportistaID);
                cmd.Parameters.AddWithValue("@Destinatario", p.Destinatario ?? "");
                cmd.Parameters.AddWithValue("@CiudadDestino", p.CiudadDestino ?? "");
                cmd.Parameters.AddWithValue("@PaisDestino", p.PaisDestino ?? "");

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Eliminar Lógicamente
        public bool EliminarLogico(int idPedido)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarPedido", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PedidoID", idPedido);

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Reporte por Fechas
        public DataTable ObtenerReporteDetallePedidos(DateTime fechaInicio, DateTime fechaFin)
        {
            using (SqlConnection cn = DbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ReporteDetallePedidosPorFechas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "ReportePedidos");
                return ds.Tables["ReportePedidos"];
            }
        }
    }
}