using UnityEngine;
using System.Collections;

public class Player : Entity {

    float firingCadence, firingCadenceTime;
    public float FiringCadence { set { firingCadence = value; } get { return firingCadence; } }
    public float FiringCadenceTime { set { firingCadenceTime = value; } get { return firingCadenceTime; } }

    Vector2 weatherResistance;
    public Vector2 WeatherResistance { set { weatherResistance = value; } get { return weatherResistance; } }

    float bulletImpact;
    public float BulletImpact { set { bulletImpact = value; } get { return bulletImpact; } }

    Pool<Bullet> ammoLeft = new Pool<Bullet>(() => new Bullet());
    //for space boots
    bool jumpThrust = true;
    public bool JumpThrust { set { jumpThrust = value; } get { return jumpThrust; } }

    FAnimatedSprite animatedSprite;

    int backPackFuel = 1;
    int currenBackPackFuel;
    public int BackPackFuel { set { backPackFuel = value; } get { return backPackFuel; } }

    public Pool<Bullet> AmmoLeft()
    {
        return ammoLeft;
    }

    public Player()
    {
        HealthPoints = 100;
        useGravity = true;
        Gravity = 19.6f;
        weatherResistance = Vector2.zero;
        bulletImpact = 5f;
        animatedSprite = new FAnimatedSprite("dude");
        animatedSprite.addAnimation(new FAnimation("walking", new int[]{
                0,1,2,3}, 200, true));

        Size = new Vector2(animatedSprite.width*0.5f, animatedSprite.height*0.9f);
        animatedSprite.x += 2f;
        boundingBox = new Rectangle();
        impactBoundingBox = new Rectangle();
        Position = new Vector2(Futile.screen.halfWidth, -Futile.screen.halfHeight);
        AddChild(animatedSprite);
    
        FloorFriction = 0.58f;
        AirFriction = 0.58f;
        ammoLeft.Preallocate(5);
        detectionAccuracy = (int)(Size.x / 2) + 1;
        firingCadence = 300f;
        firingCadenceTime = 300f;
    }

    public override void CheckAndUpdateMovement()
    {

        if (TileMap.CurrentMap.alpha > 0.2f)
        {
            float step = 15f * Time.deltaTime;
            step *= Input.GetAxis("Horizontal") * 10f;
            velocity.x += step;

            if (Ground() || currenBackPackFuel > 0)
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    velocity.y = 6f;
                    currenBackPackFuel--;
                }
            if (Ground())
                currenBackPackFuel = BackPackFuel;

        }
    }

    public override void Update()
    {
        base.Update();

        if (HealthPoints <= 0) {
            TileMap.CurrentMap.CallGameOver();
            HealthPoints = 1;
        }

        firingCadenceTime += Time.deltaTime * 1000;

        Vector2 pos = GlobalToLocal(Input.mousePosition);

        if (pos.x > Position.x)
        {
            animatedSprite.scaleX = -1;
        }
        else {
            animatedSprite.scaleX = 1;
        }

        if (FiringCadenceTime < FiringCadence)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            FiringCadenceTime = 0;
            if (ammoLeft.hasFreeItems()) {
                Vector2 wheretoSpawns = Position;
                wheretoSpawns.x += (8 ) * -animatedSprite.scaleX;
                ammoLeft.GetFreeItem().init(wheretoSpawns, animatedSprite.scaleX);
            }
        }

    }
}
