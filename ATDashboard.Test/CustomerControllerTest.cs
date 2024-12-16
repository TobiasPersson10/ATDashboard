using ATDashboard.Controllers;
using ATDashboard.Models.DTO;
using ATDashboard.Models.Requests;
using ATDashboard.Models.SkeKraftModels;
using ATDashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1;

[TestFixture]
public class CustomerControllerTest
{
    private Mock<IAuthService> _authServiceMock;
    private Mock<ICustomerService> _customerServiceMock;
    private CustomerController _controller;

    [SetUp]
    public void SetUp()
    {
        _authServiceMock = new Mock<IAuthService>();
        _customerServiceMock = new Mock<ICustomerService>();

        _controller = new CustomerController(_authServiceMock.Object, _customerServiceMock.Object);
    }

    [Test]
    public async Task GetCustomerInfo_TokenIsNull_ReturnsUnauthorized()
    {
        // Arrange
        _authServiceMock.Setup(a => a.GetAccessToken()).Returns((string?)null);

        // Act
        var result = await _controller.GetCustomerInfo();

        // Assert
        var unauthorizedResult = result as UnauthorizedObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            Assert.That("Token is not loaded, attempt a login", Is.EqualTo(unauthorizedResult.Value));
        });
    }

    [Test]
    public async Task GetCustomerInfo_TokenIsNotNullButNoCustomerInfo_ReturnsNotFound()
    {
        // Arrange
        _authServiceMock.Setup(a => a.GetAccessToken()).Returns("some-token");
        _customerServiceMock
            .Setup(cs => cs.GetCustomerInfo(It.IsAny<CustomerInfoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CustomerInfoResponse)null!);

        // Act
        var result = await _controller.GetCustomerInfo();

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task GetCustomerInfo_TokenIsNotNullAndCustomerInfoFound_ReturnsOkWithDto()
    {
        // Arrange
        _authServiceMock.Setup(a => a.GetAccessToken()).Returns("some-token");

        var customerInfoResponse = new CustomerInfoResponse
        {
            CustomerInfo = new CustomerInfo
            {
                Country = "Sweden",
                CountryCode = "SE",
                PoaRights = "Full",
                Source = "All",
                CustomerName = "John Doe",
                CustomerId = "12345",
                Address = "Main Street 1",
                CoAddress = "Care of Smith",
                ZipCode = "12345",
                Phone = "+46123456789",
                Email = "john.doe@example.com",
                Paymode = "CreditCard",
                PerOrgNr = "999999-9999",
                Type = "Private",
                IsPoa = "No",
            },
        };

        _customerServiceMock
            .Setup(cs => cs.GetCustomerInfo(It.IsAny<CustomerInfoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customerInfoResponse);

        // Act
        var result = await _controller.GetCustomerInfo();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okResult.Value, Is.InstanceOf<CustomerInfoDto>());
        });

        var dto = okResult.Value as CustomerInfoDto;
        Assert.Multiple(() =>
        {
            Assert.That("Sweden", Is.EqualTo(dto.Country));
            Assert.That("SE", Is.EqualTo(dto.CountryCode));
            Assert.That("Full", Is.EqualTo(dto.PoaRights));
            Assert.That("All", Is.EqualTo(dto.Source));
            Assert.That("John Doe", Is.EqualTo(dto.CustomerName));
            Assert.That("12345", Is.EqualTo(dto.CustomerId));
            Assert.That("Main Street 1", Is.EqualTo(dto.Address));
            Assert.That("Care of Smith", Is.EqualTo(dto.CoAddress));
        });
        Assert.Multiple(() =>
        {
            Assert.That("12345", Is.EqualTo(dto.ZipCode));
            Assert.That("+46123456789", Is.EqualTo(dto.Phone));
            Assert.That("john.doe@example.com", Is.EqualTo(dto.Email));
            Assert.That("CreditCard", Is.EqualTo(dto.Paymode));
            Assert.That("999999-9999", Is.EqualTo(dto.PerOrgNr));
            Assert.That("Private", Is.EqualTo(dto.Type));
            Assert.That("No", Is.EqualTo(dto.IsPoa));
        });
    }
}
