using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Case_AeC.Data;
using Case_AeC.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Case_AeC.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string senha)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == login && u.Senha == senha);

            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim (ClaimTypes.Name, usuario.Nome),
                    new Claim (ClaimTypes.NameIdentifier, usuario.Id.ToString())
                };

                var identify = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identify);

                await HttpContext.SignInAsync("CookieAuth", principal);

                return RedirectToAction("Index", "Endereco");
            }

            ViewBag.Error = "Login ou senha invalidos.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
                return RedirectToAction(nameof(Login));
            }
            return View(usuario);
        }
    }
}