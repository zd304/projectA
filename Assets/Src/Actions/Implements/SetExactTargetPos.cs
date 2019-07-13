using UnityEngine;

[ActionInfo("精确设置目标点位置", "技能", "")]
class SetExactTargetPos : BaseAction
{
    public override void OnEnter()
    {
        if (context != null && context.owner)
        {
            context.targetPos = context.owner.transform.position + offset;
        }
        Continue();
    }

    public Vector3 offset = Vector3.zero;
}