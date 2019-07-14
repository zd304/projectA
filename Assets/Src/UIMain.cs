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
		float btnSize = (float)Screen.height * 0.167f;

		Vector2 offsetMin, offsetMax;
		offsetMin = northBtn.rectTransform.offsetMin;
		offsetMax = northBtn.rectTransform.offsetMax;
		offsetMin.y = -btnSize;
		offsetMax.x = btnSize;
		northBtn.rectTransform.offsetMin = offsetMin;
		northBtn.rectTransform.offsetMax = offsetMax;

		offsetMax = southBtn.rectTransform.offsetMax;
		offsetMax.x = btnSize;
		offsetMax.y = btnSize;
		southBtn.rectTransform.offsetMax = offsetMax;

		float powerSize = (float)Screen.height * 0.08f;
		for (int i = 0; i < northPowers.Length; ++i)
		{
			Image poweImage = northPowers[i];
			poweImage.rectTransform.offsetMin = new Vector2(powerSize * i, -powerSize);
			poweImage.rectTransform.offsetMax = new Vector2(powerSize * (i + 1), 0.0f);
		}
		for (int i = 0; i < southPowers.Length; ++i)
		{
			Image poweImage = southPowers[i];
			poweImage.rectTransform.offsetMin = new Vector2(powerSize * i, 0.0f);
			poweImage.rectTransform.offsetMax = new Vector2(powerSize * (i + 1), powerSize);
		}
	}

	public void ClickNorthBtn()
	{
        if (GameApp.power < 0)
        {
            GameApp.power = 0;
        }
		++GameApp.power;
		UpdatePower();
	}

	public void ClickSouthBtn()
	{
        if (GameApp.power > 0)
        {
            GameApp.power = 0;
        }
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

    public void OnGUI()
    {
        if (northBtn.material)
        {
            GUI.Box(new Rect(10, 10, 200, 50), "找到材质了" + northBtn.material.name);
            if (northBtn.material.shader)
            {
                GUI.Box(new Rect(10, 110, 200, 50), "找到Shader了" + northBtn.material.shader.name);
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
	public Image northBtn;
	public Image southBtn;
	public GameObject gameoverGO;
}