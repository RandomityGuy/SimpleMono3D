using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SimpleMono3D.Graphics
{
    public static class Extensions
    {
        public static string GetWord(this string str,int n)
        {
            return str.Split(' ')[n];
        }

        public static string GetWords(this string str, int start, int end)
        {
            var words = str.Split(' ').ToList();
            return words.GetRange(start, end - start + 1).Aggregate((a, b) => a + " " + b);
        }

        public static string GetWords(this string str,int start)
        {
            var words = str.Split(' ').ToList();
            return words.GetRange(start, words.Count - start).Aggregate((a, b) => a + " " + b);
        }

        public static Vector3 FromString(string vs)
        {
            return new Vector3(float.Parse(vs.GetWord(0)), float.Parse(vs.GetWord(1)), float.Parse(vs.GetWord(2)));
        }

        public static Vector2 FromStringV2(string vs)
        {
            return new Vector2(float.Parse(vs.GetWord(0)), float.Parse(vs.GetWord(1)));
        }

        public static void GetEulerAngles(this Quaternion q,out float yaw,out float pitch,out float roll)
        {
            // roll (x-axis rotation)
            var sinr_cosp = +2.0 * (q.W * q.X + q.Y * q.Z);
            var cosr_cosp = +1.0 - 2.0 * (q.X * q.X + q.Y * q.Y);
            roll = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            var sinp = +2.0 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
                pitch = (float)Math.Sign(sinp) * (float)Math.PI / 2; // use 90 degrees if out of range
            else
                pitch = (float)Math.Asin(sinp);

            // yaw (z-axis rotation)
            var siny_cosp = +2.0 * (q.W * q.Z + q.X * q.Y);
            var cosy_cosp = +1.0 - 2.0 * (q.Y * q.Y + q.Z * q.Z);
            yaw = (float)Math.Atan2(siny_cosp, cosy_cosp);
        }

        public static void Extend(ref BoundingBox box, Vector3 vec)
        {
            if (vec.X < box.Min.X)
            {
                box.Min.X = vec.X;
            }
            if (vec.X > box.Max.X)
            {
                box.Max.X = vec.X;
            }
            if (vec.Y < box.Min.Y)
            {
                box.Min.Y = vec.Y;
            }
            if (vec.Y > box.Max.Y)
            {
                box.Max.Y = vec.Y;
            }
            if (vec.Z < box.Min.Z)
            {
                box.Min.Z = vec.Z;
            }
            if (vec.Z > box.Max.Z)
            {
                box.Max.Z = vec.Z;
            }
        }

        public static Vector3 Project(this Vector3 vec,Vector3 planenorm)
        {
            var v1 = Vector3.Cross(vec, planenorm);
            v1.Normalize();
            var v2 = Vector3.Cross(planenorm, v1);
            v2.Normalize();
            return v2;
        }
    }
}
