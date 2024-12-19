namespace WebApiProduccionAutenticacion.Models
{
    public class Maquina
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public string Modelo { get; set; }
        public int id_proceso { get; set; }
        public int id_ordenproduccion { get; set; }
    }
}
