using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Domain.Repositories;
using Auth.Domain.UseCases;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GetCurrentUserTest
{
    GetCurrentUser usecase;
    Mock<IAuthRepository> mockRepository;

    [SetUp]
    public void SetUp()
    {
        mockRepository = new Mock<IAuthRepository>();
        usecase = new GetCurrentUser(mockRepository.Object);
    }

    [Test]
    public async Task Should_get_data_if_authenticated()
    {
        mockRepository.Setup((repo) => repo.GetCurrentUser()).ReturnsAsync(new Auth.Domain.Entities.UserInfo());

        var result = await usecase.Invoke();

        Assert.IsNotNull(result);
    }

    [Test]
    public async Task Should_get_null_if_unauthenticated()
    {
        mockRepository.Setup((repo) => repo.GetCurrentUser()).ReturnsAsync(() => null);

        var result = await usecase.Invoke();

        Assert.IsNull(result);
    }
}
