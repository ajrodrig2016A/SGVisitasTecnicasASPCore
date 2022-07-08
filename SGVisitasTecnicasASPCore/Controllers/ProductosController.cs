using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SGVisitasTecnicasASPCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SGVisitasTecnicasASPCore.Controllers
{
    public class ProductosController : Controller
    {
        private readonly SgvtDB _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductosController(SgvtDB context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        //Upload file
        private string UploadedFile(productos producto)
        {
            string fileName = null;

            if (producto.ImageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                fileName = Path.GetFileNameWithoutExtension(producto.ImageFile.FileName);
                string extension = Path.GetExtension(producto.ImageFile.FileName);
                producto.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    producto.ImageFile.CopyTo(fileStream);
                }
            }
            ViewData["message"] =$"{producto.ImageFile.Length} bytes uploaded successfully!";
            return fileName;
        }

        //bool Utils.IsAnyNullOrEmpty(object myObject)
        //{
        //    foreach (PropertyInfo pi in myObject.GetType().GetProperties())
        //    {
        //        if (pi.PropertyType == typeof(string))
        //        {
        //            string value = (string)pi.GetValue(myObject);
        //            if (string.IsNullOrEmpty(value))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        // GET: ProductosController
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.productos.ToListAsync());
        }

        //// GET: ProductosController/Details/5
        //[Authorize]
        //public ActionResult Details(int id)
        //{
        //    productos producto = new productos();
        //    using (SgvtDB SgvtEntities = new SgvtDB())
        //    {
        //        producto = SgvtEntities.productos.Where(x => x.id_producto == id).FirstOrDefault();
        //    }
        //    return View(producto);
        //}

        // GET: ProductosController/Create
        [Authorize]
        public IActionResult Create()
        {
            var categoryList = _context.categorias.Select(r => new { r.id_categoria, r.nombre }).ToList();
            ViewData["CategoryID"] = new SelectList(categoryList, "id_categoria", "nombre");

            return View(new productos());
        }

        // POST: ProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 4194304)]
        public async Task<IActionResult> Create(productos producto)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                producto.ImageName = UploadedFile(producto);

                //Insert record
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var categoryList = _context.categorias.Select(r => new { r.id_categoria, r.nombre }).ToList();
            ViewData["CategoryID"] = new SelectList(categoryList, "id_categoria", "nombre");
            return View();
        }

        // GET: ProductosController/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.productos.FindAsync(id);
            var categoryList = _context.categorias.Select(r => new { r.id_categoria, r.nombre }).ToList();
            ViewData["CategoryID"] = new SelectList(categoryList, "id_categoria", "nombre");

            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);

        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, productos producto)
        {
            if (id != producto.id_producto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (producto.ImageFile != null)
                    {
                        if (producto.ImageName != null)
                        {
                            string filePath = Path.Combine(_hostEnvironment.WebRootPath, "image", producto.ImageName);
                            System.IO.File.Delete(filePath);
                        }
                        producto.ImageName = UploadedFile(producto);
                    }
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(producto.id_producto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        private bool ImageExists(int id)
        {
            return _context.productos.Any(e => e.id_producto == id);
        }

        // GET: ProductosController/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.productos
                .FirstOrDefaultAsync(m => m.id_producto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: ProductosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.productos.FindAsync(id);

            //delete from wwwroot/image
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", producto.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            //delete the record
            _context.productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
