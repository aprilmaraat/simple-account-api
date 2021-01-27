using System;
using System.Collections.Generic;

#nullable disable

namespace SimpleAccount.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public byte[] LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
