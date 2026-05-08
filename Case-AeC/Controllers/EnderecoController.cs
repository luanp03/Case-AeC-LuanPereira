using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Case_AeC.Data;
using Case_AeC.Models;
using Case_AeC.Services;

namespace Case_AeC.Controllers
{
    public class EnderecoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ViaCepService _viaCepService;

        public EnderecoController(AppDbContext context, ViaCepService viaCepService)
        {
            _context = context;
            _viaCepService = viaCepService;
        }
        
        //Lista os endereços cadastrados
        public async Task<IActionResult> Index()
        {
            var enderecos = await _context.Enderecos.ToListAsync();
            return View(enderecos);
        }

        public IActionResult Criar() => View();


        //Salva no banco
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Endereco endereco)
        {
            if(ModelState.IsValid)
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

            var endereco = await _context.Enderecos.FindAsync(id);
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
        
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(endereco);
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
            
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(m => m.Id == id);
            if (endereco == null)
                return NotFound();

            return View(endereco);
        }

        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(int id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco !=null)
            {
                _context.Enderecos.Remove(endereco);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportarCSV()
        {
            var enderecos = await _context.Enderecos.ToListAsync();
            var csv = new System.Text.StringBuilder();

            csv.AppendLine("Id;CEP;Logradouro;Bairro;Cidade;UF");

            foreach (var item in enderecos)
            {
                csv.AppendLine($"{item.Id};{item.Cep};{item.Logradouro};{item.Bairro};{item.Cidade};{item.UF}");
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