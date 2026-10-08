using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Laboratorio06.Data.Entities;
using Laboratorio06.Data.Repositories;

namespace Laboratorio06
{
    public partial class PedidosWindow : Window
    {
        private readonly PedidoRepository _repoPedido;

        public PedidosWindow()
        {
            InitializeComponent();
            _repoPedido = new PedidoRepository();
            dtpFechaPedido.SelectedDate = DateTime.Now;
            _ = CargarPedidosAsync();
        }

        private async Task CargarPedidosAsync()
        {
            try
            {
                DataTable dt = await Task.Run(() => _repoPedido.ListarPedidos());
                dgPedidos.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pedidos:\n{ex.Message}");
            }
        }

        private void dgPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPedidos.SelectedItem is DataRowView row)
            {
                txtIdPedido.Text = row["PedidoID"].ToString();
                txtDestinatario.Text = row["Destinatario"].ToString();
                txtCiudadDestino.Text = row["CiudadDestino"].ToString();
                txtPaisDestino.Text = row["PaisDestino"].ToString();

                if (DateTime.TryParse(row["FechaPedido"].ToString(), out DateTime fecha))
                    dtpFechaPedido.SelectedDate = fecha;
            }
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Pedido p = new Pedido
            {
                ClienteID = 1,
                EmpleadoID = 1,
                FechaPedido = dtpFechaPedido.SelectedDate ?? DateTime.Now,
                FechaRequerida = DateTime.Now.AddDays(7),
                FechaEnvio = DateTime.Now.AddDays(2),
                TransportistaID = 1,
                Destinatario = txtDestinatario.Text.Trim(),
                CiudadDestino = txtCiudadDestino.Text.Trim(),
                PaisDestino = txtPaisDestino.Text.Trim()
            };

            bool ok = await Task.Run(() => _repoPedido.Insertar(p));

            if (ok)
            {
                MessageBox.Show("Pedido registrado con éxito.");
                await CargarPedidosAsync();
                LimpiarFormulario();
            }
        }

        private async void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdPedido.Text)) return;

            Pedido p = new Pedido
            {
                PedidoID = Convert.ToInt32(txtIdPedido.Text),
                ClienteID = 1,
                EmpleadoID = 1,
                FechaPedido = dtpFechaPedido.SelectedDate ?? DateTime.Now,
                FechaRequerida = DateTime.Now.AddDays(7),
                FechaEnvio = DateTime.Now.AddDays(2),
                TransportistaID = 1,
                Destinatario = txtDestinatario.Text.Trim(),
                CiudadDestino = txtCiudadDestino.Text.Trim(),
                PaisDestino = txtPaisDestino.Text.Trim()
            };

            bool ok = await Task.Run(() => _repoPedido.Actualizar(p));

            if (ok)
            {
                MessageBox.Show("Pedido actualizado correctamente.");
                await CargarPedidosAsync();
                LimpiarFormulario();
            }
        }

        private async void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdPedido.Text)) return;

            int id = Convert.ToInt32(txtIdPedido.Text);

            if (MessageBox.Show("¿Desea eliminar lógicamente este pedido?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                bool ok = await Task.Run(() => _repoPedido.EliminarLogico(id));

                if (ok)
                {
                    MessageBox.Show("Pedido eliminado lógicamente.");
                    await CargarPedidosAsync();
                    LimpiarFormulario();
                }
            }
        }

        private void LimpiarFormulario()
        {
            txtIdPedido.Clear();
            txtDestinatario.Clear();
            txtCiudadDestino.Clear();
            txtPaisDestino.Clear();
            dtpFechaPedido.SelectedDate = DateTime.Now;
            dgPedidos.UnselectAll();
        }
    }
}