using System;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using SimpleAccount.Utilities;

namespace SimpleAccount.Api.Controllers
{
    public class BaseController : Controller
    {
        private IEmailConfiguration _emailConfiguration;

        public BaseController(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }
    }
}