using DotnetAppDemo.Service.Interfaces;
using DotnetAppDemo.Service.Services;

namespace DotnetAppDemo.API.Extensions
{
    internal static class ServicesExtensions
    {
        internal static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IItemService, ItemService>();

            return services;
        }
    }
}
