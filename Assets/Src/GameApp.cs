using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameApp : MonoBehaviour
{
	void Awake()
	{
		gameLoading = true;
		ExcelLoader.Init();
	}

	void Start()
	{
		GameObject camera = Resources.Load<GameObject>("Prefab/MainCamera");
		camera = GameObject.Instantiate(camera);
		mainCamera = camera.GetComponent<Camera>();

		screenHeight = mainCamera.orthographicSize;
		screenWidth = mainCamera.aspect * screenHeight;

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
		line = mainCamera.transform.FindChild("Line").GetComponent<LineRenderer>();
        mainCamera.transform.position = Vector3.zero;

        // Load Player
        GameObject spaceShip = Resources.Load<GameObject>("Prefab/spaceship");
		spaceShip = GameObject.Instantiate(spaceShip);
		player = spaceShip.GetComponent<Player>();
		player.transform.position = new Vector3(-(screenWidth - 2.0f), 0.0f, 1.0f);

		// Load Fixed Obstacle
		upBorderObstacle = new GameObject("upObstacle");
		upBorderObstacle.layer = Global.layerObstacle;
		upBorderObstacle.transform.position = new Vector3(0.0f, screenHeight + 1.5f, 1.0f);
		BoxCollider2D c = upBorderObstacle.AddComponent<BoxCollider2D>();
		c.size = new Vector2(screenWidth * 2.0f, 3.0f);
		c.isTrigger = true;

		downBorderObstacle = new GameObject("downObstacle");
		downBorderObstacle.layer = Global.layerObstacle;
		downBorderObstacle.transform.position = new Vector3(0.0f, -screenHeight - 1.5f, 1.0f);
		c = downBorderObstacle.AddComponent<BoxCollider2D>();
		c.size = new Vector2(screenWidth * 2.0f, 3.0f);
		c.isTrigger = true;

		// Load Scene
		excel_scn_list scn1 = excel_scn_list.Find(1);
		scnRoot = Resources.Load<GameObject>("Scene/" + scn1.name + "/SceneRoot");
		scnRoot = GameObject.Instantiate(scnRoot);

		gameLoading = false;
	}

	public static void UnloadScene()
	{
		GameObject.Destroy(uiRoot);
		GameObject.Destroy(player.gameObject);
		GameObject.Destroy(scnRoot);
		GameObject.Destroy(upBorderObstacle);
		GameObject.Destroy(downBorderObstacle);
	}

	public static float screenWidth = 0.0f;
	public static float screenHeight = 0.0f;

	public static Camera mainCamera = null;
	public static Player player = null;
	public static GameObject scnRoot = null;
	public static GameObject upBorderObstacle = null;
	public static GameObject downBorderObstacle = null;
	public static GameObject uiRoot = null;

	public static bool gameLoading = true;
	public static int power = 1;
	public static bool gameOver = false;

	public static float stoneWidth = 5.0f;

	static int tick = 0;
	static LineRenderer line = null;

	public bool touchFlag = false;
	public Vector3 mBeginPos = Vector3.zero;
}
