using System.Threading.Tasks;
using Auth.Domain;
using Moq;
using NUnit.Framework;

public class GetCurrentUserTest
{
    GetCurrentUserImpl usecase;
    Mock<IAuthRepository> mockRepository;

    [SetUp]
    public void SetUp()
    {
        mockRepository = new Mock<IAuthRepository>();
        usecase = new GetCurrentUserImpl(mockRepository.Object);
    }

    [Test]
    public async Task Should_get_data_if_authenticated()
    {
        mockRepository.Setup((repo) => repo.GetCurrentUser()).ReturnsAsync(new UserInfo());

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
