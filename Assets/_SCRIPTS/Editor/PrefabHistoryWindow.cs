using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PrefabHistoryWindow : EditorWindow
{
    private Vector2 scrollPos;
    private List<PrefabInfo> prefabInfos = new List<PrefabInfo>();

    [MenuItem("Window/Prefab History")]
    public static void ShowWindow()
    {
        GetWindow<PrefabHistoryWindow>("Prefab History");
    }

    void OnEnable()
    {
        RefreshList();
    }

    void OnFocus()
    {
        RefreshList();
    }

    void RefreshList()
    {
        prefabInfos.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fullPath = Path.Combine(
                Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                path
            );
            DateTime modified = File.GetLastWriteTime(fullPath);
            prefabInfos.Add(new PrefabInfo { Path = path, Modified = modified });
        }
        prefabInfos = prefabInfos.OrderByDescending(pi => pi.Modified).Take(10).ToList();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Refresh"))
            RefreshList();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        foreach (var info in prefabInfos)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select", GUILayout.Width(60)))
            {
                var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(info.Path);
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }
            EditorGUILayout.LabelField(info.Path);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    class PrefabInfo
    {
        public string Path;
        public DateTime Modified;
    }
}