using ERP_C.Helpers;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;



namespace ERP_C.Models
{
	public class Persona : IdentityUser<int>
	{
		//public int Id { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrMsgs.StrSoloAlfab)]
        [MinLength(2, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(30, ErrorMessage = ErrMsgs.StrMax)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage =ErrMsgs.StrSoloAlfab)]
        [MinLength(2, ErrorMessage =ErrMsgs.StrMin)]
        [MaxLength(30, ErrorMessage =ErrMsgs.StrMax)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = Alias.PersonaDocumento)]
        [Range(1000000, 99999999, ErrorMessage= ErrMsgs.ValidRange)]
        public int DNI { get; set; }

        //[Required(ErrorMessage = ErrMsgs.Requerido)]
        [MinLength(5, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(50, ErrorMessage = ErrMsgs.StrMax)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = Alias.NombreDeUsuario)]
        [MinLength(5, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(40, ErrorMessage = ErrMsgs.StrMax)]

        public override string UserName { 
            get { return base.UserName; }
            set { base.UserName = value; }
        }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = Alias.Password)]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(25, ErrorMessage = ErrMsgs.StrMax)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = Alias.CorreoElectronico)]
        [EmailAddress(ErrorMessage = ErrMsgs.Requerido)] 
        [MaxLength(50, ErrorMessage = ErrMsgs.StrMax)]
        public override string Email {
            get { return base.Email; }
            set { base.Email = value; }
        }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
		public DateTime FechaAlta { get; set; } = DateTime.Now;
        


	}
}
