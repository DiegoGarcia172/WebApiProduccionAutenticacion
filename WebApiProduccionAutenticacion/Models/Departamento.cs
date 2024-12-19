using System.ComponentModel.DataAnnotations;

namespace WebApiProduccionAutenticacion.Models
{
    public class Departamento
    {
        [Key]
        public int ID { get; set; }
        public string Nombre { get; set; }
    }
}
