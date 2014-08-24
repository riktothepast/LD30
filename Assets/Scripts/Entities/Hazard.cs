using UnityEngine;
using System.Collections;

using UnityEngine;
using System.Collections;

public class Hazard
{

    Vector2 position;
    Rectangle boundingBox;


    public Hazard(Vector2 pos)
    {
        position = pos;
        boundingBox = new Rectangle();
    }

    public Vector2 getPosition()
    {
        return position;
    }

    public Rectangle getAABBBoundingBox()
    {
        boundingBox.setValues(position.x, position.y, 8, 16);
        return boundingBox;
    }
}

