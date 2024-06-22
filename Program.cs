using BancaEnLinea.Models;
using BancaEnLinea.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;

namespace BancaEnLinea
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var politicaUsuariosAutenticados = new AuthorizationPolicyBuilder()
                   .RequireAuthenticatedUser()
                   .Build();

            // Add services to the container.
            builder.Services.AddControllersWithViews(opciones =>
            {
                opciones.Filters.Add(new AuthorizeFilter(politicaUsuariosAutenticados));
            });
            builder.Services.AddTransient<IRepositorioCliente, RepositorioCliente>();
            builder.Services.AddTransient<IRepositorioTransaccion, RepositorioTransaccion>();
            builder.Services.AddTransient<IservicioUsuarios, ServicioUsuarios>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
            builder.Services.AddTransient<IUserStore<Usuario>, UsuarioStore>();
            builder.Services.AddTransient<SignInManager<Usuario>>();
            builder.Services.AddIdentityCore<Usuario>(opciones =>
            {
                opciones.Password.RequireDigit = false;
                opciones.Password.RequireLowercase = false;
                opciones.Password.RequireUppercase = false;
                opciones.Password.RequireNonAlphanumeric = false;
            }).AddErrorDescriber<MensajesDeErrorIdentity>();

            //Utenticanto para que la aplicacion entienda las cookie
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, opciones =>
            {
                opciones.LoginPath = "/usuarios/login";
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}