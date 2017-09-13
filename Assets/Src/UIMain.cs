using UnityEngine;
using System;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
	void Awake()
	{
		instance = this;
		UpdatePower();
		gameoverGO.SetActive(false);
	}

	void Start()
	{
		
	}

	public void ClickNorthBtn()
	{
		++GameApp.power;
		UpdatePower();
	}

	public void ClickSouthBtn()
	{
		--GameApp.power;
		UpdatePower();
	}

	public void ClickGameOver()
	{
		GameApp.UnloadScene();
		GameApp.LoadScene(1);
	}

	public void UpdatePower()
	{
		if (GameApp.power > 0)
		{
			int p = Math.Abs (GameApp.power);
			for (int i = 0; i < northPowers.Length; ++i)
			{
				Image pimage = northPowers[i];
				if (i < p)
				{
					pimage.color = Color.yellow;
					continue;
				}
				pimage.color = Color.gray;
			}
			for (int i = 0; i < southPowers.Length; ++i)
			{
				Image pimage = southPowers[i];
				pimage.color = Color.gray;
			}
		}
		else if (GameApp.power < 0)
		{
			int p = Math.Abs (GameApp.power);
			for (int i = 0; i < southPowers.Length; ++i)
			{
				Image pimage = southPowers[i];
				if (i < p)
				{
					pimage.color = Color.yellow;
					continue;
				}
				pimage.color = Color.gray;
			}
			for (int i = 0; i < northPowers.Length; ++i)
			{
				Image pimage = northPowers[i];
				pimage.color = Color.gray;
			}
		}
		else
		{
			for (int i = 0; i < northPowers.Length; ++i)
			{
				Image pimage = northPowers[i];
				pimage.color = Color.gray;
			}
			for (int i = 0; i < southPowers.Length; ++i)
			{
				Image pimage = southPowers[i];
				pimage.color = Color.gray;
			}
		}
	}

	static UIMain mInstance = null;
	public static UIMain instance
	{
		private set
		{
			mInstance = value;
		}
		get
		{
			return mInstance;
		}
	}

	public Image[] southPowers;
	public Image[] northPowers;
	public GameObject gameoverGO;
}