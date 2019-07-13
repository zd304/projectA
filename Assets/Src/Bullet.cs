using UnityEngine;

public enum MoveType
{
    NoMove,
    MoveToTargetPos,
    MoveToTarget,
}

public class Bullet : MonoBehaviour
{
    private void Update()
    {
        if (vanish)
        {
            Destroy(gameObject);
            return;
        }
        if (ActionContext == null)
        {
            return;
        }

        Vector3 dir = Vector3.zero;
        float distance = 0.0f;
        switch (moveType)
        {
            case MoveType.NoMove:
                return;
            case MoveType.MoveToTarget:
                {
                    if (ActionContext.target)
                    {
                        dir = ActionContext.target.transform.position - transform.position;
                        distance = dir.magnitude;
                        dir.Normalize();
                    }
                }
                break;
            case MoveType.MoveToTargetPos:
                {
                    dir = ActionContext.targetPos - transform.position;
                    distance = dir.magnitude;
                    dir.Normalize();
                }
                break;
        }
        transform.position += (dir * speed);

        if (distance <= radius)
        {
            vanish = true;
        }
    }

    private ActionContext actionContext = new ActionContext();
    public ActionContext ActionContext
    {
        set
        {
            actionContext.CloneFrom(value);
            actionContext.bullet = this;
        }
        get
        {
            return actionContext;
        }
    }

    private bool vanish = false;

    public MoveType moveType = MoveType.NoMove;
    public float speed = 1.0f;
    public float radius = 1.0f;
}