using UnityEngine;

[ActionInfo("等待", "公共", "")]
public class Wait : BaseAction
{
    public override void OnEnter()
    {
        startTime = Time.realtimeSinceStartup;
        waitTime = Random.Range(minTime, maxTime);
    }

    public override void OnUpdate()
    {
        float life = Time.realtimeSinceStartup - startTime;
        if (life >= waitTime)
        {
            Continue();
        }
    }

    float waitTime;
    float startTime;

    public float minTime;
    public float maxTime;
}