using Domain.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context) { _context = context; }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

    }
}
