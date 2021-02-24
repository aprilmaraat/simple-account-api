using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleAccount.Models;
using SimpleAccount.Utilities;

namespace SimpleAccount.Services
{
    public class UserService
    {
        private readonly SimpleAccountContext _context;

        public UserService(SimpleAccountContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the list of existing users
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<User>>> UserList()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Response<List<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return Response<List<User>>.Error(ex.Message);
            }
        }

        /// <summary>
        /// Gets the user data of the specified id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns></returns>
        public async Task<Response<User>> UserDetail(int id) 
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                return Response<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Response<User>.Error(ex.Message);
            }
        }

    }
}
