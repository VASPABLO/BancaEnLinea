using BancaEnLinea.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BancaEnLinea.Controllers
{
    public class UsuariosController:Controller
    {
        private readonly UserManager<Usuario> userManager;

        public UsuariosController(UserManager<Usuario> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            var usuario = new Usuario() { Email = modelo.Email };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach(var error in resultado.Errors)
                {
                    ModelState.TryAddModelError(string.Empty, error.Description);
                }
            }

            return View(modelo);

            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
                return RedirectToAction("Index", "Home");
            
        }
    }
}
