using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_C.Models.ViewModels
{
    public class Organigrama
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public bool EsGerenciaGeneral { get; set; }
        public Posicion   Responsable { get; set; }
        public List<Gerencia> GerenciasInferiores { get; set; }

        [InverseProperty("Organigrama")]
        public  List<TarjetaContacto> Empleados { get; set; }

    }
}
