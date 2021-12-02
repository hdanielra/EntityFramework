using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkIntro.Data;
using EntityFrameworkIntro.Models;

namespace EntityFrameworkIntro.Controllers
{
    public class ClientsController : Controller
    {
        private readonly EntityFrameworkIntroContext _context;

        public ClientsController(EntityFrameworkIntroContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {

            // de la tabla Clients (representación en C#) obtenga la lista (Async solo ssignifica que es de manera asincrona)
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // el primero que encuentre para el cual se cumpla la condición
            // se presenta una iteración en c/u de los clientes
            // el elemento m que cumpla m.Id = id (operación lambda)

            var clients = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Birthdate,Address")] Clients clients)
        {
            // se recibe un objeto Clients
            // si es válido el formulario
            // Insertar

            if (ModelState.IsValid)
            {
                // Agregar al objeto del contexto de la BD 
                _context.Add(clients);

                // guardar/sincronizar cambios entre objeto C# y BD
                await _context.SaveChangesAsync();

                // redirecciona al index
                return RedirectToAction(nameof(Index));
            }
            return View(clients);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // buscar el cliente por ID (primary key)
            var clients = await _context.Clients.FindAsync(id);

            if (clients == null)
            {
                return NotFound();
            }

            // devuelve la vista con los datos del cliente
            return View(clients);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Birthdate,Address")] Clients clients)
        {
            // valida el id pasado como parámetro sea igual al del objeto de modificación
            if (id != clients.Id)
            {
                return NotFound();
            }


            // si se valida el modelo
            if (ModelState.IsValid)
            {
                try
                {
                    // se actualiza el objeto 
                    _context.Update(clients);

                    // se sincroniza con la BD
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientsExists(clients.Id))
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
            return View(clients);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // buscar el cliente por ID (primary key)
            var clients = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);

            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // buscar el cliente por ID (primary key)
            var clients = await _context.Clients.FindAsync(id);

            // elimina el objeto del contexto
            _context.Clients.Remove(clients);

            // sincroniza con la BD
            await _context.SaveChangesAsync();

            // redirecciona al index
            return RedirectToAction(nameof(Index));
        }

        private bool ClientsExists(int id)
        {
            // busca alguna coincidencia por id
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
