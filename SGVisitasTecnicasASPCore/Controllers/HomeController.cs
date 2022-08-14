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

namespace SGVisitasTecnicasASPCore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
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
                if (ModelState.IsValid)
                {
                    //enviar email
                    string emailOrigen = model.email;
                    string emailDestino = "from@example.com";
                    string asunto = "SGVT - Solicitud de Cotización: " + model.servicio + ".";
                    string cuerpo = "<p>" + model.mensaje + "</p><br/>" + "<p>Remitente: " + model.nombres + " " + model.apellidos + "</p><br/>" + "<p>Número de contacto: " + model.telefono + "</p><br/>";
                    Utils objSendMail = new Utils();
                    bool statusEmailSend = objSendMail.SendEmail(emailOrigen, emailDestino, asunto, cuerpo);

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
