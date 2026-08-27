using CleanArchitecture.Full.Application.Accounts.Commands.CreateAccount;
using CleanArchitecture.Full.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace CleanArchitecture.Full.Application.Accounts.Queries.GetAllAccounts;

public class GetAllAccountsQueryHandler(IAccountRepository repository, ILogger<GetAllAccountsQueryHandler> logger) : IRequestHandler<GetAllAccountsQuery, IReadOnlyList<AccountDto>>
{
    public async Task<IReadOnlyList<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Se inicia consulta para obtener todas las cuentas");

        var accounts = await repository.GetAllAsync(cancellationToken);

        logger.LogInformation(
            "Se han obtenido {AccountCount} cuentas",
            accounts.Count());
        return accounts.Select(a => a.ToDto()).ToList();
    }
}
