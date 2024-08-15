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
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Collections;
using ERP_C.Helpers;
using Microsoft.AspNetCore.Identity;
using ERP_C.Models.ViewModels;

namespace ERP_C.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly BDContext _context;
        private readonly UserManager<Persona> _userManager;


        public EmpleadosController(BDContext context,UserManager<Persona> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        // GET: Empleados
        public IActionResult Index()
        {
            var bDContext = _context.Empleados.Include(e => e.Foto).ToList();
            foreach (var item in bDContext)
            {
                item.Posicion=_context.Posiciones.Where(b => b.EmpleadoId == item.Id).FirstOrDefault();
                item.Telefono=_context.Telefonos.Where(b => b.EmpleadoId == item.Id).FirstOrDefault();
            }
            return View(bDContext);
        }
        // [Authorize(Roles = "RRHH")]
        // GET: Empleados/Details/5
        public  IActionResult Details(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = _context.Empleados.Include(e => e.Foto).FirstOrDefault(e => e.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }
            empleado.Posicion = _context.Posiciones.Where(b => b.EmpleadoId == empleado.Id).FirstOrDefault();
            empleado.Telefono = _context.Telefonos.Where(b => b.EmpleadoId == empleado.Id).FirstOrDefault();

            int ClaimId = Misc.GetId(User);
            if (User.IsInRole("Empleado") && ClaimId != id)
            {
                return Content("No puede ver el perfil de otro empelado");
            }

            return View(empleado);
        }
        [Authorize(Roles = "RRHH")]
        // GET: Empleados/Create
        public IActionResult Create()
        {
            ViewData["FotoId"] = new SelectList(_context.Fotos, "Id", "Nombre");
            ViewData["Posicion"] = new SelectList(_context.Posiciones, "Id", "Nombre");

            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "RRHH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("Id,ObrasocialEmpleado,Legajo,Activo,FotoId,Posicion,Nombre,Apellido,DNI,Direccion,esRRHH,Email,FechaAlta")] CrearEmpleado viewmodel)
        {
            if (ModelState.IsValid)
            {
                

                Empleado empleado = new Empleado();
                var DNI = viewmodel.DNI.ToString();
                empleado.ObrasocialEmpleado= viewmodel.ObrasocialEmpleado;
                empleado.Legajo= viewmodel.Legajo;
                empleado.Activo= viewmodel.Activo; 
                empleado.FotoId= viewmodel.FotoId;
                empleado.Posicion= viewmodel.Posicion;
                //falta gastos
                empleado.Nombre= viewmodel.Nombre;
                empleado.Apellido= viewmodel.Apellido;
                empleado.DNI= viewmodel.DNI;             
                empleado.Direccion= viewmodel.Direccion;
                empleado.UserName = viewmodel.Email;       
                empleado.Password = DNI;    //DNI   
                empleado.Email= viewmodel.Email;           
                empleado.FechaAlta= viewmodel.FechaAlta;
                

                
                var resultado = await _userManager.CreateAsync(empleado, DNI); //DNI
                if (resultado.Succeeded)
                {
                    var rol = "";
                    if (viewmodel.esRRHH)
                    {
                        rol = Alias.RoleNombreRRHH;
                    }
                    else
                    {
                        rol = Alias.RoleNombreEmpleado;
                    }

                    var resultadoR = await _userManager.AddToRoleAsync(empleado, rol);
                    
                    if (resultadoR.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, $"No se puede agregar el rol de {rol}");
                    }
                }
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            
            ViewData["FotoId"] = new SelectList(_context.Fotos, "Id", "Nombre", viewmodel.FotoId);
            return View(viewmodel);
        }

        // GET: Empleados/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = _context.Empleados.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }

            int ClaimId = Misc.GetId(User);
            if (User.IsInRole("Empleado") && ClaimId!= id)
            {
                return Content("No puede editar el perfil de otro empelado");
            }
            var posicion=_context.Posiciones.Where(x => x.EmpleadoId==id).FirstOrDefault();
            if (posicion != null)
            {
                empleado.PosicionId = posicion.Id;
            }
            ViewData["PosicionId"] = new SelectList(_context.Posiciones, "Id", "Nombre",empleado.PosicionId);
            ViewData["FotoId"] = new SelectList(_context.Fotos, "Id", "Nombre", empleado.FotoId);
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ObrasocialEmpleado,Activo,FotoId,Nombre,Apellido,PosicionId,DNI,ion,UserName,Password,Email,Direccion,Legajo")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var EmpleadoEnBD = _context.Empleados.Find(empleado.Id);
                    var PosicionVieja = _context.Posiciones.Where(x => x.EmpleadoId == id).FirstOrDefault();
                    var PosicionEnDB = _context.Posiciones.Find(empleado.PosicionId);
                    if (EmpleadoEnBD != null)
                    {
                        /*
                        if (User.IsInRole("Empleado")) //Un empleado, solo puede actualizar sus teléfonos y foto
                        {
                            if (PosicionVieja==null || PosicionVieja.Id!=PosicionEnDB.Id)
                            {
                                if (PosicionVieja!=null)
                                {
                                    PosicionVieja.EmpleadoId = null;
                            _context.Update(PosicionVieja); 
                                }                               
                            PosicionEnDB.EmpleadoId= id;
                            _context.Update(PosicionEnDB);  
                            }
                        
                        }
                        */
                        //actualizamos
                        if (User.IsInRole("RRHH"))
                        {
                            if (PosicionVieja == null || PosicionVieja.Id != PosicionEnDB.Id)
                            {
                                if (PosicionVieja != null)
                                {
                                    PosicionVieja.EmpleadoId = null;
                                    _context.Update(PosicionVieja);
                                }
                                PosicionEnDB.EmpleadoId = id;
                                _context.Update(PosicionEnDB);
                            }
                            EmpleadoEnBD.ObrasocialEmpleado = empleado.ObrasocialEmpleado;
                            //EmpleadoEnBD.Legajo = empleado.Legajo;
                            EmpleadoEnBD.Activo = empleado.Activo;
                            EmpleadoEnBD.FotoId = empleado.FotoId;
                            EmpleadoEnBD.Nombre = empleado.Nombre;
                            EmpleadoEnBD.Apellido = empleado.Apellido;
                            EmpleadoEnBD.DNI = empleado.DNI;
                            EmpleadoEnBD.UserName = empleado.UserName;
                            EmpleadoEnBD.Password = empleado.Password;
                            EmpleadoEnBD.Email = empleado.Email;
                            EmpleadoEnBD.Direccion = empleado.Direccion;
                            EmpleadoEnBD.NormalizedUserName=empleado.UserName.ToUpper();
                            EmpleadoEnBD.NormalizedEmail=empleado.Email.ToUpper();
                            EmpleadoEnBD.PasswordHash= _userManager.PasswordHasher.HashPassword(empleado, empleado.Password);
                            //EmpleadoEnBD.FechaAlta = empleado.FechaAlta; 

                        }
                        _context.Empleados.Update(EmpleadoEnBD);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
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
     
            ViewData["Posicion"] = new SelectList(_context.Posiciones, "Id", "Nombre");
            ViewData["FotoId"] = new SelectList(_context.Fotos, "Id", "Nombre", empleado.FotoId);
            return View(empleado);
        }
        [Authorize(Roles = "Admin")]
        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.Include(e => e.Foto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }
            empleado.Telefono = _context.Telefonos.Where(b => b.EmpleadoId == empleado.Id).FirstOrDefault();

            return View(empleado);
        }
        
        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] //RRHH solo lo puede restringir: No va a tener acceso a delete, pero si al edit donde puede acceder al check box de activo y cambiarlo
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empleados == null)
            {
                return Problem("Entity set 'BDContext.Empleados'  is null.");
            }
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
            }
            empleado.Telefono = _context.Telefonos.Where(b => b.EmpleadoId == empleado.Id).FirstOrDefault();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
          return _context.Empleados.Any(e => e.Id == id);
        }

        // GET: Empleados/Restringir/5
        [Authorize(Roles = "RRHH")]
        public IActionResult Restringir(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }
            var empleado = _context.Empleados.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }
            empleado.Activo = false;
            _context.Empleados.Update(empleado);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));  
        }


        //// POST: Empleados/Restringir/5
        //[Authorize(Roles = "RRHH")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Restringir(int id, [Bind("Activo")] Empleado empleado)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var EmpleadoEnBD = _context.Empleados.Find(empleado.Id);
        //        if (EmpleadoEnBD != null)
        //        {
        //            empleado.Activo = false;
        //            _context.Empleados.Update(EmpleadoEnBD);
        //            _context.SaveChanges();
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(empleado);

        //}

        }
}
