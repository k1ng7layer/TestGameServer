﻿using System.Globalization;
using System.Numerics;
using TestGameServer.Game.Helpers;

namespace TestGameServer.Game.Services.Helpers;

public static class LevelImporter
{
    public static List<Triangle> LoadGeometryFromFile(string path)
    {
        var lines = File.ReadAllLines(path);

        var vertices = new List<Vector3>();
        var faces = new List<int>();
        
        foreach (var line in lines)
        {
            if (line.Length > 0 && line[0] == 'v')
            {
                var vertex = ParseVertex(line);
                vertices.Add(vertex);
            }
            
            if (line.Length > 0 && line[0] == 'f')
            {
                var faceIndices = ParseFaceIndices(line);
                faces.AddRange(faceIndices);
            }
        }
        
        var triangles = new List<Triangle>();
        
        for (int i = 0; i < faces.Count ; i+=3)
        {
            var vertex1Id = faces[i];
            var vertex2Id = faces[i + 1];
            var vertex3Id = faces[i + 2];
            
            var v1 = vertices[vertex1Id];
            var v2 = vertices[vertex2Id];
            var v3 = vertices[vertex3Id];

            var triangle = new Triangle(new Vertex(v1), new Vertex(v2), new Vertex(v3));
            triangles.Add(triangle);
        }
        
        return triangles;
    }

    private static Vector3 ParseVertex(string line)
    {
        var subLines = line
            .Replace("v ", " ")
            .TrimStart()
            // .Substring(2, line.Length - 2)
            .Split(' ');

        if (subLines.Length != 3)
            throw new Exception($"Vertices parse error");
        
        Vector3 vertex = Vector3.Zero;
        
        for (int i = 0; i < subLines.Length; i++)
        {
            var valueStr = subLines[i];
            
            if (!float.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                continue;

            if (i == 0)
                vertex.X = value;
            
            if (i == 1)
                vertex.Y = value;
            
            if (i == 2)
                vertex.Z = value;
        }

        return vertex;
    }

    private static List<int> ParseFaceIndices(string line)
    {
        var facesStr = line
            .Replace("f ", " ")
            .TrimStart()
            .Split(' ');

        if (facesStr.Length != 3)
            throw new Exception($"Vertices parse error");

        var faces = new List<int>();

        foreach (var faceStr in facesStr)
        {
            var facesVerts = faceStr.Split("/");

            foreach (var facesVert in facesVerts)
            {
                if (!int.TryParse(facesVert, out var vertIndex))
                    throw new Exception("Cant parse face verts");
                
                faces.Add(vertIndex - 1);
                break;
            }
        }

        return faces;
    }
}