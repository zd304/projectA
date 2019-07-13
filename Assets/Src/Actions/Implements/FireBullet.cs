using UnityEngine;

[ActionInfo("发射子弹", "技能", "")]
public class FireBullet : BaseAction
{
    public override void OnEnter()
    {
        if (bulletPrefab)
        {
            GameObject bulletGO = GameObject.Instantiate(bulletPrefab.gameObject);
            bulletGO.transform.position = context.owner.transform.position;
            bulletGO.transform.rotation = Quaternion.identity;
            bulletGO.transform.localScale = Vector3.one;

            Bullet bullet = bulletGO.AddComponent<Bullet>();
            bullet.moveType = moveType;
            bullet.speed = speed;
            bullet.ActionContext = context;
        }

        Continue();
    }

    public Transform bulletPrefab = null;
    public MoveType moveType = MoveType.NoMove;
    public float speed = 1.0f;
}