using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotnetAppDemo.Web.ApiClient;

public class ItemApiClient(HttpClient httpClient, ILogger<ItemApiClient> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<ItemApiClient> _logger = logger;

    public async Task<ItemModel[]> GetItemAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        var items = new List<ItemModel>();

        try
        {
            await foreach (var item in _httpClient.GetFromJsonAsAsyncEnumerable<ItemModel>("Item", cancellationToken))
            {
                if (items.Count >= maxItems)
                    break;

                if (item is not null)
                    items.Add(item);
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro ao obter itens da API.");
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("A requisição para obter itens foi cancelada.");
        }

        return items.ToArray();
    }
}

public record ItemModel
{
    public required string Name { get; init; }
}
