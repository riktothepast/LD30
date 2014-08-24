using UnityEngine;
using System.Collections;

public class Portal  {

    Vector2 position;
    int destination;
    Rectangle boundingBox;

    public enum Destinations {
        TheVoid, Hout, OldPlace, GoldLand, Freezor, Main
    }

    public Portal(Vector2 pos, int dest)
    {
        position = pos;
        destination = dest;
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
