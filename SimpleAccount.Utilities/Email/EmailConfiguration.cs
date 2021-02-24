using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAccount.Utilities
{
	public interface IEmailConfiguration
	{
		string Email { get; set; }
		string Name { get; set; }
		string Password { get; set; }
	}

	public class EmailConfiguration : IEmailConfiguration
	{
		public string Email { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
	}
}
