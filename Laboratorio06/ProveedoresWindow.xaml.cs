using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Laboratorio06.Data.Entities;
using Laboratorio06.Data.Repositories;

namespace Laboratorio06
{
    public partial class ProveedoresWindow : Window
    {
        private readonly ProveedorRepository _repoProveedor;

        public ProveedoresWindow()
        {
            InitializeComponent();
            _repoProveedor = new ProveedorRepository();
            _ = CargarProveedoresAsync();
        }

        private async Task CargarProveedoresAsync()
        {
            try
            {
                DataTable dt = await Task.Run(() => _repoProveedor.ListarProveedores());
                dgProveedores.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proveedores:\n{ex.Message}");
            }
        }

        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string contacto = txtBuscarContacto.Text.Trim();
                string ciudad = txtBuscarCiudad.Text.Trim();

                DataTable dt = await Task.Run(() => _repoProveedor.BuscarProveedores(contacto, ciudad));
                dgProveedores.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la búsqueda:\n{ex.Message}");
            }
        }

        private async void btnLimpiarFiltro_Click(object sender, RoutedEventArgs e)
        {
            txtBuscarContacto.Clear();
            txtBuscarCiudad.Clear();
            await CargarProveedoresAsync();
        }

        private void dgProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProveedores.SelectedItem is DataRowView row)
            {
                txtIdProveedor.Text = row["ProveedorID"].ToString();
                txtCompania.Text = row["CompaniaNombre"].ToString();
                txtContacto.Text = row["NombreContacto"].ToString();
                txtCiudad.Text = row["Ciudad"].ToString();
                txtTelefono.Text = row["Telefono"].ToString();
                txtPais.Text = row["Pais"].ToString();
            }
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCompania.Text))
            {
                MessageBox.Show("El nombre de la compañía es obligatorio.");
                return;
            }

            Proveedor p = new Proveedor
            {
                CompaniaNombre = txtCompania.Text.Trim(),
                NombreContacto = txtContacto.Text.Trim(),
                Ciudad = txtCiudad.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Pais = txtPais.Text.Trim()
            };

            bool ok = await Task.Run(() => _repoProveedor.Insertar(p));

            if (ok)
            {
                MessageBox.Show("Proveedor guardado correctamente.");
                await CargarProveedoresAsync();
                LimpiarFormulario();
            }
        }

        private async void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdProveedor.Text)) return;

            Proveedor p = new Proveedor
            {
                ProveedorID = Convert.ToInt32(txtIdProveedor.Text),
                CompaniaNombre = txtCompania.Text.Trim(),
                NombreContacto = txtContacto.Text.Trim(),
                Ciudad = txtCiudad.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Pais = txtPais.Text.Trim()
            };

            bool ok = await Task.Run(() => _repoProveedor.Actualizar(p));

            if (ok)
            {
                MessageBox.Show("Proveedor actualizado correctamente.");
                await CargarProveedoresAsync();
                LimpiarFormulario();
            }
        }

        private async void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdProveedor.Text)) return;

            int id = Convert.ToInt32(txtIdProveedor.Text);

            if (MessageBox.Show("¿Desea eliminar lógicamente este proveedor?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                bool ok = await Task.Run(() => _repoProveedor.EliminarLogico(id));

                if (ok)
                {
                    MessageBox.Show("Proveedor eliminado lógicamente.");
                    await CargarProveedoresAsync();
                    LimpiarFormulario();
                }
            }
        }

        private void LimpiarFormulario()
        {
            txtIdProveedor.Clear();
            txtCompania.Clear();
            txtContacto.Clear();
            txtCiudad.Clear();
            txtTelefono.Clear();
            txtPais.Clear();
            dgProveedores.UnselectAll();
        }
    }
}