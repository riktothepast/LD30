using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public struct MovementWrapper
    {
        public Vector2 MovementToTry { get; private set; }
        public Vector2 FurthestAvailableLocationSoFar { get; set; }
        public int NumberOfStepsToBreakMovementInto { get; private set; }
        public bool IsDiagonalMove { get; private set; }
        public Vector2 OneStep { get; private set; }
        public Rectangle BoundingRectangle { get; set; }

        public MovementWrapper(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
            : this()
        {
            MovementToTry = destination - originalPosition;
            FurthestAvailableLocationSoFar = originalPosition;
            NumberOfStepsToBreakMovementInto = (int)(MovementToTry.SqrMagnitude() ) + 1;
            IsDiagonalMove = MovementToTry.x != 0 && MovementToTry.y != 0;
            OneStep = MovementToTry / NumberOfStepsToBreakMovementInto;
            BoundingRectangle = boundingRectangle;
        }

        public void setValues(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            MovementToTry = destination - originalPosition;
            FurthestAvailableLocationSoFar = originalPosition;
            NumberOfStepsToBreakMovementInto = (int)(MovementToTry.SqrMagnitude() ) + 1;
            IsDiagonalMove = MovementToTry.x != 0 && MovementToTry.y != 0;
            OneStep = MovementToTry / NumberOfStepsToBreakMovementInto;
            BoundingRectangle = boundingRectangle;
        }

    }
