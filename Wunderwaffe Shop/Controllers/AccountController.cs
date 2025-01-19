using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Wunderwaffe_Shop.Models;

namespace Wunderwaffe_Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            var user = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email, 
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                CreatedAt = DateTime.Now,
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Klient");
                await signInManager.SignInAsync(user, false);
                
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(registerDto);

        }

        public async Task<IActionResult> Logout()
        {
            if (signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult login()
        {
            return View();
        }


        [HttpPost]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{
			if (signInManager.IsSignedIn(User))
			{
				return RedirectToAction("Index", "Home");
			}

			if (!ModelState.IsValid)
			{
				return View(loginDto);
			}

			var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password,
				loginDto.RememberMe, false);

			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ViewBag.ErrorMessage = "Nie udana próba logowania";
			}

			return View(loginDto);
		}

        [Authorize]
        public async Task<IActionResult> ProfileAsync()
        {
			var appUser = await userManager.GetUserAsync(User);
            if (appUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var profileDto = new ProfileDto()
            {
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email ?? "",
                PhoneNumber = appUser.PhoneNumber,
                Address = appUser.Address,
            };


			return View(profileDto);
        }



        [Authorize]
        [HttpPost]
		public async Task<IActionResult> Profile(ProfileDto profileDto)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.ErrorMessage = "Proszę, wypełnij wymagane pola";
				return View(profileDto);
			}

			var appUser = await userManager.GetUserAsync(User);
			if (appUser == null)
			{
				return RedirectToAction("Index", "Home");
			}

			appUser.FirstName = profileDto.FirstName;
			appUser.LastName = profileDto.LastName;
			appUser.UserName = profileDto.Email;
			appUser.Email = profileDto.Email;
			appUser.PhoneNumber = profileDto.PhoneNumber;
			appUser.Address = profileDto.Address;

			var result = await userManager.UpdateAsync(appUser);

			if (result.Succeeded)
			{
				ViewBag.SuccessMessage = "Aktualizacja konta udana";
			}
			else
			{
				ViewBag.ErrorMessage = "Nie udało się zaaktualizować konta: " + result.Errors.First().Description;
			}


			ViewBag.SuccessMessage = "Twoje konto zostało zaaktualizowane";

			return View(profileDto);
		}

		[Authorize]
		public IActionResult Password()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Password(PasswordDto passwordDto)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var appUser = await userManager.GetUserAsync(User);
			if (appUser == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var result = await userManager.ChangePasswordAsync(appUser,
				passwordDto.CurrentPassword, passwordDto.NewPassword);

			if (result.Succeeded)
			{
				ViewBag.SuccessMessage = "Hasło zaktualizowane pomyślnie!";
			}
			else
			{
				ViewBag.ErrorMessage = "Error: " + result.Errors.First().Description;
			}

			return View();
		}

		public IActionResult AccessDenied()
		{
			return RedirectToAction("Index", "Home");
		}

		public IActionResult ForgotPassword()
		{
			if (signInManager.IsSignedIn(User))
			{
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([Required, EmailAddress] string email)
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Email = email;

            if (!ModelState.IsValid)
            {
                ViewBag.EmailError = ModelState["email"]?.Errors.First().ErrorMessage ?? "Podano zły adres Email";
                return View();
            }

            var user = await userManager.FindByEmailAsync(email);



            ViewBag.SuccessMessage = "Sprawdź swoją pocztę z znajdź wiadomość z linkiem do zmiany hasła";

            return View();
        }

    }
}
