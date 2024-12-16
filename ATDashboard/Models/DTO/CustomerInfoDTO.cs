using ATDashboard.Models.SkeKraftModels;

namespace ATDashboard.Models.DTO;

public class CustomerInfoDto
{
    public string? Country { get; set; }
    public string CountryCode { get; set; }
    public string PoaRights { get; set; }
    public string Source { get; set; }
    public string CustomerName { get; set; }
    public string CustomerId { get; set; }
    public string Address { get; set; }
    public string? CoAddress { get; set; }
    public string ZipCode { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; }
    public string Paymode { get; set; }
    public string PerOrgNr { get; set; }
    public string Type { get; set; }
    public string IsPoa { get; set; }

    public static CustomerInfoDto ToCustomerInfoDto(CustomerInfoResponse? response)
    {
        var customerInfo = response?.CustomerInfo;
        return new CustomerInfoDto
        {
            Country = customerInfo?.Country,
            CountryCode = customerInfo?.CountryCode,
            PoaRights = customerInfo.PoaRights,
            Source = customerInfo.Source,
            CustomerName = customerInfo.CustomerName,
            CustomerId = customerInfo.CustomerId,
            Address = customerInfo.Address,
            CoAddress = customerInfo.CoAddress,
            ZipCode = customerInfo.ZipCode,
            Phone = customerInfo.Phone,
            Email = customerInfo.Email,
            Paymode = customerInfo.Paymode,
            PerOrgNr = customerInfo.PerOrgNr,
            Type = customerInfo.Type,
            IsPoa = customerInfo.IsPoa,
        };
    }
}
