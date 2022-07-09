using System.Threading.Tasks;

namespace App.Domain
{
    public interface IJoinExistingMatch
    {
        public Task Invoke();
    }

    public class JoinExistingMatchImpl : IJoinExistingMatch
    {
        IMatchService service;
        public JoinExistingMatchImpl(IMatchService service)
        {
            this.service = service;
        }

        public Task Invoke()
        {
            return service.JoinExistingMatch();
        }
    }
}