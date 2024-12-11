namespace ATDashboard.Services;

public class CustomerService
{
    private readonly IAuthService _authService;
    public CustomerService(IAuthService authService)
    {
        _authService = authService;
    }
}