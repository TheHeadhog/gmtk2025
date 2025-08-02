// CSVMarkerImporter.cs
// ⇢ Unity Editor - menu: Tools ▸ Import ▸ Bullshit Markers CSV
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DefaultNamespace;
using Helpers;
using UnityEditor;
using UnityEngine;

public static class CSVMarkerImporter
{
    private const string menuPath = "Tools/Import/Bullshit Markers CSV";

    [MenuItem(menuPath)]
    private static void ImportBullshitMarkers()
    {
        string csvPath = EditorUtility.OpenFilePanel("Select bullshit_markers.csv", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(csvPath)) return;

        string targetDir = "Assets/Markers/Bullshit";
        Directory.CreateDirectory(targetDir);

        int created = 0;

        foreach (var line in File.ReadAllLines(csvPath, Encoding.UTF8).Skip(1)) // skip header
        {
            // id,hour,minute,channel,sender,message
            string[] parts = line.Split(new[] {','}, 6); // 6 = max splits, keeps commas inside message
            if (parts.Length < 6) continue;

            string id      = parts[0].Trim();
            int    hour    = int.Parse(parts[1]);
            int    minute  = int.Parse(parts[2]);
            string channel = parts[3].Trim().ToUpper();
            string sender  = parts[4].Trim();
            string message = parts[5].Trim().Trim('"');  // remove enclosing quotes, if any

            var marker = ScriptableObject.CreateInstance<BullshitMarker>();
            marker.Message   = message;
            marker.Timestamp = new GameTime(hour, minute);
            marker.Feature   = Enum.TryParse(channel, out Channel feat) ? feat : Channel.CHAT; // default
            marker.SenderPerson = null;             // hook up your Person assets here
            marker.ReceivedNewsData = null;
            marker.EmailData        = null;

            string assetPath = $"{targetDir}/{id}.asset";
            AssetDatabase.CreateAsset(marker, assetPath);
            created++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[CSVMarkerImporter] Imported {created} BullshitMarker assets.");
    }
}
