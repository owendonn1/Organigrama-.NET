using ERP_C.Data;
using ERP_C.Helpers;
using ERP_C.Migrations;
using ERP_C.Models;
using ERP_C.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace ERP_C.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly RoleManager<Rol> _rol;
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly List<string> _roles = new List<string>() {Alias.RoleNombreAdmin, Alias.RoleNombreAdmin, Alias.RoleNombreRRHH };

        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signInManager, RoleManager<Rol> rol)
        {
            this._usermanager = usermanager;
            this._signInManager = signInManager;
            this._rol = rol;
        }
        [Authorize(Roles = "RRHH")]
        public IActionResult Registrar()
        {
            return View();
        }

        [Authorize(Roles = "RRHH")]
        [HttpPost]
        public async Task<IActionResult> Registrar([Bind("Email", "Password", "ConfirmacionPassword", "esRRHH")] RegistrarUsuario ViewModel) 
        {
            if (ModelState.IsValid)
            {
                
                Empleado empleadoCreado = new Empleado()
                {
                    Email = ViewModel.Email,
                    Password = ViewModel.Password,
                    UserName = ViewModel.Email
                };
                var resultado = await _usermanager.CreateAsync(empleadoCreado, ViewModel.Password);
                
                if (resultado.Succeeded)  
                {
                   //Esta parte se puede hacer en un metodo privado aparte
                    var rol = "";
                    if (ViewModel.esRRHH) 
                    {
                        rol = Alias.RoleNombreRRHH;
                    }
                    else
                    {
                        rol = Alias.RoleNombreEmpleado;
                    }
                   
                    var resultadoAddRole = await _usermanager.AddToRoleAsync(empleadoCreado, rol);

                    if (resultadoAddRole.Succeeded)
                    {
                        await _signInManager.SignInAsync(empleadoCreado, isPersistent: false);
                        return RedirectToAction("Index", "Home");
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
            return View(ViewModel);
        }

        public IActionResult IniciarSesion(string returnurl)
        {
            TempData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login iniciarSesion)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(iniciarSesion.Email, iniciarSesion.Password, iniciarSesion.Recordar, false);
                if (resultado.Succeeded)
                {
                    string returnurl = TempData["ReturnUrl"] as string;

                    if (!string.IsNullOrEmpty(returnurl))
                    {
                        return Redirect(returnurl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(String.Empty, "inicio de sesion invalido");
            }
            return View(iniciarSesion);
        }


        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        public IActionResult AccesoDenegado()
        {
            return Content("No tenes permisos");
        }
        


    }
}
