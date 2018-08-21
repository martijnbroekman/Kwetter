using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwetter.Models;
using Kwetter.Service.Patterns;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.Service
{
    public interface IUserRoleService
    {
        Task<bool> SetRole(int userId, string role);
    }
    
    public class UserRoleService : IUserRoleService
    {
        private readonly DbContext _context;
        private readonly IQueryable<ApplicationUserRole> _queryable;
        
        public UserRoleService(DbContext context)
        {
            _context = context;
            _queryable = context.Set<ApplicationUserRole>();
        }


        public async Task<bool> SetRole(int userId, string role)
        {
            var roleQueryable = _context.Set<ApplicationRole>();
            var newRole = await roleQueryable.FirstOrDefaultAsync(r => r.Name.Equals(role));

            if (await _queryable.AnyAsync(ur => ur.RoleId.Equals(newRole.Id) && ur.UserId.Equals(userId)))
            {
                return false;
            }

            var oldRoles = await _queryable.Where(ur => ur.UserId.Equals(userId)).ToListAsync();
            _context.RemoveRange(oldRoles);

            await _context.AddAsync(new ApplicationUserRole {UserId = userId, RoleId = newRole.Id});
            return true;
        }
    }
}
