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

        Texture = new FSprite("Futile_White");
        Texture.scale = 0.8f;
        Size = new Vector2(Texture.width*0.2f, Texture.height*0.2f);
        
        AddChild(Texture);
        TileMap.CurrentMap.getPlayerBulletList().Add(this);
        TileMap.CurrentMap.EntityContainer().AddChild(this);
        Remove = false;
    }

    public override void CheckAndUpdateMovement()
    {
        float step = 10f * Time.deltaTime;
        velocity.x += step * Mathf.Cos(angle);
        velocity.y += step * Mathf.Sin(angle);
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
    }

}
