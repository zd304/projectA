﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameApp : MonoBehaviour
{
	void Awake()
	{
		gameLoading = true;
		ExcelLoader.Init();
	}

    private void OnDrawGizmos()
    {
        if (!mainCamera)
            return;

        Vector3 v1 = new Vector3(min.x, min.y, 0.0f) + mainCamera.transform.position;
        Vector3 v2 = new Vector3(max.x, min.y, 0.0f) + mainCamera.transform.position;
        Vector3 v3 = new Vector3(max.x, max.y, 0.0f) + mainCamera.transform.position;
        Vector3 v4 = new Vector3(min.x, max.y, 0.0f) + mainCamera.transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(v1, v2);
        Gizmos.DrawLine(v2, v3);
        Gizmos.DrawLine(v3, v4);
        Gizmos.DrawLine(v4, v1);
    }

    void Start()
	{
		GameObject camera = Resources.Load<GameObject>("Prefab/MainCamera");
		camera = GameObject.Instantiate(camera);
		mainCamera = camera.GetComponent<Camera>();

		screenHeight = mainCamera.orthographicSize;
		screenWidth = mainCamera.aspect * screenHeight;

        if (Screen.width > Screen.height)
        {
            screenWidth = Mathf.Max(screenHeight, screenWidth);
            screenHeight = Mathf.Min(screenHeight, screenWidth);
        }
        else
        {
            screenWidth = Mathf.Min(screenHeight, screenWidth);
            screenHeight = Mathf.Max(screenHeight, screenWidth);
        }
        
        min = new Vector2(-screenWidth, -screenHeight);
        max = new Vector2(screenWidth + screenWidth * 0.3f, screenHeight);

        LoadScene(1);
	}

	void FixedUpdate()
	{
		if (gameOver) return;
		++tick;
	}

	void Update()
	{
		if (gameOver) return;

		player.RenderTick();

        if (scnRoot)
        {
            //Vector3 scenePos = scnRoot.transform.position;
            //scenePos.x -= (Time.deltaTime * 2);
            //scnRoot.transform.position = scenePos;

            Vector3 p = player.transform.position;
            p.x += (Time.deltaTime * 2);
            player.transform.position = p;

            p = mainCamera.transform.position;
            p.x += (Time.deltaTime * 2);
            mainCamera.transform.position = p;
        }

        Vector2 mp = mainCamera.transform.position;
        sceneManger.UpdateLoading(min + mp, max + mp, ornamentRoot, obstacleRoot, repeatRoot);
        
        TouchUpdate();

		MouseUpdate ();
	}

	void TouchUpdate()
	{
		if (Input.touchCount != 1)
			return;
		if (Input.GetTouch(0).phase == TouchPhase.Began && !touchFlag)
		{
			if (line != null)
				line.enabled = true;
			touchFlag = true;
			mBeginPos = Input.GetTouch(0).position;
		}
		Vector3 nowPos = Input.GetTouch(0).position;
		if (Input.GetTouch(0).phase == TouchPhase.Ended && touchFlag)
		{
			if (nowPos.y - mBeginPos.y > 0)
			{
				UIMain.instance.ClickNorthBtn();
			}
			else if (nowPos.y - mBeginPos.y < 0)
			{
				UIMain.instance.ClickSouthBtn();
			}
			if (line != null)
				line.enabled = false;
			touchFlag = false;
		}
		if (!touchFlag)
			return;
		if (line == null)
			return;
		float x = mBeginPos.x / Screen.width;
		float y = mBeginPos.y / Screen.height;
		x = x * screenWidth * 2.0f - screenWidth;
		y = y * screenHeight * 2.0f - screenHeight;
		line.SetPosition(0, new Vector3(x + mainCamera.transform.position.x, y, 1.0f));

		x = nowPos.x / Screen.width;
		y = nowPos.y / Screen.height;
		x = x * screenWidth * 2.0f - screenWidth;
		y = y * screenHeight * 2.0f - screenHeight;
		line.SetPosition(1, new Vector3(x + mainCamera.transform.position.x, y, 1.0f));
	}

	void MouseUpdate()
	{
		if (Input.GetMouseButtonDown(0) && !touchFlag)
		{
			if (line != null)
				line.enabled = true;
			touchFlag = true;
			mBeginPos = Input.mousePosition;
		}
		Vector3 nowPos = Input.mousePosition;
		if (Input.GetMouseButtonUp(0))
		{
			if (nowPos.y - mBeginPos.y > 0)
			{
				UIMain.instance.ClickNorthBtn();
			}
			else if (nowPos.y - mBeginPos.y < 0)
			{
				UIMain.instance.ClickSouthBtn();
			}
			if (line != null)
				line.enabled = false;
			touchFlag = false;
		}
		if (!touchFlag)
			return;
		if (line == null)
			return;
		float x = mBeginPos.x / Screen.width;
		float y = mBeginPos.y / Screen.height;
		x = x * screenWidth * 2.0f - screenWidth;
		y = y * screenHeight * 2.0f - screenHeight;
		line.SetPosition(0, new Vector3(x + mainCamera.transform.position.x, y, 1.0f));

		x = nowPos.x / Screen.width;
		y = nowPos.y / Screen.height;
		x = x * screenWidth * 2.0f - screenWidth;
		y = y * screenHeight * 2.0f - screenHeight;
		line.SetPosition(1, new Vector3(x + mainCamera.transform.position.x, y, 1.0f));
	}

	public static void LoadScene(int scnID)
	{
		gameLoading = true;

		power = 1;
		gameOver = false;
		tick = 0;

		// Load UI
		uiRoot = Resources.Load<GameObject>("GUI/ui_main");
		uiRoot = GameObject.Instantiate(uiRoot);
		Canvas canvas = uiRoot.GetComponent<Canvas>();
		canvas.worldCamera = mainCamera;
		UIMain.instance.gameoverGO.SetActive(false);
		line = mainCamera.transform.Find("Line").GetComponent<LineRenderer>();
        mainCamera.transform.position = Vector3.zero;

        // Load Player
        GameObject spaceShip = Resources.Load<GameObject>("Prefab/space_ship_magnet");
		spaceShip = GameObject.Instantiate(spaceShip);
		player = spaceShip.GetComponent<Player>();
		player.transform.position = new Vector3(-(screenWidth - 2.0f), 0.0f, 1.0f);

        // Load Fixed Obstacle
        upBorderObstacle = new GameObject("upObstacle");
		upBorderObstacle.layer = Global.layerObstacle;
        upBorderObstacle.transform.parent = mainCamera.transform;
        upBorderObstacle.transform.position = new Vector3(0.0f, screenHeight + 1.5f, 1.0f);
		BoxCollider2D c = upBorderObstacle.AddComponent<BoxCollider2D>();
		c.size = new Vector2(screenWidth * 2.0f, 3.0f);
		c.isTrigger = true;

		downBorderObstacle = new GameObject("downObstacle");
		downBorderObstacle.layer = Global.layerObstacle;
        downBorderObstacle.transform.parent = mainCamera.transform;
        downBorderObstacle.transform.position = new Vector3(0.0f, -screenHeight - 1.5f, 1.0f);
		c = downBorderObstacle.AddComponent<BoxCollider2D>();
		c.size = new Vector2(screenWidth * 2.0f, 3.0f);
		c.isTrigger = true;

        // Load Scene
        excel_scn_list scn1 = excel_scn_list.Find(1);
        //scnRoot = Resources.Load<GameObject>("Scene/" + scn1.name + "/SceneRoot");
        //scnRoot = GameObject.Instantiate(scnRoot);

        if (scnRoot == null)
        {
            scnRoot = new GameObject("SceneRoot");
            scnRoot.transform.position = Vector3.zero;
            scnRoot.transform.rotation = Quaternion.identity;
            scnRoot.transform.localScale = Vector3.one;
        }

        if (ornamentRoot == null)
        {
            GameObject ornamentGO = new GameObject("Ornaments");
            ornamentRoot = ornamentGO.transform;
            ornamentRoot.parent = scnRoot.transform;
            ornamentRoot.localPosition = Vector3.zero;
            ornamentRoot.localRotation = Quaternion.identity;
            ornamentRoot.localScale = Vector3.one;
        }
        
        if (obstacleRoot == null)
        {
            GameObject obstacleGO = new GameObject("Obstacles");
            obstacleRoot = obstacleGO.transform;
            obstacleRoot.parent = scnRoot.transform;
            obstacleRoot.localPosition = Vector3.zero;
            obstacleRoot.localRotation = Quaternion.identity;
            obstacleRoot.localScale = Vector3.one;
        }

        if (repeatRoot == null)
        {
            GameObject repeatGO = new GameObject("Repeat");
            repeatRoot = repeatGO.transform;
            repeatRoot.parent = scnRoot.transform;
            repeatRoot.localPosition = Vector3.zero;
            repeatRoot.localRotation = Quaternion.identity;
            repeatRoot.localScale = Vector3.one;
        }

        sceneManger.Load(scn1.name);

        gameLoading = false;
	}

	public static void UnloadScene()
	{
		GameObject.Destroy(uiRoot);
		GameObject.Destroy(player.gameObject);
		GameObject.Destroy(scnRoot);
		GameObject.Destroy(upBorderObstacle);
		GameObject.Destroy(downBorderObstacle);

        scnRoot = null;
        ornamentRoot = null;
        obstacleRoot = null;
        repeatRoot = null;
        sceneManger.Unload();
        CharacterManager.Instance.Destroy();
    }

	public static float screenWidth = 0.0f;
	public static float screenHeight = 0.0f;

	public static Camera mainCamera = null;
	public static Player player = null;
	public static GameObject scnRoot = null;
	public static GameObject upBorderObstacle = null;
	public static GameObject downBorderObstacle = null;
	public static GameObject uiRoot = null;

    public static Transform ornamentRoot = null;
    public static Transform obstacleRoot = null;
    public static Transform repeatRoot = null;

    public static Vector2 min = Vector3.zero;
    public static Vector2 max = Vector3.zero;

	public static bool gameLoading = true;
	public static int power = 1;
	public static bool gameOver = false;

	public static float stoneWidth = 5.0f;

	static int tick = 0;
	static LineRenderer line = null;
    static SceneManger sceneManger = new SceneManger();

	public bool touchFlag = false;
	public Vector3 mBeginPos = Vector3.zero;
}
