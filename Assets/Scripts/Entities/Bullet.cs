using UnityEngine;
using System.Collections;

public class Bullet : Entity {
    float TTL = 1f;
    float angle;

    public Bullet()
    {

    }

    public void init(Vector2 position, float ang, float timeToLive = 1f)
    {
        Position = position;
        SetPosition(Position);
        useGravity = false;
        TTL = timeToLive;
        boundingBox = new Rectangle();
        impactBoundingBox = new Rectangle();
        angle = ang;

        Texture = new FSprite("bullet");

        Size = new Vector2(Texture.width*0.6f, Texture.height*0.6f);
        
        AddChild(Texture);
        TileMap.CurrentMap.getPlayerBulletList().Add(this);
        TileMap.CurrentMap.EntityContainer().AddChild(this);
        Remove = false;
        detectionAccuracy = (int)(Size.x / 2) + 1;
        velocity = Vector2.zero;
        boundingBox = new Rectangle();
    }

    public override void CheckAndUpdateMovement()
    {
        float step = 20f * Time.deltaTime;
        velocity.x += step * -angle;
    }

    public override void Update()
    {
        base.Update();

        TTL -= Time.deltaTime;

        if(Ground()||Ceiling()||LeftWall()||(RightWall())){
            Destroy();
            TileMap.CurrentMap.getPlayer().AmmoLeft().FlagFreeItem(this);
            velocity = Vector2.zero;
            Remove = true;
        }

        if (TTL <= 0) {
            Destroy();
            TileMap.CurrentMap.getPlayer().AmmoLeft().FlagFreeItem(this);
            velocity = Vector2.zero;
            Remove = true;
        }

        for (int x = TileMap.CurrentMap.getEnemyList().Count-1; x >= 0; x--)
        {
            if (TileMap.CurrentMap.getEnemyList() != null)
            {
                if (Rectangle.AABBCheck(getAABBBoundingBox(), TileMap.CurrentMap.getEnemyList()[x].getAABBBoundingBox()))
                {
                    Destroy();
                    TileMap.CurrentMap.getPlayer().AmmoLeft().FlagFreeItem(this);
                    velocity = Vector2.zero;
                    Remove = true;
                    TileMap.CurrentMap.getEnemyList()[x].HealthPoints = TileMap.CurrentMap.getEnemyList()[x].HealthPoints - 2;
                }
            }
        }
        
    }

}
