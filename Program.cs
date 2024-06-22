using BancaEnLinea.Models;
using BancaEnLinea.Servicios;
using Microsoft.AspNetCore.Identity;

namespace BancaEnLinea
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

         

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IRepositorioCliente, RepositorioCliente>();
            builder.Services.AddTransient<IRepositorioTransaccion, RepositorioTransaccion>();
            builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
            builder.Services.AddTransient<IUserStore<Usuario>, UsuarioStore>();
            builder.Services.AddIdentityCore<Usuario>(opciones =>
            {
                opciones.Password.RequireDigit = false;
                opciones.Password.RequireLowercase = false;
                opciones.Password.RequireUppercase = false;
                opciones.Password.RequireNonAlphanumeric = false;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}