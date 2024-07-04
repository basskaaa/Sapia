using Microsoft.Extensions.DependencyInjection;
using Sapia.Game.Services;

namespace Sapia.Game.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSapiaGameServices(this IServiceCollection services)
    {
        services.AddSingleton<IGoldService, GoldService>();
    }
}