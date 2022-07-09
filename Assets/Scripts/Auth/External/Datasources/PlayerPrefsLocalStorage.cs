using Auth.Domain;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Auth.External
{
    /// <summary>
    /// PlayerPrefs s� roda no main thread, ent�o precisa mandar as fun��es para l�
    /// </summary>
    public class PlayerPrefsLocalStorage : ILocalStorage
    {
        public Task Delete(string key)
        {
            return UnityMainThreadDispatcher.Instance().EnqueueAsync(() => PlayerPrefs.DeleteKey(key));
        }

        public async Task<T> Get<T>(string key)
        {
            object result = null;

            //Playerprefs s� funciona no main thread
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                if (IsSameType<T, string>())
                {
                    result = PlayerPrefs.GetString(key);
                }
                else if (IsSameType<T, float>())
                {
                    result = PlayerPrefs.GetFloat(key);
                }
                else if (IsSameType<T, int>())
                {
                    result = PlayerPrefs.GetInt(key);
                }
                else
                {
                    throw new NotImplementedException();
                }
            });

            return (T)result;
        }

        public Task Set<T>(string key, T value)
        {
            object val = value;

            return UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                if (IsSameType<T, string>())
                {
                    PlayerPrefs.SetString(key, (string)val);
                }
                else if (IsSameType<T, float>())
                {
                    PlayerPrefs.SetFloat(key, (float)val);
                }
                else if (IsSameType<T, int>())
                {
                    PlayerPrefs.SetInt(key, (int)val);
                }
                else
                {
                    throw new NotImplementedException();
                }
            });
        }

        bool IsSameType<T1, T2>() => typeof(T1) == typeof(T2);
    }

}