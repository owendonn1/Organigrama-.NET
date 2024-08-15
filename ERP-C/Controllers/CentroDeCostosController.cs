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
using ERP_C.Models.ViewModels;

namespace ERP_C.Controllers
{
    [Authorize]
    public class CentroDeCostosController : Controller
    {
        private readonly BDContext _context;

        public CentroDeCostosController(BDContext context)
        {
            _context = context;
        }

        // GET: CentroDeCostos
        public  IActionResult Index()
        {
            var bDContext = _context.CentroDeCostos.Include(c => c.Gerencia);
            return View( bDContext.ToList());
        }

        // GET: CentroDeCostos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.CentroDeCostos == null)
            {
                return NotFound();
            }

            var centroDeCosto = _context.CentroDeCostos .Include(c => c.Gerencia) .FirstOrDefault(m => m.Id == id);
            if (centroDeCosto == null)
            {
                return NotFound();
            }

            return View(centroDeCosto);
        }
        [Authorize(Roles = "RRHH")]
        // GET: CentroDeCostos/Create
        public IActionResult Create()
        {
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre");
            return View();
        }

        // POST: CentroDeCostos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "RRHH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nombre,MontoMaximo,GerenciaSuperiorId")] CentroDeCosto centroDeCosto)
        {
            double Monto = 10000;
            centroDeCosto.MontoMaximo = Monto;
            centroDeCosto.Gastos = new List<Gasto>();
            var gerenciaEnDb = _context.Gerencias.Find(centroDeCosto.GerenciaId);
            if (gerenciaEnDb == null) 
            { 
                return NotFound(); 
            }

            centroDeCosto.Gerencia = gerenciaEnDb;

            if (ModelState.IsValid)
            {
                _context.CentroDeCostos.Add(centroDeCosto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre", centroDeCosto.GerenciaId);
            return View(centroDeCosto);
        }
        [Authorize(Roles = "RRHH")]
        // GET: CentroDeCostos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.CentroDeCostos == null)
            {
                return NotFound();
            }

            var centroDeCosto =  _context.CentroDeCostos.Find(id);
            if (centroDeCosto == null)
            {
                return NotFound();
            }
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "Id", "Nombre", centroDeCosto.GerenciaId);
            return View(centroDeCosto);
        }

        // POST: CentroDeCostos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "RRHH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,Nombre,MontoMaximo,GerenciaSuperiorId")] CentroDeCosto centroDeCosto)
        {
            // Obtener el centro de costo desde la base de datos

            // Validar si el monto del gasto supera el monto máximo del centro de costo


            if (ModelState.IsValid)

            {

                var centroDeCostoEnDB = _context.CentroDeCostos.Find(centroDeCosto.Id);

                if (centroDeCostoEnDB == null)
                {
                    return NotFound();
                }
                else
                {
                    //Actualizaremos
                    centroDeCostoEnDB.MontoMaximo = centroDeCosto.MontoMaximo;


                    _context.CentroDeCostos.Update(centroDeCostoEnDB);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
            }
            return View(centroDeCosto);
        }


        // GET: CentroDeCostos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CentroDeCostos == null)
            {
                return NotFound();
            }

            var centroDeCosto = await _context.CentroDeCostos
                .Include(c => c.Gerencia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (centroDeCosto == null)
            {
                return NotFound();
            }

            return View(centroDeCosto);
        }

        // POST: CentroDeCostos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CentroDeCostos == null)
            {
                return Problem("Entity set 'BDContext.CentroDeCostos'  is null.");
            }
            var centroDeCosto = await _context.CentroDeCostos.FindAsync(id);
            if (centroDeCosto != null)
            {
                _context.CentroDeCostos.Remove(centroDeCosto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ExisteEmpleado(List<Posicion> Posiciones, int Id)
        {
            foreach(var posicion in Posiciones)
            {
                if (posicion.EmpleadoId == Id)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CentroDeCostoExists(int id)
        {
          return _context.CentroDeCostos.Any(e => e.Id == id);
        }

        [Authorize(Roles = "RRHH")]
        public IActionResult ListaM()  //ListarMontosTotalesPorGerencia
        {
            try
            {
                // Obtener todos los centros de costo con sus gastos asociados
                var centrosDeCostoConGastos = _context.CentroDeCostos
                    .Include(c => c.Gerencia)
                    .Include(c => c.Gastos)
                    .ToList();
                foreach(CentroDeCosto centro in centrosDeCostoConGastos)
                {
                    centro.Gastos = _context.Gastos.Where(b => b.CentroDeCostoId == centro.Id).ToList();
                }
                // Agrupar los centros de costo por gerencia y sumar los montos de gastos
                var montosPorGerencia = centrosDeCostoConGastos
                    .GroupBy(c => c.Gerencia)
                    .Select(group => new
                    {
                        Gerencia = group.Key,
                        MontoTotal = group.Sum(c => c.Gastos.Sum(g => g.Monto))
                    })
                    .OrderByDescending(result => result.MontoTotal)
                    .ToList();
                var gerenciaMontos = new List<GerenciasMontos>();
                foreach(var item in montosPorGerencia)
                {
                    gerenciaMontos.Add(new GerenciasMontos(item.Gerencia, item.MontoTotal));
                }

                return View(gerenciaMontos);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al listar los montos totales por gerencia: {ex.Message}");
                return RedirectToAction("Error");
            }
        }


    }
}
