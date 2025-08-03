// LinkMarkersToPersons.cs
// Menu: Tools ▸ Link ▸ Markers → Persons
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using Helpers;
using UnityEditor;
using UnityEngine;

public static class LinkMarkersToPersons
{
    private const string PersonsFolder  = "Assets/Markers/Persons";
    private const string BullshitFolder = "Assets/Markers/Bullshit";
    private const string InfoFolder     = "Assets/Markers/Info";
    private const string CheckFolder    = "Assets/Markers/Check";

    [MenuItem("Tools/Link/Markers → Persons")]
    private static void LinkAllMarkers()
    {
        // 1️⃣  cache all Person assets by FullName
        var personByName = new Dictionary<string, Person>();
        foreach (string guid in AssetDatabase.FindAssets("t:Person", new[] { PersonsFolder }))
        {
            var p = AssetDatabase.LoadAssetAtPath<Person>(AssetDatabase.GUIDToAssetPath(guid));
            if (p != null && !personByName.ContainsKey(p.FullName))
                personByName.Add(p.FullName, p);
        }

        // 2️⃣  event-ID ⟶ FullName mapping (hand-typed once)
        var roster = new Dictionary<string, string>
        {
            // Bullshit (B1-B80)
            ["B1"]="John (QA)",["B2"]="MemesToday",["B3"]="Lisa (Marketing)",["B4"]="Raj (SysAdmin)",
            ["B5"]="StockWatch",["B6"]="HR_Bot",["B7"]="Zoe (Designer)",["B8"]="CoffeeNow",
            ["B9"]="SpamMailer3000",["B10"]="SnackVendorBot",["B11"]="WeatherVR",["B12"]="CEO Pixel",
            ["B13"]="John (QA)",["B14"]="GamerRumors",["B15"]="FinanceFred",["B16"]="Lisa (Marketing)",
            ["B17"]="TechCrunchy",["B18"]="CoffeeMachineAI",["B19"]="Zoe (Designer)",["B20"]="MemesToday",
            ["B21"]="John (QA)",["B22"]="Raj (SysAdmin)",["B23"]="GameBiz Daily",["B24"]="HR_Bot",
            ["B25"]="SnackVendorBot",["B26"]="CoffeeNow",["B27"]="Lisa (Marketing)",["B28"]="CEO Pixel",
            ["B29"]="WeatherVR",["B30"]="MemeIntern Mia",["B31"]="John (QA)",["B32"]="GameBiz Daily",
            ["B33"]="FinanceFred",["B34"]="Raj (SysAdmin)",["B35"]="TechCrunchy",["B36"]="Zoe (Designer)",
            ["B37"]="SnackVendorBot",["B38"]="MemesToday",["B39"]="HR_Bot",["B40"]="Lisa (Marketing)",
            ["B41"]="CoffeeNow",["B42"]="John (QA)",["B43"]="Zoe (Designer)",["B44"]="StockWatch",
            ["B45"]="CEO Pixel",["B46"]="Raj (SysAdmin)",["B47"]="GamerRumors",["B48"]="CoffeeMachineAI",
            ["B49"]="SnackVendorBot",["B50"]="WeatherVR",["B51"]="MemeIntern Mia",["B52"]="Lisa (Marketing)",
            ["B53"]="TechCrunchy",["B54"]="FinanceFred",["B55"]="John (QA)",["B56"]="MemesToday",
            ["B57"]="HR_Bot",["B58"]="Raj (SysAdmin)",["B59"]="CoffeeNow",["B60"]="Zoe (Designer)",
            ["B61"]="SnackVendorBot",["B62"]="GamerRumors",["B63"]="CEO Pixel",["B64"]="John (QA)",
            ["B65"]="WeatherVR",["B66"]="Lisa (Marketing)",["B67"]="Raj (SysAdmin)",["B68"]="TechCrunchy",
            ["B69"]="FinanceFred",["B70"]="Zoe (Designer)",["B71"]="MemesToday",["B72"]="HR_Bot",
            ["B73"]="SnackVendorBot",["B74"]="CoffeeNow",["B75"]="John (QA)",["B76"]="Lisa (Marketing)",
            ["B77"]="GameBiz Daily",["B78"]="Raj (SysAdmin)",["B79"]="Zoe (Designer)",["B80"]="WeatherVR",

            // Info (I1-I10)
            ["I1"]="Raj (SysAdmin)", ["I2"]="GameBiz Daily",  ["I3"]="Zoe (Designer)",
            ["I4"]="John (QA)",     ["I5"]="Traffic Watch", ["I6"]="Mr Chips (Client)",
            ["I7"]="Lisa (Marketing)",["I8"]="Raj (SysAdmin)",["I9"]="CoffeeNow",
            ["I10"]="CEO Pixel",

            // Check (C1-C10)
            ["C1"]="CEO Pixel",["C2"]="Lisa (Marketing)",["C3"]="Mr BigBucks",["C4"]="Mr Chips (Client)",
            ["C5"]="John (QA)",["C6"]="Zoe (Recruiter)",["C7"]="SnackVendorBot",["C8"]="Raj (SysAdmin)",
            ["C9"]="CEO Pixel",["C10"]="Legal Bot",
        };

        int linked = 0, missingPerson = 0, missingMarker = 0;

        foreach (var kvp in roster)
        {
            string id = kvp.Key;
            string fullName = kvp.Value;

            // find Person reference
            if (!personByName.TryGetValue(fullName, out Person person))
            {
                Debug.LogWarning($"[Link] Missing Person asset for '{fullName}'.");
                missingPerson++;
                continue;
            }

            // resolve marker asset path (try all three folders)
            string path = $"{BullshitFolder}/{id}.asset";
            UnityEngine.Object marker = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (marker == null)
            {
                path = $"{InfoFolder}/{id}.asset";
                marker = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            }
            if (marker == null)
            {
                path = $"{CheckFolder}/{id}.asset";
                marker = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            }
            if (marker == null)
            {
                Debug.LogWarning($"[Link] No marker asset found for ID '{id}'.");
                missingMarker++;
                continue;
            }

            // reflection-safe assignment (all three marker types have SenderPerson)
            var senderField = marker.GetType().GetField("SenderPerson");
            if (senderField != null)
            {
                senderField.SetValue(marker, person);
                EditorUtility.SetDirty(marker);
                linked++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"[LinkMarkersToPersons] Linked {linked} markers. Missing Persons: {missingPerson}, Missing Markers: {missingMarker}.");
    }
}
