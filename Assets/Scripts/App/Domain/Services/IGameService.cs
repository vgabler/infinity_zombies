using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityZombies.Domain
{
    //TODO Infra
    public interface IGameService
    {
        public Task StartNewGame();
        public Task JoinExistingGame();
    }
}
