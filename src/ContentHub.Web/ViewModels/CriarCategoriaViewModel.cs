using System.ComponentModel.DataAnnotations;

namespace ContentHub.Web.ViewModels;

public sealed class CriarCategoriaViewModel
{
    [Required(ErrorMessage = "Informe o nome da categoria.")]
    [StringLength(100, ErrorMessage = "Use até 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;
}
