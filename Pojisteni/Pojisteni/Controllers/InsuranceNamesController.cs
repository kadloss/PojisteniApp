using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pojisteni.Data;
using Pojisteni.Models;

namespace Pojisteni.Controllers
{
    [Authorize(Roles="admin")]
    public class InsuranceNamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsuranceNamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InsuranceNames
        public async Task<IActionResult> Index()
        {
            return _context.InsuranceName != null ?
                        View(await _context.InsuranceName.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.InsuranceType'  is null.");
        }

        // GET: InsuranceNames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InsuranceName == null)
            {
                return NotFound();
            }

            var insuranceName = await _context.InsuranceName
                .FirstOrDefaultAsync(m => m.InsuranceNameId == id);
            if (insuranceName == null)
            {
                return NotFound();
            }

            return View(insuranceName);
        }

        // GET: InsuranceNames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InsuranceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsuranceNameId,nazevPojisteni")] InsuranceName insuranceName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insuranceName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insuranceName);
        }

        // GET: InsuranceNames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InsuranceName == null)
            {
                return NotFound();
            }

            var insuranceName = await _context.InsuranceName.FindAsync(id);
            if (insuranceName == null)
            {
                return NotFound();
            }
            return View(insuranceName);
        }

        // POST: InsuranceNames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InsuranceNameId,nazevPojisteni")] InsuranceName insuranceName)
        {
            if (id != insuranceName.InsuranceNameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuranceName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceNameExists(insuranceName.InsuranceNameId))
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
            return View(insuranceName);
        }

        // GET: InsuranceNames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InsuranceName == null)
            {
                return NotFound();
            }

            var insuranceName = await _context.InsuranceName
                .FirstOrDefaultAsync(m => m.InsuranceNameId == id);
            if (insuranceName == null)
            {
                return NotFound();
            }

            return View(insuranceName);
        }

        // POST: InsuranceNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InsuranceName == null)
            {
                return Problem("Entity set 'ApplicationDbContext.InsuranceType'  is null.");
            }
            var insuranceName = await _context.InsuranceName.FindAsync(id);
            if (insuranceName != null)
            {
                _context.InsuranceName.Remove(insuranceName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceNameExists(int id)
        {
            return (_context.InsuranceName?.Any(e => e.InsuranceNameId == id)).GetValueOrDefault();
        }
    }
}
