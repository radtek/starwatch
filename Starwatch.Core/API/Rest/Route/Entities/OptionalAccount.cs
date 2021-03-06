/*
START LICENSE DISCLAIMER
Starwatch is a Starbound Server manager with player management, crash recovery and a REST and websocket (live) API. 
Copyright(C) 2020 Lachee

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program. If not, see < https://www.gnu.org/licenses/ >.
END LICENSE DISCLAIMER
*/
using Newtonsoft.Json;
using Starwatch.Entities;
using System;

namespace Starwatch.API.Rest.Route.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public struct AccountPatch
    {
        public string Name { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsActive { get; set; }
        public string Password { get; set; }

        public AccountPatch(Account account)
        {
            Name = account.Name;
            IsAdmin = account.IsAdmin;
            Password = account.Password;
            IsActive = account.IsActive;
        }

        public Account ToAccount()
        {
            return new Account(Name)
            {
                IsAdmin = IsAdmin.GetValueOrDefault(false),
                IsActive = IsActive.GetValueOrDefault(true),
                Password = Password
            };
        }
    }
}
