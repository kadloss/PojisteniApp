﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pojisteni.Data;
using Pojisteni.Models;

namespace Pojisteni.Controllers
{
    public class InsurancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public InsurancesController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        // GET: Insurances
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Insurance.Include(i => i.InsuranceName).Include(i => i.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Insurances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Insurance == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance
                .Include(i => i.InsuranceName)
                .Include(i => i.Person)
                .FirstOrDefaultAsync(m => m.InsuranceId == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // GET: Insurances/Create
        public IActionResult Create(int? id)
        {
            ViewData["InsuranceNameId"] = new SelectList(_context.InsuranceName, "InsuranceNameId", "nazevPojisteni");
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "celeJmenosAdresou", id);
            ViewData["IsDefinedPerson"] = false;
            ViewData["CustomTitle"] = "Nové pojištění";
            TempData["PreviousUrl"] = Request.Headers["Referer"].ToString(); // Saves URL user is coming from to be used in POST Create action
            // Create insurance instance and assign default values
            Insurance insurance = new Insurance();
            insurance.platnostOd = DateTime.Now.Date;
            insurance.platnostDo = insurance.platnostOd.AddYears(1);
            if (id != null) // PersonId was routed to this action
            {
                // Insurance will be created for person defined by PersonId
                ViewData["IsDefinedPerson"] = true;
                insurance.PersonId = (int)id;

                // Person name for the view title
                var person = _context.Person.Find(id);
                if (person != null)
                {
                    ViewData["CustomTitle"] = ViewData["CustomTitle"] + string.Format($" pro {person.celeJmeno}");
                }
            }
            return View(insurance);
        }

        // POST: Insurances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsuranceId,InsuranceNameId,Castka,predmetPojisteni,platnostOd,platnostDo,PersonId")] Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                _notyf.Success("Nové pojištění přidáno");

                // Redirect to previous URL if available
                if (TryGetPreviousUrl(out string previousUrl))
                {
                    return Redirect(previousUrl);
                }

                // Redirect to defaul action People -> Details if condition above is not true
                return RedirectToAction("Details", "People", new { id = insurance.PersonId });
            }
            ViewData["InsuranceNameId"] = new SelectList(_context.InsuranceName, "InsuranceNameId", "nazevPojisteni", insurance.InsuranceNameId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "celeJmenosAdresou", insurance.PersonId);
            ViewData["IsDefinedPerson"] = false;
            ViewData["CustomTitle"] = "Nové pojištění";
            // Person name for the view title
            var person = _context.Person.Find(insurance.PersonId);
            if (person != null)
            {
                ViewData["IsDefinedPerson"] = true;
                ViewData["CustomTitle"] = ViewData["CustomTitle"] + string.Format($" pro {person.celeJmeno}");
            }
            return View(insurance);
        }

        // GET: Insurances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Insurance == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }
            ViewData["InsuranceNameId"] = new SelectList(_context.InsuranceName, "InsuranceNameId", "nazevPojisteni", insurance.InsuranceNameId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "celeJmenosAdresou", insurance.PersonId);
            TempData["PreviousUrl"] = Request.Headers["Referer"].ToString(); // Saves URL user is coming from to be used in POST Edit action
            return View(insurance);
        }

        // POST: Insurances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InsuranceId,InsuranceNameId,Castka,predmetPojisteni,platnostOd,platnostDo,PersonId")] Insurance insurance)
        {
            if (id != insurance.InsuranceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                    _notyf.Information("Změny pojištění uloženy");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.InsuranceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Redirect to previous URL if available
                if (TryGetPreviousUrl(out string previousUrl))
                {
                    return Redirect(previousUrl);
                }

                // Redirect to defaul action Insurances -> Index if condition above is not true
                return RedirectToAction(nameof(Index));
            }
            ViewData["InsuranceNameId"] = new SelectList(_context.InsuranceName, "InsuranceNameId", "nazevPojisteni", insurance.InsuranceNameId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "celeJmenosAdresou", insurance.PersonId);
            return View(insurance);
        }

        // GET: Insurances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Insurance == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance
                .Include(i => i.InsuranceName)
                .Include(i => i.Person)
                .FirstOrDefaultAsync(m => m.InsuranceId == id);
            if (insurance == null)
            {
                return NotFound();
            }

            TempData["PreviousUrl"] = Request.Headers["Referer"].ToString(); // Saves URL user is coming from to be used in POST Delete action
            return View(insurance);
        }

        // POST: Insurances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Insurance == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Insurance'  is null.");
            }
            var insurance = await _context.Insurance.FindAsync(id);
            if (insurance != null)
            {
                _context.Insurance.Remove(insurance);
                _notyf.Warning($"Pojištění smazáno");
            }

            await _context.SaveChangesAsync();

            // Redirect to previous URL if available
            if (TryGetPreviousUrl(out string previousUrl))
            {
                return Redirect(previousUrl);
            }

            // Redirect to defaul action Insurances -> Index if condition above is not true
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceExists(int id)
        {
            return (_context.Insurance?.Any(e => e.InsuranceId == id)).GetValueOrDefault();
        }

        private bool TryGetPreviousUrl(out string? previousUrl)
        {
            bool result = false;
            previousUrl = null;
            // Check if URL user is coming from is available
            if (TempData.ContainsKey("PreviousUrl"))
            {
                if (TempData["PreviousUrl"] != null)
                {
                    previousUrl = TempData["PreviousUrl"].ToString();
                    result = true;
                }
            }
            return result;
        }
    }
}
