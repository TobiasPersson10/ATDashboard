using System.Net;
using ATDashboard.Controllers;
using ATDashboard.Models;
using ATDashboard.Models.DTO;
using ATDashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1;

public class Tests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public async Task Login()
    {
        Mock<IAuthService> authServiceMock = new Mock<IAuthService>();
        var mockedResponse = new LoginResponse
        {
            PassWordExpired = "1",
            IsPoaUser = "no",
            IsInkopsStrategyAdmin = "no",
            UserFullname = "Namnen namnenssson",
            CustomerType = "Privat",
            LoginBankID = "no",
            Email = "mail@mail.se",
            Dst = Guid.NewGuid().ToString(),
            IndividFullname = "Namn namnsson",
        };

        authServiceMock
            .Setup(x => x.LoginAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockedResponse);

        var controller = new AuthController(authServiceMock.Object);

        var result = await controller.Login(new CancellationToken());
        var value = result as OkObjectResult;
        Assert.That(value?.Value, Is.Not.Null);
        Assert.Equals(value?.StatusCode, HttpStatusCode.OK);
        Assert.That(value.Value, Is.InstanceOf<LoginResponseDTO>());
    }
}
