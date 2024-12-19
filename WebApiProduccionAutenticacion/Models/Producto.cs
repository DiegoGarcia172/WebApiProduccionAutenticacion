namespace WebApiProduccionAutenticacion.Models
{
    public class Producto
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public string Calidad { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public string Descripcion { get; set; }
    }
}
