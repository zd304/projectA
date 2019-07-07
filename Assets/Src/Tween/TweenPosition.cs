using System;
using UnityEngine;

public class TweenPosition : TweenBase
{
    protected override void UpdateTween(float t)
    {
        if (transferType == TweenTransformType.World)
        {
            transform.position = Lerp(from, to, t);
        }
        else if (transferType == TweenTransformType.Local)
        {
            transform.localPosition = Lerp(from, to, t);
        }
    }

    public TweenTransformType transferType = TweenTransformType.World;
    public Vector3 from = Vector3.zero;
    public Vector3 to = Vector3.zero;
}