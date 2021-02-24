using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleAccount.Models;
using SimpleAccount.Utilities;
using System.Net;
using System.Net.Mail;

namespace SimpleAccount.Services
{
    public interface IUserService 
    {
        Task<Response<List<User>>> UserList();
        Task<Response<User>> UserDetail(int id);
        Task<Response> Login(LoginRequest login);
        Task<Response> Register(User user);
    }
    public class UserService : IUserService
    {
        private readonly SimpleAccountContext _context;
        private IEmailConfiguration _emailConfiguration;

        public UserService(SimpleAccountContext context, IEmailConfiguration emailConfiguration)
        {
            _context = context;
            _emailConfiguration = emailConfiguration;
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

        /// <summary>
        /// Checks if a record exist, simulation a login.
        /// </summary>
        /// <param name="login">Parameter that contains username and password</param>
        /// <returns>Boolean</returns>
        public async Task<Response> Login(LoginRequest login)
        {

            try
            {
                var exist = await _context.Users.AnyAsync(x => x.Email == login.Email && x.Password == login.Password);
                if (exist)
                {
                    return Response.Success();
                }
                return Response.Error("User doesn't exist.");
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message);
            }
        }

        public async Task<Response> Register(User user)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var exist = await _context.Users.AnyAsync(x => x.Email == user.Email);
                    if (exist)
                        return Response.Error("User email already exist.");
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    SendEmail(user.Email, user.LastName + ", " + user.FirstName);
                    return Response.Success();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Response.Error(ex.Message);
                }
            }
        }

        private void SendEmail(string toEmail, string toName) 
        {
            var fromAddress = new MailAddress(_emailConfiguration.Email, _emailConfiguration.Name);
            var toAddress = new MailAddress(toEmail, toName);
            string fromPassword = _emailConfiguration.Password;
            const string subject = "Test";
            string body = $"Hello {toName}! \n You have registered to SimpleAccount. \n Thank you!";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

    }
}
