namespace ATDashboard.Models;

public record LoginResponse
{
    public string PassWordExpired { get; set; }
    public string IsPoaUser { get; set; }
    public string IsInkopsStrategyAdmin { get; set; }
    public string? UserFullname { get; set; }
    public string? IndividFullname { get; set; }
    public string CustomerType { get; set; }
    public string ErrNumber { get; set; }
    public string ErrDescription { get; set; }
    public string? Dst { get; set; }
    public string UserId { get; set; }
    public string? Email { get; set; }
    public string LoginBankID { get; set; }
}
