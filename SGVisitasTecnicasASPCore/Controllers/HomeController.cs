using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SGVisitasTecnicasASPCore.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SGVisitasTecnicasASPCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(IWebHostEnvironment webHost, ILogger<HomeController> logger)
        {
            _webHost = webHost;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "ADM, COM, TEC")]
        public IActionResult Empleados()
        {
            return View();
        }
        [Authorize(Roles = "ADM, COM")]
        public IActionResult Clientes()
        {
            return View();
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

        [HttpGet]
        public IActionResult StartRequestQuote()
        {
            EmailRequestQuoteModel model = new EmailRequestQuoteModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult StartRequestQuote(EmailRequestQuoteModel model)
        {
            try
            {
                string htmlFilePath = Path.Combine(_webHost.WebRootPath, "EmailSolicitarInformacion.html");

                if (ModelState.IsValid)
                {
                    //enviar email
                    string user = model.nombres + " " + model.apellidos;
                    string emailOrigen = "rodandrews90210@gmail.com";
                    string emailDestino = model.email;
                    string asunto = "SAIMEC - Solicitud de Información: " + model.servicio + ".";
                    string datosCliente = "<p>" + model.mensaje + "</p><br/>" + "<p>Número de contacto: " + model.telefono + "</p><br/>";
                    Utils objSendMail = new Utils();
                    bool statusEmailSend = objSendMail.SendEmailSolicitarInformacion(emailOrigen, emailDestino, asunto, htmlFilePath, user,  datosCliente);

                    if (statusEmailSend)
                    {
                        return Json("success");
                    }
                }
                return Json("fail");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
