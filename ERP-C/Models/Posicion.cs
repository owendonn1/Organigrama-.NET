using ERP_C.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ERP_C.Models
{
	public class Posicion
	{
		public int Id { get; set; }


		[Required(ErrorMessage = ErrMsgs.Requerido)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrMsgs.StrSoloAlfab)]
        [MinLength(2, ErrorMessage = ErrMsgs.StrMin)]
		[MaxLength(30, ErrorMessage = ErrMsgs.StrMax)]
		[Display(Name = Alias.NombreDePocicion)]
		public string Nombre { get; set; }

		[Required(ErrorMessage = ErrMsgs.Requerido)]
		[MinLength(2, ErrorMessage = ErrMsgs.StrMin)]
		[MaxLength(200, ErrorMessage = ErrMsgs.StrMax)]
        [DataType(DataType.MultilineText)]
        [Display(Name = Alias.Descripcion)]
		public string Descripcion { get; set; }

		[Required(ErrorMessage = ErrMsgs.Requerido)]
		[Range(0, double.MaxValue, ErrorMessage = ErrMsgs.ValidSueldo)]
		public decimal Sueldo { get; set; }

    
        public int? EmpleadoId { get; set; }

        public Empleado Empleado { get; set; }
        
		[Required(ErrorMessage = ErrMsgs.Requerido)]
        public int GerenciaId { get; set; }//la Posiccion pertenece a una gerencia 

        public Gerencia Gerencia { get; set; }

        

       

        public Posicion Jefe { get; set; } //De quien dependo

		[ForeignKey("Jefe")]
		public int? JefeId { get; set; } //De quien dependo

        [InverseProperty("Jefe")]
        public List<Posicion> Empleados { get; set; } //Quienes dependen de mi


    }
}
