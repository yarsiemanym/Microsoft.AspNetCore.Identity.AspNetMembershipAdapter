# Microsoft.AspNetCore.Identity.AspNetMembershipAdapter #

`Adaptor code that allows ASP.NET Identity to consume an ASP.NET Membership database.`

Recently I was tasked with developing an authentication web app using .NET Core, ASP.NET Identity and IdentityServer4.  The catch was that I had to use an existing ASP.NET Membership database as my user store.  Out of the box, ASP.NET Identity attempts to create it's own database and use it's own schema, much like Membership did, but of course, the schema is different from Membership.

Fortunately, ASP.NET Identity offer pleanty of extension points for you to customize you authentication stack.  This project contains the code that I came up with to adapt ASP.NET Identity to comsume my ASP.NET Membership database.

If you'd like to use it of the shelf, simple reference the project in you application and run the following code in your Startup.cs file.

    public class Startup
    {
      public void ConfigureServices(IServiceCollection services)
      {
        string connectionString = Configuration.GetConnectionString("<ConnectionStringName>");
        services.AddAspNetMembershipBackedAspNetIdentity(connectionString);
      }
    }
    
This will add ASP.NET Identity to you service collection as well as all the extension points required to consume an ASP.NET Membership database.

**Caveats**:
1. If you don't use the default ASP.NET Membership password hashing algorithm then you will need to implement a custom IPasswordHasher that mimics your custom ASP.NET Membership password hashing algorithm.  Othewise, ASP.NET Identity won't be able to udnerstand the hashed passwords stored in your database.
2. I've only tested this in .NET Core.  It may work in other .NET environments or it may not. 
3. This project is just the bare essentials, if you need things like user roles or additional user data then you will have to implement that yourself.  If you can do it in an elegant and generic way then I'd be glad to approve your pull request ;-)
