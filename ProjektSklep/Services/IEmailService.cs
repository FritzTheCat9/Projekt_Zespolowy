using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Services
{
    public interface IEmailService
    {
        void SendEmail(string email, string subject, string htmlMessage);
    }
}
