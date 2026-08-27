using CleanArchitecture.Full.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Full.Application.Clients.Queries.GetAllClients
{
    public class GetAllClientsQueryHandler(IClientRepository repository, ILogger<GetAllClientsQueryHandler> logger) : IRequestHandler<GetAllClientsQuery, IReadOnlyList<ClientDto>>
    {
        public async Task<IReadOnlyList<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "Se inicia consulta para obtener todos los clientes");

            var clients = await repository.GetAllAsync(cancellationToken);

            logger.LogInformation(
                "Se han obtenido {ClientCount} clientes ",
                clients.Count());
            return clients.Select(c => c.ToDto()).ToList();
        }
    }
}
