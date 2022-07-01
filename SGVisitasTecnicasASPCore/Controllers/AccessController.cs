using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGVisitasTecnicasASPCore.Models;
using SGVisitasTecnicasASPCore.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace SGVisitasTecnicasASPCore.Controllers
{
    public class AccessController : Controller
    {
        private readonly SgvtDB _context;

        public AccessController(SgvtDB context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Usuario _usuario)
        {
            DA_Logica _da_usuario = new DA_Logica(_context);

            var usuario = _da_usuario.ValidarUsuario(_usuario.Correo, _usuario.Clave);

            if (usuario != null)
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim("Correo", usuario.Correo)
                };

                foreach (string rol in usuario.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Access");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(); ;
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    var user = await userManager.FindByEmailAsync(model.email);

            //    if (user != null && await userManager.IsEmailConfirmedAsync(user))
            //    {
            //        var token = await userManager.GeneratePasswordResetTokenAsync(user);

            //        var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.email, token = token }, Request.Scheme);

            //        logger.Log(LogLevel.Warning, passwordResetLink);

            //        return View("ForgotPasswordConfirmation");
            //    }
            //    return View("ForgotPasswordConfirmation");
            //}
            return View(model);
        }
    }
}
