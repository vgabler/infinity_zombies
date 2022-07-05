using Auth.Domain.Entities;
using Auth.Domain.Repositories;
using System.Threading.Tasks;

namespace Auth.Infra.Repositories
{
    public class MockAuthRepository : IAuthRepository
    {
        readonly UserInfo mockUserInfo;
        readonly int milisecondDelay;
        public MockAuthRepository(UserInfo mockUserInfo, int milisecondDelay = 2000)
        {
            this.mockUserInfo = mockUserInfo;
            this.milisecondDelay = milisecondDelay;
        }

        async public Task<UserInfo> GetCurrentUser()
        {
            await Task.Delay(milisecondDelay);
            return mockUserInfo;
        }
    }
}
