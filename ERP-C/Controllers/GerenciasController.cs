using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_C.Data;
using ERP_C.Models;
using ERP_C.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ERP_C.Models.ViewModels;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.SqlClient;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace ERP_C.Controllers
{
    [Authorize]
    public class GerenciasController : Controller
    {
        private readonly BDContext _context;
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;


        public GerenciasController(BDContext context, UserManager<Persona> userManager,SignInManager<Persona> signInManager)
        {
            _context = context;
            _usermanager = userManager;
            _signInManager = signInManager;
        }

        // GET: Gerencias
        public IActionResult Index()
        {
            var bDContext = _context.Gerencias.Include(g => g.Empresa).Include(g => g.GerenciaSuperior).Include(g => g.Responsable);
            return View( bDContext.ToList());
        }

        // GET: Gerencias/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Gerencias == null)
            {
                return NotFound();
            }

            var gerencia = _context.Gerencias
                .Include(g => g.Empresa)
                .Include(g => g.GerenciaSuperior)
                .Include(g => g.Responsable)
                .FirstOrDefault(m => m.Id == id);
            if (gerencia == null)
            {
                return NotFound();
            }

            return View(gerencia);
        }

        [Authorize(Roles = "RRHH")]
        // GET: Gerencias/Create
        public IActionResult Create()
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "EmailContacto");
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre");
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Descripcion");
            return View();
        }

        // POST: Gerencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "RRHH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nombre,EsGerenciaGeneral,GerenciaSuperiorId,ResponsableId,EmpresaId")] Gerencia gerencia)
        {
            if ((gerencia.EsGerenciaGeneral == true & !(gerencia.GerenciaSuperiorId == null)) || (gerencia.EsGerenciaGeneral == false & gerencia.GerenciaSuperiorId == null))
            {
                ModelState.AddModelError(nameof(gerencia.EsGerenciaGeneral), "Debe haber 1 Gerencia General");
            }
     
            if (ModelState.IsValid)
            {

                _context.Add(gerencia);
                try
                {
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbex)
                {
                    procesarDuplicado(dbex);
                }
               
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "EmailContacto", gerencia.EmpresaId);
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerencia.GerenciaSuperiorId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Descripcion", gerencia.ResponsableId);
            return View(gerencia);
        }
        [Authorize(Roles = "RRHH")]
        // GET: Gerencias/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Gerencias == null)
            {
                return NotFound();
            }

            var gerencia = _context.Gerencias.Find(id);
            if (gerencia == null)
            {
                return NotFound();
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "EmailContacto", gerencia.EmpresaId);
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerencia.GerenciaSuperiorId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Descripcion", gerencia.ResponsableId);
            return View(gerencia);
        }

        // POST: Gerencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "RRHH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Nombre,EsGerenciaGeneral,GerenciaSuperiorId,ResponsableId,EmpresaId")] Gerencia gerencia)
        {
            if (id != gerencia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var GerenciaEnDB = _context.Gerencias.Find(gerencia.Id);
                    if (GerenciaEnDB != null)
                    {
                        GerenciaEnDB.Nombre = gerencia.Nombre;
                        if ((!GerenciaEnDB.EsGerenciaGeneral) & (gerencia.GerenciaSuperiorId != 0) & (GerenciaEnDB.Id!=gerencia.GerenciaSuperiorId))
                        {
                            GerenciaEnDB.GerenciaSuperiorId = gerencia.GerenciaSuperiorId;

                        }
                        GerenciaEnDB.ResponsableId = gerencia.ResponsableId;
                        GerenciaEnDB.EmpresaId = gerencia.EmpresaId;

                        _context.Gerencias.Update(GerenciaEnDB);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GerenciaExists(gerencia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException dbex)
                {
                    procesarDuplicado(dbex);
                }
               
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "EmailContacto", gerencia.EmpresaId);
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre", gerencia.GerenciaSuperiorId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "Id", "Descripcion", gerencia.ResponsableId);
            return View(gerencia);
        }

        // GET: Gerencias/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Gerencias == null)
            {
                return NotFound();
            }

            var gerencia = _context.Gerencias
                .Include(g => g.Empresa)
                .Include(g => g.GerenciaSuperior)
                .Include(g => g.Responsable)
                .FirstOrDefault(m => m.Id == id);
            if (gerencia == null)
            {
                return NotFound();
            }

            return View(gerencia);
        }

        // POST: Gerencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {

            if (_context.Gerencias == null)
            {
                return Problem("Entity set 'BDContext.Gerencias'  is null.");
            }
            var gerencia = _context.Gerencias.Find(id);
            if (gerencia != null)
            {
                if (gerencia.EsGerenciaGeneral)
                {
                    ViewBag.ErrorMessage = "No se puede borrar la Gerencia General";
                    gerencia = _context.Gerencias
                    .Include(g => g.Empresa)
                    .Include(g => g.GerenciaSuperior)
                    .Include(g => g.Responsable)
                    .FirstOrDefault(m => m.Id == id);

                    return View(gerencia);
                }
                else
                {
                    _context.Gerencias.Remove(gerencia);
                }
            }

             _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool GerenciaExists(int id)
        {
            return _context.Gerencias.Any(e => e.Id == id);
        }

        public IActionResult Organigrama(int? id)
        {  
            Gerencia gerencia=null;
            if (id==null)
            {
                   int personaId= Int32.Parse(_usermanager.GetUserId(User));
            Empleado empleado = _context.Empleados.Find(personaId);
            var posicion = _context.Posiciones.Where(b => b.EmpleadoId == empleado.Id).Include(g =>g.Gerencia).FirstOrDefault();
                if (posicion == null)
                {
                    return Problem("No puede ver la informacion un Empleado sin Posicion asignada.");
                }
                var empresa=_context.Empresas.Find(posicion.Gerencia.EmpresaId);
            List<Gerencia> gerencias = _context.Gerencias.Where(c => c.EmpresaId==empresa.Id).Include(r=>r.Responsable).ThenInclude(t=>t.Empleado).ToList();
             var iterador=gerencias.GetEnumerator();
            while (iterador.MoveNext()&&gerencia==null)
            {
               if(iterador.Current.EsGerenciaGeneral)
                {
                    gerencia = iterador.Current;
                }
            }
            if (gerencia == null)
            {
                return NotFound();
            }
            }
            else
            {
                gerencia = _context.Gerencias.Where(c => c.Id == id).Include(r => r.Responsable).FirstOrDefault();
            }


            var organigrama = new Organigrama();
            organigrama.EsGerenciaGeneral = gerencia.EsGerenciaGeneral;
                organigrama.Nombre = gerencia.Nombre;
                organigrama.Responsable = gerencia.Responsable;
                organigrama.GerenciasInferiores=_context.Gerencias.Where(c=>c.GerenciaSuperiorId==gerencia.Id).Include(r=>r.Responsable).ThenInclude(t=>t.Empleado).ToList();
                var tarjetas = _context.Posiciones.Where(b => b.GerenciaId == gerencia.Id).Include(g => g.Empleado).ToList();
            organigrama.Empleados = CrearTarjetasEmpleados(gerencia);
   
            return View(organigrama);
        }

        //public IActionResult OrganigramaIndividual(int id)
        //{
        //    if ( _context.Gerencias == null)
        //    {
        //        return NotFound();
        //    }
        //    var gerencia = _context.Gerencias.Where(c => c.Id == id).Include(r => r.Responsable).FirstOrDefault();
        //    var organigrama = new Organigrama();

        //    if (gerencia == null) {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        organigrama.EsGerenciaGeneral = gerencia.EsGerenciaGeneral;
        //        organigrama.Nombre = gerencia.Nombre;
        //        organigrama.Responsable = gerencia.Responsable;
        //        organigrama.GerenciasInferiores = _context.Gerencias.Where(c => c.GerenciaId == gerencia.Id).ToList();
        //        organigrama.Empleados = CrearTarjetasEmpleados(gerencia);          
               
        //    }

        //    return View(organigrama);
        //}

        private List<TarjetaContacto> CrearTarjetasEmpleados(Gerencia gerencia)
        {
            var empleados= new List<TarjetaContacto>();
            var posiciones = _context.Posiciones.Where(b => b.GerenciaId == gerencia.Id&&b.EmpleadoId!=null).Include(g => g.Empleado).ToList().OrderBy(r=>r.Empleado.Apellido).ThenBy(r=>r.Empleado.Nombre);
            foreach (Posicion posicion in posiciones)
            {
                if (posicion != null&& posicion.Empleado!=null)
                {
                   if (posicion.Empleado.Activo)
                {
                        var tarjeta = new TarjetaContacto
                        {
                            Apellido = posicion.Empleado.Apellido,
                            Nombre = posicion.Empleado.Nombre,
                            NombrePosicion = posicion.Nombre,
                            Telefono = _context.Telefonos.Where(t => t.EmpleadoId == posicion.EmpleadoId).FirstOrDefault(),
                            Email = posicion.Empleado.Email,
                            Posicion = posicion
                };
                empleados.Add(tarjeta);

                }  
                }

               
              
            }
            return empleados;
        }
        //public IActionResult Tarjeta(int posicionId) {
            
        //        var posicionDB= _context.Posiciones.Include(g => g.Empleado).FirstOrDefault(p=>p.Id==posicionId);
        //    if (posicionDB != null&& posicionDB.Empleado!=null)
        //    {
        //            var tarjeta= new TarjetaContacto
        //            {
        //                Apellido = posicionDB.Empleado.Apellido,
        //                Nombre = posicionDB.Empleado.Nombre,
        //                NombrePosicion = posicionDB.Nombre,
        //                Telefono = _context.Telefonos.Where(t => t.EmpleadoId == posicionDB.EmpleadoId).FirstOrDefault(),
        //                Email = posicionDB.Empleado.Email,
        //            };
        //        return View(tarjeta);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        private void procesarDuplicado(DbUpdateException dbex)
        {
            SqlException innerException = dbex.InnerException as SqlException;
            if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
            {
                ModelState.AddModelError("Nombre", "Es duplicado");
            }
            else
            {
                ModelState.AddModelError(string.Empty, dbex.Message);
            }
        }
    }
}
