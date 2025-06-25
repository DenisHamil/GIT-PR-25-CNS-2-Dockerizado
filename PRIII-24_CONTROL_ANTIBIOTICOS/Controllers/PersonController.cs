using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private readonly BdProa1Context _context;

        public PersonController(BdProa1Context context)
        {
            _context = context;
        }

        // GET: Person
        public async Task<IActionResult> Index()
        {
            return View(await _context.People.ToListAsync());
        }
        // GET: Person/RegisterUser
        public IActionResult RegisterUser()
        {
            return View();
        }
        public IActionResult IndexUser()
        {
            return View();
        }
        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.IdPerson == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Person/Create
        public IActionResult RegisterPatient()
        {
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPatient([Bind("IdPerson,Name,LastName,SecondLastName,Ci,DateOfBirth,Gender")] Person person)
        {
            var patient = new Patient();
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();

                // Crear un nuevo paciente y generar el código de paciente
                patient = new Patient
                {
                    IdPatient = person.IdPerson,
                    PatientCode = GeneratePatientCode(person) // Método para generar el código
                };

                _context.Add(patient);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Patient");
            }
            return View();
        }

        // Método para generar el código del paciente
        private string GeneratePatientCode(Person person)
        {
            // Primera letra del primer apellido en mayúscula
            string firstLetterLastName = person.LastName.Substring(0, 1).ToUpper();

            // Primera letra del segundo apellido, si no es null
            string firstLetterSecondLastName = person.SecondLastName != null ? person.SecondLastName.Substring(0, 1).ToUpper() : "";

            // Primera letra del primer nombre en mayúscula
            string firstLetterFirstName = person.Name.Substring(0, 1).ToUpper();

            // CI completo
            string ci = person.Ci;

            // Concatenar todo en el formato deseado
            return $"{firstLetterLastName}{firstLetterSecondLastName}{firstLetterFirstName}{ci}";
        }


        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPerson,Name,LastName,SecondLastName,Ci,DateOfBirth,Gender")] Person person)
        {
            if (id != person.IdPerson)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.IdPerson))
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
            return View(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.IdPerson == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.IdPerson == id);
        }
    }
}
