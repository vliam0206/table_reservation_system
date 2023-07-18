using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons;

public class AppConfiguration
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public JwtConfiguration JwtConfiguration { get; set; }
    public EmailSetting EmailSetting { get; set; }
}
#region Classes
public class ConnectionStrings
{
    public string DefaultDB { get; set; }
}

public class JwtConfiguration
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
public class EmailSetting
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string DisplayName { get; set; }
}
#endregion
