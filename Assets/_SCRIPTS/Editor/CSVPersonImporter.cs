// CSVPersonImporter.cs
// Menu:  Tools ▶ Import ▶ Persons CSV
using System;
using System.IO;
using System.Linq;
using System.Text;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

public static class CSVPersonImporter
{
    private const string menuPath = "Tools/Import/Persons CSV";
    private const string folder   = "Assets/Persons";

    [MenuItem(menuPath)]
    private static void ImportPersons()
    {
        // Pick CSV file
        string csvPath = EditorUtility.OpenFilePanel("Select persons.csv", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(csvPath)) return;

        Directory.CreateDirectory(folder);

        int created = 0, updated = 0;

        foreach (string line in File.ReadAllLines(csvPath, Encoding.UTF8).Skip(1)) // skip header
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = ParseCsvLine(line);
            if (parts.Length < 3)
            {
                Debug.LogWarning($"[CSVPersonImporter] Bad row → {line}");
                continue;
            }

            string fullName = parts[0].Trim().Trim('"');
            string email    = parts[1].Trim().Trim('"');
            string phone    = parts[2].Trim().Trim('"');

            string safeFile = MakeSafeFileName(fullName);
            string assetPath = $"{folder}/{safeFile}.asset";

            Person person = AssetDatabase.LoadAssetAtPath<Person>(assetPath);
            bool isNew = person == null;

            if (isNew)
            {
                person = ScriptableObject.CreateInstance<Person>();
            }

            person.FullName    = fullName;
            person.Email       = email;
            person.PhoneNumber = phone;

            if (isNew)
            {
                AssetDatabase.CreateAsset(person, assetPath);
                created++;
            }
            else
            {
                EditorUtility.SetDirty(person);
                updated++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[CSVPersonImporter] Import finished → Created {created}, Updated {updated}.");
    }

    /// Split CSV row while honoring quotes.
    private static string[] ParseCsvLine(string line)
    {
        var list = new System.Collections.Generic.List<string>();
        var sb = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '\"') { inQuotes = !inQuotes; continue; }
            if (c == ',' && !inQuotes)
            {
                list.Add(sb.ToString());
                sb.Clear();
            }
            else sb.Append(c);
        }
        list.Add(sb.ToString());
        return list.ToArray();
    }

    private static string MakeSafeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name;
    }
}
