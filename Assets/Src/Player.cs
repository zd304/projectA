using UnityEngine;

public class Player : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		UIMain.instance.gameoverGO.SetActive(true);
		GameApp.gameOver = true;
	}

	public void RenderTick()
	{
		Vector3 playerPos = transform.position;
		playerPos.y += (speed * Time.deltaTime);
		transform.position = playerPos;

		speed += ((float)GameApp.power * Time.deltaTime);
	}

	float speed = 0.0f;
}