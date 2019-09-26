using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoKlub2019.Models;

namespace VideoKlub2019.Controllers
{
    public class IznajmljivanjeController : Controller
    {
        private readonly VideoKlubContext db;

        public IznajmljivanjeController(VideoKlubContext _db)
        {
            db = _db;
        }

        [Authorize(Roles = "admin")]
        // GET: Iznajmljivanje
        public async Task<IActionResult> Index()
        {
            var videoklub2019Context = db.Iznajmljivanja.Include(i => i.Film);
            return View(await videoklub2019Context.ToListAsync());
        }

        // GET: Iznajmljivanje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iznajmljivanje = await db.Iznajmljivanja
                .Include(i => i.Film)
                .FirstOrDefaultAsync(m => m.IznajmljivanjeId == id);
            if (iznajmljivanje == null)
            {
                return NotFound();
            }

            return View(iznajmljivanje);
        }

        [Authorize]
        // GET: Iznajmljivanje/Create
        public IActionResult Create()
        {
            ViewData["FilmId"] = new SelectList(db.Filmovi, "FilmId", "Naziv");
            return View();
        }

        // POST: Iznajmljivanje/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IznajmljivanjeId,FilmId,ClanId,DatumIznajmljivanja,DatumVracanja")] Iznajmljivanje iznajmljivanje)
        {
            if (ModelState.IsValid)
            {
                db.Add(iznajmljivanje);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(db.Filmovi, "FilmId", "Naziv", iznajmljivanje.FilmId);
            return View(iznajmljivanje);
        }

        [Authorize(Roles = "admin")]
        // GET: Iznajmljivanje/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iznajmljivanje = await db.Iznajmljivanja.FindAsync(id);
            if (iznajmljivanje == null)
            {
                return NotFound();
            }
            ViewData["FilmId"] = new SelectList(db.Filmovi, "FilmId", "Naziv", iznajmljivanje.FilmId);
            return View(iznajmljivanje);
        }

        // POST: Iznajmljivanje/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IznajmljivanjeId,FilmId,ClanId,DatumIznajmljivanja,DatumVracanja")] Iznajmljivanje iznajmljivanje)
        {
            if (id != iznajmljivanje.IznajmljivanjeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(iznajmljivanje);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IznajmljivanjeExists(iznajmljivanje.IznajmljivanjeId))
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
            ViewData["FilmId"] = new SelectList(db.Filmovi, "FilmId", "Naziv", iznajmljivanje.FilmId);
            return View(iznajmljivanje);
        }

        [Authorize(Roles = "admin")]
        // GET: Iznajmljivanje/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iznajmljivanje = await db.Iznajmljivanja
                .Include(i => i.Film)
                .FirstOrDefaultAsync(m => m.IznajmljivanjeId == id);
            if (iznajmljivanje == null)
            {
                return NotFound();
            }

            return View(iznajmljivanje);
        }

        // POST: Iznajmljivanje/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iznajmljivanje = await db.Iznajmljivanja.FindAsync(id);
            db.Iznajmljivanja.Remove(iznajmljivanje);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IznajmljivanjeExists(int id)
        {
            return db.Iznajmljivanja.Any(e => e.IznajmljivanjeId == id);
        }
    }
}
