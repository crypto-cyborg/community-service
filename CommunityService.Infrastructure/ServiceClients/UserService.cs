﻿using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CommunityService.Infrastructure.ServiceClients;

public class UserService(Users.UsersClient client)
{
    public async Task<ExistsReply> Exists(string idOrSlug)
    {
        var req = new ExistsRequest { IdOrSlug = idOrSlug };
        
        var res = await client.FindByIdOrSlugAsync(req);
        
        return res;
    }
}