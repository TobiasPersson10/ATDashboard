namespace ATDashboard.Services;

public class CustomerService : ICustomerService
{
    private readonly IAuthService _authService;

    public CustomerService(IAuthService authService)
    {
        _authService = authService;
    }

    public Task GetCustomerInfo()
    {
        throw new NotImplementedException();
    }
}

public interface ICustomerService
{
    Task GetCustomerInfo();
}
