//
// Entity.cs
//
// Author:
//       Rik <>
//
// Copyright (c) 2014 Rik
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : FContainer
{
    Vector2 oldPosition;
    public Vector2 velocity;
    Vector2 lastMovement;
    public float impactRotation;
    Vector2 position;
    Vector2 maxSpeed;
    Vector2 size;
    int facing;
    float gravity;
    float movementSpeed = 80f;
    float runningSpeed = 120f;
    float normalSpeed = 80f;
    float floorFriction;
    float airFriction;
    float jumpImpulse;
    //the smaller this value, the bigger the rectBounds will check
    public int detectionAccuracy = 6;
    public Rectangle boundingBox, impactBoundingBox, offsetOnePixel;
    bool active, remove;
    bool isJumping, wasJumping;
    FSprite sprite;
    int healthPoints, totalPoints;
    int scorePoints;
    bool needsReset;
    bool isPaused;
    public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
    public Vector2 Position { get { return position; } set { position = value; } }
    public Vector2 OldPosition { get { return oldPosition; } set { oldPosition = value; } }
    public Vector2 MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
    public Vector2 Size { get { return size; } set { size = value; } }
    public Vector2 LastMovement { get { return lastMovement; } set { lastMovement = value; } }
    public float Gravity { get { return gravity; } set { gravity = value; } }
    public float JumpImpulse { get { return jumpImpulse; } set { jumpImpulse = value; } }
    public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
    public float NormalSpeed { get { return normalSpeed; } set { normalSpeed = value; } }
    public float RunningSpeed { get { return runningSpeed; } set { runningSpeed = value; } }
    public float AirFriction { get { return airFriction; } set { airFriction = value; } }
    public float FloorFriction { get { return floorFriction; } set { floorFriction = value; } }
    public int DetectionAccuracy { get { return detectionAccuracy; } set { detectionAccuracy = value; } }
    public int Facing { get { return facing; } set { facing = value; } }
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    public bool WasJumping { get { return wasJumping; } set { wasJumping = value; } }
    public bool isActive { get { return active; } set { active = value; } }
    public bool Remove { get { return remove; } set { remove = value; } }
    public bool canJump { get; set; }
    public bool useGravity { get; set; }
    public bool noClip { get; set; }
    public FSprite Texture { get { return sprite; } set { sprite = value; } }
    public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
    public int TotalPoints { get { return totalPoints; } set { totalPoints = value; } }

    public int ScorePoints { get { return scorePoints; } set { scorePoints = value; } }
    public bool NeedsReset { get { return needsReset; } set { needsReset = value; } }
    public bool IsPaused { get { return isPaused; } set { isPaused = value; } }
    public bool DeadLogic { get { return deadLogic; } set { deadLogic = value; } }

    public bool running;

    float currentTime;

    bool deadLogic;

    public float timeToWait;

    public Entity()
    {

    }

    public enum EntityState
    {
        onJump,
        onDoubleJump,
        onGround,
        onAir
    }

    public virtual void Init() { }

    public virtual void CheckAndUpdateMovement()
    {
       
    }

    public void MoveAsFarAsPossible()
    {
        oldPosition = position;
        UpdatePosition();
        position = TileMap.CurrentMap.WhereCanIGetTo(oldPosition, position, getAABBBoundingBox());
    }

    public void UpdatePosition()
    {
        position += velocity;
    }

    public virtual void applyGravity()
    {
        if (!useGravity) 
            return;

        velocity -= Vector2.up * gravity * Time.deltaTime;
    }

    public virtual void applyFriction()
    {
        // applies friction based on the last type of tile detected?. Of if the entity is not grounded.
        if (Ground())
        {
            velocity.x -= velocity.x * FloorFriction;
        }
        else
        {
            velocity.x -= velocity.x * AirFriction;
        }
    }


    public virtual void Update()
    {
        if (!deadLogic)
        {
            CheckAndUpdateMovement();
            applyGravity();
            applyFriction();
            MoveAsFarAsPossible();
            StopMovingIfBlocked();
            SetPosition(position);
        }
        else {
            rotation += 10f * impactRotation;
            useGravity = true;
            applyGravity();
            UpdatePosition();
            SetPosition(position);
            if (TileMap.CurrentMap.IsOutOfBounds(Position))
                Destroy();
        }

        currentTime = timeToWait - Time.time;

        if (currentTime > 0)
            return;

            RemoveHitEffect();
    }

    public virtual void WorldWrap()
    {
        var wrap = TileMap.CurrentMap.WordWrap(getAABBBoundingBox());
        if (wrap != Vector2.zero)
        {
            if (wrap.x != 0)
                Position = new Vector2(wrap.x, Position.y);
            if (wrap.y != 0)
                Position = new Vector2(Position.x, wrap.y);
        }
    }

    public virtual void StopMovingIfBlocked()
    {
        lastMovement = position - oldPosition;
        if (lastMovement.x == 0) { velocity.x = 0; }
        if (lastMovement.y == 0) { velocity.y = 0; }
    }

    public void Destroy()
    {
        Remove = true;
        RemoveFromContainer();
    }

    public Rectangle getAABBBoundingBox()
    {
        boundingBox.setValues(position.x, position.y, size.x, size.y);
        return boundingBox;
    }
    // returns entitys actual position in tiles.
    public Vector2 PositionOnTileMap()
    {
        return new Vector2(TileMap.CurrentMap.pixelsToTiles(Position.x), TileMap.CurrentMap.pixelsToTiles(Position.y));
    }   

    //https://docs.unity3d.com/Documentation/ScriptReference/Rectangle.html
    public bool Ground()
    {
        offsetOnePixel = getAABBBoundingBox();
        offsetOnePixel.y -= offsetOnePixel.h/detectionAccuracy;
        offsetOnePixel.w *= 0.8f;
        bool val = !TileMap.CurrentMap.HasRoomForRectangle(offsetOnePixel);
        return val;
    }
    public bool Ceiling()
    {
        offsetOnePixel = getAABBBoundingBox();
        offsetOnePixel.y += offsetOnePixel.h / detectionAccuracy;
        offsetOnePixel.w *= 0.8f;
        return !TileMap.CurrentMap.HasRoomForRectangle(offsetOnePixel);
    }

    public bool LeftWall()
    {
        offsetOnePixel = getAABBBoundingBox();
        offsetOnePixel.x -= offsetOnePixel.w / detectionAccuracy;
        offsetOnePixel.y += offsetOnePixel.h / detectionAccuracy;
        offsetOnePixel.h *= 0.8f;
        return !TileMap.CurrentMap.HasRoomForRectangle(offsetOnePixel);

    }

    public bool RightWall()
    {
        offsetOnePixel = getAABBBoundingBox();
        offsetOnePixel.x +=offsetOnePixel.w / detectionAccuracy;
        offsetOnePixel.y += offsetOnePixel.h / detectionAccuracy;

        offsetOnePixel.h *= 0.8f;
        return !TileMap.CurrentMap.HasRoomForRectangle(offsetOnePixel);

    }

    public void SubstractLife(int value) {
        
    }

    public virtual void DoHitEffect() {

    }

    public virtual void RemoveHitEffect() {
 
    }

    public void WaitSeconds(float ammount)
    {
        timeToWait = Time.time + ammount;
    }


    // this code is by mylesmar10
    public void drawHitBox(Rectangle Rectangle, Color color)
    {
        Vector3 tL = new Vector3(0f, 0f, 0f);
        Vector3 tR = new Vector3(0f, 0f, 0f);
        Vector3 bL = new Vector3(0f, 0f, 0f);
        Vector3 bR = new Vector3(0f, 0f, 0f);
        tL.x = Rectangle.x;
        tL.y = Rectangle.y;
        tR.x = Rectangle.x + Rectangle.w;
        tR.y = Rectangle.y;
        bL.x = Rectangle.x;
        bL.y = Rectangle.y + Rectangle.h;
        bR.x = Rectangle.x + Rectangle.w;
        bR.y = Rectangle.y + Rectangle.h;

        Debug.DrawLine(tL, tR, color);
        Debug.DrawLine(tR, bR, color);
        Debug.DrawLine(bR, bL, color);
        Debug.DrawLine(bL, tL, color);
    }


}