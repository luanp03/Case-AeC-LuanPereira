using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Case_AeC.Data;
using Case_AeC.Models;
using Case_AeC.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Case_AeC.Controllers
{
    [Authorize]
    public class EnderecoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ViaCepService _viaCepService;

        public EnderecoController(AppDbContext context, ViaCepService viaCepService)
        {
            _context = context;
            _viaCepService = viaCepService;
        }
        
        //Lista os endereços cadastrados pelo usuário logado
        public async Task<IActionResult> Index()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var enderecos = await _context.Enderecos.Where(e => e.UsuarioId == usuarioId).ToListAsync();
            return View(enderecos);
        }

        public IActionResult Criar() => View();


        //Salva no banco
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Endereco endereco)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            endereco.UsuarioId = int.Parse(usuarioId);

            ModelState.Remove("UsuarioId");

            if (ModelState.IsValid)
            {
                _context.Add(endereco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(endereco);
        }
        //Salva edição no banco
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
                return NotFound();

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId);

            if (endereco == null)
                return NotFound();

            return View(endereco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Endereco endereco)
        {
            if (id != endereco.Id)
                return NotFound();

            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            endereco.UsuarioId = int.Parse(usuarioId);
            ModelState.Remove("UsuarioId");
        
            if (ModelState.IsValid)
            {
                var existing = await _context.Enderecos.FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == endereco.UsuarioId);
                if (existing == null)
                    return NotFound();

                try
                {
                    existing.Cep = endereco.Cep;
                    existing.Logradouro = endereco.Logradouro;
                    existing.Numero = endereco.Numero;
                    existing.Complemento = endereco.Complemento;
                    existing.Bairro = endereco.Bairro;
                    existing.Cidade = endereco.Cidade;
                    existing.UF = endereco.UF;

                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Enderecos.Any(e => e.Id ==id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(endereco);
        }

        //Deleta do banco
        public async Task<IActionResult> Deletar(int? id)
        {
            if (id == null)
                return NotFound();

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId);


            if (endereco == null)
                return NotFound();

            return View(endereco);
        }

        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(int id)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId);

            if (endereco !=null)
            {
                _context.Enderecos.Remove(endereco);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        //Exporta os endereços do usuário logado para um arquivo CSV
        public async Task<IActionResult> ExportarCSV()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var enderecos = await _context.Enderecos.Where(e => e.UsuarioId == usuarioId).ToListAsync();

            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Id;CEP;Logradouro;Numero;Complemento;Bairro;Cidade;UF");

            foreach (var item in enderecos)
            {
                csv.AppendLine($"{item.Id};{item.Cep};{item.Logradouro};{item.Numero};{item.Complemento};{item.Bairro};{item.Cidade};{item.UF}");
            }

            var bytes = System.Text.Encoding.UTF8.GetPreamble().Concat(System.Text.Encoding.UTF8.GetBytes(csv.ToString())).ToArray();

            return File(bytes, "text/csv", "enderecos.csv");
        }

        //Busca o endereço pelo CEP usando a API ViaCEP
        [HttpGet]
        public async Task<IActionResult> BuscarCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                return BadRequest();

            var resultado = await _viaCepService.BuscarCepAsync(cep);
            if (resultado == null)
                return NotFound();
            return Json(resultado);
        }
    }
}