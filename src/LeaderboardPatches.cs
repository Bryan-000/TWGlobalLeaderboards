namespace TWGlobalLeaderboards;

using HarmonyLib;
using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

[HarmonyPatch]
public static class LeaderboardPatches
{
    [HarmonyPrefix] [HarmonyPatch(typeof(ISteamUserStats), "DownloadLeaderboardEntries")]
    public static void EditDataRequest(ref LeaderboardDataRequest eLeaderboardDataRequest, ref int nRangeStart, ref int nRangeEnd)
    {
        eLeaderboardDataRequest = PrefsManager.Get("GlobalLeaderboards.mode", LeaderboardDataRequest.GlobalAroundUser);

        switch (eLeaderboardDataRequest)
        {
            case LeaderboardDataRequest.Global:
                nRangeStart = 0;
                nRangeEnd = 10;
                break;

            case LeaderboardDataRequest.GlobalAroundUser:
                nRangeStart = -5;
                nRangeEnd = 5;
                break;
        }
    }

    #region thing for adding the global rank num b4 the name

    [HarmonyPatch]
    public static class IHateMyself
    {
        // so pretty much, async methods make like an async state machine class thing but its name has characters that you
        // cant put in a type (unless ur the compiler), so when i try typing it, roslyn gets angi
        // and this pretty much gets the type for the async state machine without explicitly writing it so roslyn stays happy :D
        public static MethodBase TargetMethod()
        {
            // GetThisFUckingBULLSHTITT
            MethodInfo outer = AccessTools.Method(typeof(LeaderboardsFetcher), "GetLeaderboards");
            var attr = outer.GetCustomAttribute<AsyncStateMachineAttribute>();

            Type Get_Leaderboardsd__10 = attr.StateMachineType;
            return AccessTools.Method(Get_Leaderboardsd__10, "MoveNext");
        }

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo string_Concat = AccessTools.Method(typeof(string), "Concat", [typeof(string), typeof(string), typeof(string)]);
            FieldInfo Entry_GlobalRank = AccessTools.Field(typeof(LeaderboardEntry), "GlobalRank");
            MethodInfo Friend_getName = AccessTools.PropertyGetter(typeof(Friend), "Name");
            MethodInfo int_ToString = AccessTools.Method(typeof(int), "ToString", []);

            foreach (CodeInstruction instruction in instructions)
            {
                // string text4 = item.GlobalRank + ". " + user.Name;
                if (instruction.Calls(Friend_getName))
                {
                    // remove `user` from the evaluation stack
                    yield return new(OpCodes.Pop);

                    // item.GlobalRank.ToString()
                    yield return new(OpCodes.Ldloca_S, 5); // item
                    yield return new(OpCodes.Ldflda, Entry_GlobalRank); // .GlobalRank
                    yield return new(OpCodes.Call, int_ToString); // .ToString()

                    // ". "
                    yield return new(OpCodes.Ldstr, ". ");

                    // user.Name
                    yield return new(OpCodes.Ldloca_S, 10); // user
                    yield return instruction; // .Name

                    // adds them together
                    yield return new(OpCodes.Call, string_Concat);

                    // then the next instruction sets `string text4` so we dont do anything :3
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }

    #endregion
}