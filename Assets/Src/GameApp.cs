using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameApp : MonoBehaviour
{
	void Awake()
	{
		gameLoading = true;
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

		upStone1 = Resources.Load<GameObject>("Prefab/up_stone1");
		upStone2 = Resources.Load<GameObject>("Prefab/up_stone2");
		downStone1 = Resources.Load<GameObject>("Prefab/down_stone1");
		downStone2 = Resources.Load<GameObject>("Prefab/down_stone2");

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

		gameLoading = false;
	}

	void FixedUpdate()
	{
		if (gameOver) return;
		if (tick == 50)
		{
			GameObject go = GameObject.Instantiate(upStone1);
			stones.Add(go);
			go.transform.position = new Vector3(screenWidth + stoneWidth, screenHeight, 1.0f);
		}
		if (tick == 300)
		{
			GameObject go = GameObject.Instantiate(downStone2);
			stones.Add(go);
			go.transform.position = new Vector3(screenWidth + stoneWidth, -screenHeight, 1.0f);
		}
		if (tick == 400)
		{
			GameObject go = GameObject.Instantiate(upStone2);
			stones.Add(go);
			go.transform.position = new Vector3(screenWidth + stoneWidth, screenHeight, 1.0f);
		}
		if (tick == 650)
		{
			GameObject go = GameObject.Instantiate(downStone1);
			stones.Add(go);
			go.transform.position = new Vector3(screenWidth + stoneWidth, -screenHeight, 1.0f);
		}
		++tick;
	}

	void Update()
	{
		if (gameOver) return;
		for (int i = stones.Count - 1; i >= 0; --i)
		{
			GameObject stone = stones[i];
			Vector3 pos = stone.transform.position;
			pos.x -= (Time.deltaTime * 2.0f);
			stone.transform.position = pos;
			if (stone.transform.position.x < -(screenWidth + stoneWidth))
			{
				int endIndex = stones.Count - 1;
				GameObject tmp = stones[endIndex];
				stones[endIndex] = stone;
				stones[i] = tmp;
				stones.RemoveAt(endIndex);
				GameObject.Destroy(stone);
			}
		}
		Vector3 playerPos = player.transform.position;
		playerPos.y += (speed * Time.deltaTime);
		player.transform.position = playerPos;

		speed += ((float)power * Time.deltaTime);
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
	GameObject upStone1 = null;
	GameObject upStone2 = null;
	GameObject downStone1 = null;
	GameObject downStone2 = null;

	List<GameObject> stones = new List<GameObject>();

	int tick = 0;
}
