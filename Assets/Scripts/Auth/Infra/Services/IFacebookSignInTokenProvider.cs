using System.Threading.Tasks;

namespace Auth.Infra
{
    public interface IFacebookService
    {
        public Task<string> GetSignInToken();
    }
}
