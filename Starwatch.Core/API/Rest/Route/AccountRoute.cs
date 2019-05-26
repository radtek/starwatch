﻿using System;
using Starwatch.API.Rest.Routing;
using Starwatch.API.Rest.Route.Entities;
using Starwatch.API.Web;

namespace Starwatch.API.Rest.Route
{
    [Route("/account", AuthLevel.Bot)]
    class AccountRoute : RestRoute
    {
        public override Type PayloadType => typeof(AccountPatch);
        public AccountRoute(RestHandler handler, Authentication authentication) : base(handler, authentication) { }

        /// <summary>
        /// Creates an account.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="payloadObject"></param>
        /// <returns></returns>
        public override RestResponse OnPost(Query query, object payloadObject)
        {
#if !SKIP_SSL_ENFORCE
            //Server must be in SSL mode
            if (!Handler.ApiHandler.IsSecure)
                return new RestResponse(RestStatus.Forbidden, msg: "Cannot set passwords if the server is not SSL");
#endif

            //Validate the account
            AccountPatch account = (AccountPatch)payloadObject;
            if (string.IsNullOrWhiteSpace(account.Name)) return new RestResponse(RestStatus.BadRequest, msg: "Account name cannot be empty");
            if (string.IsNullOrWhiteSpace(account.Password)) return new RestResponse(RestStatus.BadRequest, msg: "Password cannot be empty");

            //Only SuperBot and SuperUsers can create admin
            if (account.IsAdmin.GetValueOrDefault(false) && AuthenticationLevel < AuthLevel.SuperBot)
                return new RestResponse(RestStatus.Forbidden, "Only SuperBot or above may create admin accounts.");

            //Make sure the name isnt a duplicate
            if (Starbound.Settings.Accounts.GetAccount(account.Name) != null)
                return new RestResponse(RestStatus.BadRequest, $"The username {account.Name} already exists.");
            
            //Post it
            Starbound.Settings.Accounts.AddAccount(account.ToAccount());

            //Save the settings
            var task = Starbound.SaveSettings(true);
            if (query.GetBool(Query.AsyncKey, false)) return RestResponse.Async;
            return new RestResponse(RestStatus.OK, msg: $"Saved: {task.Result}", res: account);
        }
    }
}