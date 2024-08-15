using Microsoft.Build.Framework;
using ERP_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP_C.Models.ViewModels

{
    public class CrearEmpleado
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        [EmailAddress(ErrorMessage = ErrMsgs.Requerido)]
        public String Email { get; set; }

       
        public bool esRRHH { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = Alias.ObraSocial)]
        public Obrasocial ObrasocialEmpleado { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        [Range(1, 9999999, ErrorMessage = ErrMsgs.ValidRange)]
        public int Legajo { get; set; }
        public bool Activo { get; set; }


        public int? FotoId { get; set; }
        public Foto Foto { get; set; }
        public Telefono Telefono { get; set; }
        public Posicion Posicion { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrMsgs.StrSoloAlfab)]
        [MinLength(2, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(30, ErrorMessage = ErrMsgs.StrMax)]
        public string Nombre { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrMsgs.StrSoloAlfab)]
        [MinLength(2, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(30, ErrorMessage = ErrMsgs.StrMax)]
        public string Apellido { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = Alias.PersonaDocumento)]
        [Range(1000000, 99999999, ErrorMessage = ErrMsgs.ValidRange)]
        public int DNI { get; set; }

        //[System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        [MinLength(5, ErrorMessage = ErrMsgs.StrMin)]
        [MaxLength(50, ErrorMessage = ErrMsgs.StrMax)]
        public string Direccion { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime FechaAlta { get; set; } = DateTime.Now;
    }
}
