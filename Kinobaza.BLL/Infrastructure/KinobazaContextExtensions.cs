using Kinobaza.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kinobaza.BLL.Infrastructure
{
    public static class KinobazaContextExtensions
    {
        public static void AddKinobazaDBContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<KinobazaDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
