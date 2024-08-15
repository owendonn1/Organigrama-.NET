using ERP_C.Helpers;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_C.Models
{
    public class Empleado : Persona
    {
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = Alias.ObraSocial)]
        public Obrasocial ObrasocialEmpleado { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Range(1, 9999999, ErrorMessage = ErrMsgs.ValidRange)]
        public int Legajo { get; set; }
        public bool Activo { get; set; }


        public int? FotoId { get; set; }
        public Foto Foto { get; set; }
        public Telefono Telefono { get; set; }
        [NotMapped]
        public int? PosicionId { get; set; }
        public Posicion Posicion { get; set; }

        public List<Gasto> Gastos { get; set; }



        //public Gerencia GerenciaPerteneciente { get; set; }

        //public int? CentroDeCostoId { get; set; }

        //public CentroDeCosto CentroDeCosto { get; set; } 


        //public Gerencia GerenciaPerteneciente { get; set; }

        //public int? CentroDeCostoId { get; set; }

        //public CentroDeCosto CentroDeCosto { get; set; } 


        //public Gerencia GerenciaPerteneciente { get; set; }

        //public int? CentroDeCostoId { get; set; }

        //public CentroDeCosto CentroDeCosto { get; set; } 


        //public Gerencia GerenciaPerteneciente { get; set; }

        //public int? CentroDeCostoId { get; set; }

        //public CentroDeCosto CentroDeCosto { get; set; } 

    }

}


