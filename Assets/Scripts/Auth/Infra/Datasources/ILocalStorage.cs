using System.Threading.Tasks;

namespace Auth.Domain
{
    public interface ILocalStorage
    {
        public Task<T> Get<T>(string key);
        public Task Set<T>(string key, T value);
        public Task Delete(string key);
    }
}
