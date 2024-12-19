namespace WebApiProduccionAutenticacion.Models
{
    public class Almacen
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int id_producto { get; set; }
        public int id_orden { get; set; }
    }
}
