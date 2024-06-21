using BancaEnLinea.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BancaEnLinea.Controllers
{
    public class UsuariosController:Controller
    {
        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
                return RedirectToAction("Index", "Home");
            
        }
    }
}
