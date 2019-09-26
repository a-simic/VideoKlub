using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VideoKlub2019.Data;
using VideoKlub2019.Models;
using Microsoft.AspNetCore.Authorization;

namespace VideoKlub2019.Controllers
{
    public class HomeController : Controller
    {
        private readonly VideoKlubContext db;
        private readonly RoleManager<IdentityRole> rm;
        private readonly UserManager<ApplicationUser> um;
        public HomeController(VideoKlubContext _db, RoleManager<IdentityRole> _rm, UserManager<ApplicationUser> _um)
        {
            db = _db;
            rm = _rm;
            um = _um;

        }
        public IActionResult Index()
        {
            ViewBag.Zanrovi = new SelectList(db.Zanrovi, "ZanrId", "NazivZanra");
            return View(db.Filmovi);
        }

        public IActionResult Posalji()
        {
            ViewBag.Poruka = "";
            return View();
        }

        private async Task<int> KreirajRolu(string rola)
        {
            bool rolaPostoji = await rm.RoleExistsAsync(rola);

            if (rolaPostoji)
            {
                return 0;
            }
            else
            {
                IdentityRole rolaAdmin = new IdentityRole(rola);
                var rezultat = await rm.CreateAsync(rolaAdmin);
                if (rezultat.Succeeded)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        private async Task<ApplicationUser> KreirajAdministratora()
        {
            ApplicationUser admin = await um.FindByEmailAsync("admin@gmail.com");
            if (admin == null)
            {
                //Novi korisnik
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    KorisnickoIme = "admin",
                    Email = "admin@gmail.com",
                    Ime = "Aleksandar",
                    Prezime = "Simic"
                };
                string lozinka = "admin123";
                var rezultat = await um.CreateAsync(admin, lozinka);
                if (rezultat.Succeeded)
                {
                    return admin;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //admin vec postoji
                return admin;
            }
        }

        public async Task<IActionResult> AdminSistema()
        {
            int rolaPostoji = await KreirajRolu("admin");
            ApplicationUser admin = await KreirajAdministratora();
            if (rolaPostoji == -1)
            {
                ViewBag.Poruka = "Greska pri kreiranju role";
                return View();
            }
            if (admin == null)
            {
                ViewBag.Poruka = "Greska pri kreiranju admina";
                return View();
            }
            bool rezultat1 = await um.IsInRoleAsync(admin, "admin");
            if (rezultat1)
            {
                ViewBag.Poruka = "Korisnik je vec u roli admin";
                return View();
            }

            var rezultat = await um.AddToRoleAsync(admin, "admin");
            if (rezultat.Succeeded)
            {
                ViewBag.Poruka = "Kreiran administrator sistema";
            }

            else
            {
                ViewBag.Poruka = "Greska pri dodavanju korisnika u rolu";
            }
            return View();
        }


        public PartialViewResult _TraziFilmove(string deoNaslova, int id = 0)
        {
            ViewBag.Zanrovi = new SelectList(db.Zanrovi, "ZanrId", "NazivZanra");
            IEnumerable<Film> listaFilmova = db.Filmovi;
            if (id != 0)
            {
                Zanr z1 = db.Zanrovi.Find(id);
                if (z1 != null)
                {
                    ViewBag.Zanr = z1.NazivZanra;
                    listaFilmova = listaFilmova.Where(z => z.ZanrId == id);
                }
                else
                {
                    ViewBag.Zanr = "";
                    return PartialView();
                }
            }
            else
            {
                ViewBag.Zanr = "Svi filmovi";
            }
            if (!string.IsNullOrWhiteSpace(deoNaslova))
            {
                listaFilmova = listaFilmova
                .Where(f => f.Naziv.ToLower().Contains(deoNaslova.ToLower()));
            }
            return PartialView(listaFilmova);
        }


        [HttpPost]
        public IActionResult Posalji(string ime, string prezime, string email, string poruka)
        {
            
            MailAddress admin = new MailAddress("tripledoublemachine23@gmail.com");
            MailAddress posiljaoc = new MailAddress(email, ime + " " + prezime);
            MailMessage msg = new MailMessage();
            msg.To.Add(admin);
            msg.From = posiljaoc;

            msg.Subject = "Poruka sa web sajta";
            msg.IsBodyHtml = true;
            msg.Body = poruka;

            SmtpClient klijent = new SmtpClient("smtp.gmail.com");
            klijent.Credentials = new NetworkCredential("itageneracija2018@gmail.com", "link2019a");
            klijent.EnableSsl = true;
            klijent.Port = 587;

            try
            {
                klijent.Send(msg);
                ViewBag.Poruka = "Email poslat! Odgovoricemo Vam u najkracem mogucem roku.";
                return View();
            }
            catch (System.Exception)
            {
                ViewBag.Poruka = "Greska pri slanju emaila!";
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
