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

namespace ERP_C.Controllers
{
    [Authorize]
    public class TelefonosController : Controller
    {
        private readonly BDContext _context;

        public TelefonosController(BDContext context)
        {
            _context = context;
        }

        // GET: Telefonos
        public IActionResult Index()
        {
            var bDContext = _context.Telefonos.ToList();
            foreach (var item in bDContext)
            {
                item.TipoTelefono = _context.TipoTelefonos.Where(e => e.Id == item.TipoTelefonoId).FirstOrDefault();
                item.empleado= _context.Empleados.Where(e => e.Id == item.EmpleadoId).FirstOrDefault();

            }
            return View(bDContext);
        }

        // GET: Telefonos/Details/5
        public  IActionResult Details(int? id)
        {
            if (id == null || _context.Telefonos == null)
            {
                return NotFound();
            }

            var telefono = _context.Telefonos.FirstOrDefault(t => t.Id == id);
            if (telefono == null)
            {
                return NotFound();
            }
            telefono.TipoTelefono = _context.TipoTelefonos.Where(e => e.Id == telefono.TipoTelefonoId).FirstOrDefault();
            telefono.empleado = _context.Empleados.Where(e => e.Id == telefono.EmpleadoId).FirstOrDefault();
            return View(telefono);
        }

        // GET: Telefonos/Create
        public IActionResult Create()
        {
            ViewData["TipoTelefonoId"] = new SelectList(_context.TipoTelefonos, "Id", "Nombre");
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Nombre");
            return View();
        }

        // POST: Telefonos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Numero,EmpleadoId,TipoTelefonoId")] Telefono telefono)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Telefonos.Add(telefono);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError(nameof(Telefono.EmpleadoId), "El empleado ya tiene Telefono");
                }
            }
            ViewData["TipoTelefonoId"] = new SelectList(_context.TipoTelefonos, "Id", "Nombre", telefono.TipoTelefonoId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Nombre", telefono.EmpleadoId);
            return View(telefono);
        }

        // GET: Telefonos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Telefonos == null)
            {
                return NotFound();
            }

            var telefono = _context.Telefonos.Find(id);
            if (telefono == null)
            {
                return NotFound();
            }
            ViewData["TipoTelefonoId"] = new SelectList(_context.TipoTelefonos, "Id", "Nombre", telefono.TipoTelefonoId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Nombre", telefono.EmpleadoId);
            return View(telefono);
        }

        // POST: Telefonos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero")] Telefono telefono)
        {
            if (id != telefono.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var TelefonoEnDB = _context.Telefonos.Find(telefono.Id);
                    if (TelefonoEnDB != null)
                    {
                        //Actualizamos
                        TelefonoEnDB.Numero = telefono.Numero;

                        _context.Telefonos.Update(TelefonoEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelefonoExists(telefono.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoTelefonoId"] = new SelectList(_context.TipoTelefonos, "Id", "Nombre", telefono.TipoTelefonoId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Nombre", telefono.EmpleadoId);
            return View(telefono);
        }

        // GET: Telefonos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Telefonos == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (telefono == null)
            {
                return NotFound();
            }
            telefono.TipoTelefono = _context.TipoTelefonos.Where(e => e.Id == telefono.TipoTelefonoId).FirstOrDefault();
            telefono.empleado = _context.Empleados.Where(e => e.Id == telefono.EmpleadoId).FirstOrDefault();
            return View(telefono);
        }

        // POST: Telefonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Telefonos == null)
            {
                return Problem("Entity set 'BDContext.Telefonos'  is null.");
            }
            var telefono = await _context.Telefonos.FindAsync(id);
            if (telefono != null)
            {
                _context.Telefonos.Remove(telefono);
            }
            telefono.TipoTelefono = _context.TipoTelefonos.Where(e => e.Id == telefono.TipoTelefonoId).FirstOrDefault();
            telefono.empleado = _context.Empleados.Where(e => e.Id == telefono.EmpleadoId).FirstOrDefault();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelefonoExists(int id)
        {
          return _context.Telefonos.Any(e => e.Id == id);
        }
    }
}
