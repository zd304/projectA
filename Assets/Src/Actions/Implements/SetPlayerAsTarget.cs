
[ActionInfo("设置玩家为目标", "技能", "")]
public class SetPlayerAsTarget : BaseAction
{
    public override void OnEnter()
    {
        if (context != null)
        {
            context.target = CharacterManager.Instance.MainPlayer;
        }
        Continue();
    }
}