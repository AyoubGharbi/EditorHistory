﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ExportPackage
{
    private static string _packageBasePath = "D:/Workspace/BUILDS/Unity/EditorHistory/";

    public static void Export ()
    {
        string[] projectContent = new string[]
        {
            "Assets/Fonts",
            "Assets/Scenes",
            "Assets/Scripts",
            "Assets/Materials",
            "Assets/Textures",
        };

        var executableName = string.Format ("EditorHistory_{0}.unitypackage", System.DateTime.Now.ToString ("yyyy-MM-dd"));
        var executablePath = string.Format ("{0}{1}", _packageBasePath, executableName);

        AssetDatabase.ExportPackage (projectContent, executablePath,ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
    }
}