using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_C.Models.ViewModels
{
    public class TarjetaContacto
    {
        public String Apellido { get; set; }
        public String Nombre { get; set;}
        public String NombrePosicion {  get; set;}
        public Telefono Telefono { get; set; }
        public String Email { get; set; }
        [NotMapped]
        public Posicion Posicion { get; set; }

        public Organigrama Organigrama { get; set; }

    }
}
