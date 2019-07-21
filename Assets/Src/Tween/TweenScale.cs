using System;
using UnityEngine;

public class TweenScale : TweenBase
{
    protected override void UpdateTween(float t)
    {
        transform.localScale = Lerp(from, to, t);
    }

    public Vector3 from = Vector3.zero;
    public Vector3 to = Vector3.zero;    
}