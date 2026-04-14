using System.ComponentModel.DataAnnotations;
using ContentHub.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContentHub.Web.ViewModels;

public sealed class CriarPostViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecione uma fonte.")]
    [Display(Name = "Fonte")]
    public int FonteId { get; set; }

    [Required(ErrorMessage = "Informe a URL do post.")]
    [Url(ErrorMessage = "Informe uma URL válida.")]
    [Display(Name = "URL do post")]
    public string Url { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Use até 2000 caracteres.")]
    [Display(Name = "Descrição")]
    public string? Descricao { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data do post")]
    public DateTime? DataPost { get; set; }

    public IReadOnlyCollection<SelectListItem> Fontes { get; set; } = [];

    public void LoadFontes(IReadOnlyCollection<FonteModel> fontes)
    {
        Fontes = fontes
            .OrderBy(fonte => fonte.Nome)
            .Select(fonte => new SelectListItem(fonte.DisplayName, fonte.Id.ToString()))
            .ToList();
    }
}
