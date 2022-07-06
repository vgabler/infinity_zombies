using Auth.Domain;
using System;
using System.Threading.Tasks;

namespace Auth.Infra
{
    public class BrokenAuthRepository : IAuthRepository
    {
        public Task<UserInfo> GetCurrentUser()
        {
            throw new NotImplementedException();
        }
    }
}
