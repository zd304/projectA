using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;

public class GenerateScene
{
    static string sceneDirectory = string.Empty;

    [MenuItem("Tools/场景/生成场景")]
    static void Generate()
    {
        GameObject sceneRoot = GameObject.FindGameObjectWithTag("SceneRoot");
        if (!sceneRoot)
        {
            Debug.LogError("该场景没有场景根节点");
            return;
        }

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene == null)
        {
            return;
        }
        sceneDirectory = "Assets/Resources/Scene/" + currentScene.name;
        if (!Directory.Exists(sceneDirectory))
        {
            Directory.CreateDirectory(sceneDirectory);
        }

        SceneConfig config = ScriptableObject.CreateInstance<SceneConfig>();

        GenerateOrnaments(sceneRoot.transform, config);
        GenerateObstcle(sceneRoot.transform, config);
        GenerateRepeat(sceneRoot.transform, config);

        config.Sort();

        string configPath = string.Format("{0}/{1}.asset", sceneDirectory, "config");
        if (File.Exists(configPath))
        {
            File.Delete(configPath);
        }
        AssetDatabase.CreateAsset(config, configPath);

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    static void GenerateObstcle(Transform sceneRoot, SceneConfig config)
    {
        Transform root = sceneRoot.Find("Obstacle");
        if (!root)
        {
            return;
        }

        GenerateObject(root, "Obstacles", config.obstacles);
    }

    static void GenerateOrnaments(Transform sceneRoot, SceneConfig config)
    {
        Transform root = sceneRoot.Find("Ornament");
        if (!root)
        {
            return;
        }

        GenerateObject(root, "Ornaments", config.ornaments);
    }

    static void GenerateRepeat(Transform sceneRoot, SceneConfig config)
    {
        Transform root = sceneRoot.Find("Repeat");
        if (!root)
        {
            return;
        }

        GenerateObject(root, "Repeat", config.repeats);
    }

    static void GenerateObject(Transform root, string folderName, List<SpriteObject> list)
    {
        for (int i = 0; i < root.childCount; ++i)
        {
            Transform obj = root.GetChild(i);

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();
            for (int j = 0; j < renderers.Length; ++j)
            {
                SpriteRenderer r = renderers[j];

                min = Vector3.Min(min, r.bounds.min);
                max = Vector3.Max(max, r.bounds.max);
            }

            string objDir = sceneDirectory + "/" + folderName;
            if (!Directory.Exists(objDir))
            {
                Directory.CreateDirectory(objDir);
            }

            string objPath = string.Format("{0}/{1}.prefab", objDir, obj.name);
            if (File.Exists(objPath))
            {
                File.Delete(objPath);
            }
            
            GameObject clone = GameObject.Instantiate(obj.gameObject);
            if (clone == null)
            {
                continue;
            }
            SceneObj scnObj = clone.GetComponent<SceneObj>();

            SpriteObject spriteObject = new SpriteObject();
            spriteObject.path = objPath;
            spriteObject.min = new Vector2(min.x, min.y);
            spriteObject.max = new Vector2(max.x, max.y);
            if (scnObj != null)
            {
                spriteObject.variables = scnObj.variables.ToArray();
                if (Application.isPlaying)
                {
                    GameObject.Destroy(scnObj);
                }
                else
                {
                    GameObject.DestroyImmediate(scnObj);
                }
            }
            list.Add(spriteObject);
            
            PrefabUtility.SaveAsPrefabAsset(clone, objPath);
            if (Application.isPlaying)
            {
                GameObject.Destroy(clone);
            }
            else
            {
                GameObject.DestroyImmediate(clone);
            }
        }
    }
}