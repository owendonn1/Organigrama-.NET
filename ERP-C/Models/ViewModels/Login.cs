using ERP_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP_C.Models.ViewModels
{
    public class Login
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        public String Email { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = ErrMsgs.Requerido)]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public bool Recordar { get; set; }
    }
}
