﻿using SteamWebAPI2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebAPI2
{
    public class SteamUser : SteamWebInterface
    {
        public SteamUser(string steamWebApiKey)
            : base(steamWebApiKey, "ISteamUser") { }

        public async Task<PlayerSummary> GetPlayerSummaryAsync(long steamId)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            AddToParametersIfHasValue("steamids", steamId, parameters);
            var playerSummary = await CallMethodAsync<PlayerSummaryResponseContainer>("GetPlayerSummaries", 2, parameters);

            if (playerSummary.Response.Players.Count > 0)
            {
                return playerSummary.Response.Players[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<IReadOnlyCollection<Friend>> GetFriendsListAsync(long steamId, string relationship = "")
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            AddToParametersIfHasValue("steamid", steamId, parameters);
            AddToParametersIfHasValue("relationship", relationship, parameters);
            var friendsListResult = await CallMethodAsync<FriendsListResultContainer>("GetFriendList", 1, parameters);
            return new ReadOnlyCollection<Friend>(friendsListResult.Result.Friends);
        }

        public async Task<IReadOnlyCollection<PlayerBans>> GetPlayerBansAsync(long steamId)
        {
            return await GetPlayerBansAsync(new List<long>() { steamId });
        }

        public async Task<IReadOnlyCollection<PlayerBans>> GetPlayerBansAsync(IReadOnlyCollection<long> steamIds)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();

            string steamIdsParamValue = String.Join(",", steamIds);
            
            AddToParametersIfHasValue("steamids", steamIdsParamValue, parameters);

            var playerBansContainer = await CallMethodAsync<PlayerBansContainer>("GetPlayerBans", 1, parameters);
            return new ReadOnlyCollection<PlayerBans>(playerBansContainer.PlayerBans);
        }

        public async Task<UserGroupListResult> GetUserGroupsAsync(long steamId)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            
            AddToParametersIfHasValue("steamid", steamId, parameters);

            var userGroupResultContainer = await CallMethodAsync<UserGroupListResultContainer>("GetUserGroupList", 1, parameters);
            return userGroupResultContainer.Result;
        }
    }
}