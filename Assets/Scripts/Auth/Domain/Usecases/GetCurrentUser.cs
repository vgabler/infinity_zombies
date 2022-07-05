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
    public class GetCurrentUser : IGetCurrentUser
    {
        IAuthRepository repository;
        public GetCurrentUser(IAuthRepository repository)
        {
            this.repository = repository;
        }

        async public Task<UserInfo> Invoke()
        {
            return await repository.GetCurrentUser();
        }
    }
}