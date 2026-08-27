namespace CleanArchitecture.Full.Domain;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string DocumentType { get; set; } = "DNI";
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }

    // Navigation property
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
