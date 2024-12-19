using System.ComponentModel.DataAnnotations;

namespace WebApiProduccionAutenticacion.Models
{
    public class Rechazo
    {
        [Key]
        public int ID { get; set; }
        public int id_producto { get; set; }
        public int CantidadPR { get; set; }
        public DateTime Fecha_Hora { get; set; }
        public string Descripcion { get; set; }
    }
}
