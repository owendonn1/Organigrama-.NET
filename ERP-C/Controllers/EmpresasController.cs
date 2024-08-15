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
using System.Collections;
using Microsoft.Data.SqlClient;

namespace ERP_C.Controllers
{
    [Authorize]
    public class EmpresasController : Controller
    {
        private readonly BDContext _context;

        public EmpresasController(BDContext context)
        {
            _context = context;
        }

        // GET: Empresas
        public IActionResult Index()
        {
            var bDContext = _context.Empresas.Include(e => e.Foto).ToList();
            foreach(var e in bDContext)
            {
                if (e.TelefonoId!=null)
                {
                                    e.Telefono = _context.Telefonos.Where(q => q.Id == e.TelefonoId).FirstOrDefault();

                }
            }
            return View(bDContext.ToList());
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .Include(e => e.Foto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa.TelefonoId!=null)
            {
                            empresa.Telefono=_context.Telefonos.Where(_ => _.Id == empresa.TelefonoId).FirstOrDefault();

            }
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }
        [Authorize(Roles = "RRHH")]
        // GET: Empresas/Create
        public IActionResult Create()
        {
            ViewData["FotoId"] = new SelectList(_context.Fotos, "Id", "Nombre");
            ViewData["TelefonoId"] = new SelectList(_context.Telefonos, "Id", "Numero");
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "RRHH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nombre,Rubro,FotoId,ion,EmailContacto,TelefonoId")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                _context.Empresas.Add(empresa);
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
            ViewData["FotoId"] = new SelectList(_context.Fotos, "Id", "Nombre", empresa.FotoId);
            ViewData["TelefonoId"] = new SelectList(_context.Telefonos, "Id", "Numero", empresa.TelefonoId);
            return View(empresa);
        }
        [Authorize(Roles = "RRHH")]
        // GET: Empresas/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = _context.Empresas.Find(id);
            if (empresa == null)
            {
                return NotFound();
            }
            ViewData["FotoId"] = new SelectList(_context.Fotos, "Id", "Nombre", empresa.FotoId);
            ViewData["TelefonoId"] = new SelectList(_context.Telefonos, "Id", "Numero", empresa.TelefonoId);
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "RRHH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Nombre,Rubro,FotoId,ion,EmailContacto,TelefonoId")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //preguntar ?
                    var EmpresaEnDB = _context.Empresas.Find(empresa.Id);
                    if (EmpresaEnDB != null)
                    { 
                    EmpresaEnDB.Nombre = empresa.Nombre;
                    EmpresaEnDB.Rubro = empresa.Rubro;
                    EmpresaEnDB.EmailContacto = empresa.EmailContacto;
                    EmpresaEnDB.FotoId = empresa.FotoId;
                    EmpresaEnDB.ion=empresa.ion;
                        EmpresaEnDB.TelefonoId = empresa.TelefonoId;

                    _context.Update(EmpresaEnDB);
                    _context.SaveChangesAsync();
                     return RedirectToAction(nameof(Index));
                    }

                 else
                {
                    return NotFound();
                }

            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
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
            ViewData["FotoId"] = new SelectList(_context.Fotos, "Id", "Nombre", empresa.FotoId);
            ViewData["TelefonoId"] = new SelectList(_context.Telefonos, "Id", "Numero", empresa.TelefonoId);
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .Include(e => e.Foto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa.TelefonoId != null)
            {
                empresa.Telefono = _context.Telefonos.Where(_ => _.Id == empresa.TelefonoId).FirstOrDefault();

            }
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empresas == null)
            {
                return Problem("Entity set 'BDContext.Empresas'  is null.");
            }
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
            {
                _context.Empresas.Remove(empresa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
          return _context.Empresas.Any(e => e.Id == id);
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
