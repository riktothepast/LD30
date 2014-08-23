using UnityEngine;
using System.Collections;

// Axis Aligned rectangle, with velocity for swept.
public class Rectangle
{
		public Rectangle (float center_x, float center_y, float width, float height)
		{
				// set x and y at top-left corner.
				x = center_x - width/2;
				y = center_y - height/2;
				w = width;
				h = height;
                cX = center_x;
                cY = center_y;
		}

        public Rectangle()
        {
            x = y = w = h = cX = cY = 0;
        }
	
		// position of top-left corner
		public float x, y;
	
		// dimensions
		public float w, h;

        // center;
        public float cX, cY;

        public static bool AABBCheck(Rectangle rect1, Rectangle rect2)
        {
            return !(rect1.x + rect1.w < rect2.x || rect1.x > rect2.x + rect2.w || rect1.y + rect1.h < rect2.y || rect1.y > rect2.y + rect2.h);
        }

        public void setValues(float center_x, float center_y, float width, float height)
        {
            // set x and y at top-left corner.
            x = center_x - width / 2;
            y = center_y - height / 2;
            w = width;
            h = height;
            cX = center_x;
            cY = center_y;
        }
}