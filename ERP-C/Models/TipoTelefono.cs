using ERP_C.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_C.Models
{
    public class TipoTelefono
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Telefono> Telefonos { get; set; }


    }
}
    