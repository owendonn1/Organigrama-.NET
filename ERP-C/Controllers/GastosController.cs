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
using Microsoft.AspNetCore.Identity;
using ERP_C.Models.ViewModels;

namespace ERP_C.Controllers
{
    [Authorize]
    public class GastosController : Controller
    {
        private readonly BDContext _context;
        private readonly SignInManager<Persona> _signInManager;
        private readonly UserManager<Persona> _usermanager;

        public GastosController(BDContext context, SignInManager<Persona> persona, UserManager<Persona> usuario)
        {
            _context = context;
            this._signInManager = persona;
            this._usermanager = usuario;


        }

        // GET: Gastos
        public IActionResult Index()
        {
            var bDContext = _context.Gastos.Include(g => g.CentroDeCosto).Include(g => g.Empleado);
            return View(bDContext.ToList());
        }

        // GET: Gastos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Gastos == null)
            {
                return NotFound();
            }

            var gasto = _context.Gastos
                .Include(g => g.CentroDeCosto)
                .Include(g => g.Empleado)
                .FirstOrDefault(m => m.Id == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // GET: Gastos/Create
        public IActionResult Create()
        {
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentroDeCostos, "Id", "Nombre");
            return View();
        }
        // definicion 
        private bool GastoExists(int id)
        {
            return _context.Gastos.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Descripcion,Monto,Fecha,CentroDeCostoId")] Gasto gasto)
        {
            Empleado empleadoenDB = ObtenerEmpleadoActualId();
            gasto.EmpleadoId = empleadoenDB.Id;


            if (ModelState.IsValid)
            {
                //if que si recive -1 devuelva el usuario es incorrecto


                // Busco el Gerencia en DB
                var centroDeCosto = _context.CentroDeCostos.Find(gasto.CentroDeCostoId);
                try
                {
                    // una vez que se arregle posicion agregar validacion: && centroDeCosto.GerenciaId==empleadoenDB.Posicion.GerenciaId

                    if (centroDeCosto != null)
                    {
                        var posicion = _context.Posiciones.Where(b => b.EmpleadoId == empleadoenDB.Id)
                     .FirstOrDefault();
                        if (posicion.GerenciaId == centroDeCosto.GerenciaId)
                        {
                            if (gasto.Monto <= centroDeCosto.MontoMaximo)
                            {
                                gasto.CentroDeCosto = centroDeCosto;
                                gasto.CentroDeCostoId = centroDeCosto.Id;
                                gasto.Empleado = empleadoenDB;
                                // Asignar el gasto al centro de costo
                                if (centroDeCosto.Gastos == null)
                                {
                                    centroDeCosto.Gastos = new List<Gasto>();

                                }
                                centroDeCosto.Gastos.Add(gasto);
                                _context.CentroDeCostos.Update(centroDeCosto);
                                // Resto del código para guardar el gasto
                                _context.SaveChanges();

                                return RedirectToAction(nameof(Index)); // Redirigir a la página de lista de gastos 
                            }
                            else
                            {
                                ModelState.AddModelError("Monto", "El Monto de gasto supera el monto maximo permitido en el Centro de Costo.");

                            }
                        }
                        else
                        {
                            ModelState.AddModelError("CentroDeCostoId", "El centro de costo seleccionado no corresponde al Empleado conectado.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("CentroDeCostoId", "El centro de costo seleccionado no existe.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error al guardar el gasto: {ex.Message}");
                }
            }

            return View(gasto);
        }
        [Authorize(Roles = "Admin")]
        // GET: Gastos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Gastos == null)
            {
                return NotFound();
            }

            var gasto = _context.Gastos.Find(id);
            if (gasto == null)
            {
                return NotFound();
            }
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentroDeCostos, "Id", "Nombre", gasto.CentroDeCostoId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", gasto.EmpleadoId);
            return View(gasto);
        }

        // POST: Gastos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Descripcion,Monto,Fecha,CentroDeCostoId")] Gasto gasto)
        {
            if (id != gasto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Persona empleadoenDB = ObtenerEmpleadoActualId();
                    var GastoEnDB = _context.Gastos.Find(gasto.Id);
                    var empleado = _context.Empleados.Find(empleadoenDB.Id);

                    if (GastoEnDB != null && empleado.Posicion.GerenciaId==GastoEnDB.CentroDeCosto.GerenciaId)
                    {
                        GastoEnDB.Descripcion = gasto.Descripcion;
                        GastoEnDB.Monto = gasto.Monto;
                        GastoEnDB.Fecha = gasto.Fecha;

                        _context.Gastos.Update(GastoEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }

                    _context.Update(gasto);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoExists(gasto.Id))
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
            ViewData["CentroDeCostoId"] = new SelectList(_context.CentroDeCostos, "Id", "Nombre", gasto.CentroDeCostoId);
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", gasto.EmpleadoId);
            return View(gasto);
        }
        [Authorize(Roles = "Admin")]
        // GET: Gastos/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Gastos == null)
            {
                return NotFound();
            }

            var gasto = _context.Gastos
                .Include(g => g.CentroDeCosto)
                .Include(g => g.Empleado)
                .FirstOrDefault(m => m.Id == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // POST: Gastos/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Gastos == null)
            {
                return Problem("Entity set 'BDContext.Gastos'  is null.");
            }
            var gasto = _context.Gastos.Find(id);
            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        
        public Empleado ObtenerEmpleadoActualId()

        {
            if (_signInManager.IsSignedIn(User))
            {
                int personaId = Int32.Parse(_usermanager.GetUserId(User));

                Empleado empleado = _context.Empleados.Find(personaId);

                return empleado;
            }

            return null;
        }

        //[Authorize(Roles = "RRHH")]
        public IActionResult ListarGastosEmpleado(int empleadoId)
        {
            try
            {
                //empleado actual
                Empleado empleadoActual = ObtenerEmpleadoActualId();
                empleadoActual.Gastos = _context.Gastos.Where(b => b.EmpleadoId == empleadoActual.Id).ToList();

                // Verifica si el empleado existe y tiene gastos asociados
                if (empleadoActual != null && empleadoActual.Gastos != null && empleadoActual.Gastos.Any())
                {
                    // Ordena los gastos del empleado por fecha de manera decreciente
                    var gastosOrdenados = empleadoActual.Gastos.OrderByDescending(g => g.Monto).ThenBy(g => g.Fecha);

                    return View(gastosOrdenados.ToList());
                }
                else
                {
                    
                    return View(new List<Gasto>()); 
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al listar los gastos: {ex.Message}");
                return View(new List<Gasto>());
            }
        }
        /*
         * Otra manera de solucionar Listar gasto por empleados (no funciona)
         * //el usuario tiene permisos para ver los gastos del empleado
        Empleado usuarioActual = ObtenerEmpleadoActualId();
        if (usuarioActual == null || usuarioActual.Id != empleadoId)
        {
            return  Content("Solo puede ordenar sus propios gastos");
        }

        // Obtener el empleado y sus gastos desde la base de datos
        var empleadoConGastos = _context.Empleados
            .Include(e => e.Gastos) 
            .FirstOrDefault(e => e.Id == empleadoId);

        if (empleadoConGastos == null)
        {
            return Content("No disponible"); 
        }

        //Listar sus gastos en orden decreciente y por fecha.
        var gastosFiltradosOrdenados = empleadoConGastos.Gastos
            .OrderByDescending(g => g.Monto)
            .ThenBy(g => g.Fecha);
        return View(gastosFiltradosOrdenados.ToList());
        */
        [Authorize(Roles = "RRHH")]
        public IActionResult ListarTodosLosGastos()
        {
            try
            {
                // Obtén todos los gastos de todos los empleados
                var todosLosGastos = _context.Gastos
                    .Include(g => g.Empleado) 
                    .Include(g => g.CentroDeCosto)
                    .OrderByDescending(g => g.Fecha) // Ordena por fecha de manera decreciente
                    .ThenBy(g => g.Empleado.Apellido)
                    .ThenBy(g => g.Empleado.Nombre)
                    .ToList();

                return View(todosLosGastos);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al listar todos los gastos: {ex.Message}");
                return RedirectToAction("Error");
            }
        }



        //private int ObtenerEmpleadoActualId()
        //{
        //    // puedes obtener el nombre del usuario actual y buscar el ID del empleado
        //    var nombreUsuario = User.Identity.Name;

        //    // Lógica para buscar el ID del empleado según el nombre de usuario
        //    var empleado = _context.Empleados.FirstOrDefault(e => e.UserName == nombreUsuario);

        //    // Verificar si se encontró el empleado y devolver su ID
        //    if (empleado != null)
        //    {
        //        return empleado.Id;
        //    }

        //    // Si no se encuentra el empleado, puedes manejarlo según tus requisitos.
        //    return -1;
        //}


    }
}
