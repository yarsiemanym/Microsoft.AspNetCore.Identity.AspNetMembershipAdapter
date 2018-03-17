using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetMembershipUserStore : IUserStore<AspNetMembershipUser>, IUserPasswordStore<AspNetMembershipUser>, IUserEmailStore<AspNetMembershipUser>
    {
        private readonly AspNetMembershipDbContext _dbcontext;

        public AspNetMembershipUserStore(AspNetMembershipDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public Task<IdentityResult> CreateAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    AspNetUser dbUser = new AspNetUser();
                    this.Convert(user, dbUser);
                    _dbcontext.AspNetUsers.Add(dbUser);
                    _dbcontext.SaveChanges();
                    return IdentityResult.Success;
                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = ex.GetType().Name,
                        Description = ex.Message
                    });
                }
            });
        }

        public Task<IdentityResult> DeleteAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    AspNetUser dbUser = _dbcontext.AspNetUsers
                        .Include(u => u.AspNetApplication)
                        .Include(u => u.AspNetMembership)
                        .SingleOrDefault(u => u.LoweredUserName == user.NormalizedUserName.ToLower());

                    if (dbUser != null)
                    {
                        _dbcontext.AspNetUsers.Remove(dbUser);
                        _dbcontext.SaveChanges();
                    }

                    return IdentityResult.Success;
                }
                catch (Exception ex)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = ex.GetType().Name,
                        Description = ex.Message
                    });
                }
            });
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }

        public Task<AspNetMembershipUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                AspNetUser dbUser = _dbcontext.AspNetUsers
                    .Include(u => u.AspNetApplication)
                    .Include(u => u.AspNetMembership)
                    .SingleOrDefault(u => u.AspNetMembership.Email.ToLower() == normalizedEmail.ToLower());

                if (dbUser == null)
                {
                    return null;
                }

                AspNetMembershipUser user = new AspNetMembershipUser();
                this.Convert(dbUser, user);
                return user;
            });
        }

        public Task<AspNetMembershipUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            Guid gUserId = Guid.Parse(userId);
            return Task.Factory.StartNew(() =>
            {
                AspNetUser dbUser = _dbcontext.AspNetUsers
                    .Include(u => u.AspNetApplication)
                    .Include(u => u.AspNetMembership)
                    .SingleOrDefault(u => u.UserId == gUserId);

                if (dbUser == null)
                {
                    return null;
                }

                AspNetMembershipUser user = new AspNetMembershipUser();
                this.Convert(dbUser, user);
                return user;
            });
        }

        public Task<AspNetMembershipUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                AspNetUser dbUser = _dbcontext.AspNetUsers
                    .Include(u => u.AspNetApplication)
                    .Include(u => u.AspNetMembership)
                    .SingleOrDefault(u => u.LoweredUserName == normalizedUserName.ToLower());

                if (dbUser == null)
                {
                    return null;
                }

                AspNetMembershipUser user = new AspNetMembershipUser();
                this.Convert(dbUser, user);
                return user;
            });
        }

        public Task<string> GetEmailAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.UserName);
        }

        public Task<bool> HasPasswordAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => !string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetEmailAsync(AspNetMembershipUser user, string email, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.Email = email);
        }

        public Task SetEmailConfirmedAsync(AspNetMembershipUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.EmailConfirmed = confirmed);
        }

        public Task SetNormalizedEmailAsync(AspNetMembershipUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.NormalizedEmail = normalizedEmail);
        }

        public Task SetNormalizedUserNameAsync(AspNetMembershipUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.NormalizedUserName = normalizedName);
        }

        public Task SetPasswordHashAsync(AspNetMembershipUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.PasswordHash = passwordHash);
        }

        public Task SetUserNameAsync(AspNetMembershipUser user, string userName, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.UserName = userName);
        }

        public Task<IdentityResult> UpdateAsync(AspNetMembershipUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    AspNetUser dbUser = _dbcontext.AspNetUsers
                        .Include(u => u.AspNetApplication)
                        .Include(u => u.AspNetMembership)
                        .SingleOrDefault(u => u.UserId.ToString() == user.Id);

                    if (dbUser != null)
                    {
                        this.Convert(user, dbUser);
                        _dbcontext.AspNetUsers.Update(dbUser);
                        _dbcontext.SaveChanges();
                    }
                    return IdentityResult.Success;
                }
                catch(Exception ex)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = ex.GetType().Name,
                        Description = ex.Message
                    });
                }
            });
        }

        private void Convert(AspNetUser from, AspNetMembershipUser to)
        {
            to.Id = from.UserId.ToString();
            to.UserName = from.UserName;
            to.NormalizedUserName = from.LoweredUserName.ToUpper();
            to.Email = from.UserName;
            to.NormalizedEmail = from.AspNetMembership.Email.ToUpper();
            to.EmailConfirmed = true;
            to.PasswordHash = from.AspNetMembership.Password;
            to.PasswordSalt = from.AspNetMembership.PasswordSalt;
            to.PasswordFormat = from.AspNetMembership.PasswordFormat;
            to.AccessFailedCount = from.AspNetMembership.FailedPasswordAttemptCount;
            to.EmailConfirmed = true;
            to.SecurityStamp = Guid.NewGuid().ToString(); // TODO: This isn't right.
        }

        private void Convert(AspNetMembershipUser from , AspNetUser to)
        {
            AspNetApplication application = _dbcontext.AspNetApplications.First();

            to.ApplicationId = application.ApplicationId;
            to.AspNetApplication = application;
            to.AspNetMembership = new AspNetMembership
            {
                ApplicationId = application.ApplicationId,
                AspNetApplication = application
            };

            to.UserId = Guid.Parse(from.Id);
            to.UserName = from.UserName;
            to.LoweredUserName = from.UserName.ToLower();
            to.LastActivityDate = DateTime.UtcNow;
            to.IsAnonymous = false;
            to.ApplicationId = application.ApplicationId;
            to.AspNetMembership.CreateDate = DateTime.UtcNow;
            to.AspNetMembership.Email = from.Email;
            to.AspNetMembership.IsApproved = true;
            to.AspNetMembership.LastLoginDate = DateTime.Parse("1754-01-01 00:00:00.000");
            to.AspNetMembership.LastLockoutDate = DateTime.Parse("1754-01-01 00:00:00.000");
            to.AspNetMembership.LastPasswordChangedDate = DateTime.Parse("1754-01-01 00:00:00.000");
            to.AspNetMembership.LoweredEmail = from.NormalizedEmail.ToLower();
            to.AspNetMembership.Password = from.PasswordHash;
            to.AspNetMembership.PasswordSalt = from.PasswordSalt;
            to.AspNetMembership.PasswordFormat = from.PasswordFormat;
            to.AspNetMembership.IsLockedOut = false;
            to.AspNetMembership.FailedPasswordAnswerAttemptWindowStart = DateTime.Parse("1754-01-01 00:00:00.000");
            to.AspNetMembership.FailedPasswordAttemptWindowStart = DateTime.Parse("1754-01-01 00:00:00.000");
        }
    }
}
