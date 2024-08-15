
using Microsoft.Build.Framework;
using ERP_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP_C.Models.ViewModels
{
    public class RegistrarUsuario
    {
       
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage=ErrMsgs.Requerido)]
        [EmailAddress(ErrorMessage = ErrMsgs.Requerido)]
        public String Email {get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public String ConfirmacionPassword { get; set; }

        public bool esRRHH { get; set; }


    }
}
