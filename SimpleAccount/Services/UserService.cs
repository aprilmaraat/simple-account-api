using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleAccount.Models;

namespace SimpleAccount.Services
{
    public class UserService
    {
        private readonly SimpleAccountContext _context;

        public UserService(SimpleAccountContext context)
        {
            _context = context;
        }
    }
}
