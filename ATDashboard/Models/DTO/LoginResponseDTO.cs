namespace ATDashboard.Models.DTO;

public record LoginResponseDto
{
    public string? UserFullname { get; set; }
    public string? IndividFullname { get; set; }
    public string? Email { get; set; }

    public static LoginResponseDto ToLoginResponseDto(LoginResponse? fromJson)
    {
        return new LoginResponseDto()
        {
            UserFullname = fromJson?.UserFullname,
            IndividFullname = fromJson?.IndividFullname,
            Email = fromJson?.Email,
        };
    }
}
