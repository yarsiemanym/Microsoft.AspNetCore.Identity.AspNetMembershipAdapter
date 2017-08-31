using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public static class IServiceCollectionExtensions
    {
        public static IdentityBuilder AddAspNetMembershipBackedAspNetIdentity(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AspNetMembershipDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IPasswordHasher<AspNetMembershipUser>, AspNetMembershipPasswordHasher>();

            return services.AddIdentity<AspNetMembershipUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetMembershipDbContext>()
                .AddUserStore<AspNetMembershipUserStore>()
                .AddDefaultTokenProviders();
        }

        public static IdentityBuilder AddAspNetMembershipBackedAspNetIdentity(this IServiceCollection services, string connectionString, Action<IdentityOptions> setupAction)
        {
            services.AddDbContext<AspNetMembershipDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IPasswordHasher<AspNetMembershipUser>, AspNetMembershipPasswordHasher>();

            return services.AddIdentity<AspNetMembershipUser, IdentityRole>(setupAction)
                .AddEntityFrameworkStores<AspNetMembershipDbContext>()
                .AddUserStore<AspNetMembershipUserStore>()
                .AddDefaultTokenProviders();
        }
    }
}
