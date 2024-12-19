namespace WebApiProduccionAutenticacion.Models
{
    public class OrdenProduccion
    {
        public int ID { get; set; }
        public DateTime Fecha_Orden { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha_Entrega { get; set; }
        public int id_empleado { get; set; }
    }
}
