using ERP_C.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ERP_C.Models
{
	public class Empresa
	{
		public int Id { get; set; }


        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrMsgs.StrSoloAlfab)]
        [MinLength(2, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(30, ErrorMessage = ErrMsgs.StrMax)]
        [Display(Name = Alias.NombreEmpresa)]
        public string Nombre {get; set;}

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrMsgs.ValidAlphanumeric)]
        [MinLength(2, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(50, ErrorMessage = ErrMsgs.StrMax)]
        [Display(Name = Alias.Rubro)]
        public string Rubro {get; set;}
        public int? FotoId { get; set; }
        public Foto Foto { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [MinLength(2, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(50, ErrorMessage = ErrMsgs.StrMax)]
        [DisplayName("Direccion")]
        public string ion {get; set;}
        

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = Alias.CorreoElectronico)]
        [EmailAddress(ErrorMessage = ErrMsgs.NotValid)]
        [MinLength(2, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(50, ErrorMessage = ErrMsgs.StrMax)]
        public string EmailContacto {get; set;}

		public List<Gerencia> Gerencias { get; set; }

        [AllowNull]
        public int? TelefonoId { get; set; }
        [NotMapped]
        public Telefono Telefono { get; set; }
    }
}
