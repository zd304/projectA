using UnityEngine;
using System.Collections;

public class GameApp : MonoBehaviour
{
	void Awake()
	{
		gameLoading = true;
	}

	void Start()
	{
		GameObject camera = Resources.Load<GameObject>("Prefab/MainCamera");
		camera = GameObject.Instantiate(camera);
		mainCamera = camera.GetComponent<Camera>();

		screenHeight = mainCamera.orthographicSize;
		screenWidth = mainCamera.aspect * screenHeight;

		GameObject uiMain = Resources.Load<GameObject>("GUI/ui_main");
		uiMain = GameObject.Instantiate(uiMain);
		Canvas canvas = uiMain.GetComponent<Canvas>();
		canvas.worldCamera = mainCamera;

		GameObject spaceShip = Resources.Load<GameObject>("Prefab/spaceship");
		player = GameObject.Instantiate(spaceShip);
		player.transform.position = new Vector3(-(screenWidth - 2.0f), 0.0f, 1.0f);

		gameLoading = false;
	}

	void Update()
	{
		
	}

	public static float screenWidth = 0.0f;
	public static float screenHeight = 0.0f;

	public static Camera mainCamera = null;
	public static GameObject player = null;

	public static bool gameLoading = true;
}
