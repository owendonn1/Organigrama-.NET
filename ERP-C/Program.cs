namespace ERP_C
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var app = Startup.InicializarApp(args); //pasamos los argumentos que recibden para la ejecucion.

			app.Run();
		}
	}
}