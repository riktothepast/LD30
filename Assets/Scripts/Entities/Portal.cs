using UnityEngine;
using System.Collections;

public class Portal : FContainer{

    Vector2 position;
    int destination;
    Rectangle boundingBox;
    FAnimatedSprite animatedSprite;


    public enum Destinations {
        TheVoid, Hout, OldPlace, GoldLand, Freezor, Main
    }

    public Portal(Vector2 pos, int dest)
    {
        position = pos;
        destination = dest;
        animatedSprite = new FAnimatedSprite("portal");
        animatedSprite.addAnimation(new FAnimation("glow", new int[]{
                0,1,2,3}, 200, true));
        AddChild(animatedSprite);
        boundingBox = new Rectangle();
    }

    public Vector2 getPosition()
    {
        return position;
    }

    public int getDestination() {
        return destination;
    }

    public Rectangle getAABBBoundingBox() {
        boundingBox.setValues(position.x, position.y, 16, 16);
        return boundingBox;
    }
}
