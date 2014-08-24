using UnityEngine;
using System.Collections;

public class Item :FContainer{

    Vector2 position;
    int destination;
    Rectangle boundingBox;
    FSprite animatedSprite;


    public enum Destinations {
        TheVoid, Hout, OldPlace, GoldLand, Freezor, Main
    }

    public Item(Vector2 pos)
    {
        position = pos;
        destination = Random.Range(0,3);
        animatedSprite = new FSprite("tank");
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
        boundingBox.setValues(position.x, position.y, animatedSprite.width, animatedSprite.height);
        return boundingBox;
    }
}
