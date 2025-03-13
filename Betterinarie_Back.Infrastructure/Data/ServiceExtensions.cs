using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Infrastructure.Data
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionStrings)
        {
            services.AddDbContext<BetterinarieContext>(options =>
                    options.UseSqlServer(connectionStrings));
            return services;
        }
    }
}
