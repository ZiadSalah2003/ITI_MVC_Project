using Microsoft.AspNetCore.Mvc;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Services;

namespace ITI_MVC_Project.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _accountService.RegisterAsync(model);
			if (result.Succeeded)
			{
				var userId = await _accountService.GetUserIdByEmailAsync(model.Email);
				if (userId is not null)
				{
					var code = await _accountService.GenerateEmailConfirmationTokenAsync(userId);
					if (code is not null)
					{

						var callbackUrl = Url.Action("ConfirmEmail", "Account",
							new { userId, code }, Request.Scheme)!;
						await _accountService.SendEmailConfirmationAsync(model.Email, model.FirstName, callbackUrl);
					}
				}
				return RedirectToAction(nameof(RegisterConfirmation));
			}

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			return View(model);
		}

		[HttpGet]
		public IActionResult Login(string? returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			if (!ModelState.IsValid) return View(model);

			var result = await _accountService.LoginAsync(model);

			if (result.Succeeded)
				return LocalRedirect(returnUrl ?? Url.Action("Index", "Catalog")!);

			if (result.IsNotAllowed)
				ModelState.AddModelError(string.Empty, "Please confirm your email address before logging in.");
			else
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _accountService.LogoutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult ForgotPassword() => View();

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
		{
			if (!ModelState.IsValid) return View(model);

			var code = await _accountService.GeneratePasswordResetTokenAsync(model.Email);
			if (code is not null)
			{
				var callbackUrl = Url.Action("ResetPassword", "Account",
					new { email = model.Email, code }, Request.Scheme)!;
				await _accountService.SendPasswordResetEmailAsync(model.Email, callbackUrl);
			}
			TempData["SuccessMessage"] = "If that email is registered, a password reset link has been sent.";
			return View(model);
		}


		[HttpGet]
		public IActionResult ResetPassword(string? email, string? code)
		{
			if (email is null || code is null)
				return RedirectToAction(nameof(ForgotPassword));

			return View(new ResetPasswordVM { Email = email, Code = code });
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _accountService.ResetPasswordAsync(model);
			if (result.Succeeded)
			{
				TempData["SuccessMessage"] = "Your password has been reset. You can now log in with your new password.";
				return RedirectToAction(nameof(Login));
			}

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			return View(model);
		}

		[HttpGet]
		public IActionResult RegisterConfirmation() => View();

		[HttpGet]
		public async Task<IActionResult> ConfirmEmail(string? userId, string? code)
		{
			if (userId is null || code is null)
				return RedirectToAction(nameof(Login));

			var result = await _accountService.ConfirmEmailAsync(userId, code);
			TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] = result.Succeeded
				? "Your email has been confirmed. You can now log in."
				: "Email confirmation failed. The link may be expired or invalid.";

			return RedirectToAction(nameof(Login));
		}

		public IActionResult AccessDenied() => View();
	}
}