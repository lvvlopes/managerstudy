using System.ComponentModel.DataAnnotations;
using ContentHub.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContentHub.Web.ViewModels;

public sealed class CriarFonteViewModel
{
    [Required(ErrorMessage = "Informe o nome da fonte.")]
    [StringLength(150, ErrorMessage = "Use até 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a URL do perfil.")]
    [Url(ErrorMessage = "Informe uma URL válida.")]
    [Display(Name = "URL do perfil")]
    public string UrlPerfil { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Selecione uma categoria.")]
    [Display(Name = "Categoria")]
    public int CategoriaId { get; set; }

    public IReadOnlyCollection<SelectListItem> Categorias { get; set; } = [];

    public void LoadCategorias(IReadOnlyCollection<CategoriaModel> categorias)
    {
        Categorias = categorias
            .OrderBy(categoria => categoria.Nome)
            .Select(categoria => new SelectListItem(categoria.Nome, categoria.Id.ToString()))
            .ToList();
    }
}
