using Kinobaza.BLL.Interfaces;
using Kinobaza.BLL.Services;
using Kinobaza.DAL.Interfaces;
using Kinobaza.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinobaza.BLL.Infrastructure
{
    public static class ApplicationServicesExtensions
    {
        public static void AddApplicationServicesService(this IServiceCollection services)
        {
            services.AddScoped<IForumService, ForumService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
