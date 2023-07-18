using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices;

public interface IEmailService
{    
    public Task<bool> SendMailAsync(List<string> email, string subject, string message);
}
