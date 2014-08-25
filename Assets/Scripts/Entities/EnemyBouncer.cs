using UnityEngine;
using System.Collections;

public class EnemyBouncer : Entity {

	FSprite animatedSprite;

    public EnemyBouncer(Vector2 pos)
    {
        Position = pos;
        animatedSprite = new FSprite("enemy");
        AddChild(animatedSprite);
        boundingBox = new Rectangle();
        useGravity = true;
        ListenForUpdate(Update);
        HealthPoints = 4;
        Size = new Vector2(animatedSprite.width, animatedSprite.height);
        boundingBox = new Rectangle();
        Gravity = 19.6f;

        animatedSprite.scaleX = 1;

        FloorFriction = 0.58f;
        AirFriction = 0.58f;
    }

    public override void CheckAndUpdateMovement()
    {
        if (LeftWall() || RightWall())
        {
            animatedSprite.scaleX *= -1;
            velocity.x *= -1;
        }
        float step = 5f;
        step *= 1f * animatedSprite.scaleX ;

        velocity.x += step;

        if (Ground()) {
            velocity.y = Random.Range(3, 6);
        }

        if (HealthPoints <= 0) {
            Remove = true;
        }
    }

    public override void Update()
    {
        base.Update();

        

        if (HealthPoints <= 0)
            Destroy();
    }
}
