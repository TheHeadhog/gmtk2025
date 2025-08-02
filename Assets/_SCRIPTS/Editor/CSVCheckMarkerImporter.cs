// CSVCheckMarkerImporter.cs
// Menu: Tools ▸ Import ▸ Check Markers CSV
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DefaultNamespace;
using Helpers;
using UnityEditor;
using UnityEngine;

public static class CSVCheckMarkerImporter
{
    [MenuItem("Tools/Import/Check Markers CSV")]
    private static void ImportCheckMarkers()
    {
        string csvPath = EditorUtility.OpenFilePanel("Select check_markers.csv", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(csvPath)) return;

        string targetDir = "Assets/Markers/Check";
        Directory.CreateDirectory(targetDir);

        int created = 0;

        foreach (var line in File.ReadAllLines(csvPath, Encoding.UTF8).Skip(1)) // skip header
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = ParseCsvLine(line);
            if (parts.Length < 9)
            {
                Debug.LogError($"Bad CSV row → {line}");
                continue;
            }

            string idStr        = parts[0].Trim().Trim('"');
            int    hour         = int.Parse(parts[1].Trim().Trim('"'));
            int    minute       = int.Parse(parts[2].Trim().Trim('"'));
            int    duration     = int.Parse(parts[3].Trim().Trim('"'));
            string channelStr   = parts[4].Trim().Trim('"').ToUpperInvariant();
            string senderName   = parts[5].Trim().Trim('"');
            string tile         = parts[6].Trim().Trim('"');
            string msg          = parts[7].Trim().Trim('"');
            int    points       = int.Parse(parts[8].Trim().Trim('"'));

            var marker = ScriptableObject.CreateInstance<CheckMarker>();

            // Try map string to your enum, else fall back
            if (Enum.TryParse(idStr, out CheckMarkerId enumId))
                marker.Id = enumId;
            else
                marker.Id = CheckMarkerId.C1; // fallback / edit as needed

            marker.Timestamp         = new GameTime(hour, minute);
            marker.DurationInMinutes = duration;
            marker.Feature           = Enum.TryParse(channelStr, out Channel ch) ? ch : Channel.EMAIL;
            marker.Points            = points;
            marker.Message           = msg;
            marker.TextOnCalendarTile= tile;
            marker.Responses         = new List<CheckResponse>(); // populate later as desired
            marker.SenderPerson      = null; // link real Person assets if you have them

            string assetPath = $"{targetDir}/{idStr}.asset";
            AssetDatabase.CreateAsset(marker, assetPath);
            created++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[CSVCheckMarkerImporter] Imported {created} CheckMarker assets.");
    }
    
    private static string[] ParseCsvLine(string line)
    {
        var list = new List<string>();
        var sb   = new System.Text.StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '\"')
            {
                inQuotes = !inQuotes;
                continue;                     // strip the quote itself
            }

            if (c == ',' && !inQuotes)
            {
                list.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(c);
            }
        }
        list.Add(sb.ToString());              // last field
        return list.ToArray();
    }

}
