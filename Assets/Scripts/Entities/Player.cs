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
    bool jumpThrust = false;
    public bool JumpThrust { set { jumpThrust = value; } get { return jumpThrust; } }

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
        Texture = new FSprite("Futile_White");
        FSprite UpperTexture = new FSprite("Futile_White");
        UpperTexture.color = Color.red;
        Size = new Vector2(Texture.width, Texture.height*2);
        boundingBox = new Rectangle();
        impactBoundingBox = new Rectangle();
        Position = new Vector2(Futile.screen.halfWidth, -Futile.screen.halfHeight);
        AddChild(Texture);
        AddChild(UpperTexture);
        UpperTexture.y += 8f;
        Texture.y -= 8f;
        FloorFriction = 0.58f;
        AirFriction = 0.58f;
        ammoLeft.Preallocate(5);
        detectionAccuracy = (int)(Size.x / 2) + 1;
        firingCadence = 300f;
        firingCadenceTime = 300f;
    }

    public override void CheckAndUpdateMovement()
    {
        float step = 15f * Time.deltaTime;
        step *= Input.GetAxis("Horizontal") * 10f;
        velocity.x += step;

        if(Ground()||jumpThrust)
            if (Input.GetKeyDown(KeyCode.Space)) {
                velocity.y = 7.8f;
            }
        
    }

    public override void Update()
    {
        base.Update();

        firingCadenceTime += Time.deltaTime * 1000;

        if (FiringCadenceTime < FiringCadence)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            FiringCadenceTime = 0;
            Vector2 pos = LocalToGlobal(Input.mousePosition);
            Debug.Log("shooting");
            float angle = Mathf.Atan2(Position.y - pos.y, Position.x - pos.x) * Mathf.Rad2Deg;
            Debug.Log(angle);
            if (ammoLeft.hasFreeItems()) {
                ammoLeft.GetFreeItem().init(Position, angle);
            }
        }

    }
}
