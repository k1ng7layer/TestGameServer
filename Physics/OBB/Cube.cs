using System.Numerics;
using TestGameServer.Game.Helpers;

namespace OBB
{
    [Serializable]
    public class Cube
    {
        public int Id;
        public Vector3 Position;
        
        public Quaternion Rotation;
        public Vector3 Velocity;
        
        public Vector3 Size { get; }

        public Cube(Vector3 size)
        {
            Size = size;
        }

        public Vector3 Forward => Vector3.Transform(Vector3.UnitZ, Rotation);
        public Vector3 Up => Vector3.Transform(Vector3.UnitY, Rotation);
        public Vector3 Right => Vector3.Transform(Vector3.UnitX, Rotation);

        public Vector3[] Vertices
        {
            get
            {
                return new[]
                {
                    Vector3.Zero + new Vector3(-Size.X, -Size.Y, -Size.Z) * 0.5f,
                    Vector3.Zero + new Vector3(Size.X, -Size.Y, -Size.Z) * 0.5f,
                    Vector3.Zero + new Vector3(Size.X, -Size.Y, Size.Z) * 0.5f,
                    Vector3.Zero + new Vector3(-Size.X, -Size.Y, Size.Z) * 0.5f,
                    Vector3.Zero + new Vector3(-Size.X, Size.Y, -Size.Z) * 0.5f,
                    Vector3.Zero + new Vector3(Size.X, Size.Y, -Size.Z) * 0.5f,
                    Vector3.Zero + new Vector3(Size.X, Size.Y, Size.Z) * 0.5f,
                    Vector3.Zero + new Vector3(-Size.X, Size.Y, Size.Z) * 0.5f,
                };
            }
        }

        public Vector3 TransformPointFromLocalToWorldSpace(Vector3 point)
        {
            var rotEuler = Rotation.Eulers() * ((float)Math.PI / 180f);
            
            var translationMatrix = new Matrix4x4(
                1f, 0f, 0f, Position.X, 
                0f, 1f, 0f, Position.Y, 
                0f, 0f, 1f, Position.Z, 
                0f, 0f, 0f, 1f);
            
            var scaleMatrix = new Matrix4x4(
                1f, 0f, 0f, 0f,
                0f, 1f, 0f, 0f,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f);
            
            var rotationMatrixX = new Matrix4x4(
                1f, 0f, 0f, 0f,
                0f, MathF.Cos(rotEuler.X), -MathF.Sin(rotEuler.X), 0f, 
                0f, MathF.Sin(rotEuler.X),MathF.Cos(rotEuler.X), 0f,
                0f, 0f, 0f, 1f);
            
            var rotationMatrixY = new Matrix4x4(
                MathF.Cos(rotEuler.Y), 0f, MathF.Sin(rotEuler.Y), 0f, 
                0f, 1f, 0f, 0f, 
                -MathF.Sin(rotEuler.Y), 0f, MathF.Cos(rotEuler.Y), 0f, 
                0f, 0f, 0f, 1f);
            
            var rotationMatrixZ = new Matrix4x4(
                MathF.Cos(rotEuler.Z), -MathF.Sin(rotEuler.Z), 0f, 0f, 
                MathF.Sin(rotEuler.Z), MathF.Cos(rotEuler.Z), 0f, 0f, 
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f);
            
            var transformation = translationMatrix * rotationMatrixY * rotationMatrixX * rotationMatrixZ * scaleMatrix;

            var result = Vector3.Transform(point, transformation);
            
            return result;
        }

        private float[,] GetTranslationMatrix()
        {
            return new float[,]
            {
                {1f, 0f, 0f, Position.X},
                {0f, 1f, 0f, Position.Y},
                {0f, 0f, 1f, Position.Z},
                {0f, 0f, 0f, 1f},
            };
        }


        private Vector4 MultiplyMatrixByVector(float[,] matrix4x4, float[] vector)
        {
            var rows = matrix4x4.GetUpperBound(0) + 1;
            var columns = matrix4x4.Length / rows;
            float[] result = new float[4];
            for (int i = 0; i < rows; i++)
            {
                result[i] = 0;
                
                for (int j = 0; j < columns; j++)
                {
                    result[i] += matrix4x4[i, j] * vector[j];
                }
            }

            return new Vector4(result[0], result[1], result[2], result[3]);
        }
        
        public float[,] MultiplyMatrix(float[,] A, float[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);

            if (cA != rB)
            {
                Console.WriteLine("Matrixes can't be multiplied!!");
            }
            else
            {
                float temp = 0;
                float[,] kHasil = new float[rA, cB];

                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i, k] * B[k, j];
                        }
                        kHasil[i, j] = temp;
                    }
                }

                return kHasil;
            }

            return default;
        }
        
        public Vector3 MultiplyPoint(float[, ] matrix, Vector3 point)
        {
            Vector3 res;
            float w;
            res.X =  matrix[0, 0] * point.X +  matrix[1, 0] * point.Y +  matrix[2, 0] * point.Z + matrix[3, 0];
            res.Y = matrix[0, 1] * point.X +  matrix[1, 1] * point.Y +  matrix[2, 1] * point.Z +  matrix[3, 1];
            res.Z =  matrix[0, 2] * point.X +  matrix[1, 2] * point.Y +  matrix[2, 2] * point.Z +  matrix[3, 2];
            w =  matrix[0, 3] * point.X +  matrix[1, 3] * point.Y +  matrix[2, 3] * point.Z +  matrix[3, 3];

            w = 1f / w;
            res.X *= w;
            res.Y *= w;
            res.Z *= w;
            return res;
        }
        
    }
}