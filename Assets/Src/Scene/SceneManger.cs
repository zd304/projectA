using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneManger
{
    SceneConfig currentSceneConfig = null;

    int curLoadedOrnamentIndex = 0;
    int curLoadedObstacleIndex = 0;

    public void Load(string scnName)
    {
        string sceneDirectory = "Scene/" + scnName;
        string configPath = string.Format("{0}/{1}", sceneDirectory, "config");
        currentSceneConfig = Resources.Load<SceneConfig>(configPath);
        if (currentSceneConfig == null)
        {
            return;
        }
    }

    public void UpdateLoading(Vector2 min, Vector2 max, Transform ornamentRoot, Transform obstacleRoot)
    {
        if (currentSceneConfig == null)
        {
            return;
        }
        UpdateLoadingObj(currentSceneConfig.ornaments, ornamentRoot, min, max, ref curLoadedOrnamentIndex);
        UpdateLoadingObj(currentSceneConfig.obstacles, obstacleRoot, min, max, ref curLoadedObstacleIndex);
    }

    void UpdateLoadingObj(List<SpriteObject> list, Transform root, Vector2 min, Vector2 max, ref int index)
    {
        if (index < list.Count)
        {
            var obj = list[index];
            if (obj.min.x <= max.x)
            {
                int prefixLen = "Assets/Resources/".Length;

                string resPath = obj.path.Substring(prefixLen);
                int dot = resPath.LastIndexOf('.');
                if (dot > 0)
                {
                    resPath = resPath.Substring(0, dot);
                }

                GameObject go = Resources.Load<GameObject>(resPath);
                go = GameObject.Instantiate(go);

                go.transform.parent = root.transform;
                ++index;
            }
        }
    }

    public void Unload()
    {
        // Resources.UnloadAsset(currentSceneConfig);
        // UnityEngine.Object.Destroy(currentSceneConfig);
        currentSceneConfig = null;
        curLoadedOrnamentIndex = 0;
        curLoadedObstacleIndex = 0;
    }
}