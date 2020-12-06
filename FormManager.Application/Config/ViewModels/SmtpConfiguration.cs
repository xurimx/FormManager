using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Config.ViewModels
{
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
