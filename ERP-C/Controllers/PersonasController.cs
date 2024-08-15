﻿using System;
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
    public class PersonasController : Controller
    {
        private readonly BDContext _context;

        public PersonasController(BDContext context)
        {
            _context = context;
        }

        // GET: Personas
        public IActionResult Index()
        {
              return View(_context.Personas.ToList());
        }

        // GET: Personas/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = _context.Personas.FirstOrDefault(p => p.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("Id,Nombre,Apellido,DNI,Direccion,UserName,Password,Email,FechaAlta")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                _context.Personas.Add(persona);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona =  _context.Personas.Find(id);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Anotacion: este metodo lo deje como asincronico porque en los videos no lo modificaron.
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,DNI,Direccion,UserName,Password,Email,FechaAlta")] Persona persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var personaEnDB = _context.Personas.Find(persona.Id);
                    if (personaEnDB != null)
                    {
                        //Actualizaremos
                        personaEnDB.Nombre = persona.Nombre;
                        personaEnDB.Apellido = persona.Apellido;
                        personaEnDB.DNI = persona.DNI;
                        personaEnDB.Direccion = persona.Direccion;
                        personaEnDB.UserName = persona.UserName;
                        personaEnDB.Password = persona.Password;
                        personaEnDB.Email = persona.Email;

                        _context.Personas.Update(personaEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.Id))
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
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Personas == null)
            {
                return Problem("Entity set 'BDContext.Personas'  is null.");
            }
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
          return _context.Personas.Any(e => e.Id == id);
        }
    }
}
