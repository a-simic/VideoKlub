using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VideoKlub2019.Data;

namespace VideoKlub2019.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Unesite email!")]
            [EmailAddress]
            [Display(Name = "Email:")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Unesite korisnicko ime!")]
            [Display(Name = "Korisnicko ime:")]
            public string KorisnickoIme { get; set; }

            [Required(ErrorMessage = "Unesite ime!")]
            [Display(Name = "Ime:")]
            public string Ime { get; set; }

            [Required(ErrorMessage = "Unesite prezime!")]
            [Display(Name = "Prezime:")]
            public string Prezime { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Minimalno 8 a maksimalno 100 karaktera!", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Lozinka:")]
            public string Lozinka { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potvrdi lozinku:")]
            [Compare("Lozinka", ErrorMessage = "Lozinke se ne podudaraju!")]
            public string Potvrda { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.KorisnickoIme, Email = Input.Email, Ime = Input.Ime, Prezime = Input.Prezime };
                var result = await _userManager.CreateAsync(user, Input.Lozinka);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
