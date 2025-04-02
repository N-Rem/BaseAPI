using Domain.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserProjectRepository : BaseRepository<UserProject>, IUserProjectRepository
    {
        private readonly AppDbContext _context;
        public UserProjectRepository(AppDbContext context) : base(context) { _context = context; }

    }
}
