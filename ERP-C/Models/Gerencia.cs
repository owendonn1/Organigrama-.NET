using ERP_C.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_C.Models
{
    public class Gerencia
    {
        public int Id { get; set; }


        [Required(ErrorMessage = ErrMsgs.Requerido)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrMsgs.EsGerenciaGeneral)]
        [DisplayName("Gerencia General")]
        public bool EsGerenciaGeneral { get; set; }

        [Display(Name = "Direccion")]
        public Gerencia GerenciaSuperior { get; set; } //De quien dependo

        [Display(Name=Alias.GerenciaSuperior)]
        public int? GerenciaSuperiorId { get; set; }

        [InverseProperty("GerenciaSuperior")]
        public List<Gerencia> SubGerencias { get; set; } //Las que dependen de mi

        public Posicion Responsable { get; set; }

        [ForeignKey("Responsable")]
        [Display(Name = Alias.Responsable)]
        public int? ResponsableId { get; set; } //La posición del responsable superior de esta gerencia.

        public Empresa Empresa { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [DisplayName("Empresa")]
        public int EmpresaId { get; set; }

        [InverseProperty("Gerencia")]
        public List<Posicion> Posiciones { get; set; }

       [InverseProperty("Gerencia")]
        public CentroDeCosto CentroDeCosto { get; set; }
        




        
    }
}
