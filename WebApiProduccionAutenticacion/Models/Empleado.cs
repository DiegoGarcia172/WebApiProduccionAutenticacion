namespace WebApiProduccionAutenticacion.Models
{
    public class Empleado
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellido_Materno { get; set; }
        public string Puesto { get; set; }
        public int id_departamento { get; set; }
    }
}
