using System.ComponentModel.DataAnnotations;

namespace Case_AeC.Models
{
    public class Endereco
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Cep { get; set; }
        [Required]
        public string Logradouro { get; set; }
        [Required]
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        [Required]
        public string Bairro { get; set; }
        [Required]
        public string Cidade { get; set; }
        [Required]
        public string UF { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}