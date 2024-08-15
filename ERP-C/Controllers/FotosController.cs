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

namespace ERP_C.Controllers
{
    [Authorize]
    public class FotosController : Controller
    {
        private readonly BDContext _context;

        public FotosController(BDContext context)
        {
            _context = context;
        }

        // GET: Fotos
        public IActionResult Index()
        {
              return View(_context.Fotos.ToList());
        }

        // GET: Fotos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fotos == null)
            {
                return NotFound();
            }

            var foto = await _context.Fotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foto == null)
            {
                return NotFound();
            }

            return View(foto);
        }

        // GET: Fotos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fotos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nombre,Path")] Foto foto)
        {
            if (ModelState.IsValid)
            {
                _context.Fotos.Add(foto);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(foto);
        }

        // GET: Fotos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Fotos == null)
            {
                return NotFound();
            }

            var foto = _context.Fotos.Find(id);
            if (foto == null)
            {
                return NotFound();
            }
            return View(foto);
        }

        // POST: Fotos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Path")] Foto foto)
        {
            if (id != foto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var FotoEnDB = _context.Fotos.Find(foto.Id);
                    if (FotoEnDB == null)
                    {
                        FotoEnDB.Nombre = foto.Nombre;
                        FotoEnDB.Path = foto.Path;

						_context.Update(foto);
						await _context.SaveChangesAsync();
					}
                    else
                    {
                        return NotFound();
                    }
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FotoExists(foto.Id))
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
            return View(foto);
        }

        // GET: Fotos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fotos == null)
            {
                return NotFound();
            }

            var foto = await _context.Fotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foto == null)
            {
                return NotFound();
            }

            return View(foto);
        }

        // POST: Fotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fotos == null)
            {
                return Problem("Entity set 'BDContext.Fotos'  is null.");
            }
            var foto = await _context.Fotos.FindAsync(id);
            if (foto != null)
            {
                _context.Fotos.Remove(foto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FotoExists(int id)
        {
          return _context.Fotos.Any(e => e.Id == id);
        }
    }
}
