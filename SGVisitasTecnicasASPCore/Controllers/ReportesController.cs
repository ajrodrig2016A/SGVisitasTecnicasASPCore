﻿using FastReport;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly SgvtDB _context; // for connecting to efcore.
        private readonly IClientes _clientesRepo;
        private List<visitasReporte> visitasReporte = new List<visitasReporte>();
        private List<visitas> lstVisitas = null;
        string path = "";
        string errMessage = "";

        public ReportesController(IWebHostEnvironment webHost, SgvtDB context, IClientes clientesRepo)
        {
            _webHost = webHost;
            _context = context;
            _clientesRepo = clientesRepo;
        }  
        public IActionResult VisitasReporte(visitasReporte model)
        {
            ViewBag.Clientes = GetClientes();
            return View(model);
        }

        public IActionResult GenerateVisitasReporte(visitasReporte model)
        {
            MemoryStream ms = new MemoryStream();
            string nameReport = "";          

            try
            {
                FastReport.Utils.Config.WebMode = true;
                Report rep = new Report();
                string webRootPath = _webHost.WebRootPath;
                path = Path.Combine(webRootPath, "VisitasReporte.frx");
                rep.Load(path);

                if (!String.IsNullOrEmpty(model.fecha_agendada) && !String.IsNullOrEmpty(model.fecha_cierre))
                {
                    lstVisitas = _context.visitas.Include(c => c.Cliente).Where(d => d.fecha_agendada >= DateTime.Parse(model.fecha_agendada) && d.fecha_agendada <= DateTime.Parse(model.fecha_cierre)).OrderBy(v => v.id_visita).ToList();

                    if (lstVisitas.Count == 0)
                    {
                        model = LimpiarCampos();
                        return EmptyGetDataFromDB(model);
                    }

                    foreach (var item in lstVisitas)
                    {
                        visitasReporte.Add(new visitasReporte() { fecha_agendada = item.fecha_agendada.ToString("yyyy-MM-dd"), fecha_cierre = item.fecha_cierre.ToString("yyyy-MM-dd"), cliente = item.Cliente.nombres + " " + item.Cliente.apellidos, descripcion = item.descripcion, ubicacionDispSeguridad = item.ubicacionDispSeguridad, tiempoEntrega = item.tiempoEntrega, estado = item.estado });
                    }
                    rep.SetParameterValue("prmCliente", @User.Identity.Name);
                    rep.SetParameterValue("prmFechaAgendamiento", model.fecha_agendada);
                    rep.SetParameterValue("prmFechaCierre", model.fecha_cierre);
                    rep.RegisterData(visitasReporte, "VisitasRef");
                    nameReport = "Reporte de Visitas por rango de Fechas.pdf";
                }

                if (!String.IsNullOrEmpty(model.fecha_agendada))
                {
                    lstVisitas = _context.visitas.Include(c => c.Cliente).Where(v => v.fecha_agendada == DateTime.Parse(model.fecha_agendada)).OrderBy(v => v.fecha_agendada).ToList();

                    if (lstVisitas.Count == 0)
                    {
                        model = LimpiarCampos();
                        return EmptyGetDataFromDB(model);
                    }

                    foreach (var item in lstVisitas)
                    {
                        visitasReporte.Add(new visitasReporte() { fecha_agendada = item.fecha_agendada.ToString("yyyy-MM-dd"), fecha_cierre = item.fecha_cierre.ToString("yyyy-MM-dd"), cliente = item.Cliente.nombres + " " + item.Cliente.apellidos, descripcion = item.descripcion, ubicacionDispSeguridad = item.ubicacionDispSeguridad, tiempoEntrega = item.tiempoEntrega, estado = item.estado });
                    }
                    rep.SetParameterValue("prmCliente", @User.Identity.Name);
                    rep.SetParameterValue("prmFechaAgendamiento", model.fecha_agendada);
                    rep.RegisterData(visitasReporte, "VisitasRef");
                    nameReport = "Reporte de Visitas por Fecha de Agendamiento.pdf";
                }

                if (!String.IsNullOrEmpty(model.fecha_cierre))
                {
                    lstVisitas = _context.visitas.Include(c => c.Cliente).Where(v => v.fecha_cierre == DateTime.Parse(model.fecha_cierre)).OrderBy(v => v.fecha_cierre).ToList();

                    if (lstVisitas.Count == 0)
                    {
                        model = LimpiarCampos();
                        return EmptyGetDataFromDB(model);
                    }

                    foreach (var item in lstVisitas)
                    {
                        visitasReporte.Add(new visitasReporte() { fecha_agendada = item.fecha_agendada.ToString("yyyy-MM-dd"), fecha_cierre = item.fecha_cierre.ToString("yyyy-MM-dd"), cliente = item.Cliente.nombres + " " + item.Cliente.apellidos, descripcion = item.descripcion, ubicacionDispSeguridad = item.ubicacionDispSeguridad, tiempoEntrega = item.tiempoEntrega, estado = item.estado });
                    }
                    rep.SetParameterValue("prmCliente", @User.Identity.Name);
                    rep.SetParameterValue("prmFechaCierre", model.fecha_cierre);
                    rep.RegisterData(visitasReporte, "VisitasRef");
                    nameReport = "Reporte de Visitas por Fecha de Cierre.pdf";
                }

                if (!String.IsNullOrEmpty(model.cliente))
                {
                    lstVisitas = _context.visitas.Include(c => c.Cliente).Where(vt => vt.id_cliente == Int32.Parse(model.cliente)).OrderBy(v => v.Cliente).ToList();

                    if (lstVisitas.Count == 0)
                    {
                        model = LimpiarCampos();
                        return EmptyGetDataFromDB(model);
                    }

                    foreach (var item in lstVisitas)
                    {
                        visitasReporte.Add(new visitasReporte() { fecha_agendada = item.fecha_agendada.ToString("yyyy-MM-dd"), fecha_cierre = item.fecha_cierre.ToString("yyyy-MM-dd"), cliente = item.Cliente.nombres + " " + item.Cliente.apellidos, descripcion = item.descripcion, ubicacionDispSeguridad = item.ubicacionDispSeguridad, tiempoEntrega = item.tiempoEntrega, estado = item.estado });
                    }
                    rep.SetParameterValue("prmCliente", @User.Identity.Name);
                    rep.RegisterData(visitasReporte, "VisitasRef");
                    nameReport = "Reporte de Visitas por Cliente.pdf";
                }


                if (String.IsNullOrEmpty(model.cliente) && String.IsNullOrEmpty(model.fecha_agendada) && String.IsNullOrEmpty(model.fecha_cierre))
                {
                    errMessage = "Favor seleccione un criterio de búsqueda para generar el reporte.";
                    TempData["ErrorMessage"] = errMessage;
                    ViewBag.Clientes = GetClientes();
                    return View("VisitasReporte",model);
                }

                if (rep.Report.Prepare() && lstVisitas.Count > 0)
                {
                    FastReport.Export.PdfSimple.PDFSimpleExport pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                    pdfExport.ShowProgress = false;
                    pdfExport.Subject = "Reporte de Visitas Tecnicas";
                    pdfExport.Title = "Reporte de Visitas Tecnicas";
                    
                    rep.Report.Export(pdfExport, ms);
                    rep.Dispose();
                    pdfExport.Dispose();
                    ms.Position = 0;
                    model = LimpiarCampos();
                }
                else
                {
                    errMessage = "No se pudo generar el reporte.";
                    TempData["ErrorMessage"] = errMessage;
                }
                
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
                TempData["ErrorMessage"] = errMessage;
                //ModelState.AddModelError("", errMessage);
            }

            return File(ms, "application/pdf", nameReport);            
        }

        public IActionResult VentasReporte()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CotizacìonReporte()
        {
            ViewBag.Clientes = GetClientes();
            return View();
        }
        [HttpPost]
        public IActionResult GenerateCotizacionReporte(cotizaciones model)
        {
            MemoryStream ms = new MemoryStream();
            string nameReport = "";

            try
            {
                FastReport.Utils.Config.WebMode = true;
                Report rep = new Report();
                string webRootPath = _webHost.WebRootPath;
                path = Path.Combine(webRootPath, "Cotizacion.frx");
                rep.Load(path);
                rep.SetParameterValue("prmIdCotizacion", 50);
                // rep.RegisterData(visitasReporte, "VisitasRef");
                nameReport = "CotizacionFinal.pdf";
                // if (!String.IsNullOrEmpty(model.fecha_agendada) && !String.IsNullOrEmpty(model.fecha_cierre))
                // {
                // lstVisitas = _context.visitas.Include(c => c.Cliente).Where(d => d.fecha_agendada >= DateTime.Parse(model.fecha_agendada) && d.fecha_agendada <= DateTime.Parse(model.fecha_cierre)).OrderBy(v => v.id_visita).ToList();

                // if (lstVisitas.Count == 0)
                // {
                // model = LimpiarCampos();
                // return EmptyGetDataFromDB(model);
                // }

                // foreach (var item in lstVisitas)
                // {
                // visitasReporte.Add(new visitasReporte() { fecha_agendada = item.fecha_agendada.ToString("yyyy-MM-dd"), fecha_cierre = item.fecha_cierre.ToString("yyyy-MM-dd"), cliente = item.Cliente.nombres + " " + item.Cliente.apellidos, descripcion = item.descripcion, ubicacionDispSeguridad = item.ubicacionDispSeguridad, tiempoEntrega = item.tiempoEntrega, estado = item.estado });
                // }
                // rep.SetParameterValue("prmCliente", @User.Identity.Name);
                // rep.SetParameterValue("prmFechaAgendamiento", model.fecha_agendada);
                // rep.SetParameterValue("prmFechaCierre", model.fecha_cierre);
                // rep.RegisterData(visitasReporte, "VisitasRef");
                // nameReport = "Reporte de Visitas por rango de Fechas.pdf";
                // }


                // if (String.IsNullOrEmpty(model.cliente) && String.IsNullOrEmpty(model.fecha_agendada) && String.IsNullOrEmpty(model.fecha_cierre))
                // {
                // errMessage = "Favor seleccione un criterio de búsqueda para generar el reporte.";
                // TempData["ErrorMessage"] = errMessage;
                // ViewBag.Clientes = GetClientes();
                // return View("VisitasReporte",model);
                // }

                if (rep.Report.Prepare())
                {
                    FastReport.Export.PdfSimple.PDFSimpleExport pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                    pdfExport.ShowProgress = false;
                    pdfExport.Subject = "Cotizacion por Servicios";
                    pdfExport.Title = "Cotizacion por Servicios";

                    rep.Report.Export(pdfExport, ms);
                    rep.Dispose();
                    pdfExport.Dispose();
                    ms.Position = 0;
                    //model = LimpiarCampos();
                }
                else
                {
                    errMessage = "No se pudo generar el reporte.";
                    TempData["ErrorMessage"] = errMessage;
                }

            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
                TempData["ErrorMessage"] = errMessage;
                //ModelState.AddModelError("", errMessage);
            }

            return File(ms, "application/pdf", nameReport);
        }

        private ViewResult EmptyGetDataFromDB(visitasReporte model)
        {
            errMessage = "No hay datos para generar el reporte.";
            TempData["ErrorMessage"] = errMessage;
            ViewBag.Clientes = GetClientes();
            ModelState.Clear();
            return View("VisitasReporte", model);
        }

        public IActionResult RedirectToIndex()
        {
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        private visitasReporte LimpiarCampos()
        {
            ViewBag.Clientes = GetClientes();
            ModelState.Clear();
            visitasReporte objVisitasReporte = new visitasReporte() { fecha_agendada = String.Empty, fecha_cierre = String.Empty, cliente = String.Empty };
            return objVisitasReporte;
        }

        private List<SelectListItem> GetClientes()
        {
            var lstItems = new List<SelectListItem>();

            PaginatedList<clientes> items = _clientesRepo.GetItems("nombres", Models.SortOrder.Ascending, "", 1, 1000);
            lstItems = items.Select(ut => new SelectListItem()
            {
                Value = ut.id_cliente.ToString(),
                Text = ut.nombres
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Seleccione el Cliente----"
            };

            lstItems.Insert(0, defItem);

            return lstItems;
        }
    }
}
