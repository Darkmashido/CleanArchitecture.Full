using CleanArchitecture.Full.Application.Clients;
using CleanArchitecture.Full.Application.Clients.Queries.GetAllClients;
using MediatR;

namespace CleanArchitecture.Full.Api.Endpoints
{
    public static class ClientEndpoints
    {
        public static void MapClientEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("v1/api/clients").WithTags("Clients (Minimal API)");

            group.MapGet("", async (ISender sender, CancellationToken cancellationToken) =>
                Results.Ok(await sender.Send(new GetAllClientsQuery(), cancellationToken)))
                .Produces<IReadOnlyList<ClientDto>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
