using UnityEngine;

public class ActionContext
{
    public void Reset()
    {
        owner = null;
        bullet = null;
        targetPos = Vector3.zero;
        target = null;
    }

    public void CloneFrom(ActionContext other)
    {
        owner = other.owner;
        bullet = other.bullet;
        targetPos = other.targetPos;
        target = other.target; 
    }

    public Character owner;
    public Bullet bullet;
    public Vector3 targetPos;
    public Character target;
}