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
using Microsoft.Data.SqlClient;

namespace ERP_C.Controllers
{
    [Authorize]
    public class PosicionesController : Controller
    {
        private readonly BDContext _context;

        public PosicionesController(BDContext context)
        {
            _context = context;
        }

        // GET: Posiciones
        public IActionResult Index()
        {
            var bDContext = _context.Posiciones.Include(p => p.Empleado).Include(p => p.Gerencia).Include(p => p.Jefe);
            return View(bDContext.ToList());
        }

        // GET: Posiciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posiciones == null)
            {
                return NotFound();
            }

            var posicion = await _context.Posiciones
                .Include(p => p.Empleado)
                .Include(p => p.Gerencia)
                .Include(p => p.Jefe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posicion == null)
            {
                return NotFound();
            }

            return View(posicion);
        }
        [Authorize(Roles = "RRHH")]
        // GET: Posiciones/Create
        public IActionResult Create()
        {
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido");
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre");
            ViewData["JefeId"] = new SelectList(_context.Posiciones, "Id", "Descripcion");
            return View();
        }
        [Authorize(Roles = "RRHH")]
        // POST: Posiciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("Id,Nombre,Descripcion,Sueldo,EmpleadoId,GerenciaSuperiorId,JefeId")] Posicion posicion)
        {

            var gerencia = _context.Gerencias.Find(posicion.GerenciaId);
            var hayJefe=_context.Posiciones.Where(g=>g.GerenciaId==gerencia.Id && g.JefeId==null).FirstOrDefault();
            //validacion para que una pocision no dependa de otra 
            if (posicion.JefeId == null&& hayJefe.JefeId==null)
            {
                ModelState.AddModelError("JefeId", "Una posición debe depender de otra posición (Jefe).");
            }

            if (ModelState.IsValid)
            {
                await _context.Posiciones.AddAsync(posicion);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbex)
                {
                    procesarDuplicado(dbex);
                }

            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", posicion.EmpleadoId);
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre", posicion.GerenciaId);
            ViewData["JefeId"] = new SelectList(_context.Posiciones, "Id", "Descripcion", posicion.JefeId);
            return View(posicion);
        }
        [Authorize(Roles = "RRHH")]
        // GET: Posiciones/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Posiciones == null)
            {
                return NotFound();
            }

            var posicion = _context.Posiciones.Find(id);
            if (posicion == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", posicion.EmpleadoId);
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre", posicion.GerenciaId);
            ViewData["JefeId"] = new SelectList(_context.Posiciones, "Id", "Descripcion", posicion.JefeId);
            return View(posicion);
        }
        [Authorize(Roles = "RRHH")]
        // POST: Posiciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Sueldo,EmpleadoId,GerenciaSuperiorId,JefeId")] Posicion posicion)
        {
            if (id != posicion.Id)
            {
                return NotFound();
            }
            var gerencia = _context.Gerencias.Find(posicion.GerenciaId);
            var hayJefe = _context.Posiciones.Where(g => g.GerenciaId == gerencia.Id && g.JefeId == null).FirstOrDefault();
            //validacion para que una pocision no dependa de otra 
            if (posicion.JefeId == null && hayJefe.JefeId == null&& hayJefe.Id!=posicion.Id)
            {
                ModelState.AddModelError("JefeId", "Una posición debe depender de otra posición (Jefe).");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var PocisionEnDB = _context.Posiciones.Find(posicion.Id);
                    if(PocisionEnDB != null)
                    {
                        PocisionEnDB.Nombre = posicion.Nombre;  
                        PocisionEnDB.Sueldo = posicion.Sueldo;
                        PocisionEnDB.Descripcion= posicion.Descripcion;
                        PocisionEnDB.Sueldo = posicion.Sueldo;
                    }

                    _context.Update(PocisionEnDB);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosicionExists(posicion.Id))
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
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", posicion.EmpleadoId);
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre", posicion.GerenciaId);
            ViewData["JefeId"] = new SelectList(_context.Posiciones, "Id", "Descripcion", posicion.JefeId);
            return View(posicion);
        }

        // GET: Posiciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posiciones == null)
            {
                return NotFound();
            }

            var posicion = await _context.Posiciones
                .Include(p => p.Empleado)
                .Include(p => p.Gerencia)
                .Include(p => p.Jefe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posicion == null)
            {
                return NotFound();
            }

            return View(posicion);
        }

        // POST: Posiciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posiciones == null)
            {
                return Problem("Entity set 'BDContext.Posiciones'  is null.");
            }
            var posicion = await _context.Posiciones.FindAsync(id);
            if (posicion != null)
            {
                _context.Posiciones.Remove(posicion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PosicionExists(int id)
        {
          return _context.Posiciones.Any(e => e.Id == id);
        }

        public IActionResult ListaEmpleados()
        {
            var posicionesConEmpleados = _context.Posiciones.Where(p=> p.EmpleadoId != null)
                .Include(p => p.Empleado) 
                .OrderByDescending(p => p.Sueldo)
                .ThenBy(p => p.Empleado.Apellido)
                .ThenBy(p => p.Empleado.Nombre)
                .ToList();

            return View(posicionesConEmpleados);
        }

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
