using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using Laboratorio06.Data.Repositories;

namespace Laboratorio06
{
    public partial class ReportePedidosWindow : Window
    {
        private readonly PedidoRepository _repoPedido;

        public ReportePedidosWindow()
        {
            InitializeComponent();
            _repoPedido = new PedidoRepository();

            // Establecer fechas por defecto para la prueba (Agosto 2026)
            dtpInicio.SelectedDate = new DateTime(2026, 8, 1);
            dtpFin.SelectedDate = new DateTime(2026, 8, 31);
        }

        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (!dtpInicio.SelectedDate.HasValue || !dtpFin.SelectedDate.HasValue)
            {
                MessageBox.Show("Seleccione ambas fechas para consultar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime inicio = dtpInicio.SelectedDate.Value;
            DateTime fin = dtpFin.SelectedDate.Value;

            try
            {
                DataTable dt = await Task.Run(() => _repoPedido.ObtenerReporteDetallePedidos(inicio, fin));
                dgReporte.ItemsSource = dt.DefaultView;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron registros en el rango de fechas seleccionado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar el reporte:\n{ex.Message}");
            }
        }
    }
}