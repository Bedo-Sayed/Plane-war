using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;

namespace Project1
{
    public class Geometry
    {
        static float eps = 0.00001f;
        public static int Cross(Vector2 a, Vector2 b, Vector2 p)
        {
            Vector2 pA = a - p;
            Vector2 pB = b - p;
            float cross = pA.X * pB.Y - pA.Y * pB.X;

            if (cross > 0f)
                return 1;
            else if (cross < 0f)
                return -1;

            return 0;
        }

        public static bool IsInsidePoly(Vector2 p, List<Vector2> poly)
        {
            bool goLeft = false, goRight = false;
            for (int i = 0; i < poly.Count; i++)
            {
                Vector2 a = poly[i], b = poly[(i + 1) % poly.Count];

                float dsAP = Vector2.Distance(a, p), dsPB = Vector2.Distance(p, b);
                float dsAB = Vector2.Distance(a, b);
                float left = dsAP + dsPB, right = dsAB;

                if (Math.Abs(left - right) < eps)
                {
                    return true;
                }

                if (Cross(a, b, p) == 1)
                    goLeft = true;
                else if (Cross(a, b, p) == -1)
                    goRight = true;
                else
                    return false;
            }

            if (goLeft && goRight) return false;
            return true;
        }

        public static bool Intersect(List<Vector2> poly , List<Vector2> poly2)
        {
            for(int i=0; i<poly.Count; i++)
            {
                Vector2 p = poly[i];

                if (IsInsidePoly(p, poly2))
                    return true;

                p = poly2[i];
                if (IsInsidePoly(p, poly))
                    return true;
            }

            return false;
        }

        public static bool Intersect2(List<Vector2> poly, List<Vector2> poly2)
        {
            for(int i = 0; i < poly.Count; i++)
            {
                for(int j = i+1; j < poly.Count; j++)
                {
                    if (IsInsidePoly(poly[i], poly2) && IsInsidePoly(poly[j], poly2))
                        return true;
                    if (IsInsidePoly(poly2[i], poly) && IsInsidePoly(poly2[j], poly))
                        return true;
                }
            }
            return false;
        }
        public static bool IsValid(Vector2 point)
        {
            if (point.X < Globals.topLeft.X || point.X > Globals.botRight.X
                || point.Y < Globals.topLeft.Y || point.Y > Globals.botRight.Y)
                return false;
            return true;
        }
    }
}
