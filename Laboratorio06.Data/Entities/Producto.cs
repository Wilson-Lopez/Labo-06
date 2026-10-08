namespace Laboratorio06.Data.Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int IdProveedor { get; set; }
        public int IdCategoria { get; set; }
        public string CantidadPorUnidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public int UnidadesEnExistencia { get; set; }
        public bool Activo { get; set; }
    }
}