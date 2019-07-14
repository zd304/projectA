using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneManger
{
    SceneConfig currentSceneConfig = null;

    public void Load(string scnName)
    {
        string sceneDirectory = "Assets/Resources/Scene/" + scnName;
        string configPath = string.Format("{0}/{1}", sceneDirectory, "config.asset");
        currentSceneConfig = Resources.Load<SceneConfig>(configPath);
        if (currentSceneConfig == null)
        {
            return;
        }
    }
}