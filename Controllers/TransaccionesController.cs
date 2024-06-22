using BancaEnLinea.Models;
using BancaEnLinea.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancaEnLinea.Controllers
{
    
    public class TransaccionesController : Controller
    {
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        private readonly IRepositorioCliente _repositorioCliente; // Asume que tienes un repositorio de cliente para verificar el saldo y obtener información del cliente

        public TransaccionesController(IRepositorioTransaccion repositorioTransaccion, IRepositorioCliente repositorioCliente)
        {
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioCliente = repositorioCliente;
        }
        
        public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta, string nombreCliente)
        {
            var transacciones = await _repositorioTransaccion.ObtenerConFiltrosAsync(desde, hasta, nombreCliente);
            return View(transacciones);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Transaccion transaccion)
        {
            if (ModelState.IsValid)
            {
                var cliente = await _repositorioCliente.ObtenerPorIdAsync(transaccion.ClienteId);
                if (cliente == null)
                {
                    ModelState.AddModelError("", "Cliente no encontrado");
                    return View(transaccion);
                }

                if (transaccion.Tipo == "Retiro" && cliente.Saldo < transaccion.Monto)
                {
                    TempData["ErrorMessage"] = "Saldo insuficiente.";
                    return View(transaccion);
                }

                // Ajustar el saldo del cliente
                if (transaccion.Tipo == "Deposito")
                {
                    cliente.Saldo += transaccion.Monto;
                }
                else if (transaccion.Tipo == "Retiro")
                {
                    cliente.Saldo -= transaccion.Monto;
                }

                await _repositorioCliente.ActualizarAsync(cliente);
                await _repositorioTransaccion.CrearAsync(transaccion);
                return RedirectToAction("Index");
            }
            return View(transaccion);
        }


        public async Task<IActionResult> Editar(int id)
        {
            var transaccion = await _repositorioTransaccion.ObtenerPorIdAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }
            return View(transaccion);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Transaccion transaccion)
        {
            if (ModelState.IsValid)
            {
                await _repositorioTransaccion.ActualizarAsync(transaccion);
                return RedirectToAction("Index");
            }
            return View(transaccion);
        }

        public async Task<IActionResult> Borrar(int id)
        {
            var transaccion = await _repositorioTransaccion.ObtenerPorIdAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }
            return View(transaccion);
        }

        [HttpPost, ActionName("Borrar")]
        public async Task<IActionResult> BorrarConfirmado(int id)
        {
            await _repositorioTransaccion.EliminarAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Anular(int id)
        {
            var transaccion = await _repositorioTransaccion.ObtenerPorIdAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }
            return View(transaccion);
        }

        [HttpPost, ActionName("Anular")]
        public async Task<IActionResult> AnularConfirmado(int id)
        {
            var transaccion = await _repositorioTransaccion.ObtenerPorIdAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }

            transaccion.Anulada = true;
            await _repositorioTransaccion.ActualizarAsync(transaccion);
            return RedirectToAction("Index");
        }

        // Método para exportar transacciones a Excel y enviar por correo puede ser añadido aquí.
    }
}



