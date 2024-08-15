using ERP_C.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_C.Models
{
    public class CentroDeCosto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = Alias.NombreCentroCosto)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Range(0, double.MaxValue, ErrorMessage = ErrMsgs.ValidSueldo)]
        public double MontoMaximo { get; set; }

        public List<Gasto> Gastos { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        public int GerenciaId { get; set; }
        public Gerencia Gerencia { get; set; }

        

    }
}
