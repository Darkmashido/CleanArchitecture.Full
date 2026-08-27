namespace CleanArchitecture.Full.Application.Clients;

public record ClientDto(
    Guid Id,
    string Name,
    string Email,
    string PhoneNumber,
    string DocumentNumber,
    string DocumentType,
    DateTime CreatedAt,
    DateTime? LastModifiedAt);
