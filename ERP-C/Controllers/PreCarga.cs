using ERP_C.Data;
using ERP_C.Helpers;
using ERP_C.Migrations;
using ERP_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;


namespace ERP_C.Controllers
{
    
    public class PreCarga : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly BDContext _context;

        private readonly List<String> roles = new List<String>() {"Admin", "Empleado", "RRHH"};


        public PreCarga(UserManager<Persona> userManager, RoleManager<Rol> roleManager, BDContext context)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;
        }


        //public IActionResult Seed()
        //{
        //    CrearRoles().Wait();
        //    //CrearAdmin().Wait();
        //    //CrearEmpleados().Wait();
        //    CrearGerencia().Wait();


        //    return RedirectToAction("Index", "Home");
        //}
        //private async Task CrearGerencia()
        //{
        //    Gerencia gerencia = new Gerencia {
        //    Id=1,
        //    Nombre="Alo",
        //    EsGerenciaGeneral=true,
        //    EmpresaId=1
        //            };
        //    await _context.Gerencias.AddAsync(gerencia);
        //}

        public IActionResult Seed()
        {
            CrearRoles().Wait();
            //CrearAdmin().Wait();
            //CrearEmpleados().Wait();
            if (_context.Empleados.IsNullOrEmpty())
            {
                CrearGerencia().Wait();
                CrearLegales().Wait();
                CrearLegales2().Wait();
                CrearRRHH().Wait();
                CrearRRHH1().Wait();
                crearTelefonos().Wait();
            }
           
            return RedirectToAction("Index", "Home");
        }

        private async Task<Task> crearTelefonos()
        {

            TipoTelefono tipoTelefono = new TipoTelefono
            {
                Nombre = "Claro"
            };
            _context.Add(tipoTelefono);
            await _context.SaveChangesAsync();
            TipoTelefono tipoTelefono2 = new TipoTelefono
            {
                Nombre = "Samsung"
            };
            _context.Add(tipoTelefono2);

            await _context.SaveChangesAsync();
            TipoTelefono tipoTelefono1 = new TipoTelefono { Nombre = "Iphone" };
            _context.Add(tipoTelefono1);

            await _context.SaveChangesAsync();
            var valor1 = _context.TipoTelefonos.Where(b => b.Nombre == "Claro").FirstOrDefault().Id;
            var valor2 = _context.TipoTelefonos.Where(b => b.Nombre == "Samsung").FirstOrDefault().Id;

            Telefono telefono = new Telefono
            {
                TipoTelefonoId=valor1,
                EmpleadoId=1,
                Numero=1111111111
            };
            _context.Add(telefono);
            Telefono telefono1= new Telefono
            {
                TipoTelefonoId = valor2,
                EmpleadoId = 2,
                Numero = 1111111112
            };
            _context.Add(telefono1);
            Telefono telefono2 = new Telefono
            {
                TipoTelefonoId = valor1,
                EmpleadoId = 3,
                Numero = 111111113
            };
            _context.Add(telefono2);

            _context.SaveChanges();
            return Task.CompletedTask;

        }

        private async Task<Task> CrearGerencia()
        {
            var resultados=new IdentityResult();

          
       
            Foto foto = new Foto
            {
                Nombre="Jacinto",
                Path="Jacinto/img/empresas/"
            };
            _context.Add(foto);
       
            Foto foto2 = new Foto
            {
                Nombre = "Juan",
                Path = "Jacinto/img/empleados/"
            };
            _context.Add(foto2);
            
            Empresa empresa = new Empresa()
            {
                Nombre = "Panaderia SA",
                Rubro = "Panadera",
                ion = "Av.Libertador11",
                EmailContacto = "PanaderiaSa@gmail.com",
                Foto= foto,
                FotoId=1,
                TelefonoId=null
            };
            _context.Add(empresa);
       
            Gerencia gerencia = new Gerencia
            {
                Nombre = "Administracion",
                EsGerenciaGeneral = true,
                EmpresaId = 1,
                Empresa=empresa              
                
            }; 
            _context.Add(gerencia);
            

            Posicion posicion = new Posicion
            {
                Nombre = "Encargado",
                Descripcion = "supervisar",
                Sueldo = 10000,
                GerenciaId = 1,
                Gerencia=gerencia                
            };
            _context.Add(posicion);
            
            CentroDeCosto centroDeCosto= new CentroDeCosto
            {
                Gerencia = gerencia,
                GerenciaId = 1,  
                MontoMaximo=10000,
                Nombre= "Administracion"
            };
            _context.Add(centroDeCosto);
            
           
            Empleado empleado = new Empleado
            {
                Nombre = "Juan",
                Apellido = "Carlos",
                DNI = 43084344,
                Direccion = "Sucre",
                UserName = "empleado1@ort.edu.ar",
                Password = Alias.PassPorDefecto,
                Email = "empleado1@ort.edu.ar",
                FechaAlta = DateTime.Today,
                ObrasocialEmpleado = Obrasocial.SWISS_MEDICAL,
                Legajo = 1199991,
                Activo = true,
                Posicion = posicion,
                Foto= foto2,
                FotoId=2
            };

            resultados = await _userManager.CreateAsync(empleado, Alias.PassPorDefecto); //Alias.PassPorDefecto
            if (resultados.Succeeded)
            {

                await _userManager.AddToRoleAsync(empleado, Alias.RoleNombreEmpleado);

            }

            Gasto gasto = new Gasto
            {
                CentroDeCosto = centroDeCosto,
                CentroDeCostoId = 1,
                Descripcion="Computadoras",
                Empleado= empleado,
                EmpleadoId=1,
                Fecha=DateTime.Today,
                Monto=2000
            };
            _context.Add(gasto);

            

            //mas Niveles

           

  

            Gerencia gerencia1 = new Gerencia
            {
                Nombre = "Desarrollo",
                EsGerenciaGeneral = false,
                EmpresaId = 1,
                Empresa = empresa,
                GerenciaSuperiorId = 1,
                GerenciaSuperior=gerencia              

            };
            _context.Add(gerencia1);

            Posicion posicion1 = new Posicion
            {
                Nombre = "Supervisor",
                Descripcion = "supervisar",
                Sueldo = 8000,
                GerenciaId = 2,
                Gerencia = gerencia1                
            };
            _context.Add(posicion1);
            
            Posicion posicion2 = new Posicion
            {
                Nombre = "Empleado",
                Descripcion = "trabajar",
                Sueldo = 5000,
                GerenciaId = 2,
                Gerencia = gerencia1,
                JefeId = 2,
                Jefe= posicion                
            };
            _context.Add(posicion2);
            
            CentroDeCosto centroDeCosto1 = new CentroDeCosto
            {
                Gerencia = gerencia1,
                GerenciaId = 2,
                MontoMaximo = 10000,
                Nombre = "Desarrollo"
            };
            _context.Add(centroDeCosto1);


            Empleado empleado1 = new Empleado
            {
                Nombre = "Jorge",
                Apellido = "Gonzales",
                DNI = 43084322,
                Direccion = "Calle Falsa",
                UserName = "Jorge@ort.edu.ar",
                Password = Alias.PassPorDefecto,
                Email = "Jorge@ort.edu.ar",
                FechaAlta = DateTime.Today,
                ObrasocialEmpleado = Obrasocial.SWISS_MEDICAL,
                Legajo = 1199992,
                Activo = true,
                Posicion = posicion1,
                Foto = foto2,
                FotoId = 2
            };
            var resultados1 = await _userManager.CreateAsync(empleado1, Alias.PassPorDefecto); //Alias.PassPorDefecto
            if (resultados1.Succeeded)
            {

                await _userManager.AddToRoleAsync(empleado1, Alias.RoleNombreEmpleado);

            }

            Empleado empleado2 = new Empleado
            {
                Nombre = "Armando",
                Apellido = "Quito",
                DNI = 43084311,
                Direccion = "Calle Viva",
                UserName = "Quito@ort.edu.ar",
                Password = Alias.PassPorDefecto,
                Email = "Quito@ort.edu.ar",
                FechaAlta = DateTime.Today,
                ObrasocialEmpleado = Obrasocial.SWISS_MEDICAL,
                Legajo = 1199993,
                Activo = true,
                Posicion = posicion2,
                Foto = foto2,
                FotoId = 2
            };
            resultados = await _userManager.CreateAsync(empleado2, Alias.PassPorDefecto); //Alias.PassPorDefecto
            if (resultados.Succeeded)
            {

                await _userManager.AddToRoleAsync(empleado2, Alias.RoleNombreEmpleado);

            }

            Gasto gasto1 = new Gasto
            {
                CentroDeCosto = centroDeCosto1,
                CentroDeCostoId = 2,
                Descripcion = "Computadoras",
                Empleado = empleado1,
                EmpleadoId = 2,
                Fecha = DateTime.Today,
                Monto = 2000
            };
            _context.Add(gasto1);
            
            Gasto gasto2 = new Gasto
            {
                CentroDeCosto = centroDeCosto1,
                CentroDeCostoId = 2,
                Descripcion = "Computadoras",
                Empleado = empleado2,
                EmpleadoId = 3,
                Fecha = DateTime.Today,
                Monto = 4000
            };
            _context.Add(gasto2);

            _context.SaveChanges();

            //updates
            empleado.Gastos = new List<Gasto> { gasto };
            centroDeCosto.Gastos = new List<Gasto> { gasto };
            posicion.Empleado = empleado;
            posicion.EmpleadoId = 1;
            gerencia.CentroDeCosto = centroDeCosto;
            gerencia.Posiciones= new List<Posicion> {  posicion };
            empresa.Gerencias = new List<Gerencia> { gerencia };
            gerencia.Responsable = posicion;
            gerencia.ResponsableId = 1;

            empleado1.Gastos = new List<Gasto> { gasto1 };
            empleado2.Gastos = new List<Gasto> { gasto2 };
            centroDeCosto1.Gastos = new List<Gasto> { gasto1,gasto2 };
            posicion1.Empleado = empleado1;
            posicion2.Empleado = empleado2;
            posicion1.EmpleadoId = 2;
            posicion2.EmpleadoId = 3;
            gerencia1.CentroDeCosto = centroDeCosto1;
            gerencia1.Posiciones = new List<Posicion> { posicion1,posicion2 };
            empresa.Gerencias.Add(gerencia1);
            posicion1.Empleados= new List<Posicion> { posicion2 };
            gerencia.SubGerencias = new List<Gerencia> { gerencia1 };
            gerencia1.Responsable = posicion1;
            gerencia1.ResponsableId = 2;
            empresa.TelefonoId = 1;

          

            _context.Empleados.Update(empleado);
            _context.Empleados.Update(empleado1);
            _context.Empleados.Update(empleado2);
            _context.CentroDeCostos.Update(centroDeCosto);
            _context.CentroDeCostos.Update(centroDeCosto1);
            _context.Posiciones.Update(posicion);
            _context.Posiciones.Update(posicion1);
            _context.Posiciones.Update(posicion2);
            _context.Gerencias.Update(gerencia);
            _context.Gerencias.Update(gerencia1);
            _context.Empresas.Update(empresa);

            _context.SaveChanges();

            return Task.CompletedTask;
        }
        /*
        private async Task CrearEmpleados()
        {
            Empleado empleado = new Empleado
            {
                Nombre = "Juan",
                Apellido = "Carlos",
                DNI = 43084344,
                Direccion= "Amenabar",
                UserName = "empleado1@ort.edu.ar",
                Password = Alias.PassPorDefecto,
                Email = "empleado1@ort.edu.ar", 
                FechaAlta = DateTime.Today,
                ObrasocialEmpleado = Obrasocial.SWISS_MEDICAL,
                Legajo = 1199991,
                Activo = true
            };
            var resultado = await _userManager.CreateAsync(empleado, Alias.PassPorDefecto); //Alias.PassPorDefecto
            if (resultado.Succeeded)
            {
                
                await _userManager.AddToRoleAsync(empleado, Alias.RoleNombreEmpleado);
               
            }
        }
        */
        private async Task<Task> CrearLegales()
        {
            var gerencia = _context.Gerencias.Where(e => e.Nombre == "Administracion").FirstOrDefault();
            Gerencia gerencia2 = new Gerencia
            {
                Nombre = "Legales",
                EsGerenciaGeneral = false,
                EmpresaId = 1,
                GerenciaSuperiorId = gerencia.Id
            };
            var result = await _context.AddAsync(gerencia2);
            _context.SaveChanges();
            var gerencia3 = _context.Gerencias.Where(e => e.Nombre == "Legales").FirstOrDefault();
            CentroDeCosto centroDeCosto2 = new CentroDeCosto
            {
                GerenciaId = gerencia3.Id,
                MontoMaximo = 10000,
                Nombre = "Legales"
            };
            var result2 = _context.Add(centroDeCosto2);
            _context.SaveChanges();
            return Task.CompletedTask;

        }
        private async Task<Task> CrearLegales2()
        {
            Posicion posicion = new Posicion
            {
                Nombre = "Legales",
                Descripcion = "Legales",
                Sueldo = 7000,
                GerenciaId = 3,
                JefeId = 1
            };
            await _context.AddAsync(posicion);
            _context.SaveChanges();

            Empleado empleado = new Empleado
            {
                Nombre = "Marcos",
                Apellido = "Rolon",
                DNI = 43084343,
                Direccion = "Pampa",
                UserName = "empleadoLegales@ort.edu.ar",
                Password = Alias.PassPorDefecto,
                Email = "empleadoLegales@ort.edu.ar",
                FechaAlta = DateTime.Today,
                ObrasocialEmpleado = Obrasocial.SWISS_MEDICAL,
                Legajo = 1199995,
                Activo = true,
                PosicionId = 4
            };
            var resultado = await _userManager.CreateAsync(empleado, Alias.PassPorDefecto);
            if (resultado.Succeeded)
            {

                await _userManager.AddToRoleAsync(empleado, Alias.RoleNombreRRHH);

            }
            posicion.EmpleadoId = _context.Empleados.Where(e => e.Email == empleado.Email).FirstOrDefault().Id;
            var gerencia2 = _context.Gerencias.Find(3);
            gerencia2.ResponsableId = posicion.EmpleadoId;
            _context.Posiciones.Update(posicion);
            _context.Gerencias.Update(gerencia2);
            _context.SaveChanges();
            return Task.CompletedTask;

        }
        private async Task<Task> CrearRRHH()
        {


            var gerencia = _context.Gerencias.Where(e => e.Nombre == "Legales").FirstOrDefault();
            Gerencia gerencia2 = new Gerencia
            {
                Nombre = "Recursos Humanos",
                EsGerenciaGeneral = false,
                EmpresaId = 1,
                GerenciaSuperiorId = gerencia.Id
            };
           var result= await _context.AddAsync(gerencia2);
            _context.SaveChanges();
            var gerencia3 = _context.Gerencias.Where(e => e.Nombre == "Recursos Humanos").FirstOrDefault();
            CentroDeCosto centroDeCosto2 = new CentroDeCosto
            {
                GerenciaId = gerencia3.Id,
                MontoMaximo = 10000,
                Nombre = "Recursos humanos"
            };
           var result2= _context.Add(centroDeCosto2);
            _context.SaveChanges();

            return Task.CompletedTask;
        }
        private async Task<Task> CrearRRHH1()
        {
            var gerencia = _context.Gerencias.Where(e => e.Nombre == "Recursos Humanos").FirstOrDefault();
            Posicion posicion = new Posicion
            {
                Nombre = "RRHH",
                Descripcion = "RRHH",
                Sueldo = 7000,
                GerenciaId = gerencia.Id,
                JefeId = 4
            };
             await  _context.AddAsync(posicion);
            _context.SaveChanges();
            var posicionDB = _context.Posiciones.Where(e => e.Nombre == "RRHH").FirstOrDefault();
            Empleado empleado = new Empleado
            {
                Nombre = "Jose",
                Apellido = "Juan",
                DNI = 43084343,
                Direccion = "Amenabar",
                UserName = "empleadorrhh1@ort.edu.ar",
                Password = Alias.PassPorDefecto,
                Email = "empleadorrhh1@ort.edu.ar",
                FechaAlta = DateTime.Today,
                ObrasocialEmpleado = Obrasocial.SWISS_MEDICAL,
                Legajo = 1199991,
                Activo = true,
                PosicionId = posicionDB.Id
            };
            var resultado = await _userManager.CreateAsync(empleado, Alias.PassPorDefecto);
            if (resultado.Succeeded)
            {

                await _userManager.AddToRoleAsync(empleado, Alias.RoleNombreRRHH);

            }
            posicion.EmpleadoId=_context.Empleados.Where(e => e.Email==empleado.Email).FirstOrDefault().Id;
            var gerencia2 = _context.Gerencias.Find(4);
            gerencia2.ResponsableId = posicion.EmpleadoId;
            _context.Posiciones.Update(posicion);
            _context.Gerencias.Update(gerencia2);
            _context.SaveChanges();
            return Task.CompletedTask;

        }


        private async Task CrearRoles()
        {
            foreach(var rolName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rolName))
                {
                  await  _roleManager.CreateAsync(new Rol(rolName));

                }
            }
        }
    }

    
}
