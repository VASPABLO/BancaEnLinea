using BancaEnLinea.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BancaEnLinea.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;

        //Asignando el Cookie con el SigInManager
        public UsuariosController(UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            var usuario = new Usuario() { Email = modelo.Email };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            if (resultado.Succeeded)
            {
                //Asignando la Cookie al usuario
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Transacciones");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.TryAddModelError(string.Empty, error.Description);
                }

                return View(modelo);
            }
        }
       
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
           
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await signInManager.PasswordSignInAsync(modelo.Email,
                modelo.Password, modelo.Recuerdame, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Transacciones");

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o clave incorrecto. ");
                return View(modelo);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Transacciones");
        }
    }
}
