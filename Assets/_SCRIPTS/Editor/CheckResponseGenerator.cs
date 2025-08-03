// CheckResponseGenerator.cs
// Menu: Tools ▶ Generate ▶ Default Check Responses
using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using Helpers;
using UnityEditor;
using UnityEngine;

public static class CheckResponseGenerator
{
    private const string CheckFolder = "Assets/Markers/Check";

    // quick helper to build GameTime
    private static GameTime GT(int h, int m) => new GameTime(h, m);

    [MenuItem("Tools/Generate/Default Check Responses")]
    private static void GenerateResponses()
    {
        var rules = BuildRuleBook();   // dictionary ID -> tuple data
        int updated = 0, missing = 0;

        foreach (string guid in AssetDatabase.FindAssets("t:CheckMarker", new[] { CheckFolder }))
        {
            var marker = AssetDatabase.LoadAssetAtPath<CheckMarker>(AssetDatabase.GUIDToAssetPath(guid));
            if (marker == null) continue;

            if (!rules.TryGetValue(marker.Id.ToString(), out Rule r))
            {
                Debug.LogWarning($"[CheckResponseGenerator] No rule for {marker.name}");
                missing++;
                continue;
            }

            marker.GoodResponse = r.good;
            var list = new List<CheckResponse>();

            list.Add(new CheckResponse(r.badEarly,  -5)
            {
                BeginTimestampInclusive = r.earlyFrom,
                EndTimestampExclusive   = r.earlyTo
            });
            list.Add(new CheckResponse(r.badMiss,  -15)
            {
                BeginTimestampInclusive = r.missFrom,
                EndTimestampExclusive   = r.missTo
            });
            list.Add(new CheckResponse(r.badLate,  -5)
            {
                BeginTimestampInclusive = r.lateFrom,
                EndTimestampExclusive   = r.lateTo
            });

            marker.Responses = list;
            EditorUtility.SetDirty(marker);
            updated++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"[CheckResponseGenerator] Updated {updated} markers, missing {missing}.");
    }

    // ───────────────── helpers ─────────────────

    private struct Rule
    {
        public string good, badEarly, badMiss, badLate;
        public GameTime earlyFrom, earlyTo, missFrom, missTo, lateFrom, lateTo;
    }

    private static Dictionary<string, Rule> BuildRuleBook()
    {
        // day boundaries for convenience
        GameTime dayStart = GT(9, 0);
        GameTime dayEnd   = GT(17, 0);

        // easier typing alias
        Rule R(string good, string be, string bm, string bl,
               GameTime ef, GameTime et, GameTime mf, GameTime mt, GameTime lf, GameTime lt)
            => new Rule { good=good, badEarly=be, badMiss=bm, badLate=bl,
                          earlyFrom=ef, earlyTo=et, missFrom=mf, missTo=mt, lateFrom=lf, lateTo=lt };

        // rule book keyed by enum name (C1..C10)
        var d = new Dictionary<string, Rule>
        {
            ["C1"] = R("Dev-sync right on time – caffeine keeps us alive!",
                       "Why did you book the sync so early? Build still compiling …",
                       "Where’s my dev-sync? Nobody showed up!",
                       "Daily sync after stand-up? Too late, team’s gone.",
                       dayStart, GT(9,15), GT(9,15), GT(9,25), GT(9,35), dayEnd),

            ["C2"] = R("Perfect slot – the naming storm begins!",
                       "Booked before coffee? No ideas come before caffeine.",
                       "No brainstorm when the bell rang – marketing riots!",
                       "Brainstorm after 11? Too late, trend’s already dead.",
                       dayStart, GT(10,30), GT(10,30), GT(10,40), GT(11,0), dayEnd),

            ["C3"] = R("Call started exactly when the jet landed – investors impressed.",
                       "Too early – investors still airborne.",
                       "Investors called: ‘Where’s the link?’ Nothing was scheduled!",
                       "Dialling them after lunch? They’ve left for golf.",
                       dayStart, GT(12,10), GT(12,10), GT(12,20), GT(12,30), dayEnd),

            ["C4"] = R("Client demo right on cue – nothing crashed!",
                       "Booked at 13 but traffic jam! Reschedule, please.",
                       "No meeting when they arrived – awkward lobby silence.",
                       "Demo at 15? They already caught a flight.",
                       GT(12,0), GT(13,0), GT(14,0), GT(14,10), GT(14,30), dayEnd),

            ["C5"] = R("Bug-crunch started on time – optimism intact.",
                       "Scheduled before 14? Half the bugs weren’t logged yet.",
                       "QA room empty at slot – testers cry in binary.",
                       "After 15? Too late, another 100 bugs spawned.",
                       dayStart, GT(14,20), GT(14,20), GT(14,30), GT(14,50), dayEnd),

            ["C6"] = R("Great timing – neon pink officially approved.",
                       "Booked right after lunch coma – designer asleep.",
                       "No review at the slot – colour chaos reigns!",
                       "After 15:30? Too late, build locked.",
                       dayStart, GT(15,0), GT(15,0), GT(15,10), GT(15,20), dayEnd),

            ["C7"] = R("Snacks tested, CEO satisfied, morale +50!",
                       "Too early, vendor not here yet – no snacks, sad devs.",
                       "Snack slot missed – vending machine uprising!",
                       "Tasting at 16? Everyone’s full, snacks stale.",
                       dayStart, GT(15,40), GT(15,40), GT(15,50), GT(16,0), dayEnd),

            ["C8"] = R("Audit on schedule – ‘password’ finally banned.",
                       "Too early, firewall update still running …",
                       "No audit when planned – hackers cheered.",
                       "Audit after 16:30? SysAdmin went home.",
                       dayStart, GT(15,50), GT(16,10), GT(16,20), GT(16,40), dayEnd),

            ["C9"] = R("Day wrapped – CEO rage-quit avoided.",
                       "Debrief at 15? Too early, chaos still unfolding.",
                       "No debrief – CEO furious, desk flipped.",
                       "After 17? Too late, lights off.",
                       dayStart, GT(16,40), GT(16,40), GT(16,50), GT(17,0), dayEnd),

            ["C10"]= R("T&Cs fixed – loot-boxes now ‘surprise mechanics’.",
                       "Legal before 12? Regulators not awake yet.",
                       "No meeting – we’re legally doomed!",
                       "After 13? Store already live, too late.",
                       dayStart, GT(12,20), GT(12,20), GT(12,30), GT(12,40), dayEnd),
        };
        return d;
    }
}
