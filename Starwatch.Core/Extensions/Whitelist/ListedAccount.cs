﻿using Starwatch.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Starwatch.Extensions.Whitelist
{
    public class ListedAccount : IRecord
    {
        public string Table => "!protections_accounts";

        public long ProtectionId { get; set; }
        public string AccountName { get; set; }
        public string Reason { get; set; }

        private ListedAccount() { }
        public ListedAccount(ProtectedWorld protection, string account)
        {
            ProtectionId = protection.Id;
            AccountName = account;
        }

        /// <summary>
        /// Deletes all listed accounts for a protection id
        /// </summary>
        /// <param name="db"></param>
        /// <param name="protectionId"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteAllAsync(DbContext db, long protectionId)
        {
            return await db.DeleteAsync("!protections_accounts", new Dictionary<string, object>() { { "protection", protectionId } }); 
        }

        /// <summary>
        /// Deletes the account
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(DbContext db)
        {
            return await db.DeleteAsync(Table, new Dictionary<string, object>()
            {
                { "protection", ProtectionId },
                { "account", AccountName }
            });
        }

        /// <summary>
        /// Loads the account
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<bool> LoadAsync(DbContext db)
        {
            var result = await db.SelectOneAsync<ListedAccount>(Table, (reader) =>
            {
                return new ListedAccount()
                {
                    ProtectionId = reader.GetInt64("protection"),
                    AccountName = reader.GetString("account"),
                    Reason = reader.GetString("reason")
                };
            }, new Dictionary<string, object>()
            {
                { "protection", ProtectionId },
                { "account", AccountName },
            });

            if (result == null)
                return false;

            this.ProtectionId = result.ProtectionId;
            this.AccountName = result.AccountName;
            this.Reason = result.Reason;
            return true;
        }

        /// <summary>
        /// Saves the account
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(DbContext db)
        {
            var insertedID = await db.InsertUpdateAsync(Table, new Dictionary<string, object>()
            {
                { "protection", ProtectionId },
                { "account", AccountName },
                { "reason", Reason }
            });

            return insertedID != 0;
        }
    }
}