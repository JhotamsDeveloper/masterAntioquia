using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs.SendGrid
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
