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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SGVisitasTecnicasASPCore.Controllers
{
    public class AccessController : Controller
    {
        private readonly SgvtDB _context;
        private readonly IWebHostEnvironment _webHost;
        private List<Claim> claims = null;
        private ClaimsIdentity claimsIdentity = null;

        public AccessController(SgvtDB context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Usuario _usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(_usuario);
            }

            DA_Logica _da_usuario = new DA_Logica(_context);

            if (!String.IsNullOrEmpty(_usuario.Correo))
                _usuario.Correo = Utils.cifrarTextoAES(_usuario.Correo, Utils.palabraPasoArg, Utils.valorRGBSaltArg, Utils.MD5Arg, Utils.iteracionesArg, Utils.vectorInicialArg, Utils.tamanoClaveArg);

            if (!String.IsNullOrEmpty(_usuario.Clave))
                _usuario.Clave = Utils.cifrarTextoAES(_usuario.Clave, Utils.palabraPasoArg, Utils.valorRGBSaltArg, Utils.MD5Arg, Utils.iteracionesArg, Utils.vectorInicialArg, Utils.tamanoClaveArg);
                string desClave = Utils.descifrarTextoAES(_usuario.Clave, Utils.palabraPasoArg, Utils.valorRGBSaltArg, Utils.MD5Arg, Utils.iteracionesArg, Utils.vectorInicialArg, Utils.tamanoClaveArg);

            var usuario = _da_usuario.ValidarUsuario(_usuario.Correo, _usuario.Clave);

            if (usuario != null)
            {
                claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim("Correo", Utils.cifrarTextoAES(_usuario.Correo, Utils.palabraPasoArg, Utils.valorRGBSaltArg, Utils.MD5Arg, Utils.iteracionesArg, Utils.vectorInicialArg, Utils.tamanoClaveArg)),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                };

                claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.UserNotFound = "No se encuentra registrado el email, o la contraseña es incorrecta.";
                return View(_usuario);
            }

        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Access");
        }

        [HttpGet]
        public IActionResult StartRecovery()
        {
            RecoveryViewModel model = new RecoveryViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult StartRecovery(RecoveryViewModel model)
        {
            try
            {
                string htmlFilePath = Path.Combine(_webHost.WebRootPath, "EmailRecuperacionClave.html");
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (!String.IsNullOrEmpty(model.email))
                    model.email = Utils.cifrarTextoAES(model.email, Utils.palabraPasoArg, Utils.valorRGBSaltArg, Utils.MD5Arg, Utils.iteracionesArg, Utils.vectorInicialArg, Utils.tamanoClaveArg);

                string token = Utils.GetSHA256(Guid.NewGuid().ToString());

                var oUser = _context.usuarios.Where(d => d.Correo == model.email).FirstOrDefault();

                if (oUser != null)
                {
                    oUser.token_recovery = token;
                    _context.Entry(oUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    //enviar email
                    string user = oUser.Nombre;
                    string url = "http://" + HttpContext.Request.Host.Value + "/Access/Recovery?token=" + oUser.token_recovery;
                    string emailOrigen = "ventas.saimec@gmail.com";
                    string emailDestino = Utils.descifrarTextoAES(oUser.Correo, Utils.palabraPasoArg, Utils.valorRGBSaltArg, Utils.MD5Arg, Utils.iteracionesArg, Utils.vectorInicialArg, Utils.tamanoClaveArg);
                    string asunto = "SAIMEC - Recuperación de Contraseña";
                    string urlRecovery = "<a href='"+url+"'>Click para recuperar</a>";
                    bool statusEmailSend = Utils.SendEmailRecuperarClave(emailOrigen, emailDestino, asunto, htmlFilePath, user, urlRecovery);

                    if (statusEmailSend)
                    {
                        ViewBag.SucessEmail = "Email de recuperación enviado exitosamente a: " + emailDestino;
                    }
                    else
                    {
                        ViewBag.FailEmail = "Error al enviar el email de recuperación."; 
                    }
                }
                else
                {
                    ViewBag.UserNotFound = "Usuario no registrado en la plataforma SGVT.";
                }

                return View();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult Recovery(string token)
        {
            RecoveryPasswordViewModel model = new RecoveryPasswordViewModel();
            var oUser = _context.usuarios.Where(d => d.token_recovery == model.token).FirstOrDefault();
            if (oUser == null)
            {
                ViewBag.Error = "Token expirado.";
                return View("Index");
            }

            model.token = token;
            return View(model);
        }

        [HttpPost]
        public IActionResult Recovery(RecoveryPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var oUser = _context.usuarios.Where(d => d.token_recovery == model.token).FirstOrDefault();

                if (oUser != null && (model.Password.Length < 8 || model.Password.Length > 20))
                {
                    ViewBag.Message = "La nueva contraseña debe tener mínimo 8 caracteres y máximo 20 caracteres!";
                    return View(model);
                }

                if (!model.Password.Equals(model.Password2))
                {
                    ViewBag.Message = "Las contraseñas no son iguales. Favor ingréselas nuevamente!";
                    return View(model);
                }

                if (oUser != null)
                {
                    oUser.Clave = Utils.cifrarTextoAES(model.Password, Utils.palabraPasoArg, Utils.valorRGBSaltArg, Utils.MD5Arg, Utils.iteracionesArg, Utils.vectorInicialArg, Utils.tamanoClaveArg);
                    oUser.token_recovery = null;
                    _context.Entry(oUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    //Actualizar password del usuario en catalogos
                    empleados empl = new empleados();
                    empl = _context.empleados.Where(e => e.email == oUser.Correo).FirstOrDefault();
                    if (empl != null)
                    {
                        empl.password = oUser.Clave;
                        _context.Entry(empl).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    clientes cln = new clientes();
                    cln = _context.clientes.Where(c => c.email == oUser.Correo).FirstOrDefault();
                    if (cln != null)
                    {
                        cln.password = oUser.Clave;
                        _context.Entry(cln).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    _context.SaveChanges();
                    ViewBag.Message = "Contraseña modificada con éxito";
                }
                else
                {
                    ViewBag.Error = "Token expirado.";
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return View("Index");
        }

    }
}
