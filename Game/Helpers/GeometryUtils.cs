using System.Numerics;

namespace TestGameServer.Game.Helpers;

public static class GeometryUtils
{
    public static List<HalfEdge> TransformFromTriangleToHalfEdge(List<Triangle> triangles)
    {
        //Make sure the triangles have the same orientation
        OrientTrianglesClockwise(triangles);

        //First create a list with all possible half-edges
        List<HalfEdge> halfEdges = new List<HalfEdge>(triangles.Count * 3);

        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle t = triangles[i];
	
            HalfEdge he1 = new HalfEdge(t.Vertex1);
            HalfEdge he2 = new HalfEdge(t.Vertex2);
            HalfEdge he3 = new HalfEdge(t.Vertex3);

            he1.nextEdge = he2;
            he2.nextEdge = he3;
            he3.nextEdge = he1;

            he1.prevEdge = he3;
            he2.prevEdge = he1;
            he3.prevEdge = he2;

            //The vertex needs to know of an edge going from it
            he1.V.halfEdge = he2;
            he2.V.halfEdge = he3;
            he3.V.halfEdge = he1;

            //The face the half-edge is connected to
            t.HalfEdge = he1;

            he1.t = t;
            he2.t = t;
            he3.t = t;

            //Add the half-edges to the list
            halfEdges.Add(he1);
            halfEdges.Add(he2);
            halfEdges.Add(he3);
        }

        //Find the half-edges going in the opposite direction
        for (int i = 0; i < halfEdges.Count; i++)
        {
            HalfEdge he = halfEdges[i];

            Vertex goingToVertex = he.V;
            Vertex goingFromVertex = he.prevEdge.V;

            for (int j = 0; j < halfEdges.Count; j++)
            {
                //Dont compare with itself
                if (i == j)
                {
                    continue;
                }

                HalfEdge heOpposite = halfEdges[j];

                //Is this edge going between the vertices in the opposite direction
                if (goingFromVertex.Position == heOpposite.V.Position && goingToVertex.Position == heOpposite.prevEdge.V.Position)
                {
                    he.oppositeEdge = heOpposite;

                    break;
                }
            }
        }


        return halfEdges;
    }
    
    public static void OrientTrianglesClockwise(List<Triangle> triangles)
    {
        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle tri = triangles[i];

            // Vector2 v1 = new Vector2(tri.Vertex1.Position.x, tri.Vertex1.Position.z);
            // Vector2 v2 = new Vector2(tri.Vertex2.Position.x, tri.Vertex2.Position.z);
            // Vector2 v3 = new Vector2(tri.Vertex3.Position.x, tri.Vertex3.Position.z);

            if (!IsTriangleOrientedClockwise(tri.Vertex1.Position, tri.Vertex2.Position, tri.Vertex3.Position))
            {
                tri.ChangeOrientation();
            }
        }
    }
    
    public static bool IsTriangleOrientedClockwise(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        bool isClockWise = true;

        float determinant = p1.X * p2.Z + p3.X * p1.Z + p2.X * p3.Z - p1.X * p3.Z - p3.X * p2.Z - p2.X * p1.Z;

        if (determinant > 0f)
        {
            isClockWise = false;
        }

        return isClockWise;
    }
    
    public static bool AreLinesIntersecting(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2, bool shouldIncludeEndPoints)
    {
        bool isIntersecting = false;

        float denominator = (l2_p2.Y - l2_p1.Y) * (l1_p2.X - l1_p1.X) - (l2_p2.X - l2_p1.X) * (l1_p2.Y - l1_p1.Y);

        //Make sure the denominator is > 0, if not the lines are parallel
        if (denominator != 0f)
        {
            float u_a = ((l2_p2.X - l2_p1.X) * (l1_p1.Y - l2_p1.Y) - (l2_p2.Y - l2_p1.Y) * (l1_p1.X - l2_p1.X)) / denominator;
            float u_b = ((l1_p2.X - l1_p1.X) * (l1_p1.Y - l2_p1.Y) - (l1_p2.Y - l1_p1.Y) * (l1_p1.X - l2_p1.X)) / denominator;

            //Are the line segments intersecting if the end points are the same
            if (shouldIncludeEndPoints)
            {
                //Is intersecting if u_a and u_b are between 0 and 1 or exactly 0 or 1
                if (u_a >= 0f && u_a <= 1f && u_b >= 0f && u_b <= 1f)
                {
                    isIntersecting = true;
                }
            }
            else
            {
                //Is intersecting if u_a and u_b are between 0 and 1
                if (u_a > 0f && u_a < 1f && u_b > 0f && u_b < 1f)
                {
                    isIntersecting = true;
                }
            }
		
        }

        return isIntersecting;
    }
    
    public static bool IsPointInTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p)
    {
        bool isWithinTriangle = false;

        //Based on Barycentric coordinates
        float denominator = ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));

        float a = ((p2.Y - p3.Y) * (p.X - p3.X) + (p3.X - p2.X) * (p.Y - p3.Y)) / denominator;
        float b = ((p3.Y - p1.Y) * (p.X - p3.X) + (p1.X - p3.X) * (p.Y - p3.Y)) / denominator;
        float c = 1 - a - b;

        //The point is within the triangle or on the border if 0 <= a <= 1 and 0 <= b <= 1 and 0 <= c <= 1
        if (a >= 0f && a <= 1f && b >= 0f && b <= 1f && c >= 0f && c <= 1f)
        {
            isWithinTriangle = true;
        }

        //The point is within the triangle
        if (a > 0f && a < 1f && b > 0f && b < 1f && c > 0f && c < 1f)
        {
            isWithinTriangle = true;
        }

        return isWithinTriangle;
    }
    
    public static Vector2 FindNearestPointOnLine(Vector2 origin, Vector2 end, Vector2 point)
    {
        //Get heading
        Vector2 heading = (end - origin);
        float magnitudeMax = heading.Length();
        heading = Vector2.Normalize(heading);

        //Do projection from the point but clamp it
        Vector2 lhs = point - origin;
        float dotP = Vector2.Dot(lhs, heading);
        dotP = Math.Clamp(dotP, 0f, magnitudeMax);
        return origin + heading * dotP;
    }
    
    public static Vector2 CalculateCenterOfTriangle(Triangle triangle)
    {
        var x = (triangle.Vertex1.Position.X + triangle.Vertex2.Position.X + triangle.Vertex3.Position.X) / 3f;
        var y = (triangle.Vertex1.Position.Y + triangle.Vertex2.Position.Y + triangle.Vertex3.Position.Y) / 3f;

        return new Vector2(x, y);
    }
    
    public static bool IsTriangleHasVertex(Vertex a, Triangle triangle)
    {
        if (a.Position == triangle.Vertex1.Position || a.Position == triangle.Vertex2.Position ||
            a.Position == triangle.Vertex3.Position)
            return true;

        return false;
    }
}