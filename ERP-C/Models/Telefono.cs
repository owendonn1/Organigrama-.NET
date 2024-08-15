using ERP_C.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_C.Models
{
	public class Telefono
	{
        public int Id { get; set; }
        [Range(10000000, 99999999, ErrorMessage = ErrMsgs.ValidRange)]
        public int Numero { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [DisplayName("Empleado")]
        public int EmpleadoId { get; set; }
        [NotMapped]
        public Empleado empleado { get; set; }

        [NotMapped]
        Empresa empresa { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        public int TipoTelefonoId { get; set; }
        [NotMapped]
        public TipoTelefono TipoTelefono { get; set; }


    }
}
