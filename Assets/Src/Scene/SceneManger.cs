using System;
using System.Collections.Generic;
using UnityEngine;

public struct RepeatObjectData
{
    public string path;
    public Vector2 min;
    public Vector2 max;
    public float sizeX;
    public Vector2 minAndMax;
}

public class RepeatObject
{
    public int index;
    public string path;
    public Vector2 min;
    public Vector2 max;
    public GameObject go;

    static Dictionary<int, List<RepeatObject>> pools = new Dictionary<int, List<RepeatObject>>();

    public static RepeatObject Create(int index, RepeatObjectData rod)
    {
        List<RepeatObject> pool = null;
        if (!pools.TryGetValue(index, out pool))
        {
            pool = new List<RepeatObject>();
            pools.Add(index, pool);
        }
        RepeatObject rst = null;
        if (pool.Count > 0)
        {
            rst = pool[0];
            rst.go.SetActive(true);
            pool.RemoveAt(0);
        }
        else
        {
            rst = new RepeatObject();
            rst.index = index;
            rst.path = rod.path;

            int prefixLen = "Assets/Resources/".Length;

            string resPath = rod.path.Substring(prefixLen);
            int dot = resPath.LastIndexOf('.');
            if (dot > 0)
            {
                resPath = resPath.Substring(0, dot);
            }
            GameObject goRes = Resources.Load<GameObject>(resPath);
            if (goRes != null)
            {
                rst.go = GameObject.Instantiate(goRes);
            }
        }
        return rst;
    }

    public static void Destroy(RepeatObject data)
    {
        if (data == null)
            return;
        List<RepeatObject> pool = null;
        if (!pools.TryGetValue(data.index, out pool))
        {
            pool = new List<RepeatObject>();
            pools.Add(data.index, pool);
        }
        data.go.SetActive(false);
        pool.Add(data);
    }
}

public class SceneManger
{
    SceneConfig currentSceneConfig = null;

    List<RepeatObjectData> repeatDatas = new List<RepeatObjectData>();
    Dictionary<int, List<RepeatObject>> repeatObjs = new Dictionary<int, List<RepeatObject>>();

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
        for (int i = 0; i < currentSceneConfig.repeats.Count; ++i)
        {
            SpriteObject so = currentSceneConfig.repeats[i];
            if (so.variables.Length != 2
                || so.variables[0].key != "minX"
                || so.variables[1].key != "maxX")
            {
                continue;
            }

            RepeatObjectData rod = new RepeatObjectData();
            rod.path = so.path;
            rod.min = so.min;
            rod.max = so.max;
            float minX = 0.0f;
            float maxX = 0.0f;
            float.TryParse(so.variables[0].value, out minX);
            float.TryParse(so.variables[1].value, out maxX);
            rod.minAndMax = new Vector2(minX, maxX);
            rod.sizeX = so.max.x - so.min.x;

            repeatDatas.Add(rod);
        }
    }

    public void UpdateLoading(Vector2 min, Vector2 max, Transform ornamentRoot, Transform obstacleRoot, Transform repeatRoot)
    {
        if (currentSceneConfig == null)
        {
            return;
        }
        UpdateLoadingObj(currentSceneConfig.ornaments, ornamentRoot, min, max, ref curLoadedOrnamentIndex);
        UpdateLoadingObj(currentSceneConfig.obstacles, obstacleRoot, min, max, ref curLoadedObstacleIndex);
        UpdateLoadingRepeat(repeatRoot, min, max);
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

    void UpdateLoadingRepeat(Transform root, Vector2 min, Vector2 max)
    {
        for (int i = 0; i < repeatDatas.Count; ++i)
        {
            RepeatObjectData rod = repeatDatas[i];
            
            // 当重复刷新区域和相机框有交集
            if (rod.minAndMax.x <= max.x && rod.minAndMax.y >= min.x)
            {
                List<RepeatObject> objs = null;
                if (!repeatObjs.TryGetValue(i, out objs))
                {
                    objs = new List<RepeatObject>();
                    repeatObjs.Add(i, objs);
                }
                if (objs.Count == 0)
                {
                    // 加一个新的填充到空队列里
                    RepeatObject o = RepeatObject.Create(i, rod);
                    o.min = new Vector2(rod.minAndMax.x, rod.min.y);
                    o.max = new Vector2(rod.minAndMax.x + rod.sizeX, rod.max.y);
                    float z = o.go.transform.position.z;
                    o.go.transform.position = new Vector3(rod.minAndMax.x + rod.sizeX * 0.5f,
                        (rod.min.y + rod.max.y) * 0.5f, z);
                    objs.Add(o);
                }
                else
                {
                    // 如果队尾元素已经完全进入视野，加载下一个元素
                    RepeatObject last = objs[objs.Count - 1];
                    if (last.max.x < max.x)
                    {
                        RepeatObject o = RepeatObject.Create(i, rod);
                        o.min = new Vector2(last.max.x, rod.min.y);
                        o.max = new Vector2(last.max.x + rod.sizeX, rod.max.y);
                        float z = o.go.transform.position.z;
                        o.go.transform.position = new Vector3(last.max.x + rod.sizeX * 0.5f,
                            (rod.min.y + rod.max.y) * 0.5f, z);
                        objs.Add(o);
                    }
                    // 如果队首元素出了视野，则回收队首元素
                    RepeatObject first = objs[0];
                    if (first.max.x < min.x)
                    {
                        objs.RemoveAt(0);
                        RepeatObject.Destroy(first);
                    }
                }
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