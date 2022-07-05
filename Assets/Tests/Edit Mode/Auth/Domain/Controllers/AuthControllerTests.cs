using System.Threading.Tasks;
using Auth.Controllers;
using Auth.Domain.Entities;
using Auth.Domain.UseCases;
using Moq;
using NUnit.Framework;

public class AuthControllerTests
{
    AuthControllerImpl controller;
    Mock<IGetCurrentUser> mockUsecase;

    [SetUp]
    public void SetUp()
    {
        mockUsecase = new Mock<IGetCurrentUser>();
    }

    [TearDown]
    public void TearDown()
    {
        controller.Dispose();
    }

    [Test]
    public async Task Should_initialize_with_no_user()
    {
        mockUsecase.Setup((c) => c.Invoke()).Returns(async () =>
        {
            await Task.Delay(20);
            return null;
        });

        controller = new AuthControllerImpl(mockUsecase.Object);

        Assert.That(controller.Initialized.Value, Is.EqualTo(false));
        Assert.IsNull(controller.CurrentUser.Value);

        await Task.Delay(20);

        Assert.That(controller.Initialized.Value, Is.EqualTo(true));
        Assert.IsNull(controller.CurrentUser.Value);
    }

    [Test]
    public async Task Should_initialize_with_user()
    {
        mockUsecase.Setup((c) => c.Invoke()).Returns(async () =>
        {
            await Task.Delay(20);
            return new UserInfo();
        });

        controller = new AuthControllerImpl(mockUsecase.Object);

        Assert.That(controller.Initialized.Value, Is.EqualTo(false));
        Assert.IsNull(controller.CurrentUser.Value);

        await Task.Delay(20);

        Assert.That(controller.Initialized.Value, Is.EqualTo(true));
        Assert.IsNotNull(controller.CurrentUser.Value);
    }

    [Test]
    public void Should_change_user_on_event()
    {
        mockUsecase.Setup((c) => c.Invoke()).ReturnsAsync(() => null);

        controller = new AuthControllerImpl(mockUsecase.Object);

        Assert.That(controller.Initialized.Value, Is.EqualTo(true));
        Assert.IsNull(controller.CurrentUser.Value);

        controller.UserChanged(new UserInfo());

        Assert.IsNotNull(controller.CurrentUser.Value);

        controller.UserChanged(null);

        Assert.IsNull(controller.CurrentUser.Value);
    }
}
