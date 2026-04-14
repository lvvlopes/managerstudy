using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ContentHub.Web.Models;
using ContentHub.Web.Services;
using ContentHub.Web.ViewModels;

namespace ContentHub.Web.Controllers;

public sealed class HomeController : Controller
{
    private readonly IContentHubApiClient _contentHubApiClient;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IContentHubApiClient contentHubApiClient, ILogger<HomeController> logger)
    {
        _contentHubApiClient = contentHubApiClient;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? categoria, CancellationToken cancellationToken)
    {
        var categorias = await _contentHubApiClient.GetCategoriasAsync(cancellationToken);
        var filtros = CategoryFilterViewModel.Build(categorias, categoria);
        var categoriaApi = filtros.FirstOrDefault(filtro => filtro.IsActive)?.QueryValue;

        var posts = await _contentHubApiClient.GetPostsAsync(categoriaApi, cancellationToken);
        if (!_contentHubApiClient.IsAvailable)
        {
            _logger.LogWarning("API Content Hub indisponível em {BaseUrl}.", _contentHubApiClient.BaseUrl);
        }

        return View(new FeedViewModel(
            Posts: posts,
            Categorias: filtros,
            CategoriaSelecionada: categoria,
            ApiBaseUrl: _contentHubApiClient.BaseUrl,
            ApiDisponivel: _contentHubApiClient.IsAvailable));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
