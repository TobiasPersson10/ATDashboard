namespace ATDashboard.Models.DTO;

public record LoginResponseDTO
{
    public string? UserFullname { get; set; }
    public string? IndividFullname { get; set; }
    public string? Email { get; set; }

    public static LoginResponseDTO ToDTO(LoginResponse? fromJson)
    {
        return new LoginResponseDTO()
        {
            UserFullname = fromJson?.UserFullname,
            IndividFullname = fromJson?.IndividFullname,
            Email = fromJson?.Email,
        };
    }
}
