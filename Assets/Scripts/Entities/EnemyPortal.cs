using UnityEngine;
using System.Collections;

public class EnemyPortal : Entity
{
    FAnimatedSprite animatedSprite;

    public EnemyPortal(Vector2 pos)
    {
        Position = pos;
        animatedSprite = new FAnimatedSprite("portal");
        animatedSprite.addAnimation(new FAnimation("glow", new int[]{
                0,1,2,3}, 200, true));
        AddChild(animatedSprite);
        animatedSprite.color = Color.red;
        boundingBox = new Rectangle();
        useGravity = false;
        ListenForUpdate(Update);
        HealthPoints = 10;
        Size = new Vector2(animatedSprite.width, animatedSprite.height);
        boundingBox = new Rectangle();
    }

    public override void Update()
    {
        base.Update();

        float posY = Mathf.Lerp(Position.y - 10f, Position.y + 10f, Mathf.PingPong(Time.time, 1.0f));
        SetPosition(Position.x, posY);

        if (Time.frameCount % 120 == 0)
        {
            if (Vector2.Distance(TileMap.CurrentMap.getPlayer().Position, Position) < 200)
            {
                EnemyBouncer en = new EnemyBouncer(Position);
                TileMap.CurrentMap.getEnemyList().Add(en);
                TileMap.CurrentMap.AddChild(en);
            }

        }
        if (HealthPoints <= 0)
            Destroy();
    }


}
