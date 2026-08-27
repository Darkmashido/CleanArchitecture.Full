using MediatR;

namespace CleanArchitecture.Full.Application.Clients.Queries.GetAllClients;

public record GetAllClientsQuery : IRequest<IReadOnlyList<ClientDto>>;