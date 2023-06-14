using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices;

public interface IJwtBlacklistService
{
    void RevokeToken(string token);
    bool IsTokenRevoked(string token);
}
