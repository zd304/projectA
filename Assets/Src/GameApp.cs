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
		power = 1;
		speed = 0.0f;
		gameOver = false;

		GameObject camera = Resources.Load<GameObject>("Prefab/MainCamera");
		camera = GameObject.Instantiate(camera);
		mainCamera = camera.GetComponent<Camera>();

		screenHeight = mainCamera.orthographicSize;
		screenWidth = mainCamera.aspect * screenHeight;

		GameObject uiMain = Resources.Load<GameObject>("GUI/ui_main");
		uiMain = GameObject.Instantiate(uiMain);
		Canvas canvas = uiMain.GetComponent<Canvas>();
		canvas.worldCamera = mainCamera;
		UIMain.instance.gameoverGO.SetActive(false);

		GameObject spaceShip = Resources.Load<GameObject>("Prefab/spaceship");
		player = GameObject.Instantiate(spaceShip);
		player.transform.position = new Vector3(-(screenWidth - 2.0f), 0.0f, 1.0f);

		GameObject borderObstacle = new GameObject("upObstacle");
		borderObstacle.layer = Global.layerObstacle;
		borderObstacle.transform.position = new Vector3(0.0f, screenHeight + 1.5f, 1.0f);
		BoxCollider2D c = borderObstacle.AddComponent<BoxCollider2D>();
		c.size = new Vector2(screenWidth * 2.0f, 3.0f);
		c.isTrigger = true;

		borderObstacle = new GameObject("downObstacle");
		borderObstacle.layer = Global.layerObstacle;
		borderObstacle.transform.position = new Vector3(0.0f, -screenHeight - 1.5f, 1.0f);
		c = borderObstacle.AddComponent<BoxCollider2D>();
		c.size = new Vector2(screenWidth * 2.0f, 3.0f);
		c.isTrigger = true;

		excel_scn_list scn1 = excel_scn_list.Find(1);
		scnRoot = Resources.Load<GameObject>("Scene/" + scn1.name + "/SceneRoot");
		scnRoot = GameObject.Instantiate(scnRoot);

		gameLoading = false;
	}

	void FixedUpdate()
	{
		if (gameOver) return;
		++tick;
	}

	void Update()
	{
		if (gameOver) return;
		Vector3 playerPos = player.transform.position;
		playerPos.y += (speed * Time.deltaTime);
		player.transform.position = playerPos;

		speed += ((float)power * Time.deltaTime);

		Vector3 scenePos = scnRoot.transform.position;
		scenePos.x -= (Time.deltaTime * 2);
		scnRoot.transform.position = scenePos;
	}

	public static float screenWidth = 0.0f;
	public static float screenHeight = 0.0f;

	public static Camera mainCamera = null;
	public static GameObject player = null;

	public static bool gameLoading = true;
	public static int power = 1;
	public static float speed = 0.0f;
	public static bool gameOver = false;

	public static float stoneWidth = 5.0f;
	GameObject scnRoot = null;

	int tick = 0;
}
