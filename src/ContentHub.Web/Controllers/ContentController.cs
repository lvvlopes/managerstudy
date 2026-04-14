using ContentHub.Web.Services;
using ContentHub.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.Web.Controllers;

public sealed class ContentController(IContentHubApiClient contentHubApiClient) : Controller
{
    [HttpGet("categorias/nova")]
    public IActionResult NovaCategoria()
    {
        return View(new CriarCategoriaViewModel());
    }

    [HttpPost("categorias/nova")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NovaCategoria(CriarCategoriaViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await contentHubApiClient.CreateCategoriaAsync(model, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Não foi possível salvar a categoria.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Categoria salva com sucesso.";
        return RedirectToAction(nameof(NovaFonte));
    }

    [HttpGet("fontes/nova")]
    public async Task<IActionResult> NovaFonte(CancellationToken cancellationToken)
    {
        var model = new CriarFonteViewModel();
        await LoadCategoriasAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost("fontes/nova")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NovaFonte(CriarFonteViewModel model, CancellationToken cancellationToken)
    {
        await LoadCategoriasAsync(model, cancellationToken);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await contentHubApiClient.CreateFonteAsync(model, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Não foi possível salvar a fonte.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Fonte salva com sucesso. Agora salve o link como post.";
        return RedirectToAction(nameof(NovoPost), new { fonteId = result.Data?.Id });
    }

    [HttpGet("posts/novo")]
    public async Task<IActionResult> NovoPost(int? fonteId, CancellationToken cancellationToken)
    {
        var model = new CriarPostViewModel
        {
            FonteId = fonteId ?? 0
        };
        await LoadFontesAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost("posts/novo")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NovoPost(CriarPostViewModel model, CancellationToken cancellationToken)
    {
        await LoadFontesAsync(model, cancellationToken);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await contentHubApiClient.CreatePostAsync(model, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Não foi possível salvar o post.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Post salvo com sucesso.";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("posts/{id:int}/editar")]
    public async Task<IActionResult> EditarPost(int id, CancellationToken cancellationToken)
    {
        var post = await contentHubApiClient.GetPostAsync(id, cancellationToken);
        if (post is null)
        {
            TempData["ErrorMessage"] = "Post não encontrado.";
            return RedirectToAction("Index", "Home");
        }

        var model = EditarPostViewModel.FromPost(post);
        await LoadFontesAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost("posts/{id:int}/editar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarPost(int id, EditarPostViewModel model, CancellationToken cancellationToken)
    {
        model.Id = id;
        await LoadFontesAsync(model, cancellationToken);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await contentHubApiClient.UpdatePostAsync(model, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Não foi possível atualizar o post.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Post atualizado com sucesso.";
        return RedirectToAction("Index", "Home");
    }

    private async Task LoadCategoriasAsync(CriarFonteViewModel model, CancellationToken cancellationToken)
    {
        var categorias = await contentHubApiClient.GetCategoriasAsync(cancellationToken);
        model.LoadCategorias(categorias);
    }

    private async Task LoadFontesAsync(CriarPostViewModel model, CancellationToken cancellationToken)
    {
        var fontes = await contentHubApiClient.GetFontesAsync(cancellationToken);
        model.LoadFontes(fontes);
    }

    private async Task LoadFontesAsync(EditarPostViewModel model, CancellationToken cancellationToken)
    {
        var fontes = await contentHubApiClient.GetFontesAsync(cancellationToken);
        model.LoadFontes(fontes);
    }
}
