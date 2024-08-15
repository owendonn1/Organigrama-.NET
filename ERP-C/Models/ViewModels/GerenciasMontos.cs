namespace ERP_C.Models.ViewModels
{
    public class GerenciasMontos
    {
        public GerenciasMontos(Gerencia gerencia, double montoTotal)
        {
            this.Gerencia = gerencia;
            this.montoTotal = montoTotal;
        }

        public Gerencia Gerencia { get; set; }
        public double montoTotal { get; set; }
    }
}
