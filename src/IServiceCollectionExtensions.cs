using IdentityModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public static class IServiceCollectionExtensions
    {
        public static IdentityBuilder AddAspNetMembershipAdaptor(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AspNetMembershipDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IPasswordHasher<AspNetMembershipUser>, AspNetMembershipPasswordHasher>();

            return services.AddIdentity<AspNetMembershipUser, IdentityRole>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
            })
                .AddEntityFrameworkStores<AspNetMembershipDbContext>()
                .AddUserStore<AspNetMembershipUserStore>()
                .AddDefaultTokenProviders();
        }
    }
}
