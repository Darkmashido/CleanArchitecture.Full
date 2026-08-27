namespace CleanArchitecture.Full.Domain;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Client client, CancellationToken cancellationToken = default);
    void Update(Client client);
    void Delete(Client client);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
