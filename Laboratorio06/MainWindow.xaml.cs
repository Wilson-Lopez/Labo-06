using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Laboratorio06.Data.Entities;
using Laboratorio06.Data.Repositories;

namespace Laboratorio06
{
    public partial class MainWindow : Window
    {
        private readonly ProductoRepository _repoProducto;

        public MainWindow()
        {
            InitializeComponent();
            _repoProducto = new ProductoRepository();
            _ = CargarProductosAsync();
        }

        private async Task CargarProductosAsync()
        {
            try
            {
                DataTable dt = await Task.Run(() => _repoProducto.ListarProductos());
                dgProductos.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductos.SelectedItem is DataRowView row)
            {
                txtIdProducto.Text = row["ProductoID"].ToString();
                txtNombre.Text = row["NombreProducto"].ToString();
                txtPrecio.Text = row["PrecioUnidad"].ToString();
                txtStock.Text = row["UnidadesEnExistencia"].ToString();
            }
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarFormulario()) return;

            Producto nuevoProducto = new Producto
            {
                NombreProducto = txtNombre.Text.Trim(),
                IdProveedor = 1,
                IdCategoria = 1,
                CantidadPorUnidad = "1 unidad",
                PrecioUnidad = Convert.ToDecimal(txtPrecio.Text),
                UnidadesEnExistencia = Convert.ToInt16(txtStock.Text)
            };

            bool ok = await Task.Run(() => _repoProducto.Insertar(nuevoProducto));

            if (ok)
            {
                MessageBox.Show("Producto registrado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                await CargarProductosAsync();
                LimpiarFormulario();
            }
            else
            {
                MessageBox.Show("No se pudo guardar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdProducto.Text))
            {
                MessageBox.Show("Seleccione un producto de la tabla para editar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidarFormulario()) return;

            Producto productoEditar = new Producto
            {
                IdProducto = Convert.ToInt32(txtIdProducto.Text),
                NombreProducto = txtNombre.Text.Trim(),
                IdProveedor = 1,
                IdCategoria = 1,
                CantidadPorUnidad = "1 unidad",
                PrecioUnidad = Convert.ToDecimal(txtPrecio.Text),
                UnidadesEnExistencia = Convert.ToInt16(txtStock.Text)
            };

            bool ok = await Task.Run(() => _repoProducto.Actualizar(productoEditar));

            if (ok)
            {
                MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                await CargarProductosAsync();
                LimpiarFormulario();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdProducto.Text))
            {
                MessageBox.Show("Seleccione un producto de la tabla para eliminar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = Convert.ToInt32(txtIdProducto.Text);

            var confirm = MessageBox.Show($"¿Desea eliminar lógicamente el producto con ID {id}?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                bool ok = await Task.Run(() => _repoProducto.EliminarLogico(id));

                if (ok)
                {
                    MessageBox.Show("Producto eliminado lógicamente (Activo = 0).", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    await CargarProductosAsync();
                    LimpiarFormulario();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el registro.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Navegación a las otras ventanas requeridas
        private void btnProveedores_Click(object sender, RoutedEventArgs e)
        {
            ProveedoresWindow ventanaProveedores = new ProveedoresWindow();
            ventanaProveedores.Owner = this;
            ventanaProveedores.ShowDialog();
        }

        private void btnPedidos_Click(object sender, RoutedEventArgs e)
        {
            PedidosWindow ventanaPedidos = new PedidosWindow();
            ventanaPedidos.Owner = this;
            ventanaPedidos.ShowDialog();
        }

        private void btnAbrirReporte_Click(object sender, RoutedEventArgs e)
        {
            ReportePedidosWindow ventanaReporte = new ReportePedidosWindow();
            ventanaReporte.Owner = this;
            ventanaReporte.ShowDialog();
        }

        private void LimpiarFormulario()
        {
            txtIdProducto.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            dgProductos.UnselectAll();
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingrese el nombre del producto.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text, out _))
            {
                MessageBox.Show("Ingrese un precio unitario válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!short.TryParse(txtStock.Text, out _))
            {
                MessageBox.Show("Ingrese un número válido de unidades en stock.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}