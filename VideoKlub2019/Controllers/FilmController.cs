using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoKlub2019.Models;

namespace VideoKlub2019.Controllers
{
    public class FilmController : Controller
    {
        private readonly VideoKlubContext db;

        public FilmController(VideoKlubContext _db)
        {
            db = _db;
        }

        public FileContentResult CitajSliku(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Film film = db.Filmovi.Find(id);

            if (film == null)
            {
                return null;
            }
            return File(film.Slika, film.SlikaTip);
        }

        [Authorize(Roles = "admin")]
        // GET: Film
        public async Task<IActionResult> Index()
        {
            var videoklub2019Context = db.Filmovi.Include(f => f.Zanr);
            return View(await videoklub2019Context.ToListAsync());
        }

        // GET: Film/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await db.Filmovi
                .Include(f => f.Zanr)
                .FirstOrDefaultAsync(m => m.FilmId == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        [Authorize(Roles = "admin")]
        // GET: Film/Create
        public IActionResult Create()
        {
            ViewData["ZanrId"] = new SelectList(db.Zanrovi, "ZanrId", "NazivZanra");
            return View();
        }

        // POST: Film/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FilmId,Naziv,ZanrId,Reziser,Godina,Slika,SlikaTip,CenaPoDanu")] Film film, IFormFile odabranaSlika)
        {
            if (ModelState.IsValid)
            {
            using (MemoryStream ms = new MemoryStream())
            {
                await odabranaSlika.CopyToAsync(ms);
                film.Slika = ms.ToArray();
            }
            film.SlikaTip = odabranaSlika.ContentType;
            db.Add(film);
            await db.SaveChangesAsync();

             }
            ViewData["ZanrId"] = new SelectList(db.Zanrovi, "ZanrId", "NazivZanra", film.ZanrId);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        // GET: Film/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await db.Filmovi.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            ViewData["ZanrId"] = new SelectList(db.Zanrovi, "ZanrId", "NazivZanra", film.ZanrId);
            return View(film);
        }

        // POST: Film/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmId,Naziv,ZanrId,Reziser,Godina,Slika,SlikaTip,CenaPoDanu")] Film film, IFormFile odabranaSlika, int promena = 0)
        {
            if (promena == 1 && odabranaSlika == null)
            {
                ModelState.AddModelError("Slika", "Niste odabrali sliku");
            }
            if (id != film.FilmId)
            {
                return NotFound();
            }
            Film fl = db.Filmovi.Find(film.FilmId);

            if (promena == 1)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await odabranaSlika.CopyToAsync(ms);
                    fl.Slika = ms.ToArray();
                }
                fl.SlikaTip = odabranaSlika.ContentType;
            }
            fl.Naziv = film.Naziv;
            fl.ZanrId = film.ZanrId;
            fl.Reziser = film.Reziser;
            fl.Godina = film.Godina;
            fl.CenaPoDanu = film.CenaPoDanu;
            if (ModelState.IsValid)
            {
            try
            {
                db.Update(fl);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(fl.FilmId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            }
            ViewData["ZanrId"] = new SelectList(db.Zanrovi, "ZanrId", "NazivZanra", film.ZanrId);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "admin")]
        // GET: Film/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await db.Filmovi
                .Include(f => f.Zanr)
                .FirstOrDefaultAsync(m => m.FilmId == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Film/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await db.Filmovi.FindAsync(id);
            db.Filmovi.Remove(film);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return db.Filmovi.Any(e => e.FilmId == id);
        }
    }
}
