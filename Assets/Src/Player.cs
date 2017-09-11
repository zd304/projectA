using UnityEngine;

public class Player : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		UIMain.instance.gameoverGO.SetActive(true);
		GameApp.gameOver = true;
	}
}