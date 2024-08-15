using ERP_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP_C.Models
{
	public class Foto
	{
		public int Id { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrMsgs.StrSoloAlfab)]
        [MaxLength(30, ErrorMessage = ErrMsgs.StrMax)]
        public string Nombre { get; set; } = "yo.jpg";

        [Required(ErrorMessage = ErrMsgs.FormatoValido)]
        [MaxLength(100, ErrorMessage = ErrMsgs.StrMax)]
        [Display(Name = Alias.Path)]
        public string Path { get; set; } = "~/img/empleados/ o ~/img/empresas/";

        
 



	}

}

