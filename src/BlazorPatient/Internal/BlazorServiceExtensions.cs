using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace BlazorPatient.Internal
{
    public static class BlazorServiceExtensions
    {
        public static IServiceCollection AddCustomBlazorService(this IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddServerSideBlazor();
            services.AddMudServices();

            return services;
        }
    }
}
