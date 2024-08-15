using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_C.Data;
using ERP_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace ERP_C.Controllers
{
    [Authorize]
    public class TipoTelefonosController : Controller
    {
        private readonly BDContext _context;

        public TipoTelefonosController(BDContext context)
        {
            _context = context;
        }

        // GET: TipoTelefonos
        public IActionResult Index()
        {
              return View(_context.TipoTelefonos.ToList());
        }

        // GET: TipoTelefonos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.TipoTelefonos == null)
            {
                return NotFound();
            }

            var tipoTelefono = _context.TipoTelefonos.FirstOrDefault(t => t.Id == id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }

            return View(tipoTelefono);
        }

        // GET: TipoTelefonos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoTelefonos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nombre")] TipoTelefono tipoTelefono)
        {
            if (ModelState.IsValid)
            {
                _context.TipoTelefonos.Add(tipoTelefono);
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
            return View(tipoTelefono);
        }

        // GET: TipoTelefonos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.TipoTelefonos == null)
            {
                return NotFound();
            }

            var tipoTelefono =  _context.TipoTelefonos.Find(id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }
            return View(tipoTelefono);
        }

        // POST: TipoTelefonos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] TipoTelefono tipoTelefono)
        {
            if (id != tipoTelefono.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tipoTelefonoEnBD =  _context.TipoTelefonos.Find(tipoTelefono.Id);
                    if(tipoTelefonoEnBD != null)
                    {
                        //Actualizamos
                        tipoTelefonoEnBD.Nombre = tipoTelefono.Nombre;

                        _context.TipoTelefonos.Update(tipoTelefonoEnBD);
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
                    if (!TipoTelefonoExists(tipoTelefono.Id))
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
            return View(tipoTelefono);
        }

        // GET: TipoTelefonos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipoTelefonos == null)
            {
                return NotFound();
            }

            var tipoTelefono = await _context.TipoTelefonos.FirstOrDefaultAsync(m => m.Id == id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }

            return View(tipoTelefono);
        }

        // POST: TipoTelefonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoTelefonos == null)
            {
                return Problem("Entity set 'BDContext.TipoTelefonos'  is null.");
            }
            var tipoTelefono = await _context.TipoTelefonos.FindAsync(id);
            if (tipoTelefono != null)
            {
                _context.TipoTelefonos.Remove(tipoTelefono);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoTelefonoExists(int id)
        {
          return _context.TipoTelefonos.Any(e => e.Id == id);
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
