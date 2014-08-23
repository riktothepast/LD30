using UnityEngine;
using System.Collections;

public class Player : Entity {
    Vector2 weatherResistance;
    public Vector2 WeatherResistance { set { weatherResistance = value; } get { return weatherResistance; } }

    float bulletImpact;
    public float BulletImpact { set { bulletImpact = value; } get { return bulletImpact; } }

    public Player()
    {
        HealthPoints = 100;
        useGravity = true;
        Gravity = 19.6f;
        weatherResistance = Vector2.zero;
        bulletImpact = 5f;
    }

}
