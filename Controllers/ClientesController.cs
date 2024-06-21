using BancaEnLinea.Models;
using BancaEnLinea.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BancaEnLinea.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IRepositorioCliente _repositorioCliente;

        public ClientesController(IRepositorioCliente repositorioCliente)
        {
            _repositorioCliente = repositorioCliente;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _repositorioCliente.ObtenerTodosAsync();
            return View(clientes);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _repositorioCliente.CrearAsync(cliente);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _repositorioCliente.ObtenerPorIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _repositorioCliente.ActualizarAsync(cliente);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public async Task<IActionResult> Borrar(int id)
        {
            var cliente = await _repositorioCliente.ObtenerPorIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost, ActionName("Borrar")]
        public async Task<IActionResult> BorrarConfirmado(int id)
        {
            await _repositorioCliente.EliminarAsync(id);
            return RedirectToAction("Index");
        }
    }
}


