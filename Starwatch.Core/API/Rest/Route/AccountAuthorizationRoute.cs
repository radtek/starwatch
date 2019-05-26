﻿using System;
using Starwatch.API.Rest.Routing;
using Starwatch.Entities;
using Starwatch.API.Web;

namespace Starwatch.API.Rest.Route
{
    [Route("/account/:account/authorize", AuthLevel.Bot)]
    class AccountAuthorizationRoute : RestRoute
    {
        [Argument("account")]
        public Account Account { get; set; }

        public override Type PayloadType => typeof(Payload);
        public struct Payload
        {
            public string Hash { get; set; }
        }

        public AccountAuthorizationRoute(RestHandler handler, Authentication authentication) : base(handler, authentication) { }

        /// <summary>
        /// Authorizes the account
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override RestResponse OnPost(Query query, object payloadObject)
        {

#if !SKIP_SSL_ENFORCE
            if (!Handler.ApiHandler.IsSecure)
                return new RestResponse(RestStatus.Forbidden, msg: "Server is forbidden to handle passwords unless its using SSL.");
#endif

            Payload payload = (Payload)payloadObject;
            return new RestResponse(RestStatus.OK, BCrypt.Net.BCrypt.Verify(Account.Password, payload.Hash));
        }
    }
}