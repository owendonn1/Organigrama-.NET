using ERP_C.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERP_C.Models
{
	public class Gasto
	{
		public int Id { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [MinLength(2, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(200, ErrorMessage = ErrMsgs.StrMax)]
        [DataType(DataType.MultilineText)]
        [Display(Name = Alias.Descripcion)]
        public string Descripcion { get; set; }
        
       
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Range(0, double.MaxValue, ErrorMessage = ErrMsgs.ValidMonto)]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]

        public double Monto { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime Fecha { get; set; }

        //[Required(ErrorMessage = ErrMsgs.Requerido)]
        [DisplayName("Empleado")]
		public int? EmpleadoId { get; set; }
		public Empleado Empleado { get; set; }


		[Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = Alias.CentroDeCostos)]
        public int? CentroDeCostoId { get; set; }

        [Display(Name = Alias.CentroDeCostos)]
        public CentroDeCosto CentroDeCosto { get; set; }



	}
}
