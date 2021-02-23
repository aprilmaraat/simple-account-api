using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAccount.Utilities
{
    public enum ResponseState
    {
        /// <summary>
        /// Success response state
        /// </summary>
        Success,
        /// <summary>
        /// Error response state
        /// </summary>
        Error,
        /// <summary>
        /// Exception response state
        /// </summary>
        Exception
    }
}
