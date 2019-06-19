using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Chatison.DataLayer
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? CurrentSignInAt { get; set; }
        [MaxLength(40)]
        public string CurrentSignInIp { get; set; }
        public DateTime? LastSignInAt { get; set; }
        [MaxLength(40)]
        public string LastSignInIp { get; set; }
        public int SignInCount { get; set; }
        public Utilities.Constants.RecordStatus Status { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
