﻿using Newtonsoft.Json;
using Starwatch.Starbound;
using System.Threading.Tasks;

namespace Starwatch.Entities
{
    public class Player
    {
        [JsonIgnore]
        public Server Server { get; private set; }

        public int Connection { get; private set; }
        public string Username { get; internal set; }
        public string Nickname { get; internal set; }
        public string AccountName { get; internal set; }
        public string UUID { get; internal set; }
        public string IP { get; internal set; }
        public bool IsAdmin { get { return (Server?.Settings?.Accounts.GetAccount(AccountName)?.IsAdmin).GetValueOrDefault(false); } }

        [JsonIgnore] public World Location { get; internal set; }
        [JsonProperty("Location")] private string LocationSerialize { get => Location?.Whereami; set { Location = World.Parse(value); } }

        public Player(Server server, int cid)
        {
            this.Server = server;
            this.Connection = cid;
        }

        /// <summary>
        /// Bans the player from the server, automatically reloading the server and kicking the palyer.
        /// </summary>  
        /// <param name="reason">The reason of the ban. This is formatted automatically. {ticket} will be replaced with the Ticket and {moderator} will be replaced with the moderator who added the ban.</param>
        /// <param name="moderator">The author of the ban</param>
        /// <returns></returns>
        public async Task<long> Ban(string reason, string moderator) => await Server.Ban(this, reason, moderator, true, true);
        

        /// <summary>
        /// Kicks the user from the server. Only available if the server has RCON enabled.
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task<Starbound.Rcon.RconResponse> Kick(string reason) => await Server.Kick(this, reason);

        /// <summary>
        /// Gets the account linked to this player.
        /// </summary>
        /// <returns></returns>
        public Account GetAccount() => Server.Settings.Accounts.GetAccount(this.AccountName);
        
    }
}