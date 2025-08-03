// CSVInfoMarkerImporter.cs
// Menu: Tools ▸ Import ▸ Info Markers CSV
using System;
using System.IO;
using System.Linq;
using System.Text;
using DefaultNamespace;
using Helpers;
using UnityEditor;
using UnityEngine;

public static class CSVInfoMarkerImporter
{
    [MenuItem("Tools/Import/Info Markers CSV")]
    private static void ImportInfoMarkers()
    {
        string csvPath = EditorUtility.OpenFilePanel("Select info_markers.csv", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(csvPath)) return;

        string targetDir = "Assets/Markers/Info";
        Directory.CreateDirectory(targetDir);

        foreach (var line in File.ReadAllLines(csvPath, Encoding.UTF8).Skip(1)) // skip header
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = ParseCsvLine(line);
            if (parts.Length < 7)
            {
                Debug.LogError($"Bad CSV row → {line}");
                continue;
            }

            // Columns → id,hour,minute,channel,sender,devNotes,message
            string idStr     = parts[0].Trim().Trim('"');
            int    hour      = int.Parse(parts[1].Trim().Trim('"'));
            int    minute    = int.Parse(parts[2].Trim().Trim('"'));
            string channel   = parts[3].Trim().Trim('"').ToUpperInvariant();
            string sender    = parts[4].Trim().Trim('"');
            string devNotes  = parts[5].Trim().Trim('"');
            string msg       = parts[6].Trim().Trim('"');

            // ✔ NOTE: InfoMarker must inherit ScriptableObject
            var marker = ScriptableObject.CreateInstance<InfoMarker>();
            marker.Message   = msg;
            marker.Timestamp = new GameTime(hour, minute);
            marker.DevNotes  = devNotes;
            marker.Feature   = Enum.TryParse(channel, out Channel ch) ? ch : Channel.CHAT;
            marker.SenderPerson      = null;              // hook to Person asset if desired

            string assetPath = $"{targetDir}/{idStr}.asset";
            AssetDatabase.CreateAsset(marker, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[CSVInfoMarkerImporter] InfoMarkers imported!");
    }

    /// Splits a CSV line while respecting quotes.
    private static string[] ParseCsvLine(string line)
    {
        var list = new System.Collections.Generic.List<string>();
        var sb   = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '\"')
            {
                inQuotes = !inQuotes;
                continue;
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
        list.Add(sb.ToString());
        return list.ToArray();
    }
}
