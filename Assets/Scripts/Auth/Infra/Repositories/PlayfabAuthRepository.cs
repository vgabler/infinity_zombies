using Auth.Domain;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Auth.Infra
{
    //TODO na verdade isso deveria ser um External DataSource
    public class PlayfabAuthRepository : IAuthRepository
    {
        //As chamadas do playfab precisam ser na main thread..
        static readonly string PERSIST_ID_KEY = "REMEMBER_ME_ID";

        UserInfo currentUser;
        ILocalStorage localStorage;

        public PlayfabAuthRepository(ILocalStorage localStorage)
        {
            this.localStorage = localStorage;
            //Precisa disso se não falha tudo...
            PlayFabSettings.TitleId = "25392";
        }

        public async Task<UserInfo> GetCurrentUser()
        {
            //Se já está logado, só entra
            if (currentUser != null)
            {
                return currentUser;
            }

            //Se não tem usuário, verifica se tem no local storage token pra login automático
            var persistId = await localStorage.Get<string>(PERSIST_ID_KEY);

            if (string.IsNullOrEmpty(persistId))
            {
                return null;
            }

            //Se tem token, tenta fazer o login automático
            var tcs = new TaskCompletionSource<LoginResult>();
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                try
                {
                    PlayFabClientAPI.LoginWithCustomID(
                        new LoginWithCustomIDRequest { CustomId = persistId },
                        (r) => tcs.TrySetResult(r),
                        (error) =>
                        {
                            Debug.LogError(error);
                            //TODO retornar esse erro
                            tcs.TrySetCanceled();
                        }
                    );
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    //tcs.TrySetException(e);
                    tcs.SetResult(null);
                }
            });

            var result = await tcs.Task;

            if (result == null)
            {
                //Se falhou, o persist ID é inválido
                await ClearPersistID();
                return UpdateCurrentUser();
            }

            return UpdateCurrentUser(result.PlayFabId, (string)result.CustomData);
        }

        public async Task<UserInfo> SignIn(string email, string password)
        {
            var tcs = new TaskCompletionSource<LoginResult>();
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                try
                {
                    PlayFabClientAPI.LoginWithEmailAddress(
                        new LoginWithEmailAddressRequest
                        {
                            Email = email,
                            Password = password,
                        },
                        (r) => tcs.SetResult(r),
                        (error) =>
                        {
                            Debug.LogError(error);
                            tcs.SetResult(null);
                            //TODO retornar esse erro
                        }
                    );
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    //tcs.SetException(e);
                    tcs.SetResult(null);
                }
            });

            var result = await tcs.Task;

            if (result == null)
            {
                return UpdateCurrentUser();
            }

            //Se teve sucesso, salva um novo persist id
            await SavePersistId();
            return UpdateCurrentUser(result.PlayFabId, (string)result.CustomData);
        }

        public Task SignOut()
        {
            return ClearPersistID();
        }

        public async Task<UserInfo> SignUp(string email, string password, string nickname)
        {
            //TODO criptografar a senha

            var tcs = new TaskCompletionSource<RegisterPlayFabUserResult>();
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                try
                {
                    PlayFabClientAPI.RegisterPlayFabUser(
                        new RegisterPlayFabUserRequest
                        {
                            Email = email,
                            Password = password,
                            RequireBothUsernameAndEmail = false,
                        },
                        (r) =>
                            tcs.SetResult(r),
                        (error) =>
                        {
                            Debug.LogError(error);
                            tcs.SetResult(null);
                            //TODO retornar esse erro
                        }
                    );
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    //tcs.TrySetException(e);
                    tcs.SetResult(null);
                }
            });

            var result = await tcs.Task;

            if (result == null)
            {
                return UpdateCurrentUser();
            }

            //Se teve sucesso, salva um novo persist id
            await SavePersistId();
            return UpdateCurrentUser(result.PlayFabId, (string)result.CustomData);
        }

        UserInfo UpdateCurrentUser(string id = null, string nickname = null)
        {
            if (id == null)
            {
                return currentUser = null;
            }
            else
            {
                return currentUser = new UserInfo { Id = id, Nickname = nickname };
            }
        }

        async Task ClearPersistID()
        {
            var persistId = await localStorage.Get<string>(PERSIST_ID_KEY);

            //Se não tiver id persistido, ignora
            if (string.IsNullOrEmpty(persistId))
            {
                return;
            }

            //Remove do local storage
            await localStorage.Delete(PERSIST_ID_KEY);

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                //Remove da conta, ignorando falha ou sucesso
                PlayFabClientAPI.UnlinkCustomID(
                    new UnlinkCustomIDRequest { CustomId = persistId },
                    null,
                    null
                );
            });
        }

        async Task SavePersistId()
        {
            //Cria um novo id
            var persistId = Guid.NewGuid().ToString();

            //Salva localmente
            await localStorage.Set(PERSIST_ID_KEY, persistId);

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                //Adiciona à conta, ignorando falha ou sucesso
                PlayFabClientAPI.LinkCustomID(
                    new LinkCustomIDRequest
                    {
                        CustomId = persistId,
                        ForceLink = true
                    },
                    null,
                    null
                );
            });
        }
    }
}