using ATDashboard.Models.SkeKraftModels;

namespace ATDashboard.Models.DTO;

public class InvoiceDto
{
    public string? Amount { get; set; }
    public string? Balance { get; set; }
    public string? ExpiryDate { get; set; }
    public string? InvoiceDate { get; set; }
    public string? Address { get; set; }

    public static InvoiceDto ToInvoiceDto(Invoice invoice)
    {
        return new InvoiceDto
        {
            Amount = invoice?.Amount,
            Balance = invoice?.Balance,
            ExpiryDate = invoice?.ExpiryDate,
            InvoiceDate = invoice?.InvoiceDate,
            Address = invoice?.Address,
        };
    }
}
