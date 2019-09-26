using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoKlub2019.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VideoKlub2019.Data;

namespace VideoKlub2019.Controllers
{
    public class KomentarController : Controller
    {
        private readonly VideoKlubContext db;

        public KomentarController(VideoKlubContext _db)
        {
            db = _db;
        }

        // GET: Komentar
        public async Task<IActionResult> Index(int id=0)
        {
            ViewBag.Film = db.Filmovi.Where(f => f.FilmId == id);
            var videoKlubContext = db.Komentari.Where(k=>k.FilmId==id).Include(k => k.Film);
            return View(await videoKlubContext.ToListAsync());
        }

        // GET: Komentar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await db.Komentari
                .Include(k => k.Film)
                .FirstOrDefaultAsync(m => m.KomentarId == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        [Authorize]
        // GET: Komentar/Create
        public IActionResult Create(int id=0)
        {
            ViewBag.Id = id;
            ViewData["FilmId"] = new SelectList(db.Filmovi, "FilmId", "Naziv");
            return View();
        }

        // POST: Komentar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KomentarId,ClanId,Korisnik,Poruka,FilmId")] Komentar komentar)
        {
            if (ModelState.IsValid)
            {
                db.Add(komentar);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewData["FilmId"] = new SelectList(db.Filmovi, "FilmId", "Naziv", komentar.FilmId);
            return RedirectToAction("Index","Home");
        }
        [Authorize]
        // GET: Komentar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await db.Komentari.FindAsync(id);
            if (komentar == null)
            {
                return NotFound();
            }
            ViewData["FilmId"] = new SelectList(db.Filmovi, "FilmId", "Naziv", komentar.FilmId);
            return View(komentar);
        }
        [Authorize]
        // POST: Komentar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KomentarId,ClanId,Korisnik,Poruka,FilmId")] Komentar komentar)
        {

                if (id != komentar.KomentarId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        db.Update(komentar);
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!KomentarExists(komentar.KomentarId))
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
                ViewData["FilmId"] = new SelectList(db.Filmovi, "FilmId", "Naziv", komentar.FilmId);
                return View(komentar);
            
        }

        // GET: Komentar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await db.Komentari
                .Include(k => k.Film)
                .FirstOrDefaultAsync(m => m.KomentarId == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // POST: Komentar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var komentar = await db.Komentari.FindAsync(id);
            db.Komentari.Remove(komentar);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool KomentarExists(int id)
        {
            return db.Komentari.Any(e => e.KomentarId == id);
        }
    }
}
