using CleanArchitecture.Full.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Full.Application.Clients
{
    public static class ClientMappingExtensions
    {
        public static ClientDto ToDto(this Client client) =>
        new(
            client.Id,
            client.Name,
            client.Email,
            client.PhoneNumber,
            client.DocumentNumber,
            client.DocumentType,
            client.CreatedAt,
            client.LastModifiedAt);
    }
}
