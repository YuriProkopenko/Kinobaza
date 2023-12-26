using Microsoft.Extensions.DependencyInjection;
using Kinobaza.DAL.Interfaces;
using Kinobaza.DAL.Repositories;

namespace Kinobaza.BLL.Infrastructure
{
    public static class UnitOfWorkServiceExtensions
    {
        public static void AddUnitOfWorkService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
        }
    }
}
