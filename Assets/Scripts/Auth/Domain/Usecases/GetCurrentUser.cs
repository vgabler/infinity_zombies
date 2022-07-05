using Auth.Domain.Repositories;
using Auth.Domain.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using UnityEngine;

namespace Auth.Domain.UseCases
{
    public interface IGetCurrentUser
    {
        public Task<UserInfo> Invoke();
    }
    public class GetCurrentUserImpl : IGetCurrentUser
    {
        readonly IAuthRepository repository;
        public GetCurrentUserImpl(IAuthRepository repository)
        {
            this.repository = repository;
        }

        async public Task<UserInfo> Invoke()
        {
            return await repository.GetCurrentUser();
        }
    }
}